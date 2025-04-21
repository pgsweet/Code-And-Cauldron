using System.Collections.Generic;
using System.IO;
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


    void Start()
    {
        updateButtonListeners();
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
        string level = EventSystem.current.currentSelectedGameObject.GetComponentInChildren<TMPro.TMP_Text>().text;
        Debug.LogError(level);
    }
}
