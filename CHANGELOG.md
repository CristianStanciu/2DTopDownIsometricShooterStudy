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
- `docs/qa/smoke.md`: MT-SMOKE-01 record — automated play-mode subset GREEN on a fresh
  clone (PASS=13/FAIL=0; boot→move→dash→shoot→weapon-swap→pause/resume→quit, exit 0) — P0.E2.T6
- CI pipeline `.github/workflows/ci.yml`: PR/push → `develop` runs game-ci batchmode
  (import · compile · EditMode tests, editor pinned 6000.3.20f1) with results artifact +
  check run, and a **warning gate** failing on any `warning CS|error CS` line — P0.E6.T1
- `docs/qa/warning-policy.md`: warning-zero policy + catalogue of every warning class on
  6000.3.20f1 (gated / accepted / classified) with suppression rules — P0.E2.T4
- Manual Windows build pipeline `.github/workflows/build.yml`: `workflow_dispatch`
  (optional `version` override) → game-ci `StandaloneWindows64` build (editor pinned
  6000.3.20f1, Library cache) → zipped artifact named `{version}-{sha}`
  (bundleVersion + short SHA, P0.E1.T6 scheme) — P0.E6.T3
- Deterministic package restore: `ci.yml` fails if `Packages/packages-lock.json` drifts
  during import (fresh-clone import verified byte-stable) — P0.E2.T7
- Onboarding quickstart `docs/essential/onboarding.md`: engine pin, CI secrets
  (`UNITY_LICENSE`/`UNITY_EMAIL`/`UNITY_PASSWORD`), branch rules, how tests run —
  new-contributor entry point linked from the docs index — P0.E9.T4
- Dead-code pass: `Assets/Scripts/_NOTES/JustNotes.cs` removed (0 GUID references, content
  archived to `docs/notes/author-notes.md`); grep report `docs/qa/dead-code-grep.md`
  (0 unjustified empty stubs, 1 live TODO) — P0.E4.T7
- E3 triage retro (P0.E3.T14): CHANGELOG audit — all 14 prior defect fixes referenced
  (T1–T13, T15); full MT-SMOKE-01 re-run GREEN @ `5fecd1b` (13/0/1, 0 tracked files
  modified post-run) — evidence in `docs/qa/smoke.md`
- Asset ledger + license drift gate (P0.E8.T1): `docs/legal/asset-ledger.md` — 11 audio +
  9 sprite rows with Unity GUIDs, origin URLs (README credits), commercial-use flags and
  attribution text; fonts = 0 proven; 64 `com.unity.*` packages (URP → shadergraph 17.3.0,
  zero third-party UPM deps); third-party code table for E8.T4. `scripts/get-asset-ledger.ps1`
  (`-Check` path/GUID drift gate) wired as CI job `asset-ledger`. **Findings: the NC music
  and all pixel art (upstream: "free for personal use") are commercial blockers — feeds
  E8.T2 brief scope (music + sprites) and P8.E3.T4 remediation**
- `.editorconfig` (P0.E5.T1): survey-based C# conventions for all 48 scripts — no `var`,
  no `this.`, Allman braces, 4-space indent, UTF-8-BOM, naming suggestions, IDE0052
  suppression kept; CI formatter enforcement deferred with P0.E5.T2
- Assembly definition layout (P0.E4.T1): 9 domain asmdefs (`Game.Core` default root assembly
  + `Game.Player`/`Game.Weapons`/`Game.Audio`/`Game.Camera`/`Game.Environment`/`Game.UI`/
  `Game.DebugTools`/`Game.EditorTools`) + `Tests.EditMode`/`Tests.PlayMode`; full cross-folder
  edge audit (every class→folder reference enumerated), 2 cycle cuts shipped as new Core
  statics (`SceneAnchors`, `FacingState`); verified in throwaway clone — import exit 0,
  0 `error CS`/0 `warning CS`, 8 `Game.*.dll` + `Tests.EditMode.dll`, graph acyclic
  (ADR-0001 amended as-built) — P0.E4.T1
- EditMode test suite (P0.E7.T1): `TadaInputTests` ×13 (consumption, edge clearing, axis
  press/hold/release, reset incl. axis memory, `NextUseRate` single down/up) +
  `ArraysHandlerTests` ×11 (wrap/roundtrip/full-cycle/empty-range) = **24 tests, batch run
  24/24 GREEN** in the fresh-clone verifier — satisfies the EditMode ≥10 exit criterion
  locally; `GameFlowTests`/`TuningRegistryTests` deferred to P0.E4.T5/T6, CI run pending push

### Changed

- Re-serialized `SingleWall_Test.unity` for 6000.3.20f1 (+800/−207 lines; census identical, 0 missing
  scripts; lighting settings extracted to `SingleWall_TestSettings.lighting`) — P0.E2.T1
- Re-serialized `Level_00.unity` for 6000.3.20f1 (+1512/−380 lines; census identical
  15 roots/2518 GOs/4210 components/0 missing scripts, save idempotent) — found by the
  P0.E1.T1 round-trip probe: `c9d5f0e` had only partially updated the scene — P0.E2.T1
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
- Dash highlight no longer mutates the shared material asset: `PlayerMaterials` applies
  `_ColorIntensity` through per-renderer `MaterialPropertyBlock`s bound in `Awake()`
  (play sessions leave `mat_PlayerBody.mat` byte-identical; MT-SMOKE-01 re-run GREEN
  13/0/1 + binding harness 17/17 renderers, asset 1→1) — P0.E3.T15 (Known Issues B.14)
- Static-facade lifecycle: `CameraShake`, `PauseController`, `CrosshairMouse/Joystick`,
  `PlayerBodyPartsHandler` reset their statics on subsystem registration
  (`[RuntimeInitializeOnLoadMethod]`, same lifecycle as `TadaInput` in P0.E3.T8) —
  pause/shake/aim/body-direction state can't leak across plays, even with domain reload
  disabled; verified by a 5× enter/exit cycle run (GREEN PASS=50/FAIL=0 —
  `docs/qa/static-reset.md`) — P0.E4.T4
- Cycle cuts for the asmdef split (P0.E4.T1): crosshair player lookups resolve through the
  new `SceneAnchors` registry in `Game.Core` (lazy re-resolve keeps E3.T9 null-guards) instead
  of `FindAnyObjectByType<PlayerController/PlayerShoulderMain>`; `Projectile` reads
  `FacingState.IsRightDirection` (Core) instead of `PlayerBodyPartsHandler.isRightDirection`,
  which now delegates to it — behavior identical, QA tripwires stay valid
- Crosshair NRE when `PlayerController` absent (mouse + joystick lookups null-guarded) — P0.E2.T3
  (defect P0.E3.T9 code part)
- 17 documentation defects from the QA review (details: `docs/review-report.md`)
- Fresh-clone phantom modifications: Unity writes serialized text as LF while
  `core.autocrlf=true` checkouts were CRLF, so the first editor save always showed `M`
  (reproduced on `ShaderGraphSettings.asset`, content byte-identical); `.gitattributes`
  pins `eol=lf` for 13 Unity-serialized text types — open→save→close now leaves
  `git status` clean — P0.E1.T1
