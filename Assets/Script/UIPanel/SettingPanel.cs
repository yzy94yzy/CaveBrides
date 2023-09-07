using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;
using UnityEngine.Audio;
using DG.Tweening;

public class SettingPanel : BasePanel
{
    //��¼��ҵ�ǰ�Ƿ��Ǿ�ֹ��
    private bool isStop = false;
    [SerializeField]
    private Button chineseBtn, englishBtn, saveBtn, backBtn;
    [SerializeField]
    private Slider allVolumeSlider, effectSlider, musicSlider;
    [SerializeField]
    private AudioMixer audioMixer;

    // Start is called before the first frame update
    void Start()
    {
        chineseBtn.onClick.AddListener(ChangeCN);
        englishBtn.onClick.AddListener(ChangeEN);
        saveBtn.onClick.AddListener(OnSaveBtnClick);
        if (backBtn != null)
            backBtn.onClick.AddListener(OnBackBtnClick);
    }

    // Update is called once per frame
    void Update()
    {
        ChangeAudioMixer();
    }

    private void OnEnable()
    {
        allVolumeSlider.value = StartAudioManager.Instance.AllVolumeValue;
        effectSlider.value = StartAudioManager.Instance.EffectValue;
        musicSlider.value = StartAudioManager.Instance.MusicValue;
        //��������ƶ��͵�����
        GameObject player = GameObject.Find("Player");
        GameObject bird = GameObject.Find("Bird");
        if (player != null)
        {
            if (PlayerManager.Instance.enabled)
                isStop = false;
            else
                isStop = true;
            player.GetComponent<PlayerManager>().StopPlayerAnim();
            player.GetComponent<PlayerManager>().enabled = false;
            if (player.GetComponent<PropBarControl>() != null)
                player.GetComponent<PropBarControl>().enabled = false;
        }
        //��Ŀ��� ������ȥ��if���� ��Ϊ���˵���û������Ҳû����
        else if (bird != null)
        {
            if (BirdManager.Instance.enabled)
                isStop = false;
            else
                isStop = true;
            BirdManager.Instance.StopBirdAnim();
            BirdManager.Instance.enabled = false;
        }

        //���ݵ�ǰ�����л�
        if (LanguageManager.Instance.IsChinese)
        {
            chineseBtn.transform.GetChild(0).gameObject.SetActive(true);
            englishBtn.transform.GetChild(0).gameObject.SetActive(false);
        }
        else
        {
            chineseBtn.transform.GetChild(0).gameObject.SetActive(false);
            englishBtn.transform.GetChild(0).gameObject.SetActive(true);
        }
    }

    private void OnDisable()
    {
        GameObject player = GameObject.Find("Player");
        GameObject bird = GameObject.Find("Bird");
        if (player != null)
        {
            if (!isStop)
            {
                player.GetComponent<PlayerManager>().enabled = true;
                if (player.GetComponent<PropBarControl>() != null)
                    player.GetComponent<PropBarControl>().enabled = true;
            }
        }
        else if (bird != null)
        {
            if (!isStop)
                BirdManager.Instance.enabled = true;
        }
    }
    //�л�������
    public void ChangeCN()
    {
        //�л�ѡ��ͼƬ
        chineseBtn.transform.GetChild(0).gameObject.SetActive(true);
        englishBtn.transform.GetChild(0).gameObject.SetActive(false);
        LanguageManager.Instance.ChangeLanguage(true);
        DialogueManager.SetLanguage("default");
    }

    // �л���Ӣ��
    public void ChangeEN()
    {
        chineseBtn.transform.GetChild(0).gameObject.SetActive(false);
        englishBtn.transform.GetChild(0).gameObject.SetActive(true);
        LanguageManager.Instance.ChangeLanguage(false);
        DialogueManager.SetLanguage("EN");
    }

    private void OnSaveBtnClick()
    {
        LoadManager.Instance.SaveGame();
    }

    private void OnBackBtnClick()
    {
        LoadManager.Instance.SaveGame();
        DOTween.KillAll();
        GameManager.Instance.LoadSceneByIndex(1);
    }

    private void ChangeAudioMixer()
    {
        StartAudioManager.Instance.SetVolumeValue(allVolumeSlider.value, effectSlider.value, musicSlider.value);
        audioMixer.SetFloat("ExposeOfMaster", allVolumeSlider.value);
        audioMixer.SetFloat("ExposeOfSound", effectSlider.value);
        audioMixer.SetFloat("ExposeOfMusic", musicSlider.value);
    }

    public override void OnEnter()
    {
        base.OnEnter();
        Enter();
    }

    public override void OnExit()
    {
        base.OnExit();
        Exit();
    }

    public override void OnPause()
    {
        base.OnPause();
        Exit();
    }

    public override void OnRecovery()
    {
        base.OnRecovery();
        Enter();
    }

    private void Enter()
    {
        gameObject.SetActive(true);
    }

    private void Exit()
    {
        gameObject.SetActive(false);
    }

}
