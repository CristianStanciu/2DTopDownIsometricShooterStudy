# Phase 3 — UI / UX & Onboarding

**Goal:** a complete, navigable, localizable, accessible front-end and HUD — the layer players judge first; replaces the debug `GUIHandler` overlay entirely.
**Depends on:** P0.E4.T5 (Game Flow) · **DG-2** (UI stack) · data contracts from P1/P2.
**Effort:** ~18–24 person-weeks · **Milestone:** core at M2, complete at M3

## Exit criteria
- [ ] Full front-end loop (boot → menu → settings → play → pause → results → menu) with gamepad-only navigation
- [ ] HUD covers HP/ammo/cooldowns/objectives/boss bar; zero `GUIHandler` debug text in release builds
- [ ] Localization pipeline live (≥2 languages), accessibility set complete (P3.E7), UI passes console-style focus/edge-case tests

---

## Epic P3.E1 — UI foundation & shell (~4 pw)

- [ ] **P3.E1.T1** — **DG-2 spike**: same settings screen in UI Toolkit vs UGUI+TMP (0.5 day each): performance, authoring speed, nine-slice/skinning; ADR written; recommendation default UGUI (already in project)
- [ ] **P3.E1.T2** — UI assembly + prefab structure: `UIRoot` prefab (EventSystem, canvases: HUD / Frontend / Overlays) instanced by a UI service; screen stack API (`Show<T>(args)` / back navigation) fed by Game Flow events
  - [ ] Test: screen stack push/pop ordering + back behavior; no input leaks to gameplay while frontend open (TadaInput focus gate)
- [ ] **P3.E1.T3** — UI skin foundation: cyberpunk design tokens (colors, fonts incl. CJK-capable fallback, spacing, nine-slice panels) as prefab variants + a small component library (button/toggle/slider rows)
- [ ] **P3.E1.T4** — `GUIHandler` retirement plan: debug overlay becomes `DevelopmentOnly` component (compiled out of release via `#if DEVELOPMENT_BUILD || UNITY_EDITOR`)

## Epic P3.E2 — HUD (~4 pw)

- [ ] **P3.E2.T1** — Player status cluster: HP bar (+ armor pips), i-frame flash, damage-direction indicators — reads P1.E2 contract via events (no polling)
- [ ] **P3.E2.T2** — Weapon HUD: current weapon icon/name, ammo/heat bar, slot 1/2 indicators, reload/overheat states, weapon-switch flash — reads P1.E5.T9 contract
- [ ] **P3.E2.T3** — Ability & dash cooldown wheels (charges for dash per P1.E6.T5), low-cooldown micro-feedback
- [ ] **P3.E2.T4** — Objective tracker (P2.E2.T5 events): current objective, progress, timer variants; boss health bar (P1.E7.T1) with phase ticks
- [ ] **P3.E2.T5** — Feedback layer: floating damage numbers (P1.E1.T5), pickup/unlock toasts (P2.E3.T4), kill combo popups; all pooled (P6)
- [ ] **P3.E2.T6** — HUD layout modes: safe-zone insets (ultrawide/TV), scale slider 80–140%, minimal-HUD toggle; screenshot matrix at 16:9 / 16:10 / 21:9 attached to PR

## Epic P3.E3 — Frontend screens (~5 pw)

- [ ] **P3.E3.T1** — Boot/splash: engine/publisher logos (legal attribution P0.E8.T4), autosave-safe continue → straight into last context
- [ ] **P3.E3.T2** — Main menu: Continue / New Game / Missions / Loadout / Settings / Extras / Quit; news slot (static markdown for now); pad + KBM glyphs (P3.E5)
- [ ] **P3.E3.T3** — Mission select: biome grouping (P2.E2), difficulty select (P1.E8.T1), star ratings (P2.E5.T2), locked-state explanations, briefing panel (objectives, rewards, modifiers)
- [ ] **P3.E3.T4** — Loadout screen: weapon slots + unlock/purchase states from P1.E5/P2.E3, ability loadout (P1.E6), stats panel (P2.E5)
- [ ] **P3.E3.T5** — Pause menu (in-game): Resume / Settings / Controls / Restart mission / Abandon → confirm; wired to `PauseController` events (no new pause logic); timeScale audited against P1.E6.T4
- [ ] **P3.E3.T6** — Results screen: score breakdown (kills/objective/time/accuracy), currency, unlocks, rank, buttons (Retry/Next/Menu) — reads P2.E3.T4; game-over variant reads P1.E2.T3
- [ ] **P3.E3.T7** — Credits & legal screen: THIRD-PARTY attributions rendered from `THIRD-PARTY.md` (generated), licenses of shipped assets, open-source notices
  - [ ] Test: every ledger entry appears; screen reachable from menu and after credits roll

