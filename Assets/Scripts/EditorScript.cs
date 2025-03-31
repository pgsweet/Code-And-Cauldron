using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class EditorScript : MonoBehaviour
{
    private bool toggled = false;
    public float editorWidth = 900.0f;
    private GameObject openEditorButton;
    private GameObject textInput;
    private GameObject runButton;
    private GameObject clearButton;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        openEditorButton = transform.Find("Open Editor Button").gameObject;  
        textInput = transform.Find("Editor Text Input").gameObject;
        runButton = transform.Find("Run Button").gameObject;
        clearButton = transform.Find("Clear Button").gameObject;
    }

    public void toggleEditor()
    {
        int moveDirection = -1;
        if (toggled)
        {
            moveDirection = 1;
        }

        openEditorButton.transform.position += new Vector3(editorWidth * moveDirection, 0, 0);
        textInput.transform.position += new Vector3(editorWidth * moveDirection, 0, 0);
        runButton.transform.position += new Vector3(editorWidth * moveDirection, 0, 0);
        clearButton.transform.position += new Vector3(editorWidth * moveDirection, 0, 0);

        toggled = !toggled;
    }

    public void runCode()
    {
        string rawCode = textInput.GetComponent<UnityEngine.UI.InputField>().text;

        if (string.IsNullOrEmpty(rawCode))
        {
            Debug.LogError("Code is empty.");
            return;
        }

        List<List<string>> parsedCode = parseCode(rawCode);
        foreach (List<string> line in parsedCode)
        {
            // Process each line of code here
            Debug.Log(string.Join(",", line));
        }
    }

    public void clearCode()
    {
        textInput.GetComponent<UnityEngine.UI.InputField>().text = string.Empty;
        Debug.Log("Code cleared.");
    }

    private List<List<string>> parseCode(string code)
    {
        List<List<string>> parsedCode = new List<List<string>>();
        string[] lines = code.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (string line in lines)
        {
            // Split the line by spaces and add it to the parsed code
            List<string> parsedLine = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).ToList();
            parsedCode.Add(parsedLine);
        }

        return parsedCode;
    }

    
}
