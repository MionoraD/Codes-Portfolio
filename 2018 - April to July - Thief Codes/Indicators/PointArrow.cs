using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointArrow : MonoBehaviour
{
    [SerializeField] private Transform arrow;
    private Transform target;

    // Update is called once per frame
    void Update()
    {
        if (target == null) return;

        Vector3 targetDir = target.position - transform.position;

        // The step size is equal to speed times frame time.
        float step = 1 * Time.deltaTime;

        Vector3 newDir = Vector3.RotateTowards(arrow.forward, targetDir, step, 0.0f);

        // Move our position a step closer to the target.
        Quaternion rotation = Quaternion.LookRotation(newDir);
        rotation.x = 0;
        rotation.z = 0;
        arrow.localRotation = rotation;
    }

    public void UpdateIndicator(List<Transform> items, Transform start)
    {
        if(items.Count <= 0 || EndLevel.end.CheckFinished())
        {
            target = start;
            return;
        }
        float lowestDistance = 0;
        bool filled = false;
        foreach (Transform item in items)
        {
            float distance = Vector3.Distance(item.position, transform.position);
            if (filled)
            {
                if (distance < lowestDistance)
                {
                    lowestDistance = distance;
                    target = item;
                }
            }
            else
            {
                lowestDistance = distance;
                target = item;
                filled = true;
            }
        }
    }
}
