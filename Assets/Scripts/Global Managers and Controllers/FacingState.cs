using UnityEngine;

/// <summary>
/// Player body-facing state owned by Core so Game.Weapons can read it without referencing
/// Game.Player (ADR-0001 no cycles, P0.E4.T1). PlayerBodyPartsHandler.isRightDirection stays
/// the public facade and delegates here, so existing readers, docs and QA tripwires keep working.
/// </summary>
public static class FacingState
{
    // --------------------------------------
    // - 2D TopDown Isometric Shooter Study -
    // ----------- by Tadadosi --------------
    // --------------------------------------

    /// <summary>
    /// True while the body is pointing right; Projectile uses it to pick travel direction.
    /// </summary>
    public static bool IsRightDirection { get { return _IsRightDirection; } set { _IsRightDirection = value; } }
    private static bool _IsRightDirection;

    /// <summary>
    /// Restores the facing default before each play session starts (and on app start) so a stale
    /// direction can't leak across sessions with domain reload disabled (P0.E4.T4, same lifecycle
    /// as TadaInput.ResetStaticState from P0.E3.T8). PlayerBodyPartsHandler also resets through
    /// the facade in its own ResetStaticState - both run before the first scene load.
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStaticState()
    {
        _IsRightDirection = true;
    }
}
