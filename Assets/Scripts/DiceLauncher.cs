using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static RaiseCalculator;

public class DiceLauncher : MonoBehaviour
{
    public static DiceLauncher instance;

    public void Start()
    {
        Application.targetFrameRate = 120;
        instance = this;
    }

    public void LaunchDices()
    {
        Roll();
    }
}
