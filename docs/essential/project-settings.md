# Project Settings

[Index](index.md) · [Scene Inventory](scenes.md) · [Rendering](rendering.md)

Reference for every `ProjectSettings/*.asset` relevant to gameplay/tech.

## Project identity

| File | Value |
|---|---|
| `ProjectVersion.txt` | Unity **6000.3.20f1** (revision `c9ba695d4f07`) — upgraded from 2019.4.0f1 in commit `c9d5f0e` |
| `ProjectSettings.asset` | product name `2DTopDownShooterStudy`, company `TadadosiGames`, `bundleVersion 0.1` (README says v0.2), 2D template |
| Color space | **Gamma** (2D template default) |

## Tags and layers (`TagManager.asset`)

- **Tags**: `Wall` (projectile collision — `Projectile.OnTriggerEnter2D`),
  `EnvironmentTrigger` (wall occlusion — `PlayerPhysics.OnTriggerEnter2D`).
- **Layers**: stock Unity layers plus
  - index **8 = `Player`**
  - index **9 = `Projectile`**
- **Sorting layers**: only `Default`.
- **Rendering Layers**: `Default` + `Light Layer 1…7` (auto-added by the Unity 6 TagManager
  serializedVersion 3 migration — unused).

## Input axes (`InputManager.asset` — legacy Input Manager)

Custom axes added by the project (on top of Unity defaults `Horizontal`, `Vertical`, `Mouse X`,
`Mouse Y`, `Mouse ScrollWheel`, `Fire1/2/3`, `Jump`, `Submit`, `Cancel`, debug axes):

| Axis | Purpose | Consumed by |
|---|---|---|
| `HorizontalAim` | gamepad right-stick X | `TadaInput._AimAxisSmooth/RawInput` |
| `VerticalAim` | gamepad right-stick Y | `TadaInput._AimAxisSmooth/RawInput` |
| `Xbox360LeftTrigger` | secondary action (RMB alt) | `TadaInput` → `ThisKey.Xbox360LeftTrigger` |
| `Xbox360RightTrigger` | primary action (LMB alt) | `TadaInput` → `ThisKey.Xbox360RightTrigger` |

Author's note in `TadaInput`: *"To support all controllers, use these names and add new inputs to
Unity's Input Manager. At the moment it only supports Xbox controllers."*
There is **no** `com.unity.inputsystem` package — migrating is roadmap material
([Known Issues](known-issues-and-roadmap.md)).

## Physics 2D (`Physics2DSettings.asset`)

| Setting | Value |
|---|---|
| Gravity | `(0, -9.81)` |
| Velocity iterations | 8 |
| Position iterations | 3 |
| Velocity threshold | 1 |
| Default contact offset | 0.01 |
| Auto Sync Transforms | **off** |

Physics materials: `Assets/Physics Materials/NoFriction.physicsMaterial2D`.
Layer collision matrix defaults (Player/Projectile layers use default matrix).
The Unity 6 upgrade touched this file's code usage: `Physics2DHandler` was updated from
`Rigidbody2D.velocity` → **`linearVelocity`** (API rename in Unity 6).

## Quality (`QualitySettings.asset`)

Six levels: `Very Low`, `Low`, `Medium`, `High`, `Very High`, `Ultra`.
`m_CurrentQuality: 3` → **High** active in editor.
Pipeline binding: URP asset `Forward_Low_NoLights` (see [Rendering](rendering.md#pipeline-assets)).

## Audio (`AudioManager.asset`)

| Setting | Value |
|---|---|
| Master volume | 1 |
| Speaker mode | 2 (Stereo) |
| DSP buffer size | 1024 (requested: 1024) |
| Virtual / real voices | 512 / 32 |
| Virtualize effects | on |
| Spatializer / ambisonic | none assigned |

No AudioMixer assets exist — all mixing is per-`AudioSource` volume
([Audio System](audio.md)).

## Graphics (`GraphicsSettings.asset`)

- `m_CustomRenderPipeline` → URP asset guid `fd70f05e59dd5794ea7b0fc50fa3c908`
  (`Assets/Rendering/Forward_Low_NoLights.asset`).
- Built-in shader references (sprite/default materials) stock; default sprite material set.
- `UniversalRenderPipelineGlobalSettings` entry (guid `7e07a41e98a62ba4d8cc29b2211029ab`) —
  added by the Unity 6 upgrade.

## Build (`EditorBuildSettings.asset`)

```
Scene 0: enabled=1  Assets/Scenes/Level_00.unity  (guid 2cda990e2423bbf4892e6590ba056729)
Scene 1: enabled=0  path empty (leftover entry — safe to delete)
```

`SingleWall_Test` is not in the build.

## Other notable settings

- **`AudioManager`/`TimeManager`/`DynamicsManager`**: defaults except as listed above.
- **`.editorconfig`** (repo root): C# formatting rules for IDEs.
- **`.vsconfig`**: Visual Studio workloads (added by upgrade).
- **`UserSettings/` is git-ignored** since P0.E1.T1 (previously committed: editor layouts for
  2021/2022/2023/6000 + `Search.index` — removed from the index, kept on disk).
- **Tag/layer usage**: projectiles ignore everything except `Wall` triggers; verify collision matrix
  when adding enemies (roadmap item).

## Related pages

- [Input System](input.md) — axis consumers.
- [Rendering](rendering.md) — pipeline asset details.
- [Known Issues & Roadmap Foundation](known-issues-and-roadmap.md) — upgrade deltas.
