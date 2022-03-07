using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayersInMenu : MonoBehaviour
{
    private int _maxplayers = 8;
    private int _currentplayers = 0;
    [SerializeField] private GameObject menuItem;

    void Start()
    {
        Transform playerListTransform = GameObject.Find("PlayerLoader").transform;
        Debug.Log(playerListTransform.childCount);

    }

    void Update()
    {
        Debug.Log(transform.childCount);
    }

    public GameObject AddPlayerToMenu()
    {
        GameObject item = null;

        if(_currentplayers < _maxplayers)
        {
            item = Instantiate(menuItem, transform);
            _currentplayers++;
        }
        else
        {
            Debug.Log("Max players reached");
            return null;
        }

        return item;
    }
}
