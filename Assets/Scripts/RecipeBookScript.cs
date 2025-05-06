using UnityEngine;

public class RecipeBookScript : MonoBehaviour
{
    private bool toggled = false;
    public GameObject bookPanel;
    public GameObject recipeBookButton;
    public GameObject taskListButton;

    public void toggleRecipeBook()
    {
        int moveDirection = 1;
        if (toggled)
        {
            moveDirection = -1;
        }

        float panelWidth = bookPanel.GetComponent<RectTransform>().rect.width;

        gameObject.transform.localPosition += new Vector3(panelWidth * moveDirection, 0, 0);
        recipeBookButton.transform.localPosition += new Vector3(panelWidth * moveDirection, 0, 0);
        taskListButton.transform.localPosition += new Vector3(panelWidth * moveDirection, 0, 0);

        toggled = !toggled;
    }
}
