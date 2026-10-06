# Play-mode Smoke Test — MT-SMOKE-01

Roadmap: [P0.E2.T6](../roadmap/phase-00-foundation.md) · [Known Issues](../essential/known-issues-and-roadmap.md)

Manual test case **MT-SMOKE-01**: boot a clean checkout → `Level_00` →
move / dash / shoot / weapon-swap → pause → quit. This page is the running
record; every run appends a dated entry.

## Steps

| # | Step | Pass criteria |
|---|------|---------------|
| 1 | Launch on a clean checkout (pinned 6000.3.20f1) | 0 `error CS` / 0 `warning CS` in the compile |
| 2 | Open `Level_00`, press Play | No exceptions/errors in Console; `GUIHandler` overlay appears |
| 3 | Move (WASD / LStick) | Player moves; camera follows toward aim |
| 4 | Dash (Space / RB) while moving | Trails + after-images spawn; no NRE |
| 5 | Shoot: LMB hold + release (RMB charge variant too) | Projectiles spawn at both use rates; charge release fires |
| 6 | Weapon swap: `1`/`2` (gamepad X/Y), Q/E | Active weapon GameObject toggles (`WeaponHandler.weapons[i]`) |
| 7 | Pause (Esc / Start), resume | `Time.timeScale` 0 → back to 1; audio pauses |
| 8 | Quit | Clean exit; no exceptions in the exit log |

The gamepad pass + audio checks + wall-façade enter/exit observation
(P0.E3.T7) are part of the manual run.

## Automated subset — 2026-10-06 — GREEN

Headless batchmode execution of the same sequence by a temporary probe
(`Assets/Editor/QaPlaySmoke.cs` in the **fresh-clone workdir only** — never
committed). Technique: disable the scene `TadaInput` component, then write
`TadaInput`'s private statics (`currentKeys*`, `_MoveAxisRawInput`,
`_AimAxisRawInput`) so the game consumes simulated input.

- Environment: fresh `git clone` of `develop` @ `74b6552`, Unity **6000.3.20f1**,
  Windows, `BatchMode -nographics`, scene `Level_00`.
- Result: **GREEN — PASS=13, FAIL=0, WARN=1, INFO=6**, editor exit code **0**.

| Step | Result |
|---|---|
| boot: opened `Level_00` (roots=15) | PASS |
| presence: `TadaInput` / `PlayerController` / `PauseController` | PASS ×3 |
| presence: `WeaponHandler` with ≥2 weapons; `PlayerPhysics` + `PlayerSkills` | PASS ×2 |
| weapon slot 0 active at start | PASS |
| shoot: projectiles spawned (max simultaneous=2) | PASS |
| swap → `weapons[1]` active, `weapons[0]` inactive | PASS |
| swap back → `weapons[0]` active, `weapons[1]` inactive | PASS |
| move: player moved 2.52 u over 160 frames (dash included) | PASS |
| pause: `isGamePaused=True timeScale=0` | PASS |
| resume: `isGamePaused=False timeScale=1` | PASS |
| play exit → editor quit (`EditorApplication.Exit(0)`) | PASS |

**WARN ×1 (recorded, not failed):** `ArgumentOutOfRangeException` from
`UnityEditor.Search.SearchDatabase.EnumerateAll` (`SearchInit.IndexationOnStartup`) —
a Unity 6000.3.20f1 editor-internal exception that fires in batch when the search
index db is empty; the probe classifies it `editor-internal` by stack attribution,
so any game-code exception still fails the run. Game warnings during play: **0**
(the `Missing behaviour classes!` / `weapons missing` guard-warnings are treated
as failures by the probe).

## Checkout-hygiene evidence (P0.E1.T1 round-trip, same clone)

- Fresh clone → batchmode open: 0 `error CS` / 0 `warning CS`; no generated files tracked.
- After `.gitattributes` (LF pin, `74b6552`) + scene re-serialization (`d6455af`):
  **open → save → close both scenes leaves `git status` clean** — only untracked probe
  tooling, `git diff --stat` empty (2026-10-06).
- Re-serialization idempotent (save1 == save2): `Level_00` `32D319F21D36`,
  `SingleWall_Test` `3F414AE40883`; census identical before/after —
  Level_00 15 roots / 2518 GOs / 4210 components / 0 missing scripts,
  SingleWall 13 / 94 / 230 / 0.

## Known caveats

- **`PlayerMaterials` dirties a material asset during play** (verified this run):
  `Update()` writes `_ColorIntensity` on the shared `mat_PlayerBody.mat` asset
  (serialized field reference) during the dash highlight; the batch quit persisted the
  mid-animation value `1.5622337` — a *real content* diff (not EOL). Recovery:
  `git checkout -- Assets/Materials/Player/mat_PlayerBody.mat`; proper fix = runtime
  material instance / `MaterialPropertyBlock` (candidate defect: Known Issues §B.14).

## Pending manual coverage

- [ ] Gamepad pass (aim stick, X/Y slot swap, Start pause) — folded in from P0.E3.T4
- [ ] Audio audible (SFX on fire/dash, music autoplay)
- [ ] Wall-façade enter → exit visual restore (P0.E3.T7 observation)
- [ ] Interactive (GUI) quit pass on a dev machine

## Related

- [Known Issues & Roadmap](../essential/known-issues-and-roadmap.md) ·
  [Phase 0 roadmap](../roadmap/phase-00-foundation.md) ·
  [Scene Inventory](../essential/scenes.md)
