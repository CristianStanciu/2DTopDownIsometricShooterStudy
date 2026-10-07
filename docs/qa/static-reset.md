# Static-Facade Reset Verifier — P0.E4.T4 (5× Cycle)

Roadmap: [P0.E4.T4](../roadmap/phase-00-foundation.md) · [Known Issues](../essential/known-issues-and-roadmap.md)

Evidence for the static-facade lifecycle (P0.E4.T4, building on `TadaInput` from
P0.E3.T8): **enter/exit play mode 5× leaves no stale `GetKeyDown` state**.

## Method

Temporary probe `Assets/Editor/QaStaticReset.cs` in a **fresh-clone workdir only**
(same technique as `smoke.md`'s `QaPlaySmoke`; never committed):

1. In **edit mode**, dirty every targeted static (fresh `DIRTY` line each cycle):
   `TadaInput` key state (a `GetKey` hold, a `GetKeyDown` edge, a `GetKeyUp` edge,
   axis vectors, `IsMouseActive`), `PauseController.isGamePaused = true`,
   `Time.timeScale = 0`, `AudioListener.pause = true`, `CameraShake.Shake(99f, …)`,
   crosshair aim vectors, `isRightDirection = false`.
2. Enter play (`Level_00`) with **domain reload disabled**
   (`EnterPlayModeOptions.DisableDomainReload`) — the scenario where a missing
   reset actually leaks; with reload enabled the run would pass vacuously.
3. At frame ≥10 assert every static is back to its fresh state. Every dirtied value
   is non-default, so only a `SubsystemRegistration` reset can restore it (scene
   load itself does not).
4. Exit play, **re-dirty everything**, repeat — 5 independent cycles.

Any logged exception fails the run (`FAIL`), with the pre-agreed carve-out for the
editor-internal `UnityEditor.Search` exception (counted `WARN`).

## Result — 2026-10-07 — GREEN

- Environment: fresh `git clone` of `develop`, Unity **6000.3.20f1**, Windows,
  `BatchMode -nographics`, scene `Assets/Scenes/Level_00.unity`.
- `cycles: 5/5` · `PASS=50` · `FAIL=0` · `WARN=1` (editor-internal) · editor exit code **0**.

| Check (dirtied → asserted at frame 10) | c1–c5 |
|---|---|
| `TadaInput.GetKey(MoveUp)` held → false | PASS ×5 |
| `TadaInput.GetKeyDown(Dash)` edge → false | PASS ×5 |
| `TadaInput.GetKeyUp(Pause)` edge → false | PASS ×5 |
| `TadaInput.MoveAxisRawInput` (1,1) → zero | PASS ×5 |
| `TadaInput.IsMouseActive` true → false | PASS ×5 |
| `PauseController.isGamePaused` true → false | PASS ×5 |
| `Time.timeScale` 0 → 1 | PASS ×5 |
| `AudioListener.pause` true → false | PASS ×5 |
| `CameraShake.isActive` true → false | PASS ×5 |
| `CameraShake.shakeDuration` 99 → 0 | PASS ×5 |
| `PlayerBodyPartsHandler.isRightDirection` false → true | INFO ×5¹ |

¹ `Awake()` also forces `true`, so this check is non-discriminating on its own
(recorded `INFO`, not `PASS`).

## Notes

- The strongest input assertion is `GetKey` (non-consuming): nothing releases
  `MoveUp` in headless batch, so a stale hold-state would persist forever if the
  reset didn't run. `GetKeyDown`/`GetKeyUp` are consuming — a stale edge still
  returns `true` → `FAIL` on the spot.
- `Time.timeScale` / `AudioListener.pause` are weaker tripwires (Unity may restore
  them itself); `isGamePaused`, shake state, and the input checks are the
  discriminating set.
- After the run the clone's `ProjectSettings/` was restored via scoped
  `git checkout -- ProjectSettings` → `git status` clean (clone hygiene).

## Run log (verbatim)

```text
== P0.E4.T4 sub-test: 5x enter/exit play, no stale state (QaStaticReset, clone)
editor: 6000.3.20f1
date(UTC): 2026-10-07 08:07
cycles: 5/5
counts: PASS=50 FAIL=0 WARN=1
result: GREEN
-- log --
INFO| scene Assets/Scenes/Level_00.unity open; domain reload DISABLED
DIRTY| cycle 1: pause timeScale audio shake crosshairs bodyDir=false input[MoveUp held, Dash down, Pause up, axis=(1,1), mouse=true]
WARN| editor-internal: ArgumentOutOfRangeException: Index was out of range. Must be non-negative and less than the size of the collection. / Parameter name: index
PASS| c1: input GetKey(MoveUp) == false (was held)
PASS| c1: input GetKeyDown(Dash) == false (was edge)
PASS| c1: input GetKeyUp(Pause) == false (was edge)
PASS| c1: input MoveAxisRawInput == 0 (was 1,1)
PASS| c1: input IsMouseActive == false (was true)
PASS| c1: pause: isGamePaused == false (was true)
PASS| c1: timeScale == 1 (was 0)
PASS| c1: audio: AudioListener.pause == false (was true)
PASS| c1: shake isActive == false (was true)
PASS| c1: shake duration == 0 (was 99)
INFO| c1 bodyDir == True (Awake also forces true — non-discriminating)
DIRTY| cycle 2: pause timeScale audio shake crosshairs bodyDir=false input[MoveUp held, Dash down, Pause up, axis=(1,1), mouse=true]
PASS| c2: input GetKey(MoveUp) == false (was held)
PASS| c2: input GetKeyDown(Dash) == false (was edge)
PASS| c2: input GetKeyUp(Pause) == false (was edge)
PASS| c2: input MoveAxisRawInput == 0 (was 1,1)
PASS| c2: input IsMouseActive == false (was true)
PASS| c2: pause: isGamePaused == false (was true)
PASS| c2: timeScale == 1 (was 0)
PASS| c2: audio: AudioListener.pause == false (was true)
PASS| c2: shake isActive == false (was true)
PASS| c2: shake duration == 0 (was 99)
INFO| c2 bodyDir == True (Awake also forces true — non-discriminating)
DIRTY| cycle 3: pause timeScale audio shake crosshairs bodyDir=false input[MoveUp held, Dash down, Pause up, axis=(1,1), mouse=true]
PASS| c3: input GetKey(MoveUp) == false (was held)
PASS| c3: input GetKeyDown(Dash) == false (was edge)
PASS| c3: input GetKeyUp(Pause) == false (was edge)
PASS| c3: input MoveAxisRawInput == 0 (was 1,1)
PASS| c3: input IsMouseActive == false (was true)
PASS| c3: pause: isGamePaused == false (was true)
PASS| c3: timeScale == 1 (was 0)
PASS| c3: audio: AudioListener.pause == false (was true)
PASS| c3: shake isActive == false (was true)
PASS| c3: shake duration == 0 (was 99)
INFO| c3 bodyDir == True (Awake also forces true — non-discriminating)
DIRTY| cycle 4: pause timeScale audio shake crosshairs bodyDir=false input[MoveUp held, Dash down, Pause up, axis=(1,1), mouse=true]
PASS| c4: input GetKey(MoveUp) == false (was held)
PASS| c4: input GetKeyDown(Dash) == false (was edge)
PASS| c4: input GetKeyUp(Pause) == false (was edge)
PASS| c4: input MoveAxisRawInput == 0 (was 1,1)
PASS| c4: input IsMouseActive == false (was true)
PASS| c4: pause: isGamePaused == false (was true)
PASS| c4: timeScale == 1 (was 0)
PASS| c4: audio: AudioListener.pause == false (was true)
PASS| c4: shake isActive == false (was true)
PASS| c4: shake duration == 0 (was 99)
INFO| c4 bodyDir == True (Awake also forces true — non-discriminating)
DIRTY| cycle 5: pause timeScale audio shake crosshairs bodyDir=false input[MoveUp held, Dash down, Pause up, axis=(1,1), mouse=true]
PASS| c5: input GetKey(MoveUp) == false (was held)
PASS| c5: input GetKeyDown(Dash) == false (was edge)
PASS| c5: input GetKeyUp(Pause) == false (was edge)
PASS| c5: input MoveAxisRawInput == 0 (was 1,1)
PASS| c5: input IsMouseActive == false (was true)
PASS| c5: pause: isGamePaused == false (was true)
PASS| c5: timeScale == 1 (was 0)
PASS| c5: audio: AudioListener.pause == false (was true)
PASS| c5: shake isActive == false (was true)
PASS| c5: shake duration == 0 (was 99)
INFO| c5 bodyDir == True (Awake also forces true — non-discriminating)
```

