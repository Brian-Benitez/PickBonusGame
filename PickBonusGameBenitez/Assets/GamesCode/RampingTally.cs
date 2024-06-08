using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class RampingTally : MonoBehaviour
{
    [Header("Count Speed")]
    public float countDuration = 1;
    [Header("Text")]
    public TextMeshProUGUI numberText;

    float currentValue = 0, targetValue = 0;
    Coroutine _C2T;

    void Awake()
    {
        //numberText = GetComponent<TextMeshProUGUI>();
    }

    void Start()
    {
        currentValue = float.Parse(numberText.text);
        targetValue = currentValue;
    }

    IEnumerator CountTo(float targetValue)
    {
        var rate = Mathf.Abs(targetValue - currentValue) / countDuration;
        while (currentValue != targetValue)
        {
            currentValue = Mathf.MoveTowards(currentValue, targetValue, rate * Time.deltaTime);
            numberText.text = string.Format("{0:C}", currentValue);
            yield return null;
        }
    }

    public void AddValue(float value)
    {
        targetValue += value;
        if (_C2T != null)
            StopCoroutine(_C2T);
        _C2T = StartCoroutine(CountTo(targetValue));
    }

    public void SetTarget(float target)
    {
        targetValue = target;
        if (_C2T != null)
            StopCoroutine(_C2T);
        _C2T = StartCoroutine(CountTo(targetValue));
    }
}