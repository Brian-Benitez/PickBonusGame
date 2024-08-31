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

    [Header("Solver info")]
    public int MaxAttemptsToSolve;

    [Header("Nums for safety net")]
    public int FailedSolvedAttempts;
    public int MaxAmountOfResolves;

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
        FailedSolvedAttempts = 0;
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
        
        for (int i = 0; i < MaxAttemptsToSolve; i++)
        {
            do
            {
                int index = Random.Range(0, _numbersToDivideBy.Count);//Pick a radom number to divid up the win
                NewIndex = index;
            }
            while (NewIndex == OldIndex);
            OldIndex = NewIndex;
            
            Debug.Log("the number to divde by: " + _numbersToDivideBy[NewIndex]);
            dividedWinAmount = ((float)(PlayerWinAmount * _numbersToDivideBy[NewIndex]));//Divide the number up with the one of the numbers in NumsToDivideBy
            
            Debug.Log(dividedWinAmount + " a win amount");
            Debug.Log("formated " + dividedWinAmount.ToString("#.##"));
            ListOfWins.Add((decimal)dividedWinAmount);//Add it to the list

            if (AmountChecker())//If amount checker is true stop solving
            {
                Debug.Log("Stop solving, the list of wins is full and ready");
                Debug.Log("we check if the list is good for a feature");
                if (MultChestFeatureRef.WonChest)
                    CheckListForEligibilityOfFeature();
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

            FailedSolvedAttempts++;
            Debug.Log("attempts made " + FailedSolvedAttempts);

            if (FailedSolvedAttempts == MaxAmountOfResolves)
            {
                GiveLeftOverWinAmountToPlayer();
                Debug.Log("give player leftovers");
                return true;
            }
        }

        return false;
    }

    //All functions below are helping functions! ---------------------------------------------------------------->

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
    /// Grants 40 cents, Then it goes to solve again. This is for a edge case...
    /// </summary>
    private void GiveFortyCents()
    {
        ListOfWins = new List<decimal>();
        ListOfWins.Clear();
        FailedSolvedAttempts = 0;
        decimal fortyCents = 0.40m;
        ListOfWins.Add(fortyCents);
        SolveTurn();
        Debug.Log("added forty cents and has checked off every check to obtain this");
    }
    /// <summary>
    /// Checks to see if the numbers in the list will work for a feature chest win and if they are divisible by 0.05 and can be divided by 8.
    /// </summary>
    public void CheckListForEligibilityOfFeature()
    {
        int amountOfNumsThatWontSolve = 0;
        foreach (decimal item in ListOfWins.ToList())
        {
            if (item == -1)// checks if its a feature chest, if is tally up that var because its not a real number in the list
                amountOfNumsThatWontSolve++;

            decimal dividedResults = item / 8;
            Debug.Log("whats the num " + dividedResults);
            // checks if its divisible by 0.05, has max of 2 decimal points and its not -1
            if (dividedResults % 0.05m == 0 && HasMoreThanDecimalPlaces(dividedResults, 2) == false && item != -1)
            {
                Debug.LogWarning("this number is able to be divided " + item + " Add last");
                ListOfWins.Remove(item);
                ListOfWins.Add(item);
            }
            else if (item != -1)// checks if its its not -1 and is also a number that cant be divided well
            {
                Debug.LogWarning("this number is NOT able to be divdied " + item + "MOVE TO FRONT!! ");
                ListOfWins.Remove(item);
                ListOfWins.Insert(0, item);
                amountOfNumsThatWontSolve++;
                Debug.LogWarning("attemps so far " + amountOfNumsThatWontSolve + " count of list " + ListOfWins.Count);
            }

            if (amountOfNumsThatWontSolve == ListOfWins.Count)//sees if the whole list cannot be used.
            {
                Debug.Log("This list isnt good");
                GiveFortyCents();
            }
        }
    }

    /// <summary>
    /// This checks if the number that is given has a certain amount of decimal places in it, if its more than 4, do not use it in the list of wins.
    /// </summary>
    /// <param name="number"></param>
    /// <param name="maxDecimalPlaces"></param>
    /// <returns></returns>
    public bool HasMoreThanDecimalPlaces(decimal number, int maxDecimalPlaces)
    {
        Debug.LogWarning("tHIS NUMBER IS... " + number);
        // Convert to string and split by decimal point
        string numberString = number.ToString("G17");
        Debug.Log("number string " +  numberString);    
        if (numberString.Contains("."))
        {
            int decimalPlaces = numberString.Split('.')[1].Length;
            Debug.Log("ITS Wont work " + number);
            Debug.Log("how many decimals places " + decimalPlaces);
            return decimalPlaces > maxDecimalPlaces;
        }
        Debug.Log("should be fine " + number);
        return false;
    }
    /// <summary>
    /// Function to restart game solvers vars
    /// </summary>
    public void RestartGameSolver()
    {
        FailedSolvedAttempts = 0;
        TotalWinBoxAmount = 0;
        IsUsingFeature = false;
    }
}
