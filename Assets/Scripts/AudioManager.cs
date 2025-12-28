using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public AudioSource eat;
    public AudioSource play;
    public AudioSource notify;
    public AudioSource sweep;
    public AudioSource click;
    public AudioSource paper;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {

    }

    public void PlayEat()
    {
        if (!eat.isPlaying)
            eat.Play();
    }

    public void PlayPlay()
    {
        if (!play.isPlaying)
            play.Play();
    }
    public void PlayNotify()
    {
        if (!notify.isPlaying)
            notify.Play();
    }
    public void PlaySweep()
    {
        if (!sweep.isPlaying)
            sweep.Play();
    }
    public void PlayClick()
    {
        if (!click.isPlaying)
            click.Play();
    }
    public void PlayPaper()
    {
        if (!paper.isPlaying)
            paper.Play();
    }


}
