using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InputScript : MonoBehaviour
{
    private List<System.Object[]> inputItems;
    public GameObject inputText;
    public GameObject inputCount;
    public GameObject spriteContainer;
    public GameObject infoPanel;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        spriteContainer.SetActive(false);
        inputText.SetActive(false);
        inputCount.SetActive(false);
        infoPanel.SetActive(false);

        // TEMP DEBUG CODE
        setInput(new List<System.Object[]> {
            new System.Object[] {"Feather", 2, -1},
            new System.Object[] {"Feather", 2, -1}
        });
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setInput(List<System.Object[]> newInputItems)
    {
        inputItems = newInputItems;
        
        setFields();

        if (infoPanel.activeSelf)
        {
            enableInfoPanel();
        }

        spriteContainer.SetActive(true);
        inputText.SetActive(true);
        inputCount.SetActive(true);
    }

    private void setFields()
    {
        if (inputItems.Count == 0)
        {
            // Debug.LogError("No more input items.");
            spriteContainer.SetActive(false);
            inputText.SetActive(false);
            inputCount.SetActive(false);
            return;
        }

        Sprite newSprite = Resources.Load<Sprite>("Items/" + inputItems[0][0].ToString());
        if (newSprite == null)
        {
            newSprite = Resources.Load<Sprite>("Potions/" + inputItems[0][0].ToString());
            if (newSprite == null)
            {
                Debug.LogError("Sprite not found: " + inputItems[0][0].ToString());
                return;
            }
        }
        spriteContainer.GetComponent<SpriteRenderer>().sprite = newSprite;
        inputText.GetComponent<TMP_Text>().text = inputItems[0][0].ToString().Replace("_", " ");
        inputCount.GetComponent<Text>().text = inputItems[0][1].ToString();
    }

    public System.Object[] getInput()
    {
        if (inputItems.Count == 0)
        {
            Debug.LogError("No more input items.");
            return null;
        }
        System.Object[] nextItem = inputItems[0];
        inputItems.RemoveAt(0);
        setFields();
        if (infoPanel.activeSelf)
        {
            enableInfoPanel();
        }
        return nextItem;
    }

    public int getInputCount()
    {
        return inputItems.Count;
    }

    public void decrementInputLife()
    {
        for (int i = 0; i < inputItems.Count; i++) 
            {
                if ((int)inputItems[i][2] == -1)
                {
                    continue;
                }
                // cannot be less than the position its in
                if ((int)inputItems[i][2] >= i) 
                {
                    inputItems[i][2] = (int)inputItems[i][2] - 1;
                }

                if ((int)inputItems[i][2] == 0)
                {
                    Debug.LogError("Item " + inputItems[i][0] + " has expired.");
                    inputItems.RemoveAt(i);
                    i--;
                }
            }
    }

    public void enableInfoPanel()
    {  
        string newText = $"Remaining Input:\n";
        if (inputItems.Count != 0)
        {
            for (int i = 0; i < inputItems.Count; i++)
            {
                newText += $"{inputItems[i][0].ToString().Replace("_", " ")}(s): {inputItems[i][1]}\n";
            }
        }
        else
        {
            newText += "None.";
        }

        Transform[] children = infoPanel.GetComponentsInChildren<Transform>(true);
        children[1].gameObject.GetComponent<TMP_Text>().text = newText;

        infoPanel.SetActive(true);
        
    }

    public void disableInfoPanel()
    {
        infoPanel.SetActive(false);
    }
}
