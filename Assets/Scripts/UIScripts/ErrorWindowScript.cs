using UnityEngine;

public class ErrorWindowScript : MonoBehaviour
{
    public GameObject lineNumber;
    public GameObject errorMessage;

    public void setErrorMessage(int line, string message)
    {
        lineNumber.GetComponent<TMPro.TMP_Text>().text = "Error on line: " + line.ToString();
        errorMessage.GetComponent<TMPro.TMP_Text>().text = message;
        openWindow();
    }
    
    public void openWindow()
    {
        gameObject.SetActive(true);
    }

    public void closeWindow()
    {
        gameObject.SetActive(false);
    }
}
