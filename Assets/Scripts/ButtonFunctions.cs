using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using static Variables;

public class ButtonFunctions : MonoBehaviour
{
    public TMP_InputField inputField;

    int lastPressed = 0;
    bool bonusGiven = false;

    private void Start()
    {
        if (transform.parent.name == "Traits")
        {
            transform.GetChild(0).GetComponent<Toggle>().SetIsOnWithoutNotify(true);
            transform.GetChild(1).GetComponent<Toggle>().SetIsOnWithoutNotify(true);
        }
    }

    public void ChangeButtonStatus(int level)
    {
        bool cond = level == lastPressed && level == 1;
        if (transform.parent.name == "Traits")
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<Toggle>().SetIsOnWithoutNotify(i < Mathf.Max(2, level));
            }
            traitLvl = Mathf.Max(2, level);
            DiceTaker.instance.CatchDices();
        }
        else if (transform.parent.name == "Skills")
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<Toggle>().SetIsOnWithoutNotify(!cond && i < level);
            }
            ButtonManager.instance.LockedBySkills(level == 5);
            skillLvl = cond ? 0 : level;
            DiceTaker.instance.CatchDices();
        }
        else if (transform.parent.name == "Dramatic")
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<Toggle>().SetIsOnWithoutNotify(!cond && i < level);
            }
            nbDramaticWounds = cond ? 0 : level;
            ButtonManager.instance.LockedByWounds(level == 3);
            if (nbDramaticWounds > 0 && !bonusGiven)
            {
                AddABonusDice();
                bonusGiven = true;
            }
            else if (nbDramaticWounds == 0 && bonusGiven)
            {
                RemoveABonusDice();
                bonusGiven = false;
            }
            SetBonusDice();
        }
        lastPressed = cond ? 0 : level;
    }

    public void UpdateBonusDiceDisplay()
    {
        inputField.text = bonusDices.ToString();
        DiceTaker.instance.CatchDices();
    }

    public void SetBonusDice()
    {
        bonusDices = Mathf.Clamp(int.Parse(inputField.text), nbDramaticWounds > 0 ? 1 : 0, 10);
        UpdateBonusDiceDisplay();
    }

    public void AddABonusDice()
    {
        bonusDices = Mathf.Min(bonusDices + 1, 10);
        UpdateBonusDiceDisplay();
    }

    public void AddSeveralBonusDice()
    {
        bonusDices = Mathf.Min(bonusDices + 5, 10);
        UpdateBonusDiceDisplay();
    }

    public void RemoveABonusDice()
    {
        bonusDices = Mathf.Max(bonusDices - 1, nbDramaticWounds > 0 ? 1 : 0);
        UpdateBonusDiceDisplay();
    }

    public void RemvoeSeveralBonusDice()
    {
        bonusDices = Mathf.Max(bonusDices - 5, nbDramaticWounds > 0 ? 1 : 0);
        UpdateBonusDiceDisplay();
    }
}
