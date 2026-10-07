using UnityEngine;

/// <summary>
/// This class derives from the base class Crosshair. It overrides the method UpdateCrosshair 
/// to snap the crosshair position to <see cref="TadaInput.MouseWorldPos"/>.
/// </summary>
public class CrosshairMouse : Crosshair
{
    // --------------------------------------
    // ----- 2D Isometric Shooter Study -----
    // ----------- by Tadadosi --------------
    // --------------------------------------
    // ---- Support my work by following ----
    // ---- https://twitter.com/tadadosi ----
    // --------------------------------------

    [TextArea(3, 10)]
    public string notes = "This class derives from the base class Crosshair. It overrides the method UpdateCrosshair " +
    "to snap the crosshair position to TadaInput.MouseWorldPos";

    private Transform player;

    /// <summary>
    /// Vector that goes from player position to crosshair world position.
    /// </summary>
    public static Vector3 AimDirection
    {
        get { return _AimDirection; }
        private set { _AimDirection = value; }
    }
    private static Vector3 _AimDirection;

    protected override void Awake()
    {
        base.Awake();
        player = SceneAnchors.Player;
    }

    public override void UpdateCrosshair()
    {
        base.UpdateCrosshair();

        // P0.E4.T1: SceneAnchors registrations happen in other Awakes - resolve lazily until then.
        if (player == null)
            player = SceneAnchors.Player;

        // P0.E2.T3: player may be absent (defect P0.E3.T9) — tick without NRE.
        if (player == null)
            return;

        crosshair.transform.position = TadaInput.MouseWorldPos;

        _AimDirection = (crosshair.transform.position - player.position).normalized;
        _AimDirection.z = 0f;
    }
    /// <summary>
    /// Clears the static aim direction before each play session starts (and on app start) so a
    /// stale direction can't be read before the first crosshair update with domain reload
    /// disabled (P0.E4.T4, same lifecycle as TadaInput.ResetStaticState from P0.E3.T8).
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStaticState()
    {
        _AimDirection = Vector3.zero;
    }

}
