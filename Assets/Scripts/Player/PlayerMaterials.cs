using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class will be used to control all the behaviours related to the player's materials.
/// </summary>
public class PlayerMaterials : MonoBehaviour
{
    // --------------------------------------
    // ----- 2D Isometric Shooter Study -----
    // ----------- by Tadadosi --------------
    // --------------------------------------
    // ---- Support my work by following ----
    // ---- https://twitter.com/tadadosi ----
    // --------------------------------------

    [TextArea(4, 10)]
    public string notes = "This class will be used to control all the behaviours related to the player's materials.";

    public Material bodyMaterial;

    // P0.E3.T15 (Known Issues B.14): the dash highlight drives every consumer of
    // bodyMaterial through per-renderer property blocks, so the shared material
    // ASSET is never mutated (a play session used to persist mid-animation values
    // into mat_PlayerBody.mat — real content diff on batch quit, MT-SMOKE-01).
    private MaterialPropertyBlock _propertyBlock;
    private Renderer[] _bodyRenderers;

    private bool isHighlightBodyActive;
    private float highlightBodyIntensity;
    private float HighlightBodyTime;
    private float highlightBodyDuration;

    private void Awake()
    {
        _propertyBlock = new MaterialPropertyBlock();
        _bodyRenderers = FindBodyRenderers();
    }

    // Binds every renderer (inactive body parts included) that uses the shared body
    // material. The set is stable for the scene lifetime: nothing re-binds renderers
    // to bodyMaterial at runtime (after-images use their own mat_AfterImages_* set).
    // Scene-wide matching mirrors the old asset-wide effect — every consumer of
    // bodyMaterial flashed on dash.
    private Renderer[] FindBodyRenderers()
    {
        if (bodyMaterial == null)
            return new Renderer[0];

        List<Renderer> bound = new List<Renderer>();
        Renderer[] allRenderers = FindObjectsByType<Renderer>(FindObjectsInactive.Include, FindObjectsSortMode.None);

        for (int i = 0; i < allRenderers.Length; i++)
        {
            Renderer bodyRenderer = allRenderers[i];
            if (bodyRenderer != null && bodyRenderer.sharedMaterial == bodyMaterial)
                bound.Add(bodyRenderer);
        }

        return bound.ToArray();
    }

    private void Update()
    {
        if (isHighlightBodyActive)
        {
            HighlightBodyTime -= Time.deltaTime;

            // Reduce body color intensity from highlightBodyIntensity to 1.
            ApplyBodyIntensity((HighlightBodyTime / highlightBodyDuration) + highlightBodyIntensity);

            if (HighlightBodyTime <= 0)
            {
                // Set body color intensity back to 1
                ApplyBodyIntensity(1f);

                // Stop this behaviour
                isHighlightBodyActive = false;
            }
        }
    }

    // Applies the intensity to every bound renderer via a property block — the
    // material asset itself stays untouched (clean play sessions, warning-free).
    private void ApplyBodyIntensity(float intensity)
    {
        if (_bodyRenderers == null)
            return;

        for (int i = 0; i < _bodyRenderers.Length; i++)
        {
            Renderer bodyRenderer = _bodyRenderers[i];
            if (bodyRenderer == null)
                continue;

            bodyRenderer.GetPropertyBlock(_propertyBlock);
            _propertyBlock.SetFloat("_ColorIntensity", intensity);
            bodyRenderer.SetPropertyBlock(_propertyBlock);
        }
    }

    /// <summary>
    /// Use this class to activate the highlight body effect. This effect will set the Color Intensity of the bodyMaterial to
    /// the argument intensity and it will go back to its default value of 1 after duration.
    /// </summary>
    public void SetActiveHighlightBody(float duration, float intensity)
    {
        isHighlightBodyActive = true;
        highlightBodyIntensity = intensity;
        HighlightBodyTime = duration;
        highlightBodyDuration = duration;
    }
}
