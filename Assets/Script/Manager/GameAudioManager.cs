using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudioManager : MonoSingleton<GameAudioManager>
{
    //全局可用音效
    [SerializeField]
    private AudioSource opendoor, earingRing, short_pickup, long_pickup, fireAll;

    //第一章的相关音效
    [SerializeField]
    private AudioSource torchAudio, showAltar;

    //每章节持续播放的音效
    [SerializeField]
    private List<AudioSource> soundsList_chap1;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PlayOpenDoorAudio()
    {
        opendoor.Play();
    }
    public void PlayEaringRingAudio()
    {
        earingRing.Play();
    }

    public void PlayTorchAudio()
    {
        torchAudio.Play();
    }


    public void PlayShowAltarAudio()
    {
        showAltar.Play();
    }

    public void PlayShortPickUpAudio()
    {
        short_pickup.Play();
    }
    public void PlayLongPickUpAudio()
    {
        long_pickup.Play();
    }
    public void PlayFireAllAudio()
    {
        fireAll.Play();
    }

    public void Chap1GameSoundPlay()
    {
        foreach (AudioSource s in soundsList_chap1)
        {
            s.Play();
        }
    }

    public void Chap1GameSoundStop()
    {
        foreach (AudioSource s in soundsList_chap1)
        {
            s.Stop();
        }
    }

}
