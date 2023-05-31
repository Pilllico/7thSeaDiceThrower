using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Reflection;
using System.Linq;
using System.Collections;

using static Variables;

public static class RaiseCalculator
{
    private static readonly List<int> dicesRolled = new();
    private static readonly List<int> firstHalfRolled = new();
    private static readonly List<int> secondHalfRolled = new();

    private static int nbRaises = 0;

    public static void DisplayRaises()
    {
        GameObject.Find("Canvas/Raises").transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = nbRaises.ToString();
        GameObject.Find("Canvas/Raises").transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = LookDices(dicesRolled);
        GameObject.Find("Canvas/Raises").transform.GetChild(5).GetComponent<TextMeshProUGUI>().text = nbExplosions.ToString();
        ExplosionsTracker.instance.SetExplosionTracker();
    }

    public static void Roll()
    {
        dicesRolled.Clear();
        firstHalfRolled.Clear();
        secondHalfRolled.Clear();
        nbRaises = 0;
        nbExplosions = 0;
        for (int i = 0; i < (traitLvl + skillLvl + bonusDices); i++)
        {
            int die = UnityEngine.Random.Range(1, 11);
            while (die == 10 && explosion)
            {
                Debug.Log("EXPLOSION");
                nbExplosions++;
                die = UnityEngine.Random.Range(1, 11);
            }
            dicesRolled.Add(die);
        }
        if (skillLvl >= 3)
        {
            int die = UnityEngine.Random.Range(1, 11);
            dicesRolled[dicesRolled.IndexOf(dicesRolled.Min())] = die;
            while (die == 10 && explosion)
            {
                Debug.Log("EXPLOSION");
                nbExplosions++;
                die = UnityEngine.Random.Range(1, 11);
            }
        }
        if (plusOne)
        {
            for (int i = 0; i < dicesRolled.Count; i++)
            {
                if (dicesRolled[i] < 10)
                {
                    dicesRolled[i]++;
                }
            }
        }
        for (int i = 0; i < nbExplosions; i++)
        {
            dicesRolled.Add(10);
        }
        DiceTaker.instance.RotateDices(dicesRolled);
        Debug.Log("HERE'S THE DICES USED : " + LookDices(dicesRolled));
        Debug.Log("THINK ABOUT +5 MALUS IN CASE OF DANGER");
        Calculation();
    }

    private static void Calculation()
    {
        SeparateIntoHalf();
        int raiseInitialValue = skillLvl >= 4 ? 15 : 10;
        bool preCalcul = true;
        while (raiseInitialValue >= 10)
        {
            int nbDicesUsedForRaises = raiseInitialValue == 15 ? 2 : 1;
            while (dicesRolled.Sum() >= raiseInitialValue)
            {
                bool raiseFound = false;
                for (int raiseValue = raiseInitialValue; raiseValue < 2 * raiseInitialValue; raiseValue++)
                {
                    if (preCalcul)
                    {
                        bool C1 = GetCombo(out var firstHalfCombo, firstHalfRolled, nbDicesUsedForRaises, raiseValue);
                        if (C1)
                        {
                            raiseFound = true;
                            foreach (var die in firstHalfCombo)
                            {
                                firstHalfRolled.RemoveAt(firstHalfRolled.IndexOf(die));
                            }
                            nbRaises += raiseInitialValue == 15 ? 2 : 1;
                        }

                        bool C2 = GetCombo(out var secondHalfCombo, secondHalfRolled, nbDicesUsedForRaises, raiseValue);
                        if (C2)
                        {
                            raiseFound = true;
                            foreach (var die in secondHalfCombo)
                            {
                                secondHalfRolled.RemoveAt(secondHalfRolled.IndexOf(die));
                            }
                            nbRaises += raiseInitialValue == 15 ? 2 : 1;
                        }

                        MergeIntoOne();
                        preCalcul = false;
                    }
                    if (!preCalcul)
                    {
                        if (GetCombo(out var combo, dicesRolled, nbDicesUsedForRaises, raiseValue))
                        {
                            raiseFound = true;
                            foreach (var die in combo)
                            {
                                dicesRolled.RemoveAt(dicesRolled.IndexOf(die));
                            }
                            nbRaises += raiseInitialValue == 15 ? 2 : 1;
                            break;
                        }
                    }
                }
                if (!raiseFound)
                {
                    nbDicesUsedForRaises++;
                }
            }
            raiseInitialValue -= 5;
        }
        Debug.Log("TOTAL OF RAISES : " + nbRaises);
        Debug.Log("REMAINING DICES : " + LookDices(dicesRolled));
        DisplayRaises();
    }

    private static void SeparateIntoHalf()
    {
        for (int i = 0; i < dicesRolled.Count; i++)
        {
            if (i < dicesRolled.Count / 2)
            {
                firstHalfRolled.Add(dicesRolled[i]);
            }
            else
            {
                secondHalfRolled.Add(dicesRolled[i]);
            }
        }
    }

    private static void MergeIntoOne()
    {
        dicesRolled.Clear();
        foreach (var item in firstHalfRolled)
        {
            dicesRolled.Add(item);
        }
        foreach (var item in secondHalfRolled)
        {
            dicesRolled.Add(item);
        }
    }

    static readonly int limit = 100000;

    private static bool GetCombo(out List<int> combo, List<int> dicesList, int setSize, int raiseValue)
    {
        int countOfOperations = 0;
        if (setSize > dicesList.Count)
        {
            combo = new();
            return false;
        }

        for (int i = 1; i < (int)Math.Pow(2, dicesList.Count); i++)
        {
            combo = new();
            for (int j = 0; j < dicesList.Count; j++)
            {
                if ((i >> j) % 2 != 0)
                {
                    combo.Add(dicesList[j]);
                    countOfOperations++;
                }
                if (combo.Count == setSize)
                {
                    if (combo.Sum() == raiseValue)
                    {
                        return true;
                    }
                    break;
                }
            }
            if (countOfOperations == limit)
            {
                combo = new();
                return false;
            }
        }
        combo = new();
        return false;
    }

    /*
    private static List<List<int>> GetCombination(List<int> intList, int setSize, int raiseValue)
    {
        if (setSize > intList.Count)
        {
            return new();
        }
        List<List<int>> combos = new();
        for (int i = 1; i < (int)Math.Pow(2, intList.Count); i++)
        {
            combos.Add(new List<int>());
            for (int j = 0; j < intList.Count; j++)
            {
                if ((i >> j) % 2 != 0)
                {
                    combos[^1].Add(intList[j]);
                }
            }
        }
        Debug.Log("NUMBER OF ELEMENT IN COMBOS : " + combos.Count);
        //return combos.Where(c => c.Count == setSize).ToList();
        return new();
    }
    */


    private static string LookDices(List<int> diceList)
    {
        string s = "";
        if (diceList.Count > 0)
        {
            for (int i = 0; i < diceList.Count - 1; i++)
            {
                s += diceList[i] + " - ";
            }
            s += diceList[^1];
        }
        else
        {
            s = "None";
        }
        return s;
    }
}