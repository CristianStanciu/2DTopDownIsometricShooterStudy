# ADR-0004: Git LFS policy — not adopted (yet)

- **Status:** Accepted
- **Date:** 2026-10-05
- **Related tasks:** P0.E1.T2
- **Deciders:** Project owner

## Context

Roadmap P0.E1.T2 requires evaluating Git LFS for `Assets/Sprites/**` and `Assets/Audio/**` when
the repository exceeds 500 MB. Measured 2026-10-05: packed history **13.68 MiB**, Assets working
tree **18.8 MB** (Sprites 1.0 MB, Audio 15.4 MB, remainder — prefabs/scenes/meta). Binaries are
tiny relative to the threshold; the project is a 2D game with no 4K video/texture churn yet.

## Decision

We will **not** adopt Git LFS now. LFS adds clone friction (bandwidth quotas on free tiers,
extra toolchain on every machine) for no measurable gain at this size. Triggers to revisit
(re-evaluate within one week of crossing either):
- repository (packed) > 500 MB, or
- any single tracked binary > 50 MB, or
- binary churn > ~100 MB/month from audio/PFX/vector exports (expected around P6–P7 content work).

Migration procedure if triggered: `.gitattributes` patterns for `Assets/Sprites/**` and
`Assets/Audio/**`, `git lfs migrate import` with **history rewrite optional** (default: forward
only — rewriting published history breaks upstream sync with `tadadosii/2DTopDownIsometricShooterStudy`).

## Consequences

Zero tooling overhead today; one explicit re-check point at content-heavy phases; if triggered
mid-project, contributors need `git lfs` installed (documented in getting-started at that time).
