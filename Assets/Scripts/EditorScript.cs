using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;


public static class CommandConstants
{
    public const string MOV = "mov";
    public const string BOT = "bot";
    public const string SPL = "spl";
    public const string CLR = "clr";
    public const string INP = "inp";
    public const string OUT = "out";
}

public class EditorScript : MonoBehaviour
{
    private bool toggled = false;
    public float editorWidth = 900.0f;
    private GameObject openEditorButton;
    private GameObject textInput;
    private GameObject runButton;
    private GameObject clearButton;

    // COMMANDS
    private string[][] commands = {
        new string[] {"mov", "NON_EMPTY", "CAULDRON CONTAINER SIMILAR", "NUMBER ALL"},
        new string[] {"bot", "EMPTY", "NUMBER ONE"},
        new string[] {"spl"},
        new string[] {"clr", "CAULDRON CONTAINER"},
        new string[] {"inp", "CONTAINER SIMILAR"},
        new string[] {"out", "NON_EMPTY", "NUMBER ALL"}
    };

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
        // foreach (List<string> line in parsedCode)
        // {
        //     // Process each line of code here
        //     Debug.Log(string.Join(",", line));
        // }

        for (int i = 0; i < parsedCode.Count; i++)
        {
            switch(parsedCode[i][0])
            {
                case (CommandConstants.MOV):
                    mov(parsedCode[i].ToArray());
                    Debug.Log("MOV command executed.");
                    break;
                case (CommandConstants.BOT):
                    Debug.Log("BOT command executed.");
                    break;
                case (CommandConstants.SPL):
                    Debug.Log("SPL command executed.");
                    break;
                case (CommandConstants.CLR):
                    Debug.Log("CLR command executed.");
                    break;
                case (CommandConstants.INP):
                    Debug.Log("INP command executed.");
                    break;
                case (CommandConstants.OUT):
                    Debug.Log("OUT command executed.");
                    break;
                default:
                    Debug.LogError("Unknown command: " + parsedCode[i][0]);
                    break;
            }
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

    private void mov(string[] command)
    {
        
    }

}
