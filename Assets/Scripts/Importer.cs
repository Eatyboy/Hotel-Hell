using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Importer
{
    private const string sinsResourcePath = "TextData/sins";
    private const string namesResourcePath = "TextData/names";
    private const string dialogueResourcePath = "TextData/dialogue";

    private static StringReader OpenResource(string resourcePath)
    {
        TextAsset asset = Resources.Load<TextAsset>(resourcePath);
        if (asset == null)
        {
            Debug.LogWarning($"Could not load text resource at Resources/{resourcePath}");
            return null;
        }
        return new StringReader(asset.text);
    }

    public static List<Sin> LoadSins()
    {
        using StringReader sr = OpenResource(sinsResourcePath);
        if (sr == null) return null;

        List<Sin> sins = new();

        string line;
        int idAccumulator = 1;
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
                idAccumulator++;
            }
        }

        return sins;
    }

    public static (List<string> firstNames, List<string> lastNames) LoadNames()
    {
        using StringReader sr = OpenResource(namesResourcePath);
        if (sr == null) return (null, null);

        bool isFirstName = false;
        bool isLastName = false;

        List<string> firstNamesList = new();
        List<string> lastNamesList = new();

        string line;
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
                if (isFirstName) firstNamesList.Add(sinnerName);
                else if (isLastName) lastNamesList.Add(sinnerName);
            }
        }

        return (firstNamesList, lastNamesList);
    }

    public static List<string> LoadDialogue()
    {
        using StringReader sr = OpenResource(dialogueResourcePath);
        if (sr == null) return null;

        List<string> dialogues = new();

        string line;
        while ((line = sr.ReadLine()) != null)
        {
            dialogues.Add(line);
        }

        return dialogues;
    }
}
