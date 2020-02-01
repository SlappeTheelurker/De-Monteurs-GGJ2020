using UnityEngine;
using System.Collections;
using System.IO;

public static class ScoreSaver
{

    public static void SaveScore(string name, int score)
    {
        string path = Application.persistentDataPath + "/score.txt";
        string data = name + "," + score + "\n";


        
          
            if (File.Exists(path))
            {
                File.AppendAllText(path,data);
            }
            else
            {
                File.WriteAllText(path, data);
            }
        
    }

    public static string LoadScores()
    {

        string path = Application.persistentDataPath + "/score.txt";
        string data = "";
        if (File.Exists(path))
        {
            data = string.Join("", File.ReadAllLines(path));
        }
        
        return data;
    }


}
