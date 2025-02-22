using System.Collections.Generic;
using UnityEngine;

public class Synthesizer : PlacableObject
{
    [SerializeField] SynthButton synthButtonPrefab;
    [SerializeField] List<Item> unlockedRecipes;
    [SerializeField] GameObject craftingUI;
    [SerializeField] GameObject buttonList;

    bool menuOpen;

    private void Update()
    {
        if(Global.inMenu && menuOpen && Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E))
        {
            //close the crafting window

            craftingUI.SetActive(false);
            Global.inMenu = false;
        }
    }

    public override void Interact()
    {
        base.Interact();

        //instantiate crafting menu
        craftingUI.SetActive(true);
        Global.inMenu = true;

        //update items on the menu screen
        foreach (var item in unlockedRecipes)
        {
            SynthButton tmp = Instantiate(synthButtonPrefab, buttonList.transform);

            tmp.SetItem(item);
        }
    }

    public void UpdateScreen()
    {
        //go through all the synthbuttons and see if they have the necessary materials to be crafted
    }
}
