using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestBehavior : MonoBehaviour
{
    public GameMain GameMainRef;
    /// <summary>
    /// Enables and disables chests.
    /// </summary>
    public void ChestColldiers()
    {
        Debug.Log("is game " + GameMainRef.GameStarts);
        if (GameMainRef.GameStarts)
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