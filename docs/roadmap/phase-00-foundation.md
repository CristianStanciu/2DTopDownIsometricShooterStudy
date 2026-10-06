# Phase 0 — Engineering Foundation & Hygiene

**Goal:** turn the upgraded study project into a maintainable, CI-verified, warning-free codebase with legal clarity — the substrate every later phase depends on.
**Depends on:** nothing (start immediately after roadmap approval).
**Effort:** ~14–18 person-weeks · **Milestone:** M0 · **Risk:** low/known

## Exit criteria
- [ ] All Phase 0 tasks `[x]`; `develop` builds green on CI with zero warnings
- [ ] Asset license ledger exists; every non-commercial asset flagged with remediation plan
- [ ] Both scenes re-serialized on 6000.3.20f1; smoke test passes on a clean machine
- [ ] Test projects running in CI (EditMode ≥10 tests); backlog imported to tracker with these IDs

---

## Epic P0.E1 — Repository & environment hygiene (~2 pw)

- [x] **P0.E1.T1** — Fix `.gitignore` for Unity: stop tracking `UserSettings/`, generated `*.sln`, `*.csproj`, `*.slnx` (keep `.vsconfig`); add `Logs/`, `MemoryCaptures/`, `.vs/`; `git rm --cached` the tracked ones
  - [x] `git status` stays clean after open → edit → close editor round-trip *(fresh-clone batchmode open→save→close of both scenes: `git diff --stat` empty, only untracked probe tooling — 2026-10-06; evidence in `docs/qa/smoke.md`)*
  - [x] Fresh clone + open in pinned editor produces no unexpected diffs (only `Library/`, which is ignored) *(fresh clone @`74b6552`: import 0 `error CS` / 0 `warning CS`; no generated files tracked; `.gitignore` complete)*
  - *Round-trip finding (2026-10-06): `core.autocrlf=true` checkouts were CRLF while Unity always writes LF → the first editor save produced a phantom `M` (reproduced on `ProjectSettings/ShaderGraphSettings.asset`: index 576 vs disk 557 bytes, content byte-identical). Fixed by `.gitattributes` pinning `eol=lf` for 13 Unity-serialized text types (`74b6552`); the probe also discovered `Level_00.unity` was never written in 6000.3.20f1 format — re-saved and committed as `d6455af`.*
- [x] **P0.E1.T2** — Evaluate Git LFS for `Assets/Sprites/**` and `Assets/Audio/**`; if repo >500 MB, migrate binaries (history rewrite optional; decision recorded in an ADR)
- [~] **P0.E1.T3** — Branch/PR process: `main` protected, `develop` integration branch, feature branches `feat/P1.E4.T2-*`; PR template (summary, refs IDs, test evidence, doc updates); CODEOWNERS
  - [ ] Branch protection requires 1 green CI run before merge
  - *Files shipped (PR template, CODEOWNERS, CONTRIBUTING); branch protection pending `gh auth login` (token invalid) + P0.E6.T1 CI.*
- [x] **P0.E1.T4** — Pin editor version org-wide: document `6000.3.20f1` in `getting-started.md`; rule: upgrades only via dedicated PR with full regression run
- [x] **P0.E1.T5** — Add `CHANGELOG.md` (Keep a Changelog format); every merged PR adds an entry; this becomes launch patch notes
- [~] **P0.E1.T6** — Versioning scheme: `0.x` internal, `0.9` RC, `1.0` release; wire to `PlayerSettings.bundleVersion` (currently `0.1`) and CI build naming
  - *Scheme documented in `CHANGELOG.md`; `bundleVersion 0.1` conforms; CI artifact naming wired in P0.E6.T3 (`build.yml`).*

## Epic P0.E2 — Unity 6.3 hardening (~3 pw)

- [x] **P0.E2.T1** — Re-serialize `SingleWall_Test.unity` in 6000.3.20f1 (upgrade commit `c9d5f0e` never touched it) and commit
  - [x] Script-component census after save matches `docs/essential/scenes.md` (no missing scripts / lost `m_Script` GUIDs) *(batchmode open→save→reopen census: 13 roots / 94 GOs / 230 components / 51 types identical before vs after, 0 missing scripts; diff +800/−207 lines)*
