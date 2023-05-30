using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static Variables;

public class ButtonManager : MonoBehaviour
{
    public static ButtonManager instance;
    public Toggle plusOneToggle;
    public Toggle explosionToggle;
    [HideInInspector] public bool explosionLockedBySkills = false;
    [HideInInspector] public bool explosionLockedByWounds = false;
    [HideInInspector] public bool explosionLockedByButton = false;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public void LockedBySkills(bool status)
    {
        explosionLockedBySkills = status;
        SetExplosionStatus();
    }

    public void LockedByWounds(bool status)
    {
        explosionLockedByWounds = status;
        SetExplosionStatus();
    }

    public void LockedByButton(bool status)
    {
        explosionLockedByButton = status;
        SetExplosionStatus();
    }

    public void ButtonExplosion()
    {
        LockedByButton(explosionToggle.isOn);
    }

    public void ButtonPlusOne()
    {
        plusOne = plusOneToggle.isOn;
    }

    public void SetExplosionStatus()
    {
        bool explosionIsOn = explosionLockedBySkills || explosionLockedByWounds || explosionLockedByButton;
        explosionToggle.SetIsOnWithoutNotify(explosionIsOn);
        explosion = explosionIsOn;
    }
}
