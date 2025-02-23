using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class Synthesizer : PlacableObject
{
    [SerializeField] SynthButton synthButtonPrefab;
    [SerializeField] GameObject craftingUI;
    [SerializeField] int colCount; //how many buttons wide the layout group is
    [SerializeField] RectTransform buttonList;
    [SerializeField] GridLayoutGroup layout;

    bool menuOpen;

    private void Update()
    {
        if(Global.inMenu && menuOpen && Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E))
        {
            //close the crafting window

            craftingUI.SetActive(false);
            Global.inMenu = false;
            menuOpen = false;
        }
    }

    public override void Interact()
    {
        base.Interact();

        //instantiate crafting menu
        craftingUI.SetActive(true);
        Global.inMenu = true;
        menuOpen = true;

        UpdateScreen();
    }

    public void UpdateScreen()
    {
        //go through all the synthbuttons and see if they have the necessary materials to be crafted

        int added = 0;

        //remove the existing item buttons

        foreach (Transform child in buttonList)
        {
            Destroy(child.gameObject);
        }

        layout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        layout.constraintCount = colCount;

        //add the item buttons
        foreach (Item item in CraftingManager.Instance.GetUnlockedRecipes())
        {
            SynthButton tmp = Instantiate(synthButtonPrefab, buttonList);

            tmp.SetItem(item);

            if (++added % colCount == 0)
            {
                buttonList.sizeDelta = new Vector2(buttonList.sizeDelta.x, buttonList.sizeDelta.y + layout.cellSize.y + layout.spacing.y);
            }
        }
    }
}
