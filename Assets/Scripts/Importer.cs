using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Importer : EditorUtility
{
    [MenuItem("Tools/Import Sins")]
    public static void ImportSins()
    {
        string path = OpenFilePanel("Select the sins text file", "", "txt");

        if (string.IsNullOrEmpty(path))
        {
            Debug.LogWarning("Invalid sins file path");
            return;
        }

        using StreamReader sr = new(path);
        string line;
        int count = 0;
        int idAccumulator = 1;
        int sinsCreated = 0;
        HellCircle currentCircle = HellCircle.None;
        while ((line = sr.ReadLine()) != null)
        {
            foreach (HellCircle circle in Enum.GetValues(typeof(HellCircle)))
            {
                if (line.Contains(Enum.GetName(typeof(HellCircle), circle) + ":"))
                {
                    currentCircle = circle;
                    break;
                }
            }
            if (line.StartsWith("- ") && !string.IsNullOrEmpty(line[2..]))
            {
                Sin sin = ScriptableObject.CreateInstance<Sin>();
                sin.sinId = idAccumulator;
                sin.hellCircle = currentCircle;
                sin.description = line[2..];

                string sinsFolderPath = "Assets/DataObjects/Sins/";
                string fileName = idAccumulator.ToString() + "_" + currentCircle.ToString() + ".asset";
                string sinPath = sinsFolderPath + fileName;

                AssetDatabase.DeleteAsset(sinPath);
                AssetDatabase.CreateAsset(sin, sinPath);

                sinsCreated++;
                idAccumulator++;
            }
            count++;
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log($"Created {sinsCreated} sins");
    }
}
