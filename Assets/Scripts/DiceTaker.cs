using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class DiceTaker : MonoBehaviour
{
    public static DiceTaker instance;

    private readonly List<Transform> traitDices = new();
    private readonly List<Transform> skillDices = new();
    private readonly List<Transform> bonusDicesRow1 = new();
    private readonly List<Transform> bonusDicesRow2 = new();
    int traits = 2;
    int skills = 0;
    int bonus = 0;

    private void Start()
    {
        instance = this;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (i < 5)
            {
                traitDices.Add(child);
            }
            else if (i < 10)
            {
                skillDices.Add(child);
            }
            else if (i < 15)
            {
                bonusDicesRow1.Add(child);
            }
            else if (i < 20)
            {
                bonusDicesRow2.Add(child);
            }
        }
    }

    public void CatchDices(int newTraits, int newSkills, int newBonus)
    {
        for (int i = 0; i < traitDices.Count; i++)
            traitDices[i].DOScale(i < newTraits ? 0.125f : 0f, 0.75f).SetEase(i < newTraits ? Ease.OutBack : Ease.InBack);
        for (int i = 0; i < skillDices.Count; i++)
            skillDices[i].DOScale(i < newSkills ? 0.125f : 0f, 0.75f).SetEase(i < newSkills ? Ease.OutBack : Ease.InBack);
        for (int i = 0; i < bonusDicesRow1.Count; i++)
            bonusDicesRow1[i].DOScale(i < newBonus ? 0.125f : 0f, 0.75f).SetEase(i < newBonus ? Ease.OutBack : Ease.InBack);
        for (int i = 0; i < bonusDicesRow2.Count; i++)
            bonusDicesRow2[i].DOScale(i + 5 < newBonus ? 0.125f : 0f, 0.75f).SetEase(i + 5 < newBonus ? Ease.OutBack : Ease.InBack);

        traits = newTraits;
        skills = newSkills;
        bonus = newBonus;

        Debug.Log("PATATE");
    }
}
