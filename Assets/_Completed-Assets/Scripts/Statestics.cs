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

        using (StreamWriter w = File.AppendText(Directory.GetCurrentDirectory().ToString() + filename))
        {
            w.WriteLine(Time.time + ";" + type.ToString() + ";" + value + "\n");
        }
    }
}

public enum StateType
{
    TimeToKill,
    TimeToHit,
    AmountOfMovement,
    Winner
}
