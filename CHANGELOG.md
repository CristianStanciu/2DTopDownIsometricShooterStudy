# Changelog

All notable changes to this project are documented in this file.
Format based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).
Every merged PR adds an entry under `[Unreleased]` (rule: `CONTRIBUTING.md`).

## Versioning (P0.E1.T6)

- `0.x` — internal development; minor bump per phase milestone.
- `0.9` — release candidate; `1.0` — first release.
- `PlayerSettings.bundleVersion` is the source of truth (currently `0.1`, within `0.x` spec).
- CI build artifacts are named `{version}-{sha}` (wired in P0.E6.T3 `build.yml`).

## [Unreleased]

### Added

- Full documentation suite: `docs/essential/` (15 pages) + `docs/roadmap/` (12 pages),
  QA review report with 17 findings (all fixed) — roadmap **APPROVED v1.2** (`docs/review-report.md`)
- ADR process and starter decisions ADR-0001…0003 (`docs/adr/`) — P0.E9.T3
- Git LFS evaluation: not adopted, revisit triggers recorded (ADR-0004) — P0.E1.T2
- Contribution flow: PR template, CODEOWNERS, `CONTRIBUTING.md` — P0.E1.T3
- This changelog (Keep a Changelog) — P0.E1.T5

### Changed

- Re-serialized `SingleWall_Test.unity` for 6000.3.20f1 (+800/−207 lines; census identical, 0 missing
  scripts; lighting settings extracted to `SingleWall_TestSettings.lighting`) — P0.E2.T1
- Migrated 13 obsolete `FindObjectOfType` call sites (9 scripts) to `FindFirstObjectByType` —
  zero CS0618 warnings on compile — P0.E2.T2
- Root `README.md` refreshed: engine pin 6000.3.20f1 + upgrade story, documentation links,
  controls table, code sample updated — P0.E2.T5
- Project upgraded from Unity 2019.4.0f1 to **6000.3.20f1** (commit `c9d5f0e`), URP 17.3.0
- `.gitignore` hardened: `UserSettings/` (8 files) and `my-game.slnx` untracked, `*.slnx` ignored —
  P0.E1.T1
- `getting-started.md`: editor upgrade policy (dedicated PR + full regression) — P0.E1.T4

### Removed

- Dead class `Weapon_ChargeContinuousShooting` (WIP, empty `OnChargeShooting()`, zero scene/prefab
  references) — decision: delete rather than implement; continuous beams get designed in P1 — P0.E3.T6

### Fixed

- `WeaponHandler.CheckWeaponsAvailability()` returns a result and every caller aborts on failure
  (empty/missing weapon lists can no longer NRE) — P0.E3.T1
- `PlayerSkills.SetActiveTrails()` null-checks trail elements (array guarded once, before the loop)
  — P0.E3.T2
- `SoundManager` SFX methods guard the SFX emitter (`sfx`/`unstoppableSfx`), not `music` — P0.E3.T3
- `PlayerController` weapon slots read `TadaInput.ThisKey.WeaponSlot1/2` instead of raw
  `Input.GetKeyDown(KeyCode.Alpha1/Alpha2)` → slot switching works on gamepad (X/Y buttons);
  overlay gamepad table and `getting-started.md` show the new bindings — P0.E3.T4
- Charge weapon: `chargeReuseTimer` cooldown started in `OnChargingEnd()` stops the weak charging
  shake from overriding the release shake (author's NOTE fixed); window serialized per weapon
  (`chargeReuseCooldown = 0.25s`), moves to the tuning SO with P0.E4.T6 — P0.E3.T5
- `TadaInput`: duplicate `KeyUp(NextUseRate)` block removed; `MouseAnyMovement` now stored (axis
  semantics); statics reset every play start via `[RuntimeInitializeOnLoadMethod]` so `GetKeyDown`
  state can't leak across sessions — P0.E3.T8
- Wall façades restore when the player exits an environment trigger (`OnTriggerExit2D` mirrors enter
  logic via `EnvironmentObjectTrigger.RestoreObjects()` / `EnvironmentObject.RestoreStartSet()`) — P0.E3.T7
- Camera follow smoothing uses `Time.fixedDeltaTime` → frame-rate-independent convergence — P0.E3.T10
- Debug overlay hints generated from binding tables; `2: Weapon2` corrected to `2: Weapon1` (key 2
  selects weapon index 1) — P0.E3.T11
- Leftover empty-path scene entry removed from `EditorBuildSettings.asset` — P0.E3.T12
- `EnvironmentObjectTrigger.SetActiveObjects()` null-guards the arrays themselves — P0.E3.T13
- Crosshair NRE when `PlayerController` absent (mouse + joystick lookups null-guarded) — P0.E2.T3
  (defect P0.E3.T9 code part)
- 17 documentation defects from the QA review (details: `docs/review-report.md`)
