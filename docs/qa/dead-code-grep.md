# Dead-code grep report — P0.E4.T7

Date: 2026-10-06 · Tree: `develop` @ `137b157` + working-tree changes below.
Scope: `Assets/Scripts/**/*.cs` (49 files before the removal, 48 after).

## 1. Removed — `Assets/Scripts/_NOTES/JustNotes.cs`

- **What it was**: author scratchpad `MonoBehaviour` — v0.2 work-time log, personal TODOs,
  early convention reminders. Compiled into `Assembly-CSharp`, referenced by nothing.
- **Reference proof**: GUID `cc673a1d335ac474b9d6e9a7041682a5` (from `JustNotes.cs.meta`)
  scanned across every non-`.meta` file under `Assets/` → **0 references** (no scene,
  prefab, asset, or script uses it) → deletion cannot break a scene or the build.
- **Also removed**: `JustNotes.cs.meta`, `_NOTES.meta` (folder meta) — `_NOTES/` no longer exists.
- **Content preserved verbatim** in [`docs/notes/author-notes.md`](../notes/author-notes.md)
  (provenance: work-time log feeds the project-credits record).
- Content-based reference check: `_NOTES|JustNotes` matched only documentation pages
  (`architecture.md`, `known-issues-and-roadmap.md`, `review-report.md`, roadmap) — all
  updated by this task except the historical `review-report.md` F-12 (intentionally frozen).

## 2. Empty method bodies

| Scan | Result |
|---|---|
| Multi-line bodies (signature line → `{` → only whitespace/comments → `}`) | **0 findings** |
| Inline bodies `) { }` | **2 findings — both intentional** (table below) |

| Location | Verdict |
|---|---|
| `Audio/SoundManager.cs:27` — `protected SoundManager() { }` | Singleton pattern constructor: intentionally empty (prevents external instantiation). **Keep.** |
| `Crosshair/Crosshair.cs:25` — `public virtual void UpdateCrosshair() { }` | Virtual base no-op; concrete subclasses override. **Keep.** |

**Historical empty stubs — already accounted for in P0:**

- `Weapon_ChargeContinuousShooting.OnChargeShooting()` — class deleted by **P0.E3.T6**.
- `PlayerPhysics.OnTriggerExit2D()` — implemented by **P0.E3.T7** (mirrors the enter logic
  and restores wall façades; live body verified during this scan).

## 3. `TODO` / `FIXME` / `STUB` / `HACK` markers

Count before removal: **2** → after removal: **1**.

- ~~`_NOTES/JustNotes.cs:2`~~ — archived with the file.
- `Inputs/TadaInput.cs:297` — `// TODO: Improve mouseScrollDelta input conditions.`
  — live note on shipped input code; intentionally kept (not dead code).

## 4. Verdict

No dead classes (last one removed by P0.E3.T6), no unjustified empty stubs, no dangling
GUID references after the `_NOTES/` removal. The only remaining marker is a live input TODO.
Reproduction: any tree-sitter/regex pass over `Assets/Scripts` with the patterns above
(`\)\s*\{\s*\}` for inline; signature→brace→empty-body for multi-line; `guid:` scan for refs).
