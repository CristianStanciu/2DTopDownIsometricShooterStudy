# Scene Inventory

[Index](index.md) · [Architecture](architecture.md) · [Project Settings](project-settings.md)

Two scenes exist under `Assets/Scenes/`. Only `Level_00` is enabled in
`EditorBuildSettings` (a leftover second entry has `enabled: 0` and an empty path).

## `Level_00.unity` (main)

**13 root GameObjects** (verified from transform parents):

```
TadaInput | Post Processing | Player | CameraPivot | SoundManager | MusicHandler
CrosshairHandler | Level_01 | Level | AfterImages | MouseCrosshair
PauseController | DebugHandler | JoystickCrosshair
```

### Root responsibilities

| Root | Contents |
|---|---|
| `TadaInput` | input manager component |
| `Post Processing` | URP `Volume` + `Post Processing Volume.asset` profile (see [Rendering](rendering.md)) |
| `Player` | full player rig + weapons (see [Player System](player.md#hierarchy-as-serialized-in-both-scenes)) |
| `CameraPivot` | child `Main Camera`; components `CameraBehaviour` (on), `CameraShake` (on), `CameraSizeHandler` (**off**) |
| `SoundManager` | singleton (`DontDestroyOnLoad`), spawns emitters at runtime |
| `MusicHandler` | music autoplay (volume 0.65) |
| `CrosshairHandler` | crosshair switching |
| `Level_01` | newer level geometry: `SnapPoints` + `Ground` children |
| `Level` | original room block: `Room_A_{Center,Top,Bottom,Left,Right,TopLeft,TopRight,BottomLeft,BottomRight}`, wall groups `Top_FrontWalls`, `Bot_FrontWalls`, `L_SideWalls`, `R_SideWalls`, `SnapPoints` |
| `AfterImages` | after-image handler hierarchy |
| `MouseCrosshair` / `JoystickCrosshair` | the two crosshair objects |
| `PauseController` | pause toggling + UnityEvents |
| `DebugHandler` | `GUIHandler` (on) + `GizmosHandler` (on) |

### Prefab instances (verified counts)

| Prefab | Count |
|---|---|
| `pfab_Wall_Frontal_A` | **88** |
| `pfab_Wall_Side_A` | **78** |
| `pfab_EnvTrigger_SideWall_Top_Size1` | 6 |
| `pfab_EnvTrigger_FrontWall_Right_Size1` | 2 |
| `pfab_EnvTrigger_FrontWall_Left_Size2` | 2 |
| `pfab_EnvTrigger_FrontWall_Left_Size1` | 2 |
| `pfx_Bullet_Laser_Charge_Yellow` | 2 |
| `pfx_Warp_Yellow` | 1 |
| **Total** | **181 instances** (166 walls + 12 triggers + 3 particles) |

Unused-by-this-scene prefabs: `pfab_Tiles_A`, `pfab_EnvTrigger_SideWall_Bottom_Size1`,
projectile prefabs (spawned at runtime), impact particles (spawned at runtime),
`pfab_AfterImage` (spawns via generator).

### Script component census (serialized `m_Script` occurrences)

| Script | Count | Note |
|---|---|---|
| `AfterImageGenerator` | **17** | one per rendered player part |
| `EnvironmentObject` | 76 stripped refs | wall prefab components referenced by trigger arrays (walls also each carry one inside their prefab) |
| `LookAt2Dv2` | 4 | aim rotation targets |
| `Weapon_ShootProjectileCanCharge` | 2 | the two weapons |
| `WeaponAnim_ShootProjectileCanCharge` | 2 | weapon animators |
| `SoundHandlerLocal` | 2 | weapon charge SFX |
| **Single instances (1 each)** | | `CameraShake`, `LookAt2Dv2Handler`, `LookAt2Dv2Updater`, `CrosshairHandler`, `CrosshairMouse`, `CrosshairJoystick`, `PlayerController`, `PlayerAnimations`, `PlayerPhysics`, `PlayerSkills`, `PlayerBodyPartsHandler`, `PlayerHandTargetToLookAt`, `PlayerShoulderMain`, `PlayerShoulderSecondary`, `PlayerMaterials`, `PlayerHandTargetToLookAt`, `WeaponHandler`, `AfterImageHandler`, `AfterImageMaterials`, `SoundManager`, `SoundHandlerGlobal`, `MusicHandler`, `TadaInput`, `PauseController`, `GUIHandler`, `GizmosHandler`, `CameraBehaviour`, `CameraSizeHandler` |

`Projectile` does **not** appear (runtime-instantiated);
`SoundFXEmitter`/`MusicEmitter`/`UnstoppableSoundEmitter` don't appear (runtime-created by
`SoundManager`); `EnvironmentObjectTrigger` lives inside trigger prefabs.

## `SingleWall_Test.unity` (test scene)

**13 root GameObjects** — same rig as `Level_00` except there is **no `Level` / `Level_01`**;
instead a single root **`Level_00`** (confusingly named) containing `Room_A_Center`:

```
TadaInput | Post Processing | Player | CameraPivot | SoundManager | MusicHandler
CrosshairHandler | Level_00 | AfterImages | MouseCrosshair
PauseController | DebugHandler | JoystickCrosshair
```

| Fact | Value |
|---|---|
| Purpose | minimal harness for wall occlusion tests |
| Prefab instances | `pfab_Wall_Frontal_A` ×1, `pfab_EnvTrigger_FrontWall_Left_Size2` ×1, `pfx_Bullet_Laser_Charge_Yellow` ×2 |
| Script census | identical player/audio/camera singles as `Level_00`; `EnvironmentObject` ×1 direct; same 17 `AfterImageGenerator`, 4 `LookAt2Dv2`, 2 weapons + 2 weapon anims, 2 `SoundHandlerLocal` |
| GameObject component refs | 190 (vs 355 in `Level_00`) |

Both scenes share the entire gameplay rig — they differ **only** in level geometry. This means any
scene-level setup (new managers, wiring) must usually be done **twice** (roadmap risk: see
[Known Issues](known-issues-and-roadmap.md)).

## Scene file facts

| Property | `Level_00.unity` | `SingleWall_Test.unity` |
|---|---|---|
| GUID (build settings) | `2cda990e2423bbf4892e6590ba056729` | not in build |
| In build | yes (index 0, enabled) | no |
| Re-serialized by Unity 6 upgrade commit `c9d5f0e` | yes (~4.3k lines changed) | **no** by the upgrade — re-serialized later by **P0.E2.T1** (2026-10-05: +800/−207 lines, census preserved) |
| Lighting settings asset embedded | `Settings.lighting` | `SingleWall_TestSettings.lighting` (external — extracted by P0.E2.T1) |

## Runtime-created objects (not in any scene file)

| Object | Created by | When |
|---|---|---|
| `SoundFXs`, `UnstoppableSoundFXs`, `Music` (children of `SoundManager`) | `SoundManager.Awake` | first load, if not found |
| Basic + charged projectile clones per weapon | `Weapon_ShootProjectileCanCharge.SpawnProjectiles()` | `OnEnable`/`OnCanUse` |
| Impact particles | `Projectile.OnTriggerEnter2D` | on wall hit |
| Ghost copies | `AfterImageGenerator` | during dash |

## Related pages

- [Player System](player.md) · [Environment, FX & Pause](environment-and-fx.md) ·
  [Rendering](rendering.md) · [Known Issues](known-issues-and-roadmap.md)

