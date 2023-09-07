using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DragPen : DragProps
{

    //��һ�����Ե��ж��㣬�����жϵĵ��Ǳ�ͷ
    [SerializeField]
    private Transform completePos_2, judgePos;
    //�����۵��ж�
    [SerializeField]
    private GameObject left_eye, right_eye;
    [SerializeField]
    private PaperPeoplePanel paperPeoplePanel;
    [SerializeField]
    private AudioSource addEyeAudio;
    public override void CheckPosition()
    {
        if (completePos != null && Mathf.Sqrt((completePos.position - judgePos.position).magnitude) < 7)
        {
            if (!left_eye.activeInHierarchy)
            {
                addEyeAudio.Play();
                left_eye.SetActive(true);
            }
            if (left_eye.activeInHierarchy && right_eye.activeInHierarchy)
            {
                paperPeoplePanel.DollsComplete();
                PenMove();
            }
            else
                transform.position = startPos.position;
        }
        else if (completePos_2 != null && Mathf.Sqrt((completePos_2.position - judgePos.position).magnitude) < 7)
        {
            if (!right_eye.activeInHierarchy)
            {
                addEyeAudio.Play();
                right_eye.SetActive(true);
            }
            if (left_eye.activeInHierarchy && right_eye.activeInHierarchy)
            {
                paperPeoplePanel.DollsComplete();
                PenMove();
            }
            else
                transform.position = startPos.position;
        }
        else
        {
            transform.position = startPos.position;
        }
    }

    private void PenMove()
    {
        transform.DOMove(startPos.position, paperPeoplePanel.doll_time);
    }


}
