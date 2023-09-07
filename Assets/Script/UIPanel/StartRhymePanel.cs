using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


//单独用来做开幕动画的UI
public class StartRhymePanel : MonoBehaviour
{
    //目前播放的诗句
    private int curPoem;


    //用于后续修改诗句间隔时间和开幕消失时间。
    [SerializeField]
    private float poemShowDelayTime, poemStayTime, endPanelTime;
    [SerializeField]
    private CanvasGroup poemGroup;
    //存储所有诗句
    [SerializeField]
    private Text[] hintPoems;

    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("HintPoemShow", 0f, poemShowDelayTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (curPoem == 4)
        {
            CancelInvoke("HintPoemShow");
            Invoke("HintPoemFade", poemStayTime);
        }
    }

    private void OnDisable()
    {
        DOTween.Clear();
    }

    public void HintPoemShow()
    {
        DOTween.Init();
        hintPoems[curPoem++].DOFade(1, poemShowDelayTime);
    }

    //诗句消失
    public void HintPoemFade()
    {
        DOTween.Clear();
        DOTween.Init();
        poemGroup.DOFade(0, poemStayTime);
    }
}
