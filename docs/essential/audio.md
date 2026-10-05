# Audio System

[Index](index.md) · [Architecture](architecture.md) · [Environment, FX & Pause](environment-and-fx.md)

## Overview — two parallel architectures

The project deliberately keeps **two ways to play sound** (explained in the original README):

1. **Global**: the `SoundManager` singleton with scene-spawned emitters — for one-shot SFX/music
   routed from anywhere (`SoundHandlerGlobal`).
2. **Local**: `SoundHandlerLocal` components that own an `AudioSource` **on their GameObject** and
   play from a serialized `Sound[]` array — for object-specific sounds (weapon charge, projectiles).

```
SoundEmitter (abstract base, [RequireComponent(AudioSource)], public Source property)
├── SoundFXEmitter            — plain one-shot SFX source (playOnAwake=false, loop=false)
├── UnstoppableSoundEmitter   — sounds that "cannot be stopped" (shots, impacts)
├── MusicEmitter              — looping music source (loop=true)
└── SoundHandlerLocal         — inspector-driven Sound[] player (loop/playOnStart/playAllSounds flags)

SoundManager : Singleton<SoundManager>  — static facade over the emitters
SoundHandlerGlobal : MonoBehaviour      — Sound[] wrapper that calls SoundManager.Instance
Sound                                 — [Serializable] clip + volume + min/max pitch
MusicHandler : MonoBehaviour           — plays MusicHandler.musicClips[] via SoundManager
```

## `SoundManager` (global singleton)

- On `Awake()` (after `base.Awake()`), it creates child emitters **if the scene doesn't already
  contain them** (`FindObjectOfType<SoundFXEmitter/UnstoppableSoundEmitter/MusicEmitter>()`):
  `"SoundFXs"`, `"UnstoppableSoundFXs"`, `"Music"` GameObjects parented under the manager.
  (`Level_00` has **0** emitter instances serialized — they're all spawned at runtime.)
- API:

| Method | Behavior |
|---|---|
| `SFX_PlayOneShot(clip, volume, minPitch = 1, maxPitch = 1, unstoppable = false)` | Randomizes `Source.pitch`, `PlayOneShot`. Routes to unstoppable source when requested. |
| `SFX_PlayDelayedOneShot(..., delay)` | Same via coroutine `WaitForSeconds(delay)`. |
| `SFX_SetVolume(value)` / `SFX_Stop()` | Control the normal SFX source only. |
| `Music_PlayOneShot(clip, volume)` / `Music_Play(clip)` | One-shot or `clip`+`Play()` on the looping music source. |
| `Music_SetVolume(value)` | Music source volume. |

- **Quirk**: the SFX methods guard with `if (clip != null && music != null)` — they check the
  *music* emitter instead of the *sfx* emitter (latent bug if music creation failed; harmless in
  practice since all three are created together). Listed in [Known Issues](known-issues-and-roadmap.md).

## `Sound` (serializable data)

`notes`, `clip`, `[Range] _Volume = 1`, `[Range] _MinPitch = 1`, `[Range] _MaxPitch = 1` — pitch is
randomized between min/max on every play (variation without extra clips).

## `SoundHandlerLocal` (per-object)

Serialized `Sound[] sounds`, `isLoop`, `isPlayOnStart`, `isPlayAllSounds`.
`Start()` applies loop, runs `CheckIfReady()` (`LogError` + `Debug.Break()` on empty/missing slots)
and optionally auto-plays. `PlaySound(index = 0)` plays slot 0 (or all slots) as `PlayOneShot`
with random pitch; overload accepts explicit clip/volume/pitch. `StopSound()` stops the source.

Used in scenes: **2 instances per scene** — the two weapons' `chargingSFX` (charge loop + cancel
via `StopSound()`) and `Projectile` components (each projectile prefab carries one; `Fire()` plays
slot 0).

## `SoundHandlerGlobal` (bridge to the singleton)

Serialized `Sound[] sounds`, `playOnStart`, `playAllSounds`, `isUnstoppable`. `PlaySound(index, unstoppable)`
forwards each sound to `SoundManager.Instance.SFX_PlayOneShot(...)`; `StopSound()` calls
`SoundManager.Instance.SFX_Stop()`. Scene usage: **1 instance** — the dash SFX wired into
`PlayerSkills.dashSFXHandler`.

## `MusicHandler`

Root object **`MusicHandler`** exists in both scenes (enabled). Serialized `volume = 0.65`,
`musicClips[]`, `playOnAwake = true` → `Start()` plays clip 0 through
`SoundManager.Instance.Music_SetVolume(volume)` + `Music_Play(clip)`.

Music asset: `Assets/Audio/Music/Sci_Fi_Industries_-_01_-_Azimutez_CC_BY_NC_SA_3.mp3`
(CC BY-NC-SA 3.0 — see [Assets Pipeline](assets.md#audio-assetsaudio)).

## Pause interaction

`PauseController.OnGamePause/Resume()` sets `AudioListener.pause = true/false`, silencing **all**
sources during pause regardless of which handler owns them. The `UnstoppableSoundEmitter` name
refers to `SFX_Stop()` not stopping it — not to pause immunity.

## Scene census (verified)

| Object/Component | Level_00 | SingleWall_Test |
|---|---|---|
| `SoundManager` (root, singleton) | 1 | 1 |
| `SoundHandlerGlobal` | 1 | 1 |
| `SoundHandlerLocal` | 2 (weapons) | 2 (weapons) |
| `MusicHandler` (root, enabled) | 1 | 1 |
| `SoundFXEmitter` / `UnstoppableSoundEmitter` / `MusicEmitter` serialized | 0 (runtime-spawned) | 0 |
| `SFX` object | child of Player rig | child of Player rig |

## Audio configuration

`ProjectSettings/AudioManager.asset`: master volume 1, speaker mode Stereo, DSP buffer 1024,
512 virtual / 32 real voices, virtualize effects on. See
[Project Settings](project-settings.md#audio-audiomanagerasset).

## Known gaps (audio)

- No AudioMixer, buses, snapshots, or volume settings UI.
- No 3D/spatialized sounds (all 2D one-shots).
- `SoundManager` music-guard quirk (above); `FindObjectOfType` usage (deprecated in Unity 6).
- Static `SoundManager.Instance` persists via `DontDestroyOnLoad` while scenes reload — emitters
  are only created once (by design, but worth knowing when adding scenes).

## Related pages

- [Environment, FX & Pause](environment-and-fx.md) — pause/audio listener behavior.
- [Project Settings](project-settings.md#audio-audiomanagerasset) — manager defaults.
- [Known Issues](known-issues-and-roadmap.md) — defect list.
