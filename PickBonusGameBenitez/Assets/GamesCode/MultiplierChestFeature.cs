using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MultiplierChestFeature : MonoBehaviour
{
    public int FeatureMult = 0;
    public int ChestIndex = 0;

    [Header("Lists")]
    public List<int> FeatureMultsTierList;
    private List<decimal> OddNums;

    [Header("Percentage")]
    public int FeaturePercent;
    public int LosingFeatureThershold = 50;
    public int WinThreeChestThershold = 60;
    public int WinTwoChestThershold = 75;
    public int WinOneChestThershold = 100;


    public int ChestRemaining;
    public bool WonChest;

    [Header("Scripts")]
    public ChoosingAMult ChoosingAMult;
    public DenominationController DenominationController;

    /// <summary>
    /// Spits out a random number to see if the player is granted a Multiper Feature.
    /// </summary>
    public void RandomPercentageOfFeature()
    {
        //testing shit rn
        //FeaturePercent = 90;
        //FeaturePercent = Random.Range(0, 100);
        FeaturePercent = 60;
        Debug.Log("feature percent " + FeaturePercent);

        if (ChoosingAMult.ChoosenMult >= 5)//If the mult is greater than 5 then the player can win a feature chest.
        {
            if (FeaturePercent < LosingFeatureThershold)
            {
                //50
                Debug.Log("NO mult feature");
                WonChest = false;
            }
            else if (FeaturePercent <= WinThreeChestThershold)
            {
                //60
                Debug.Log("3 mult chest");
                ChestRemaining = 3;
                WonChest = true;
            }

            else if (FeaturePercent <= WinTwoChestThershold)
            {
                //75
                Debug.Log("2 mult chest");
                ChestRemaining = 2;
                WonChest = true;
            }

            else if (FeaturePercent <= WinOneChestThershold)
            {
                //100
                Debug.Log("1 mult chest");
                ChestRemaining = 1;
                WonChest = true;
            }
        }
        else if (ChoosingAMult.ChoosenMult < 5)//if less than 5 player cannot win chest.
        {
            WonChest = false;
            Debug.Log("dont give chest its less than five");
        }

        AddingAndOrgainzingChestInList();
    }

    /// <summary>
    /// Adds a chest or more depending on what percentage was won the shuffles the board and adds back odd numbers.
    /// </summary>
    /// 
    private void AddingAndOrgainzingChestInList()
    {
        GameSolver.Instance.ListOfWins.Sort();
       
        Debug.Log("org the list");
        if (WonChest)
        {
            var ChestWon = Enumerable.Repeat(-1, ChestRemaining).ToList();

            foreach (int Chests in ChestWon)
            {
                int index = Random.Range(0, GameSolver.Instance.ListOfWins.Count - 1);
                Debug.Log("index is " + index);
                GameSolver.Instance.ListOfWins.Insert(index, Chests);
                Debug.Log("place chest in this index " + index);
            }
            TakeOddNumsAndMoveToFront();
        }
        else
        {
            Debug.Log("player did not win any chest. No Chest are added");
        }

    }
    /// <summary>
    /// Takes out odd numbers in the list and readds them back in front of the list before a chest opening.
    /// </summary>
    private void TakeOddNumsAndMoveToFront()
    {
        OddNums = new List<decimal>();
        //take out odd nums
        foreach (decimal Nums in GameSolver.Instance.ListOfWins.ToList())
        {
            if (Nums == 0)
                Debug.Log("do nothing");
            if (Nums % 2 == 1)//check this.
            {
                Debug.Log("odd num is " + Nums);
                OddNums.Add(Nums);
                GameSolver.Instance.ListOfWins.Remove(Nums);
            }
        }
        //check to see what denom it is... then add them back to the og list.
        if ((float)DenominationController.CurrentDenom < 1f)
        {
            Debug.Log("dont worry abou it rn");
            //do something
        }
        else
        {
            foreach (decimal oddNuM in OddNums.ToList())
            {
                GameSolver.Instance.ListOfWins.Insert(0, oddNuM);
                OddNums.Remove(oddNuM);
                Debug.Log("add this back to the list. " + oddNuM);
            }
        }
    }
   
    /// <summary>
    /// Increments the chest index so the game will use the right feature multipler for the chests
    /// </summary>
    /// 
    public void IncrementFeatureMult()
    {
        ChestIndex++;
    }
    /// <summary>
    /// Restarts MultiplierChestFeature function
    /// </summary>
    public void RestartMultiplierChestFeature()
    {
        ChestRemaining = 0;
        FeatureMult = 0;
        FeaturePercent = 0;
        ChestIndex = 0;
        WonChest = false;
    }
}