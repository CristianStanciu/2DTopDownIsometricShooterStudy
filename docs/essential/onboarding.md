# Onboarding — New Contributor Quickstart

[Index](index.md) · [Getting Started](getting-started.md) · [Contributing](../../CONTRIBUTING.md) · [Warning policy](../qa/warning-policy.md)

One page for everything you must know before your first PR (P0.E9.T4): the engine pin,
CI secrets, branch rules and how tests run. Detail lives in the linked pages — this is
the ordered checklist.

## 1. Prerequisites

- **Unity 6000.3.20f1** via Unity Hub — exact version, read from
  `ProjectSettings/ProjectVersion.txt`. Upgrades happen only via a dedicated PR with the
  full regression suite ([Getting Started](getting-started.md#requirements)).
- **Git** (the repo does not use Git LFS — decision in ADR-0004).
- An IDE with Unity support (Visual Studio via the committed `.vsconfig` workloads, or
  Rider/VS Code).

## 2. Clone & first open

```bash
git clone https://github.com/CristianStanciu/2DTopDownIsometricShooterStudy.git
```

Add `my-game/` in Unity Hub (first open regenerates `Library/` — a few minutes,
git-ignored), open **`Assets/Scenes/Level_00.unity`**, press Play. Full walkthrough:
[Getting Started](getting-started.md).

## 3. CI secrets (one-time, repository admin)

`ci.yml` (tests) and `build.yml` (Windows build) license the pinned Unity editor inside
the cloud runner using the same three secrets — **Settings → Secrets and variables →
Actions → New repository secret**:

| Secret | Contents |
|---|---|
| `UNITY_LICENSE` | The personal-license `.ulf` file contents (one-time activation) |
| `UNITY_EMAIL` | Unity account email used for license validation |
| `UNITY_PASSWORD` | Unity account password — alphanumeric only (special characters are known to break game-ci) |

Obtain the `.ulf` once via game-ci's activation flow:
<https://game.ci/docs/github/activation>. Until these exist, every CI run fails at the
license step (see `docs/essential/getting-started.md` and the workflow header comments).

## 4. Branch rules

- **`main`** — protected, always releasable; merges only via PR with green CI.
- **`develop`** — integration branch and default PR target.
- Feature branches: `feat/<roadmap-task-id>-<slug>`, bugfixes `fix/<id>-<slug>`
  (e.g. `fix/P0.E3.T10-camera-deltatime`); one task per PR.
- PRs use the template (**Refs** = roadmap task IDs, **test evidence** mandatory, docs +
  `CHANGELOG.md` updated); CODEOWNERS + `CONTRIBUTING.md` hold the full contract.

> **Status (2026-10-06):** branch-protection enforcement is pending `gh auth login` and
> the first green `ci.yml` run (P0.E1.T3 / P0.E6.T4). Until then, run the self-review
> checklist from the PR template instead of requesting a reviewer.

## 5. How to run tests

**On CI** — `ci.yml` runs on every PR and push to `develop`/`main`:
import · compile · **EditMode tests** in one batchmode session (game-ci
`unity-test-runner@v4`, editor pinned `6000.3.20f1`), results attached as an artifact +
check run. Two gates fail the run independently:

1. **Warning gate** — any `warning CS…`/`error CS…` line breaks the warning-zero policy
   ([`docs/qa/warning-policy.md`](../qa/warning-policy.md)).
2. **Restore gate** — `Packages/packages-lock.json` drifting during import means
   nondeterministic package restore (P0.E2.T7).

**Locally** — `Window → General → Test Runner → EditMode → Run All`, or headless:

```bash
Unity.exe -batchmode -projectPath <repo> -runTests -testPlatform editmode -logFile - 
```

> **Status:** test assemblies shipped with **P0.E7.T1** — EditMode currently runs 24 tests
> (`TadaInputTests`, `ArraysHandlerTests`); PlayMode smoke tests land with **P0.E7.T2**.
> Gameplay verification meanwhile remains the manual smoke case **`MT-SMOKE-01`**
> ([`docs/qa/smoke.md`](../qa/smoke.md); the automated subset runs headless on a fresh clone).

## 6. Before you push

- Closing Unity leaves `git status` clean (LF pinned by `.gitattributes` — if you see
  phantom EOL diffs, stop and check P0.E1.T1 notes).
- Compile is warning-free per the [warning policy](../qa/warning-policy.md).
- `CHANGELOG.md` entry added; affected `docs/essential/` pages updated (DoD rule,
  roadmap §5).
- Commit message references the roadmap task ID (repo history style:
  `type(P0.E…T…): summary`).

## What to read next

[Getting Started](getting-started.md) (controls, layout) ·
[Code Architecture](architecture.md) (conventions) ·
[Known Issues & Roadmap](known-issues-and-roadmap.md) ·
[Master Roadmap](../roadmap/README.md) ·
[Contributing](../../CONTRIBUTING.md)
