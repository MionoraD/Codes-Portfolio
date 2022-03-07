using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    protected int itemId;
    protected GameObject objItem;

    protected bool playerhasitem;
    public bool HasItem
    {
        get { return playerhasitem;  }
        private set {
            playerhasitem = value;

            LevelManager.Level.UpdateUI();
        }
    }

    public void PickUpItem()
    {
        HasItem = true;
    }

    public void LoseItem()
    {
        HasItem = false;
    }

    public GameObject FindObject()
    {
        return objItem;
    }
}
