using Unity.VisualScripting;
using UnityEngine;

public class ContainerScript : MonoBehaviour
{

    private string itemName = null;
    private int itemCount = 0;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setItem(string newItemName, int newItemCount)
    {
        itemName = newItemName;
        itemCount = newItemCount;

        Sprite newSprite = Resources.Load<Sprite>("Items/" + itemName);
        if (newSprite == null)
        {
            Debug.LogError("Sprite not found: " + itemName);
            return;
        }
        gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;

        gameObject.SetActive(true);
    }

    public void addToItem(int count)
    {
        // add to the item count in the container
        itemCount += count;
        if (itemCount <= 0)
        {
            removeItem();
        }
    }

    public void removeItem()
    {
        itemCount = 0;
        itemName = null;
        gameObject.SetActive(false);
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
