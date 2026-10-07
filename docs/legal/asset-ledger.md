# Asset Ledger — provenance, license, commercial-use

**Task:** P0.E8.T1 · **Created:** 2026-10-07
**Inventory check:** `scripts/get-asset-ledger.ps1 -Check` (CI job `asset-ledger`, runs on
every push/PR — fails if an asset under `Assets/Audio`, `Assets/Sprites` or any font file is
missing from this ledger, carries a GUID mismatch, or is listed here but no longer exists).

**Maintaining this file**
1. Add the asset following the naming convention (audio: license token in the filename —
   see [Assets Pipeline](../essential/assets.md#naming-conventions)).
2. Run `powershell -NoProfile -File scripts/get-asset-ledger.ps1` and paste the emitted rows
   (`path | GUID | license`) into the matching table below.
3. Fill origin / commercial / attribution columns. Upstream audio origins come from the
   README [credits](../../README.md#credits).
4. Re-run with `-Check` — it must print `Ledger OK`.

**Commercial column:** `YES` = usable in a commercial release · `YES*` = usable but
attribution is mandatory (keep in the in-game credits, P3.E3.T7) · `NO` = non-commercial
terms — **replace or relicense before paid release** (remediation: P0.E8.T2 → P8.E3.T4) ·
`GAP` = provenance incomplete — verify or replace.

## Audio (11 clips)

| Path | GUID | Origin | License | Commercial | Attribution / notes |
|---|---|---|---|---|---|
| `Assets/Audio/Music/Sci_Fi_Industries_-_01_-_Azimutez_CC_BY_NC_SA_3.mp3` | `915836caddf064a4fb104f955d3731a2` | [Free Music Archive — Sci Fi Industries](https://freemusicarchive.org/music/Sci_Fi_Industries/Blame_the_Lord/01_sci_fi_industries_-_azimutez) | CC BY-NC-SA 3.0 | **NO** | "Azimutez" by Sci Fi Industries — non-commercial + share-alike. Replacement brief = P0.E8.T2; used by `MusicHandler` (both scenes). |
| `Assets/Audio/SFX/Impacts/330629__stormwaveaudio__sci-fi-force-field-impact-15_Mod1_CCby3.wav` | `5c878401132cf284290df85c259f5784` | [Freesound — StormwaveAudio #330629](https://freesound.org/people/StormwaveAudio/sounds/330629/) | CC BY 3.0 | YES* | "Sci-Fi Force Field Impact 15 by StormwaveAudio (Freesound) - CC-BY-3.0" (Mod1 = edited with Audacity per upstream README). |
| `Assets/Audio/SFX/Impacts/Laser_Wall_Impact_mod_of_dpren_cannon_shot_CC0.wav` | `f830748fb4ec34d46b23346a26c77004` | Modification of dpren's cannon shot ([Freesound #440147](https://freesound.org/people/dpren/sounds/440147/)) by Tadadosi/Audacity | CC0 1.0 | YES | Derived from a CC0 source — no obligations; crediting dpren optional. |
| `Assets/Audio/SFX/Skills/Dash/Dash_03_CCBY0.wav` | `f89001437c5cc0c49abbcefc91140604` | **GAP** — not listed in upstream README credits; origin page unrecovered (capture receipt when found: P0.E8.T5) | CC BY 0.0 (filename-declared) | YES | No attribution required either way; provenance gap is the only open item. |
| `Assets/Audio/SFX/Weapons/Lasers/260155__emanuele-correani__short-laser-shots_Single_02_CCBy3.wav` | `48e3d612dec52554ca0d8c01bf518e90` | [Freesound — Emanuele_Correani #260155](https://freesound.org/people/Emanuele_Correani/sounds/260155/) | CC BY 3.0 | YES* | "Short Laser Shots by Emanuele_Correani (Freesound) - CC-BY-3.0". |
| `Assets/Audio/SFX/Weapons/Lasers/338783__st303__sci-fi-weapon-01_AddingBullet_01_CC0.wav` | `610fae0dafa5cab40987273b4c24deac` | [Freesound — ST303 #338783](https://freesound.org/people/ST303/sounds/338783/) | CC0 1.0 | YES | "Sci_FI_Weapon_01 by ST303 (Freesound) - CC0 1.0" (credited voluntarily). |
| `Assets/Audio/SFX/Weapons/Lasers/338783__st303__sci-fi-weapon-01_NoBullet_01_CC0.wav` | `e8ea00f759269e74fa3e1b250cd68eb6` | [Freesound — ST303 #338783](https://freesound.org/people/ST303/sounds/338783/) | CC0 1.0 | YES | Same pack as above. |
| `Assets/Audio/SFX/Weapons/Lasers/338783__st303__sci-fi-weapon-01_ShotOnly_01_CC0.wav` | `a23300688b0d9c642959a9f100c94349` | [Freesound — ST303 #338783](https://freesound.org/people/ST303/sounds/338783/) | CC0 1.0 | YES | Same pack as above. |
| `Assets/Audio/SFX/Weapons/Lasers/440147__dpren__scifi-gun-mega-charge-cannon_ChargeOnly_01_CC0.wav` | `0371498ebbd6306488066dfbdda41460` | [Freesound — dpren #440147](https://freesound.org/people/dpren/sounds/440147/) | CC0 1.0 | YES | "SciFi Gun - Mega Charge Cannon by dpren (Freesound) - CC0 1.0" (credited voluntarily). |
| `Assets/Audio/SFX/Weapons/Lasers/440147__dpren__scifi-gun-mega-charge-cannon_Shot_01_CC0.wav` | `697ba87d5a9247d4caf18aecafeb6d9e` | [Freesound — dpren #440147](https://freesound.org/people/dpren/sounds/440147/) | CC0 1.0 | YES | Same pack as above. |
| `Assets/Audio/SFX/Weapons/Lasers/440147__dpren__scifi-gun-mega-charge-cannon_Shot_02_CC0.wav` | `d98338c585bbd114bb65e86475f32056` | [Freesound — dpren #440147](https://freesound.org/people/dpren/sounds/440147/) | CC0 1.0 | YES | Same pack as above. |

## Sprites (9 PNG + authored `.anim` clips)

All pixel art below was imported with the original study (upstream commits `90e74e3` v0.1 /
`5f05774` v0.2 — https://github.com/tadadosii/2DTopDownIsometricShooterStudy). The upstream
README License section states: *"The MIT License … does not include the sprites and the audio
files. Pixel art sprites are free for personal use."* → **personal-use only = NO commercial
use for every row.** Remediation: written permission from Tadadosi (receipt → P0.E8.T5) or
art replacement (P8.E3.T4). The `.anim` clips stored next to these sheets are authored
animation data (part of the MIT project setup, not art assets).

| Path | GUID | Origin | License | Commercial | Role / notes |
|---|---|---|---|---|---|
| `Assets/Sprites/Characters/char_main_skin_001.png` | `6a8a497184980dd4080e64d88b08d5c2` | Upstream study (personal-use grant) | Personal use only | **NO** | Player skin sheet. |
| `Assets/Sprites/Level/Ground/Ground_Seamless_Tile_01.png` | `deec21617e896ab4a8dab3506841d7c2` | Upstream study (personal-use grant) | Personal use only | **NO** | Seamless ground tile. |
| `Assets/Sprites/Level/Ground/Ground_Tiles.png` | `7848538534479494ca1d86273d3fe763` | Upstream study (personal-use grant) | Personal use only | **NO** | Ground tile set. |
| `Assets/Sprites/Level/Walls/Wall_Frontal_001_Color.png` | `0eec548e9da56974d8022f850983e341` | Upstream study (personal-use grant) | Personal use only | **NO** | Wall color map (pair with Glow). |
| `Assets/Sprites/Level/Walls/Wall_Frontal_001_Glow.png` | `99914923e127f1f4cba320131c6bcd5d` | Upstream study (personal-use grant) | Personal use only | **NO** | Wall emissive/glow map. |
| `Assets/Sprites/Level/Walls/Wall_Side_001_Color.png` | `22efe2d4b4d789f4990b5dce906c6b2b` | Upstream study (personal-use grant) | Personal use only | **NO** | Side-wall color map. |
| `Assets/Sprites/Level/Walls/Wall_Side_001_Glow.png` | `3578bcf451c8ed649b572e59791b3309` | Upstream study (personal-use grant) | Personal use only | **NO** | Side-wall glow map. |
| `Assets/Sprites/Projectiles/Bullet_Laser.png` | `bebe3451e71bbd44abc913d4a47b1356` | Upstream study (personal-use grant) | Personal use only | **NO** | Laser bullet sprite. |
| `Assets/Sprites/Weapons/Weapons_001.png` | `cc0b6187cdbef634f9861383b3d749f9` | Upstream study (personal-use grant) | Personal use only | **NO** | Weapon sprite sheet. |

## Fonts

No font files exist under `Assets` (the scan proves it stays that way). Record any font added
later with its origin/license here before use. Note: the default TextMeshPro font asset ships
from `com.unity.ugui` (LiberationSans, SIL OFL 1.1) — nothing to track until custom text is used.

## UPM packages (Packages/manifest.json + packages-lock.json)

- All **64 locked packages are Unity-official** (`com.unity.*`), zero third-party UPM deps:
  Unity Companion License (per-package `LICENSE.md` in `Library/PackageCache`) — commercial
  use permitted in Unity-built products. IDE packages (rider/visualstudio/vscode) are
  editor-only and never shipped in builds.
- **Shader Graph:** `com.unity.shadergraph` 17.3.0 is a transitive dependency of
  `com.unity.render-pipelines.universal` 17.3.0 (same Unity Companion License).
- Re-verify this section whenever the manifest changes; the CI deterministic-restore gate
  keeps `packages-lock.json` stable.

## Third-party code (feeds P0.E8.T4 THIRD-PARTY.md)

| Component | Origin | License | Notes |
|---|---|---|---|
| `CameraShake` | https://gist.github.com/ftvs/5822103 | no explicit license on gist | Credited in README; keep attribution, or replace with a known-license shake in P4. |
| `UnitySingleton` pattern | https://github.com/UnityCommunity/UnitySingleton | MIT | Used by `Singleton<T>`; keep copyright notice. |
| `sampleREADME.md` template | https://gist.github.com/fvcproductions/1bfc2d4aecb01a834b46 | gist, attribution kept | Docs text only — not shipped in the game. |

## Findings (2026-10-07) & obligations

1. **Commercial blockers** (before paid marketing P9 / RC): the music clip (CC BY-NC-SA)
   **and all 9 pixel-art sprites (personal-use-only)** are non-commercial. P0.E8.T2 brief
   scope = music *and* sprites; replacement/licensing must land per P8.E3.T4 before RC.
2. **Attribution required in the in-game credits (P3.E3.T7):** Emanuele_Correani
   (CC-BY-3.0), StormwaveAudio (CC-BY-3.0), Sci Fi Industries (until the NC track is
   replaced). CC0/CC-BY-0 clips carry no attribution obligation (credited voluntarily per
   upstream README).
3. **Provenance gaps:** `Dash_03_CCBY0.wav` origin page unrecovered → capture a receipt when
   found (P0.E8.T5); sprite permission-vs-replacement decision pending.
4. `LICENSE` (MIT, © 2020 Tadadosi) is asserted to cover code/project setup only — upstream
   README explicitly excludes sprites/audio. Confirm scope + generate `THIRD-PARTY.md` = P0.E8.T4.

