using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftingRecipe : MonoBehaviour
{
    //private static CompoundItems inventort;
    public GameObject compoundItems;
    public int[] inventorySlot = new int[8];
    public GameObject player;
    public InventoryHolder inventoryHolder;
    public int stackSize1, stackSize2;
    private static CraftingRecipe crafting;
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("Player");
        inventoryHolder = player.GetComponent<InventoryHolder>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public (int, int) RefreshAmount(InventoryItemData item)
    {
        for(int i = 0; i<8; i++){
           inventorySlot[i] = inventoryHolder.InventorySystem.InventorySlots[i].StackSize;
        }
        switch(item.DisplayName){
            case("棉花糖"):
                Debug.Log("棉花糖");
                stackSize1 = inventorySlot[4];
                stackSize2 = inventorySlot[2];
                Debug.Log(inventorySlot[0]);
                break;
            case("鯛魚燒"):
                stackSize1 = inventorySlot[0];
                stackSize2 = inventorySlot[1];
                break;
            case("蘋果糖"):
                stackSize1 = inventorySlot[3];
                stackSize2 = inventorySlot[2];
                break;
            case("章魚燒"):
                stackSize1 = inventorySlot[5];
                stackSize2 = inventorySlot[2];
                break;
            case("刨冰"):
                stackSize1 = inventorySlot[6];
                stackSize2 = inventorySlot[7];
                break;
        }
    //    if(item.DisplayName == "棉花糖"){
    //     Debug.Log("棉花糖");
    //     //crafting.stackSize1 = inventorySlot[0].ToString();
    //     Debug.Log(inventorySlot[0]);
        
    //    }
    //    else if(item.DisplayName == "鯛魚燒"){
    //     Debug.Log("鯛魚燒");
    //     //crafting.stackSize1 = inventorySlot_UI.AssignedInventorySlot.StackSize.ToString();
    //    }
    //    else if(item.DisplayName == "蘋果糖"){
    //     Debug.Log("蘋果糖");
    //     //crafting.stackSize1 = inventorySlot_UI.AssignedInventorySlot.StackSize.ToString();
    //    }
    //    else if(item.DisplayName == "章魚燒"){
    //     Debug.Log("章魚燒");
    //     //crafting.stackSize1 = inventorySlot_UI.AssignedInventorySlot.StackSize.ToString();
    //    }
    //    else{
    //     //crafting.stackSize1 = inventorySlot_UI.AssignedInventorySlot.StackSize.ToString();
    //    }
       return(stackSize1, stackSize2);
    }
}
