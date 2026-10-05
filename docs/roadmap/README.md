# Master Development Roadmap — [Working Title] Neo Cyber Shooter

**Status: REVIEWED v1.1 — full QA review passed ([review report](../review-report.md)); implementation starts only after owner approval.**
Baseline: verified codebase docs in [`docs/essential/`](../essential/index.md) · Unity `6000.3.20f1` · commit `c9d5f0e` · branch `develop`.

---

## 0. How to use this roadmap

### Status markers
| Marker | Meaning | Rule |
|---|---|---|
| `- [ ]` | Not started | default state |
| `- [~]` | In progress | set when work starts |
| `- [-]` | Blocked | must be followed by `**Blocked by:** …` note |
| `- [x]` | Done | only after all child boxes are `[x]` AND verification evidence exists (test/CI/PR/screenshot) |

- **IDs are stable**: `P<phase>.E<epic>.T<task>` (e.g. `P0.E2.T3`). Reference them in commits (`refs P0.E2.T3`), PR titles, and the tracker (Jira keys map 1:1).
- **Sub-bullets are acceptance criteria** — they are themselves `- [ ]` boxes; a task is complete only when every child is checked.
- **Phase done** = every task `[x]` + phase **Exit Criteria** all checked.
- Update the **Global progress tracker** (§4) whenever a phase changes ≥10%.
- Dependencies are stated per epic; never start an epic whose dependency is not `[x]` (or explicitly waived).

### Linked documents
- Defect details with `file:line`: [`known-issues-and-roadmap.md`](../essential/known-issues-and-roadmap.md)
- Architecture / scenes / settings baselines: [`architecture.md`](../essential/architecture.md), [`scenes.md`](../essential/scenes.md), [`project-settings.md`](../essential/project-settings.md)

---

## 1. Product vision & pillars (assumed — confirm at review)

**Pitch (assumption A1):** a fast, readable top-down isometric cyberpunk shooter where dash-guns and controlled chaos rule — tight mouse/gamepad aim, an escalating weapon arsenal, distinct enemy swarms — a $15–20 premium PC game players replay for mastery.

**Pillars** (gate every feature request):
1. **Feel first** — one-frame input response; hits/dashes/deaths are tactile (hit-stop, shake, VFX).
2. **Readable chaos** — density without noise; silhouette, color and audio always tell the truth.
3. **Arsenal depth** — weapons/skills that change *how you move*, not just stat bumps.
4. **Repeatable structure** — 15–45 min sessions with meta-progression and unlockables.
5. **Premium finish** — no placeholders in the release build; 2026 table stakes (accessibility, remapping, Steam Deck, ultrawide) are mandatory.

### Explicit assumptions (flag anything wrong at review)
- **A1** Windows PC → Steam, price **$14.99–$19.99**, regional pricing on, no microtransactions; DLC optional post-launch.
- **A2** Small team (solo–5) + contractors; effort shown as **person-weeks**; milestones M0–M6 are *relative* — map to calendar yourself.
- **A3** Structure: **mission-based action + meta-progression** (DG-1 can still switch to roguelite/horde).
- **A4** Existing free-tier audio/sprites are **provisional**; provenance audit in P0, remediation before paid marketing (P8).
- **A5** Keep URP 17 + 2D physics; upgrade `Animation` selectively (DG-4), no full Mecanim rewrite by default.
- **A6** Single-player only; no netcode/anti-cheat (DG-6 defers consoles).

### Decision gates (resolve before dependent work)
| ID | Decision | Needed by | Options / recommendation |
|---|---|---|---|
| DG-1 | Missions+meta vs roguelite vs horde | P2 start | 1-week spike of 3 prototypes; recommend missions+meta |
| DG-2 | UI Toolkit vs UGUI+TMP | P3 start | recommend UGUI (uGUI already in project) unless profiling says otherwise |
| DG-3 | Legacy Input vs `com.unity.inputsystem` | P6 (or earlier) | migrate *behind* `TadaInput` façade so consumers don't change |
| DG-4 | Legacy clips vs Animator vs procedural aim | P5 | hybrid: legacy locomotion + procedural upper-body aim |
| DG-5 | Steam leaderboards/stats | P8 | include if ≤3 days work |
| DG-6 | Console ports | post-launch | keeps P6 abstraction/cert-friendly |

---

## 2. Definition of "100% release-ready" (gate for RC)

