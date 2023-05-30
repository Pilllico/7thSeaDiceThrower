using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceLauncher : MonoBehaviour
{
    public static DiceLauncher instance;

    [HideInInspector] public int traitLvl = 2;
    [HideInInspector] public int skillLvl = 0;
    [HideInInspector] public bool plusOne = false;
    [HideInInspector] public int nbDramaticWounds = 0;
    [HideInInspector] public int bonusDices = 0;
    [HideInInspector] public bool explosion = false;

    public void Start()
    {
        instance = this;
        /*
        Debug.Log("WIDTH " + Screen.width + " HEIGHT " + Screen.height);
        Debug.Log("CURRENT RESOLUTION : " + Screen.currentResolution);
        Debug.Log("ANOTHER TRY : " + Display.main.systemWidth + "x" + Display.main.systemHeight);
        */
    }

    public void LaunchDices()
    {
        RaiseCalculator.SetDatas(traitLvl, skillLvl, plusOne, nbDramaticWounds, bonusDices, explosion);
        RaiseCalculator.LaunchDices();
    }

    public void CatchDices()
    {
        DiceTaker.instance.CatchDices(traitLvl, skillLvl, bonusDices);
    }
}
