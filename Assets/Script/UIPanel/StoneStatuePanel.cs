using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class StoneStatuePanel : MonoBehaviour
{
    //����������ʼλ�úͷ���λ��
    Vector3 sendPos;

    //�ӽ��������޲��ƶ�
    private bool isMove;

    //��������ʱ��
    public float switchTime, moveTime, sendTime;

    
    [SerializeField]
    private Button sendDollsBtn;
    [SerializeField]
    private RectTransform stoneStatue_open, stoneStatue_close, dolls;
    //С���ƶ��ı߽�㡢������ӽǵ�ת��
    [SerializeField]
    private RectTransform leftPoint, rightPoint, downPos, upPos;
    //���䵽��λ�á��ж��Ƿ����ʤ����λ��
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
        //�ֱ�����Ͷι
        if (Input.GetKeyDown(KeyCode.JoystickButton0) && sendDollsBtn.enabled)
        {
            OnSendDollsBtnClick();
        }
    }


    //�����¼����ʱ��������û�������׿Ҫ����һ����ҵĳ���
    private void OnEnable()
    {
        if (ChapManager.Instance.CurChap == 3 && !PlayerManager.Instance.facingRight)
            PlayerManager.Instance.Filp();
    }

    private void OnDisable()
    {
        ResetStoneStatue();
    }

    //open���ӽ�ת��
    public void MoveOpenStatue()
    {
        //�ӽ����� ����ͼƬ�����Ƶ�
        stoneStatue_open.DOMoveY(downPos.position.y, moveTime);
        Invoke("ActiveSendDollsGame", moveTime);
        moveCamera.Play();
    }

    //ת���ӽ��Ժ��ټ�����Ϸ
    public void ActiveSendDollsGame()
    {
        //��ʼ�ƶ�
        ResetDolls();
        isMove = true;

        //��ֹû�гɹ�ת���ӽ�
        stoneStatue_open.position = new Vector3(stoneStatue_open.position.x, downPos.position.y, stoneStatue_open.position.z);

        stoneStatue_open.gameObject.GetComponent<Button>().enabled = true;
    }


    //����С���ƶ�
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

    //����С��
    public void OnSendDollsBtnClick()
    {
        //��ֹ���������޳���bug
        sendDollsBtn.enabled = false;
        
        //ִ�������޵��߼�
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
            //ʧ�����¼��ť
            sendDollsBtn.enabled = true;
            DOTween.KillAll();
            dolls_lose.Play();
            ResetDolls();
        }
    }

    //��������
    public void ResetDolls()
    {
        Vector3 resetPos = leftPoint.position;
        dolls.position = resetPos;
    }

    //���쵽����
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

    //close���ӽ�ת��
    public void MoveCloseStatue()
    {
        DOTween.KillAll();
        stoneStatue_close.DOMoveY(upPos.position.y, moveTime);
        Invoke("FadeStoneStatueAlpha", moveTime);
    }

    public void FadeStoneStatueAlpha()
    {
        //��ֹû�гɹ�ת���ӽ�
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
