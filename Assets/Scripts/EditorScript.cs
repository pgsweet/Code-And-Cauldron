using UnityEngine;
using UnityEngine.EventSystems;

public class EditorScript : MonoBehaviour
{
    private bool toggled = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void toggleEditor()
    {
        int moveDirection = -1;
        if (toggled)
        {
            moveDirection = 1;
        }
        
        GameObject button = transform.Find("Open Editor Button").gameObject;  
        GameObject background = transform.Find("Square").gameObject;
        float editorSize = background.GetComponent<Transform>().localScale.x;

        button.transform.position += new Vector3(editorSize * 18 * moveDirection, 0, 0);
        background.transform.position += new Vector3(editorSize * moveDirection, 0, 0);




        toggled = !toggled;
    }
}
