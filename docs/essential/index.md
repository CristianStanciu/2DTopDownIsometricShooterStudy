# Developer Documentation — 2D Top-Down Isometric Shooter Study (Unity 6.3)

This directory is the canonical developer documentation for the project. It documents the
codebase as it exists on the `develop` branch after the Unity 6.3 upgrade
(commit `c9d5f0e`), and is intended to be exhaustive enough to derive a product roadmap
(epics, stories, technical debt) directly from it.

**Project at a glance**

| Item | Value |
|---|---|
| Engine | Unity `6000.3.20f1` (upgraded from `2019.4.0f1`) |
| Render pipeline | URP `17.3.0`, forward low-quality renderer (`Forward_Low_NoLights`) |
| Input | Legacy Input Manager (custom `TadaInput` wrapper) |
| Scripts | ~50 C# files, single default assembly (`Assembly-CSharp`, no `.asmdef`) |
| Scenes | `Assets/Scenes/Level_00.unity` (main, in build), `Assets/Scenes/SingleWall_Test.unity` (test, not in build) |
| License | MIT (code + project setup only — see [Project Overview](project-overview.md#license-and-credits)) |

---

## Table of contents

### Foundations
1. [Project Overview](project-overview.md) — what the project is, provenance, package stack, license and credits.
2. [Getting Started](getting-started.md) — opening the project, running it, controls, and where everything lives.
3. [Onboarding](onboarding.md) — new-contributor quickstart: engine pin, CI secrets, branch rules, how tests run.
4. [Code Architecture](architecture.md) — folder layout, inheritance map, singleton/patterns, coding conventions.

### Gameplay
5. [Player System](player.md) — the 9 player classes, movement, dash skill, body parts, aim rig.
6. [Weapons & Projectiles](weapons-and-projectiles.md) — `Weapon` base class, both laser weapons, charge state machine, projectile lifecycle.

### Core systems
7. [Input System](input.md) — legacy Input Manager, `TadaInput` static API, custom axes, key bindings, mouse/gamepad duality.
8. [Audio System](audio.md) — the dual architecture: global `SoundManager` vs local/global sound handlers, emitters, music, pause interaction.
9. [Camera & Crosshair](camera-and-crosshair.md) — follow/offset camera, camera shake, static API, and the two-crosshair switching system.
10. [Environment, FX & Pause](environment-and-fx.md) — wall façade switching, `EnvironmentObject`/`Trigger`, dash after-images, `PauseController`, debug tooling.

### Content & configuration
11. [Scene Inventory](scenes.md) — root objects, hierarchies, prefab instance counts, per-scene component census for both scenes.
12. [Rendering & Post-Processing](rendering.md) — URP assets, camera settings, volume overrides, shader graph shaders, quality levels.
13. [Assets Pipeline](assets.md) — sprites, audio clips, materials, prefabs, particles, animations and naming conventions.
14. [Project Settings](project-settings.md) — tags, layers, input axes, physics 2D, quality, audio manager, build scenes.

### Engineering status
15. [Known Issues & Roadmap Foundation](known-issues-and-roadmap.md) — Unity 6 upgrade artifacts, deprecated API usage, latent bugs, and candidate backlog themes.

**→ [Master Development Roadmap](../roadmap/README.md)** (outside this directory) — the full phase-by-phase development plan derived from these docs: epics, tasks with `- [ ]` status tracking, acceptance criteria, dependencies, effort estimates and release gates.

---

## How to use these docs

- **New to the project?** Start with [Onboarding](onboarding.md) (setup checklist: engine
  pin, CI secrets, branch rules, tests), then [Getting Started](getting-started.md) and
  [Code Architecture](architecture.md).
- **Working on gameplay?** Start from [Player System](player.md) and [Weapons & Projectiles](weapons-and-projectiles.md).
- **Planning work?** [Known Issues & Roadmap Foundation](known-issues-and-roadmap.md) lists verified defects and
  modernization gaps, each mapped to suggested backlog themes.
- Every page links back to this index and cross-links related pages; all links are relative
  to `docs/essential/`.
