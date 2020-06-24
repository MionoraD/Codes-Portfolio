using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script moves the camera around in the menu

public class ControlCamera : MonoBehaviour
{
    [SerializeField] private GameObject camera;

    [SerializeField] private Transform field;
    private List<FieldBlock> blocksInField = new List<FieldBlock>();

    private Vector3 startPosition;
    private float _smallx = 0, _bigx = 0, _z = 0;

    // Movement speed in units/sec.
    public float speed = 0.5F;
    private float fraction = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        // Set the current position as the start position
        startPosition = camera.transform.position;

        // Find all possible camera positions
        foreach (Transform child in field)
        {
            FieldBlock block = child.GetComponent<FieldBlock>();
            if(block != null) blocksInField.Add(block);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Find where the actors are and calculate how far the camera must be to see them all
        int i = 0;
        foreach (FieldBlock block in blocksInField)
        {
            if(block.crowd > 0)
            {
                float x = block.transform.position.x;
                float z = block.transform.position.z;

                if (i == 0)
                {
                    _smallx = x;
                    _bigx = x;
                    _z = z;
                }
                else
                {
                    if (x < _smallx) _smallx = x;
                    else if (x > _bigx) _bigx = x;

                    if (z < _z) _z = z;
                }

                i++;
            }
        }

        _z += 20;
        _z = Mathf.Clamp(_z, 0, 50);

        // The new position
        Vector3 newPosition = new Vector3(0, 0, _z);

        float currentSpeed = speed;
        if (transform.position.z > _z) currentSpeed = speed * 2;

        // Set the new position
        transform.position = Vector3.Lerp(transform.position, newPosition, Time.deltaTime * currentSpeed);
    }
}
