# Phase 7 — QA & Testing

**Goal:** systematic quality assurance from M1 onward (not a phase that "starts at the end") — automated regression, structured manual coverage, compatibility matrix, playtest programs, and a repeatable RC process.
**Depends on:** P0.E6/E7 (CI + test foundation) · content from P1–P6 progressively.
**Effort:** ~12–16 person-weeks (rolling across M1–M4) · **Gate:** M4

## Exit criteria
- [ ] Regression suite (auto + manual) green on RC; compatibility matrix complete; RC certified by 2 independent testers
- [ ] Bug database: 0 open S1/S2, S3 only with accepted workarounds; triage cadence respected for ≥4 weeks before RC

---

## Epic P7.E1 — Test strategy & infrastructure (~2 pw)

- [ ] **P7.E1.T1** — `docs/qa/test-strategy.md`: scope per level (unit/PlayMode/manual/perf/compat), entry/exit per milestone, severity definitions (S1 crash/blocker … S4 nit), env matrix (dev build, staging build, release build)
- [ ] **P7.E1.T2** — Test case library structure `docs/qa/cases/`: `MT-*` manual cases with steps/expected/results columns; seed with smoke (P0.E2.T6), combat regression (P1.E9.T4), settings (P3.E4), save (P2.E4), audio (P4.E4)
- [ ] **P7.E1.T3** — Automated suite organization in CI: per-PR quick set (<3 min), nightly full set (smoke + perf-lite), weekly soak/perf; failure routing (issue auto-filed with CI link)
- [ ] **P7.E1.T4** — Build flavors pipeline: `dev` (overlay + debug keys), `staging` (telemetry to test sink), `release` (hygiene script P6.E4.T2) — produced by `build.yml`

## Epic P7.E2 — Automated regression coverage (rolling, ~3 pw)

- [ ] **P7.E2.T1** — Gameplay invariants: player cannot take damage during i-frames; enemy death exactly once; projectiles never leak beyond bounds; currency never negative; pause blocks all gameplay events
- [ ] **P7.E2.T2** — Flow/menu smoke: boot → menu → mission start → pause → resume → quit (headless)
- [ ] **P7.E2.T3** — Save/settings round-trip suite on every PR touching those services
- [ ] **P7.E2.T4** — Content validation: every mission loads, has ≥1 checkpoint + spawn volume + valid objectives; every prefab in build has no missing scripts (extends P0.E5.T4)
  - [ ] Coverage metric: all shipped missions included; runs nightly + pre-RC
- [ ] **P7.E2.T5** — Perf harness wired to CI (P6.E2.T1): nightly CSV + regression alarm >10% vs baseline

## Epic P7.E3 — Manual test campaigns (~3 pw)

- [ ] **P7.E3.T1** — Alpha campaign (M2): full playthrough ×2 difficulties, all screens/settings, full case library run; results + defects filed
- [ ] **P7.E3.T2** — Beta campaign (M3): repeat + accessibility cases (P3.E7) + localization pass (all languages, pseudo-loc already CI) + controller-only/keyboard-only runs
- [ ] **P7.E3.T3** — Compatibility campaign (see P7.E4) integrated with beta
- [ ] **P7.E3.T4** — Regression re-run discipline: every fixed S1/S2 gets a case added before closing (repro steps → case)
- [ ] **P7.E3.T5** — Test data & profiles: fresh profile, mid-profile, 100%-profile fixtures for save/migration checks

## Epic P7.E4 — Compatibility & hardware matrix (~2 pw)

- [ ] **P7.E4.T1** — Matrix doc: min-spec GPU (e.g. GTX 1050-class), mid (RTX 3060-class), integrated (Deck-class), × OS Win10/11 × 1080p/1440p/4K/21:9
- [ ] **P7.E4.T2** — Executions at M3 and M4 with pass/fail + fps captures (harness CSV + in-game fps cap verification)
- [ ] **P7.E4.T3** — Steam Deck full pass (P6.E5.T3): controls, glyphs, suspend/resume, 40 fps profile, battery claim; store-page-ready evidence
- [ ] **P7.E4.T4** — Peripheral checks: 2 gamepads (Xbox + DS4/DS5 via dual glyph support), KBM, ultra-wide monitor; input hot-plug (P6.E3.T3)
- [ ] **P7.E4.T5** — Localization/device screenshots archived (`docs/qa/evidence/`) — reused for P9 store assets

## Epic P7.E5 — External playtests & feedback (~2 pw)

- [ ] **P7.E5.T1** — Closed playtest group (10–20 recruited via Discord/Next Fest waitlist): NDA-free build under staging; structured survey per build (fun, difficulty, clarity, performance, would-buy) — feeds P9 copy
- [ ] **P7.E5.T2** — Streamer/press preview build (pre-launch, P9 coordination): bug-free slice + feedback form
- [ ] **P7.E5.T3** — Feedback pipeline: raw feedback → tagged issues → backlog; every session has retro notes in `docs/qa/playtests/`
- [ ] **P7.E5.T4** — Metrics thresholds from playtests feed RC: crash rate <1% sessions, avg session ≥20 min, would-buy ≥60%

## Epic P7.E6 — Bug process & RC certification (~2 pw)

- [ ] **P7.E6.T1** — Tracker workflow: labels (S1–S4, area, regression), templates (repro steps, build, hardware), weekly triage meeting; SLA: S1 same-day, S2 48 h
- [ ] **P7.E6.T2** — Release branch policy: `release/1.0` from M4; only fixes merge; cherry-pick discipline documented
- [ ] **P7.E6.T3** — RC certification checklist (sign-off sheet): all P7 exit criteria + README §2 definition-of-release items with named signers
- [ ] **P7.E6.T4** — Hotfix rehearsal: ship a fix through branch → CI → build → depot (dry-run against a beta branch) — proves P10 day-1 pipeline before launch

**Phase 7 DoD:** exit criteria checked; RC sheet signed.

---

**Related:** [README](README.md) · [Phase 6 performance](phase-06-technical-quality.md) · [Phase 8 gates](phase-08-platform-business.md) · [Smoke baseline](../essential/getting-started.md)
