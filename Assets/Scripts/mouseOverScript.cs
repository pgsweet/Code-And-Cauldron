using UnityEngine;
using UnityEngine.EventSystems;

public class mouseOverScript : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    public GameObject thingToHoverOver;
        public void OnPointerEnter(PointerEventData data)
        {
            if (thingToHoverOver.GetComponent<ContainerScript>() != null)
            {
                thingToHoverOver.GetComponent<ContainerScript>().enableInfoPanel();
            }
            else if (thingToHoverOver.GetComponent<OutputScript>() != null)
            {
                thingToHoverOver.GetComponent<OutputScript>().enableInfoPanel();
            }
            else if (thingToHoverOver.GetComponent<InputScript>() != null)
            {
                thingToHoverOver.GetComponent<InputScript>().enableInfoPanel();
            }
            else if (thingToHoverOver.GetComponent<CauldronScript>() != null)
            {
                thingToHoverOver.GetComponent<CauldronScript>().enableInfoPanel();
            }
        }

        public void OnPointerExit(PointerEventData data)
        {   
            if (thingToHoverOver.GetComponent<ContainerScript>() != null)
            {
                thingToHoverOver.GetComponent<ContainerScript>().disableInfoPanel();
            }
            else if (thingToHoverOver.GetComponent<OutputScript>() != null)
            {
                thingToHoverOver.GetComponent<OutputScript>().disableInfoPanel();
            }
            else if (thingToHoverOver.GetComponent<InputScript>() != null)
            {
                thingToHoverOver.GetComponent<InputScript>().disableInfoPanel();
            }
            else if (thingToHoverOver.GetComponent<CauldronScript>() != null)
            {
                thingToHoverOver.GetComponent<CauldronScript>().disableInfoPanel();
            }
        }
}