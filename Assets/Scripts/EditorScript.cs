using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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
    public ContainerScript[] containers = new ContainerScript[4];
    public InputScript inputItems;


    void Start()
    {
        openEditorButton = transform.Find("Open Editor Button").gameObject;  
        textInput = transform.Find("Editor Text Input").gameObject;
        runButton = transform.Find("Run Button").gameObject;
        clearButton = transform.Find("Clear Button").gameObject;
        checkContainersButton = transform.Find("Check Button").gameObject;

        if (containers[0] == null || containers[1] == null || containers[2] == null || containers[3] == null)
        {
            Debug.LogError("Container not found.");
            return;
        }
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
            if (isContainerEmpty(containers[i]))
            {
                Debug.Log("Container " + (i+1) + " is empty.");
            }
            else
            {
                Debug.Log("Container " + (i+1) + " has " + containers[i].getItemCount() + " " + containers[i].getItemName() + "(s).");
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

            inputItems.decrementInputLife();

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
        if (isContainerEmpty(containers[container1Number]))
        {
            Debug.LogError("Container 1 is empty.");
            return;
        }
        // check if container2 is not a cauldron and not empty or does not contain a similar item
        if (command[2] != "cauldron" && !(isContainerEmpty(containers[container2Number]) || isSameItem(containers[container1Number], containers[container2Number])))
        {
            Debug.LogError("Container 2 is not empty or does not contain a similar item.");
            return;
        }
        
        // check if the user inputed an amount of items to move
        if (command.Count() == 3){
            // add the number of items in arg1 to arg3
            int itemsToMove = containers[container1Number].getItemCount();
            command.Add(itemsToMove.ToString());
        }

        // check if the user inputed a valid number of items to move
        if (Int32.Parse(command[3]) > containers[container1Number].getItemCount())
        {
            Debug.LogError("Cannot move more items than are in the container.");
            return;
        }

        // move items 
        int numItemsToMove = Int32.Parse(command[3]);
        string itemName = containers[container1Number].getItemName();
        if (command[2] == "cauldron")
        {
            cauldron.Add(new System.Object[] { itemName, numItemsToMove });
            containers[container1Number].addToItem(-numItemsToMove);
        }
        else
        {
            // check if container 2 is empty, if it has been just do .setItem()
            // if not, add the number of items to the container2 and subtract from container1
            if (isContainerEmpty(containers[container2Number]))
            {
                containers[container2Number].setItem(itemName, numItemsToMove);
                containers[container1Number].addToItem(-numItemsToMove);
            }
            else
            {
                containers[container2Number].addToItem(numItemsToMove);
                containers[container1Number].addToItem(-numItemsToMove);
            }
        }
        Debug.Log($"MOV command ran: {command[1]}, {command[2]}, {numItemsToMove}");
    }

// TODO:
    private void botCommand(List<string> command)
    {
        if (command.Count() < 2)
        {
            Debug.LogError("BOT command requires at least 2 arguments.");
            return;
        }
        if (command.Count() > 3)
        {
            Debug.LogError("BOT command accepts a maximum of 3 arguments.");
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

        // make sure arg1 is an empty container
        if (!isContainerEmpty(containers[container1Number]))
        {
            Debug.LogError($"Container{container1Number} is not empty.");
            return;
        }

        // check if user inputed a number of potions to bottle
        if (command.Count() == 2)
        {
            // add the number of items in arg1 to arg3
            command.Add("1");
        }   

        // TODO: check if cauldron contains valid a valid recipie

        // TODO: create the potions and place them into arg1
        Debug.LogError("BOT command not finsihed.");
        return;
    }

// TODO:
    private void splCommand(List<string> command)
    {
        if (command.Count() != 1)
        {
            Debug.LogError("SPL command requires exactly 1 argument.");
            return;
        }
        // TODO: Cast spell and remove items from cauldron
        Debug.LogError("SPL command not implemented.");
        return;
    }

    private void clrCommand(List<string> command)
    {
        // check if the command has exactly 2 arguments
        if (command.Count() != 2)
        {
            Debug.LogError("CLR command requires exactly 2 arguments.");
            return;
        }

        // make sure arg1 is a valid container or cauldron
        int container1Number = -1;
        bool arg2HasNum = Int32.TryParse(command[1].Substring(command[1].Length-1), out container1Number);
        container1Number -= 1; // Adjust for zero-based index
        if (!(command[1].Substring(0,command[1].Length-1) == "container" && arg2HasNum && container1Number >= 0 && container1Number <= 3) &&
        !(command[1] == "cauldron"))
        {
            Debug.LogError("Must have a container or cauldron in the second argument.");
            return;
        }

        // make sure arg1 has items
        if (command[1] == "cauldron" && cauldron.Count() == 0)
        {
            Debug.LogError("Cauldron is already empty.");
            return;
        }
        else if (command[1] != "cauldron" && isContainerEmpty(containers[container1Number]))
        {
            Debug.LogError($"Container {container1Number} is already empty.");
            return;
        }

        if (command[1] == "cauldron")
        {
            // remove all items from the cauldron
            cauldron.Clear();
            Debug.Log("Cauldron cleared.");
        }
        else 
        {
            containers[container1Number].removeItem();
        }

        Debug.Log($"CLR command ran: {command[1]}");
        return;
    }

    private void inpCommand(List<string> command)
    {
        // check if theres 1 argument
        if (command.Count() != 2)
        {
            Debug.LogError("INP command requires exactly 1 argument.");
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

        // make sure theres an item to input
        if (inputItems.getInputCount() == 0)
        {
            Debug.LogError("No items to input.");
            return;
        }

        System.Object[] nextItem = inputItems.getInput();


        // make sure arg1 is either empty or has the same item
        if (!isContainerEmpty(containers[container1Number]) && containers[container1Number].getItemName() != nextItem[0].ToString())
        {
            Debug.LogError($"Container {container1Number} is not empty or does not contain a similar item.");
            return;
        }

        // pop the first input item and add it to the container
        string itemName = nextItem[0].ToString();
        int itemAmount = (int)nextItem[1];
        if (isContainerEmpty(containers[container1Number]))
        {
            containers[container1Number].setItem(itemName, itemAmount);
        }
        else
        {
            containers[container1Number].addToItem(itemAmount);
        }
 

        Debug.Log("inp command ran: " + command[1] + ", " + itemName + ", " + itemAmount);
        return;
    }

// TODO:
    private void outCommand(List<string> command)
    {
        // check if has at least 1 argument
        if (command.Count() < 2)
        {
            Debug.LogError("OUT command requires at least 1 argument.");
            return;
        }
        // check if has at most 2 arguments
        if (command.Count() > 3)
        {
            Debug.LogError("OUT command accepts a maximum of 2 arguments.");
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

        // make sure arg1 is not empty
        if (isContainerEmpty(containers[container1Number]))
        {
            Debug.LogError($"Container {container1Number} is empty.");
            return;
        }
        // check if user inputed a number of items to output
        if (command.Count() == 2)
        {
            // add the number of items in arg1 to arg3
            command.Add("1");
        }
        // check if user inputed a valid number of items to output
        if (Int32.Parse(command[2]) > containers[container1Number].getItemCount())
        {
            Debug.LogError("Cannot output more items than are in the container.");
            return;
        }

        // TODO: move arg2 items from arg1 to the output
        Debug.LogError("OUT command not finsihed.");
        return;
    }

    // HELPER FUNCTIONS
    private bool isContainerEmpty(ContainerScript container)
    {
        return container.getItemCount() == 0;
    }

    private bool isSameItem(ContainerScript container1, ContainerScript container2)
    {
        return container1.getItemName() == container2.getItemName();
    }

}
