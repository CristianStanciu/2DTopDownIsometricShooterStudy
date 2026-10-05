# Phase 8 — Platform, Business & Compliance

**Goal:** make the game *sellable and legal*: Steam integration, store page, ratings, privacy, licensing remediation, pricing, analytics consent — no launch-blocking paperwork surprises.
**Depends on:** P0.E8 (ledger) · P2.E4 (save) · content stable enough for marketing (M2).
**Effort:** ~8–12 person-weeks · **Gate:** M4 (store approval before M5)

## Exit criteria
- [ ] Store page approved by Steam ≥2 weeks before launch; achievements/cloud verified in production appid
- [ ] Zero non-commercial assets shipped (ledger diff vs build = clean); ratings filed; privacy/ToS live; THIRD-PARTY shipped
- [ ] Pricing/regional plan documented; refund-risk factors (crash rate) within tolerance from P7

---

## Epic P8.E1 — Steam integration (~3 pw)

- [ ] **P8.E1.T1** — Steamworks setup: partner account, appid (early-access-free 1.0), Steamworks.NET or facepunch wrapper (ADR: pick, integrate behind `IPlatformServices` façade like P2.E4.T4)
- [ ] **P8.E1.T2** — Cloud saves: hook `ISaveStorage` to Steam Cloud (settings + profile), quota check, conflict test (two-machine) — reuses P6.E4.T1 policy
- [ ] **P8.E1.T3** — Achievements (10–15): story (finish biome), mastery (no-hit mission, all weapons used), grind (1000 kills), secret (find hidden cache) — with in-game trigger layer + P3.E7.T3 "assists disable" flags honored; icons generated
  - [ ] Test: unlock fires once, persists, syncs on next session
- [ ] **P8.E1.T4** — Stats/leaderboards (DG-5): mission score leaderboard if decision = yes (≤3 days budget) else document skip
- [ ] **P8.E1.T5** — Steam Deck metadata: input glyphs verified, "Steam Deck: Verified/Playable" questionnaire items met (P7.E4.T3 evidence)
- [ ] **P8.E1.T6** — Overlay/community: screenshots binding, FPS overlay compatibility, rich presence (mission name, difficulty), Steam Input default action sets

## Epic P8.E2 — Store page & assets (~3 pw) — *starts at M2 (risk R7)*

- [ ] **P8.E2.T1** — Name finalization (P0.E8.T3) + store copy: short/long description, tagline, feature bullets (validated against shipped features), system requirements from P6.E5.T4, supported languages table from P3.E6
- [ ] **P8.E2.T2** — Capsule set (main 616×353, small 462×174, header 616×353, 230×87 logo, 1232×706 hero): brief to artist/asset pipeline; readability tested at thumbnail size
- [ ] **P8.E2.T3** — Trailer: ≤60 s (gameplay-first 0–15 s), captures from P7.E4 evidence builds at 1440p60; captions + end card (price, wishlist CTA)
- [ ] **P8.E2.T4** — Screenshot set (8–10): every biome, boss, HUD visible, one accessibility-menu shot, one Deck shot; localized variants if needed
- [ ] **P8.E2.T5** — Steam review & setup: age gates (if violence rating requires), pricing incl. regional matrix + launch discount (10–20%, recommended 10% — align with P9.E5.T3), release-date set ≥2 weeks out, categories/tags (Action, Top-Down, Cyberpunk, Roguelite-if-applicable), review embargo plan for keys (P9)
- [ ] **P8.E2.T6** — Demo decision & build (if P9 wants Next Fest — recommended): content slice from P1.E9.T1 + first biome mission, separate appid, feedback survey link

## Epic P8.E3 — Legal & compliance (~2 pw)

- [ ] **P8.E3.T1** — Ratings: IARC questionnaire (Steam requires for store) → PEGI/ESRB equivalents if other platforms; content questionnaire reviewed against actual content (violence depiction, no real-money gambling); rating badges in assets
- [ ] **P8.E3.T2** — Privacy policy + terms of service: hosted URLs (store page + in-game links), covering telemetry events (P6.E4.T4, default-off), analytics vendor (if any), cookies on website, GDPR/CCPA rights contact; draft reviewed by professional or trusted template + counsel-lite pass
- [ ] **P8.E3.T3** — In-game compliance: first-run consent for telemetry + "Open privacy policy" in settings; credits screen live (P3.E3.T7); THIRD-PARTY.md packaged
- [ ] **P8.E3.T4** — License remediation execution (from P0.E8.T2): replace all non-commercial audio/sprites in build; final GUID-diff audit: shipped assets ⊆ ledger with commercial-use = yes
  - [ ] **Blocked-by check:** must be `[x]` before P9 paid advertising and before RC (README §2)
- [ ] **P8.E3.T5** — Open-source compliance: all package licenses (UPM) captured into THIRD-PARTY (script from manifest.json + custom list); MIT code attribution preserved in repo + game
- [ ] **P8.E3.T6** — Business ops: revenue account/tax forms for Valve payouts, bank setup, accounting cadence (monthly), contingency for refunds/chargebacks; studio entity decision documented

## Epic P8.E4 — Analytics & market validation (~1 pw)

- [ ] **P8.E4.T1** — Analytics vendor decision (or first-party: Steam + local telemetry): needs consent gate (P6.E4.T4), dashboards for funnel (boot→tutorial-complete→mission-1-complete), retention D1/D7 from beta builds
- [ ] **P8.E4.T2** — Beta data review at M3: funnel numbers vs targets (tutorial completion ≥80%, mission-1 completion ≥60%); fixes become P2/P3 tasks
- [ ] **P8.E4.T3** — Pricing sanity check: competitor scan (top-down shooters $12–25 band), wishlists-per-dollar read, final price + launch discount decision recorded in `docs/business/pricing.md`

**Phase 8 DoD:** exit criteria checked; store approval email archived; final license audit green.

---

**Related:** [README](README.md) · [Asset ledger task](phase-00-foundation.md) · [Phase 9 launch](phase-09-marketing-launch.md) · [Licensing baseline](../essential/project-overview.md)
