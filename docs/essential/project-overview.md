# Project Overview

[Index](index.md) · [Getting Started](getting-started.md) · [Architecture](architecture.md)

## What this is

A playable **2D top-down isometric shooter prototype with a sci-fi/cyber aesthetic**, originally
created by *Tadadosi* as a public learning resource ("2D Isometric Shooter Study v0.2", built with
Unity 2019.4.0f1 after ~19h on v0.1 + ~70h on v0.2). This repository is a fork that has upgraded
the project to **Unity 6000.3.20f1** (upgrade performed in commit `c9d5f0e`, *"chore: upgrade
project to Unity v6.3"*).

The prototype contains a fully playable player controller with movement, dash, aim rig and body
animations, **two laser weapons** (rapid-fire + charge-to-shoot) with projectiles, a wall-occlusion
environment system, dash after-image FX, a follow/offset camera with shake, a hand-rolled input
manager, dual-architecture audio, and URP post-processing. It contains **no enemies, no UI canvas,
no game loop/win conditions and no tests** — it is a systems study, not a complete game.

## Repository provenance

| Remote | URL |
|---|---|
| `origin` | `https://github.com/CristianStanciu/2DTopDownIsometricShooterStudy.git` |
| `upstream` | `https://github.com/tadadosii/2DTopDownIsometricShooterStudy.git` |

Branch: `develop`. The README in the repo root is the original upstream README (v0.2) and still
references Unity 2019.4.0f1 — see [Known Issues](known-issues-and-roadmap.md).

## Technology stack

- **Unity 6000.3.20f1** (revision `c9ba695d4f07`), 2D project, **Gamma** color space.
- **Universal Render Pipeline 17.3.0** — default asset `Assets/Rendering/Forward_Low_NoLights.asset`
  with renderer `Assets/Rendering/UniversalRenderPipelineAsset_Renderer.asset`; HDR on, MSAA off.
- **Legacy Input Manager** (no `com.unity.inputsystem` package) wrapped by the custom `TadaInput`
  static class; extra axes `HorizontalAim`, `VerticalAim`, `Xbox360LeftTrigger`,
  `Xbox360RightTrigger` were added to `ProjectSettings/InputManager.asset`.
- **No assembly definitions** — all ~50 scripts compile into the single default `Assembly-CSharp`.
- No third-party assets beyond Unity packages: `com.unity.2d.animation 13.0.5`,
  `2d.pixel-perfect 5.1.1`, `2d.psdimporter 12.0.2`, `2d.sprite`, `2d.spriteshape 13.0.0`,
  `2d.tilemap`, `ai.navigation 2.0.13`, `collab-proxy 2.12.4`, IDE packages (Rider/VS/VS Code),
  `multiplayer.center`, `test-framework 1.6.0` (unused — there are no tests),
  `timeline 1.8.12` (unused), `ugui 2.0.0`.

## Feature inventory (what actually works in Play mode)

- **Movement**: WASD/left-stick 8-direction movement with diagonal clamping, driven through
  `PlayerPhysics` (Rigidbody2D velocity set in `FixedUpdate`).
- **Dash**: 0.2s impulse dash with trails, body-highlight flash, after-images and SFX.
- **Aim**: two crosshairs (mouse + joystick) auto-switching via `TadaInput.IsMouseActive`;
  shoulder/arm rig aimed with the `LookAt2Dv2` family; legacy `Animation` clips for walk/aim poses.
- **Weapons**: two `Weapon_ShootProjectileCanCharge` instances —
  **LaserRifle_Auto-Charge** (slot 0) and **LaserBeam_Rifle_Body** (slot 1) — with primary
  rapid fire, secondary 2-second charge shot, switchable fire rates and weapon switching.
- **Projectiles**: pooled-by-reuse (per-weapon) projectile objects, `Transform.Translate` travel,
  wall-only trigger collision with impact particle + sound.
- **Environment**: 166 wall prefab instances in `Level_00` that swap façades depending on which
  side the player stands on, driven by `EnvironmentObject` + `EnvironmentObjectTrigger` + tag
  `EnvironmentTrigger`.
- **Audio**: global `SoundManager` singleton (created SFX/unstoppable/music emitters), per-object
  handlers, music playback on start.
- **Camera**: smooth follow with crosshair-based offset, static `CameraShake.Shake(...)`.
- **Pause**: Esc/Start toggles `Time.timeScale` + `AudioListener.pause` + UnityEvents.
- **Debug**: OnGUI control-scheme overlay (`GUIHandler`) and gizmo toggles (`GizmosHandler`).

## License and credits

- **MIT License, Copyright (c) 2020 Tadadosi** (`LICENSE` in repo root). Per the README, the MIT
  license applies **only to the code tagged "by Tadadosi" and the Unity project setup — it does not
  include the sprites and audio files**; pixel-art sprites are free for personal use.
- Third-party code credits: `CameraShake` by ftvs (GitHub gist), `Singleton` pattern by
  UnityCommunity (MIT).
- Audio credits (Freesound / Free Music Archive): Emanuele Correani, StormwaveAudio, ST303, dpren
  (CC0 / CC-BY-3.0), music "Azimutez" by Sci Fi Industries (CC BY-NC-SA 3.0). Filenames embed the
  license tags — see [Assets Pipeline](assets.md).

## Related pages

- [Getting Started](getting-started.md) — run/controls guide.
- [Scene Inventory](scenes.md) — exact content of both scenes.
- [Known Issues & Roadmap Foundation](known-issues-and-roadmap.md) — upgrade state and backlog seeds.