## Epic P3.E4 — Settings system (~3 pw)

- [ ] **P3.E4.T1** — Settings service: typed settings model (video/audio/gameplay/accessibility), JSON in `Application.persistentDataPath`, atomic writes, defaults + "Reset section", applied live where possible
  - [ ] Tests: round-trip, corrupted file → defaults + user message, unknown fields preserved
- [ ] **P3.E4.T2** — Video settings: resolution, display mode (fullscreen/borderless/windowed), VSync, FPS cap (30/60/120/144/unlimited), quality preset (Low/Med/High/Ultra → URP asset switch via `QualitySettings`), post-process toggles (grain/vignette/SMH per `rendering.md`), camera shake slider (juice, P5)
  - [ ] URP assets for presets authored (reuse `Forward_Low_NoLights` as base); preset switch verified in play mode
- [ ] **P3.E4.T3** — Audio & gameplay settings: 4 volume sliders (P4.E1), language selector (P3.E6), damage numbers on/off, screen-shake 0–150%, flash reduction, crosshair options (color/size from existing `CrosshairHandler` params)
- [ ] **P3.E4.T4** — Controller settings: vibration 0–100%, stick sensitivity + deadzone curves (feeds `TadaInput`), invert-Y

## Epic P3.E5 — Input UX & device parity (~2 pw)

- [ ] **P3.E5.T1** — Input display layer: per-action glyph lookup (keyboard/mouse/Xbox/DS) replacing hardcoded overlay text; consumes P0.E3.T11 binding table
- [ ] **P3.E5.T2** — Full rebinding UI (if DG-3 stays legacy: runtime `InputManager` axis remap is limited — prefer `Input System` per DG-3; else keyboard remap of named actions + controller alias mapping): conflict detection, reset-to-default, "press any key" capture, reserved-binding guard
  - [ ] Test: rebinding persists across restart; invalid combos rejected with message
- [ ] **P3.E5.T3** — Navigation audit: every screen fully usable with controller only (focus memory, no mouse-only hover, wrap-around), plus keyboard-only pass

## Epic P3.E6 — Onboarding & localization (~3 pw)

- [ ] **P3.E6.T1** — Tutorial mission (Biome A M1) with contextual hints: first dash, weapon switch, ability, swap-cover; hints data-driven (localizable strings), skippable, "show again" option
- [ ] **P3.E6.T2** — Diegetic tip cards + glossary entry for each enemy archetype (first encounter intel pop)
- [ ] **P3.E6.T3** — Localization framework: string table per language (start EN + one more, e.g. DE or ZH per market data), pseudo-localization mode (length expansion + accents) to catch truncation in CI build, all UI text through table (no raw strings — includes P0.E3.T11 overlay)
  - [ ] Pseudo-loc pass: zero missing keys, zero clipped layouts at 140% expansion
- [ ] **P3.E6.T4** — Text fit QA: longest strings × smallest supported resolution; auto-shrink components where needed

## Epic P3.E7 — Accessibility (~2 pw)

- [ ] **P3.E7.T1** — Color: 3 colorblind modes (protan/deutan/tritan) recoloring crosshair, enemy telegraphs, team indicators; "high-contrast enemies" option
- [ ] **P3.E7.T2** — Motion: screen-shake off/scale (P3.E4.T3), full-screen flash reduction (post-process + shader flag), camera FOV/kick scale, reduce-motion UI transitions
- [ ] **P3.E7.T3** — Difficulty assists menu: damage-taken scaling, auto-aim magnetism slider (new: aim assist on controller — feeds `TadaInput` aim), slow-motion helper, invulnerability (flag disables achievements via P8)
- [ ] **P3.E7.T4** — Accessibility audit vs shipping checklist (industry baseline); report in `docs/qa/a11y.md`; **DoD for phase requires this green**

**Phase 3 DoD:** exit criteria checked; navigation audit (P3.E5.T3) + pseudo-loc CI gate green.

---

**Related:** [README](README.md) · [Input baseline](../essential/input.md) · [Phase 1](phase-01-core-combat.md) · [Phase 6 settings persistence](phase-06-technical-quality.md)

