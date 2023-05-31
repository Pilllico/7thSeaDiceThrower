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
        //Debug.Log(" ================================================ BEGINNING ");
        nbRaises = 0;
        nbExplosions = 0;
        for (int i = 0; i < (traitLvl + skillLvl + bonusDices); i++)
        {
            int die = UnityEngine.Random.Range(1, 11);
            while (die == 10 && explosion)
            {
                //Debug.Log("EXPLOSION BY NATURAL 10");
                nbExplosions++;
                die = UnityEngine.Random.Range(1, 11);
            }
            dicesRolled.Add(die);
        }
        if (skillLvl >= 3)
        {
            dicesRolled.Remove(dicesRolled.Min());
            int die = UnityEngine.Random.Range(1, 11);
            while (die == 10 && explosion)
            {
                //Debug.Log("EXPLOSION BY REROLL");
                nbExplosions++;
                die = UnityEngine.Random.Range(1, 11);
            }
            dicesRolled.Add(die);
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
        /*
        Debug.Log(" ================================================ ");
        Debug.Log("HERE'S THE DICES TO USE : " + LookDices(dicesRolled));
        Debug.Log("THINK ABOUT +5 MALUS IN CASE OF DANGER");
        Debug.Log(" ================================================ ");
        */
        Calculation();
    }

    private static void Calculation()
    {
        int raiseInitialValue = skillLvl >= 4 ? 15 : 10;
        while (raiseInitialValue >= 10)
        {
            List<int> temp = new();
            while (dicesRolled.Sum() >= raiseInitialValue)
            {
                int maxDice = dicesRolled.Max();
                dicesRolled.Remove(maxDice);

                int idealDice = raiseInitialValue - maxDice;
                //Debug.Log("MAX IS " + maxDice + " SHOULD SEARCH FOR " + idealDice + " WHICH EXISTS ? " + dicesRolled.Contains(idealDice));

                if (idealDice == 0)
                {
                    Debug.Log("NATURAL 10 FOUND");
                    nbRaises++;
                    continue;
                }
                if (dicesRolled.Contains(idealDice))
                {
                    Debug.Log("BEST PAIR FOUND WITH " + maxDice + " AND " + idealDice);
                    dicesRolled.Remove(idealDice);
                    nbRaises += raiseInitialValue == 15 ? 2 : 1;
                    continue;
                }

                temp.Add(maxDice);
                /*
                string newNewS = "";
                foreach (var item in soloDices)
                    newNewS += item.ToString() + " ";
                Debug.Log("SOLO DICES : " + newNewS);
                */
            }
            dicesRolled.AddRange(temp);
            dicesRolled.Sort();
            dicesRolled.Reverse();
            /*
            string leftoverS = "";
            foreach (var item in dicesRolled)
                leftoverS += item.ToString() + " ";
            Debug.Log(" LEFT OVER DICES : " + leftoverS);
            Debug.Log(" ================================================ GOING TO COMBOS ");
            */
            while (dicesRolled.Sum() >= raiseInitialValue)
            {
                for (int raiseValue = raiseInitialValue; raiseValue < 22; raiseValue++)
                {
                    GetCombo(out var combo, dicesRolled, raiseValue);
                    if (combo.Count > 0)
                    {
                        string comboS = "";
                        foreach (var item in combo)
                            comboS += item.ToString() + " ";
                        Debug.Log("COMBO FOUND WITH : " + comboS);
                        foreach (var die in combo)
                            dicesRolled.RemoveAt(dicesRolled.IndexOf(die));
                        nbRaises += raiseInitialValue == 15 ? 2 : 1;
                        break;
                    }
                }
            }
            raiseInitialValue -= 5;
        }
        /*
        Debug.Log(" ================================================ ");
        Debug.Log("TOTAL OF RAISES : " + nbRaises);
        Debug.Log("REMAINING DICES : " + LookDices(dicesRolled));
        Debug.Log(" ================================================ ");
        */
        DisplayRaises();
    }

    private static void GetCombo(out List<int> combo, List<int> dicesList, int raiseValue)
    {
        for (int i = 1; i < (int)Math.Pow(2, dicesList.Count); i++)
        {
            combo = new();
            for (int j = 0; j < dicesList.Count; j++)
            {
                if ((i >> j) % 2 != 0)
                    combo.Add(dicesList[j]);
                int sum = combo.Sum();
                if (sum == raiseValue)
                    return;
                if (sum > raiseValue)
                    break;
            }
        }
        combo = new();
    }

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