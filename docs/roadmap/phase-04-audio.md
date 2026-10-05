# Phase 4 — Audio

**Goal:** replace provisional tracks with a licensed soundtrack and build a dynamic, mixable, reactive audio system — audio is ≥50% of game feel.
**Depends on:** P1 combat events · P0.E8 audit (replacement budget) · **starts at M1 with placeholder swap plan**.
**Effort:** ~8–12 person-weeks (+ composer/marketplace cost) · **Milestone:** core M2, final M3

## Exit criteria
- [ ] No non-commercial audio in any build; `THIRD-PARTY.md` matches shipped clips (verified by GUID diff)
- [ ] Mixer with Music/SFX/Ambience/UI/Voice buses wired to persisted settings; zero clipping (loudness targets met)
- [ ] Dynamic music responds to combat states; every gameplay event audible & distinct (audio coverage test passed)

---

## Epic P4.E1 — Audio architecture & mixer (~2 pw)

- [ ] **P4.E1.T1** — Audio Mixer graph: buses Music/SFX/Ambience/UI + subgroups (weapons, enemies, player), exposed params `MasterVolume`, `MusicVolume`, `SFXVolume` hooked to settings (P3.E4) via `SoundManager` API; snapshots: Gameplay / Paused (duck music) / Menu
  - [ ] Test: volume sliders move exposed params; pause ducks within 100 ms and restores once (single-fire assertion)
- [ ] **P4.E1.T2** — Extend `SoundManager` per defect P0.E3.T3 + replace per-clip `AudioSource` creation with pooled emitters (align with P6 pooling)
- [ ] **P4.E1.T3** — Audio catalogue asset: every clip registered with id, bus, priority, volume trim, pitch range, cooldown (prevents stacking machine-gun overlap); SFX API takes ids, not raw clips
- [ ] **P4.E1.T4** — 2D spatial policy: panning + distance attenuation for world sounds (top-down), listener anchored to camera (existing `AudioListener` audit — ensure single listener after rig prefab extraction P0.E4.T2)

## Epic P4.E2 — Dynamic music system (~3 pw)

- [ ] **P4.E2.T1** — Layered stems system: intensity 0–4 (explore / fight / elite / boss / low-HP stress) switching via crossfade + bar-synced transitions; data in `MusicTrack` SO (stems, loop points, transition rules)
- [ ] **P4.E2.T2** — Intensity sources: subscribe to combat events (P1.E9.T2: enemies alive, damage taken, boss phase); debounce rules to prevent flapping
- [ ] **P4.E2.T3** — Boss stingers + death/victory musical pays (one-shot layer over track); mission-start intro stinger per biome
- [ ] **P4.E2.T4** — Music content: 3 biome ambient/stem packs + boss theme(s) per P0.E8.T2 replacement brief (marketplace purchase or commissioned); loop-point QC on every track

## Epic P4.E3 — SFX production & coverage (~4 pw)

- [ ] **P4.E3.T1** — Weapon SFX set: per-weapon fire/charge/reload/empty/overheat with 3+ round-robin variants + subtle pitch variation; must remain legible under fire (frequency slots documented: highs=player, lows=enemies)
- [ ] **P4.E3.T2** — Enemy SFX: per-archetype alert/attack/death (matches P1.E4 telegraphs — bomber has unmistakable warn), elite variants pitched down
- [ ] **P4.E3.T3** — Player SFX: footsteps (surface-aware via existing wall/environment queries), dash whoosh, hurt/low-HP heartbeat loop, death sting
- [ ] **P4.E3.T4** — UI SFX: hover/confirm/deny/unlock/checkbox ticks, pause whoosh; consistent 1-shots, no repeats within 50 ms
- [ ] **P4.E3.T5** — Ambience per biome: loops + random one-shots (district hum, industrial clank, vault hum); weather/room-tone beds with duck rules under music intensity ≥3
- [ ] **P4.E3.T6** — **Audio coverage audit**: checklist table event → clip id → bus → test result; every event in P1.E9.T2 has ≥1 distinct clip; report in `docs/qa/audio-coverage.md`

## Epic P4.E4 — Audio QA & settings (~2 pw)

- [ ] **P4.E4.T1** — Loudness normalization: integrated targets (e.g., gameplay ≈ −16 LUFS, peaks < −1 dBTP); sweep every shipped clip through analysis; fix outliers
- [ ] **P4.E4.T2** — Ducking/priority matrix test: 20 simultaneous SFX scenarios (full firefight + boss + UI spam) — no clipping, no dropped critical cues (player hurt always audible)
- [ ] **P4.E4.T3** — Settings integration tests: sliders persist (P6 settings service), mute when unfocused optional toggle, `AudioListener` pause behavior
- [ ] **P4.E4.T4** — Headphone + speaker check on min-spec setup + Deck speakers; report attached to M3 gate

**Phase 4 DoD:** exit criteria checked; audio coverage report green.

---

**Related:** [README](README.md) · [Audio baseline](../essential/audio.md) · [Phase 1 events](phase-01-core-combat.md) · [Phase 3 settings](phase-03-ui-ux.md)