- [x] **P0.E2.T2** — Replace all 13 obsolete `FindObjectOfType` call sites (in 9 scripts) with `FindFirstObjectByType`/`FindAnyObjectByType`: `SoundManager.cs:37,43,49`, `CrosshairHandler.cs:26,27`, `CrosshairJoystick.cs:55,56`, `CrosshairMouse.cs:36`, `AfterImageGenerator.cs:36`, `AfterImageGroup.cs:14`, `PlayerBodyPartsHandler.cs:50`, `PlayerSkills.cs:20`, `Singleton.cs:33`; also drop the stale comment mention in `JustNotes.cs:14`
  - [x] Zero CS0618/obsolete warnings in console & CI log *(batchmode compile 2026-10-05: 0 warning/0 error/0 CS0618; re-check in CI at P0.E6.T1)*
- [x] **P0.E2.T3** — Null-guard unguarded lookups (`CrosshairMouse`/`CrosshairJoystick` → `FindAnyObjectByType<PlayerController>()?.transform`) — closes defect P0.E3.T9
- [x] **P0.E2.T4** — **Warning-zero policy**: catalogue every warning class; fix or suppress-with-reason; CI fails on new `Warning CS|error CS` lines *(policy + 7-class catalogue: `docs/qa/warning-policy.md` — 0 `warning CS`/`error CS` across all batchmode runs of 6000.3.20f1, CS0618 eliminated in E2.T2, Unity `/nowarn:0169,0649,0282,1701,1702` scaffold accepted with reasons, 0 game warnings + 1 editor-internal WARN classified in MT-SMOKE-01; fail-gate implemented as the "Warning gate" step of `ci.yml` — first enforced run pending repo secrets, see P0.E6.T1)*
- [x] **P0.E2.T5** — Refresh `README.md`: correct engine `6000.3.20f1`, upgrade story (2019.4 → 6000.3), link to `docs/essential/`, controls table
- [~] **P0.E2.T6** — **Play-mode validation** on a clean checkout: run manual case `MT-SMOKE-01` (boot → Level_00 → move/dash/shoot/weapon-swap → pause → quit); record in `docs/qa/smoke.md` *(automated subset GREEN 2026-10-06 on fresh clone @`74b6552`: PASS=13 / FAIL=0 / WARN=1 editor-internal, exit 0 — full record in `docs/qa/smoke.md`; pending manual: gamepad, audio, wall-façade observation, interactive quit)*
- [~] **P0.E2.T7** — Confirm deterministic package restore from committed `packages-lock.json`; CI uses locked mode
  - *Lock committed + LF-pinned; a full fresh-clone import (B.14 smoke runs, 2026-10-06) left it byte-identical (blob `7d10942`); `ci.yml` gained a restore gate failing on any `packages-lock.json` drift. Pending: first CI run (blocked with `gh auth login`).*

## Epic P0.E3 — Defect burn-down (14 verified defects, ~3 pw)

*Source: `docs/essential/known-issues-and-roadmap.md` §B. One task per defect + regression proof. (B.14 was appended to §B after it was found by P0.E2.T6 → task T15.)*

- [~] **P0.E3.T1** — `WeaponHandler.CheckWeaponsAvailability()`: make guards actually protect callers (null-safe early abort + callers check result) so `weapons[i]`/`currentWeapon` can never NRE *(code done: returns bool, all 3 callers abort on false; batchmode compile 0 warn/0 error — `724250d`; test pending P0.E7)*
  - [ ] EditMode test: handler with empty/destroyed weapon list doesn't throw; callers skip fire *(blocked on P0.E7 test asmdefs)*
- [x] **P0.E3.T2** — `PlayerSkills.SetActiveTrails()`: null-check `dashTrails[i]`, not the array *(array guarded once before the loop; commit `9aaee2a`, compile-verified)*
- [~] **P0.E3.T3** — `SoundManager.SFX_PlayOneShot/SFX_PlayDelayedOneShot`: guard on `sfx != null` (currently checks `music`) *(code done: guards the resolved emitter (`sfx`/`unstoppableSfx`); commit `cc51c5d`; test pending P0.E7)*
  - [ ] Test: music-only SoundManager doesn't break SFX calls *(blocked on P0.E7 test asmdefs)*
