using UnityEngine;

public class AccordionScript : MonoBehaviour
{
    void Start()
    {
        updateObjects();
    }

    public void updateObjects()
    {
        updateList();
        setTotalHeight();
        
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        float parentHeight = gameObject.transform.parent.GetComponent<RectTransform>().sizeDelta.y;
        rectTransform.localPosition = new Vector2(0, (parentHeight / 2) - (rectTransform.sizeDelta.y / 2) - 120);
    }

    private void updateList()
    {
        foreach (Transform child in this.transform)
        {
            if (child.parent == transform)
            {
                child.GetComponent<AccordionObjectScript>().initalize();
            }
        }
    }

    private void setTotalHeight()
    {
        float totalHeight = 0;
        foreach (Transform child in this.transform)
        {
            if (child.parent == transform)
            {
                totalHeight += child.GetComponent<RectTransform>().rect.height;
            }
        }
        
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, totalHeight);
    }
}
