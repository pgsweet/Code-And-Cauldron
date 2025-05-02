using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
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
    
    public GameObject codeEditorButton;
    public GameObject levelSelectButton;

    public GameObject textInput;
    public MenuButtonScript menuButtonScript;
    public ErrorWindowScript errorWindowScript;

    // public List<System.Object[]> cauldron = new List<System.Object[]>{};
    public CauldronScript cauldron;
    public ContainerScript[] containers = new ContainerScript[4];
    public InputScript inputItems;
    public OutputScript outputScript;
    public LevelSelectScript levelSelectScript;


    public void startGame()
    {
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

        float textInputWidth = textInput.GetComponent<RectTransform>().rect.width;

        gameObject.transform.localPosition += new Vector3(textInputWidth * moveDirection, 0, 0);
        codeEditorButton.transform.localPosition += new Vector3(textInputWidth * moveDirection, 0, 0);
        levelSelectButton.transform.localPosition += new Vector3(textInputWidth * moveDirection, 0, 0);

        toggled = !toggled;
    }

    public void runCode()
    {
        string rawCode = textInput.GetComponent<TMP_InputField>().text;

        if (string.IsNullOrEmpty(rawCode))
        {
            Debug.LogError("Code is empty.");
            return;
        }

        levelSelectScript.resetLevel();

        List<List<string>> parsedCode = splitCode(rawCode);
        // menuButtonScript.openEditor();

        StartCoroutine(parseCode(parsedCode));
    }

    public void clearCode()
    {
        textInput.GetComponent<TMP_InputField>().text = string.Empty;
        Debug.Log("Code cleared.");
    }

    // unused function used for debugging
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
            foreach (System.Object[] item in cauldron.getItems())
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

    System.Collections.IEnumerator parseCode(List<List<string>> parsedCode)
    {
        yield return new WaitForSeconds(0.5f);
        int lineCount = 0;
        foreach (List<string> line in parsedCode)
        {
            string error = null;
            switch (line[0])
            {
                case (CommandConstants.MOV):
                    error = movCommand(line);
                    break;
                case (CommandConstants.BOT):
                    error = botCommand(line);
                    break;
                case (CommandConstants.SPL):
                    error = splCommand(line);
                    break;
                case (CommandConstants.CLR):
                    error = clrCommand(line);
                    break;
                case (CommandConstants.INP):
                    error = inpCommand(line);
                    break;
                case (CommandConstants.OUT):
                    error = outCommand(line);
                    break;
                default:
                    Debug.LogError("Unknown command: " + line[0]);
                    error = "Unknown command: " + line[0];
                    break;
            }

            // inputItems.decrementInputLife();

            if (error != null) {
                errorWindowScript.setErrorMessage(lineCount, error);
                break;
            }

            yield return new WaitForSeconds(1f);

            // Debug script
            // Debug.Log(string.Join(",", line));
            lineCount++;
        }
        outputScript.checkOutput();
    }

    private string movCommand(List<string> command)
    {
        if (command.Count() < 3)
        {
            Debug.LogError("MOV command requires at least 3 arguments.");
            return "MOV command requires at least 3 arguments.";
        }
        if (command.Count() > 4)
        {
            Debug.LogError("MOV command accepts a maximum of 4 arguments.");
            return "MOV command accepts a maximum of 4 arguments.";
        }

        // make sure arg1 is a valid container
        int container1Number = -1;
        bool arg1HasNum = Int32.TryParse(command[1].Substring(command[1].Length-1), out container1Number);
        container1Number -= 1; // Adjust for zero-based index
        if (!(command[1].Substring(0,command[1].Length-1) == "container" && arg1HasNum && container1Number >= 0 && container1Number <= 3))
        {
            Debug.LogError("Must have a valid container in the first argument.");
            return "Must have a valid container in the first argument.";
        }

        // make sure arg2 is a valid container or cauldron
        int container2Number = -1;
        bool arg2HasNum = Int32.TryParse(command[2].Substring(command[2].Length-1), out container2Number);
        container2Number -= 1; // Adjust for zero-based index
        if (!(command[2].Substring(0,command[2].Length-1) == "container" && arg2HasNum && container2Number >= 0 && container2Number <= 3) &&
        !(command[2] == "cauldron"))
        {
            Debug.LogError("Must have a container or cauldron in the second argument.");
            return "Must have a container or cauldron in the second argument.";
        }

        // check if container1 has items
        if (isContainerEmpty(containers[container1Number]))
        {
            Debug.LogError($"Container{container1Number} is empty.");
            return $"Container{container1Number} is empty.";
        }
        // check if container2 is not a cauldron and not empty or does not contain a similar item
        if (command[2] != "cauldron" && !(isContainerEmpty(containers[container2Number]) || isSameItem(containers[container1Number], containers[container2Number])))
        {
            Debug.LogError($"Container{container2Number} is not empty or does not contain a similar item.");
            return $"Container{container2Number} is not empty or does not contain a similar item.";
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
            return "Cannot move more items than are in the container.";
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
        return null;
    }

    private string botCommand(List<string> command)
    {
        if (command.Count() < 2)
        {
            Debug.LogError("BOT command requires at least 2 arguments.");
            return "BOT command requires at least 2 arguments.";
        }
        if (command.Count() > 3)
        {
            Debug.LogError("BOT command accepts a maximum of 3 arguments.");
            return "BOT command accepts a maximum of 3 arguments.";
        }
        // make sure arg1 is a valid container
        int container1Number = -1;
        bool arg1HasNum = Int32.TryParse(command[1].Substring(command[1].Length-1), out container1Number);
        container1Number -= 1; // Adjust for zero-based index
        if (!(command[1].Substring(0,command[1].Length-1) == "container" && arg1HasNum && container1Number >= 0 && container1Number <= 3))
        {
            Debug.LogError("Must have a valid container in the first argument.");
            return "Must have a valid container in the first argument.";
        }

        // make sure arg1 is an empty container
        if (!isContainerEmpty(containers[container1Number]))
        {
            Debug.LogError($"Container{container1Number} is not empty.");
            return $"Container{container1Number} is not empty.";
        }

        // check if user inputed a number of potions to bottle
        if (command.Count() == 2)
        {
            // add the number of items in arg1 to arg3
            command.Add("1");
        }   
        RecipeCheck recipeCheck = new RecipeCheck();
        System.Object[] craftedPotion = recipeCheck.CheckRecipe(cauldron.getItems());

        if (craftedPotion[0] == null)
        {
            Debug.LogError("Cauldron does not contain a valid recipe.");
            return "Cauldron does not contain a valid recipe.";
        }

        string potionName = craftedPotion[0].ToString();
        int itemAmount = (int)craftedPotion[1];
        if (isContainerEmpty(containers[container1Number]))
        {
            containers[container1Number].setItem(potionName, itemAmount);
            cauldron.Clear();
        }
        else
        {
            containers[container1Number].addToItem(itemAmount);
            cauldron.Clear();
        }

        Debug.Log("BOT command ran: " + command[1] + ", " + potionName + ", " + itemAmount);
        return null;
    }

// TODO:
    private string splCommand(List<string> command)
    {
        if (command.Count() != 1)
        {
            Debug.LogError("SPL command requires exactly 1 argument.");
            return "SPL command requires exactly 1 argument.";
        }
        // TODO: Cast spell and remove items from cauldron
        Debug.LogError("SPL command not implemented.");
        return null;
    }

    private string clrCommand(List<string> command)
    {
        // check if the command has exactly 2 arguments
        if (command.Count() != 2)
        {
            Debug.LogError("CLR command requires exactly 2 arguments.");
            return "CLR command requires exactly 2 arguments.";
        }

        // make sure arg1 is a valid container or cauldron
        int container1Number = -1;
        bool arg2HasNum = Int32.TryParse(command[1].Substring(command[1].Length-1), out container1Number);
        container1Number -= 1; // Adjust for zero-based index
        if (!(command[1].Substring(0,command[1].Length-1) == "container" && arg2HasNum && container1Number >= 0 && container1Number <= 3) &&
        !(command[1] == "cauldron"))
        {
            Debug.LogError("Must have a container or cauldron in the second argument.");
            return "Must have a container or cauldron in the second argument.";
        }

        // make sure arg1 has items
        if (command[1] == "cauldron" && cauldron.Count() == 0)
        {
            Debug.LogError("Cauldron is already empty.");
            return "Cauldron is already empty.";
        }
        else if (command[1] != "cauldron" && isContainerEmpty(containers[container1Number]))
        {
            Debug.LogError($"Container {container1Number} is already empty.");
            return $"Container {container1Number} is already empty.";
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
        return null;
    }

    private string inpCommand(List<string> command)
    {
        // check if theres 1 argument
        if (command.Count() != 2)
        {
            Debug.LogError("INP command requires exactly 1 argument.");
            return "INP command requires exactly 1 argument.";
        }

        // make sure arg1 is a valid container
        int container1Number = -1;
        bool arg1HasNum = Int32.TryParse(command[1].Substring(command[1].Length-1), out container1Number);
        container1Number -= 1; // Adjust for zero-based index
        if (!(command[1].Substring(0,command[1].Length-1) == "container" && arg1HasNum && container1Number >= 0 && container1Number <= 3))
        {
            Debug.LogError("Must have a valid container in the first argument.");
            return "Must have a valid container in the first argument.";
        }

        // make sure theres an item to input
        if (inputItems.getInputCount() == 0)
        {
            Debug.LogError("No items to input.");
            return "No items to input.";
        }

        System.Object[] nextItem =  inputItems.getInput();


        // make sure arg1 is either empty or has the same item
        if (!isContainerEmpty(containers[container1Number]) && containers[container1Number].getItemName() != nextItem[0].ToString())
        {
            Debug.LogError($"Container {container1Number} is not empty or does not contain a similar item.");
            return $"Container {container1Number} is not empty or does not contain a similar item.";
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
        return null;
    }

    private string outCommand(List<string> command)
    {
        // check if has at least 1 argument
        if (command.Count() < 2)
        {
            Debug.LogError("OUT command requires at least 1 argument.");
            return "OUT command requires at least 1 argument.";
        }
        // check if has at most 2 arguments
        if (command.Count() > 3)
        {
            Debug.LogError("OUT command accepts a maximum of 2 arguments.");
            return "OUT command accepts a maximum of 2 arguments.";
        }
        // make sure arg1 is a valid container
        int container1Number = -1;
        bool arg1HasNum = Int32.TryParse(command[1].Substring(command[1].Length-1), out container1Number);
        container1Number -= 1; // Adjust for zero-based index
        if (!(command[1].Substring(0,command[1].Length-1) == "container" && arg1HasNum && container1Number >= 0 && container1Number <= 3))
        {
            Debug.LogError("Must have a valid container in the first argument.");
            return "Must have a valid container in the first argument.";
        }

        // make sure arg1 is not empty
        if (isContainerEmpty(containers[container1Number]))
        {
            Debug.LogError($"Container {container1Number} is empty.");
            return $"Container {container1Number} is empty.";
        }
        // check if user inputed a number of items to output
        if (command.Count() == 2)
        {
            // add the number of items in arg1 to arg3
            int itemsToMove = containers[container1Number].getItemCount();
            command.Add(itemsToMove.ToString());
        }
        // check if user inputed a valid number of items to output
        if (Int32.Parse(command[2]) > containers[container1Number].getItemCount())
        {
            Debug.LogError("Cannot output more items than are in the container.");
            return "Cannot output more items than are in the container.";
        }

        System.Object[] itemToOutput = new System.Object[2];
        itemToOutput[0] = containers[container1Number].getItemName();
        itemToOutput[1] = Int32.Parse(command[2]);

        containers[container1Number].addToItem(-Int32.Parse(command[2]));

        outputScript.recieveOutput(itemToOutput);

        Debug.Log("out command ran: " + command[1] + ", " + command[2]);
        return null;
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

    public void clearAllContainers()
    {
        for (int i = 0; i < containers.Count(); i++)
        {
            containers[i].removeItem();
        }
        cauldron.Clear();
    }

}
