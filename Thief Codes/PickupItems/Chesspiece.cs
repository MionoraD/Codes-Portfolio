using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Chesspiece : Item
{
    private GameObject piece;
    private string name;
    private string color;
    private int boxPlace;

    public Chesspiece(int id, string _name, string _color, int box, GameObject chesspiece)
    {
        itemId = id;
        playerhasitem = false;

        name = _name;
        color = _color;
        boxPlace = box;

        objItem = chesspiece;
    }

    public bool CheckNameColor(string _name, string _color)
    {
        if(_name.Equals(name) && _color.Equals(color))
            return true;
        else
            return false;
    }

    public bool CheckHasChessPiece(string _name, string _color)
    {
        if (_name.Equals(name) && _color.Equals(color) && HasItem)
            return true;
        else
            return false;
    }
}
