using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TabletPanel : MonoBehaviour
{
    public float changeMemorial_time, move_time, stickChange_time, drawerChange_time;
    [SerializeField]
    private RectTransform moveToMidPos, moveToDrawerPos;
    [SerializeField]
    private Image tablet, stick, stick_desk, drawers_close;
    [SerializeField]
    private CanvasGroup drawers_open;
    [SerializeField]
    private GameObject memorialHall;
    //保证移动过程中不会发生按钮触发
    [SerializeField]
    private List<Button> buttonList;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //大堂转场到祠堂
    public void ChangeToMemorialHall()
    {
        DOTween.Init();
        tablet.DOFade(0, changeMemorial_time);
        memorialHall.gameObject.SetActive(true);
        memorialHall.GetComponent<CanvasGroup>().DOFade(1, changeMemorial_time);
        //停留一秒看看牌位
        Invoke("CompleteChangeAnim", changeMemorial_time);
    }

    public void CompleteChangeAnim()
    {
        tablet.gameObject.SetActive(false);
        memorialHall.transform.DOMoveY(moveToMidPos.position.y, move_time);
        Invoke("CompleteMove", move_time);
        Invoke("ResetPositionToMid", move_time);
    }

    //防止DOTWeen没有成功
    private void ResetPositionToMid()
    {
        memorialHall.transform.position = new Vector3(memorialHall.transform.position.x, moveToMidPos.position.y, memorialHall.transform.position.z);
    }

    //上香成功以后的一系列操作动画
    public void HideDeskStick()
    {
        ChangeAllButtonState(false);
        stick_desk.DOFade(0, stickChange_time);
        Invoke("ShowThuribleStick", stickChange_time);
    }
    private void ShowThuribleStick()
    {
        stick.gameObject.SetActive(true);
        stick.DOFade(1, stickChange_time);
        Invoke("MoveToDrawer", stickChange_time);
    }
    private void MoveToDrawer()
    {
        memorialHall.transform.DOMoveY(moveToDrawerPos.position.y, move_time);
        Invoke("CompleteMove", move_time);
        Invoke("ResetPositionToDrawer", move_time);
    }

    //防止DOTWeen没有成功
    private void ResetPositionToDrawer()
    {
        memorialHall.transform.position = new Vector3(memorialHall.transform.position.x, moveToDrawerPos.position.y, memorialHall.transform.position.z);
    }

    //移动镜头完毕的处理
    private void CompleteMove()
    {
        //激活按钮交互
        ChangeAllButtonState(true);
    }

    //处理按钮交互
    public void ChangeAllButtonState(bool state)
    {
        foreach(Button b in buttonList)
        {
            if (b != null)
                b.enabled = state;
        }
    }

    //处理抽屉打开
    public void OpenDrawers()
    {
        ChangeAllButtonState(false);
        drawers_open.gameObject.SetActive(true);
        drawers_close.DOFade(0, drawerChange_time);
        drawers_open.DOFade(1, drawerChange_time);
        Invoke("OpenDrawersEnd", drawerChange_time);
    }

    private void OpenDrawersEnd()
    {
        drawers_close.gameObject.SetActive(false);
        ChangeAllButtonState(true);
    }
}
