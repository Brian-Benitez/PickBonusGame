using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameSolver : MonoBehaviour
{
    //Making one instance of Gamesolver
    //Also making so other scripts cannot adjust the values but just read them.
    public static GameSolver Instance; //{ get; private set; }//May use this idk

    [Header("Lists")]
    public List<decimal> ListOfWins;
    public List<float> EvenNumsToDivdeBy;
    public List<float> OddEvenNumsToDivideBy;

    public List<float> _numbersToDivideBy;

    [Header("Player info")]
    public float PlayerWinAmount;
    public decimal PlayerBalance = 10;
    public decimal TotalWinBoxAmount = 0;

    [Header("Booleans")]
    public bool IsUsingFeature;
    public bool NeedsToResolve;

    [Header("Solver info")]
    public int AttemptsToSolve;

    [Header("Nums for safety net")]
    public int FailedAttempts;
    public int MaxOfTriesLeft;

    [Header("Scripts")]
    public DenominationController DenomController;
    public UIBehaviour UIBehaviour;
    public ChoosingAMult ChoosingAMult;
    public MultiplierChestFeature MultChestFeatureRef;

    int NewIndex = 0;
    int OldIndex = 0;

    private void Awake()
    {
        //If there is a instance, and its not me, yeet myself.
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        DenomController.CurrentDenom = 1m;
        FailedAttempts = 0;
    }


    /// <summary>
    /// This function divides up wins for the player to receive then adds it to the list of wins.
    /// </summary>
    public void SolveTurn()
    {
        float dividedWinAmount = 0;
        Debug.Log("Target Win Amount: " + PlayerWinAmount);

        _numbersToDivideBy.Clear();

        if((float)DenomController.CurrentDenom <= .25f)
            _numbersToDivideBy.AddRange(EvenNumsToDivdeBy);
        else
        _numbersToDivideBy.AddRange(OddEvenNumsToDivideBy);

        for (int i = 0; i < AttemptsToSolve; i++)
        {
            do
            {
                int index = Random.Range(0, _numbersToDivideBy.Count);//Pick a radom number to divid up the win
                NewIndex = index;
            }
            while (NewIndex == OldIndex);//NOTE: IF IT GETS STUCK ITS HERE THIS IS THE ISSUE.
            OldIndex = NewIndex;
            
            Debug.Log("the number to divde by: " + _numbersToDivideBy[NewIndex]);
            dividedWinAmount = ((float)(PlayerWinAmount * _numbersToDivideBy[NewIndex]));//Divide the number up with the one of the numbers in NumsToDivideBy
            
            Debug.Log(dividedWinAmount + " a win amount");
            Debug.Log("formated " + dividedWinAmount.ToString("#.##"));
            ListOfWins.Add((decimal)dividedWinAmount);//Add it to the list

            //AmountChecker();//Make sure it fits in the list of wins

            if (AmountChecker())//If amount checker is true stop solving
            {
                Debug.Log("Stop solving, the list of wins is full and ready");
                return;
            }
        }
    }
    /// <summary>
    /// Checks to see if the full sum of numbers won adds up to the right win amount.
    /// If does, stop adding numbers to the list.
    /// </summary>
    private bool AmountChecker()
    {

        if (ListOfWins.Sum() == (decimal)PlayerWinAmount)
        {
            Debug.Log("done");
            foreach (decimal item in ListOfWins)
            {
                Debug.Log("NUMS " + item);
            }
            return true;
        }
        if (ListOfWins.Sum() > (decimal)PlayerWinAmount)
        {
            ListOfWins.RemoveAt(ListOfWins.Count - 1);
            Debug.Log("Number is too big for list" + " this is the list " + ListOfWins.Sum());

            FailedAttempts++;
            Debug.Log("attempts made " + FailedAttempts);

            if (FailedAttempts == MaxOfTriesLeft)
            {
                GiveLeftOverWinAmountToPlayer();
                Debug.Log("give player leftovers");
                return true;
            }
        }

        return false;
    }
    /// <summary>
    /// This function just does the math to give to the player in case they do not get the full win amount.
    /// </summary>
    private void GiveLeftOverWinAmountToPlayer()
    {
        decimal LeftOverWinAmount = (decimal)PlayerWinAmount - ListOfWins.Sum();
        ListOfWins.Add(LeftOverWinAmount);
        Debug.Log("left over " + LeftOverWinAmount);    
    }

    /// <summary>
    /// Takes out odd numbers in the list and readds them back in front of the list before a chest opening.
    /// </summary>
    public void CheckListForEligibilityOfFeature()
    {
        int amountOfNumsThatWontSolve = 0;
        if (MultChestFeatureRef.WonChest)
        {
            foreach (decimal item in ListOfWins.ToList())
            {
                decimal dividedResults = item / 8;
                /* we wanna not have this check, we wanna add chest once everything is good to go..
                if (item == -1)
                {
                    amountOfNumsThatWontSolve++;
                    Debug.LogWarning("ignore this");
                }
                */
                if (dividedResults % 0.05m == 0)
                {
                    Debug.LogWarning("this number is able to be divided " + item);
                    ListOfWins.Remove(item);
                    ListOfWins.Add(item);
                    NeedsToResolve = false;
                    return;
                }
                else
                {
                    Debug.LogWarning("this number is NOT able to be divdied " + item + "MOVE TO FRONT!! ");
                    amountOfNumsThatWontSolve++;
                    Debug.LogWarning("attemps so far " + amountOfNumsThatWontSolve + " count of list " + ListOfWins.Count);
                    ListOfWins.Remove(item);
                    ListOfWins.Insert(0, item);
                }

                if (amountOfNumsThatWontSolve == ListOfWins.Count)
                {
                    Debug.Log("if it crashes it will here " + amountOfNumsThatWontSolve + " then this " + ListOfWins.Count);
                    NeedsToResolve = true;
                    Debug.LogWarning("NNEEEDS TO RESOLVE");
                    //FailedAttempts = 0;
                    //AttemptsToSolve = 0;
                }
            }
        }
        else
            Debug.Log("Did not win chest");
    }


    /// <summary>
    /// Function to restart game solvers vars
    /// </summary>
    public void RestartGameSolver()
    {
        FailedAttempts = 0;
        TotalWinBoxAmount = 0;
        IsUsingFeature = false;
        NeedsToResolve = false;
    }
}
