# Phase 6 — Technical Quality & Performance

**Goal:** ship-grade engineering: fast, stable, configurable — pooling, budgets, input modernization, robust settings/save, logging/telemetry hooks, platform matrix.
**Depends on:** P0 (CI, asmdefs, statics reset) · **DG-3** · P1/P6 object lifecycles.
**Effort:** ~10–14 person-weeks · **Milestone:** M3 (perf gate), rolling otherwise

## Exit criteria
- [ ] Perf budgets met on min-spec + mid-spec (README §5) for 3 worst-case combat scenarios; zero steady-state GC spikes
- [ ] DG-3 executed with zero consumer-code changes (façade preserved); rebinding works (P3.E5.T2)
- [ ] Settings + save services hardened (fuzz green); release build has logging gated, telemetry opt-in, no editor-only code

---

## Epic P6.E1 — Object pooling & allocation control (~3 pw)

- [ ] **P6.E1.T1** — Generic `Pool<T>` service (prefab-based, prewarm counts from tuning SO, expandable with warning thresholds); scene bootstrap warms pools (projectiles, VFX from P5, damage numbers from P3)
  - [ ] Test: 1000 spawn/despawn cycles → zero allocations after warmup (profiler assertion in PlayMode or manual profiler capture)
- [ ] **P6.E1.T2** — Migrate existing allocation offenders: projectiles, after-images (`AfterImageGenerator/Group` — code comments already anticipate pooling), `GUIHandler`/HUD string building (P3: cached labels, no per-frame `ToString` unless changed)
- [ ] **P6.E1.T3** — Enemy spawn/despawn via pools where viable (P1.E3.T3); corpse cleanup aligned; per-frame LINQ/boxing audit pass (CI: `grep`-style check for known sins list, documented)

## Epic P6.E2 — Performance budgets & profiling (~3 pw)

- [ ] **P6.E2.T1** — Benchmark harness scene: 3 scenarios (max enemies + full VFX; 500 projectiles; boss + music layers) with automated frame-time/CPU/GPU stats output to CSV; run on min-spec target GPU + mid-spec
  - [ ] Budgets enforced: 16.6 ms avg, no frame >50 ms in 60 s run (documented tolerances), draw calls ≤300, memory stable ±5%
- [ ] **P6.E2.T2** — URP optimization pass: SRP batcher on, overdraw reduction (VFX sizing), renderer features audited, quality presets mapped (P3.E4.T2); capture before/after profiler snapshots
- [ ] **P6.E2.T3** — Startup & load: async scene loads (`SceneManager` + loading screen contract for P3), first-input-to-response latency measured (pillar 1: ≤1 frame logic delay), domain-reload/init time acceptable
- [ ] **P6.E2.T4** — Long-session stability: 8-hour soak (P7 runs it) with memory graph — no leak (pools checked, emitters, scene reload cycles ×50)

## Epic P6.E3 — Input modernization (DG-3, ~3 pw)

- [ ] **P6.E3.T1** — **DG-3 decision**: if migrating, install `com.unity.inputsystem`, implement `TadaInput` façade internals on new backend; consumer call sites **unchanged** (verify by diff) — legacy `Input` usages inside façade only
  - [ ] Existing `TadaInput` EditMode tests (P0.E7.T1) run against new backend unmodified (adapt only fake-device fixtures)
- [ ] **P6.E3.T2** — Action map: move/aim/fire/dash/switch/ability/pause/menu — gameplay vs UI contexts; input action asset checked in; focus-loss state reset (replaces statics reset reliance)
- [ ] **P6.E3.T3** — Device detection & glyph layer feeding P3.E5.T1; hot-plug switch mid-game tested (unplug pad → keyboard glyphs, no stuck keys)
- [ ] **P6.E3.T4** — Rebinding storage (P3.E5.T2) via new backend's `InputActionMap` save/restore JSON; migration of any user presets (n/a at 1.0 — documented)

## Epic P6.E4 — Settings, save robustness & build config (~2 pw)

- [ ] **P6.E4.T1** — Central `SettingsService` (P3.E4.T1) finalized: versioned schema, `ISaveStorage` backends (local; Steam via P8), crash-safe writes (reuse P2.E4 machinery), settings vs profile separation, cloud conflict policy (keep newest)
- [ ] **P6.E4.T2** — Release build hygiene script: strips `DEVELOPMENT_BUILD`/editor-only components (P3.E1.T4), verifies `bundleVersion`, symbols (`RELEASE`), IL2CPP? (decision: Mono for speed vs IL2CPP for perf/protection — ADR; default IL2CPP if build time allows), Managed stripping level test
- [ ] **P6.E4.T3** — Logging & error policy: release builds log to file (rotating 5 MB) + Unity `LogException` hook captured to `crash-*.txt` in persistentDataPath (support artifact); user-facing fatal-error dialog with log path
- [ ] **P6.E4.T4** — Opt-in telemetry client (events: boot, session_end, level_result, crash, settings_change — **no PII**), consent gate + settings toggle, offline queue; legal text reviewed in P8
  - [ ] Privacy review checklist: events documented in `docs/legal/telemetry-events.md`, default OFF
- [ ] **P6.E4.T5** — Windows code signing: obtain a code-signing certificate; `signtool` step in `build.yml` signs the shipped exe; SmartScreen/AV behavior verified on a clean Windows 10/11 machine (ADR if deliberately deferred — README §2 launch config requires "build signed")

## Epic P6.E5 — Platform matrix (~2 pw)

- [ ] **P6.E5.T1** — Resolution/mode matrix: 1280×720 → 3840×2160, 16:9/16:10/21:9/32:9; borderless/fullscreen/windowed; DPI scaling on Windows — screenshot evidence per mode (uses P3.E2.T6 safe zones)
- [ ] **P6.E5.T2** — High-refresh support: uncapped + 120/144 Hz verified (camera and `CameraBehaviour` smoothing audited at high fps — regression vs P0.E3.T10)
- [ ] **P6.E5.T3** — Steam Deck target pass: 800p UI scale, 40/60 fps profiles, TDP-friendly preset, glyph swap, input parity, battery-drain note for store page (≥2 h target)
- [ ] **P6.E5.T4** — Min-spec definition doc: CPU/GPU/RAM/OS chosen, tested hardware list; CI smoke runs on the weakest box

**Phase 6 DoD:** exit criteria checked; benchmark CSV attached to M3; ADRs for DG-3 + IL2CPP.

---

**Related:** [README](README.md) · [Input baseline](../essential/input.md) · [Project settings](../essential/project-settings.md) · [Phase 7 QA](phase-07-qa-testing.md)
