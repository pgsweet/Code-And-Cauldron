using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class LevelSelectScript : MonoBehaviour
{
    private bool toggled = false;
    public GameObject panel;
    // formatted as:
    // { {inputItems}, {requiredOutputItems}, {completed, score?}}
    private List<List<List<System.Object[]>>> levels = new List<List<List<System.Object[]>>>()
    {
        new List<List<System.Object[]>>()
        {
            new List<System.Object[]>()
            {
                new System.Object[] {"Bone", 1, -1}
            },
            new List<System.Object[]>()
            {
                new System.Object[] {"Bone", 1, -1}
            },
            new List<System.Object[]>()
            {
                new System.Object[] {false, 0}
            }

        }
    };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        panel = transform.Find("Panel").gameObject;
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

        float rectWidth = panel.GetComponent<RectTransform>().rect.width;

        gameObject.transform.localPosition += new Vector3(moveDirection * rectWidth, 0, 0);

        toggled = !toggled;
    }
}
