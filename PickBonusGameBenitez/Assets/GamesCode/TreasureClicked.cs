using DG.Tweening;
using System.Linq;
using TMPro;
using UnityEngine;

public class TreasureClicked : MonoBehaviour
{
    [Header("Scripts")]
    public TextMeshProUGUI WinAmountText;
    public TextMeshProUGUI FeatMultText;
    public TextMeshProUGUI PlayerBalanceText;
    public UIBehaviour UIBehaviour;
    public ParticleBehavior Particles;
    public MultiplierChestFeature MultiplerChests;
    public ChestController ChestController;
    public DenominationController DenomController;

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
                Particles.ChecktiersAndSetParticles(MultiplerChests.ChestIndex);
                MultiplerChests.IncrementFeatureMult();

                Debug.Log("chest index is " + MultiplerChests.ChestIndex);
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
            ChestController.EnableColldiersOnChest();
            GameSolver.Instance.ListOfWins.Remove(Win);
            Debug.Log("took out " + Win);
            return;
        }

    }

    public void SetTextsAndMults()
    {
        WinAmountText.text = "New Mult! " + MultiplerChests.FeatureMultsTierList[MultiplerChests.ChestIndex] + "x";
        FeatMultText.text = MultiplerChests.FeatureMultsTierList[MultiplerChests.ChestIndex] + "x";
        MultiplerChests.FeatureMult = MultiplerChests.FeatureMultsTierList[MultiplerChests.ChestIndex];
        Debug.Log(MultiplerChests.FeatureMult + " Feat mult is");
        GameSolver.Instance.IsUsingFeature = true;
    }

    public void FeatureChestMath(decimal Win)
    {
        _dividedWMult = 0;
        _dividedWMult = Win / MultiplerChests.FeatureMult;
        WinAmountText.text = string.Format("{0:C}", _dividedWMult);
        ChestController.DisableCollidersOnChest();
        Debug.Log("wait");
        Delay(2f, () =>
        {
            ChestController.EnableColldiersOnChest();
            _finalFeatureAmount = 0;
            _finalFeatureAmount = MultiplerChests.FeatureMult * _dividedWMult * 1;
            Debug.Log("whats the num " + _dividedWMult);
            WinAmountText.text = string.Format("{0:C}", _finalFeatureAmount);
            GameSolver.Instance.TotalWinBoxAmount += Win;
            UIBehaviour.WinboxAmountText.text = string.Format("{0:C}", GameSolver.Instance.TotalWinBoxAmount);
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
