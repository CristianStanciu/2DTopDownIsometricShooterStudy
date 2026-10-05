# Rendering & Post-Processing

[Index](index.md) · [Assets Pipeline](assets.md) · [Project Settings](project-settings.md)

## Pipeline assets

| Asset | Path | Role |
|---|---|---|
| URP asset | `Assets/Rendering/Forward_Low_NoLights.asset` | default quality pipeline (GraphicsSettings `m_CustomRenderPipeline` → guid `fd70f05e59dd5794ea7b0fc50fa3c908`) |
| Renderer data | `Assets/Rendering/UniversalRenderPipelineAsset_Renderer.asset` | forward renderer referenced by the URP asset's `m_RendererDataList` |
| Global settings | `Assets/UniversalRenderPipelineGlobalSettings.asset` | URP 17 global shader/settings object (**added by the Unity 6 upgrade**) |

`Forward_Low_NoLights` configuration (verified): **HDR on**, **MSAA off** (`m_MSAA: 1`),
render scale 1, main light rendering mode 0 (disabled — the game is unlit/vertex-lit 2D),
shadow distance 50, color grading mode 0 (LDR). The name is literal: a forward, low-quality,
no-lights configuration appropriate for the sprite-based look.

URP package version: **17.3.0** (`com.unity.render-pipelines.universal`).

## Camera settings (`Level_00` Main Camera)

| Property | Value |
|---|---|
| Projection | **Orthographic**, size **6.306675** |
| Background | Solid color `RGB(0.1333, 0.0604, 0.2170)` (dark purple), clear flags = solid |
| HDR | on (`m_HDR: 1`) |
| MSAA | off (`m_AllowMSAA: 0`) |
| Depth | −1 |
| Clipping | near 0.3, far 1000 |
| Culling mask | Everything |
| Position | child of `CameraPivot` (root object with `CameraBehaviour`/`CameraShake`) |

The exact orthographic size (6.306675) is a hand-tuned value — note it when adding a
pixel-perfect camera (the `com.unity.2d.pixel-perfect` package is installed but **unused**).

## Post-processing (Volume)

- Scene object root: **`Post Processing`** in both scenes, carrying a URP `Volume`
  (global, using `Assets/Scenes/Level_00/Post Processing Volume.asset` as its profile).
- Profile overrides (verified): **Vignette**, **Film Grain**, **Shadows/Midtones/Highlights** —
  the sci-fi grade described by the original README ("cool sci-fi aesthetic").
- `Assets/DefaultVolumeProfile.asset` also exists (empty/default — created by the Unity 6 upgrade).
- The camera has the URP additional-camera-data component (serialized as a stripped/built-in
  script reference in the scene).

## Shaders (Shader Graph)

`Assets/Shaders/` contains five custom **URP Shader Graph** shaders (all Unlit, HDR-aware —
sprites are authored with emissive glow channels):

```
Unlit_Sprite_HDR.shadergraph
Unlit_Sprite_HDR_ColorIntensity.shadergraph
Unlit_Sprite_NoTexColor_JustAlphaAndHDRColor.shadergraph
Unlit_Tex_Alpha_ColorHDR_TwoSided.shadergraph
Unlit_Tex_Alpha_VertexColorHDR_TwoSided.shadergraph
```

They were re-serialized by the Unity 6 upgrade (shader graph data model changed). Materials using
them: wall color/glow, player body, projectiles, weapon energy, after-images, particles
(see [Assets Pipeline](assets.md#materials-assetsmaterials)).

## Quality settings

`ProjectSettings/QualitySettings.asset`: **6 levels** — Very Low, Low, Medium, High, Very High, Ultra.
`m_CurrentQuality: 3` → **High** is the editor default. Quality→pipeline mapping is managed by
Unity's per-level render pipeline entries; the single URP asset above is the active one.

## Sorting & layers

- One sorting layer: **Default** (walls/players rely on sprite order + sorting orders).
- Tags/layers used by rendering: layers `Player` (8) and `Projectile` (9); see
  [Project Settings](project-settings.md#tags-and-layers-tagmanagerasset).
- `Rendering Layers` entries (`Default`, `Light Layer 1-7`) were added by the Unity 6 upgrade's
  TagManager migration (serializedVersion 3) — unused by content.

## Upgrade notes (rendering)

The commit `c9d5f0e` re-serialized: URP asset (~96 lines), renderer (40), materials, shader
graphs, and added `UniversalRenderPipelineGlobalSettings`, `MemorySettings`, `ShaderGraphSettings`.
Expect further re-serialization diffs on first editor open with a slightly different Unity 6.x
patch. Details: [Known Issues & Roadmap Foundation](known-issues-and-roadmap.md).

## Related pages

- [Assets Pipeline](assets.md) — the materials/textures feeding these shaders.
- [Project Settings](project-settings.md#graphics-graphicssettingsasset) — GraphicsSettings wiring.
- [Scene Inventory](scenes.md) — `Post Processing` root placement.
