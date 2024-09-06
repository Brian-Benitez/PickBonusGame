using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RearrangeList : MonoBehaviour
{
    /// <summary>
    /// Checks to see if the numbers in the list will work for a feature chest win and if they are divisible by 0.05 and can be divided by 8.
    /// </summary>
    public void CheckListForEligibilityOfFeature()
    {
        int amountOfNumsThatWontSolve = 0;
        foreach (decimal item in GameSolver.Instance.ListOfWins)
        {
            if (item == -1)// checks if its a feature chest, if is tally up that var because its not a real number in the list
                amountOfNumsThatWontSolve++;

            decimal dividedResults = item / 8;
            Debug.Log("whats the num " + dividedResults);
            // checks if its divisible by 0.05, has max of 2 decimal points and its not -1
            if (dividedResults % 0.05m == 0 && HasMoreThanDecimalPlaces(dividedResults, 2) == false && item != -1)
            {
                Debug.LogWarning("this number is able to be divided " + item + " Add last");
                GameSolver.Instance.ListOfWins.Remove(item);
                GameSolver.Instance.ListOfWins.Add(item);
            }
            else if (item != -1)// checks if its not -1 and is also a number that cant be divided well
            {
                Debug.LogWarning("this number is NOT able to be divdied " + item + "MOVE TO FRONT!! ");
                GameSolver.Instance.ListOfWins.Remove(item);
                GameSolver.Instance.ListOfWins.Insert(0, item);
                amountOfNumsThatWontSolve++;
                Debug.LogWarning("attemps so far " + amountOfNumsThatWontSolve + " count of list " + GameSolver.Instance.ListOfWins.Count);
            }

            if (amountOfNumsThatWontSolve == GameSolver.Instance.ListOfWins.Count)//sees if the whole list cannot be used.
            {
                Debug.Log("This list isnt good");
                GameSolver.Instance.GiveFortyCents();
            }
        }
    }


    /// <summary>
    /// This checks if the number that is given has a certain amount of decimal places in it, if its more than 4, do not use it in the list of wins.
    /// </summary>
    /// <param name="number"></param>
    /// <param name="maxDecimalPlaces"></param>
    /// <returns></returns>
    private bool HasMoreThanDecimalPlaces(decimal number, int maxDecimalPlaces)// 0.05 wont work here, i think it should? maybe check out max deciaml num.
    {
        Debug.LogWarning("tHIS NUMBER IS... " + number);
        // Convert to string and split by decimal point
        string numberString = number.ToString("G17");
        Debug.Log("number string " + numberString);
        if (numberString.Contains("."))
        {
            int decimalPlaces = numberString.Split('.')[1].Length;
            Debug.Log("ITS Wont work " + number);
            Debug.Log("how many decimals places " + decimalPlaces);
            return decimalPlaces > maxDecimalPlaces;
        }
        Debug.Log("should be fine " + number);
        return false;
    }
}
