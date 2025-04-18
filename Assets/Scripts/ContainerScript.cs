using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using System.Linq;

public class ContainerScript : MonoBehaviour
{
    private string itemName = null;
    private int itemCount = 0;

    public GameObject numItemsText;
    public GameObject itemNameText;
    public GameObject spriteContainer;
    public GameObject infoPanel;

    void Start()
    {
        spriteContainer.SetActive(false);
        numItemsText.SetActive(false);
        itemNameText.SetActive(false);
        infoPanel.SetActive(false);
    }

    void Update()
    {

    }

    public void setItem(string newItemName, int newItemCount)
    {
        itemName = newItemName;
        itemCount = newItemCount;
        itemNameText.GetComponent<TMP_Text>().text = newItemName.Replace("_", " ");
        numItemsText.GetComponent<Text>().text = newItemCount.ToString();

        Sprite newSprite = Resources.Load<Sprite>("Items/" + newItemName);
        if (newSprite == null)
        {
            Debug.LogError("Sprite not found: " + newItemName);
            return;
        }
        spriteContainer.GetComponent<SpriteRenderer>().sprite = newSprite;

        spriteContainer.SetActive(true);
        numItemsText.SetActive(true);
        itemNameText.SetActive(true);

        if (infoPanel.activeSelf)
        {
            enableInfoPanel();
        }
    }

    public void addToItem(int count)
    {
        itemCount += count;
        numItemsText.GetComponent<Text>().text = itemCount.ToString();
        if (itemCount <= 0)
        {
            removeItem();
        }
    }

    public void removeItem()
    {
        spriteContainer.SetActive(false);
        numItemsText.SetActive(false);
        itemNameText.SetActive(false);

        spriteContainer.GetComponent<SpriteRenderer>().sprite = null;
        itemName = null;
        itemNameText.GetComponent<TMP_Text>().text = "Empty";
        itemCount = 0;
        numItemsText.GetComponent<Text>().text = "0";

    }

    public string getItemName()
    {
        return itemName;
    }

    public int getItemCount()
    {
        return itemCount;
    }


    public void enableInfoPanel()
    {
        string newText = $"Name: {gameObject.name.ToLower().Replace(" ", "")}\nContains:\n";
        if (itemName != null)
        {
            newText += $"{itemCount} {itemName.Replace("_", " ")}(s)";
        }
        else
        {
            newText += "Nothing";
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
