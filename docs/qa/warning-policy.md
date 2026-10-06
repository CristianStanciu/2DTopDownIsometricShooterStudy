# Warning-zero policy & catalogue (P0.E2.T4)

**Status:** adopted 2026-10-06 · **Editor:** Unity 6000.3.20f1 (pinned)
**Enforced by:** `.github/workflows/ci.yml` → step *Warning gate* (P0.E6.T1)

## Policy

1. Project code (`Assets/**` → Assembly-CSharp) compiles with **zero `warning CS` and zero
   `error CS` lines** in every batchmode run on the pinned editor.
2. **CI fails** when any scanned Unity log line matches `(warning|error) CS[0-9]+`
   (case-insensitive). New diagnostics are regressions regardless of severity.
3. Play sessions emit **zero warnings/errors from project code**: a project-source
   `Debug.LogWarning/LogError` in a smoke run is a **FAIL**; editor-internal exceptions are
   classified **WARN** by stack trace (see `docs/qa/smoke.md`).
4. Every warning is either **fixed** or **suppressed with a reason** — never left open:
   ```csharp
   #pragma warning disable CS0414 // assigned but never used — kept for future tuning (P0.E4.T6)
   ```
   on the narrowest possible scope. File/assembly-wide suppressions and `#warning` are
   forbidden. Any *new* warning class gets a catalogue row in the same PR that introduces it.

## Catalogue — every warning class observed on 6000.3.20f1

| # | Class | Source | Evidence (2026-10-05/06) | Disposition |
|---|-------|--------|--------------------------|-------------|
| 1 | `warning CS\d+` | Roslyn diagnostics from project code (batchmode compile) | **0 hits** across 4 logs: full fresh-clone import (1.1 MB), MT-SMOKE-01 play, 2× round-trip open→save→close | **Gated** — any hit fails CI |
| 2 | `error CS\d+` | Roslyn compile errors | **0 hits** (same 4 logs) | **Gated** — any hit fails CI |
| 3 | CS0618 (obsolete API) | Project code: 13 `FindObjectOfType` call sites | Eliminated by P0.E2.T2; 0 since | **Gated** (recurrence) |
| 4 | Unity scaffold `/nowarn:0169,0649,0282,1701,1702` | Unity's own csc invocation for engine/package sources — 0169 unused private field, 0649 serialized-not-assigned field, 0282 generic-attribute quirk, 1701/1702 assembly-version unification | Unity defaults, confirmed in generated compile args | **Accepted baseline** (engine-internal, not project code); re-verified on editor upgrades (P0.E1.T4) |
| 5 | Project runtime `Debug.LogWarning/LogError` | Game code during play | **0** in MT-SMOKE-01 play log | **Gated by smoke** — project-source = FAIL |
| 6 | Editor-internal exception | `UnityEditor.Search` → `ArgumentOutOfRangeException` (stock editor, triggered by the smoke probe's Search usage) | 1 in MT-SMOKE-01 | **WARN-classified** — stack contains no project assemblies; tracked in `docs/qa/smoke.md`; becomes FAIL if it reproduces with project code on the stack |
| 7 | Asset-pipeline / package import messages | Unity import (URP, packages, textures) | No warning-level lines in the full import log (only asset *filenames* containing "Warning") | **Monitored, not gated** — would be noise today; revisit if a real class appears |

## The CI gate

`ci.yml` → *Warning gate* greps `unity-editor.log` (passed via
`customParameters: -logFile`) plus `editmode-results/**/*.log` and fails on a match.
If no log file is reachable the step emits `::warning::` (visible, non-failing) instead of
passing silently — check the plumbing on the first run.

Local equivalent (PowerShell, same evidence the catalogue was built from):

```powershell
& "C:\Program Files\Unity\Hub\Editor\6000.3.20f1\Editor\Unity.exe" -batchmode -quit `
  -projectPath . -logFile compile.log   # then poll the process, don't block a 30 s shell
Select-String -Path compile.log -Pattern 'warning CS|error CS'
```

## Known limitations / future hardening

- The gate sees only logs that land in the workspace; a game-ci plumbing change could
  redirect them — the `::warning::` fallback makes that visible, not silent.
- Warnings *inside* package sources are out of scope (Unity compiles them under its own
  `/nowarn` scaffold) — covered later by P0.E5.T2 analyzer enablement.
- Play-mode warning gating at scale (audio, façades, gamepad paths) lands with the nightly
  PlayMode smoke (P0.E6.T2) and the test suite (P0.E7).

**Related:** [smoke record](smoke.md) · [roadmap](../roadmap/phase-00-foundation.md) (E2.T4, E6.T1) · [known issues](../essential/known-issues-and-roadmap.md)
