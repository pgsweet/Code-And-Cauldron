using UnityEngine;

public class TaskListScript : MonoBehaviour
{
    private bool toggled = false;

    public GameObject taskPanel;

    public void toggleTaskList()
    {
        int moveDirection = 1;
        if (toggled)
        {
            moveDirection = -1;
        }

        float panelWidth = taskPanel.GetComponent<RectTransform>().rect.width;

        gameObject.transform.localPosition += new Vector3(panelWidth * moveDirection, 0, 0);

        toggled = !toggled;
    }
}
