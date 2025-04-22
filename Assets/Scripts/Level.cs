using System.Collections.Generic;

public class Level
{
    private List<System.Object[]> inputItems = new List<System.Object[]>();
    private List<System.Object[]> requiredOutputItems = new List<System.Object[]>();
    private bool completed = false;
    private int score = 0;
    private int levelNum = 0;
    private List<string> startingDialogue = null;
    private List<string> endDialogue = null;

    public Level(List<System.Object[]> inputItems, List<System.Object[]> requiredOutputItems, int levelNum, List<string> startDialogue, List<string> endDialogue)
    {
        this.inputItems = inputItems;
        this.requiredOutputItems = requiredOutputItems;
        this.levelNum = levelNum;
        this.startingDialogue = startDialogue;
        this.endDialogue = endDialogue;
    }

    public Level(List<System.Object[]> inputItems, List<System.Object[]> requiredOutputItems, int levelNum, List<string> startDialogue)
    {
        this.inputItems = inputItems;
        this.requiredOutputItems = requiredOutputItems;
        this.levelNum = levelNum;
        this.startingDialogue = startDialogue;
    }

    public Level(List<System.Object[]> inputItems, List<System.Object[]> requiredOutputItems, int levelNum)
    {
        this.inputItems = inputItems;
        this.requiredOutputItems = requiredOutputItems;
        this.levelNum = levelNum;
    }

    public List<string> getStartingDialogue()
    {
        return startingDialogue;
    }

    public List<string> getEndDialogue()
    {
        return endDialogue;
    }

    public int GetLevelNum()
    {
        return levelNum;
    }

    public List<System.Object[]> GetInputItems()
    {
        return inputItems;
    }

    public List<System.Object[]> GetRequiredOutputItems()
    {
        return requiredOutputItems;
    }

    public bool IsCompleted()
    {
        return completed;
    }

    public void SetCompleted(bool completed)
    {
        this.completed = completed;
    }

    public int GetScore()
    {
        return score;
    }

    public void SetScore(int score)
    {
        this.score = score;
    }

    // public void SetInputItems(List<System.Object[]> inputItems)
    // {
    //     this.inputItems = inputItems;
    // }

    // public void SetRequiredOutputItems(List<System.Object[]> requiredOutputItems)
    // {
    //     this.requiredOutputItems = requiredOutputItems;
    // }

    // public void SetLevelNum(int levelNum)
    // {
    //     this.levelNum = levelNum;
    // }

    // public void SetDialogue(List<string> startingDialogue)
    // {
    //     this.startingDialogue = startingDialogue;
    // }

    // public void SetEndDialogue(List<string> endDialogue)
    // {
    //     this.endDialogue = endDialogue;
    // }

}