# Camera & Crosshair

[Index](index.md) · [Input System](input.md) · [Player System](player.md)

## Scene objects

Both scenes contain the same rig: a root **`CameraPivot`** (owns `CameraBehaviour`, `CameraShake`,
and the disabled `CameraSizeHandler`), a root **`CrosshairHandler`**, plus two crosshair roots
**`MouseCrosshair`** (with `CrosshairMouse`) and **`JoystickCrosshair`** (with `CrosshairJoystick`).

Main Camera (child of `CameraPivot`, GameObject id `519420028` in `Level_00`): **orthographic**,
size **6.306675**, HDR on, MSAA off, solid-color background
`RGB(0.1333, 0.0604, 0.2170)` (dark purple), depth −1, near 0.3 / far 1000, full culling mask.
Full details: [Rendering & Post-Processing](rendering.md#camera-settings-level00-main-camera).

## `CameraBehaviour`

- Serialized: `target` (Transform — the player), `_FollowSpeed = 3`, `_MaxOffset = 3`,
  `isOffsetActive = true`.
- Runs in **`FixedUpdate`**; early-outs if `target == null` (warning) or `PauseController.isGamePaused`.
- Offset logic: when `isOffsetActive`,
  - mouse active → `offset = Vector2.ClampMagnitude(CrosshairMouse.AimDirection, _MaxOffset)`
    (the camera leans toward wherever the crosshair points, capped at 3 units),
  - else → `offset = TadaInput.AimAxisSmoothInput * _MaxOffset` (right stick drives the lean).
- Final position: `Vector3.Lerp(current, target.position + offset (z preserved), Time.deltaTime * FollowSpeed)`.

## `CameraShake` (static API, third-party)

Based on the ftvs GitHub gist (credited in README). Public **static** `CameraShake.Shake(duration,
shakeAmount, decreaseFactor)` — called from anywhere without a reference:

| Caller | Shake |
|---|---|
| `Weapon_ShootProjectileCanCharge.OnChargingStart()` | `Shake(2s, 0.065, 1)` — subtle charge rumble |
| `OnChargingEnd()` | `Shake(0.2s, 1, 3)` — big release kick |
| `OnChargeCancel()` | `Shake(0, 0, 0)` — stops the shake |

It checks `PauseController.isGamePaused` (line 42) so shakes don't tick while paused. Author's NOTE:
the charging shake overrides the release shake without a timer (defect — see
[Known Issues](known-issues-and-roadmap.md)).

## `CameraSizeHandler`

A test/utility class that adjusts orthographic size — **present but disabled**
(`m_Enabled: 0`) on `CameraPivot` in `Level_00`. Not active in gameplay.

## Crosshair system

```
Crosshair (abstract base)
├── CrosshairMouse     — static AimDirection; follows MouseWorldPos
└── CrosshairJoystick  — static AimDirection; orbits PlayerShoulderMain around the player

CrosshairHandler (scene root, enabled) — owns both, switches per frame
```

- `CrosshairHandler.Awake()` resolves both crosshairs via **`FindObjectOfType`** (deprecated in
  Unity 6 — see [Known Issues](known-issues-and-roadmap.md)).
- `Update()` every frame:
  - if `!TadaInput.IsMouseActive` → ensure mouse crosshair inactive / joystick active, then
    `joystickCrosshair.UpdateCrosshair()`;
  - else → ensure mouse active / joystick inactive, then `mouseCrosshair.UpdateCrosshair()`;
  - `CheckIfReady()` logs an error if either crosshair is missing.
- `CrosshairJoystick.Awake()` also does `FindObjectOfType<PlayerController>()` (for position) and
  `FindObjectOfType<PlayerShoulderMain>()` (orbit pivot).
- Both classes expose **static `AimDirection`** vectors consumed by:
  - `GUIHandler` (live readout), `CameraBehaviour` (offset), and the `LookAt2Dv2` aim pipeline
    feeding the player's arms (see [Player System](player.md#aim-rig-how-the-body-points-at-the-crosshair)).

## Device duality in one line

`TadaInput.IsMouseActive` (any mouse movement ⇒ mouse; right-stick movement ⇒ gamepad) decides
which crosshair updates, which offset source the camera uses, and which control hint
`GUIHandler` prints. See [Input System](input.md#mouse-vs-gamepad-duality-ismouseactive).

## Known gaps (camera/crosshair)

- Camera follows in `FixedUpdate` with `Time.deltaTime` (frame-rate dependent smoothing — standard
  Unity gotcha; `fixedDeltaTime` would be correct there).
- `FindObjectOfType` in crosshair startup paths.
- No camera bounds/clamping, no zoom in gameplay (`CameraSizeHandler` disabled), no Cinemachine.

## Related pages

- [Rendering & Post-Processing](rendering.md) — camera + volume rendering configuration.
- [Input System](input.md) — aim axes and `IsMouseActive`.
- [Environment, FX & Pause](environment-and-fx.md) — pause checks shared with the camera.
