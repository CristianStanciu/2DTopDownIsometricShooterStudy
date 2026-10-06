# ADR-0002: Structured logging shim

- **Status:** Proposed (implemented by P0.E5.T3)
- **Date:** 2026-10-05
- **Related tasks:** P0.E5.T3, P0.E5.T2
- **Deciders:** Project owner

## Context

The codebase logs through raw `Debug.Log`/`LogError` with inconsistent formats, which cannot be
level-filtered, tagged, or stripped from release builds; support/log triage at launch (P9/P10)
needs machine-parseable output.

## Decision

We will add a thin `Log.Info/Warn/Error(tag, message, ctx)` shim wrapping Unity's logger, with a
tag convention, compile-time level filtering, and stripping of verbose levels in release builds.
No third-party logging framework (rejected: extra dependency for a small game). Migrate the noisiest
scripts first, opportunistically.

## Consequences

Consistent, greppable logs; release builds quieter and marginally faster; one wrapper API to learn;
migration is incremental — mixed styles remain until scripts are touched.
