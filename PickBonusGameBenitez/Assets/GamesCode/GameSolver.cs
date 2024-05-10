using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using Random = UnityEngine.Random;

public class GameSolver : MonoBehaviour
{
    //Making one instance of Gamesolver
    //Also making so other scripts cannot adjust the values but just read them.
    public static GameSolver Instance; //{ get; private set; }//May use this idk

    [Header("Lists")]
    public List<decimal> ListOfWins;

    public List<decimal> OddNumbers;

    public List<float> NumsToDivdeBy;

    [Header("Player info")]
    public float PlayerWinAmount;
    public decimal PlayerBalance = 10;
    public decimal TotalWinBoxAmount = 0;

    [Header("Booleans")]
    public bool IsUsingFeature;

    [Header("Solver info")]
    public int AttemptsToSolve;

    [Header("Nums for safety net")]
    public int FailedAttempts;
    public int MaxOfTriesLeft;


    [Header("Scripts")]
    public DenominationController DenomController;
    public UIBehaviour UIBehaviour;
    public ChoosingAMult ChoosingAMult;
    public MultiplierChestFeature RandomNumOfChests;

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
        PlayerWinAmount = (float)ChoosingAMult.ChoosenMult * (float)DenomController.CurrentDenom;// Multiply the mult with the current denom to get the player win amount

        float dividedWinAmount = 0;
        Debug.Log("heres the final amount X the denom " + PlayerWinAmount);
        for (int i = 0; i < AttemptsToSolve; i++)
        {
            int index = Random.Range(0, NumsToDivdeBy.Count);//Pick a radom number to divid up the win
            Debug.Log("the number to divde by: " + NumsToDivdeBy[index]);
            dividedWinAmount = ((float)(PlayerWinAmount * NumsToDivdeBy[index]));//Divide the number up with the one of the numbers in NumsToDivideBy

            if ((float)DenomController.CurrentDenom >= .50f)//This checks for when the denom is less than or equal to 50 cents, round it up more
            {
                dividedWinAmount = (float)(dividedWinAmount * 0.05 * 10);
                Debug.Log("round up number");
            }
            Debug.Log(dividedWinAmount + " a win amount");
            Debug.Log("formated " + dividedWinAmount.ToString("#.##"));
            ListOfWins.Add((decimal)dividedWinAmount);//Give it to the player
            AmountChecker();//Make sure it fits in the list of wins

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
            Debug.Log("Number is too big for list");

            FailedAttempts++;
            Debug.Log("attempts made " + FailedAttempts);

            if (FailedAttempts == MaxOfTriesLeft)
            {
                GiveLeftOverWinAmountToPlayer();
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
        ListOfWins.RemoveAt(ListOfWins.Count - 1);
        decimal LeftOverWinAmount = (decimal)PlayerWinAmount - ListOfWins.Sum();
        ListOfWins.Add(LeftOverWinAmount);
    }
    /// <summary>
    /// Function to restart game solvers vars
    /// </summary>
    public void RestartGameSolver()
    {
        FailedAttempts = 0;
        TotalWinBoxAmount = 0;
        IsUsingFeature = false;
    }
}
