using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMap : Item
{
    private string name;

    public LevelMap(int id, string _name, GameObject levelMap)
    {
        itemId = id;
        playerhasitem = false;

        name = _name;

        objItem = levelMap;
    }

    public bool HasMap(string _name)
    {
        if (name.Equals(_name) && HasItem) return true;
        else return false;
    }
}
