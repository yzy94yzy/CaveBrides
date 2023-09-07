using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class CalendarPanel_Inside : MonoBehaviour
{
    private int curShowPage;
    public float change_time;
    [SerializeField]
    private List<Image> pageList;
    [SerializeField]
    private Button leftBtn, rightBtn;
    [SerializeField]
    private AudioSource changePageAudio;
    // Start is called before the first frame update
    void Start()
    {
        curShowPage = 0;
        leftBtn.onClick.AddListener(OnLeftBtnClick);
        rightBtn.onClick.AddListener(OnRightBtnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnLeftBtnClick()
    {
        if (curShowPage == 0)
            return;
        changePageAudio.Play();
        pageList[curShowPage--].DOFade(0, change_time);
        pageList[curShowPage].DOFade(1, change_time); 
    }

    public void OnRightBtnClick()
    {
        if (curShowPage == 2)
            return;
        changePageAudio.Play();
        pageList[curShowPage++].DOFade(0, change_time);
        pageList[curShowPage].DOFade(1, change_time);
    }

    public void BanButtonClick()
    {
        leftBtn.enabled = false;
        rightBtn.enabled = false;
    }

    public void ActiveButtonClick()
    {
        leftBtn.enabled = true;
        rightBtn.enabled = true;
    }
}
