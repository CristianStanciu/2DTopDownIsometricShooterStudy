# Changelog

All notable changes to this project are documented in this file.
Format based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/).
Every merged PR adds an entry under `[Unreleased]` (rule: `CONTRIBUTING.md`).

## [Unreleased]

### Added

- Full documentation suite: `docs/essential/` (15 pages) + `docs/roadmap/` (12 pages),
  QA review report with 17 findings (all fixed) — roadmap **APPROVED v1.2** (`docs/review-report.md`)
- ADR process and starter decisions ADR-0001…0003 (`docs/adr/`) — P0.E9.T3
- Git LFS evaluation: not adopted, revisit triggers recorded (ADR-0004) — P0.E1.T2
- Contribution flow: PR template, CODEOWNERS, `CONTRIBUTING.md` — P0.E1.T3
- This changelog (Keep a Changelog) — P0.E1.T5

### Changed

- Project upgraded from Unity 2019.4.0f1 to **6000.3.20f1** (commit `c9d5f0e`), URP 17.3.0
- `.gitignore` hardened: `UserSettings/` (8 files) and `my-game.slnx` untracked, `*.slnx` ignored —
  P0.E1.T1
- `getting-started.md`: editor upgrade policy (dedicated PR + full regression) — P0.E1.T4

### Fixed

- 17 documentation defects from the QA review (details: `docs/review-report.md`)
