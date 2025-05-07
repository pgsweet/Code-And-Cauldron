using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class AccordionTextBodyScript : MonoBehaviour
{
    public void initalize()
    {
        TMP_Text textComponent = gameObject.GetComponent<TMP_Text>();

        string text = textComponent.text;

        Vector2 textSize = textComponent.GetPreferredValues(text);
        
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(rectTransform.sizeDelta.x, textSize.y);
    }

}
