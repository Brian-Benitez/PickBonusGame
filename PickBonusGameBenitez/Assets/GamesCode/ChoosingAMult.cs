using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ChoosingAMult : MonoBehaviour
{
    [Header("List of mults")]
    public float[] Tier1Mults = { 1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f, 9f, 10f };
    public float[] Tier2Mults = { 12f, 16f, 24f, 32f, 48f, 64f };
    public float[] Tier3Mults = { 100f, 200f, 300f, 400f, 500f };

    private float[] _pickedMults;

    [Header("Percentages")]
    public int MultPercent;
    public int LosingPercentage = 50;
    public int TierThreeMultPercentage = 55;
    public int TierTwoMultPercentage = 70;
    public int TierOneMultPercentage = 100;

    [Header("Solving Mult number")]
    public decimal ChoosenMult;

    private int _randomNum;

    /// <summary>
    /// Checks for a percent to play what list of mults will be won.
    /// </summary>
    public void RandomPercentageForMultList()
    {
        //just taking off this below just for testing
        //MultPercent = Random.Range(0, 100);
        MultPercent = 100;

        Debug.Log("percentage " + MultPercent);
        //if it falls between either or, clone the tier mult to the picked mult then go to the solver
        if (MultPercent < LosingPercentage)
        {
            Debug.Log("losing turn, present this to player");
        }
        else if (MultPercent <= TierThreeMultPercentage)
        {
            //percentage is 50 + 5
            _pickedMults = (float[])Tier3Mults.Clone();
            Debug.Log("Use these 100m, 200m, 300m, 400m, 500m");
        }

        else if (MultPercent <= TierTwoMultPercentage)
        {
            //percentage is 50+20
            Debug.Log("use these 12,16,24,32,48,64");
            _pickedMults = (float[])Tier2Mults.Clone();
        }

        else if (MultPercent <= TierOneMultPercentage)
        {
            //percentage is 50 + 50
            Debug.Log("use these mults to solve 1,2,3,4,5,6,7,8,9,10");
            _pickedMults = (float[])Tier1Mults.Clone();
        }

        PickRandomMult();
    }

    /// <summary>
    /// Picks a random mult from the list of mults
    /// </summary>
    private void PickRandomMult()
    {
        if (MultPercent < LosingPercentage)
        {
            Debug.Log("no mults are picked, its a losing turn " + MultPercent + "%" + " mult is " + ChoosenMult);
            ChoosenMult = 0;
        }
        else
        {
            _randomNum = Random.Range(0, _pickedMults.Count());
            ChoosenMult = (decimal)_pickedMults[_randomNum];
            Debug.Log("mult randomly picked is " + _pickedMults[_randomNum]);
        }
    }
    /// <summary>
    /// This function is restarting the script
    /// </summary>
    public void RestartChoosingAMult()
    {
        MultPercent = 0;
        ChoosenMult = 0;
    }

}