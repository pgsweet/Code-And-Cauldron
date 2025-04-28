using System;
using System.Collections.Generic;
using System.IO;
using Unity.CodeEditor;
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
    public MenuButtonScript menuButtonScript;


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
    }

    public void resetLevel()
    {
        setLevel(currentLevel);
    }

    private void setLevel(int levelNum)
    {
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
            editorScript.clearCode();
            currentLevel = levelNum;
        }

        editorScript.clearAllContainers();
        if (toggled){
            menuButtonScript.openEditor();
        }
        menuButtonScript.enableCodeEditor();
    }


    public void completedLevel()
    {
        levels[currentLevel].SetCompleted(true);
        // Debug.Log($"Level {currentLevel} completed!");
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
                new System.Object[] { "Mushroom", 1, -1 },
                new System.Object[] { "Goblin_eye", 1, -1 },
            },
            new List<System.Object[]>() // output items
            {
                new System.Object[] { "Goblin_eye", 1 },
                new System.Object[] { "Mushroom", 1 },
            },
            1, // level num
            new List<string>() // starting dialog
            {
                "Welcome to level 1!",
                "In this level, you will learn how to use the INP and OUT commands with multiple items.",
                "Input both items and then send them to the output.",
                "But make sure to pay attention to the order of the output!",
            },
            new List<string>() // ending dialog
            {
                "Great job! You've completed level 1!",
                "Next up we'll learn how to craft a potion!"
            }
        ));

        // Level 2
        levels.Add(new Level(
            new List<System.Object[]>() // input items
            {
                new System.Object[] {"Feather", 2}
            },
            new List<System.Object[]>() // output items
            {
                new System.Object[] {"Potion of Weightlessness", 1}
            },
            2, // level num
            new List<string>() // starting dialog
            {
                "Now that you've learned how to input and output items, lets talk about crafting potions.",
                "the BOT command is used to bottle all ingredients in the cauldron!",
                "You can MOV command to move items around, either from one container to another, or into the cauldron!",
                "Input the feathers, move them into the cauldron, bottle the potion, then output the potion"
            },
            new List<string>() // ending dialog
            {
                "Great work apprentice! There's only a few more commands left for you to master.",
                "Navigate on over to the next level to learn about them."
            }
        ));

        // Level 3
        levels.Add(new Level(
            new List<System.Object[]>() // input items
            {
                new System.Object[] {"Emerald", 2},
                new System.Object[] {"Green_Mushroom", 2}
            },
            new List<System.Object[]>() // output items
            {
                new System.Object[] {"Potion of Invisibility", 1}
            },
            3, // level num
            new List<string>() // starting dialog
            {
                "Now lets talk about some extra arguments in some of the commands.",
                "The MOV command actually takes in 3 arguments, the first two are containers, and the third is the number of items to move.",
                "By default, it will move every item, thats why you havent noticed it yet.",
                "Try crafting a Potion of Invisibility with the input items, don't forget to have the exact number of items in the cauldron!"
            },
            new List<string>() // ending dialog
            {
                "Great work! The OUT command also has this extra argument, It may come in handy in the future..."
            }
        ));

        // levels.Add(new Level(
        //     new List<System.Object[]>() // input items
        //     {
                
        //     },
        //     new List<System.Object[]>() // output items
        //     {
                
        //     },
        //     -1, // level num
        //     new List<string>() // starting dialog
        //     {
                
        //     },
        //     new List<string>() // ending dialog
        //     {
                
        //     }
        // ));
    }
}
