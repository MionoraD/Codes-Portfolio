using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapItems : MonoBehaviour
{
    [SerializeField] private Transform items;
    [SerializeField] private GameObject prefabMapItemUI;

    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform item in items)
        {
            ItemControl control = item.GetComponent<ItemControl>();
            if(control != null)
            {
                if (control.IsChessPiece())
                {
                    GameObject mapItem = Instantiate(prefabMapItemUI, this.transform);
                    mapItem.transform.position = new Vector3(item.position.x, item.position.y, item.position.z);

                    MapCollect collect = mapItem.GetComponent<MapCollect>();
                    if (collect != null) collect.SetItem(item);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
