using UnityEngine;

public class EnvironmentObjectTrigger : MonoBehaviour
{
    // --------------------------------------
    // ----- 2D Isometric Shooter Study -----
    // ----------- by Tadadosi --------------
    // --------------------------------------
    // ---- Support my work by following ----
    // ---- https://twitter.com/tadadosi ----
    // --------------------------------------

    public EnvironmentObject[] objectsAbovePlayer;
    public EnvironmentObject[] objectsLeftOfPlayer;
    public EnvironmentObject[] objectsRightOfPlayer;
    public EnvironmentObject[] objectsBelowPlayer;

    public void SetActiveObjects()
    {
        // P0.E3.T13: guard the arrays themselves (elements were already guarded).
        if (objectsAbovePlayer != null)
        {
            for (int i = 0; i < objectsAbovePlayer.Length; i++)
            {
                if (objectsAbovePlayer[i] != null)
                    objectsAbovePlayer[i].SetActiveObject(EnvironmentObject.WhenPlayerIsOnPosition.Below);
            }
        }

        if (objectsLeftOfPlayer != null)
        {
            for (int i = 0; i < objectsLeftOfPlayer.Length; i++)
            {
                if (objectsLeftOfPlayer[i] != null)
                    objectsLeftOfPlayer[i].SetActiveObject(EnvironmentObject.WhenPlayerIsOnPosition.Right);
            }
        }

        if (objectsRightOfPlayer != null)
        {
            for (int i = 0; i < objectsRightOfPlayer.Length; i++)
            {
                if (objectsRightOfPlayer[i] != null)
                    objectsRightOfPlayer[i].SetActiveObject(EnvironmentObject.WhenPlayerIsOnPosition.Left);
            }
        }

        if (objectsBelowPlayer != null)
        {
            for (int i = 0; i < objectsBelowPlayer.Length; i++)
            {
                if (objectsBelowPlayer[i] != null)
                    objectsBelowPlayer[i].SetActiveObject(EnvironmentObject.WhenPlayerIsOnPosition.Above);
            }
        }
    }

    /// <summary>
    /// Restores every referenced façade object to its start set (P0.E3.T7).
    /// </summary>
    public void RestoreObjects()
    {
        RestoreSet(objectsAbovePlayer);
        RestoreSet(objectsLeftOfPlayer);
        RestoreSet(objectsRightOfPlayer);
        RestoreSet(objectsBelowPlayer);
    }

    private static void RestoreSet(EnvironmentObject[] set)
    {
        if (set == null)
            return;

        for (int i = 0; i < set.Length; i++)
        {
            if (set[i] != null)
                set[i].RestoreStartSet();
        }
    }
}
