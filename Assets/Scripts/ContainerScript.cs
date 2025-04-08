using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ContainerScript : MonoBehaviour
{

    private string itemName = null;
    private int itemCount = 0;

    public GameObject numItemsText;
    public GameObject itemNameText;

    void Start()
    {
        gameObject.SetActive(false);
        numItemsText.SetActive(false);
        itemNameText.SetActive(false);
    }

    void Update()
    {
        
    }

    public void setItem(string newItemName, int newItemCount)
    {
        itemName = newItemName;
        itemNameText.GetComponent<Text>().text = itemName;
        itemCount = newItemCount;
        numItemsText.GetComponent<Text>().text = itemCount.ToString();

        Sprite newSprite = Resources.Load<Sprite>("Items/" + itemName);
        if (newSprite == null)
        {
            Debug.LogError("Sprite not found: " + itemName);
            return;
        }
        gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;

        gameObject.SetActive(true);
        numItemsText.SetActive(true);
        itemNameText.SetActive(true);
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
        gameObject.SetActive(false);
        numItemsText.SetActive(false);
        gameObject.GetComponent<SpriteRenderer>().sprite = null;
        itemName = null;
        itemNameText.GetComponent<Text>().text = "Empty";
        itemCount = 0;
        numItemsText.GetComponent<Text>().text = "0";
        itemNameText.SetActive(false);
    }

    public string getItemName()
    {
        return itemName;
    }

    public int getItemCount()
    {
        return itemCount;
    }



}
