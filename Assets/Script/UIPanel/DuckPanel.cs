using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DuckPanel : MonoBehaviour
{
    public float change_time, move_time;


    [SerializeField]
    private CanvasGroup duck_all, duck_hurt;
    [SerializeField]
    private GameObject knife, knifeHint, exitBtn, duckHintTrigger;
    [SerializeField]
    private Image heart_in, heart_out;
    [SerializeField]
    private RectTransform knifeStartPos, knifeEndPos, heartEndPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        //ӵ��С�����ǻ�ûȡѼ�ĵ�ʱ�򲻿��˳�
        if (PropManager.Instance.CheckPropIsOwn(PropType.Knife) && !PropManager.Instance.CheckPropIsOwn(PropType.DuckHeart))
        {
            knife.SetActive(true);
            knifeHint.SetActive(true);
            exitBtn.SetActive(false);
        }
        //û��С����ʱ����ʾһ��
        else if (!PropManager.Instance.CheckPropIsOwn(PropType.Knife))
        {
            duckHintTrigger.SetActive(true);
            exitBtn.SetActive(true);
        }
        else
            exitBtn.SetActive(true);
    }
    private void OnDisable()
    {
        if (!PropManager.Instance.CheckPropIsOwn(PropType.Knife))
        {
            duckHintTrigger.SetActive(false);
        }
    }

    //��Ѽ�Ӷ���
    public void MoveKnife()
    {
        knife.GetComponent<RectTransform>().position = knifeStartPos.position;
        knife.GetComponent<RectTransform>().DOMove(knifeEndPos.position, move_time);
        Invoke("ChangeDuckState", move_time);
    }

    public void ChangeDuckState()
    {
        duck_hurt.gameObject.SetActive(true);
        duck_all.DOFade(0, change_time);
        duck_hurt.DOFade(1, change_time);
        Invoke("ChangeStateEnd", change_time);
    }

    private void ChangeStateEnd()
    {
        heart_in.GetComponent<Button>().enabled = true;
        duck_all.gameObject.SetActive(false);
    }


    //�ƶ�����
    public void ChangeHeart()
    {
        heart_out.gameObject.SetActive(true);
        heart_in.DOFade(0, change_time / 4);
        heart_out.DOFade(1, change_time / 2);
        Invoke("MoveHeartPosition", change_time / 2);
    }

    private void MoveHeartPosition()
    {
        heart_in.gameObject.SetActive(false);
        heart_out.rectTransform.DOMove(heartEndPos.position, move_time * 1.5f);
        Invoke("MoveHeartEnd", move_time * 1.5f);
    }

    private void MoveHeartEnd()
    {
        heart_out.GetComponent<Button>().enabled = true;
    }
}
