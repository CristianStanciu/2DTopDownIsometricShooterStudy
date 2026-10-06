# ADR-0003: Test policy

- **Status:** Proposed (implemented by P0.E7)
- **Date:** 2026-10-05
- **Related tasks:** P0.E7.T1–T4, P0.E6.T1, P0.E4.T5
- **Deciders:** Project owner

## Context

The project has zero automated tests. CI (P0.E6) must gate merges, and the release definition
(roadmap §2) requires all tests green — but a solo/small team cannot afford heavyweight coverage
targets or slow suites.

## Decision

We will test at three levels: **EditMode** for pure logic (input consumption, state machines,
tuning registry, save parsing — target ≥10 tests by end of P0); **PlayMode** for a thin combat
smoke (spawn → fire → despawn) and scene-validation tests; **manual cases** (`MT-*` IDs in
`docs/qa/`) for feel/visual/accessibility checks that resist automation. Policy: no phase may
reduce critical-path coverage; PRs that fix a bug include a regression test when feasible.
Rejected: full TDD mandate (too rigid for juice-heavy work) and no-tests-until-M4 (too late).

## Consequences

CI runtime grows modestly; bug fixes become cheap to verify; some gameplay code will need minor
refactoring to be testable (accepted, done in P0.E4).
