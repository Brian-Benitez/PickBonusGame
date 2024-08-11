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

    public float currentValue = 0, targetValue = 0;
    Coroutine RampTallyCoroutine;

    void Start()
    {
        //Funny bug here, the current value needs to place a number in the text so then it can convert the string to a float then it will not cry.
        numberText.text = "0";
        currentValue = float.Parse(numberText.text);
        targetValue = currentValue;
        numberText.text = " ";
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
        targetValue = 0;
        targetValue += value;
        if (RampTallyCoroutine != null)
            StopCoroutine(RampTallyCoroutine);
        RampTallyCoroutine = StartCoroutine(CountTo(targetValue));
    }

    public void SetTarget(float target)
    {
        targetValue = target;
        if (RampTallyCoroutine != null)
            StopCoroutine(RampTallyCoroutine);
        RampTallyCoroutine = StartCoroutine(CountTo(targetValue));
    }
}