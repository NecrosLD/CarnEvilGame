using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    public GameObject[] InventorySlots;

    public List<GameObject> Inventory;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddItemToInventory(GameObject item)
    {
        GameObject inventoryObject = item.transform.GetComponent<ItemScript>().InventoryObject;

        int emptySlot = Inventory.Count;

        Inventory.Add(inventoryObject);

        Instantiate(inventoryObject, InventorySlots[emptySlot].transform);
    }
}
