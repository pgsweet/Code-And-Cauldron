using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class OutputScript : MonoBehaviour
{

    private List<System.Object[]> requiredItems = new List<System.Object[]>() {
        new System.Object[] {"Black_feather", 2},
    };
    public GameObject outputText;
    public GameObject outputCount;
    private List<System.Object[]> currentOutputItems = new List<System.Object[]>();

    void Start()
    {
        gameObject.SetActive(false);
        outputText.SetActive(false);
        outputCount.SetActive(false);
    }


    void Update()
    {
        
    }

    public void recieveOutput(System.Object[] item)
    {
        currentOutputItems.Add(item);
        setFields();
        checkOutput();
    }

    public void setRequiredItems(List<System.Object[]> newRequiredItems)
    {
        requiredItems = newRequiredItems;
    }

    private void setFields()
    {
        System.Object[] currItem = currentOutputItems[currentOutputItems.Count - 1];
        Sprite newSprite = Resources.Load<Sprite>("Items/" + currItem[0].ToString());
        if (newSprite == null)
        {
            Debug.LogError("Sprite not found: " + currItem[0].ToString());
            return;
        }
        gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
        outputText.GetComponent<TMP_Text>().text = currItem[0].ToString().Replace("_", " ");
        outputCount.GetComponent<Text>().text = currItem[1].ToString();

        gameObject.SetActive(true);
        outputText.SetActive(true);
        outputCount.SetActive(true);
    }

    private void checkOutput()
    {
        bool goodOutput = true;
        string errorMessage = "";
        for (int i = 0; i < currentOutputItems.Count; i++)
        {
            if (requiredItems.Count <= i)
            {
                goodOutput = false;
                errorMessage = "Too many output items, expected: " + requiredItems.Count + " but got: " + currentOutputItems.Count;
            }
            else if (Int32.Parse(currentOutputItems[i][1].ToString()) != Int32.Parse(requiredItems[i][1].ToString()) || currentOutputItems[i][0].ToString() != requiredItems[i][0].ToString())
            {
                goodOutput = false;
                errorMessage = "Incorrect Output, expected: " + requiredItems[i][0].ToString() + " " + requiredItems[i][1].ToString() + " but got: " + currentOutputItems[i][0].ToString() + " " + currentOutputItems[i][1].ToString();
                // TODO: reset the level
            }
            
        }
        if (goodOutput)
        {
            Debug.Log("Correct Output so far");
        }
        else
        {
            Debug.LogError(errorMessage);
        }

    }
}
