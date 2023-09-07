using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


//������������Ļ������UI
public class StartRhymePanel : MonoBehaviour
{
    //Ŀǰ���ŵ�ʫ��
    private int curPoem;


    //���ں����޸�ʫ����ʱ��Ϳ�Ļ��ʧʱ�䡣
    [SerializeField]
    private float poemShowDelayTime, poemStayTime, endPanelTime;
    [SerializeField]
    private CanvasGroup poemGroup;
    //�洢����ʫ��
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

    //ʫ����ʧ
    public void HintPoemFade()
    {
        DOTween.Clear();
        DOTween.Init();
        poemGroup.DOFade(0, poemStayTime);
    }
}
