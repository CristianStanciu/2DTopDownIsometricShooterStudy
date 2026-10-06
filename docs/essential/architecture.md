# Code Architecture

[Index](index.md) · [Getting Started](getting-started.md) · [Player System](player.md)

## Assembly & compilation

- **No `.asmdef` files** — everything in `Assets/Scripts/` compiles into the default
  `Assembly-CSharp` assembly (also referenced by the generated `Assembly-CSharp.csproj` /
  `my-game.sln` / `my-game.slnx`). There are **no tests** despite `com.unity.test-framework`
  being installed.

## Script folder map (`Assets/Scripts/`)

| Folder | Files | Responsibility |
|---|---|---|
| `Animations/` | `AnimationHandler` | Base class for legacy `Animation` clip playing (`PlayAnimation`, speed control). |
| `Audio/` | 9 classes | Dual audio architecture — see [Audio System](audio.md). |
| `Camera/` | `CameraBehaviour`, `CameraShake`, `CameraSizeHandler` | Follow/offset camera, static shake API, orthographic-size helper (**disabled** in scene). |
| `Crosshair/` | `Crosshair`, `CrosshairMouse`, `CrosshairJoystick`, `CrosshairHandler` | Crosshair base + per-device implementations + switching logic. |
| `Debug/` | `GUIHandler`, `GizmosHandler` | OnGUI overlay (controls + aim vectors) and gizmo toggles. |
| `Environment/` | `EnvironmentObject`, `EnvironmentObjectTrigger` | Wall façade swapping by player position — see [Environment, FX & Pause](environment-and-fx.md). |
| `FX/AfterImage/` | 5 classes | Dash after-image pool system. |
| `Global Managers and Controllers/` | `PauseController` | Pause/resume via `Time.timeScale`, `AudioListener.pause`, `UnityEvent`s. |
| `Inputs/` | `TadaInput` | Static input manager wrapping legacy `UnityEngine.Input` — see [Input System](input.md). |
| `Physics/` | `Physics2DHandler` | Rigidbody2D base class (`SetVelocity`, `AddForce`, `CanMove`). |
| `Player/` | 9 classes | Player composition — see [Player System](player.md). |
| `Utilities/` | `Singleton<T>`, `ArraysHandler`, `LookAt2Dv2` + `Handler` + `Updater` | Singleton pattern, circular array indexing, 2D aim-rotation system. |
| `Weapons/` | `Weapon`, `WeaponHandler`, `Projectile`, `Weapon_ShootProjectileCanCharge`, `WeaponAnim_ShootProjectileCanCharge` | Weapon framework — see [Weapons & Projectiles](weapons-and-projectiles.md). |
| `_NOTES/` | `JustNotes` | Author's scratchpad MonoBehaviour (harmless, can be ignored/removed). |

## Inheritance map

```
MonoBehaviour
├── Physics2DHandler                 (Assets/Scripts/Physics)
│   └── PlayerPhysics
├── AnimationHandler                 (Assets/Scripts/Animations)
│   ├── PlayerAnimations
│   └── WeaponAnim_ShootProjectileCanCharge
├── Weapon                           (Assets/Scripts/Weapons)
│   └── Weapon_ShootProjectileCanCharge      ← both scene weapons use this
├── Crosshair                        (Assets/Scripts/Crosshair)
│   ├── CrosshairMouse
│   └── CrosshairJoystick
├── SoundEmitter                     (Assets/Scripts/Audio)  [RequireComponent(AudioSource)]
│   ├── SoundFXEmitter
│   ├── UnstoppableSoundEmitter
│   ├── MusicEmitter
│   └── SoundHandlerLocal
└── Singleton<T>  (abstract, generic) → SoundManager
```

Non-inheritance composition to know about:

- `PlayerController` **does not contain logic** — it is a pure input dispatcher that
  `TryGetComponent`s `PlayerPhysics`, `PlayerSkills`, `PlayerAnimations`, `WeaponHandler` and
  calls into them (see [Player System](player.md)).
