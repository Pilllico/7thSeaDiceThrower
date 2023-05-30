using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using static Variables;

public class DiceTaker : MonoBehaviour
{
    public static DiceTaker instance;

    private readonly float animTime = 0.5f;
    private readonly List<Transform> traitDices = new();
    private readonly List<Transform> skillDices = new();
    private readonly List<Transform> bonusDicesRow1 = new();
    private readonly List<Transform> bonusDicesRow2 = new();

    private void Start()
    {
        instance = this;
        for (int i = 0; i < transform.childCount; i++)
        {
            Transform child = transform.GetChild(i);
            if (i < 5)
                traitDices.Add(child);
            else if (i < 10)
                skillDices.Add(child);
            else if (i < 15)
                bonusDicesRow1.Add(child);
            else if (i < 20)
                bonusDicesRow2.Add(child);
        }
    }

    public void CatchDices()
    {
        for (int i = 0; i < traitDices.Count; i++)
        {
            traitDices[i].GetComponent<RandomRotation>().enabled = true;
            traitDices[i].DOScale(i < traitLvl ? 0.125f : 0f, animTime).SetEase(i < traitLvl ? Ease.OutBack : Ease.InBack);
        }
        for (int i = 0; i < skillDices.Count; i++)
        {
            skillDices[i].GetComponent<RandomRotation>().enabled = true;
            skillDices[i].DOScale(i < skillLvl ? 0.125f : 0f, animTime).SetEase(i < skillLvl ? Ease.OutBack : Ease.InBack);
        }
        for (int i = 0; i < bonusDicesRow1.Count + bonusDicesRow2.Count; i++)
        {
            Transform dice = (i < bonusDicesRow1.Count ? bonusDicesRow1 : bonusDicesRow2)[i % bonusDicesRow1.Count];
            dice.GetComponent<RandomRotation>().enabled = true;
            dice.DOScale(i < bonusDices ? 0.125f : 0f, animTime).SetEase(i < bonusDices ? Ease.OutBack : Ease.InBack);
        }
    }

    public void RotateDices(List<int> results)
    {
        for (int i = 0; i < traitLvl; i++)
        {
            traitDices[i].GetComponent<RandomRotation>().enabled = false;
            traitDices[i].DOLocalRotate(diceRotations[results[i] % 10], animTime).SetEase(Ease.OutBack);
        }
        for (int i = 0; i < skillLvl; i++)
        {
            skillDices[i].GetComponent<RandomRotation>().enabled = false;
            skillDices[i].DOLocalRotate(diceRotations[results[i + traitLvl] % 10], animTime).SetEase(Ease.OutBack);
        }
        for (int i = 0; i < bonusDices; i++)
        {
            Transform dice = (i < bonusDicesRow1.Count ? bonusDicesRow1 : bonusDicesRow2)[i % bonusDicesRow1.Count];
            dice.GetComponent<RandomRotation>().enabled = false;
            dice.DOLocalRotate(diceRotations[results[i + traitLvl + skillLvl] % 10], animTime).SetEase(Ease.OutBack);
        }
    }
}
