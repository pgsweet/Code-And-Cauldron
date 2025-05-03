using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RecipeBook : MonoBehaviour
{
    public Text recipeText;
    private Dictionary<string, string> recipes = new();

    void Start()
    {
        // Example Recipes
        recipes.Add("Healing Potion", "Water + Herb + Crystal");
        recipes.Add("Fireball Spell", "Fire Essence + Wand + Powder");

        UpdateRecipeBook();
    }

    public void AddRecipe(string recipeName, string ingredients)
    {
        if (!recipes.ContainsKey(recipeName))
        {
            recipes.Add(recipeName, ingredients);
            UpdateRecipeBook();
        }
    }

    private void UpdateRecipeBook()
    {
        recipeText.text = "Recipes:\n";
        foreach (var recipe in recipes)
        {
            recipeText.text += recipe.Key + ": " + recipe.Value + "\n";
        }
    }
}