
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DenominationController : MonoBehaviour
{
    public int index;
    public decimal CurrentDenom;
    public Button MinusButton;
    public Button PlusButton;
    public TextMeshProUGUI DenomText;

    [SerializeField]
    private float[] Denoms;

    [Header("Script")]
    public UIBehaviour UI;

    void Start()
    {
        index = 2;
        DenomText.text = string.Format("{0:C}", CurrentDenom);
    }
    /// <summary>
    /// Controller for the denoms to switch
    /// </summary>
    public void PlusAndMinusButtonBehaviour()
    {

        UI.PlayButtonBehaviour();

        if (index <= Denoms[0])
        {
            Debug.Log("denom is " + CurrentDenom + " End of denom");
            MinusButton.interactable = false;
        }
        else if (index >= Denoms.Length - 1)
        {
            Debug.Log("denom is " + CurrentDenom + " Max of denom");
            PlusButton.interactable = false;
        }
        else
        {
            PlusButton.interactable = true;
            MinusButton.interactable = true;
        }
    }

    public void Increase()
    {
        index++;
        CurrentDenom = (decimal)Denoms[index];
        DenomText.text = string.Format("{0:C}", CurrentDenom);
        Debug.Log("look here " + CurrentDenom);
    }

    public void Decrease()
    {
        index--;
        CurrentDenom = (decimal)Denoms[index];
        DenomText.text = string.Format("{0:C}", CurrentDenom);
        Debug.Log("look here " + CurrentDenom);
    }
}