- [ ] Zero compiler warnings/errors on CI for the pinned editor version; all unit/play tests green
- [ ] All roadmap tasks `[x]` or explicitly descoped with sign-off in this file's change log
- [ ] Content complete: full level set, full enemy roster, final bosses, economy & unlocks verified in a fresh profile run-through
- [ ] Performance: locked 60 fps on min-spec GPU (1080p), 120+ fps on mid-spec; no hitches >50 ms during combat (budgets in P6)
- [ ] Stability: 8-hour soak session with zero exceptions; save/load migration tested from every prior schema version
- [ ] Settings: video/audio/controls/accessibility options all persist, reset, and apply live
- [ ] Accessibility: colorblind modes, subtitles, remapping, screen-shake/flash toggles, difficulty assists — all usable end-to-end
- [ ] Platforms verified: Windows 10/11, Steam Deck (playable/verified target), ultrawide + windowed/borderless/fullscreen, keyboard-mouse + 2 controllers (Xbox/DS)
- [ ] Legal clean: asset ledger complete, no non-commercial licenses in build, ratings filed (IARC/PEGI/ESRB), privacy policy + ToS live, attribution file shipped
- [ ] Store ready: capsules, trailer, screenshots, copy, tags, system requirements, achievements, cloud saves — Steam page passes review
- [ ] QA sign-off: regression suite run + RC build certified by 2 independent testers; open bug count = 0 at S1/S2, no S3+ with known workarounds
- [ ] Ops ready: crash/analytics reporting (opt-in), support inbox + Discord staffed, day-1 patch plan + hotfix rollback tested
- [ ] Marketing gates: ≥10,000 wishlists or documented alternative launch plan, demo tested at a festival, press keys seeded
- [ ] Launch config: build signed/versioned (`bundleVersion` = release version), changelog written, launch discount decided

---

## 3. Milestone plan (relative — map to calendar at review)

| Milestone | Scope | Exit gate | Target |
|---|---|---|---|
| **M0 — Foundation green** | P0 complete | CI green, zero warnings, ledger exists, smoke passes | Month 1–2 |
| **M1 — Combat vertical slice** | P1.E1–E4 + slice build (P1.E9) + minimal HUD/pause (thin slices of P3) + placeholder audio swap plan | one mission vs 6 archetypes feels good (≥5 testers rate feel ≥4/5) | Month 3–5 |
| **M2 — Content-complete alpha** | P1.E5–E8 (arsenal, abilities, bosses, baseline balance) + P2 complete + P3 core + P4 core | full loop new-profile → finish; save works | Month 6–9 |
| **M3 — Feature complete beta** | P3 final + P4–P6 complete | perf budgets met; accessibility set complete; Deck played | Month 10–12 |
| **M4 — Release candidate** | P7 + P8 gates | RC cert checklist all green; store approved | Month 13–14 |
| **M5 — Launch** | P9 | 1.0 shipped, day-1 ops live | Month 15 |
| **M6 — Season 1** | P10 first content update + hotfix cadence | metrics reviewed, top post-launch bugs closed | Month 16–18 |

*Float: ~1 month built in after M3. If slipping >6 weeks: cut P2.E3.T5 (challenges), P1.E4.T7 (elites), P5.E4.T5 (UI juice) — in that order (descope list).*

## 4. Global progress tracker

| Phase | File | Effort (pw) | Depends on | Status |
|---|---|---|---|---|
| P0 Foundation | [phase-00](phase-00-foundation.md) | 14–18 | — | 0% |
| P1 Combat | [phase-01](phase-01-core-combat.md) | 26–34 | P0.E3, P0.E4 | 0% |
| P2 Progression | [phase-02](phase-02-progression-content.md) | 24–30 | DG-1, P1.E1–E3, P0.E4.T6 | 0% |
| P3 UI/UX | [phase-03](phase-03-ui-ux.md) | 18–24 | P0.E4.T5, DG-2 | 0% |
| P4 Audio | [phase-04](phase-04-audio.md) | 8–12 | P1, P0.E8 | 0% |
| P5 Feel & visuals | [phase-05](phase-05-juice-visual.md) | 12–16 | P1, DG-4, P0.E4.T6 | 0% |
| P6 Technical quality | [phase-06](phase-06-technical-quality.md) | 10–14 | P0, DG-3 | 0% |
| P7 QA | [phase-07](phase-07-qa-testing.md) | 12–16 | rolling, gate M4 | 0% |
| P8 Platform & compliance | [phase-08](phase-08-platform-business.md) | 8–12 | P0.E8, M2 | 0% |
| P9 Marketing & launch | [phase-09](phase-09-marketing-launch.md) | 8–12 | P8 store | 0% |
| P10 Post-launch | [phase-10](phase-10-post-launch.md) | ongoing | M5 | 0% |
| **Total** | | **~140–190 pw** | | **0%** |

