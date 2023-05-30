using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour
{
    public static ButtonManager instance;
    public Toggle explosion;
    public Toggle plusOne;
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
        LockedByButton(explosion.isOn);
    }

    public void ButtonPlusOne()
    {
        DiceLauncher.instance.plusOne = plusOne.isOn;
    }

    public void SetExplosionStatus()
    {
        bool explosionIsOn = explosionLockedBySkills || explosionLockedByWounds || explosionLockedByButton;
        explosion.SetIsOnWithoutNotify(explosionIsOn);
        DiceLauncher.instance.explosion = explosionIsOn;
    }
}
