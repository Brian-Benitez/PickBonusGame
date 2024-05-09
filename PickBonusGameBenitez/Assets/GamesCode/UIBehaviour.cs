using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIBehaviour : MonoBehaviour
{
    [Header("Scripts")]
    public DenominatonController DenomController;

    [Header("Texts")]
    public TextMeshProUGUI PlayerBalanceText;
    public TextMeshProUGUI WinBoxText;
    public TextMeshProUGUI WinboxAmountText;

    [Header("Buttons")]
    public Button PlayButton;

    private void Start()
    {
        WinboxAmountText.text = string.Format("{0:C}", 0);
    }

    /// <summary>
    /// Disables all buttons
    /// </summary>
    public void DisableAllButtons()
    {
        Debug.Log("buttons disable ");
        DenomController.PlusButton.interactable = false;
        DenomController.MinusButton.interactable = false;
        PlayButton.interactable = false;
    }
    /// <summary>
    /// Enables all buttons
    /// </summary>
    public void EnableAllButtons()
    {
        Debug.Log("buttons enable ");
        DenomController.PlusButton.interactable = true;
        DenomController.MinusButton.interactable = true;
        PlayButton.interactable = true;
    }
    /// <summary>
    /// Disables or enables the play button depending if the player balance is less or greater than the denom.
    /// </summary>
    public void PlayButtonBehaviour()
    {
        if (DenomController.CurrentDenom > GameSolver.Instance.PlayerBalance)
        {
            PlayButton.interactable = false;
        }
        else
        {
            PlayButton.interactable = true;
        }
    }

}