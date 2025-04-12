using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{
    public Text levelText;
    private List<string> levels = new List<string>();
    private int unlockedLevel = 0;

    void Start()
    {
        // Example Levels
        levels.Add("Level 1: Basic Potion");
        levels.Add("Level 2: Fire Spell");
        levels.Add("Level 3: Advanced Potion");

        UpdateLevelSelect();
    }

    public void UnlockLevel(int levelIndex)
    {
        if (levelIndex >= 0 && levelIndex < levels.Count)
        {
            unlockedLevel = levelIndex;
            UpdateLevelSelect();
        }
    }

    private void UpdateLevelSelect()
    {
        levelText.text = "Select a Level:\n";
        for (int i = 0; i < levels.Count; i++)
        {
            if (i <= unlockedLevel)
            {
                levelText.text += (i + 1) + ". " + levels[i] + "\n";
            }
            else
            {
                levelText.text += (i + 1) + ". " + levels[i] + " (Locked)\n";
            }
        }
    }
}