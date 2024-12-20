﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class RestartGame : MonoBehaviour
{
    [Header("Text")]
    public List<TextMeshProUGUI> WinTexts;
    public List<TextMeshProUGUI> ChestMultTexts;
    public TextMeshProUGUI PlayerXMult;

    [Header("Scripts")]
    public GameMain GameMainBool;
    public ChestBehavior ChestBehavior;
    public UIBehaviour UIBehaviour;
    public MultiplierChestFeature Chests;
    public ChoosingAMult Mult;
    public ParticleBehavior Particles;
    public ChestController ChestsController;
    public DenominationController DenominatonController;

    /// <summary>
    /// Restarts everything needed for a game to start.
    /// </summary>
    public void RestartingGame()//I have this function be called onClick of the start button!
    {
        GameMainBool.GameStarts = false;
        PlayerXMult.text = "1X";
        UIBehaviour.WinboxAmountText.text = string.Format("{0:C}", 0);
        GameSolver.Instance.RestartGameSolver();
        Mult.RestartChoosingAMult();
        Chests.RestartMultiplierChestFeature();
        UIBehaviour.EnableAllButtons();
        DenominatonController.PlusAndMinusButtonBehaviour();
        Particles.DisableAllParticles();
        ChestsController.EnableAllChestColldiers();

        foreach (TextMeshProUGUI WinText in WinTexts)
        {
            WinText.text = " ";
        }
        foreach (TextMeshProUGUI ChestMultText in ChestMultTexts)
        {
            ChestMultText.text = " ";
        }
    }
}
