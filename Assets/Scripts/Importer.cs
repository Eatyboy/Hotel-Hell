using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class Importer
{
    private static readonly string sinsTextPath = Application.dataPath + "/TextData/sins.txt";
    private static readonly string namesTextPath = Application.dataPath + "/TextData/names.txt";

    public static List<Sin> LoadSins()
    {
        if (string.IsNullOrEmpty(sinsTextPath))
        {
            Debug.LogWarning("Invalid sins file path");
            return null;
        }

        List<Sin> sins = new();

        using StreamReader sr = new(sinsTextPath);
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
                Sin sin = new()
                {
                    sinId = idAccumulator,
                    hellCircle = currentCircle,
                    description = line[2..]
                };

                sins.Add(sin);

                sinsCreated++;
                idAccumulator++;
            }
            count++;
        }

        return sins;
    }

    public static (List<string> firstNames, List<string> lastNames) LoadNames()
    {
        if (string.IsNullOrEmpty(namesTextPath))
        {
            Debug.LogWarning("Invalid names file path");
            return (null, null);
        }

        using StreamReader sr = new(namesTextPath);
        string line;
        int count = 0;
        int firstNamesAdded = 0;
        int lastNamesAdded = 0;

        bool isFirstName = false;
        bool isLastName = false;

        List<string> firstNamesList = new();
        List<string> lastNamesList = new();

        while ((line = sr.ReadLine()) != null)
        {
            if (line.Contains("First Name:"))
            {
                isFirstName = true;
                isLastName = false;
            }
            else if (line.Contains("Last Name:"))
            {
                isFirstName = false;
                isLastName = true;
            }
            else if (!string.IsNullOrEmpty(line))
            {
                string sinnerName = line.Trim();
                if (isFirstName)
                {
                    firstNamesList.Add(sinnerName);
                    firstNamesAdded++;
                } 
                else if (isLastName)
                {
                    lastNamesList.Add(sinnerName);
                    lastNamesAdded++;
                }
            }
            count++;
        }

        return (firstNamesList, lastNamesList);
    }
}
