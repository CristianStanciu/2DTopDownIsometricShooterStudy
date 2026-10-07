# Replacement Brief — non-commercial assets (P0.E8.T2)

**Task:** P0.E8.T2 · **Created:** 2026-10-07 · **Source:** [asset-ledger](asset-ledger.md) findings
**Gate:** every replacement must land before paid marketing (P9) and RC — execution is tracked
as P8.E3.T4 (final GUID-diff audit: shipped assets ⊆ ledger with commercial-use = yes).

## 1. What must be replaced

| Asset | Current terms | Why blocked | Action |
|---|---|---|---|
| Music loop "Azimutez" | CC BY-NC-SA 3.0 | NC forbids commercial use; SA compounds it | Replace (§2) **or** written permission from the rightsholder |
| All 9 pixel-art sheets | upstream README: "Pixel art sprites are free for personal use" | Personal-use only; MIT explicitly excludes sprites | Replace (§3) **or** written permission from Tadadosi |

**Not in scope:** the two CC-BY-3.0 SFX (attribution only — kept and credited) and all
CC0/CC-BY-0 SFX (unrestricted). If permission is obtained instead of replacement, file the
receipt (P0.E8.T5) and update the ledger — `-Check` stays green either way.

## 2. Music replacement brief

**Current use:** single clip in `MusicHandler.musicClips[0]` — `playOnAwake`, looped by
`MusicEmitter` (loop=true), volume 0.65, both scenes. Swap = drop-in file + ledger row.

- **Style:** dark sci-fi industrial with a pulse — match the current reference
  ([Azimutez by Sci Fi Industries](https://freemusicarchive.org/music/Sci_Fi_Industries/Blame_the_Lord/01_sci_fi_industries_-_azimutez)):
  mid-tempo, mechanical, **no vocals**, safe to loop forever.
- **Length:** 90–120 s, seamless loop (sample-accurate; no fade or level jump on the seam).
- **Structure:** steady background bed — no distinctive intro that reads wrong on every repeat.
- **Delivery:** 48 kHz WAV master + OGG for the build; loop points baked in; DAW/project
  files retained for future edits.
- **Stems (optional, +cost):** drums / bass / lead / pads — only needed later for adaptive
  music (P4 dynamic music); request at commission time if commissioning.
- **License required:** commercial use in a paid game — work-for-hire with IP assigned to the
  project, **or** royalty-free/CC0/CC-BY license (CC-BY fine: credits screen exists).
- **Acceptance:** loops seamlessly via `MusicHandler` in both scenes; ≈ −16…−14 LUFS
  integrated, true peak ≤ −1 dBTP; sits under gameplay SFX (no mixer yet — P4.E1).

## 3. Sprite replacement brief

Measured inventory (replace 1:1 or restyle — dims/PPU are what the project consumes today):

| Sheet | Size (px) | PPU | Role |
|---|---|---|---|
| `char_main_skin_001.png` | 32×32 | 16 | Player skin (rig-driven `.anim` clips) |
| `Ground_Seamless_Tile_01.png` | 86×86 | 16 | Seamless floor tile |
| `Ground_Tiles.png` | 260×146 | 16 | Ground tile set |
| `Wall_Frontal_001_Color.png` / `_Glow.png` | 58×54 / 51×42 | 16 | Wall pair — Color feeds albedo, Glow feeds HDR shader-graph material |
| `Wall_Side_001_Color.png` / `_Glow.png` | 39×75 / 32×63 | 16 | Side-wall pair (same Color/Glow contract) |
| `Bullet_Laser.png` | 32×32 | 32 | Projectile sprite |
| `Weapons_001.png` | 128×128 | 16 | Weapon sprite sheet |

- **Style:** keep the neon isometric/top-down look; references = current sheets + the glow
  language in `docs/essential/rendering.md` (glow maps drive the HDR shaders).
- **Color/Glow contract:** wall replacements must ship paired glow maps; keep each file at its
  current pixel dimensions so sprite UVs and shader sampling survive the swap (verify walls in
  `Level_00` after import).
- **Animation contract:** the `.anim` clips under `Assets/Sprites/Animations/` bind the rig to
  these sheets — keep PPU (16; bullet 32) and slicing so clip bindings survive; re-verify
  player idle/walk + weapon BasicShot/ChargedShot/Charging anims after the swap.
- **Delivery:** PNG per sheet at matched dimensions + editable source (Aseprite/Libresprite).
- **License required:** full commercial use with ownership transferred/assignable, **or CC0**.

## 4. Options & budget (indicative — re-verify at commission time)

| Route | What | Cost | Pros | Risks |
|---|---|---|---|---|
| **A. CC0 / royalty-free libraries** | Music: [FreePD.com](https://freepd.com) (CC0), [Incompetech](https://incompetech.com) (CC-BY 4.0). Art: [Kenney](https://kenney.nl) (CC0), OpenGameArt CC0 filter, itch.io CC0 packs, Unity Asset Store packs | $0–$50 | Fastest, zero legal friction, receipts trivial | Style mismatch; less "own" look |
| **B. Commission** | Music at entry-tier **$50–150/min** of finished music (simple electronic $50–200/min; mid-tier $150–300/min — [Twine pricing guide, Sep 2025](https://www.twine.net/blog/game-composer-pricing/)). Pixel art: character from **$75**, animation cycle from **$120**, full sheet/scene from **$350** ([Snorpixel](https://snorpixel.com/pixel-art-commissions/)); hourly reference ≈ **$50/h** ([Brillay](https://brillay.com/resources/pixel-art-sprite-sheet-commission-pricing)) | Music ≈ **$100–400** (2 min loop). Sprites ≈ **$350–700** for the 9-sheet set + **25–50% commercial-use uplift** (negotiated separately — Snorpixel/Brillay both quote commercial on top) · **total ≈ $500–1,200** | Bespoke, matches art direction, ownable | 1–3 wk lead time + revision rounds; needs contract (§5); rush surcharges (+25–50%) — avoid |
| **C. Hybrid (recommended)** | CC0 music now; commission only the art (art is the visible, hard-to-source IP risk; music is easy to source permissively) | ≈ **$350–800** | Spends budget where it matters; music ships immediately | Two parallel workflows |

**Schedule:** pick route at the start of P4 (audio) / P3 (UI) at the latest; everything must
replace the flagged files **before P9 paid advertising** (fix schedule requirement of this task).

## 5. Contract & process requirements (feeds P0.E8.T5)

1. Commissioned work needs a **written IP assignment** (US: 17 U.S.C. §204(a) — transfers must
   be signed writings) or an explicit **commercial-license grant**; personal-use-only default
   terms are exactly the trap the current sprites are in.
2. Store the receipt/license proof in `docs/legal/receipts/` (folder lands with E8.T5).
3. Update [asset-ledger](asset-ledger.md) (origin, license, commercial columns) →
   `scripts/get-asset-ledger.ps1 -Check` must stay green.
4. Update the in-game credits screen (P3.E3.T7) if the new license requires attribution.
5. Final gate: P8.E3.T4 GUID-diff audit — shipped ⊆ ledger with commercial-use = yes.

## 6. Acceptance checklist

- [ ] Music: replacement clip loops seamlessly in both scenes (manual listen) — ledger row updated
- [ ] Sprites: 9 sheets swapped (or permissions received) — player rig, walls (Color+Glow),
      ground, projectile and weapon rendering verified in `Level_00`
- [ ] Receipts filed; THIRD-PARTY/credits updated (P0.E8.T4 / P3.E3.T7)
- [ ] `get-asset-ledger.ps1 -Check` green; P8.E3.T4 audit passes

