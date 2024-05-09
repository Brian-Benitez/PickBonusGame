using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBehavior : MonoBehaviour
{
    public GameMain Check;
    /// <summary>
    /// Enables and disables chests.
    /// </summary>
    public void ChestColldiers()
    {
        Debug.Log("is game " + Check.GameStarts);
        if (Check.GameStarts)
        {
            GetComponent<BoxCollider>().enabled = true;
            return;
        }
        else
        {
            GetComponent<BoxCollider>().enabled = false;
            return;
        }

    }

}