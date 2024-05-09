using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class RestartGame : MonoBehaviour
{
    [Header("Text")]
    public List<TextMeshProUGUI> WinTexts;
    public TextMeshProUGUI PlayerXMult;

    [Header("Scripts")]
    public GameMain GameMainBool;
    public ChestBehavior ChestBehavior;
    public UIBehaviour UIBehaviour;
    public MultiplierChestFeature Chests;
    public ChoosingAMult Mult;
    public ParticleBehavior Particles;
    public ChestsController ChestsController;
    public DenominatonController DenominatonController;
    /// <summary>
    /// Restarts everything needed for a game to start.
    /// </summary>
    public void RestartingGame()//I have this function be called onClick of the start button!
    {
        GameMainBool.GameStarts = false;
        PlayerXMult.text = " ";
        UIBehaviour.WinboxAmountText.text = string.Format("{0:C}", 0);

        GameSolver.Instance.RestartGameSolver();
        Mult.RestartChoosingAMult();
        Chests.RestartMultiplierChestFeature();
        UIBehaviour.EnableAllButtons();
        DenominatonController.PlusAndMinusButtonBehaviour();
        Particles.DisableAllParticles();
        ChestsController.EnableAllChestColldiers();

        foreach (TextMeshProUGUI text in WinTexts)
        {
            text.text = " ";
        }
    }
}
