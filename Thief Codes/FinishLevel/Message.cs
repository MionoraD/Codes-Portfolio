using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Message : MonoBehaviour
{
    private Text messageText;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private Vector3 resetPosition;
    private bool move = false;
    public float speed = 0.1F;

    void Start()
    {
        RectTransform objectRectTransform = gameObject.GetComponent<RectTransform>();
        float height = objectRectTransform.rect.height;

        startPosition = transform.localPosition;
        resetPosition = startPosition + new Vector3(0, height / 4.1f, 0);
        endPosition = startPosition + new Vector3(0, height / 4, 0);

        messageText = gameObject.GetComponent<Text>();
        messageText.gameObject.SetActive(false);
        move = false;
    }

    void Update()
    {
        if (move)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, endPosition, Time.deltaTime * speed);

            if (transform.localPosition.y >= resetPosition.y)
            {
                messageText.gameObject.SetActive(false);
                move = false;
            }
        }
    }

    public void ShowMessage(string message)
    {
        transform.localPosition = startPosition;
        messageText.text = message;
        messageText.gameObject.SetActive(true);
        move = true;
    }
}
