# Getting Started

[Index](index.md) · [Project Overview](project-overview.md) · [Architecture](architecture.md)

## Requirements

- **Unity 6000.3.20f1** (exact version — `ProjectSettings/ProjectVersion.txt`). Opening with a
  different editor will trigger a re-serialization of scene/asset files; avoid committing those
  diffs unless the upgrade is intentional (see [Known Issues](known-issues-and-roadmap.md)).
- Any desktop platform supported by Unity 6. The project targets the **editor**: there are no
  platform-specific build configs beyond defaults.

## Opening the project

1. Clone both remotes if you plan to contribute upstream:
   `origin` → `CristianStanciu/2DTopDownIsometricShooterStudy`, `upstream` → `tadadosii/2DTopDownIsometricShooterStudy`.
2. Add the project folder (`my-game/`) via Unity Hub; Unity will regenerate `Library/` on first open
   (takes a few minutes; `Library/` is git-ignored).
3. Open scene **`Assets/Scenes/Level_00.unity`** — it is the only scene enabled in
   `EditorBuildSettings` (a second, disabled, empty-path entry exists as a leftover).
4. Press **Play**. The `GUIHandler` OnGUI overlay shows live aim vectors and the control scheme.

> `Assets/Scenes/SingleWall_Test.unity` is a minimal test scene (one wall + one trigger + player +
> camera rig) used for iterating on wall occlusion; it is not in the build.

## Controls (verbatim from `GUIHandler` overlay)

**Keyboard + mouse**

```
WASD: Move | Mouse: Aim | Space: Dash | 1: Weapon0 | 2: Weapon2
LMB: Act1 | RMB: Act2 | Q/E: SwitchWpn | C/V: SwitchFireRate | Esc: Pause
```

*(Note: the overlay text says "2: Weapon2" but the code maps key `2` to weapon index 1; also
weapon switching works with Q/E and the mouse scroll wheel.)*

**Gamepad (Xbox layout, joystick #1)**

```
LStick: Move | RStick: Aim | RB: Dash | RTrigger: Action1 | LTrigger: Action2
A/B: SwitchWeapon | LStickButton: SwitchFireRate | Start: Pause
```

Exact key mapping lives in `Assets/Scripts/Inputs/TadaInput.cs` — see [Input System](input.md).

## Repository layout (repo root)

```
my-game/
├── Assets/               # everything game-facing (see Assets Pipeline)
├── docs/essential/       # this documentation
├── Packages/             # manifest.json (URP 17.3.0, 2D packages, etc.)
├── ProjectSettings/      # tags, layers, input, physics, quality, build scenes
├── UserSettings/         # per-user editor state (git-ignored; untracked in P0.E1.T1)
├── .editorconfig         # C# style rules
├── .gitignore            # standard Unity ignore (Library/, Temp/, Logs/…)
├── .vsconfig             # Visual Studio workload list (added by Unity 6 upgrade)
├── Assembly-CSharp.csproj / my-game.sln / my-game.slnx   # generated IDE files
├── LICENSE               # MIT (c) 2020 Tadadosi
└── README.md             # original upstream README (v0.2, references Unity 2019.4)
```

## First things to try in Play mode

1. Move with WASD, aim with the mouse — the camera offsets toward the crosshair.
2. Hold **RMB** to charge the secondary shot (2s), release to fire the big projectile; **LMB**
   rapid-fires at the current fire rate; **C/V** switches between `{0.125s, 0.05s}` use rates.
3. **Space** dashes (only while moving) — watch trails + after-images.
4. Walk behind a wall: the wall swaps its façade so the player stays visible.
5. **Esc** pauses (time scale 0 + audio paused); UnityEvents on `PauseController` are the hook
   point for a future pause menu.

## Conventions

- Code style is enforced by `.editorconfig`; naming/structure conventions are described in
  [Architecture](architecture.md#coding-conventions).
- Commit style observed in history: Conventional-Commits-ish (`chore: upgrade project to Unity v6.3`).

## Related pages

- [Code Architecture](architecture.md) — how the scripts fit together.
- [Scene Inventory](scenes.md) — what's inside each scene.
- [Project Settings](project-settings.md) — configuration reference.
