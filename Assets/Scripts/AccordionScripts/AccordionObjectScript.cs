using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class AccordionObjectScript : MonoBehaviour, IPointerClickHandler
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

    public void disableText()
    {
        foreach (Transform c in this.transform)
        {
            if (c.GetComponent<AccordionTextBodyScript>() != null)
            {
                c.gameObject.SetActive(false);
            }
        }
    }

    public void disableArrow()
    {
        foreach (Transform c in this.transform)
        {
            if (c.GetComponent<AccordionTitleScript>() != null)
            {
                c.GetComponent<AccordionTitleScript>().disableArrow();
            }
        }
    }

    public void OnPointerClick(PointerEventData pointerEventData)
    {
        foreach (Transform c in this.transform)
        {
            if (c.GetComponent<AccordionTextBodyScript>() != null)
            {
                c.gameObject.SetActive(!c.gameObject.activeSelf);
            }
            else if (c.GetComponent<AccordionTitleScript>() != null) 
            {
                c.GetComponent<AccordionTitleScript>().toggleArrow();
            }
        }

        GameObject parent = this.transform.parent.gameObject;

        foreach (Transform child in parent.transform)
        {
            if (child != this.transform)
            {
                child.GetComponent<AccordionObjectScript>().disableText();
                child.GetComponent<AccordionObjectScript>().disableArrow();
            }
        }

        parent.GetComponent<AccordionScript>().updateObjects();
    }
}
