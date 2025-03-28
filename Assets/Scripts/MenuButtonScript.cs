using UnityEngine;

public class MenuButtonScript : MonoBehaviour
{
    private GameObject editor;

    void Start()
    {
        editor = GameObject.FindGameObjectWithTag("Editor");
    }
    public void openEditor()
    {
        // get the editor from the tag and call the open editor function
        if (editor == null)
        {
            Debug.LogError("Editor not found.");
            return;
        }
        editor.GetComponent<EditorScript>().toggleEditor();
    }

    public void runCode()
    {
        if (editor == null)
        {
            Debug.LogError("Editor not found.");
            return;
        }
        editor.GetComponent<EditorScript>().runCode();
    }
}
