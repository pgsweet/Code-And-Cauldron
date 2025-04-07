using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ContainerScript : MonoBehaviour
{

    private string itemName = null;
    private int itemCount = 0;

    public GameObject numItemsText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(false);
        numItemsText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setItem(string newItemName, int newItemCount)
    {
        itemName = newItemName;
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
    }

    public void addToItem(int count)
    {
        // add to the item count in the container
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
        numItemsText.GetComponent<Text>().text = "0";
        itemName = null;
        itemCount = 0;
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
