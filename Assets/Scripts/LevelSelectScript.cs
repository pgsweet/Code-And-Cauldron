using System;
using System.Collections.Generic;
using System.IO;
using Unity.CodeEditor;
using Unity.VisualScripting;
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
        // TODO: levels after level 7 arent being clicked
        for (int i = 0; i < levelButtons.Count; i++)
        {
            if (i < levels.Count)
            {
                if (levels[i].IsCompleted())
                {
                    levelButtons[i].GetComponent<Image>().color = new Color(0, 255, 0);
                }

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
        Debug.Log($"Switching to level {levelNum}");
        
        if (levels.Count <= levelNum)
        {
            Debug.Log("No level found... Creating a new level");
            generateNewLevel();
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

    private void generateNewLevel()
    {
        System.Random rnd = new System.Random();
        RecipeCheck recipeCheck = new RecipeCheck();
        // generate 2-4 potions and shffle the required input around by 1-2 positions (small chance)
        // have a chance to reqiure multiple potions

        int newPotionsToMake = rnd.Next(2,4);
        List<List<System.Object[]>> newInputItems = new List<List<System.Object[]>>();
        List<System.Object[]> newPotionNames = new List<System.Object[]>();
        for (int i = 0; i < newPotionsToMake; i++)
        {
            System.Object[] p = recipeCheck.getRandomPotion();
            newInputItems.Add((List<System.Object[]>)p[1]);
            newPotionNames.Add(new System.Object[] {(string)p[0], 1});
        }

        List<System.Object[]> allInputItems = new List<System.Object[]>();
        foreach (List<System.Object[]> items in newInputItems)
        {
            foreach (System.Object[] item in items)
            {
                double chance = rnd.NextDouble();
                if (allInputItems.Count == 0)
                {
                    allInputItems.Add(item);
                }
                else if (allInputItems.Count <= 3)
                {
                    if (chance <= 0.4)
                    {
                        allInputItems.Insert(allInputItems.Count-2, item);
                    }
                    else{
                        allInputItems.Add(item);
                    }
                }
                else
                {
                    if (chance <= 0.2)
                    {
                        allInputItems.Insert(allInputItems.Count-3, item);
                    }
                    else if (chance <= 0.6)
                    {
                        allInputItems.Insert(allInputItems.Count-2, item);
                    }
                    else
                    {
                        allInputItems.Add(item);
                    }
                }
            }
        }

        Level newLevel = new Level(
            allInputItems, newPotionNames, levels.Count
        );

        levels.Add(newLevel);

    }

    private void initalizeLevels()
    {
        // Level 0 input and output
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
        levels[0].SetCompleted(true);

        // Level 1 different output order
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
        levels[1].SetCompleted(true);

        // Level 2 potion crafting
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
        levels[2].SetCompleted(true);

        // Level 3 mov command num of items
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
        levels[3].SetCompleted(true);

        // Level 4 clearing containers
        levels.Add(new Level(
            new List<System.Object[]>() // input items
            {
                new System.Object[] {"Feather", 3},
                new System.Object[] {"Spider", 2},
                new System.Object[] {"Ruby", 5},
                new System.Object[] {"Dragon_blood", 1},
                new System.Object[] {"Goblin_Eye", 1},
                new System.Object[] {"Red_Mushroom", 2}
            },
            new List<System.Object[]>() // output items
            {
                new System.Object[] {"Potion of Gigantification", 1}
            },
            4, // level num
            new List<string>() // starting dialog
            {
                "It seems like I accidently left some random items in the input...",
                "You can use the CLR command to clear the items in a container, just specify what container to clear and it'll empty it!"
            },
            new List<string>() // ending dialog
            {
                "Sorry about that mess... hopefully it won't happen again"
            }
        ));
        levels[4].SetCompleted(true);

        // Level 5 Combining items
        levels.Add(new Level(
            new List<System.Object[]>() // input items
            {
                new System.Object[] {"Blue_Mushroom", 1},
                new System.Object[] {"Spider", 1},
                new System.Object[] {"Blue_Mushroom", 1}
            },
            new List<System.Object[]>() // output items
            {
                new System.Object[] {"Potion of Shrinking"}
            },
            5, // level num
            new List<string>() // starting dialog
            {
                "It seems like the mushrooms got split into two piles, but they'll need to be combined before they go into the cauldron",
                "You can use the MOV command to combine them into a singular pile"
            },
            new List<string>() // ending dialog
            {
                "Wow youre getting the hang of this"
            }
        ));
        levels[5].SetCompleted(true);

        // Level 6 Batch crafting
        levels.Add(new Level(
            new List<System.Object[]>() // input items
            {
                new System.Object[] {"Black_Feather", 6},
                new System.Object[] {"Dragon_Blood", 3},
                new System.Object[] {"Ruby", 3}
            },
            new List<System.Object[]>() // output items
            {
                new System.Object[] {"Potion of Dragonification", 3}
            },
            6, // level num
            new List<string>() // starting dialog
            {
                "I'm gonna need a few Potions of Dragonification, you can batch craft them like you were crafting any other potion"
            },
            new List<string>() // ending dialog
            {
                "Batch crafting can be a huge time saver, try to use it whenever possible"
            }
        ));
        levels[6].SetCompleted(true);

        // Level 7 Final test
        levels.Add(new Level(
            new List<System.Object[]>() // input items
            {
                new System.Object[] {"Goblin_Eye", 1},
                new System.Object[] {"Green_Mushroom", 1},
                new System.Object[] {"Feather", 2},
                new System.Object[] {"Spider", 2},
                new System.Object[] {"Feather", 2},
                new System.Object[] {"Amethyst", 1},
                new System.Object[] {"Bone", 1},
                new System.Object[] {"Feather", 1}
            },
            new List<System.Object[]>() // output items
            {
                new System.Object[] {"Potion of X-Ray Vision", 1},
                new System.Object[] {"Potion of Love", 1},
                new System.Object[] {"Potion of Weightlessness", 2}
            },
            7, // level num
            new List<string>() // starting dialog
            {
                "Here's your first real test, I have a lot of potions I need so lets see how you can handle this..."
            },
            new List<string>() // ending dialog
            {
                "Impressive!",
                "You handled that really well, I see a bright future for you..."
            }
        ));
        levels[7].SetCompleted(true);
    }
}
