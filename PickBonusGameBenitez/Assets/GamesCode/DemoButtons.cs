using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoButtons : MonoBehaviour
{
    public UIBehaviour Behaviour;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            GameSolver.Instance.PlayerBalance += 5;
            Behaviour.PlayerBalanceText.text = string.Format("{0:C}", GameSolver.Instance.PlayerBalance);
        }
    }
}
