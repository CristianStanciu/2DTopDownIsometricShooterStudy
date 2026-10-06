# Player System

[Index](index.md) · [Architecture](architecture.md) · [Weapons & Projectiles](weapons-and-projectiles.md)

The player is a composition of **9 scripts** in `Assets/Scripts/Player/` plus support classes, all
living under the `Player` root GameObject (present in both scenes). `PlayerController` is only an
input dispatcher; every actual behaviour is delegated.

## Class responsibilities

| Class | Base | Role |
|---|---|---|
| `PlayerController` | `MonoBehaviour` | Reads `TadaInput` each `Update()` (skipped while `PauseController.isGamePaused`) and calls into the other classes. `Awake` grabs sibling components via `TryGetComponent`; `CheckIfMissingClasses()` warns if any is unassigned. |
| `PlayerPhysics` | `Physics2DHandler` | Movement: `FixedUpdate` → `SetVelocity(TadaInput.MoveAxisRawInput, MoveSpeed)` when `CanMove`. `MoveSpeed = 6`. Handles `OnTriggerEnter2D` with tag `EnvironmentTrigger` → `EnvironmentObjectTrigger.SetActiveObjects()` (wall façade switching). `OnTriggerExit2D` is an empty stub. |
| `PlayerSkills` | `MonoBehaviour` | The **dash**. See below. |
| `PlayerAnimations` | `AnimationHandler` | Legacy `Animation` clips (Idle / WalkForward / WalkBackwards). `PlayMoveAnimationsByMoveInputAndLookDirection(moveInput)` selects the clip; `SetAnimationSpeed` ties clip speed to `PlayerPhysics.Velocity.magnitude`. |
| `PlayerMaterials` | `MonoBehaviour` | Player body material highlight (used by dash). |
| `PlayerBodyPartsHandler` | `MonoBehaviour` | Coordinates body aim states; exposes static `isRightDirection` consumed by `Projectile.Fire()` for travel direction; resolves `PlayerShoulderSecondary` via `FindObjectOfType`; respects pause. |
| `PlayerShoulderMain` | `MonoBehaviour` | Main shoulder pivot — follow target for the joystick crosshair (`CrosshairJoystick.pointToFollow`). |
| `PlayerShoulderSecondary` | `MonoBehaviour` | Secondary shoulder pivot used by body-parts logic. |
| `PlayerHandTargetToLookAt` | `MonoBehaviour` | Marker transform that aim rotations (`LookAt2Dv2`) point the arms/hands at. |

## The dash (`PlayerSkills`)

`Dash()` starts coroutine `CO_Dash()`:

1. `canDash = false`; enable `TrailRenderer[] dashTrails` (EyeTrail, FootTrail).
2. `_AfterImageHandler.SetActiveAfterImages()` (after-image burst).
3. `_PlayerPhysics.CanMove = false` — otherwise `FixedUpdate` would overwrite the impulse.
4. `ActualDash()`: play dash SFX (`SoundHandlerGlobal.PlaySound()`), flash body highlight
   (`PlayerMaterials.SetActiveHighlightBody(DASH_DURATION, intensity: 1.25)`), zero velocity,
   then `AddForce(TadaInput.MoveAxisRawInput.normalized, dashForce: 5, ForceMode2D.Impulse)`.
5. `yield return new WaitForSeconds(DASH_DURATION = 0.2f)`; disable trails; `CanMove = true`;
   `canDash = true`.

Trigger condition (in `PlayerController`): `TadaInput.GetKeyDown(Dash) && Velocity.sqrMagnitude > 0`
— dashing requires current movement. `canDash` is hard-wired `true` in `Awake` with a comment that
it should eventually be granted by progression.

## Hierarchy (as serialized in both scenes)

The `Player` root contains the rig (names from scene files):

```
Player
├── Hips → UpperBody → Neck → Head ("Head ---- LookAt MousePos")
│                        └── (neck/eye parts: Neck_Decal, Neck_BG, EnergyLines_Body)
├── Shoulder_L / Shoulder_R → UpperArm_L/R → ForeArm_* → Hand_* → weapons
│     pivots: pivot_Shoulder_Main, pivot_Shoulder_Secondary, pivot_ForeArm_L
├── Thigh_L/R, Calf_L/R, Quad/Quad (1)          (legs)
├── Shadow (blob)                               (shadow quad)
├── Dash trails: EyeTrail, FootTrail            (TrailRenderers for dash)
├── Projectile_SpawnPoint, Muzzle               (weapon spawn transforms)
└── LaserRifle_Auto-Charge, LaserBeam_Rifle_Body (weapon objects, children of hands)
```

- Weapon placement: `weapons[0]` = **LaserRifle_Auto-Charge** (GameObject `LaserRifle_Auto-Charge`),
  `weapons[1]` = **LaserBeam_Rifle_Body** (GameObject `LaserBeam_Rifle_Body`); both are children of
  the hand rig and both host `Weapon_ShootProjectileCanCharge` + `WeaponAnim_ShootProjectileCanCharge`.

## Movement details

- Input → velocity: `TadaInput.MoveAxisRawInput` (raw axis, diagonals clamped to magnitude 1) ×
  `MoveSpeed 6`, applied every `FixedUpdate` through `Physics2DHandler.SetVelocity`.
- Dash **overrides** movement: velocity is zeroed first so the impulse is consistent, and
  `CanMove = false` prevents `FixedUpdate` from re-setting velocity during the 0.2s window.
- Rigidbody2D/physics configuration (gravity, iterations) is in
  [Project Settings](project-settings.md#physics-2d-physics2dsettingsasset).

## Aim rig (how the body points at the crosshair)

1. `CrosshairHandler` updates either `CrosshairMouse.AimDirection` or `CrosshairJoystick.AimDirection`
   (static vectors) — see [Camera & Crosshair](camera-and-crosshair.md).
2. `LookAt2Dv2` components (4 instances) rotate their transforms toward the aim; they are *not*
   self-updating — a single `LookAt2Dv2Updater` ticks them and a `LookAt2Dv2Handler` owns the list.
3. `PlayerHandTargetToLookAt` provides the target the arms converge on; `PlayerBodyPartsHandler`
   flips `isRightDirection`, which decides weapon/shoulder facing and projectile travel direction
   (`Projectile.Fire()` moves along `±Vector3.right` based on it).

## Pause interaction

- `PlayerController.Update()` returns early when `PauseController.isGamePaused`.
- `PlayerBodyPartsHandler` also checks pause.
- `PlayerPhysics.FixedUpdate` relies on `Time.timeScale = 0` freezing `FixedUpdate`.

## Known gaps (player)

- Dash availability is hard-wired `true` (`canDash = true` in `Awake`) with a comment saying it
  "should be handled by other behaviour that grants the ability after completing a task".
- Body/aim logic depends on `FindFirstObjectByType<PlayerShoulderSecondary>()` at startup.

*(Resolved in P0.E3: `SetActiveTrails` element null-check (T2), `OnTriggerExit2D` façade restore
(T7), weapon slots 1/2 routed through `TadaInput` (T4).)*

Full defect list: [Known Issues & Roadmap Foundation](known-issues-and-roadmap.md).

## Related pages

- [Weapons & Projectiles](weapons-and-projectiles.md) — what `PlayerController` drives.
- [Input System](input.md) — where the dispatched inputs come from.
- [Environment, FX & Pause](environment-and-fx.md) — after-images and wall trigger callback.

