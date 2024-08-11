using DG.Tweening;
using System.Linq;
using TMPro;
using UnityEngine;

public class TreasureClicked : MonoBehaviour
{
    [Header("Text")]
    public TextMeshProUGUI WinAmountText;
    public TextMeshProUGUI FeatMultText;
    public TextMeshProUGUI PlayerBalanceText;
    public TextMeshProUGUI ChestMultText;

    [Header("Scripts")]
    public UIBehaviour UIBehaviour;
    public ParticleBehavior Particles;
    public MultiplierChestFeature MultiplierChests;
    public ChestController ChestController;
    public DenominationController DenomController;
    public RampingTally TallyNumber;

    private decimal _dividedWMult;
    private decimal _finalFeatureAmount;

    private void Start()
    {
        PlayerBalanceText.text = string.Format("{0:C}", GameSolver.Instance.PlayerBalance);
    }
    /// <summary>
    /// Checks to see if the win is a feature or is using it then displays whats needed, if not using it, just display.
    /// </summary>
    public void DisplayWin()
    {
        foreach (decimal Win in GameSolver.Instance.ListOfWins.ToList())
        {
            if (Win == 0)//If a zero is picked, its a pooper.
            {
                WinAmountText.text = "Pooper!";
                GameSolver.Instance.PlayerBalance += GameSolver.Instance.TotalWinBoxAmount;
                PlayerBalanceText.text = string.Format("{0:C}", GameSolver.Instance.PlayerBalance);
                Debug.Log("pooper!!!!!!!!!!");
                UIBehaviour.EnableAllButtons();
                ChestController.StopAnimations();
                ChestController.DisableAllChestColliders();
                DenomController.PlusAndMinusButtonBehaviour();
                return;
            }
            else if (Win == -1)//If a chest is picked, do whats below.
            {
                Debug.Log("its a chest");
                SetTextsAndMults();
                Particles.ChecktiersAndSetParticles(MultiplierChests.ChestIndex);
                MultiplierChests.IncrementFeatureMult();

                Debug.Log("chest index is " + MultiplierChests.ChestIndex);
            }
            else if (GameSolver.Instance.IsUsingFeature)//If the game is using the feature, use below.
            {
                FeatureChestMath(Win);
                Debug.Log("using feature doing math");
            }
            else//If just a normal turn, use below.
            {
                Debug.Log("normal win");
                WinAmountText.text = string.Format("{0:C}", Win);
                GameSolver.Instance.TotalWinBoxAmount += Win;
                UIBehaviour.WinboxAmountText.text = string.Format("{0:C}", GameSolver.Instance.TotalWinBoxAmount);
            }
            //ChestController.EnableColldiersOnChest();
            GameSolver.Instance.ListOfWins.Remove(Win);
            Debug.Log("took out " + Win);
            return;
        }

    }

    private void SetTextsAndMults()
    {
        WinAmountText.text = "New Mult! " + MultiplierChests.FeatureMultsTierList[MultiplierChests.ChestIndex] + "x";
        FeatMultText.text = MultiplierChests.FeatureMultsTierList[MultiplierChests.ChestIndex] + "x";
        MultiplierChests.FeatureMult = MultiplierChests.FeatureMultsTierList[MultiplierChests.ChestIndex];
        Debug.Log(MultiplierChests.FeatureMult + " Feat mult is");
        GameSolver.Instance.IsUsingFeature = true;
    }
    /// <summary>
    /// This function shows the "old" win then shows the new win!
    /// </summary>
    /// <param name="Win"></param>
    private void FeatureChestMath(decimal Win)
    {
        _dividedWMult = 0;
        _dividedWMult = Win / MultiplierChests.FeatureMult;
        WinAmountText.text = string.Format("{0:C}", _dividedWMult);
        ChestController.DisableCollidersOnChest();

        Delay(2f, () =>
        {
            Debug.Log("im delya hi");
            _finalFeatureAmount = 0;
            _finalFeatureAmount = MultiplierChests.FeatureMult * _dividedWMult * 1;
            ChestMultText.text = MultiplierChests.FeatureMultsTierList[MultiplierChests.ChestIndex - 1] + "x!";
            TallyNumber.AddValue((float)_finalFeatureAmount);
            Debug.Log("whats the final feature amount " + _finalFeatureAmount + " whats the win amount " + Win);
            GameSolver.Instance.TotalWinBoxAmount += Win;
            UIBehaviour.WinboxAmountText.text = string.Format("{0:C}", GameSolver.Instance.TotalWinBoxAmount);
            Delay(2f, () =>
            {
                ChestController.EnableCollidersOnChest();
            });
         
        });
    }
    /// <summary>
    /// Addes a delay to showcasing wins.
    /// </summary>
    /// <param name="time"></param>
    /// <param name="_callBack"></param>
    private static void Delay(float time, System.Action _callBack)
    {
        Sequence seq = DOTween.Sequence();

        seq.AppendInterval(time).AppendCallback(() => _callBack());
    }
}
