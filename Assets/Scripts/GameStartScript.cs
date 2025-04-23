using UnityEngine;

public class GameStartScript : MonoBehaviour
{
    public LevelSelectScript levelSelectScript;
    public EditorScript editorScript;
    public CauldronScript cauldronScript;
    public InputScript inputScript;
    public OutputScript outputScript;
    public ContainerScript[] ContainerScripts;
    public DialogScript dialogScript;

    void Start()
    {
        foreach (ContainerScript containerScript in ContainerScripts)
        {
            containerScript.startGame();
        }
        cauldronScript.startGame();
        inputScript.startGame();
        outputScript.startGame();
        dialogScript.startGame();

        levelSelectScript.startGame();
        editorScript.startGame();

    }

    void Update()
    {
        
    }
}
