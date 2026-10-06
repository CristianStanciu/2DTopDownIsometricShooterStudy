# Known Issues & Roadmap Foundation

[Index](index.md) · [Architecture](architecture.md) · [Project Settings](project-settings.md)

Everything in this page was **verified against the code/scenes/settings on `develop`**
(commit `c9d5f0e`). It is organized as: (A) Unity 6 upgrade status, (B) verified defects & tech
debt with file references, (C) capability gaps, (D) candidate roadmap themes.

---

## A. Unity 6 upgrade status (commit `c9d5f0e`, "chore: upgrade project to Unity v6.3")

**What the commit changed** (from git diff inspection):

| Area | Change |
|---|---|
| `ProjectVersion.txt` | `2019.4.0f1` → `6000.3.20f1` (direct jump across ~7 major versions) |
| `Physics2DHandler.cs` | `Rigidbody2D.velocity` → **`linearVelocity`** (Unity 6 rename) — the *only* code fix |
| `Level_00.unity` | fully re-serialized (~4,315 lines changed) |
| `TagManager.asset` | serializedVersion 2 → 3, `m_RenderingLayers` block added |
| URP assets, materials, shader graphs | re-serialized to URP 17 formats |
| `manifest.json` | packages bumped (URP 17.3.0, 2D packages 13.x/12.x, `com.unity.ugui 2.0.0`; old `com.unity.textmeshpro` removed — merged into uGUI) |
| New settings assets | `UniversalRenderPipelineGlobalSettings`, `MemorySettings`, `MultiplayerManager`, `SceneTemplateSettings`, `ShaderGraphSettings`, `VersionControlSettings`, `DefaultVolumeProfile`, `PackageManagerSettings` |
| Repo additions | `.vsconfig`, `my-game.slnx`, `packages-lock.json` regenerated |
| `UserSettings/` | committed (layouts for 2021/2022/2023/6000 + `Search.index`) |

**Outstanding upgrade risks (verified):**

1. **`SingleWall_Test.unity` was never re-serialized** — *resolved: re-serialized by **P0.E2.T1***
   (2026-10-05, batchmode open→save→reopen; census identical before/after, 0 missing scripts,
   +800/−207 lines; `SingleWall_TestSettings.lighting` extracted on save).
2. **Deprecated API warnings**: `FindObjectOfType<T>()` is obsolete in Unity 6 (should be
   `FindAnyObjectByType`/`FindFirstObjectByType`). Occurrences (13 call sites in 9 scripts):
   - `Audio/SoundManager.cs:37,43,49`
   - `Crosshair/CrosshairHandler.cs:26,27`
   - `Crosshair/CrosshairJoystick.cs:55,56` (note: `FindObjectOfType<PlayerController>().transform`
     — no null guard, NRE if player missing)
   - `Crosshair/CrosshairMouse.cs:36` (same NRE risk)
   - `FX/AfterImage/AfterImageGenerator.cs:36`, `FX/AfterImage/AfterImageGroup.cs:14`
   - `Player/PlayerBodyPartsHandler.cs:50`, `Player/PlayerSkills.cs:20`
   - `Utilities/Singleton.cs:33` (inside `Instance` getter)
   - `_NOTES/JustNotes.cs:14` (comment only)
3. **README is stale**: still says "made with Unity 2019.4.0f1" and v0.2 while
   `bundleVersion` is 0.1 and the editor is 6000.3.20f1.
4. **No play-mode validation was recorded** — the upgrade commit contains no evidence of a test run
   (no tests exist in the project at all).
5. First open with any other 6.x patch will re-serialize again — recommend pinning 6000.3.20f1
   for the team.

---

## B. Verified defects & tech debt

### Code defects