- [x] **P0.E3.T4** — `PlayerController`: replace direct `Input.GetKeyDown(KeyCode.Alpha1/Alpha2)` with `TadaInput` so slot switching works on gamepad and honours the abstraction *(new `ThisKey.WeaponSlot1/2` = `Alpha1/Alpha2` + `Joystick1Button2/3` (X/Y); `PlayerController` reads via `TadaInput.GetKeyDown`; overlay gamepad table + `getting-started.md` updated; gamepad check folds into MT-SMOKE-01)*
- [~] **P0.E3.T5** — `Weapon_ShootProjectileCanCharge`: implement charge-shake cooldown (author NOTE); window configurable in tuning SO (P0.E4.T6) *(code done: `chargeReuseTimer` gates the charging start after `OnChargingEnd`; window = serialized `chargeReuseCooldown = 0.25s` per weapon; moves into the GameTuning SO when P0.E4.T6 lands)*
- [x] **P0.E3.T6** — Remove dead class `Weapon_ChargeContinuousShooting` (empty `OnChargeShooting()`) or implement it; record decision *(decision: remove — zero scene/prefab/asset GUID refs; −191 lines; recorded in CHANGELOG + `weapons-and-projectiles.md`; commit `fdc6c90`)*
- [~] **P0.E3.T7** — `PlayerPhysics.OnTriggerExit2D()`: restore wall façades on exit (mirror enter logic) *(code done: exit mirrors enter via `RestoreObjects()`/`RestoreStartSet()`; enter also null-guards `TryGetComponent`; commit `acf3aab`; test pending P0.E7)*
  - [ ] Test: enter → exit sequence restores sprite colors without requiring a second enter *(blocked on P0.E7 PlayMode infra; manual observation folds into P0.E2.T6 smoke)*
- [~] **P0.E3.T8** — `TadaInput`: remove duplicated `StoreCurrentKeyUp(NextUseRate)` block; reset statics on play-stop; store `MouseAnyMovement`; EditMode tests for consumption + `NextUseRate` timing *(code done: duplicate `V` block removed; `ResetStaticState()` via `[RuntimeInitializeOnLoadMethod(SubsystemRegistration)]` clears all statics + key arrays each play start; `MouseAnyMovement` stored with axis semantics; tests pending P0.E7)*
  - [ ] EditMode tests for consumption + `NextUseRate` timing *(blocked on P0.E7 test asmdefs)*
- [~] **P0.E3.T9** — Close crosshair NRE risk (same code as P0.E2.T3) *(code done in P0.E2.T3 — `?.transform` null-conditionals verified at `CrosshairMouse.cs:36` / `CrosshairJoystick.cs:55-56`; test pending P0.E7)*
  - [ ] Test: crosshair ticks without player in scene, logs warning instead of throwing *(blocked on P0.E7)*
- [x] **P0.E3.T10** — `CameraBehaviour.FixedUpdate`: use `Time.fixedDeltaTime` for smoothing *(commit `e9f1ef2`)*
  - [x] Behaviour converges identically in 30/60/144 fps checks (sim or documented A/B) *(documented A/B: old factor per FixedUpdate step = frame `deltaTime`×FollowSpeed → 0.066/0.033/0.014 at 30/60/144 fps; new = constant `fixedDeltaTime`×FollowSpeed = 0.06/step with fixed 50 steps/s → frame-rate independent)*
- [x] **P0.E3.T11** — `GUIHandler` overlay: fix `2: Weapon2` mapping text; generate text from binding table so it can't drift (input for P3 localization) *(labels now rendered from `MouseBindings`/`GamepadBindings` tables via `FormatBindings`; `2: Weapon1` corrects Alpha2→index 1; commit `532cc11`)*
- [x] **P0.E3.T12** — Remove leftover empty-path entry from `EditorBuildSettings.asset` *(disabled entry + zero GUID dropped; Level_00 stays the only scene; commit `aabde53`)*
- [x] **P0.E3.T13** — `EnvironmentObjectTrigger.SetActiveObjects()`: null-guard the arrays themselves *(each of the 4 loops wrapped in its array guard; committed with T7 in `acf3aab` — shared file kept one commit so every tree stays compilable)*
- [ ] **P0.E3.T14** — Triage retro: every fix referenced in CHANGELOG; full smoke `MT-SMOKE-01` re-run green
- [x] **P0.E3.T15** — Known Issues **B.14** — `PlayerMaterials`: dash highlight wrote `_ColorIntensity` directly on the shared `mat_PlayerBody.mat` **asset** (play sessions dirtied the asset; found by P0.E2.T6). Rewrite to per-renderer `MaterialPropertyBlock`s bound in `Awake()` (`FindObjectsByType<Renderer>` matching `bodyMaterial`, inactive parts included) — no scene rewiring, public API unchanged *(MT-SMOKE-01 re-run GREEN PASS=13/FAIL=0/WARN=1 with dash at frame 50; post-run `git status` byte-clean on the mat; binding harness 17/17 renderers + asset `_ColorIntensity` 1→1; 0 CS diagnostics; fresh clone @`b36b61e`+fix — 2026-10-06)*

