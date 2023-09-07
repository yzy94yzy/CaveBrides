using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class DragPhoto : DragProps
{
    public float showAll_time, white_time, showShootPhoto_time;
    [SerializeField]
    private Transform photo, startRiverTrigger;
    [SerializeField]
    private Image photo_all, whiteCross;
    [SerializeField]
    private CanvasGroup shootImg;

    //原来弃用的鼠标反向移动代码
    //private Vector3 photoPos;
    private Vector3 photoFixPos;
    
    // Start is called before the first frame update
    void Start()
    {
        photoFixPos = photo.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public override void GetOffsetPos()
    {
        /* photoPos = transform.position;*/
        offsetPos = Input.mousePosition - transform.position;
    }

    public override void DragPropsMethod()
    {
        /* Vector3 diff = Input.mousePosition - photoPos;
         transform.position = photoPos - diff + offsetPos;*/
        transform.position = Input.mousePosition - offsetPos;
        photo.position = photoFixPos;
    }

    public override void CheckPosition()
    {
        if (Mathf.Sqrt((completePos.transform.position - transform.position).magnitude) < 5)
        {
            photo_all.gameObject.SetActive(true);
            photo_all.DOFade(1, showAll_time);
            Invoke("WhiteCross", showAll_time);
            gameObject.SetActive(false);
        }
    }

    public void WhiteCross()
    {
        whiteCross.DOFade(1, white_time);
        Invoke("WhiteFade", white_time);
    }

    public void WhiteFade()
    {
        whiteCross.DOFade(0, white_time);
        Invoke("ShowShootPhoto", white_time);
    }

    public void ShowShootPhoto()
    {
        ChapManager.Instance.Chap3ForestChangeToRiver();
        shootImg.gameObject.SetActive(true);
        shootImg.DOFade(1, showShootPhoto_time);
        Invoke("HideShootPhoto", showShootPhoto_time);
    }

    public void HideShootPhoto()
    {
        shootImg.transform.parent.GetComponent<CanvasGroup>().DOFade(0, showShootPhoto_time);
        Invoke("PhotoGamePanelFalse", showShootPhoto_time);
    }

    public void PhotoGamePanelFalse()
    {
        shootImg.transform.parent.gameObject.SetActive(false);
        startRiverTrigger.gameObject.SetActive(true);
    }
}
