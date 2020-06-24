using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    GameManager gmc;
    gameState activeState;
    AudioSource audio;
    [SerializeField]AudioClip menuMusic;
    [SerializeField] AudioClip gameMusic;
    // Start is called before the first frame update
    void Start()
    {
        gmc = FindObjectOfType<GameManager>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gmc.getGameState() == gameState.menu|| gmc.getGameState() == gameState.died)
        {
            audio.clip = menuMusic;
           // GetComponent<AudioSource>().Play();
        }
        if(gmc.getGameState()== gameState.playing)
        {
            audio.clip = gameMusic;
            
            //  GetComponent<AudioSource>().Play();
        }

        if (audio.isPlaying) return;
            else audio.Play();
    }
}
