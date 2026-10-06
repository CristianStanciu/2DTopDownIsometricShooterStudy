# Input System

[Index](index.md) · [Architecture](architecture.md) · [Camera & Crosshair](camera-and-crosshair.md)

## Overview

The project uses Unity's **legacy Input Manager** (`UnityEngine.Input`) — there is **no**
`com.unity.inputsystem` package. All access is funnelled through **`TadaInput`**
(`Assets/Scripts/Inputs/TadaInput.cs`), a scene MonoBehaviour that polls every frame and exposes a
**static API** (`TadaInput.GetKey(...)`, `TadaInput.MoveAxisRawInput`, …), mirroring Unity's own
`Input.GetKey` ergonomics.

`TadaInput` is a root GameObject in **both scenes** (must exist exactly once). Its static state is
cleared before every play session by `ResetStaticState()` (a `[RuntimeInitializeOnLoadMethod]`,
P0.E3.T8), so `GetKeyDown` state can't leak across sessions.

## The `ThisKey` enum

```csharp
None, MoveLeft, MoveRight, MoveUp, MoveDown, PrimaryAction, SecondaryAction,
PreviousWeapon, NextWeapon, PreviousUseRate, NextUseRate, Xbox360RightTrigger,
Xbox360LeftTrigger, MouseAnyMovement, Dash, Pause, WeaponSlot1, WeaponSlot2, Count
```

State is stored in four arrays sized `Count + 1`: `currentKeys` (held), `currentKeysDown`,
`currentKeysUp`, `currentAxisDown`. API semantics:

- `GetKey(key)` — true while held (non-consuming).
- `GetKeyDown(key)` / `GetKeyUp(key)` — true for **one poll then cleared** (consuming: the slot is
  reset to `None` on read).
- Axes can be converted into virtual keys via `StoreCurrentAxisAsKeyType`: value `> 0` → down+held,
  `== 0` → up. Used for the analog triggers and `MouseAnyMovement` (any mouse motion → key held).

## Key bindings (as implemented in `TadaInput.Update()`)

| Action | Keyboard/Mouse | Gamepad (Joystick1 / axes) |
|---|---|---|
| Move | `A/D/W/S` → `MoveLeft/Right/Up/Down` + `Horizontal`/`Vertical` axes | Left stick (`Horizontal`/`Vertical`) |
| Aim | Mouse position (see below) | Right stick → `HorizontalAim`/`VerticalAim` custom axes |
| Primary action | `LMB` | `Xbox360RightTrigger` axis → virtual key |
| Secondary action | `RMB` | `Xbox360LeftTrigger` axis → virtual key |
| Dash | `Space` | `Joystick1Button5` (RB) |
| Previous / Next weapon | `Q` / `E`; mouse **scroll wheel** also maps to Previous/Next | `Joystick1Button0` (A) / `Joystick1Button1` (B) |
| Previous / Next fire rate | `C` / `V` | `Joystick1Button9` (L3) for Next only |
| Pause | `Escape` | `Joystick1Button7` (Start) |
| Weapon slot 0 / 1 | `Alpha1` / `Alpha2` → `WeaponSlot1`/`WeaponSlot2` (via `TadaInput`, P0.E3.T4) | `Joystick1Button2` (X) / `Joystick1Button3` (Y) → `WeaponSlot1`/`WeaponSlot2` |

The scroll wheel has special handling (`isScrollWheelActive`) so one wheel movement emits a single
`KeyDown` instead of continuous `KeyUp` spam (`// TODO: Improve mouseScrollDelta input conditions`).

## Mouse vs gamepad duality (`IsMouseActive`)

- `_isMouseActive = true` when `_MouseInput.sqrMagnitude > 0` (any mouse movement);
- set back to `false` as soon as `_AimAxisRawInput.sqrMagnitude > 0` (right-stick movement).
- Consumed by: `CrosshairHandler` (which crosshair is active), `CameraBehaviour` (offset source),
  `GUIHandler` (which control-scheme hint to display).

## Exposed state (static properties)

| Property | Meaning |
|---|---|
| `IsMouseActive` | device duality flag (above) |
| `MouseInput` | `Mouse X/Y` axes, clamped to magnitude 1 |
| `MousePixelPos` | `Input.mousePosition` with `z = 20` |
| `MouseWorldPos` | `cam.ScreenToWorldPoint(...)`, `z = 0` (camera cached in `Awake`) |
| `MoveAxisSmoothInput` / `MoveAxisRawInput` | `Horizontal/Vertical` (smoothed or raw), diagonals clamped to 1 |
| `AimAxisSmoothInput` / `AimAxisRawInput` | custom `HorizontalAim/VerticalAim` axes, clamped to 1 |

## Custom axes in `ProjectSettings/InputManager.asset`

Added on top of the Unity defaults:

```
HorizontalAim        (right stick X)
VerticalAim          (right stick Y)
Xbox360LeftTrigger   (trigger axis)
Xbox360RightTrigger  (trigger axis)
```

plus the stock `Horizontal`, `Vertical`, `Mouse X`, `Mouse Y`, `Mouse ScrollWheel`, `Fire1/2/3`,
`Jump`, `Submit`, `Cancel`, debug axes. Author note in code: *"To support all controllers, use
these names and add new inputs to Unity's Input Manager. At the moment it only supports Xbox
controllers."*

## Control scheme overlay

`GUIHandler` (enabled in both scenes) draws the canonical control strings at the bottom of the
screen plus live aim vectors (`CrosshairMouse.AimDirection`, `CrosshairJoystick.AimDirection`),
switching keyboard vs gamepad text based on `TadaInput.IsMouseActive`. See
[Getting Started](getting-started.md#controls-verbatim-from-guihandler-overlay).

## Known issues (input)

- Trigger axis mapping assumes Xbox-style `0…1` positive axes (axis-as-key only tests `> 0` / `== 0`;
  a `-1…1` trigger would never release the key).
- Full list: [Known Issues & Roadmap Foundation](known-issues-and-roadmap.md).

**Resolved in P0.E3:** direct `Alpha1/2` reads bypassing `TadaInput` (→ `WeaponSlot1/2`, T4);
static state leaking across play sessions (`ResetStaticState`, T8); dead `MouseAnyMovement` enum
member (now stored with axis semantics, T8); duplicate `KeyUp(NextUseRate)` block for `V` (T8).

## Related pages

- [Camera & Crosshair](camera-and-crosshair.md) — consumers of `IsMouseActive`/aim axes.
- [Player System](player.md) — `PlayerController` dispatch table.
- [Project Settings](project-settings.md#input-axes-inputmanagerasset--legacy-input-manager) — raw axis configuration.
