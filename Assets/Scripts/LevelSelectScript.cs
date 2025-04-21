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
    public InputScript inputScript;
    public OutputScript outputScript;
    public EditorScript editorScript;


    void Start()
    {
        updateButtonListeners();
        initalizeLevels();
    }

    private void updateButtonListeners()
    {
        for (int i = 0; i < levelButtons.Count; i++)
        {
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
        Debug.Log($"Switching to level {levelNum}");
        
        if (levels.Count <= levelNum)
        {
            Debug.Log(levels.Count);
            Debug.LogError($"Level {levelNum} does not exist in the levels list.");
            return;
        }

        inputScript.setInput(levels[levelNum].GetInputItems());
        outputScript.setRequiredItems(levels[levelNum].GetRequiredOutputItems());
        
        editorScript.clearAllContainers();
    }

    public void initalizeLevels()
    {
        Level level0 = new Level(
            new List<System.Object[]>()
            {
                new System.Object[] { "Amethyst", 1, -1 },

            },
            new List<System.Object[]>()
            {
                new System.Object[] { "Amethyst", 1 },
            },
            0,
            new List<string>() 
            {
                "So youre the new apprentice huh?",
                "Well I guess I need to train you on how to create my potions and spells.",
                "Let's start off with the INP and OUT commands.",
                "The INP command inputs the item from the input teleporter and places it into the container you specify.",
                "The OUT command takes the item from the container and places it into the output teleporter.",
                "Give it a try with the Amethyst I've given you."
            }
        );
        levels.Add(level0);
    }
}
