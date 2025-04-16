using System.Collections.Generic;
using UnityEngine;

public class CauldronScript : MonoBehaviour
{
    public GameObject infoPanel;

    private List<System.Object[]> currentItems = new List<System.Object[]>();

    void Start()
    {
        infoPanel.SetActive(false);
    }

    public int Count()
    {
        return currentItems.Count;
    }

    public List<System.Object[]> getItems()
    {
        return currentItems;
    }

    public void Add(System.Object[] item)
    {
        currentItems.Add(item);
        if (infoPanel.activeSelf)
        {
            enableInfoPanel();
        }
    }

    public void Clear()
    {
        currentItems.Clear();
        if (infoPanel.activeSelf)
        {
            enableInfoPanel();
        }
    }

    public void enableInfoPanel()
    {
        string newText = "Name: cauldron\nContains:\n";
        if (currentItems.Count == 0)
        {
            newText += "Nothing";
        }
        else
        {
            foreach (System.Object[] item in currentItems)
            {
                newText += item[1].ToString() + " " + item[0].ToString().Replace("_", " ") + "(s)\n";
            }
        }

        Transform[] children = infoPanel.GetComponentsInChildren<Transform>(true);
        children[1].gameObject.GetComponent<TMPro.TMP_Text>().text = newText;

        infoPanel.SetActive(true);
    }

    public void disableInfoPanel()
    {
        infoPanel.SetActive(false);
    }
}