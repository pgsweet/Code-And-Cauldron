using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using Unity.VisualScripting;
using System.Linq;

public class OutputScript : MonoBehaviour
{
    private List<System.Object[]> requiredItems = new List<System.Object[]>();
    public GameObject outputText;
    public GameObject outputCount;
    public GameObject spriteContainer;
    public GameObject infoPanel;
    private List<System.Object[]> currentOutputItems = new List<System.Object[]>();
    public LevelSelectScript LevelSelectScript;

    public void startGame()
    {
        spriteContainer.SetActive(false);
        outputText.SetActive(false);
        outputCount.SetActive(false);
        infoPanel.SetActive(false);
    }

    public void recieveOutput(System.Object[] item)
    {
        currentOutputItems.Add(item);
        setFields();
        if (infoPanel.activeSelf)
        {
            enableInfoPanel();
        }
    }

    public void setRequiredItems(List<System.Object[]> newRequiredItems)
    {
        requiredItems = newRequiredItems;
        currentOutputItems.Clear();
        setFields();
        if (infoPanel.activeSelf)
        {
            enableInfoPanel();
        }
    }

    private void setFields()
    {
        if (currentOutputItems.Count == 0)
        {
            spriteContainer.SetActive(false);
            outputText.SetActive(false);
            outputCount.SetActive(false);
            return;
        }

        System.Object[] currItem = currentOutputItems[currentOutputItems.Count - 1];

        if (char.ToLower(currItem[0].ToString()[0]) == 'p')
        {
            spriteContainer.transform.localScale = new Vector3(450, 450);
        }
        else
        {
            spriteContainer.transform.localScale = new Vector3(800, 800);
        }

        Sprite newSprite = Resources.Load<Sprite>("Items/" + currItem[0].ToString());
        if (newSprite == null)
        {
            newSprite = Resources.Load<Sprite>("Potions/" + currItem[0].ToString());
            if (newSprite == null)
            {
                Debug.LogError("Sprite not found: " + currItem[0].ToString());
                return;
            }
        }
        spriteContainer.GetComponent<SpriteRenderer>().sprite = newSprite;
        outputText.GetComponent<TMP_Text>().text = currItem[0].ToString().Replace("_", " ");
        outputCount.GetComponent<Text>().text = currItem[1].ToString();

        spriteContainer.SetActive(true);
        outputText.SetActive(true);
        outputCount.SetActive(true);
    }

    public void checkOutput()
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
                break;
            }
        }
        if (currentOutputItems.Count < requiredItems.Count)
        {
            goodOutput = false;
            errorMessage = "Not enough output items, expected: " + requiredItems.Count + " but got: " + currentOutputItems.Count;
        }

        if (goodOutput)
        {
            LevelSelectScript.completedLevel();
        }
        else
        {
            Debug.LogError(errorMessage);
        }

    }

    public void enableInfoPanel()
    {
        string newText = "Required Output:\n";
        
        List<string> t = convertToText();

        foreach (string line in t)
        {
            newText += line + '\n';
        }

        Transform[] children = infoPanel.GetComponentsInChildren<Transform>(true);
        children[1].gameObject.GetComponent<TMP_Text>().text = newText;

        infoPanel.SetActive(true);
    }

    public List<string> convertToText()
    {
        // TODO: check this
        List<string> text = new List<string>();
        for (int i = 0; i < requiredItems.Count; i++)
        {
            string t = requiredItems[i][1].ToString() + " " + requiredItems[i][0].ToString().Replace("_", " ") + "(s),";
            if (i < currentOutputItems.Count && currentOutputItems[0][0].ToString() == requiredItems[i][0].ToString() && Int32.Parse(currentOutputItems[0][1].ToString()) == Int32.Parse(requiredItems[i][1].ToString()))
            {
                t += " Done";
            }
            else
            {
                t += " Not Done";
            }
            text.Add(t);
        }
        return text;
    }

    public void disableInfoPanel()
    {
        infoPanel.SetActive(false);
    }
}
