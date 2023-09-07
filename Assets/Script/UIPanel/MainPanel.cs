using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class MainPanel : BasePanel
{
    public float change_time;
    [SerializeField]
    private GameObject continuePanel, settingPanel;
    [SerializeField]
    private Button startBtn, loadSaveBtn, settingBtn, exitBtn;
    // Start is called before the first frame update
    void Start()
    {
        startBtn.onClick.AddListener(OnStartBtnClick);
        loadSaveBtn.onClick.AddListener(OnLoadBtnClick);
        settingBtn.onClick.AddListener(OnSettingBtnClick);
        exitBtn.onClick.AddListener(OnExitBtnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        startBtn.gameObject.GetComponent<Image>().DOFade(1, change_time);
        settingBtn.gameObject.GetComponent<Image>().DOFade(1, change_time);
        loadSaveBtn.gameObject.GetComponent<Image>().DOFade(1, change_time);
#if UNITY_WEBGL
#else
        exitBtn.gameObject.GetComponent<Image>().DOFade(1, change_time);
#endif
        Invoke("ActiveBtn", change_time);
    }

    private void OnDisable()
    {
        DOTween.KillAll();
    }

    private void ActiveBtn()
    {
        startBtn.enabled = true;
        loadSaveBtn.enabled = true;
        settingBtn.enabled = true;
#if UNITY_WEBGL
#else
        exitBtn.enabled = true;
#endif
    }

    private void ActiveExitBtn()
    {

    }

    private void OnStartBtnClick()
    {
        DOTween.KillAll();
        //存档的时候会打开黑幕过渡 在开始新游戏的时候不需要 通过gamemanager等待进游戏以后关掉黑幕
        //GameManager.Instance.CloseBlackCrossDelay();
        //GameManager.Instance.LoadSceneByIndex(2);
        LoadManager.Instance.ChangeSceneDelay(1);
    }

    private void OnLoadBtnClick()
    {
        continuePanel.gameObject.SetActive(true);
    }
    private void OnSettingBtnClick()
    {
        settingPanel.gameObject.SetActive(true);
    }

    private void OnExitBtnClick()
    {
        Application.Quit();
    }
}
