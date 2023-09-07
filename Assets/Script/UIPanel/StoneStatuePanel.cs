using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class StoneStatuePanel : MonoBehaviour
{
    //保存娃娃起始位置和发送位置
    Vector3 sendPos;

    //视角下移娃娃才移动
    private bool isMove;

    //用来配置时间
    public float switchTime, moveTime, sendTime;

    
    [SerializeField]
    private Button sendDollsBtn;
    [SerializeField]
    private RectTransform stoneStatue_open, stoneStatue_close, dolls;
    //小人移动的边界点、摄像机视角的转换
    [SerializeField]
    private RectTransform leftPoint, rightPoint, downPos, upPos;
    //发射到的位置、判断是否可以胜利的位置
    [SerializeField]
    private RectTransform sendPosition, winLeftPos, winRightPos;
    [SerializeField]
    private AudioSource dropDolls, dolls_win, dolls_lose, stoneLaugh, moveCamera;



    // Start is called before the first frame update
    void Start()
    {
        isMove = false;

        sendDollsBtn.onClick.AddListener(OnSendDollsBtnClick);
    }

    // Update is called once per frame
    void Update()
    {
        if (isMove)
            MoveDollsAnim();
        //手柄控制投喂
        if (Input.GetKeyDown(KeyCode.JoystickButton0) && sendDollsBtn.enabled)
        {
            OnSendDollsBtnClick();
        }
    }


    //第三章激活的时候如果玩家没有面对秧卓要调整一下玩家的朝向
    private void OnEnable()
    {
        if (ChapManager.Instance.CurChap == 3 && !PlayerManager.Instance.facingRight)
            PlayerManager.Instance.Filp();
    }

    private void OnDisable()
    {
        ResetStoneStatue();
    }

    //open的视角转换
    public void MoveOpenStatue()
    {
        //视角往下 但是图片是上移的
        stoneStatue_open.DOMoveY(downPos.position.y, moveTime);
        Invoke("ActiveSendDollsGame", moveTime);
        moveCamera.Play();
    }

    //转完视角以后再激活游戏
    public void ActiveSendDollsGame()
    {
        //开始移动
        ResetDolls();
        isMove = true;

        //防止没有成功转到视角
        stoneStatue_open.position = new Vector3(stoneStatue_open.position.x, downPos.position.y, stoneStatue_open.position.z);

        stoneStatue_open.gameObject.GetComponent<Button>().enabled = true;
    }


    //控制小人移动
    public void MoveDollsAnim()
    {
        if (Mathf.Abs(leftPoint.position.x - dolls.position.x) < 0.1)
        {
            dolls.DOMoveX(rightPoint.position.x, moveTime);
        }
        else if (Mathf.Abs(rightPoint.position.x - dolls.position.x) < 0.1)
        {
            dolls.DOMoveX(leftPoint.position.x, moveTime);
        }
    }

    //发射小人
    public void OnSendDollsBtnClick()
    {
        //防止反复丢娃娃出现bug
        sendDollsBtn.enabled = false;
        
        //执行扔娃娃的逻辑
        dropDolls.Play();
        sendPos = dolls.position;
        DOTween.KillAll();
        dolls.DOMoveY(sendPosition.position.y, sendTime);
        Invoke("JugdeWinOrLose", sendTime);
    }

    public void JugdeWinOrLose()
    {
        if (sendPos.x > winLeftPos.position.x && sendPos.x < winRightPos.position.x)
        {
            DOTween.KillAll();
            dolls_win.Play();
            stoneLaugh.PlayDelayed(0.5f);
            OpenToClose();
        }
        else
        { 
            //失败重新激活按钮
            sendDollsBtn.enabled = true;
            DOTween.KillAll();
            dolls_lose.Play();
            ResetDolls();
        }
    }

    //重置娃娃
    public void ResetDolls()
    {
        Vector3 resetPos = leftPoint.position;
        dolls.position = resetPos;
    }

    //张嘴到闭嘴
    public void OpenToClose()
    {
        stoneStatue_open.gameObject.GetComponent<Button>().enabled = false;
        PropManager.Instance.UseDolls();
        DOTween.KillAll();
        CanvasGroup openCanvas = stoneStatue_open.gameObject.GetComponent<CanvasGroup>();
        CanvasGroup closeCanvas = stoneStatue_close.gameObject.GetComponent<CanvasGroup>();
        openCanvas.DOFade(0, switchTime);
        closeCanvas.DOFade(1, switchTime);
        isMove = false;
        Invoke("MoveCloseStatue", switchTime);
    }

    //close的视角转换
    public void MoveCloseStatue()
    {
        DOTween.KillAll();
        stoneStatue_close.DOMoveY(upPos.position.y, moveTime);
        Invoke("FadeStoneStatueAlpha", moveTime);
    }

    public void FadeStoneStatueAlpha()
    {
        //防止没有成功转到视角
        stoneStatue_close.position = new Vector3(stoneStatue_close.position.x, upPos.position.y, stoneStatue_close.position.z);
        CanvasGroup close = gameObject.GetComponent<CanvasGroup>();
        close.DOFade(0, switchTime);
        Invoke("SetStoneStatueFalse", switchTime);
    }


    public void SetStoneStatueFalse()
    {
        gameObject.SetActive(false);
    }
    
    public void ResetStoneStatue()
    {
        isMove = false;
        transform.GetComponent<CanvasGroup>().DOFade(1, 0);
        stoneStatue_open.GetComponent<CanvasGroup>().DOFade(1, 0);
        stoneStatue_close.GetComponent<CanvasGroup>().DOFade(0, 1);
        ResetDolls();
        stoneStatue_open.DOMoveY(upPos.position.y, 0);
        stoneStatue_close.DOMoveY(downPos.position.y, 0);
    }
}
