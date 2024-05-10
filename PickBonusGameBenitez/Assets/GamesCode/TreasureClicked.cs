using DG.Tweening;
using System.Linq;
using TMPro;
using UnityEngine;

public class TreasureClicked : MonoBehaviour
{
    [Header("Scripts")]
    public TextMeshProUGUI WinAmountText;
    public GameSolver WinAmount;//fix this.
    private decimal _dividedWMult;
    private decimal _finalFeatureAmount;
    public TextMeshProUGUI FeatMultText;
    public TextMeshProUGUI PlayerBalanceText;
    public UIBehaviour UIBehaviour;
    public ParticleBehavior Particles;
    public MultiplierChestFeature chests;
    public ChestsController chestsController;
    public DenominatonController denominatonController;

    private void Start()
    {
        PlayerBalanceText.text = string.Format("{0:C}", WinAmount.PlayerBalance);
    }
    /// <summary>
    /// Checks to see if the win is a feature or is using it then displays whats needed, if not using it, just display.
    /// </summary>
    public void DisplayWin()
    {
        foreach (decimal Win in WinAmount.ListOfWins.ToList())
        {
            if (Win == 0)
            {
                WinAmountText.text = "Pooper!";
                WinAmount.PlayerBalance += WinAmount.TotalWinBoxAmount;
                PlayerBalanceText.text = string.Format("{0:C}", WinAmount.PlayerBalance);
                Debug.Log("pooper!!!!!!!!!!");
                UIBehaviour.EnableAllButtons();
                chestsController.StopAnimations();
                chestsController.DisableAllChestColliders();
                denominatonController.PlusAndMinusButtonBehaviour();
                return;
            }
            else if (Win == -1)
            {
                Debug.Log("its a chest");
                SetTextsAndMults();
                Particles.ChecktiersAndSetParticles(chests.ChestIndex);
                chests.IncrementFeatureMult();

                Debug.Log("chest index is " + chests.ChestIndex);
            }
            else if (WinAmount.IsUsingFeature)
            {
                FeatureChestMath(Win);
                Debug.Log("using feature doing math");
            }
            else
            {
                Debug.Log("normal win");
                WinAmountText.text = string.Format("{0:C}", Win);
                WinAmount.TotalWinBoxAmount += Win;
                UIBehaviour.WinboxAmountText.text = string.Format("{0:C}", WinAmount.TotalWinBoxAmount);
            }

            WinAmount.ListOfWins.Remove(Win);
            Debug.Log("took out " + Win);
            return;
        }

    }

    public void SetTextsAndMults()
    {
        WinAmountText.text = "New Mult! " + chests.FeatureMultsTierList[chests.ChestIndex] + "x";
        FeatMultText.text = chests.FeatureMultsTierList[chests.ChestIndex] + "x";
        chests.FeatureMult = chests.FeatureMultsTierList[chests.ChestIndex];
        Debug.Log(chests.FeatureMult + " Feat mult is");
        WinAmount.IsUsingFeature = true;
    }

    public void FeatureChestMath(decimal Win)
    {
        _dividedWMult = 0;
        _dividedWMult = Win / chests.FeatureMult;
        WinAmountText.text = string.Format("{0:C}", _dividedWMult);
        chestsController.DisableCollidersOnChest();
        Debug.Log("wait");
        Delay(2f, () =>
        {
            chestsController.EnableColldiersOnChest();
            _finalFeatureAmount = 0;
            _finalFeatureAmount = chests.FeatureMult * _dividedWMult * 1;
            Debug.Log("whats the num " + _dividedWMult);
            WinAmountText.text = string.Format("{0:C}", _finalFeatureAmount);
            WinAmount.TotalWinBoxAmount += Win;
            UIBehaviour.WinboxAmountText.text = string.Format("{0:C}", WinAmount.TotalWinBoxAmount);
        });
    }
    /// <summary>
    /// Addes a delay to showcasing wins.
    /// </summary>
    /// <param name="time"></param>
    /// <param name="_callBack"></param>
    public static void Delay(float time, System.Action _callBack)
    {
        Sequence seq = DOTween.Sequence();

        seq.AppendInterval(time).AppendCallback(() => _callBack());
    }
}
