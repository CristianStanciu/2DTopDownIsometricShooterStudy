# Contributing

Short guide for anyone touching this repo (full process: `docs/roadmap/README.md` §5).

## Branch model

- `main` — protected; always releasable; only merged via PR with green CI.
- `develop` — integration branch; default PR target.
- `feat/P0.E1.T1-short-slug` — feature branches, named after the roadmap task ID they implement.
  Bugfix branches: `fix/P0.E3.T10-camera-deltatime`.

## Pull requests

1. Use the PR template; reference the roadmap task ID(s) in **Refs**.
2. One task (or tightly coupled task group) per PR — keeps review and revert cheap.
3. **Test evidence is mandatory**: unit/play test output, a manual case ID (`MT-*`), or compiler
   output for hygiene changes.
4. Update `docs/essential/` pages affected by the change, and add a `CHANGELOG.md` entry.
5. Merge only when CI (`ci.yml`) is green (branch protection; while solo, run the self-review
   checklist in the PR template instead of requesting a reviewer).

## Environment rules

- Editor is pinned to **Unity 6000.3.20f1** — upgrades only via dedicated PR with full regression
  (see `docs/essential/getting-started.md`).
- Do not commit `UserSettings/`, `*.sln(x)`, `*.csproj`, `Library/` — all git-ignored (P0.E1.T1).
- Every merged PR adds an entry to `CHANGELOG.md` (Keep a Changelog format).
