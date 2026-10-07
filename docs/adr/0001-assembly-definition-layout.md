# ADR-0001: Assembly definition layout

- **Status:** Accepted — implemented by P0.E4.T1 (2026-10-07)
- **Date:** 2026-10-05
- **Related tasks:** P0.E4.T1, P0.E5.T2
- **Deciders:** Project owner

## Context

The project compiles as a single `Assembly-CSharp`, so any script change recompiles everything —
slow iteration and no enforceable dependency direction between domains (player, weapons, audio, UI).

## Decision

We will split scripts into domain asmdefs: `Game.Core` (utilities, global managers, inputs, game
flow), `Game.Player`, `Game.Weapons`, `Game.Audio`, `Game.Camera`, `Game.Environment`, `Game.UI`,
`Game.EditorTools`, plus `Tests.EditMode` and `Tests.PlayMode` referencing the game assemblies.
Dependencies point inward to `Game.Core`; no cycles allowed. Rejected: status quo (fast to start,
scales poorly) and a single `Game.All` asmdef (no dependency enforcement).

## As-built (P0.E4.T1, 2026-10-07)

Nine game asmdefs + two test asmdefs, verified with a batchmode import on a throwaway clone
(exit 0, zero `error CS`/`warning CS`, dependency graph acyclic):

| Assembly | Scope (`Assets/…`) | References |
|---|---|---|
| `Game.Core` | **default root assembly** — everything under `Scripts/` not claimed below: `Animations`, `FX`, `Global Managers and Controllers`, `Inputs`, `Physics`, `Utilities` | — |
| `Game.Player` | `Scripts/Player` | Core, Weapons, Audio, Environment, UI |
| `Game.Weapons` | `Scripts/Weapons` | Core, Audio, Camera |
| `Game.Audio` | `Scripts/Audio` | Core |
| `Game.Camera` | `Scripts/Camera` | Core, UI |
| `Game.Environment` | `Scripts/Environment` | Core |
| `Game.UI` | `Scripts/Crosshair` | Core |
| `Game.DebugTools` | `Scripts/Debug` | Core, UI |
| `Game.EditorTools` | `Editor/` (Editor-only, empty until P0.E5.T4) | — |
| `Tests.EditMode` | `Tests/EditMode` (Editor-only) | Core + test runners |
| `Tests.PlayMode` | `Tests/PlayMode` (empty until P0.E7.T2) | Core + test runner |

Two deviations from the original proposal, both forced by the edge audit:

1. **`Game.DebugTools` added.** `Debug/GizmosHandler` reads `CrosshairJoystick.CrosshairPosition`;
   without its own asmdef the `Debug` folder would fall into Core → a Core→UI edge.
2. **Two cycle cuts.** `Crosshair → Player` (3 `FindAnyObjectByType` lookups) now resolves through
   the Core `SceneAnchors` registry; `Weapons → Player` (`Projectile` reading
   `PlayerBodyPartsHandler.isRightDirection`) now reads Core `FacingState`, with the original
   static kept as a delegating facade. A third candidate (`Weapons → Camera` via `CameraShake`)
   is acyclic as-built (`Weapons → Camera → UI → Core`) and was left untouched.
   `Animations → Weapons` looked like a cycle but is a false positive: `Animation` is a nested
   enum; `AnimationHandler` binds `UnityEngine.Animation`.

## Consequences

Faster domain reloads and compile cycles; CI can compile/test per assembly; new code must declare
its asmdef explicitly; some cross-domain refactoring needed up front (one-time cost, P0.E4).
Consumers outside Core must never reference Core-registered scene objects by their concrete
component type — register `Transform`s in `SceneAnchors` (or the future `SceneContext`, P0.E4.T3)
instead.
