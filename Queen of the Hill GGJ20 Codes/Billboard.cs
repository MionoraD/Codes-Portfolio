using UnityEngine;


public class Billboard : MonoBehaviour
{

    [SerializeField] public bool flip;

    void Update()
    {
        if (flip)
        {
            transform.LookAt(transform.position - Camera.main.transform.rotation * Vector3.forward,
            Camera.main.transform.rotation * Vector3.up);

        }
        else
        {
            transform.LookAt(transform.position + Camera.main.transform.rotation * Vector3.forward,
            Camera.main.transform.rotation * Vector3.up);
        }
    }
}

