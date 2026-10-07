using System.Reflection;
using NUnit.Framework;
using UnityEngine;

/// <summary>
/// EditMode coverage for the TadaInput static input facade (P0.E7.T1). Store/Reset members are
/// private statics by design (they mirror Unity's Input polling lifecycle), so tests drive them
/// through reflection exactly like TadaInput.Update does internally.
/// </summary>
public class TadaInputTests
{
    private static readonly BindingFlags STATIC_MEMBERS = BindingFlags.NonPublic | BindingFlags.Static;

    [SetUp]
    public void SetUp()
    {
        // Mirror app start / SubsystemRegistration so every test runs against clean static state.
        InvokeStatic("ResetStaticState");
    }

    private static void InvokeStatic(string methodName, params object[] arguments)
    {
        MethodInfo method = typeof(TadaInput).GetMethod(methodName, STATIC_MEMBERS);
        Assert.IsNotNull(method, "TadaInput is missing private static method: " + methodName);
        method.Invoke(null, arguments);
    }

    private static void SetStaticField(string fieldName, object value)
    {
        FieldInfo field = typeof(TadaInput).GetField(fieldName, STATIC_MEMBERS);
        Assert.IsNotNull(field, "TadaInput is missing private static field: " + fieldName);
        field.SetValue(null, value);
    }

    [Test]
    public void AllDefinedKeys_ReportNotPressed_AfterReset()
    {
        // Skips index 0 (None) which is the sentinel every slot is initialized with.
        for (int i = 1; i <= (int)TadaInput.ThisKey.Count; i++)
        {
            TadaInput.ThisKey key = (TadaInput.ThisKey)i;
            Assert.IsFalse(TadaInput.GetKey(key), "GetKey(" + key + ")");
            Assert.IsFalse(TadaInput.GetKeyDown(key), "GetKeyDown(" + key + ")");
            Assert.IsFalse(TadaInput.GetKeyUp(key), "GetKeyUp(" + key + ")");
        }
    }

    [Test]
    public void GetKey_RemainsTrue_WhileKeyHeld()
    {
        InvokeStatic("StoreCurrentKey", TadaInput.ThisKey.Dash);

        Assert.IsTrue(TadaInput.GetKey(TadaInput.ThisKey.Dash));
        Assert.IsTrue(TadaInput.GetKey(TadaInput.ThisKey.Dash));
        Assert.IsFalse(TadaInput.GetKeyDown(TadaInput.ThisKey.Dash));
        Assert.IsFalse(TadaInput.GetKeyUp(TadaInput.ThisKey.Dash));
    }

    [Test]
    public void GetKeyDown_FiresOnce_ThenIsConsumed()
    {
        InvokeStatic("StoreCurrentKeyDown", TadaInput.ThisKey.Dash);

        Assert.IsTrue(TadaInput.GetKeyDown(TadaInput.ThisKey.Dash));
        Assert.IsFalse(TadaInput.GetKeyDown(TadaInput.ThisKey.Dash));
        Assert.IsFalse(TadaInput.GetKeyDown(TadaInput.ThisKey.Dash));
    }

    [Test]
    public void GetKeyUp_FiresOnce_ThenIsConsumed()
    {
        InvokeStatic("StoreCurrentKeyUp", TadaInput.ThisKey.Dash);

        Assert.IsTrue(TadaInput.GetKeyUp(TadaInput.ThisKey.Dash));
        Assert.IsFalse(TadaInput.GetKeyUp(TadaInput.ThisKey.Dash));
    }

    [Test]
    public void StoreCurrentKeyUp_ClearsHeldAndDownState()
    {
        InvokeStatic("StoreCurrentKeyDown", TadaInput.ThisKey.Dash);
        InvokeStatic("StoreCurrentKey", TadaInput.ThisKey.Dash);
        Assert.IsTrue(TadaInput.GetKey(TadaInput.ThisKey.Dash));

        InvokeStatic("StoreCurrentKeyUp", TadaInput.ThisKey.Dash);

        Assert.IsFalse(TadaInput.GetKey(TadaInput.ThisKey.Dash));
        Assert.IsFalse(TadaInput.GetKeyDown(TadaInput.ThisKey.Dash));
        Assert.IsTrue(TadaInput.GetKeyUp(TadaInput.ThisKey.Dash));
        Assert.IsFalse(TadaInput.GetKeyUp(TadaInput.ThisKey.Dash));
    }

    [Test]
    public void StoreCurrentKeyUp_OnlyAffectsItsOwnKey()
    {
        InvokeStatic("StoreCurrentKey", TadaInput.ThisKey.Dash);
        InvokeStatic("StoreCurrentKey", TadaInput.ThisKey.Pause);

        InvokeStatic("StoreCurrentKeyUp", TadaInput.ThisKey.Pause);

        Assert.IsTrue(TadaInput.GetKey(TadaInput.ThisKey.Dash));
        Assert.IsFalse(TadaInput.GetKey(TadaInput.ThisKey.Pause));
    }

