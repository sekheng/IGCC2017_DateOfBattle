using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioSource seSource;
    public AudioSource bgmSource;
    public AudioClip[] se;
    public AudioClip[] bgm;
    // Use this for initialization
    void Start () {

    }
    public bool PlayMusic(int clipNum)
    {
        if(clipNum > bgm.Length)
        {
            return false;
        }
        bgmSource.clip = bgm[clipNum];
        bgmSource.Play();
        return true;
    }

    public bool PlaySE(int clipNum)
    {
        if (clipNum > se.Length)
        {
            return false;
        }
        seSource.PlayOneShot(se[clipNum]);
        return true;
    }
    public bool bgmLoop
    {
        set
        {
            bgmSource.loop = value;
        }
        get
        {
            return bgmSource.loop;
        }
    }
    public float bgmVolume
    {
        set
        {
            bgmSource.volume = value;
        }
        get
        {
            return bgmSource.volume;
        }
    }
    public float seVolume
    {
        set
        {
            seSource.volume = value;
        }
        get
        {
            return seSource.volume;
        }
    }

}
