
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAudioManager : MonoSingleton<StartAudioManager>
{
    public float AllVolumeValue
    {
        get;
        set;
    }
    public float EffectValue
    {
        get;
        set;
    }
        
    public float MusicValue
    {
        get;
        set;
    }

    [SerializeField]
    private AudioSource startBGM;


    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        startBGM.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //”√¿¥øÿ÷∆±≥æ∞“Ù¿÷µƒ«–ªª°£
    public void StartBGMPlay()
    {
        startBGM.Play();
    }

    public void StartBGMStop()
    {
        startBGM.Stop();
    }

    public void SetVolumeValue(float allVolumeValue, float effectValue, float musicValue)
    {
        AllVolumeValue = allVolumeValue;
        EffectValue = effectValue;
        MusicValue = musicValue;
    }

}
