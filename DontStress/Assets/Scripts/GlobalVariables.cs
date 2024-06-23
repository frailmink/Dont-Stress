using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public static int squareWidth, squareHeight;

    static GlobalVariables()
    {
        squareWidth = 20;
        squareHeight = 20;
    }
}
