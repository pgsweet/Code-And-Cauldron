using UnityEngine;
using UnityEngine.UI;

public class MenuButtonScript : MonoBehaviour
{
    public GameObject codeEditor;
    public GameObject levelSelect;
    public GameObject levelSelectButton;
    public GameObject codeEditorButton;
    public GameObject helpPanel;
    public GameObject recipeBook;
    public GameObject taskList;
    public GameObject recipeBookButton;
    public GameObject taskListButton;


    [ContextMenu("Open Editor")]
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

    public void openRecipeBook()
    {
        if (recipeBook == null)
        {
            Debug.LogError("RecipeBook not found.");
        }

        recipeBook.GetComponent<RecipeBookScript>().toggleRecipeBook();
        taskList.GetComponent<TaskListScript>().toggleTaskList();
    }

    public void runCode()
    {
        if (codeEditor == null)
        {
            Debug.LogError("Editor not found.");
            return;
        }
        codeEditor.GetComponent<EditorScript>().runCode();
        taskList.GetComponent<TaskListScript>().closeIfOpen();
        recipeBook.GetComponent<RecipeBookScript>().closeIfOpen();
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

    public void enableRecipeBook()
    {
        recipeBook.SetActive(true);
        taskList.SetActive(false);
        recipeBookButton.GetComponent<Button>().interactable = false;
        taskListButton.GetComponent<Button>().interactable = true;
    }

    public void enableTaskList()
    {
        recipeBook.SetActive(false);
        taskList.SetActive(true);
        recipeBookButton.GetComponent<Button>().interactable = true;
        taskListButton.GetComponent<Button>().interactable = false;
        taskList.GetComponent<TaskListScript>().updateTaskList();
    }

    public void toggleHelp()
    {
        if (helpPanel == null)
        {
            Debug.LogError("Help panel not found.");
            return;
        }
        helpPanel.SetActive(!helpPanel.activeSelf);
    }


}
