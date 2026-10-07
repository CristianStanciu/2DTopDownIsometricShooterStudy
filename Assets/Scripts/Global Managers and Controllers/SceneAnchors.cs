using UnityEngine;

/// <summary>
/// Scene-wide transform registry filled by the owning components during Awake. View-side code
/// (crosshair, debug gizmos) resolves the player through this instead of referencing Game.Player
/// types, keeping assemblies acyclic (ADR-0001 no cycles, P0.E4.T1). Interim solution until the
/// SceneContext bootstrap lands (P0.E4.T3).
/// </summary>
public static class SceneAnchors
{
    // --------------------------------------
    // - 2D TopDown Isometric Shooter Study -
    // ----------- by Tadadosi --------------
    // --------------------------------------

    public static Transform Player { get { return _Player; } private set { _Player = value; } }
    private static Transform _Player;

    public static Transform MainShoulder { get { return _MainShoulder; } private set { _MainShoulder = value; } }
    private static Transform _MainShoulder;

    public static void RegisterPlayer(Transform player)
    {
        Player = player;
    }

    public static void RegisterMainShoulder(Transform shoulder)
    {
        MainShoulder = shoulder;
    }

    /// <summary>
    /// Clears registrations before each play session starts (and on app start) so references to
    /// destroyed scene objects can't leak across sessions with domain reload disabled
    /// (P0.E4.T4, same lifecycle as TadaInput.ResetStaticState from P0.E3.T8).
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStaticState()
    {
        Player = null;
        MainShoulder = null;
    }
}
