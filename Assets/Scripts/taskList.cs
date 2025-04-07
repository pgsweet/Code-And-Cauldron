using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TaskList : MonoBehaviour
{
    public Text taskText;
    private List<string> tasks = new List<string>();

    void Start()
    {
        // Example Tasks
        tasks.Add("Create a Healing Potion");
        tasks.Add("Cast Fireball Spell");
        UpdateTaskList();
    }

    public void AddTask(string task)
    {
        tasks.Add(task);
        UpdateTaskList();
    }

    public void CompleteTask(int taskIndex)
    {
        if (taskIndex >= 0 && taskIndex < tasks.Count)
        {
            tasks.RemoveAt(taskIndex);
            UpdateTaskList();
        }
    }

    private void UpdateTaskList()
    {
        taskText.text = "Tasks:\n";
        foreach (string task in tasks)
        {
            taskText.text += "- " + task + "\n";
        }
    }
}