using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
            DiceLauncher.instance.traitLvl = Mathf.Max(2, level);
            DiceLauncher.instance.CatchDices();
        }
        else if (transform.parent.name == "Skills")
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<Toggle>().SetIsOnWithoutNotify(!cond && i < level);
            }
            ButtonManager.instance.LockedBySkills(level == 5);
            DiceLauncher.instance.skillLvl = cond ? 0 : level;
            DiceLauncher.instance.CatchDices();
        }
        else if (transform.parent.name == "Dramatic")
        {
            for (int i = 0; i < transform.childCount; i++)
            {
                transform.GetChild(i).GetComponent<Toggle>().SetIsOnWithoutNotify(!cond && i < level);
            }
            DiceLauncher.instance.nbDramaticWounds = cond ? 0 : level;
            ButtonManager.instance.LockedByWounds(level == 3);
            if (DiceLauncher.instance.nbDramaticWounds > 0 && !bonusGiven)
            {
                AddABonusDice();
                bonusGiven = true;
            }
            else if (DiceLauncher.instance.nbDramaticWounds == 0 && bonusGiven)
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
        inputField.text = DiceLauncher.instance.bonusDices.ToString();
        DiceLauncher.instance.CatchDices();
    }

    public void SetBonusDice()
    {
        DiceLauncher.instance.bonusDices = Mathf.Clamp(int.Parse(inputField.text), DiceLauncher.instance.nbDramaticWounds > 0 ? 1 : 0, 10);
        UpdateBonusDiceDisplay();
    }

    public void AddABonusDice()
    {
        DiceLauncher.instance.bonusDices = Mathf.Min(DiceLauncher.instance.bonusDices + 1, 10);
        UpdateBonusDiceDisplay();
    }

    public void AddSeveralBonusDice()
    {
        DiceLauncher.instance.bonusDices = Mathf.Min(DiceLauncher.instance.bonusDices + 5, 10);
        UpdateBonusDiceDisplay();
    }

    public void RemoveABonusDice()
    {
        DiceLauncher.instance.bonusDices = Mathf.Max(DiceLauncher.instance.bonusDices - 1, DiceLauncher.instance.nbDramaticWounds > 0 ? 1 : 0);
        UpdateBonusDiceDisplay();
    }

    public void RemvoeSeveralBonusDice()
    {
        DiceLauncher.instance.bonusDices = Mathf.Max(DiceLauncher.instance.bonusDices - 5, DiceLauncher.instance.nbDramaticWounds > 0 ? 1 : 0);
        UpdateBonusDiceDisplay();
    }
}
