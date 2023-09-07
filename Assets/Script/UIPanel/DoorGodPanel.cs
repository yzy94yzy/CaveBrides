using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DoorGodPanel : MonoBehaviour
{
    private int completeNum;
    //门神拼好显示时间
    public float change_time;
    [SerializeField]
    private GameObject windowpaper, door_open, door_close;
    [SerializeField]
    private Image doorgod_break, doorgod_all;
    [SerializeField]
    private CanvasGroup piecesGroup;

    // Start is called before the first frame update
    void Start()
    {
        completeNum = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        if (PropManager.Instance.CheckPropIsOwn(PropType.Windowpaper))
            windowpaper.SetActive(true);
    }

    public void CompleteOnePiece()
    {
        completeNum++;
        if (completeNum == 6)
            CompleteDoorGodWithBreak();
    }

    public void RemoveOnePiece()
    {
        completeNum--;
    }

    //完成门神除窗花外部分
    public void CompleteDoorGodWithBreak()
    {
        piecesGroup.DOFade(0, change_time);
        doorgod_break.gameObject.SetActive(true);
        doorgod_break.DOFade(1, change_time);
        PropManager.Instance.ActiveWindowpaperCutGet();
    }

    public void CompleteDoorGod()
    {
        PropManager.Instance.UseProp(PropType.Windowpaper);
        DOTween.Init();
        doorgod_break.DOFade(0, change_time);
        windowpaper.GetComponent<Image>().DOFade(0, change_time);
        doorgod_all.DOFade(1, change_time);
        door_open.SetActive(true);
        door_close.SetActive(false);
    }
}