- `LookAt2Dv2` instances (4 in each scene) are managed by one `LookAt2Dv2Handler` and ticked by a
  single `LookAt2Dv2Updater` — a manual update pipeline instead of per-component `Update()`.
- `AfterImageGenerator` (17 in each scene — one per skinned part) registers groups into the scene's
  single `AfterImageHandler`.

## Key patterns

1. **Singleton<T>** (from UnityCommunity, MIT): lazy `Instance` with `FindObjectOfType<T>()`
   fallback, `DontDestroyOnLoad` on first `Awake`, duplicates destroyed. Only user:
   `SoundManager`.
2. **Static facade**: `TadaInput` (input), `CameraShake` (shake), `CrosshairMouse.AimDirection`
   / `CrosshairJoystick.AimDirection` (aim vectors), `PauseController.isGamePaused`.
   These create hidden scene-level coupling — read before refactoring.
3. **Inspector-serialized arrays over dynamic lookup**: `WeaponHandler.weapons[]`,
   `SoundHandlerLocal.sounds[]`, `EnvironmentObjectTrigger.objects*[]`, `MusicHandler.musicClips[]`.
4. **Base-class virtual hooks**: `Weapon.Awake/Update/OnCanUse/PrimaryAction/SecondaryAction`,
   `SoundEmitter.Awake`, `Physics2DHandler.Awake` — derived classes call `base.*()` first.
5. **Notes field convention**: virtually every MonoBehaviour has
   `[TextArea] public string notes = "..."` describing itself in the Inspector.
6. **`FindObjectOfType<T>()` at startup in 9 scripts** (SoundManager ×3, CrosshairHandler ×2,
   CrosshairMouse, CrosshairJoystick ×2, AfterImageGenerator, AfterImageGroup,
   PlayerBodyPartsHandler, PlayerSkills, Singleton) — deprecated in Unity 6, listed as tech debt
   in [Known Issues](known-issues-and-roadmap.md).

## Coding conventions

Observed consistently across the codebase (follow these for new code):

- Header banner comment in every file (project title + author social links).
- `#region` blocks: `PROPERTIES`, `UNITY CALLBACKS`, `METHODS` (and subregions).
- Serialized private fields: `[SerializeField] private float _Name = value;` with a public
  property `public float Name { get; private set; }` accessor above it.
- Public Inspector fields used as wiring (`public Weapon[] weapons;`, `public Transform target;`).
- Unity callbacks in order: `Awake` → `OnEnable` → `Start` → `FixedUpdate`/`Update` → …
- `TryGetComponent(out field)` for same-GameObject dependencies; `FindObjectOfType` for
  scene-level ones (legacy habit — prefer `FindAnyObjectByType` for new code on Unity 6).
- Defensive `CheckIfMissing...` methods log `Debug.LogWarning`/`LogError` (sometimes
  `Debug.Break()`) instead of throwing.
- Comments explain *why*, authored as learning notes; `debug` bools gate verbose logging.
- `.editorconfig` at repo root defines indentation/charset rules for IDEs.

## Data flow of one frame (summary)

```
TadaInput.Update()            — polls Input, stores key/axis state (static)
PlayerController.Update()     — if !paused: dispatch dash/animations/weapon actions
PlayerPhysics.FixedUpdate()   — if CanMove: SetVelocity(MoveAxisRawInput × 6)
Weapon.Update()               — use-rate cooldown timer → OnCanUse()
Weapon_ShootProjectileCanCharge.Update() — charge state machine (if receiving input)
CrosshairHandler.Update()     — picks active crosshair → updates static AimDirection(s)
CameraBehaviour.FixedUpdate() — lerp follow + crosshair offset (CameraShake offsets after)
LookAt2Dv2Updater             — rotates body/arm parts toward aim
SoundManager / handlers       — play SFX/music on events
```

## Related pages

- [Player System](player.md) · [Weapons & Projectiles](weapons-and-projectiles.md) ·
  [Input System](input.md) · [Audio System](audio.md) · [Known Issues](known-issues-and-roadmap.md)

