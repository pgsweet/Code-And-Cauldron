using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class InputScript : MonoBehaviour
{
    private List<System.Object[]> inputItems = new List<System.Object[]>();
    
    public GameObject inputText;
    public GameObject inputCount;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(false);
        inputText.SetActive(false);
        inputCount.SetActive(false);

        // TEMP DEBUG CODE
        setInput(new List<System.Object[]> {
            new System.Object[] {"5", 2, -1},
            new System.Object[] {"3", 1, -1},
            new System.Object[] {"1", 4, -1}
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

        gameObject.SetActive(true);
        inputText.SetActive(true);
        inputCount.SetActive(true);
    }

    private void setFields()
    {
        if (inputItems.Count == 0)
        {
            Debug.LogError("No more input items.");
            gameObject.SetActive(false);
            inputText.SetActive(false);
            inputCount.SetActive(false);
            return;
        }

        Sprite newSprite = Resources.Load<Sprite>("Items/" + inputItems[0][0].ToString());
        if (newSprite == null)
        {
            Debug.LogError("Sprite not found: " + inputItems[0][0].ToString());
            return;
        }
        gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
        inputText.GetComponent<Text>().text = inputItems[0][0].ToString();
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
}
