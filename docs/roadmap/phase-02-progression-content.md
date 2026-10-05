# Phase 2 — Progression, Economy & Content

**Goal:** give combat *purpose* and *longevity*: structure, levels, objectives, rewards, unlocks, saves — the reason players return.
**Depends on:** DG-1 resolved (spike before start) · P1 combat framework (P1.E1–E3) · P0.E4.T6 tuning SOs.
**Effort:** ~24–30 person-weeks · **Milestone:** M2 (content-complete alpha)

## Exit criteria
- [ ] DG-1 decision recorded as ADR; structure implemented end-to-end (new profile → win → unlock → replay harder content)
- [ ] Full level set playable with 3 objective types; economy/ unlocks verified by fresh-profile run; save survives version bump
- [ ] Content authoring documented so a designer can add a level without a programmer

---

## Epic P2.E1 — Structure decision & game-flow spine (~3 pw)

- [ ] **P2.E1.T1** — **DG-1 spike**: build 3 one-day prototypes (A: missions+meta, B: roguelite with run upgrades, C: endless horde with leaderboard) using P1 systems; playtest with 5 people; record results
- [ ] **P2.E1.T2** — Write ADR: chosen structure + rationale + rejected alternatives (feeds marketing copy too)
- [ ] **P2.E1.T3** — Session loop implementation: Mission Select → Briefing → Combat → Results/Reward → Meta menu; wired through Game Flow manager (P0.E4.T5)
  - [ ] Flow test: each transition fires once, pauses respect `PauseController` events, no orphaned inputs (TadaInput reset covered)
- [ ] **P2.E1.T4** — Mission metadata asset (`MissionDefinition` SO): scenes/levels, objectives, difficulty tier, rewards, unlock requirements, best-times storage keys

## Epic P2.E2 — Level & mission content (~12 pw)

*Content pipeline: blockout → gameplay pass → art pass → encounter tuning → QA sign-off per level. Using existing tileset/environment prefabs where possible; art upgrades tracked in P5.*

- [ ] **P2.E2.T1** — Authoring toolkit: editor script to stamp spawn volumes/checkpoints/objectives + validation (missing refs, unreachable exits) run on save
- [ ] **P2.E2.T2** — **Biome A — Neon District**: 3 missions (tutorial-friendly → mixed → gauntlet)
- [ ] **P2.E2.T3** — **Biome B — Industrial Core**: 3 missions with hazard mechanics (laser grids/conveyors — environment trigger pattern exists in `EnvironmentObjectTrigger`)
- [ ] **P2.E2.T4** — **Biome C — Data Vault**: 3 missions with defense/hold-objective rooms
- [ ] **P2.E2.T5** — Objective system: [clear area] + [reach exit] + [defend X seconds] + [eliminate target]; objective UI contract for P3 (event stream: objective id, progress, completion)
- [ ] **P2.E2.T6** — Encounter scripting: per-mission wave timelines (budget-driven via P1.E3.T3), optional secrets/side caches
- [ ] **P2.E2.T7** — Difficulty modifiers per mission (enemy HP/damage/spawn budget multipliers) + "Hard" unlock rule
- [ ] **P2.E2.T8** — Per-mission QA: completion run at Easy/Normal/Hard logged; average clear-time within design band; no blockers (docs/qa/mission-qa.md)

## Epic P2.E3 — Economy, meta-progression & unlocks (~5 pw)

- [ ] **P2.E3.T1** — Currency: earned from kills/objectives/time; server-less local ledger with anti-dupe checks (single source of truth class + tests)
- [ ] **P2.E3.T2** — Intermission hub or menu-based shop (decision within DG-1): buy permanent upgrades (HP, dash charge, starting ammo) with escalating costs — costs in tuning SOs
  - [ ] Economy sim test: full upgrade path reachable in target session count (design target: 8–15 h to full unlock)
- [ ] **P2.E3.T3** — Unlock tree: weapons (gated by P1.E5 availability), skills (P1.E6), weapon skins, profile badges; each unlock has explicit source rule (mission reward / purchase / achievement)
- [ ] **P2.E3.T4** — Results screen data contract: score formula (kills/objective/time/accuracy), currency awarded, unlock toasts — consumed by P3 UI
- [ ] **P2.E3.T5** — Daily/weekly challenge stubs (local, no server): 3 rotating modifiers reusing difficulty modifier machinery

## Epic P2.E4 — Save system (~3 pw)

- [ ] **P2.E4.T1** — Save service: single JSON profile (settings separate in P6) — progress, currency, unlocks, best times, challenge state, playtime
  - [ ] Atomic write (temp+rename) + backup file; corruption falls back to backup with user message
- [ ] **P2.E4.T2** — Versioned schema + migration chain: `SaveMigrator` with tests migrating v1→v2 synthetic fixtures (define v2 now = adds fields from P2.E3)
- [ ] **P2.E4.T3** — Multiple profiles/slots (3) + delete/rename with confirmation; safe-mode recovery if load throws
- [ ] **P2.E4.T4** — Cloud save hook point for Steam (P8): `ISaveStorage` interface with local + Steam implementations; integration test deferred to P8
- [ ] **P2.E4.T5** — Save/load fuzz test: kill process mid-write simulation (unit-level), 100 random ops round-trip equality

## Epic P2.E5 — Run/mission stats & replayability (~2 pw)

- [ ] **P2.E5.T1** — Telemetry-free local stats: per-weapon kills/accuracy, deaths per mission, personal bests; surfaced via stats screen (P3) and Steam stats (P8, DG-5)
- [ ] **P2.E5.T2** — Star/rank rating per mission (time, damage taken, accuracy) driving replay incentives
- [ ] **P2.E5.T3** — If DG-1 = B: run-replay seeding (seed shown, same spawns) — otherwise mark N/A with note


---

## Handoff to next phases

- P1 combat consumes: Game Flow (P0.E4.T5), Tuning SOs (P0.E4.T6), SceneContext (P0.E4.T3).
- P3 UI consumes contracts produced here: objective stream (P2.E2.T5), results data (P2.E3.T4), mission metadata (P2.E1.T4).
- P8 cloud saves consume `ISaveStorage` (P2.E4.T4); P9 marketing claims ("9 missions, 3 biomes…") must match the content census maintained in `docs/essential/`.

**Related:** [README](README.md) · [Phase 1](phase-01-core-combat.md) · [Phase 3](phase-03-ui-ux.md) · [Scene inventory](../essential/scenes.md)

