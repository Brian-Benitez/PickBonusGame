﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    public List<LootBox> LootBoxes;

    private void Start()
    {
        StopAnimations();
    }

    public void StopAnimations()
    {
        foreach (LootBox item in LootBoxes)
        {
            item.bouncingBox = false;
            item.BounceBox(item.bouncingBox);
        }
        Debug.Log("Stop chest animations");
    }

    public void StartChestAnimations()
    {
        LootBoxes.ToList().ForEach(Boxes => Boxes.BounceBox(true));
        Debug.Log("start animating chest");
    }

    public void DisableAllChestColliders()
    {
        LootBoxes.ToList().ForEach(Boxes => Boxes.GetComponent<Collider>().enabled = false);
    }

    public void EnableAllChestColldiers()
    {
        foreach (LootBox Boxes in LootBoxes)
        {
            Boxes.Close();
            Boxes.bouncingBox = true;
            Boxes.BounceBox(Boxes.bouncingBox);
            Boxes.GetComponent<Collider>().enabled = true;
        }
    }
    /// <summary>
    /// Disables chest for chest feature
    /// </summary>
    public void DisableCollidersOnChest()
    {
        foreach (LootBox box in LootBoxes)
        {
            if (box.isOpen == false)
            {
                box.GetComponent<Collider>().enabled = false;
                Debug.Log("disable this box");
            }
        }
    }
    /// <summary>
    /// Enable chest colldiers if there not open
    /// </summary>
    public void EnableCollidersOnChest()
    {
        foreach (LootBox box in LootBoxes)
        {
            if (box.isOpen == false)
            {
                box.GetComponent<Collider>().enabled = true;
                Debug.Log("enable this box");
            }
        }
    }
}
