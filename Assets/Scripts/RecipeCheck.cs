using UnityEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;

class Potion {
    string name;
    List<System.Object[]> requiredItems = new List<System.Object[]>();

    public Potion(string name, List<System.Object[]> requiredItems) {
        this.name = name;
        this.requiredItems = requiredItems;
    }
    public string GetName() {
        return name;
    }
    public List<System.Object[]> GetRequiredItems() {
        return requiredItems;
    }
}

class Spell {
    string name;
    List<System.Object[]> requiredItems = new List<System.Object[]>();

    public Spell(string name, List<System.Object[]> requiredItems) {
        this.name = name;
        this.requiredItems = requiredItems;
    }
    public string GetName() {
        return name;
    }
    public List<System.Object[]> GetRequiredItems() {
        return requiredItems;
    }
}

public class RecipeCheck : MonoBehaviour
{
    private List<Potion> PotionList = new List<Potion>() 
    {
        new Potion(
            "Potion of Love",
            new List<System.Object[]>() 
            {
                new System.Object[] { "Bone", 1 },
                new System.Object[] { "Feather", 1 },
                new System.Object[] { "Amethyst", 1 }
            }
        ),
        new Potion(
            "Potion of X-Ray Vision",
            new List<System.Object[]>()
            {
                new System.Object[] {"Green_Mushroom", 1},
                new System.Object[] {"Spider", 2},
                new System.Object[] {"Goblin_Eye", 1}
            }
        ),
        new Potion(
            "Potion of Invisibility",
            new List<System.Object[]>()
            {
                new System.Object[] {"Green_Mushroom", 2},
                new System.Object[] {"Emerald", 1}
            }
        ),
        new Potion(
            "Potion of Shrinking",
            new List<System.Object[]>()
            {
                new System.Object[] {"Blue_Mushroom", 2},
                new System.Object[] {"Spider", 1}
            }
        ),
        new Potion(
            "Potion of Gigantification",
            new List<System.Object[]>()
            {
                new System.Object[] {"Red_Mushroom", 2},
                new System.Object[] {"Goblin_Eye", 1}
            }
        ),
        new Potion(
            "Potion of Dragonification",
            new List<System.Object[]>()
            {
                new System.Object[] {"Black_Feather", 2},
                new System.Object[] {"Dragon_Blood", 1},
                new System.Object[] {"Ruby", 1}
            }
        ),
        new Potion(
            "Potion of Weightlessness",
            new List<System.Object[]>()
            {
                new System.Object[] {"Feather", 2}
            }
        )
    };

    public System.Object[] CheckRecipe(List<System.Object[]> inputItems) 
    {
        string craftName = null;
        bool foundCraft = false;
        int numCrafts = -1;

        for (int p = 0; p < PotionList.Count && !foundCraft; p++)
        {
            Potion potion = PotionList[p];
            bool validItems = true;
            List<System.Object[]> requiredItems = potion.GetRequiredItems();
            numCrafts = -1;
            for (int i = 0; i < inputItems.Count && i < requiredItems.Count && validItems; i++)
            {
                System.Object[] item = requiredItems[i];
                if (!MatchItem(item, inputItems[i]))
                {
                    validItems = false;
                }
                else
                {
                    if (numCrafts == -1)
                    {
                        numCrafts = (int)inputItems[i][1] / (int)item[1];
                    }
                    else if (numCrafts != (int)((int)inputItems[i][1] / (int)item[1])) 
                    {
                        validItems = false;
                    }
                }
            }

            if (validItems)
            {
                craftName = potion.GetName();
                foundCraft = true;
                break;
            }
        }

        return new System.Object[] { craftName, numCrafts };
    }

    private bool MatchItem(System.Object[] item1, System.Object[] item2)
    {
        if (item1[0].Equals(item2[0]) && (int)item2[1] % (int)item1[1] == 0)
        {
            return true;
        }
        return false;
    }

    public System.Object[] getRandomPotion(){
        System.Random rnd = new System.Random();
        int potionNum = rnd.Next(0, PotionList.Count);
        Potion p = PotionList[potionNum];
        return new System.Object[] {p.GetName(), p.GetRequiredItems()};
    }
}