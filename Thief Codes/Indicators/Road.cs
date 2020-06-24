using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Road : MonoBehaviour
{
    public bool updating = false;

    [SerializeField] private Transform parentItems;
    private List<Transform> items;

    [SerializeField] private Transform parentIndicators;
    private List<Indicator> indicators;

    [SerializeField] private Transform parentPointers;
    private List<PointArrow> pointers;

    [SerializeField] private Transform startPosition;

    // Start is called before the first frame update
    void Start()
    {
        indicators = new List<Indicator>();
        foreach (Transform indicator in parentIndicators)
        {
            Indicator newIndicator = indicator.gameObject.GetComponent<Indicator>();
            if (newIndicator != null) indicators.Add(newIndicator);
        }

        if(parentPointers != null)
        {
            pointers = new List<PointArrow>();
            foreach (Transform pointer in parentPointers)
            {
                PointArrow newPointer = pointer.gameObject.GetComponent<PointArrow>();
                if (newPointer != null) pointers.Add(newPointer);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // UpdateRoad();
        if (updating)
        {
            UpdateRoad();
            updating = false;
        }
    }

    private void UpdateItemsList()
    {
        items = new List<Transform>();
        foreach(Transform item in parentItems)
        {
            if (item.gameObject.activeSelf)
                items.Add(item);
        }
    }

    public void UpdateRoad()
    {
        UpdateItemsList();
        foreach(Indicator item in indicators)
        {
            item.UpdateIndicator(items, startPosition);
        }

        foreach(PointArrow arrow in pointers)
        {
            arrow.UpdateIndicator(items, startPosition);
        }
    }
}