## Epic P0.E4 — Architecture refactor (~5 pw) — *unblocks P1/P2/P3*

- [ ] **P0.E4.T1** — Introduce `.asmdef` set: `Game.Core` (utilities/global), `Game.Player`, `Game.Weapons`, `Game.Audio`, `Game.Camera`, `Game.Environment`, `Game.UI`, `Game.EditorTools`, `Tests.EditMode`, `Tests.PlayMode`; single assembly per folder family, no cycles
  - [ ] CI compile green; total domain reload time measured before/after (expect improvement)
- [ ] **P0.E4.T2** — Extract duplicated scene rig into shared prefabs: `PlayerRig`, `CameraRig`, `AudioRig`, `CrosshairRig` (both scenes carry identical copies — `docs/essential/scenes.md`); replace scene copies with prefab instances preserving all overrides
  - [ ] Instance override diff reviewed (no wiring lost); both scenes pass smoke
- [ ] **P0.E4.T3** — Scene context bootstrap: `SceneContext` component holding explicit refs (player, camera, sound manager, crosshair, spawner); consumers get refs via context instead of startup `Find*` calls
  - [ ] Test: context missing → fail-fast with clear error, not NRE deep in consumer
- [ ] **P0.E4.T4** — Static-facade lifecycle: `IResettableStatics` pattern via `[RuntimeInitializeOnLoadMethod]` resetting `TadaInput`, `CameraShake`, `PauseController`, `CrosshairMouse/Joystick.AimDirection`, `PlayerBodyPartsHandler.isRightDirection` on subsystem registration
  - [ ] Test: enter/exit play mode 5× leaves no stale `GetKeyDown` state
- [ ] **P0.E4.T5** — Game flow manager (states: Boot → Menu → Briefing → Gameplay → Paused → Results → Quit): pure state machine + Unity events; `PauseController` becomes one consumer; minimal UI switch (can be keyboard-driven until P3)
  - [ ] Transition matrix test: every illegal transition rejected; pause events fire exactly once
- [ ] **P0.E4.T6** — Tuning/ScriptableObject registry: `GameTuning` root SO with sub-assets (player, weapons, enemies, economy, camera, juice); replace magic numbers in shipped code paths incrementally (each moved number = subtask noted in PR)
  - [ ] Registry loads in editor + runtime; unknown field policy documented (fail soft with warning)
- [ ] **P0.E4.T7** — Dead-code pass: `_NOTES/JustNotes.cs` removed or moved to docs; empty stubs (`PlayerPhysics.OnTriggerExit2D` handled in P0.E3.T7) accounted for; `grep` report attached to PR

## Epic P0.E5 — Tooling & code quality (~2 pw)

