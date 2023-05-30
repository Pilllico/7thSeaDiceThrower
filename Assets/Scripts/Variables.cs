using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Variables
{
    public static readonly List<Vector3> diceRotations = new()
    {
        new(0, 0, 180),         // n°0
        new(0, 180, 0),         // n°1
        new(18, 303, 212.5f),   // n°2
        new(306, 298, 35),      // n°3
        new(55, 120, 147),      // n°4
        new(340.5f, 122, 328),  // n°5
        new(18.5f, 57, 147.5f), // n°6
        new(305.5f, 62, 325),   // n°7
        new(55, 240, 212),      // n°8
        new(342, 237, 32),      // n°9
    };

    public static int traitLvl = 2;
    public static int skillLvl = 0;
    public static int bonusDices = 0;
    public static int nbDramaticWounds = 0;
    public static int nbExplosions = 0;
    public static bool plusOne = false;
    public static bool explosion = false;
}
