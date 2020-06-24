using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemControl : MonoBehaviour
{
    [SerializeField] private bool chessPiece, levelMap, location;
    [SerializeField] private int itemId;
    [SerializeField] private GameObject itemUI;
    [SerializeField] private Transform itemLocation;
    [SerializeField] private CircleColor circle;

    private Item item;
    private Inventory inventory;
    bool hasItem = false;

    void Start()
    {
        itemUI.SetActive(false);

        GameObject objInventory = GameObject.Find("Inventory");
        if(objInventory != null)
        {
            inventory = objInventory.GetComponent<Inventory>();
        }
    }

    void Update()
    {
        if(!hasItem && inventory.FilledPieces)
        {
            if (chessPiece) item = inventory.FindChessPiece(itemId);
            else if (levelMap) item = inventory.FindLevelMap(itemId);

            if (item != null)
            {
                Instantiate(item.FindObject(), itemLocation);
                circle.ChangeCircle(chessPiece);
            }

            hasItem = true;
        }
    }

    // When player is within the bounds of the item
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            PlayerMovement movement = other.gameObject.GetComponent<PlayerMovement>();
            if (movement != null)
            {
                // Tell the PlayerMovement script that there is an item that can be picked up
                movement.AddItem(this);
                itemUI.SetActive(true);
            }
        }
    }
    // When player leaves the bounds of the item
    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            PlayerMovement movement = other.gameObject.GetComponent<PlayerMovement>();
            if (movement != null)
            {
                // Tell the PlayerMovement script that they can no longer use this item
                movement.RemoveItem(this);
                itemUI.SetActive(false);
            }
        }
    }

    public virtual void ItemAction()
    {
        item.PickUpItem();
        LevelManager.Level.GiveMessage("Picked up an item");
        LevelManager.Level.UpdateRoad();
        gameObject.SetActive(false);
    }

    public void ResetItem()
    {
        item.LoseItem();
        LevelManager.Level.UpdateRoad();
    }

    public bool HasItem()
    {
        return item.HasItem;
    }

    public bool IsChessPiece()
    {
        return chessPiece;
    }
}
