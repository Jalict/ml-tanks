using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Statestics
{
    public static void WriteDataPoint(int playerNumber, StateType type, float value)
    {
        string filename = @"\Statestics\" + SceneManager.GetActiveScene().name + @"\player_" + playerNumber + @"\" + type.ToString() + ".csv";

        File.AppendAllText(
            Directory.GetCurrentDirectory().ToString() + filename, 
            type.ToString() + ";" + value + "\n"
            );
    }
}

public enum StateType
{
    TimeToKill,
    TimeToHit,
    AmountOfMovement
}
