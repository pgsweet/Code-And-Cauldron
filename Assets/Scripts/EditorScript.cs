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
    private GameObject checkContainersButton;

    // CONTAINERS, formatted as [name, number of items]
    public List<System.Object[]> cauldron = new List<System.Object[]>{};
    private List<System.Object[]> containers = new List<System.Object[]>{
        new System.Object[] {"test", 5}, 
        new System.Object[] {"", 0}, 
        new System.Object[] {"test", 1}, 
        new System.Object[] {"", 0}
    };

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
        checkContainersButton = transform.Find("Check Button").gameObject;
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
        checkContainersButton.transform.position += new Vector3(editorWidth * moveDirection, 0, 0);

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

        List<List<string>> parsedCode = splitCode(rawCode);
        // foreach (List<string> line in parsedCode)
        // {
        //     // Process each line of code here
        //     Debug.Log(string.Join(",", line));
        // }

        parseCode(parsedCode);
    }

    public void clearCode()
    {
        textInput.GetComponent<UnityEngine.UI.InputField>().text = string.Empty;
        Debug.Log("Code cleared.");
    }

    public void checkContainers(){
        // check if the containers are empty
        for (int i = 0; i < containers.Count(); i++)
        {
            if ((int)containers[i][1] == 0)
            {
                Debug.Log("Container " + (i+1) + " is empty.");
            }
            else
            {
                Debug.Log("Container " + (i+1) + " has " + (int)containers[i][1] + " " + containers[i][0] + "(s).");
            }
        }
        // print cauldron contents
        if (cauldron.Count() == 0)
        {
            Debug.Log("Cauldron is empty.");
        }
        else
        {
            Debug.Log("Cauldron has " + cauldron.Count() + " items inside.");
            foreach (System.Object[] item in cauldron)
            {
                Debug.Log(item[0] + ": " + item[1]);
            }
        }
    }

    private List<List<string>> splitCode(string code)
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

    private void parseCode(List<List<string>> parsedCode)
    {
        foreach (List<string> line in parsedCode)
        {
            switch (line[0])
            {
                case (CommandConstants.MOV):
                    movCommand(line);
                    break;
                case (CommandConstants.BOT):
                    botCommand(line);
                    break;
                case (CommandConstants.SPL):
                    splCommand(line);
                    break;
                case (CommandConstants.CLR):
                    clrCommand(line);
                    break;
                case (CommandConstants.INP):
                    inpCommand(line);
                    break;
                case (CommandConstants.OUT):
                    outCommand(line);
                    break;
                default:
                    Debug.LogError("Unknown command: " + line[0]);
                    break;
            }
            // Debug script
            // Debug.Log(string.Join(",", line));
        }
    }

    private void movCommand(List<string> command)
    {
        if (command.Count() < 3)
        {
            Debug.LogError("MOV command requires at least 3 arguments.");
            return;
        }
        if (command.Count() > 4)
        {
            Debug.LogError("MOV command accepts a maximum of 4 arguments.");
            return;
        }

        // make sure arg1 is a valid container
        int container1Number = -1;
        bool arg1HasNum = Int32.TryParse(command[1].Substring(command[1].Length-1), out container1Number);
        container1Number -= 1; // Adjust for zero-based index
        if (!(command[1].Substring(0,command[1].Length-1) == "container" && arg1HasNum && container1Number >= 0 && container1Number <= 3))
        {
            Debug.LogError("Must have a valid container in the first argument.");
            return;
        }

        // make sure arg2 is a valid container or cauldron
        int container2Number = -1;
        bool arg2HasNum = Int32.TryParse(command[2].Substring(command[2].Length-1), out container2Number);
        container2Number -= 1; // Adjust for zero-based index
        if (!(command[2].Substring(0,command[2].Length-1) == "container" && arg2HasNum && container2Number >= 0 && container2Number <= 3) &&
        !(command[2] == "cauldron"))
        {
            Debug.LogError("Must have a container or cauldron in the second argument.");
            return;
        }

        // check if container1 has items
        if ((int)containers[container1Number][1] == 0)
        {
            Debug.LogError("Container 1 is empty.");
            return;
        }
        // check if container2 is not a cauldron and not empty or does not contain a similar item
        if (command[2] != "cauldron" && !((int)containers[container2Number][1] == 0 || containers[container2Number][0].ToString() == containers[container1Number][0].ToString()))
        {
            Debug.LogError("Container 2 is not empty or does not contain a similar item.");
            return;
        }
        
        // check if the user inputed an amount of items to move
        if (command.Count() == 3){
            // add the number of items in arg1 to arg3
            int itemsToMove = (int)containers[container1Number][1];
            command.Add(itemsToMove.ToString());
        }

        // check if the user inputed a valid number of items to move
        if (Int32.Parse(command[3]) > (int)containers[container1Number][1])
        {
            Debug.LogError("Cannot move more items than are in the container.");
            return;
        }



        // move items 
        int numItemsToMove = Int32.Parse(command[3]);
        string itemName = containers[container1Number][0].ToString();
        if (command[2] == "cauldron")
        {
            cauldron.Add(new System.Object[] { itemName, numItemsToMove });
            containers[container1Number][1] = (int)containers[container1Number][1] - numItemsToMove;
            if ((int)containers[container1Number][1] == 0)
            {
                containers[container1Number][0] = "";
            }
        }
        else
        {
            containers[container2Number][0] = itemName;
            containers[container2Number][1] = (int)containers[container2Number][1] + numItemsToMove;
            containers[container1Number][1] = (int)containers[container1Number][1] - numItemsToMove;
            if ((int)containers[container1Number][1] == 0)
            {
                containers[container1Number][0] = "";
            }
        }
        Debug.Log($"MOV command ran: {command[1]}, {command[2]}, {numItemsToMove}");
    }

    private void botCommand(List<string> command)
    {
        Debug.LogError("BOT command not implemented.");
        return;
    }

    private void splCommand(List<string> command)
    {
        Debug.LogError("SPL command not implemented.");
        return;
    }

    private void clrCommand(List<string> command)
    {
        Debug.LogError("CLR command not implemented.");
        return;
    }

    private void inpCommand(List<string> command)
    {
        Debug.LogError("INP command not implemented.");
        return;
    }

    private void outCommand(List<string> command)
    {
        Debug.LogError("OUT command not implemented.");
        return;
    }

}
