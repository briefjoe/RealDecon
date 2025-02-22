using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CraftingManager : MonoBehaviour
{
    public static CraftingManager Instance;

    [SerializeField] InventoryController inventoryController;

    List<CraftingRecipe> recipes;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }

        LoadRecipes();
    }

    void LoadRecipes()
    {
        string path = Path.Combine(Application.dataPath, "Scripts", "Inventory", "Crafting", "recipes.json");
        if (File.Exists(path))
        {
            string jsonText = File.ReadAllText(path);
            RecipeList recipeList = JsonUtility.FromJson<RecipeList>("{\"recipes\":" + jsonText + "}");
            recipes = recipeList.recipes;

            Debug.Log("Loaded " + recipes.Count + " crafting recipes.");
        }
        else
        {
            Debug.LogError("Recipe file not found");
        }
    }

    public CraftingRecipe GetRecipe(string id)
    {
        return recipes.Find(r => r.outputID == id);
    }

    bool CanCraft(CraftingRecipe recipe)
    {
        foreach(Ingredient ingredient in recipe.ingredients)
        {
            Debug.Log("Checking for " + ingredient.itemID + " " + ingredient.amount);

            //check if player inventory has anitem
            if (!inventoryController.HasItem(ingredient.itemID, ingredient.amount))
            {
                return false;
            }
        }

        return true;
    }

    public void Craft(string id)
    {
        CraftingRecipe recipe = GetRecipe(id);
        if(recipe == null)
        {
            Debug.Log("Recipe not found");
            return;
        }

        if (CanCraft(recipe))
        {
            foreach(Ingredient ingeredient in recipe.ingredients)
            {
                //remove crafting item from player's inventory
                inventoryController.RemoveItems(ItemManager.Instance.GetItemList().Find(i => i.id == ingeredient.itemID), false, ingeredient.amount);
                Debug.Log("removing ingredient" + ingeredient.itemID);
            }

            //add recipe item
            inventoryController.AddItem(ItemManager.Instance.GetItemList().Find(i => i.id == recipe.outputID), false, recipe.outputCount);
            Debug.Log("adding item");
        }
    }

    public List<CraftingRecipe> GetRecipeList()
    {
        return recipes;
    }
}