    [Test]
    public void ResetStaticState_ClearsHeldKeyState()
    {
        InvokeStatic("StoreCurrentKeyDown", TadaInput.ThisKey.Dash);
        InvokeStatic("StoreCurrentKey", TadaInput.ThisKey.Dash);

        InvokeStatic("ResetStaticState");

        Assert.IsFalse(TadaInput.GetKey(TadaInput.ThisKey.Dash));
        Assert.IsFalse(TadaInput.GetKeyDown(TadaInput.ThisKey.Dash));
        Assert.IsFalse(TadaInput.GetKeyUp(TadaInput.ThisKey.Dash));
    }

    [Test]
    public void ResetStaticState_ClearsMouseStatics()
    {
        SetStaticField("_isMouseActive", true);
        SetStaticField("_MouseWorldPos", new Vector3(1f, 2f, 3f));
        Assert.IsTrue(TadaInput.IsMouseActive);

        InvokeStatic("ResetStaticState");

        Assert.IsFalse(TadaInput.IsMouseActive);
        Assert.AreEqual(Vector3.zero, TadaInput.MouseWorldPos);
    }

    [Test]
    public void Axis_PressAndHold_FiresSingleDownWhileHeld()
    {
        InvokeStatic("StoreCurrentAxisAsKeyType", TadaInput.ThisKey.Dash, 1f);

        Assert.IsTrue(TadaInput.GetKeyDown(TadaInput.ThisKey.Dash), "down on press frame");
        Assert.IsTrue(TadaInput.GetKey(TadaInput.ThisKey.Dash));

        InvokeStatic("StoreCurrentAxisAsKeyType", TadaInput.ThisKey.Dash, 1f);

        Assert.IsFalse(TadaInput.GetKeyDown(TadaInput.ThisKey.Dash), "no down re-fire while held");
        Assert.IsTrue(TadaInput.GetKey(TadaInput.ThisKey.Dash));
    }

    [Test]
    public void Axis_Release_FiresSingleUp()
    {
        InvokeStatic("StoreCurrentAxisAsKeyType", TadaInput.ThisKey.Dash, 1f);
        Assert.IsTrue(TadaInput.GetKeyDown(TadaInput.ThisKey.Dash), "consume the press");

        InvokeStatic("StoreCurrentAxisAsKeyType", TadaInput.ThisKey.Dash, 0f);

        Assert.IsTrue(TadaInput.GetKeyUp(TadaInput.ThisKey.Dash), "up on release frame");
        Assert.IsFalse(TadaInput.GetKey(TadaInput.ThisKey.Dash));
        Assert.IsFalse(TadaInput.GetKeyUp(TadaInput.ThisKey.Dash), "up is consumed too");
    }

    [Test]
    public void Axis_WithoutPriorPress_DoesNotFireEvents()
    {
        InvokeStatic("StoreCurrentAxisAsKeyType", TadaInput.ThisKey.Dash, 0f);

        Assert.IsFalse(TadaInput.GetKey(TadaInput.ThisKey.Dash));
        Assert.IsFalse(TadaInput.GetKeyDown(TadaInput.ThisKey.Dash));
        Assert.IsFalse(TadaInput.GetKeyUp(TadaInput.ThisKey.Dash));
    }

    [Test]
    public void ResetStaticState_ClearsAxisPressMemory()
    {
        InvokeStatic("StoreCurrentAxisAsKeyType", TadaInput.ThisKey.Dash, 1f);

        InvokeStatic("ResetStaticState");

        Assert.IsFalse(TadaInput.GetKey(TadaInput.ThisKey.Dash));
        Assert.IsFalse(TadaInput.GetKeyDown(TadaInput.ThisKey.Dash));

        // Releasing after a reset must not surface a stale Up event.
        InvokeStatic("StoreCurrentAxisAsKeyType", TadaInput.ThisKey.Dash, 0f);
        Assert.IsFalse(TadaInput.GetKeyUp(TadaInput.ThisKey.Dash));
    }

    [Test]
    public void NextUseRate_PressReleaseCycle_YieldsExactlyOneDownAndOneUp()
    {
        // Mirrors TadaInput.Update polling: Down on press, Key while held, Up on release.
        InvokeStatic("StoreCurrentKeyDown", TadaInput.ThisKey.NextUseRate);
        InvokeStatic("StoreCurrentKey", TadaInput.ThisKey.NextUseRate);
        InvokeStatic("StoreCurrentKey", TadaInput.ThisKey.NextUseRate);

        Assert.IsTrue(TadaInput.GetKeyDown(TadaInput.ThisKey.NextUseRate), "press frame");
        Assert.IsFalse(TadaInput.GetKeyDown(TadaInput.ThisKey.NextUseRate), "hold frame");

        InvokeStatic("StoreCurrentKeyUp", TadaInput.ThisKey.NextUseRate);

        Assert.IsTrue(TadaInput.GetKeyUp(TadaInput.ThisKey.NextUseRate), "release frame");
        Assert.IsFalse(TadaInput.GetKeyUp(TadaInput.ThisKey.NextUseRate), "release consumed");
        Assert.IsFalse(TadaInput.GetKey(TadaInput.ThisKey.NextUseRate));
    }
}
