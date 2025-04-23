using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LevelSelectScript : MonoBehaviour
{
    public List<Button> levelButtons = new List<Button>();
    private bool toggled = false;
    public GameObject levelBackground;
    private List<Level> levels = new List<Level>();
    private int currentLevel = -1;
    public InputScript inputScript;
    public OutputScript outputScript;
    public EditorScript editorScript;
    public DialogScript dialogScript;


    public void startGame()
    {
        initalizeLevels();
        updateButtons();
        setLevel(0);
    }

    private void updateButtons()
    {
        for (int i = 0; i < levelButtons.Count; i++)
        {
            if (i < levels.Count)
            {
                if (i == 0)
                {
                    levelButtons[i].interactable = true;
                }
                else if (levels[i - 1].IsCompleted())
                {
                    levelButtons[i].interactable = true;
                }
                else
                {
                    levelButtons[i].interactable = false;
                }
            }
            else
            {
                levelButtons[i].interactable = false;
            }
            levelButtons[i].onClick.AddListener(changeLevel);
        }
    }

    public void AddLevel(Level level)
    {
        levels.Add(level);
    }

    public void toggleLevelSelect()
    {
        int moveDirection = -1;
        if (toggled)
        {
            moveDirection = 1;
        }

        float rectWidth = levelBackground.GetComponent<RectTransform>().rect.width;

        gameObject.transform.localPosition += new Vector3(moveDirection * rectWidth, 0, 0);

        toggled = !toggled;
    }

    public void changeLevel()
    {
        int levelNum = -1;
        bool hasLevel = Int32.TryParse(EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TMPro.TMP_Text>().text, out levelNum);
        if (!hasLevel)
        {
            Debug.LogError("Failed to parse level number from button text.");
            return;
        }
        // Debug.Log($"Switching to level {levelNum}");
        
        if (levels.Count <= levelNum)
        {
            Debug.LogError($"Level {levelNum} does not exist in the levels list.");
            return;
        }

        setLevel(levelNum);

        // TODO: enable the code editor and then minimize it
    }

    public void resetLevel()
    {
        setLevel(currentLevel);
    }

    private void setLevel(int levelNum)
    {
        Debug.Log("Set Level");
        if (levelNum == -1)
        {
            Debug.LogError("No Level Selected");
            return;
        }

        inputScript.setInput(new List<System.Object[]>(levels[levelNum].GetInputItems()));
        outputScript.setRequiredItems(new List<System.Object[]>(levels[levelNum].GetRequiredOutputItems()));
    
        if (levelNum != currentLevel)
        {
            dialogScript.setDialog(new List<string>(levels[levelNum].getStartingDialogue()));
            // TODO: clear the editor
            currentLevel = levelNum;
        }

        editorScript.clearAllContainers();
    }


    public void completedLevel()
    {
        levels[currentLevel].SetCompleted(true);
        Debug.Log($"Level {currentLevel} completed!");
        updateButtons();
        dialogScript.setDialog(new List<string>(levels[currentLevel].getEndDialogue()));
    }

    public void initalizeLevels()
    {
        // Level 0
        levels.Add(new Level(
            new List<System.Object[]>() // input items
            {
                new System.Object[] { "Amethyst", 1, -1 },

            },
            new List<System.Object[]>() // output items
            {
                new System.Object[] { "Amethyst", 1 },
            }, 
            0, // level num
            new List<string>() // starting dialog
            {
                "So you're the new apprentice huh?",
                "Well I guess I need to train you on how to create my potions and spells.",
                "Let's start off with the INP and OUT commands.",
                "The INP command inputs the item from the input teleporter and places it into the container you specify.",
                "The OUT command takes the item from the container and places it into the output teleporter.",
                "Give it a try with the Amethyst I've given you."
            },
            new List<string>() // ending dialog
            {
                "Great job! You've completed level 0!",
                "Now you can move on to the next level, navigate to the level select screen and select the next level."
            }
        ));

        // Level 1
        levels.Add(new Level(
            new List<System.Object[]>() // input items
            {
                new System.Object[] { "Amethyst", 1, -1 },
                new System.Object[] { "Ruby", 1, -1 },
            },
            new List<System.Object[]>() // output items
            {
                new System.Object[] { "Ruby", 1 },
                new System.Object[] { "Amethyst", 1 },
            },
            1, // level num
            new List<string>() // starting dialog
            {
                "Welcome to level 1!",
                "In this level, you will learn how to use the INP and OUT commands with multiple items.",
                "You will need to input both the Amethyst and Ruby into the cauldron.",
                "Then, you will need to output both items from the cauldron. Make sure to take note of the order of the items!",
            },
            new List<string>() // ending dialog
            {
                "Great job! You've completed level 1!",
                "Now you can move on to the next level, navigate to the level select screen and select the next level."
            }
        ));
    }
}
