using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelSelectScript : MonoBehaviour
{
    private bool toggled = false;
    public GameObject levelBackground;
    // formatted as:
    // { {inputItems}, {requiredOutputItems}, {completed, score?}}
    List<Level> levels = new List<Level>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
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
}
