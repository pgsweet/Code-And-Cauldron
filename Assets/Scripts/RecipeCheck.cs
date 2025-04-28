using System.Collections.Generic;


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

public class RecipeCheck
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
    private List<Spell> SpellList = new List<Spell>();


    public System.Object[] CheckRecipe(List<System.Object[]> inputItems) 
    {
        string craftName = null;
        bool foundCraft = false;
        int numCrafts = -1;

        foreach (Potion potion in PotionList)
        {
            List<System.Object[]> requiredItems = potion.GetRequiredItems();
            foreach (System.Object[] item in requiredItems)
            {
                bool validItems = true;
                for (int i = 0; i < inputItems.Count; i++)
                {
                    if (!MatchItem(item, inputItems[i]))
                    {
                        validItems = false;
                        break;
                    }
                    else
                    {
                        if (numCrafts == -1)
                        {
                            numCrafts = (int)((int)inputItems[i][1] / (int)item[1]);
                        }
                        else if (numCrafts != (int)((int)inputItems[i][1] / (int)item[1]))
                        {
                            validItems = false;
                            break;
                        }
                    }
                }
                if (validItems)
                {
                    craftName = potion.GetName();
                    foundCraft = true;
                    break;
                }
                else {
                    break;
                }
            }
            if (foundCraft)
            {
                break;
            }
        }
        // TODO: check spells

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
}