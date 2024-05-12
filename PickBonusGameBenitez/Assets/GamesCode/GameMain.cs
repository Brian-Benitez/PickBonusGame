using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMain : MonoBehaviour
{
    [Header("Booleans")]
    public bool GameStarts;

    [Header("Scripts")]
    public UIBehaviour UIBehaviour;
    public ChoosingAMult ChoosingAMult;
    public MultiplierChestFeature RandomNumOfChests;
    public DenominationController DenomController;
    public ChestController ChestsController;

    /// <summary>
    /// This function checks if the player can play the game, also checks for if player can afford to play etc etc.
    /// </summary>
    public void PrePlayConditionCheck()
    {

        if (GameSolver.Instance.PlayerBalance < .25m)
        {
            Debug.Log("You dont have money");
            UIBehaviour.EnableAllButtons();
            return;
        }
        else
        {
            GameSolver.Instance.PlayerBalance -= DenomController.CurrentDenom;
            UIBehaviour.PlayerBalanceText.text = string.Format("{0:C}", GameSolver.Instance.PlayerBalance);
            GameSolver.Instance.ListOfWins = new List<decimal>();

            if (GameSolver.Instance.ListOfWins == null)
                Debug.LogError("List null crash here.");
            else
            {
                Play();
                ChestsController.StartChestAnimations();
            }
        }
    }

    private void Play()
    {
        GameStarts = true;
        ChoosingAMult.RandomPercentageForMultList();
        //If its a zero mult, give the player zero dollars when they pick the chest
        if (ChoosingAMult.ChoosenMult == 0)
        {
            Debug.Log("This is a losing turn");
            GameSolver.Instance.ListOfWins.Add(0);
        }
        else//If not, Solve the turn and see if they get a feature chest as well
        {
            GameSolver.Instance.SolveTurn();
            RandomNumOfChests.RandomPercentageOfFeature();
            GameSolver.Instance.ListOfWins.Add(0);
            //remember to delete this below before sending its just for testing
            foreach (decimal item in GameSolver.Instance.ListOfWins)
            {
                Debug.Log("Win " + item);
            }
        }
    }
}