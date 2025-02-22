using System;
using System.Collections.Generic;

[Serializable]
public class CraftingRecipe
{
    public string outputID;
    public int outputCount;
    public List<Ingredient> ingredients;
}

[Serializable]
public class Ingredient
{
    public string itemID;
    public int amount;
}

[Serializable]
public class RecipeList
{
    public List<CraftingRecipe> recipes;
}
