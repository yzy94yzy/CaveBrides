using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAudioManager : MonoSingleton<GameAudioManager>
{
    //ȫ�ֿ�����Ч
    [SerializeField]
    private AudioSource opendoor, earingRing, short_pickup, long_pickup, fireAll;

    //��һ�µ������Ч
    [SerializeField]
    private AudioSource torchAudio, showAltar;

    //ÿ�½ڳ������ŵ���Ч
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
