using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    public static FadeInOut screen;

    [SerializeField] private GameObject fadeScreen;
    [SerializeField] private Image background;

    private bool fading = false;
    private bool fadeIn = false;

    private float timer = 0;
    [SerializeField] private float time = 2f;
    public float SetTime
    {
        get { return time; }
        private set { time = value; }
    }

    private Color fadeOutClr;
    private Color fadeInClr;

    private Color startColor, endColor;

    void Awake()
    {
        screen = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        fadeOutClr = background.color;

        fadeInClr = fadeOutClr;
        fadeInClr.a = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (fading)
        {
            if (timer < time)
            {
                background.color = Color.Lerp(startColor, endColor, timer);
                timer += Time.deltaTime / time;
            }
            else
            {
                fading = false;
            }

            if(timer > (time / 2))
            {
                if (fadeIn) fadeScreen.SetActive(false);
            }
        }
    }

    public void FadeIn()
    {
        startColor = background.color;
        endColor = fadeInClr;
        fadeIn = true;
        fading = true;

        timer = 0;
    }

    public void FadeOut()
    {
        fadeScreen.SetActive(true);

        startColor = background.color;
        endColor = fadeOutClr;
        fadeIn = false;
        fading = true;

        timer = 0;
    }
}
