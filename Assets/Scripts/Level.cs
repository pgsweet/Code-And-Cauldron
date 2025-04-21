using System.Collections.Generic;

public class Level
{
    List<System.Object[]> inputItems = new List<System.Object[]>();
    List<System.Object[]> requiredOutputItems = new List<System.Object[]>();
    bool completed = false;
    int score = 0;
    int levelNum = 0;
    List<string> dialogue = null;

    public Level(List<System.Object[]> inputItems, List<System.Object[]> requiredOutputItems, int levelNum, List<string> dialogue)
    {
        this.inputItems = inputItems;
        this.requiredOutputItems = requiredOutputItems;
        this.levelNum = levelNum;
        this.dialogue = dialogue;
    }

    public Level(List<System.Object[]> inputItems, List<System.Object[]> requiredOutputItems, int levelNum)
    {
        this.inputItems = inputItems;
        this.requiredOutputItems = requiredOutputItems;
        this.levelNum = levelNum;
    }

    public List<string> GetDialogue()
    {
        return dialogue;
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

    public void SetInputItems(List<System.Object[]> inputItems)
    {
        this.inputItems = inputItems;
    }

    public void SetRequiredOutputItems(List<System.Object[]> requiredOutputItems)
    {
        this.requiredOutputItems = requiredOutputItems;
    }

    public void SetLevelNum(int levelNum)
    {
        this.levelNum = levelNum;
    }

    public void SetDialogue(List<string> dialogue)
    {
        this.dialogue = dialogue;
    }


}