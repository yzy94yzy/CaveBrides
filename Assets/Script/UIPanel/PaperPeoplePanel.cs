using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PaperPeoplePanel : MonoBehaviour
{
    //娃娃消失的时间
    public float doll_time;
    [SerializeField]
    private DragPen dragPen;
    [SerializeField]
    private CanvasGroup eyedolls;
    [SerializeField]
    private GameObject exitBtn, pen;

    public void ShowDolls()
    {
        eyedolls.gameObject.SetActive(true);
        eyedolls.DOFade(1, doll_time);
        Invoke("ActiveDragPen", doll_time);
    }

    private void ActiveDragPen()
    {
        dragPen.enabled = true;
    }


    public void DollsComplete()
    {
        exitBtn.GetComponent<Button>().enabled = false;
        DOTween.Init();
        eyedolls.DOFade(0, doll_time);
        Invoke("GetDollsOver", doll_time);
    }

    private void GetDollsOver()
    {
        eyedolls.gameObject.SetActive(false);
        PropManager.Instance.UseDolls();
        PropManager.Instance.AddNewProp(PropType.EyeDoll);
        DOTween.Kill(eyedolls);
        //destory eyedoll展示获取提示
        PropManager.Instance.ShowNewPropHint(5);
        //解锁石像互动
        PropManager.Instance.Chap4StoneStatueInsideUnlock();
        exitBtn.GetComponent<Button>().enabled = true;
    }
}
