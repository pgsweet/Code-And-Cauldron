using UnityEngine;

public class AccordionTitleScript : MonoBehaviour
{
    private bool toggled = false;

    public void toggleArrow()
    {
        foreach (Transform c in this.transform)
        {
            if (c.gameObject.name == "Arrow")
            {
                toggled = !toggled;
                int rotateDirection = 1;
                if (toggled)
                {
                    rotateDirection = -1;
                }

                c.Rotate(0, 0, 90 * rotateDirection);
            }
        }
    }

    public void disableArrow()
    {
        if (toggled)
        {
            foreach (Transform c in this.transform)
            {
                if (c.gameObject.name == "Arrow")
                {
                    toggled = !toggled;
                    int rotateDirection = 1;
                    if (toggled)
                    {
                        rotateDirection = -1;
                    }

                    c.Rotate(0, 0, 90 * rotateDirection);
                }
            }
        }
    }
}
