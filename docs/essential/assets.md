# Assets Pipeline

[Index](index.md) · [Rendering](rendering.md) · [Scene Inventory](scenes.md)

Complete inventory of `Assets/` (excluding `.meta`), grouped by type, with naming conventions.

## Naming conventions (observed, follow them)

| Prefix | Meaning | Example |
|---|---|---|
| `pfab_` | prefab | `pfab_Wall_Frontal_A` |
| `pfx_` | particle-effect prefab | `pfx_Bullet_Laser_Charge_Yellow` |
| `mat_` | material | `mat_Bullet_Laser_Yellow` |
| `char_` | character texture | `char_main_skin_001` |
| `LaserRifle_…` / `LaserBeam_Rifle_…` | per-weapon folders/assets | materials, animations |
| FreeSound IDs + author + license | audio file names | `260155__emanuele-correani__…_CCBy3.wav` |

## Sprites (`Assets/Sprites/`)

```
Characters/char_main_skin_001.png                      — player skin sheet
Level/Ground/Ground_Seamless_Tile_01.png, Ground_Tiles.png
Level/Walls/Wall_Frontal_001_Color.png, Wall_Frontal_001_Glow.png
Level/Walls/Wall_Side_001_Color.png,    Wall_Side_001_Glow.png
Projectiles/Bullet_Laser.png
Weapons/Weapons_001.png                                — weapon sprite sheet
```

Walls use **Color + Glow** texture pairs (Glow feeds the HDR shader-graph shaders — see
[Rendering](rendering.md#shaders-shader-graph)).

## Audio (`Assets/Audio/`)

11 clips total, filenames encode source + license:

```
Music/Sci_Fi_Industries_-_01_-_Azimutez_CC_BY_NC_SA_3.mp3      (CC BY-NC-SA 3.0)
SFX/Impacts/330629__stormwaveaudio__sci-fi-force-field-impact-15_Mod1_CCby3.wav   (CC-BY-3.0)
SFX/Impacts/Laser_Wall_Impact_mod_of_dpren_cannon_shot_CC0.wav                    (CC0)
SFX/Skills/Dash/Dash_03_CCBY0.wav                                                  (CC0)
SFX/Weapons/Lasers/260155__emanuele-correani__short-laser-shots_Single_02_CCBy3.wav (CC-BY-3.0)
SFX/Weapons/Lasers/338783__st303__sci-fi-weapon-01_{AddingBullet_01, NoBullet_01, ShotOnly_01}_CC0.wav
SFX/Weapons/Lasers/440147__dpren__scifi-gun-mega-charge-cannon_{ChargeOnly_01, Shot_01, Shot_02}_CC0.wav
```

**License obligation**: CC-BY-3.0/CC-BY-NC-SA clips require attribution (already listed in
`README.md` credits); the NC music is non-commercial — relevant if the roadmap includes shipping.

## Materials (`Assets/Materials/`)

```
AfterImages/mat_AfterImages_00 … _05.mat        (6-frame ghost trail set)
Particles/mat_Generic_Particle_HDR.mat
Player/mat_PlayerBody.mat                       (highlight/flash target)
Projectiles/mat_Bullet_Laser_{Blue,Cyan,Green,Magenta,Yellow}.mat
Weapons/LaserBeam_Rifle/LaserBeam_Rifle_Energy_{Battery,Body,Muzzle}.mat
Weapons/LaserRifle_Auto-Charge/LaserRifle_Auto-Charge_Energy.mat
```

Plus `Assets/Physics Materials/NoFriction.physicsMaterial2D` (used by player/projectile bodies).

## Prefabs (`Assets/Prefabs/`) — 17 total

```
Environment/Ground/pfab_Tiles_A.prefab                       (currently unused in scenes)
Environment/Walls/pfab_Wall_Frontal_A.prefab                 (×88 in Level_00)
Environment/Walls/pfab_Wall_Side_A.prefab                    (×78)
Environment/Walls/Triggers/pfab_EnvTrigger_FrontWall_Left_Size1.prefab   (×2)
Environment/Walls/Triggers/pfab_EnvTrigger_FrontWall_Left_Size2.prefab   (×2, ×1 in test)
Environment/Walls/Triggers/pfab_EnvTrigger_FrontWall_Right_Size1.prefab  (×2)
Environment/Walls/Triggers/pfab_EnvTrigger_SideWall_Top_Size1.prefab     (×6)
Environment/Walls/Triggers/pfab_EnvTrigger_SideWall_Bottom_Size1.prefab  (unused)
FX/AfterImage/pfab_AfterImage.prefab
Particles/pfx_Bullet_Laser_Charge_Yellow.prefab               (×2 in each scene)
Particles/pfx_Bullet_Laser_Impact_{Big_Yellow, Mid_Blue, Small_Cyan, Small_Green}.prefab
Particles/pfx_Warp_Yellow.prefab                              (×1 in Level_00)
Projectiles/pfab_Bullet_Laser_{Huge_Yellow, MidSize_Blue, Small_Cyan}.prefab
```

Wall prefabs each contain an `EnvironmentObject`; trigger prefabs contain `EnvironmentObjectTrigger`
with cross-references into scene wall instances (which serialize as stripped components —
see [Scene Inventory](scenes.md#script-component-census-serialized-mscript-occurrences)).

## Animation (`Assets/Sprites/Animations/`) — legacy `Animation` clips

```
Main Character/{Idle, WalkForward, WalkBackwards}.anim
Weapons/LaserBeam_Rifle/{Idle, BasicShot, Charging, ChargedShot}.anim
Weapons/LaserRifle_Auto-Charge/{Idle, BasicShot, Charging, ChargedShot}.anim
```

Played through the `AnimationHandler` base class (`PlayerAnimations`, `WeaponAnim_…`), **not**
Animator/AnimatorController — there are no `.controller` assets.

## Shaders, rendering, volume

- 5 Shader Graph shaders — [Rendering](rendering.md#shaders-shader-graph).
- `Assets/Rendering/{Forward_Low_NoLights, UniversalRenderPipelineAsset_Renderer}.asset`.
- `Assets/Scenes/Level_00/Post Processing Volume.asset`, `Assets/DefaultVolumeProfile.asset`.
- `Assets/UniversalRenderPipelineGlobalSettings.asset` (upgrade-created).

## What's missing (roadmap-relevant)

No fonts/UI atlases, no enemy/NPC art, no tilemaps (despite `2d.tilemap` package), no
`.psd` sources (despite `2d.psdimporter`), no AudioMixer assets, no prefabs for player/camera
(both are scene-only hierarchies — duplication risk noted in
[Scene Inventory](scenes.md#singlewalltestunity-test-scene)).

## Related pages

- [Rendering](rendering.md) — how these assets are drawn.
- [Scene Inventory](scenes.md) — where each prefab is instantiated.
- [Known Issues & Roadmap Foundation](known-issues-and-roadmap.md).
