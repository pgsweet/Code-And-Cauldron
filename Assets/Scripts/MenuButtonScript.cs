using UnityEngine;

public class MenuButtonScript : MonoBehaviour
{
    public void openEditor()
    {
        // get the editor from the tag and call the open editor function
        GameObject editor = GameObject.FindGameObjectWithTag("Editor");
        if (editor == null)
        {
            Debug.LogError("Editor not found.");
            return;
        }
        editor.GetComponent<EditorScript>().toggleEditor();
    }
}