- [ ] **P0.E5.T1** — `.editorconfig` (Unity C# conventions: explicit types policy, file headers with ID refs, `this.` style per existing code) + formatter configured in CI
- [ ] **P0.E5.T2** — Roslyn/Unity analyzers enabled; warnings-as-errors in CI (local dev keeps soft warnings); baseline suppressions documented with reasons
- [ ] **P0.E5.T3** — Structured logging shim: `Log.Info/Warn/Error(tag, msg, ctx)` wrapping `Debug.Log`, stripped/level-filtered in release builds; migrate the noisiest scripts first
- [ ] **P0.E5.T4** — Scene validation editor test: both scenes load, zero missing scripts, `SceneContext` present, no duplicate singletons — runs in CI (extends census approach from `docs/essential/scenes.md`)

## Epic P0.E6 — CI/CD pipeline (~2 pw)

- [~] **P0.E6.T1** — GitHub Actions workflow `ci.yml` on PR→`develop`: checkout (+LFS), game-ci Unity activation (license secret), import, compile, EditMode tests → JUnit results artifact; fails on warnings (policy from P0.E2.T4)
  - *Workflow written & YAML-validated 2026-10-06: checkout(LFS) → `game-ci/unity-test-runner@v4` pinned `6000.3.20f1` EditMode (license via `UNITY_LICENSE` secret; docker image `ubuntu-6000.3.20f1-*` confirmed on Docker Hub) → "Warning gate" step (E2.T4) → results artifact + check run. Pending: push + secret setup + first green run (blocked with `gh auth login`). Note: zero test assemblies until P0.E7.T1 — first run must confirm editmode-with-no-tests behaviour.*
- [ ] **P0.E6.T2** — Nightly `nightly.yml`: PlayMode smoke (headless): load `Level_00`, simulate 5 s input, assert no exceptions; results emailed/Slack
- [~] **P0.E6.T3** — `build.yml` (manual dispatch): Windows standalone build → zipped artifact named `{version}-{sha}`; later reused by P8/P9 for RC and depot uploads
  - *Workflow written & YAML/bash-validated 2026-10-06: `workflow_dispatch` (optional `version` override) → label `{bundleVersion}-{sha7}` read from `ProjectSettings.asset` → `game-ci/unity-builder@v4` `StandaloneWindows64` on `ubuntu-latest` (same license secrets as `ci.yml`; `buildName` = label; `Library` cache) → zip of `build/StandaloneWindows64/` → artifact `{version}-{sha}`. Pending: push + first manual run (blocked with `gh auth login`) incl. confirming builder log plumbing.*
- [ ] **P0.E6.T4** — Branch protection: require `ci.yml` green + 1 review (or self-review checklist for solo) ; add `CONTRIBUTING.md` short guide

## Epic P0.E7 — Test foundation (~2 pw)

- [ ] **P0.E7.T1** — Test asmdefs (P0.E4.T1) + fixtures: `TadaInputTests` (consumption, rate limits, reset), `ArraysHandlerTests`, `GameFlowTests` (P0.E4.T5 matrix), `TuningRegistryTests`
- [ ] **P0.E7.T2** — PlayMode combat smoke: player spawns, fire spawns projectile, projectile despawns at bounds (uses existing prefabs)
- [ ] **P0.E7.T3** — Coverage baseline report (Unity coverage or coverlet-style tooling); store report in CI artifact; policy: no phase may *reduce* critical-path coverage
- [ ] **P0.E7.T4** — Test data conventions: synthetic save fixtures folder (v1 files for P2.E4 migrator tests)

## Epic P0.E8 — Asset & license provenance audit (~2 pw) — *critical path for monetization (R1)*

- [ ] **P0.E8.T1** — Build `docs/legal/asset-ledger.md`: every file under `Assets/Sprites`, `Assets/Audio`, fonts, shader-graph packages — origin URL, license, commercial-use flag, attribution text; script the GUID scan to keep it current
- [ ] **P0.E8.T2** — Flag all CC BY-NC-SA / non-commercial audio (known: multiple tracks per `docs/essential/audio.md`) → replacement brief (style, length, stems) + budget + contractor/marketplace options; **Blocked by:** none, but fix schedule must precede paid marketing (P9)
- [ ] **P0.E8.T3** — Name check: trademark search + Steam/domain/social handle availability for chosen title; record in ledger
- [ ] **P0.E8.T4** — `THIRD-PARTY.md` generated from ledger; confirm root `LICENSE` (MIT) scope = code only; add in-game credits scene requirement (P3)
- [ ] **P0.E8.T5** — Contract templates: contractor IP-assignment + asset-licence receipts folder (`docs/legal/receipts/`)

## Epic P0.E9 — Documentation & process (≈1 pw)

- [ ] **P0.E9.T1** — Keep `docs/essential/` current: any scene/prefab/script change updates the matching page (DoD rule from §5)
- [ ] **P0.E9.T2** — Import this roadmap into the tracker: epics = Jira epics, tasks = stories, IDs preserved in custom field; acceptance criteria pasted into description
- [x] **P0.E9.T3** — ADR process: `docs/adr/NNNN-title.md` template (context/decision/consequences); bootstrap with ADR-0001 asmdefs, ADR-0002 logging, ADR-0003 test policy
- [x] **P0.E9.T4** — Onboarding doc: engine pin, CI secrets, branch rules, how to run tests — one page for a new contributor *(new `docs/essential/onboarding.md`: 6-section checklist — prerequisites · clone/first-open · CI secrets table (`UNITY_LICENSE`/`EMAIL`/`PASSWORD` + activation link) · branch rules (with pending-protection status) · how tests run on CI + Test Runner (E7.T1 status) · pre-push DoD; linked from the docs index TOC + "How to use" section; local-link check 14/14 pass — 2026-10-06)*

**Phase 0 DoD:** all boxes above `[x]`, exit criteria satisfied, M0 announced.

---

**Related:** [README](README.md) · [Phase 1](phase-01-core-combat.md) · [Known issues](../essential/known-issues-and-roadmap.md)