| # | Location | Issue |
|---|---|---|
| 1 | `WeaponHandler.CheckWeaponsAvailability()` | Doc comment says it will "break the execution", but it only `return`s from itself — callers proceed and can hit NRE on `weapons[i]` / `currentWeapon`. |
| 2 | `PlayerSkills.SetActiveTrails()` | Null-checks `dashTrails != null` (the array) inside the loop instead of `dashTrails[i] != null`. |
| 3 | `SoundManager.SFX_PlayOneShot/SFX_PlayDelayedOneShot` | Guard checks `music != null` instead of `sfx != null` (wrong emitter null-check). |
| 4 | `PlayerController` | Reads `Input.GetKeyDown(KeyCode.Alpha1/Alpha2)` directly — bypasses `TadaInput` (breaks abstraction & controller parity for slot switching). |
| 5 | `Weapon_ShootProjectileCanCharge` | Author's NOTE: charging `CameraShake` overrides the release shake; needs a re-use cooldown timer. |
| 6 | `Weapon_ChargeContinuousShooting.OnChargeShooting()` | Empty override — dead class still on disk. |
| 7 | `PlayerPhysics.OnTriggerExit2D()` | Empty stub — wall façades never restore on exit, only on next enter. |
| 8 | `TadaInput` | Duplicate `StoreCurrentKeyUp(NextUseRate)` block (lines ~261–265); statics never reset on play-stop (stale `GetKeyDown` can leak across sessions); `MouseAnyMovement` enum member never stored. |
| 9 | `CrosshairMouse`/`CrosshairJoystick` | `FindObjectOfType<PlayerController>().transform` — unguarded, NRE if player absent. |
| 10 | `CameraBehaviour.FixedUpdate` | Uses `Time.deltaTime` for Lerp smoothing inside `FixedUpdate` (frame-rate dependent). |
| 11 | `GUIHandler` overlay text | Says `2: Weapon2` but key `2` maps to weapon **index 1** (cosmetic misinformation). |
| 12 | `EditorBuildSettings` | Leftover disabled scene entry with empty path. |
| 13 | `EnvironmentObjectTrigger.SetActiveObjects()` | No null-guard on the arrays themselves (elements are guarded). |

### Structural debt

- **Duplicated scene rig**: both scenes carry identical copies of Player/Camera/Audio/Crosshair
  roots; any wiring change must be made twice → strong candidate for shared prefab(s).
- **`FindObjectOfType` startup coupling** (13 sites, listed in A.2) — hidden dependencies that
  also generate Unity 6 warnings.
- **Static facades everywhere** (`TadaInput`, `CameraShake`, `PauseController.isGamePaused`,
  `CrosshairMouse/Joystick.AimDirection`, `PlayerBodyPartsHandler.isRightDirection`) — no reset
  lifecycle, hard to unit-test.
- **No assembly definitions** — everything recompiles in one assembly; slow iteration as the
  project grows; no way to isolate editor/test code.
- **No tests** despite `com.unity.test-framework 1.6.0` in the manifest.
- **`UserSettings/` + generated IDE files committed** (`.sln`, `.csproj`, `.slnx`) — repo noise;
  standard Unity `.gitignore` normally excludes them (only `Library/`, `Temp/`, `Logs/` are covered).
- **Legacy Input Manager** — Xbox-only axis names; no rebinding; incompatible with console/mobile.
- **Legacy `Animation`** (no Animator) — no blending/layering/state machine.
- **`_NOTES/JustNotes.cs`** scratch class ships in the build assembly.

---

## C. Capability gaps (nothing exists yet)

No enemies/AI · no damage/health · no scoring/UI (no Canvas, no TextMeshPro usage) · no game loop
(waves/missions/win-lose) · no save/settings · no pause menu (only `UnityEvent` seams) · no
object pooling (code comments anticipate it: `Projectile`, after-images) · no audio mixer/volume
settings · no localization · no control rebinding · no analytics/telemetry · no CI/CD · no build
pipeline beyond the single editor scene list.

---

## D. Candidate roadmap themes (derived from A–C)

Suggested epics; each maps back to verified items above:

1. **E1 — Unity 6 hardening**: finish re-serialization (`SingleWall_Test`), replace 13
   `FindObjectOfType` sites, fix warnings, pin editor version, refresh README, validate Play mode
   (A.1–A.5, B.8–B.9).
2. **E2 — Defect burn-down**: fix B.1–B.7, B.10–B.13 (small, well-scoped stories, good sprint filler).
3. **E3 — Content architecture**: extract Player/Camera/Audio/Crosshair roots into prefabs;
   single scene-loading bootstrap; add `.asmdef` layering (duplicated-rig + structural debt).
4. **E4 — Core loop**: enemies + damage + health, projectiles hit actors (needs tag/collision-matrix
   design on layers 8/9), scoring — turns the study into a game (C).
5. **E5 — UX shell**: UI canvas, pause menu wired to existing `PauseController.onGamePause/Resume`
   events, settings (audio volumes via `SoundManager` API), control hints (C + B.8).
6. **E6 — Input modernization**: migrate `TadaInput` to `com.unity.inputsystem` behind the same
   static API to avoid touching consumers (A.2, legacy-input debt).
7. **E7 — Performance/pooling**: projectile + particle + after-image pooling (explicit code
   comments ask for it), profiling on URP (C pooling).
8. **E8 — Quality infrastructure**: Play-mode smoke test, EditMode tests for `ArraysHandler`/
   `TadaInput` state machine, CI on 6000.3.20f1 (tests/CI debt).

---

## Related pages

- [Architecture](architecture.md) · [Scene Inventory](scenes.md) ·
  [Project Settings](project-settings.md) · [Rendering](rendering.md)

