using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapCollect : MonoBehaviour
{
    private Transform item;
    [SerializeField] private GameObject mapItem;

    void Update()
    {
        if (item == null) return;

        mapItem.SetActive(item.gameObject.activeSelf);
    }

    public void SetItem(Transform itm)
    {
        item = itm;
    }
}
