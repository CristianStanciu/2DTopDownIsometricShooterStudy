# Phase 10 — Post-Launch, Live Operations & End-of-Life

**Goal:** protect launch momentum: fast fixes, community trust, measured content updates, and a clear end-state — sustaining "players come back constantly" (retention pillars: runs + unlocks + challenges).
**Depends on:** M5 shipped · telemetry & support channels live.
**Effort:** ongoing (budget 1 person for 8–12 weeks post-launch) · **Cadence-based, not milestone-based**

## Exit criteria (first 90 days)
- [ ] Day-1 patch shipped within 48 h if needed; S1 hotfix SLA (24 h) honored; Steam review score ≥80% (Very Positive trajectory) or corrective plan active
- [ ] ≥1 content update shipped; retention D7 ≥25% (or diagnosed); community process stable (response SLA met)

---

## Epic P10.E1 — Day-1 & hotfix operations (~1 pw)

- [ ] **P10.E1.T1** — Day-1 patch window: known-issue list from launch-week triage, fix → `release/1.0` → CI → RC re-cert (abbreviated checklist from P7.E6.T3) → depot ≤48 h
- [ ] **P10.E1.T2** — Hotfix SLA: S1 = build+verify within 24 h (rehearsed in P7.E6.T4); emergency rollback = previous depot build (documented steps in runbook)
- [ ] **P10.E1.T3** — Crash triage pipeline: `crash-*.txt` uploads (opt-in) + telemetry → daily review → issues tagged `postlaunch`; top-3 crash causes closed in first 2 weeks
- [ ] **P10.E1.T4** — Review response playbook: thank/deflect/report templates; reviews citing bugs linked to tracker issues; never astroturf (policy written down)

## Epic P10.E2 — Community & support (~1 pw + ongoing)

- [ ] **P10.E2.T1** — Support inbox + Discord staffed daily (launch month): response SLA <24 h business days; FAQ/knowledge base seeded (known issues, save recovery, settings tips)
- [ ] **P10.E2.T2** — Feedback loops: Steam discussions + Discord channels triaged weekly into backlog labels (`postlaunch`, area tags); monthly "you asked, we fixed" post
- [ ] **P10.E2.T3** — Mod-friendliness assessment: tuning SOs + localization tables already data-driven — decide expose-folder policy (loose files) and document what's supported vs unsupported
- [ ] **P10.E2.T4** — Community milestones: thank-you trailer at 100k wishlists/10k sales; wishlist/sale announcements coordinated with P9 cadence

## Epic P10.E3 — Content updates & monetization sustain (~4–8 pw per update)

- [ ] **P10.E3.T1** — Update roadmap (first 6 months): U1 (quality/perf + requested settings), U2 (new mission pack or horde mode — data reuse of P2.E2 pipeline), U3 (weapon/ability pack), seasonal challenges reusing P2.E3.T5
  - [ ] Each update = mini-phase: scope → build → QA (P7 suite) → patch notes (CHANGELOG source) → coordinated announce
- [ ] **P10.E3.T2** — Free vs paid decision framework: free updates grow reviews early; first paid DLC (if any) only after base ≥90% positive + content ≥3 h; pricing $4.99–7.99 documented in `docs/business/dlc-policy.md`
- [ ] **P10.E3.T3** — Balance & difficulty patches: post-launch telemetry (death points, weapon usage share from P6 events) → tuning SO releases (no code changes needed — validates P0.E4.T6)
- [ ] **P10.E3.T4** — Localization expansions: ship languages with proven demand (data from P8/P10 analytics), professional review pass for typos before each update

## Epic P10.E4 — Measurement & iteration (~ongoing, ~0.5 pw/mo)

- [ ] **P10.E4.T1** — Monthly health dashboard: DAU/MAU (Steam), session length, D1/D7/D30 retention, completion funnel, crash-free sessions ≥99%, review score trend
- [ ] **P10.E4.T2** — Quarterly roadmap re-plan: pull top community asks + data outliers into this file (new epics in P10), check boxes forward; update README §4 statuses
- [ ] **P10.E4.T3** — Sale strategy: participate in 2 major Steam sales/year (first at ≥4 months post-launch), never below -30% in year 1; calendar in `docs/business/calendar.md`

## Epic P10.E5 — End-of-life & archiving (final, ~1 pw)

- [ ] **P10.E5.T1** — Final stability release when winding down active support: last patch bundles all fixes; announce "final version" honestly
- [ ] **P10.E5.T2** — Preservation: final build archived (depot + zipped artifact + symbols), repo tagged `eol/1.x`, docs snapshot, save-format freeze documented
- [ ] **P10.E5.T3** — Postmortem v2 (business): what worked vs roadmap estimates (calibration for next project), archived in `docs/postmortems/`
- [ ] **P10.E5.T4** — Optional sequel/DLC green-light memo using retention + review data (go/no-go documented)

---

**Related:** [README](README.md) · [Phase 9 launch](phase-09-marketing-launch.md) · [Phase 7 bug process](phase-07-qa-testing.md) · [Save design](phase-02-progression-content.md)
