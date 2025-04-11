using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;

public class MenuButtonScript : MonoBehaviour
{
    public GameObject codeEditor;
    public GameObject levelSelect;
    public GameObject levelSelectButton;
    public GameObject codeEditorButton;

    void Start()
    {

    }
    public void openEditor()
    {
        // get the editor from the tag and call the open editor function
        if (codeEditor == null)
        {
            Debug.LogError("Editor not found.");
            return;
        }
        if (levelSelect == null)
        {
            Debug.LogError("Level select not found.");
            return;
        }
        codeEditor.GetComponent<EditorScript>().toggleEditor();
        levelSelect.GetComponent<LevelSelectScript>().toggleLevelSelect();
    }

    public void runCode()
    {
        if (codeEditor == null)
        {
            Debug.LogError("Editor not found.");
            return;
        }
        codeEditor.GetComponent<EditorScript>().runCode();
    }

    public void clearCode()
    {
        if (codeEditor == null)
        {
            Debug.LogError("Editor not found.");
            return;
        }
        codeEditor.GetComponent<EditorScript>().clearCode();
    }

    public void checkContainers()
    {
        if (codeEditor == null)
        {
            Debug.LogError("Editor not found.");
            return;
        }
        codeEditor.GetComponent<EditorScript>().checkContainers();
    }

    public void enableCodeEditor()
    {
        codeEditor.SetActive(true);
        levelSelect.SetActive(false);
        levelSelectButton.GetComponent<Button>().interactable = true;
        codeEditorButton.GetComponent<Button>().interactable = false;
    }

    public void enableLevelSelect()
    {
        codeEditor.SetActive(false);
        levelSelect.SetActive(true);
        levelSelectButton.GetComponent<Button>().interactable = false;
        codeEditorButton.GetComponent<Button>().interactable = true;
    }
}
