# Environment, FX & Pause

[Index](index.md) · [Player System](player.md) · [Audio System](audio.md)

## Wall occlusion system (`Assets/Scripts/Environment/`)

The isometric look requires walls to swap which side is drawn so the player is never hidden.
Two classes implement this:

### `EnvironmentObject` (on every wall)

- Enum `WhenPlayerIsOnPosition { Above, Left, Right, Below }` + serialized
  `startsOnPlayerPosition` (default `Right`).
- Four serialized GameObject slots: `whenPlayerIsAbove`, `whenPlayerIsOnLeft`,
  `whenPlayerIsOnRight`, `whenPlayerIsOnBelow` — the wall's **façade variants**.
- `Awake()`: caches the slots array, activates the starting variant, `TryGetComponent<SpriteRenderer>`
  (optional debug `CheckIfMissingObjects()` when its private `debug` flag is on).
- `SetActiveObject(position)`: disables **all** variants, then enables the one matching the
  player's side. (The GameObject *holding* `EnvironmentObject` stays active; only children swap.)

### `EnvironmentObjectTrigger` (on trigger volumes)

- Four serialized arrays: `objectsAbovePlayer`, `objectsLeftOfPlayer`, `objectsRightOfPlayer`,
  `objectsBelowPlayer` (of `EnvironmentObject`).
- `SetActiveObjects()` maps player position → wall variant:
  - player *above* a wall → that wall shows its **Below** variant,
  - player *left* → **Right** variant, player *right* → **Left** variant, player *below* → **Above** variant.

### The full loop

1. Trigger prefabs (`pfab_EnvTrigger_*`) sit at room edges, tagged **`EnvironmentTrigger`**,
   with a trigger `Collider2D` and hand-wired `EnvironmentObject[]` arrays pointing at
   neighbouring wall instances.
2. When the player enters, `PlayerPhysics.OnTriggerEnter2D` detects the tag and calls
   `collision.TryGetComponent(out EnvironmentObjectTrigger)` → `SetActiveObjects()`
   (`PlayerPhysics.cs` lines 30–40).
3. Every affected wall swaps its façade.

**Content**: `Level_00` has **166 wall prefab instances** (88 `pfab_Wall_Frontal_A` + 78
`pfab_Wall_Side_A`) and **12 trigger instances** (6× `pfab_EnvTrigger_SideWall_Top_Size1`,
2× each `..._FrontWall_Right_Size1`, `..._FrontWall_Left_Size2`, `..._FrontWall_Left_Size1`);
`SingleWall_Test` has 1 wall + 1 trigger (`pfab_EnvTrigger_FrontWall_Left_Size2`). Each wall
prefab carries an `EnvironmentObject` component. See [Scene Inventory](scenes.md).

**Gap**: `OnTriggerExit2D` in `PlayerPhysics` is empty — walls only re-swap on the *next* enter.
Also `EnvironmentObjectTrigger.SetActiveObjects()` has no null guards on the arrays themselves
(null *elements* are guarded).

## Dash after-image FX (`Assets/Scripts/FX/AfterImage/`)

| Class | Role |
|---|---|
| `AfterImageHandler` | Scene singleton-ish holder: `List<AfterImageGroup>`; `AddAfterImageGroup(group)` parents+registers; `SetActiveAfterImages()` activates every group. Called by `PlayerSkills.Dash()`. |
| `AfterImageGenerator` | Attached **17× per scene** (one per rendered player part). Finds `AfterImageMaterials` via `FindObjectOfType` and spawns/updates ghost copies. |
| `AfterImageGroup` | A set of ghosts for one part; resolves `AfterImageHandler` via `FindObjectOfType` in startup path. |
| `AfterImage` | Single ghost sprite instance (fade/return behavior). |
| `AfterImageMaterials` | Scene object (1× per scene) holding the 6-frame material set `mat_AfterImages_00…05` used to stagger the ghost trail. |

Prefab: `Assets/Prefabs/FX/AfterImage/pfab_AfterImage.prefab`. Root object **`AfterImages`** hosts
the handler hierarchy in both scenes. `PlayerSkills` also toggles the dash `TrailRenderer`s
(`EyeTrail`, `FootTrail`) around the same event.

**Gaps**: heavy `FindObjectOfType` startup usage; 17 generators per scene is a per-part
instantiation cost; no pooling beyond reuse of the pre-made groups.

## Pause (`PauseController`)

Root object **`PauseController`** in both scenes (enabled), class in
`Assets/Scripts/Global Managers and Controllers/`.

- Static `bool isGamePaused`; public `UnityEvent onGamePause`, `onGameResume` — explicitly designed
  as inspector hooks "to easily connect UI actions without hardcoding" (a future pause menu's seam).
- `Update()` toggles on `TadaInput.GetKeyDown(Pause)` (Esc / Joystick1Button7 / Start).
- `OnGamePause()`: `Time.timeScale = 0`; `AudioListener.pause = true`; invoke `onGamePause`.
- `OnGameResume()`: `Time.timeScale = 1`; `AudioListener.pause = false`; invoke `onGameResume`.

**Consumers of `isGamePaused`** (verified by grep): `PlayerController.Update`,
`CameraBehaviour.FixedUpdate`, `CameraShake`, `PlayerBodyPartsHandler`. `TadaInput` itself keeps
polling while paused (by design so Esc can resume).

## Debug tooling (`Assets/Scripts/Debug/`)

- **`GUIHandler`** (root `DebugHandler`, enabled): OnGUI yellow labels showing
  `PlayerPos->MousePos` aim vector, `PlayerPos->CrosshairJoyDir` vector, and the control scheme
  string for the active device (see [Getting Started](getting-started.md#controls-verbatim-from-guihandler-overlay)).
  Toggle via its `drawGUI` field.
- **`GizmosHandler`** (enabled): gizmo drawing toggles for editor-time visualization.

## Related pages

- [Scene Inventory](scenes.md) — exact placement/counts of all the objects above.
- [Player System](player.md) — dash + trigger callback source.
- [Known Issues & Roadmap Foundation](known-issues-and-roadmap.md) — defect list.
