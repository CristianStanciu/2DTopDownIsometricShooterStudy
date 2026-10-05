# Phase 5 — Game Feel & Visual Polish

**Goal:** pillar-1/pillar-2 execution: turn correct gameplay into *tactile, readable* gameplay — hit-stop, shake, particles, animation, lighting — and remove every placeholder look.
**Depends on:** P1 events · **DG-4** (animation) · P0.E4.T6 (juice tuning).
**Effort:** ~12–16 person-weeks · **Milestone:** starts M1 (slice feedback), complete M3

## Exit criteria
- [ ] Feel-gate metrics hit (P1.E9.T3 ≥4/5 after P5 passes); readability checklist green for every enemy/boss
- [ ] Zero placeholder VFX/art in shipped scenes; VFX within GPU budget (P6); animation covers all combat states
- [ ] Consistent camera/crosshair language: every action has defined feedback (feedback matrix doc complete)

---

## Epic P5.E1 — Feel layer (hit-stop, shake, feedback, ~3 pw)

- [ ] **P5.E1.T1** — Unified `GameFeel` service: hit-stop (scaled pause ×ms per hit weight), directional shake requests through existing `CameraShake` (respect P0.E3.T5 cooldown), controller rumble, chromatic pulse hook — all with global intensity multipliers (accessibility P3.E7)
  - [ ] Test: hit-stop never stacks infinitely (clamped), timeScale restored exactly, respects pause state
- [ ] **P5.E1.T2** — Feedback matrix: action → feedback recipe (e.g. kill = stop 40 ms + shake S + burst + rumble + audio); documented as tuning data, enforced in review checklist
- [ ] **P5.E1.T3** — Player-facing telegraph system: standardized flash/scale/audio grammar for incoming attacks (shared by enemies + bosses) — pillar 2 cornerstone
  - [ ] Readability test: 10 testers identify each attack type from first frame of telegraph ≥90% accuracy
- [ ] **P5.E1.T4** — Impact VFX on `OnDamaged`/`OnKilled` events (P1.E1.T4): sparks, debris, scorch decals (URP decal projector), death dissolve material

## Epic P5.E2 — VFX library & effects (~4 pw)

- [ ] **P5.E2.T1** — VFX catalogue + pooling integration (P6): id-addressable effects (muzzle, impact by surface, shell casings, dash streak, ability bursts), budgets per effect (particles, overdraw), LOD/shrink off-screen
- [ ] **P5.E2.T2** — Weapon VFX identity: distinct muzzle/impact/tracer per weapon (P1.E5) — charge states visibly escalate (rail glow stages)
- [ ] **P5.E2.T3** — Enemy VFX identity: per-archetype spawn/materialize, hurt, shield break, drone explode; elites add aura
- [ ] **P5.E2.T4** — Environment VFX: neon flicker, steam vents, rain/dust motes per biome (P2.E2), hazard warnings (lasers charge-up)
- [ ] **P5.E2.T5** — Post-process choreography (URP Volume per `rendering.md`): global GR/SMH + per-event pulses (hit = micro-GR 50 ms, low HP = subtle vignette), all gated by accessibility toggles; profile-switch perf test (no GC alloc on pulse)

## Epic P5.E3 — Animation upgrade (~4 pw) — *per DG-4*

- [ ] **P5.E3.T1** — **DG-4 spike**: compare legacy `Animation` (current: `AnimationHandler` + clips in `Assets/Sprites/Animations`) vs Animator vs hybrid procedural for the player; ADR with evidence (authoring speed, blend needs, aim layer)
- [ ] **P5.E3.T2** — Player animation completeness: idle/run/dash (4 dirs or flipped), fire, hurt, death, weapon-switch, ability use; directional aim via existing body-parts rig (`PlayerBodyPartsHandler` aim pipeline preserved)
- [ ] **P5.E3.T3** — Enemy animation set per archetype (P1.E4): move variants, attack telegraph, stagger, death; legacy clips or Animator per ADR; retargetable naming convention documented
- [ ] **P5.E3.T4** — Boss animation direction: multi-phase transitions scripted (timeline or state events), weak-point reactions; approved by readability review (P1.E7.T4)
- [ ] **P5.E3.T5** — Procedural layers: squash/stretch on dash-land, weapon recoil kick, additive hurt flinch — small, tuning-driven

## Epic P5.E4 — Environment art, lighting & camera polish (~3 pw)

- [ ] **P5.E4.T1** — Art pass per biome level (P2.E2): tileset consistency, edge treatments, parallax/backdrop layers, prop dressing; art bible page in `docs/design/` (palette, material language)
- [ ] **P5.E4.T2** — Lighting pass: 2D lights where they earn cost, emissive neon materials, per-room mood via Volume profiles (P5.E2.T5); no readability regressions (enemies remain highest contrast)
- [ ] **P5.E4.T3** — Camera polish: screen-center offset logic already exists — add look-ahead on dash, subtle zoom on boss intro, trauma-based shake weighting (P5.E1.T1), all scale sliders respected (P3.E4)
- [ ] **P5.E4.T4** — Crosshair polish: hit-confirm tick, kill-confirm pulse, controller magnetism visual (P3.E7.T3), crosshair presets (P3.E4.T3) — builds on `CrosshairHandler` params
- [ ] **P5.E4.T5** — UI juice (descope candidate): animated counters, card flips on unlock, button micro-motion, screen transitions — polished last per §3 descope rule

**Phase 5 DoD:** exit criteria checked; feel-gate re-run ≥4/5; readability tests green.

---

**Related:** [README](README.md) · [Rendering baseline](../essential/rendering.md) · [FX baseline](../essential/environment-and-fx.md) · [Phase 6 budgets](phase-06-technical-quality.md)
