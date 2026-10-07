# Third-Party Notices

> **Generated from** [`docs/legal/asset-ledger.md`](docs/legal/asset-ledger.md) (P0.E8.T4).
> The ledger is the source of truth — its inventory is CI-checked by the `asset-ledger` job
> (`scripts/get-asset-ledger.ps1 -Check`). When the ledger changes, mirror the change here.
> Rendered in-game by the credits & legal screen (P3.E3.T7).

## License scope of this repository

The root [`LICENSE`](LICENSE) (MIT, © 2020 Tadadosi) covers **the code and Unity project
setup only**. The upstream README states: *"The MIT License … does not include the sprites and
the audio files. Pixel art sprites are free for personal use."* Third-party art and audio are
tracked per file in the ledger; third-party code is listed below.

## Required attributions (in-game credits, P3.E3.T7)

| Component | Author | License | Source |
|---|---|---|---|
| Short Laser Shots | Emanuele_Correani | CC BY 3.0 | https://freesound.org/people/Emanuele_Correani/sounds/260155/ |
| Sci-Fi Force Field Impact 15 (Mod1) | StormwaveAudio | CC BY 3.0 | https://freesound.org/people/StormwaveAudio/sounds/330629/ |
| Azimutez *(non-commercial — being replaced, P0.E8.T2)* | Sci Fi Industries | CC BY-NC-SA 3.0 | https://freemusicarchive.org/music/Sci_Fi_Industries/Blame_the_Lord/01_sci_fi_industries_-_azimutez |

Credit the CC0/CC-BY-0 clips voluntarily (no obligation): Sci_FI_Weapon_01 by ST303
(https://freesound.org/people/ST303/sounds/338783/), SciFi Gun – Mega Charge Cannon by dpren
(https://freesound.org/people/dpren/sounds/440147/), plus the dpren-derived wall-impact mod.

## Third-party code

| Component | Author | License | Source |
|---|---|---|---|
| `CameraShake` | ftvs | no explicit license on the gist — attribution kept; replace with a known-license helper if reused broadly | https://gist.github.com/ftvs/5822103 |
| `Singleton<T>` pattern | UnityCommunity | MIT | https://github.com/UnityCommunity/UnitySingleton |
| `sampleREADME.md` template | fvcproductions | gist (attribution kept) — docs text only, not shipped | https://gist.github.com/fvcproductions/1bfc2d4aecb01a834b46 |

## Non-commercial assets in the current build (replace before P9/RC — P8.E3.T4)

- Music loop: **Azimutez** — CC BY-NC-SA 3.0.
- **All 9 pixel-art sheets** — personal-use-only per upstream README.
- Replacement plan, specs and budgets: [`docs/legal/replacement-brief.md`](docs/legal/replacement-brief.md).

## Unity packages

All UPM dependencies (64 locked `com.unity.*` packages, including
`com.unity.shadergraph` 17.3.0 via Universal RP 17.3.0) are Unity-official packages under the
**Unity Companion License** ([Unity Terms of Service](https://unity.com/legal/as-terms-of-service));
editor-only IDE packages never ship in builds. Full list: `Packages/manifest.json` +
`Packages/packages-lock.json`. No third-party (non-Unity) UPM packages are present.

## License texts

- CC BY 3.0: https://creativecommons.org/licenses/by/3.0/
- CC BY-NC-SA 3.0: https://creativecommons.org/licenses/by-nc-sa/3.0/
- CC0 1.0: https://publicdomain.net/zero/1.0/ · CC BY 0.0: https://creativecommons.org/publicdomain/zero/1.0/
- MIT: see [`LICENSE`](LICENSE)
