using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioSource seSource;
    public AudioSource bgmSource;
    public AudioClip[] se;
    public AudioClip[] bgm;

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
            // If the BGM exists, then you set!
            if (bgmSource)
                bgmSource.loop = value;
        }
        get
        {
            if (bgmSource)
                return bgmSource.loop;
            return false;
        }
    }
    public float bgmVolume
    {
        set
        {
            if (bgmSource)
                bgmSource.volume = value;
        }
        get
        {
            if (bgmSource)
                return bgmSource.volume;
            return 0;
        }
    }
    public float seVolume
    {
        set
        {
            if (seSource)
                seSource.volume = value;
        }
        get
        {
            if (seSource)
                return seSource.volume;
            return 0;
        }
    }

}