## 5. Cross-cutting engineering standards

- **Definition of Done (task):** code + tests (unit for logic, PlayMode for scene behavior) + docs updated + CHANGELOG entry + CI green + self-reviewed against pillar impact.
- **Definition of Done (epic):** all tasks `[x]` + epic acceptance line at its bottom checked.
- **Git:** trunk-ish — `develop` always releasable; every commit references an ID; rebase/PR only, no direct pushes to protected branches.
- **Testing policy:** pure logic → EditMode unit tests; scene/physics behavior → PlayMode; content → validation editor scripts + manual cases (`MT-*` IDs in `docs/qa/`).
- **Performance budgets (enforced from M3):** 60 fps min-spec 1080p (16.6 ms), CPU main ≤10 ms, draw calls ≤300, allocation spikes 0 in steady-state combat, load ≤5 s per mission.
- **Code style:** `.editorconfig` + Unity defaults; no new `FindObjectOfType`; all magic numbers live in tuning SOs (P0.E4.T6); statics must implement reset (P0.E4.T4).
- **Docs:** `docs/essential/` stays accurate (scene census re-run when hierarchy changes); ADRs in `docs/adr/` for every DG decision.
- **Cadence:** weekly 30-min triage (bug severity S1–S4), biweekly roadmap status update (check boxes), monthly risk review.

## 6. Top risks (review monthly)

| # | Risk | Impact | Likelihood | Mitigation | Owner-task |
|---|---|---|---|---|---|
| R1 | Non-commercial audio/sprites block sale (A4) | **critical** | high | audit P0 → replace budget in P8 | P0.E8.T1/T2 |
| R2 | Scope creep vs small team (A2) | high | high | descope list §3; pillars gate features | §3 |
| R3 | Feel debt: bolted-on juice reads cheap | high | medium | P5 starts at M1, not after content | P5.E1 |
| R4 | Balance whiplash late (numbers frozen too late) | medium | medium | tuning SOs from day one; playtest cadence P1.E8.T4 | P1.E8 |
| R5 | Save corruption on launch day | high | low | P2.E4 fuzz tests + backup + opt-in telemetry | P2.E4 |
| R6 | Unity patch upgrade re-serializes repo mid-dev | low | medium | version pin P0.E1.T4 | P0.E1.T4 |
| R7 | Steam page late → weak wishlists | high | medium | P8 store starts at M2, not M4 | P8.E2 |
| R8 | Deck/ultrawide found late | medium | medium | compat matrix runs from M3 | P7.E4 |
| R9 | Solo bus-factor / knowledge silo | high | medium | docs-first DoD; ADRs; pairing on P1/P6 | §5 |
| R10 | License mis-audit (wrong attribution) | medium | low | THIRD-PARTY.md generated from ledger, reviewed by lawyer-lite checklist | P0.E8.T4 |

## 7. Phase index

1. [Phase 0 — Foundation](phase-00-foundation.md) · 2. [Phase 1 — Combat](phase-01-core-combat.md) · 3. [Phase 2 — Progression & content](phase-02-progression-content.md) · 4. [Phase 3 — UI/UX](phase-03-ui-ux.md) · 5. [Phase 4 — Audio](phase-04-audio.md) · 6. [Phase 5 — Feel & visuals](phase-05-juice-visual.md) · 7. [Phase 6 — Technical quality](phase-06-technical-quality.md) · 8. [Phase 7 — QA](phase-07-qa-testing.md) · 9. [Phase 8 — Platform & compliance](phase-08-platform-business.md) · 10. [Phase 9 — Marketing & launch](phase-09-marketing-launch.md) · 11. [Phase 10 — Post-launch](phase-10-post-launch.md)

---
*Change log: v1.0 initial draft → **v1.1 first full QA review** — structure, cross-references, milestone coverage and code-fact verification fixes (all findings closed; see [review report](../review-report.md)).*

