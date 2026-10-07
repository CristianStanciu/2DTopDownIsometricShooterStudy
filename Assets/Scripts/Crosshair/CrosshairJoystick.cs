using UnityEngine;

/// <summary>
/// This class derives from the base class Crosshair. It overrides the method UpdateCrosshair 
/// to rotate this gameobject towards the joystick AimAxis direction and to store a public static Vector3 that goes from 
/// player position to crosshair world position.
/// </summary>
public class CrosshairJoystick : Crosshair
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
        "to rotate this gameobject towards the joystick AimAxis direction and to store a public static Vector3 that goes from " +
        "player position to crosshair world position.";

    private Transform player;
    private Transform pointToFollow;
    private Quaternion targetRotation;
    private Vector3 upwardAxis;

    private bool isReady;

    private const float TURN_RATE = 16f;

    /// <summary>
    /// The position of the actual crosshair in world space.
    /// </summary>
    public static Vector3 CrosshairPosition
    {
        get { return _CrosshairPosition; }
        private set { _CrosshairPosition = value; }
    }
    private static Vector3 _CrosshairPosition;
   
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
        player = FindAnyObjectByType<PlayerController>()?.transform;
        pointToFollow = FindAnyObjectByType<PlayerShoulderMain>()?.transform;
    }

    public override void UpdateCrosshair()
    {
        base.UpdateCrosshair();

        if (!isReady)
        {
            if (pointToFollow == null)
                return;
            isReady = true;
        }

        // P0.E2.T3: player may be absent (defect P0.E3.T9) — tick without NRE.
        if (player == null)
            return;

        // Multiply by AimAxis to create a vector that points towards that AimAxis Direction.
        upwardAxis = Quaternion.Euler(0, 0, 90) * TadaInput.AimAxisSmoothInput;
        targetRotation = Quaternion.LookRotation(forward: Vector3.forward, upwards: upwardAxis);

        // New Vector3 to zero out the Z axis.
        Vector3 positionToFollow = pointToFollow.position;
        positionToFollow.z = 0f;

        // Snap this gameobject position to the mainShoulder position.
        transform.position = positionToFollow;

        // Set crosshair world position.
        _CrosshairPosition = crosshair.transform.position;
        _CrosshairPosition.z = 0f;

        // Vector that goes from player to crosshair world position.
        _AimDirection = (_CrosshairPosition - player.position).normalized;
        _AimDirection.z = 0f;

        // Stop rotating if there is no AimAxis input.
        if (TadaInput.AimAxisRawInput.sqrMagnitude <= 0)
            return;

        // Smooth rotation.
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, TURN_RATE * Time.deltaTime);
    }
    /// <summary>
    /// Clears static crosshair state before each play session starts (and on app start) so a stale
    /// position/direction can't be read before the first update with domain reload disabled
    /// (P0.E4.T4, same lifecycle as TadaInput.ResetStaticState from P0.E3.T8).
    /// </summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private static void ResetStaticState()
    {
        _CrosshairPosition = Vector3.zero;
        _AimDirection = Vector3.zero;
    }

}
