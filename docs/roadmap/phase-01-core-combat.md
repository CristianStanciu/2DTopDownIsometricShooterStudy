# Phase 1 — Core Combat Loop

**Goal:** make it a *game*: player can be hurt and die, enemies exist with distinct behaviors, weapons feel like an arsenal, encounters are challenging and tunable.
**Depends on:** P0.E4 (scene context/tuning SOs) recommended first; P0.E3 defects fixed.
**Effort:** ~26–34 person-weeks · **Milestone:** M1 (vertical slice: E1–E4 + E9) → M2 (E5–E8 complete)
**Design north-star:** pillar 1 (feel) + pillar 3 (arsenal depth). Every task below feeds both.

## Exit criteria
- [ ] Full combat loop playable: spawn → fight 6 enemy archetypes → boss → reward → death/victory → restart, no placeholders
- [ ] Balance spreadsheet live with baseline numbers for all weapons/enemies; 10 internal playtests logged
- [ ] Zero combat-related S1/S2 bugs; damage/AI covered by PlayMode tests

---

## Epic P1.E1 — Damage & health framework (~3 pw) — *foundation for everything*

- [ ] **P1.E1.T1** — `Health` component (max/current, invulnerability window, shields, regen hooks, death event via `UnityEvent`/C# event); works on player and enemies
  - [ ] Unit tests: damage clamp, i-frame immunity, death fires exactly once, heal caps at max
- [ ] **P1.E1.T2** — `DamageInfo` struct (amount, source, type, direction, crit, hit point) + `IDamageable` interface; all projectiles/melee route through one `DealDamage` entry point
- [ ] **P1.E1.T3** — Faction/teams on layers: reuse Physics2D layers 8 (`Player`) / 9 (`Projectile`) from `project-settings.md`; add `Enemy` layer + collision matrix update (projectile vs enemy, enemy vs player, no friendly fire)
  - [ ] Collision matrix screenshot + matrix test for every projectile/enemy/player pair
- [ ] **P1.E1.T4** — Hit resolution pipeline: projectile `OnTriggerEnter2D` → faction check → `IDamageable` → events (`OnDamaged`, `OnKilled`) consumed by VFX (P5), audio (P4), score (P2), camera shake (P5)
- [ ] **P1.E1.T5** — Damage numbers & hit feedback hook (data only; visuals in P5, UI in P3)
- [ ] **P1.E1.T6** — Death handling: despawn policy (ragdoll-lite pop, loot drop hook, corpse cleanup timer), pooling-friendly (P6)

## Epic P1.E2 — Player survivability (~3 pw)

- [ ] **P1.E2.T1** — Player HP + HUD data contract (event-driven, read by P3 HUD): current/max/armor
- [ ] **P1.E2.T2** — Hit reactions: screen flash hook, controller vibration hook, knockback resistance stat, i-frames on hit (tunable)
- [ ] **P1.E2.T3** — Death & respawn flow: death sequence → results screen hook (P3) → restart at checkpoint/mission start; integrate with Game Flow manager (P0.E4.T5)
- [ ] **P1.E2.T4** — Checkpointing rule per mission (P2 defines placement API now: serializable checkpoint component)
- [ ] **P1.E2.T5** — Difficulty-facing parameters (player damage taken multipliers) exposed to difficulty system (P1.E8)
- [ ] **P1.E2.T6** — Playtest: survive/feel test with i-frame tuning; record numbers in tuning doc

## Epic P1.E3 — Enemy framework & spawning (~5 pw)

- [ ] **P1.E3.T1** — Base `EnemyController` state machine (Spawn → Idle/Patrol → Chase → Attack → Stagger → Dead) as reusable base class — follows project pattern of abstract bases (`Weapon`, `SoundEmitter`)
- [ ] **P1.E3.T2** — Data-driven enemy definition: `EnemyArchetype` ScriptableObject (stats, weights, attack patterns, loot, spawn budget cost)
- [ ] **P1.E3.T3** — Spawner system: spawn volumes/waves, budget-based composition, spawn telegraph (portal/materialize VFX hook), despawn when off-camera with hysteresis
  - [ ] Test: 200-enemy stress spawn holds budget; off-screen despawn never removes engaged enemies
- [ ] **P1.E3.T4** — Aggro/targeting service: player target acquisition (sight radius, LOS raycast vs walls using existing wall collision), threat table for future summons
- [ ] **P1.E3.T5** — Navigation: choose approach — grid A* on tilemap vs steering+obstacle avoidance; implement selected approach with fallback (DG in ADR); must handle isometric offsets
  - [ ] Perf: path requests ≤5 ms/frame budget, amortized queue
- [ ] **P1.E3.T6** — Enemy animation integration per DG-4 baseline: legacy clips driven by state machine (`AnimationHandler` pattern already in project); stub animator hooks if DG-4 flips
- [ ] **P1.E3.T7** — Enemy archetypes pass "readability review": silhouette, color, telegraph audit against pillar 2 (checklist in docs/design/readability.md)

## Epic P1.E4 — Enemy roster (6 archetypes + elites, ~8 pw)

*Each archetype = prefab + Archetype SO + attack pattern + VFX/audio hooks + tuning entry + tests.*

- [ ] **P1.E4.T1** — **Rusher** (melee, fast, weak): lunge attack with telegraph; teaches dash timing
- [ ] **P1.E4.T2** — **Gunner** (ranged, mid HP): 3-round bursts, takes cover/strafes; projectile uses P1.E1 pipeline
- [ ] **P1.E4.T3** — **Shielded** (frontal block): must be flanked or broken; interacts with existing wall-façade idea (visual parity)
- [ ] **P1.E4.T4** — **Bomber** (explodes on death/proximity): AoE damage volume, clear audio tell (P4)
- [ ] **P1.E4.T5** — **Sniper** (long-range laser telegraph): teaches movement; line-of-sight checks against walls
- [ ] **P1.E4.T6** — **Drone/Flier** (ignores ground nav, erratic): exercises anti-air weapon selection
- [ ] **P1.E4.T7** — **Elite variants**: size/color/material tint + HP/damage multipliers + drop rates; data-driven (no new prefabs required)
- [ ] **P1.E4.T8** — Roster balance pass: time-to-kill matrix (player TTK per enemy, enemy TTK on player) documented; no archetype outside ±25% of design target

## Epic P1.E5 — Weapon arsenal expansion (~8 pw)

*Baseline: current weapon set documented in `docs/essential/weapons-and-projectiles.md`; task T1 first so every later weapon is data-driven.*

- [ ] **P1.E5.T1** — Weapon definition layer: `WeaponDefinition` SO (damage, cadence, spread, ammo/heat, VFX/audio ids, charge rules) wrapping existing `Weapon` subclasses so balance values leave code; migrate shipped weapons with zero behavior change
  - [ ] Golden test: migrated weapon stats identical to pre-migration values (serialized snapshot diff)
- [ ] **P1.E5.T2** — Ammo & heat systems: finite ammo with reserves + reload, OR overheat for energy weapons (both supported by definition); integrates with `WeaponHandler.CheckWeaponsAvailability` after P0.E3.T1
- [ ] **P1.E5.T3** — **New weapon: SMG** (fast tracking shots, spread growth) — rusher counter
- [ ] **P1.E5.T4** — **New weapon: Shotgun** (pellet cone, knockback) — crowd control; knockback hooks `DamageInfo.direction`
- [ ] **P1.E5.T5** — **New weapon: Beam/Laser** (hitscan channel, uses existing hitscan path, per-tick damage) — shielded/drone counter
- [ ] **P1.E5.T6** — **New weapon: Rocket/Grenade launcher** (arc or straight projectile, AoE volume, self-damage option off by default) — heavy/sniper counter
- [ ] **P1.E5.T7** — **New weapon: Chain/Tesla** (bounce to N nearby targets via target-acquisition reuse from P1.E3.T4)
- [ ] **P1.E5.T8** — **New weapon: Charge Rail** (upgrade of existing charge archetype: hold → pierce + shatter shields) — mastery weapon
- [ ] **P1.E5.T9** — Weapon switching UX: slot model (2 equipped from unlocked pool, current model), switch cooldown, ammo state on HUD contract (P3 consumes)
  - [ ] Test: switching during charge/reload never strands state (fuzz 50 random op sequences)
- [ ] **P1.E5.T10** — Mod/variant hooks: per-weapon 3 stat variants (e.g. burst/auto) unlocked via P2.E3; data only, no new code paths
- [ ] **P1.E5.T11** — Full arsenal playtest: each weapon rated for "changes how you move" (pillar 3) by ≥5 testers; revise or cut below 3/5 average

## Epic P1.E6 — Abilities & skills (~4 pw)

- [ ] **P1.E6.T1** — Active ability framework: `Ability` SO (cost/cooldown/effect) + cooldown UI contract + input binding via `TadaInput` (works KBM + pad)
- [ ] **P1.E6.T2** — Ability: **Frag Grenade** (thrown arc, AoE, shares damage pipeline)
- [ ] **P1.E6.T3** — Ability: **EMP Pulse** (disables projectiles + stuns drones/shielded briefly) — strategic counterplay
- [ ] **P1.E6.T4** — Ability: **Bullet Time / Focus** (time-scale effect respecting `PauseController` and fixed-step logic; must not break `CameraBehaviour` smoothing — regression check vs P0.E3.T10)
- [ ] **P1.E6.T5** — Dash skill tree: second charge, cancel-window reduction, damage-through — evolves existing `PlayerSkills` dash trails
  - [ ] Each node has tuning entry + test for stacking rules (no infinite uptime: verify min downtime ≥ X s)
- [ ] **P1.E6.T6** — Ability audiovisual hooks registered with P4/P5 systems (ids + events only)

## Epic P1.E7 — Boss encounters (2 bosses + final, ~7 pw)

- [ ] **P1.E7.T1** — Boss framework: multi-phase state machine (extends `EnemyController`), phase-transition invulnerability + cinematic beat hooks, weak-point `IDamageable` sub-targets, boss health bar data contract (P3)
- [ ] **P1.E7.T2** — Boss A — **Warden** (Biome A finale): shield-mirror mechanic reusing shielded-enemy rules; 2 phases; arena = mission scene with lockdown volumes
- [ ] **P1.E7.T3** — Boss B — **Foundry Serpent** (Biome B): segments as separate colliders with inheritance rules; hazard interaction (laser grids)
- [ ] **P1.E7.T4** — Boss C — **Archon** (Biome C finale): 3 phases combining roster mechanics (rushers + beams + EMP); final-form readability review (pillar 2)
  - [ ] Each boss: kill possible with every loadout (no soft-locks); time-to-kill within 3–6 min band on Normal
- [ ] **P1.E7.T5** — Boss checkpoints + results hooks; boss spawn uses P1.E3.T3 spawn telegraph machinery
- [ ] **P1.E7.T6** — Boss music cues requested from P4 (state events); death sequence VFX per P5

## Epic P1.E8 — Difficulty & balance system (~3 pw)

- [ ] **P1.E8.T1** — Difficulty modes (Easy/Normal/Hard/Insanity): multipliers on enemy HP/damage/spawn budget, player i-frames, economy rewards; single `DifficultyProfile` SO per mode
- [ ] **P1.E8.T2** — Adaptive-assist hooks (optional): performance-based grace (fewer spawns after repeated deaths) — default on for Easy only
- [ ] **P1.E8.T3** — Balance sheet (spreadsheet or in-repo CSV): TTK matrices, DPS-by-weapon, spawn budget costs; CI script exports current SO values → sheet diff (design sees every tuning change)
- [ ] **P1.E8.T4** — Playtest cadence: structured session every 2 weeks from M1 (≥5 internal + rotating externals); log results in `docs/qa/playtests/`; top-5 issues become tuning tasks
  - [ ] Target metrics: ≥70% testers finish Normal without >3 retries/mission; ≥60% rate difficulty "fair"

## Epic P1.E9 — Combat integration & slice gate (~2 pw)

- [ ] **P1.E9.T1** — One **vertical slice mission**: Biome A mission 1 with full stack (spawn → 3 archetypes → mini-encounter → results stub → death/restart); this is the M1 demo build
  - [ ] Slice runs on min-spec hardware at 60 fps with all placeholder art/audio (P5/P4 replace later)
- [ ] **P1.E9.T2** — Combat event bus audit: every gameplay event (damaged, killed, weaponFired, abilityReady, died) published exactly once, documented in `docs/essential/architecture.md` addendum — consumers: P3 UI, P4 audio, P5 VFX, P2 score
- [ ] **P1.E9.T3** — Feel gate review (pillar 1): 5-tester structured review of slice (input latency, feedback clarity, death fairness); ≥4/5 average required to proceed to content mass-production; failures spawn tuning tasks in P1.E8/P5
- [ ] **P1.E9.T4** — Combat QA regression pack: PlayMode tests for damage pipeline, faction matrix, spawner budget, weapon switching fuzz — wired into CI (P0.E6)

**Phase 1 DoD:** exit criteria all checked; M1 signed off.

---

**Related:** [README](README.md) · [Phase 0](phase-00-foundation.md) · [Phase 2](phase-02-progression-content.md) · [Weapons baseline](../essential/weapons-and-projectiles.md) · [Player baseline](../essential/player.md)


