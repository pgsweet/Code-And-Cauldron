using System.Collections.Generic;
using UnityEngine;

public class DialogScript : MonoBehaviour
{
    private List<string> dialogLines = new List<string>();
    private int currentLineIndex = -1;
    public GameObject dialogText;

    public void startGame()
    {
        gameObject.SetActive(false);
    }

    public void setDialog(List<string> dialogLines)
    {
        this.dialogLines = dialogLines;
        currentLineIndex = 0;
        openDialog();
    }

    public void nextLine()
    {
        currentLineIndex++; 
        if (currentLineIndex < dialogLines.Count)
        {
            dialogText.GetComponent<TMPro.TMP_Text>().text = dialogLines[currentLineIndex];
        }
        else
        {
            closeDialog();
        }
    }

    public void openDialog()
    {
        if (dialogLines == null)
        {
            return;
        }

        if (dialogLines.Count > 0)
        {
            dialogText.GetComponent<TMPro.TMP_Text>().text = dialogLines[0];
            gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Dialog lines are empty.");
        }
    }

    public void closeDialog()
    {
        gameObject.SetActive(false);
    }
}
