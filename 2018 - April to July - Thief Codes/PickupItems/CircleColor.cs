using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleColor : MonoBehaviour
{
    [SerializeField] private Sprite redCircle;
    [SerializeField] private Sprite blueCircle;

    private SpriteRenderer rendering;

    // Start is called before the first frame update
    void Start()
    {
        rendering = gameObject.GetComponent<SpriteRenderer>();
    }

    public void ChangeCircle(bool chesspiece)
    {
        if (chesspiece)
            rendering.sprite = redCircle;
        else
            rendering.sprite = blueCircle;
    }
}
