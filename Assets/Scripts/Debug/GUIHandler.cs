using UnityEngine;

/// <summary>
/// Basic class to draw messages on the screen.
/// </summary>
public class GUIHandler : MonoBehaviour
{
    // --------------------------------------
    // ----- 2D Isometric Shooter Study -----
    // ----------- by Tadadosi --------------
    // --------------------------------------
    // ---- Support my work by following ----
    // ---- https://twitter.com/tadadosi ----
    // --------------------------------------

    [TextArea(1, 10)]
    public string notes = "Basic class to draw messages on the screen.";

    [SerializeField] private bool drawGUI = true;

    // Single source of truth for the overlay control hints (P0.E3.T11) — the displayed text is
    // generated from these tables so it can't drift away from the actual bindings.
    private static readonly (string input, string action)[] MouseBindings =
    {
        ("WASD", "Move"),
        ("Mouse", "Aim"),
        ("Space", "Dash"),
        ("1", "Weapon0"),
        ("2", "Weapon1"),
        ("LMB", "Act1"),
        ("RMB", "Act2"),
        ("Q/E", "SwitchWpn"),
        ("C/V", "SwitchFireRate"),
        ("Esc", "Pause")
    };

    private static readonly (string input, string action)[] GamepadBindings =
    {
        ("LStick", "Move"),
        ("RStick", "Aim"),
        ("RB", "Dash"),
        ("RTrigger", "Action1"),
        ("LTrigger", "Action2"),
        ("A/B", "SwitchWeapon"),
        ("X", "Weapon0"),
        ("Y", "Weapon1"),
        ("LStickButton", "SwitchFireRate"),
        ("Start", "Pause")
    };

    private static string FormatBindings((string input, string action)[] bindings)
    {
        string result = string.Empty;
        for (int i = 0; i < bindings.Length; i++)
        {
            if (i > 0)
                result += " | ";
            result += bindings[i].input + ": " + bindings[i].action;
        }
        return result;
    }

    private void OnGUI()
    {
        if (drawGUI)
        {
            GUIStyle style = new GUIStyle
            {
                fontSize = Screen.height / 40,
                alignment = TextAnchor.MiddleCenter
            };

            // Not exactly sure of how can I pass this inside the GUIStyle information or if it's actually possible to do it...
            style.normal.textColor = Color.yellow;
            
            string s0 = $"PlayerPos->MousePos: {CrosshairMouse.AimDirection.x:0.0} , {CrosshairMouse.AimDirection.y:0.0}";
            string s1 = $"PlayerPos->CrosshairJoyDir: {CrosshairJoystick.AimDirection.x:0.0} , {CrosshairJoystick.AimDirection.y:0.0}";
            string s2 = FormatBindings(MouseBindings);
            string s3 = FormatBindings(GamepadBindings);
            string s4 = "";

            if (TadaInput.IsMouseActive)
                s4 = s2;
            else
                s4 = s3;

            GUI.Label(new Rect(Screen.width / 2f, Screen.height - Screen.height / 10f, 0, 0), s0, style);
            GUI.Label(new Rect(Screen.width / 2f, Screen.height - Screen.height / 15f, 0, 0), s1, style);
            GUI.Label(new Rect(Screen.width / 2f, Screen.height - Screen.height / 30f, 0, 0), s4, style);
        }
    }
}
