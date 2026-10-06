# ADR-0001: Assembly definition layout

- **Status:** Proposed (implemented by P0.E4.T1)
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

## Consequences

Faster domain reloads and compile cycles; CI can compile/test per assembly; new code must declare
its asmdef explicitly; some cross-domain refactoring needed up front (one-time cost, P0.E4).
