using Unity.VisualScripting;
using UnityEngine;

public class AccordionObjectScript : MonoBehaviour
{
    public void initalize()
    {
        foreach (Transform c in this.transform)
        {
            GameObject child = c.gameObject;
            if (child.GetComponent<AccordionTextBodyScript>() != null)
            {
                child.GetComponent<AccordionTextBodyScript>().initalize();
            }
        }
        updateSize();
    }

    private void updateSize()
    {   
        float totalHeight = 0;
        foreach (Transform child in this.transform)
        {
            if (child.gameObject.activeSelf)
            {
                totalHeight += child.GetComponent<RectTransform>().rect.height;
            }
        }

        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, totalHeight);
    }

    void OnMouseDown()
    {
        Debug.Log("Clicked");
        foreach (Transform c in this.transform)
        {
            if (c.GetComponent<AccordionTextBodyScript>() != null)
            {
                c.gameObject.SetActive(c.gameObject.activeSelf);
            }
        }
    }
}
