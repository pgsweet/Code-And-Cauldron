using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TaskListScript : MonoBehaviour
{
    private bool toggled = false;

    public GameObject taskPanel;
    public GameObject taskText;
    public OutputScript outputScript;

    public void toggleTaskList()
    {
        int moveDirection = 1;
        if (toggled)
        {
            moveDirection = -1;
        }

        float panelWidth = taskPanel.GetComponent<RectTransform>().rect.width;

        gameObject.transform.localPosition += new Vector3(panelWidth * moveDirection, 0, 0);

        toggled = !toggled;

        updateTaskList();
    }

    public void updateTaskList()
    {
        // TODO: check this
        string text = "";
        List<string> lines = outputScript.convertToText();
        foreach (string line in lines)
        {
            text += "- " + line + "\n";
        }

        taskText.GetComponent<TMP_Text>().text = text;
    }

    public void closeIfOpen()
    {
        if (toggled)
        {
            this.toggleTaskList();
        }
    }
}
