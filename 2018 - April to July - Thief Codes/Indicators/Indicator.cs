using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indicator : MonoBehaviour
{
    private Renderer rendering;

    [SerializeField] private Color hotColor;
    [SerializeField] private Color coldColor;

    [SerializeField] private float max = 75;

    // Start is called before the first frame update
    void Start()
    {
        rendering = gameObject.GetComponent<Renderer>();
        rendering.material.color = Color.Lerp(hotColor, coldColor, .2f);
    }

    public void UpdateIndicator(List<Transform> items, Transform start)
    {
        float lowestDistance = 0;
        if (items.Count > 0)
        {
            bool filled = false;
            foreach (Transform item in items)
            {
                float distance = Vector3.Distance(item.position, transform.position);
                if (filled)
                {
                    if (distance < lowestDistance) lowestDistance = distance;
                }
                else
                {
                    lowestDistance = distance;
                    filled = true;
                }
            }
        }
        else
        {
            lowestDistance = Vector3.Distance(start.position, transform.position);
        }
        

        Color newColor = coldColor;
        if(lowestDistance < max)
        {
            float scale = lowestDistance / max;
            newColor = Color.Lerp(hotColor, coldColor, scale);
        }

        rendering.material.color = newColor;
    }
}
