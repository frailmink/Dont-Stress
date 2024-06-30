using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalVariables : MonoBehaviour
{
    public static bool Paused = false;
    public static int squareWidth, squareHeight;
    private static bool buildingModeOn;

    private static readonly object _lockObject = new object();
    private static bool _isFunctionRunning = false;

    static GlobalVariables()
    {
        squareWidth = 20;
        squareHeight = 20;
        buildingModeOn = false;
    }

    public static bool GetBuildingMode()
    {
        return buildingModeOn;
    }

    public static void SetBuildingMode(bool b)
    {
        lock (_lockObject)
        {
            if (_isFunctionRunning)
            {
                // If the function is already running, exit immediately
                return;
            }
            _isFunctionRunning = true;
        }

        try
        {
            // Critical section: code that should only be run by one thread at a time
            buildingModeOn = b;
        }
        finally
        {
            // Ensure that the flag is reset even if an exception occurs
            lock (_lockObject)
            {
                _isFunctionRunning = false;
            }
        }
    }
}
