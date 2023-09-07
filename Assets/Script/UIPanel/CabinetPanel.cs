using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CabinetPanel : MonoBehaviour
{
    private bool isOpen;


    //�ж�����ı��� ���Ը��ĵ�
    private List<List<int>> rightNumList = new List<List<int>> { new List<int> { 4, 8 }, new List<int> { 6, 2 }, new List<int> { 2, 1, 3} , new List<int> { 4, 1, 2} };
    private List<int> rightNum;


    
    //���������ƶ������ʱ�䣬���޵���ʱ��
    public float rotate_time, open_time, fall_time, showThree_time;
    [SerializeField]
    private RectTransform moveEnd_l, moveEnd_r, moveEnd_dolls;
    [SerializeField]
    private RectTransform door_left, door_right, falldolls;
    [SerializeField]
    private GameObject exitBtn, isUnlockTrigger;
    [SerializeField]
    private Image hintThree;
    [SerializeField]
    private AudioSource fallDollsAudio, openDoorAudio;

    //��������
    public List<GameObject> cursor;
    //��ǰ������
    private int nowActCursor;

    //��һ�µĽ�����ʽ
    [SerializeField]
    private List<Button> circle;
    //�ڶ��µĽ�����ʽ
    [SerializeField]
    private List<Lock> lockList;
    //�����µĽ�����ʽ
    private int currentOwnPropWithChap3 = 0;
    [SerializeField]
    private List<GameObject> iconList;
    //�����µĶԻ�Ҫ�л�
    [SerializeField]
    private GameObject lockDoor, unlockDoor_cantopen, unlockDoor_open;
    //�����µĽ���
    [SerializeField]
    private Button openDoorBtn, exitBtn_chap4;
    [SerializeField]
    private AudioSource lockDoorAudio;

    [SerializeField]
    private List<GameObject> firepitList = new List<GameObject>();


    // Start is called before the first frame update
    void Start()
    {
        isOpen = false;
        //��һ����ת����
        if (ChapManager.Instance.CurChap == 1)
        {
            circle[0].onClick.AddListener(OnCircle1Click);
            circle[1].onClick.AddListener(OnCircle2Click);
            circle[2].onClick.AddListener(OnCircle3Click);
            nowActCursor = 2;
            //Debug.Log(nowActCursor);
            cursor[0].SetActive(false);
            cursor[1].SetActive(false);
            cursor[2].SetActive(false);
        }
        else if (ChapManager.Instance.CurChap == 4)
            openDoorBtn.onClick.AddListener(OnOpenDoorBtnClick);
    }

    // Update is called once per frame
    void Update()
    {
        //��һ�µĽ�����ʽ
        if (ChapManager.Instance.CurChap == 1 && !isOpen && CheckIsOpen_Chap1())
        {
            //��ֹ�����Ժ������Ȼ��������
            circle[0].enabled = false;
            circle[1].enabled = false;
            circle[2].enabled = false;
            OpenDoorDelay();
            isOpen = true;
        }
        //�ڶ��½�����ʽ
        else if (ChapManager.Instance.CurChap == 2 && !isOpen && CheckIsOpen_Chap2())
        {
            foreach(Lock l in lockList)
            {
                l.BanButton();
            }
            OpenDoorDelay();
            isOpen = true;
        }

        //��һ���ֱ���������
        if (ChapManager.Instance.CurChap == 1)
        {
            //��A���л���������
            if (Input.GetKeyDown(KeyCode.JoystickButton1))
            {
                //��ǰ�й��ʱ������Ϊ�л�����
                if (cursor[nowActCursor].activeInHierarchy)
                {
                    cursor[nowActCursor].SetActive(false);
                    if (nowActCursor == 0)
                    {
                        nowActCursor = 2;
                    }
                    else
                    {
                        nowActCursor = nowActCursor - 1;
                    }
                    cursor[nowActCursor].SetActive(true);
                    //Debug.Log(nowActCursor);
                    //cursor[nowActCursor].SetActive(true);
                }
                //��ǰ�޹��ʱ���������ֵ�ǰ���
                else
                {
                    cursor[nowActCursor].SetActive(true);
                }
            }
            //��B��ת������
            else if (Input.GetKeyDown(KeyCode.JoystickButton0) && circle[nowActCursor].enabled && cursor[nowActCursor].activeInHierarchy)
            {
                circle[nowActCursor].onClick.Invoke();
            }
            //����ƶ�ʱ�ֱ������ʧ
            else if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                cursor[nowActCursor].SetActive(false);
            }
        }
    }

    public void OnEnable()
    {
        //����������������������ҽ������ֱ�ӽ���
        if (ChapManager.Instance.CurChap == 3 && unlockDoor_open.activeInHierarchy)
        {
            OpenDoor();
        }
    }

    //�����µ����ť�Ժ��ж��Ƿ���Կ���
    public void OnOpenDoorBtnClick()
    {
        //���ʹ�ù�ë���Ѽ�Ļ����Ժ�
        if (PropManager.Instance.CheckPropIsOwn(PropType.Bamboo) == false && PropManager.Instance.CheckPropIsOwn(PropType.DuckHeart) == false)
        {
            openDoorBtn.gameObject.SetActive(false);
            OpenDoor();
        }
        else
            lockDoorAudio.Play();
    }

    //�ȵ���׼���ٿ���
    public void OpenDoorDelay()
    {
        //�����һ����ת���
        Invoke("OpenDoor", rotate_time);

        if (ChapManager.Instance.CurChap == 1)
        {
            foreach (Button c in circle)
            {
                c.enabled = false;
            }
        }
    }


    //���� ��ֹ��ť����
    public void OpenDoor()
    {
        if (openDoorAudio != null)
            openDoorAudio.Play();
        DOTween.KillAll();
        DOTween.Init();
        door_left.DOMoveX(moveEnd_l.position.x, open_time);
        door_right.DOMoveX(moveEnd_r.position.x, open_time);
        Invoke("HideDoor", open_time);
        if (ChapManager.Instance.CurChap == 1)
            Invoke("FallDolls", open_time);
    }

    public void HideDoor()
    {
        door_left.gameObject.SetActive(false);
        door_right.gameObject.SetActive(false);
    }


    //�����Ժ��������
    public void FallDolls()
    {
        fallDollsAudio.Play();
        DOTween.Init();
        falldolls.DOMoveY(moveEnd_dolls.position.y, fall_time);
    }

    private float curRotate_1 = 90;
    public void OnCircle1Click()
    {
        if (Mathf.Abs(curRotate_1 - 360) < 0.01)
            curRotate_1 = 0;
        curRotate_1 += 90;
        Vector3 vec = new Vector3(0, 0, curRotate_1);
        DOTween.Init();
        circle[0].transform.DOLocalRotate(vec, rotate_time, RotateMode.Fast);

        circle[0].enabled = false;
        Invoke("ActiveCircle_1", rotate_time);
    }

    private float curRotate_2 = 120;
    public void OnCircle2Click()
    {
        if (Mathf.Abs(curRotate_2 - 360) < 0.01)
            curRotate_2 = 0;
        curRotate_2 += 120;
        Vector3 vec = new Vector3(0, 0, curRotate_2);
        DOTween.Init();
        circle[1].transform.DOLocalRotate(vec, rotate_time, RotateMode.Fast);

        circle[1].enabled = false;
        Invoke("ActiveCircle_2", rotate_time);
    }

    private float curRotate_3 = 180;
    public void OnCircle3Click()
    {
        if (Mathf.Abs(curRotate_3 - 360) < 0.01)
            curRotate_3 = 0;
        curRotate_3 += 180;
        Vector3 vec = new Vector3(0, 0, curRotate_3);
        DOTween.Init();
        circle[2].transform.DOLocalRotate(vec, rotate_time, RotateMode.Fast);
        //�Ƚ�ֹһ�·�ֹ����bug
        circle[2].enabled = false;
        Invoke("ActiveCircle_3", rotate_time);
    }

    //���ť
    public void ActiveCircle_1()
    {
        if (isOpen) return;
        circle[0].enabled = true;
    }


    public void ActiveCircle_2()
    {
        if (isOpen) return;
        circle[1].enabled = true;
    }


    public void ActiveCircle_3()
    {
        if (isOpen) return;
        circle[2].enabled = true;
    }


    private int curJudgeNum = 0;
    bool isComplete = false;

    //����û��ϸ�������
    /*public void ShowFirepitPanel(int firepitIndex)
    {
        showFirepitPanel.gameObject.SetActive(true);
        showFirepitPanel.ShowFirepitDetailPanel(firepitIndex);
    }*/

    //��ֹ�ظ�����ȫ���ڵ����߼�
    private bool prepareCloseAll = false;
    public void ChangeFirepitState(int firepitIndex, bool isFire)
    {
        rightNum = rightNumList[ChapManager.Instance.CurChap - 1];
        firepitList[firepitIndex].GetComponent<Firepit>().SwitchStatue(isFire);
        if (isFire && firepitIndex + 1 == rightNum[curJudgeNum] && !isComplete && !prepareCloseAll)
        {
            if (curJudgeNum == rightNum.Count - 1)
            {
                FireAllFirepit();
                BanAllFirepit();
                isComplete = true;
                return;
            }
            curJudgeNum++;
        }
        else if (isFire && !isComplete && !prepareCloseAll)
        {
            Invoke("CloseAllFirepit", 1f);
            prepareCloseAll = true;
        }
    }

    //��ֹ���л���̨�Ľ���
    public void BanAllFirepit()
    {
        foreach (GameObject g in firepitList)
        {
            g.GetComponent<Firepit>().BanInteractive();
        }
    }

    //��ȫ������̨�����ճɹ�
    public void FireAllFirepit()
    {
        foreach (GameObject g in firepitList)
        {
            g.GetComponent<Firepit>().SwitchStatue(true);
        }
        GameAudioManager.Instance.PlayFireAllAudio();
        //����������ʾ
        if (ChapManager.Instance.CurChap == 1)
            ShowHintThree();
        //���ŵڶ��½��cg
        else if (ChapManager.Instance.CurChap == 2)
        {
            VideoManager.Instance.PlayGetMarried();
        }
        //�ƶ�NPCλ��
        else if (ChapManager.Instance.CurChap == 3)
            PropManager.Instance.Chap3RiverChangeNPCPosition();
        //���ְ���ͶԻ�
        else if (ChapManager.Instance.CurChap == 4)
        {
            VideoManager.Instance.PlaySisterShowVideo();
            gameObject.SetActive(false);
        }
    }

    //�ص�ȫ������̨
    public void CloseAllFirepit()
    {
        curJudgeNum = 0;
        prepareCloseAll = false;
        for (int i = 0; i < firepitList.Count; i++)
        {
            ChangeFirepitState(i, false);
        }
    }



    //��һ�¿�������Ƿ������ȷ
    public bool CheckIsOpen_Chap1()
    {
        if (Mathf.Abs(curRotate_1 - 360) < 0.01 && Mathf.Abs(curRotate_2 - 360) < 0.01 && Mathf.Abs(curRotate_3 - 360) < 0.01)
        {
            return true;
        }
        return false;
    }

    //����ľͷ�� һ�޶�������
    public void ShowHintThree()
    {
        isUnlockTrigger.SetActive(true);
        DOTween.Init();
        hintThree.DOFade(1, showThree_time);
        Invoke("ShowHintComplete", showThree_time);
    }

    //��ʾ�Ѿ��������ˣ��ý�����ս���
    public void ShowHintComplete()
    {
        exitBtn.gameObject.SetActive(true);
    }

    //�ڶ��½�������
    public bool CheckIsOpen_Chap2()
    {
        if (lockList[0].CurNum == 3 && lockList[1].CurNum == 4 && lockList[2].CurNum == 1)
            return true;
        return false;
    }

    //�����½�������
    public bool CheckIsOpen_Chap3()
    {
        //�������Ƿ����ռ���Ѽ�ĺ�ë��
        if (currentOwnPropWithChap3 == 3 && PropManager.Instance.CheckPropIsOwn(PropType.Bamboo) && PropManager.Instance.CheckPropIsOwn(PropType.DuckHeart))
            return true;
        else
            return false;
    }

    //�������ռ���ë���Ѽ�ĵ�ʱ�����Ƿ��ռ���ȫ ��ʱ�ı�����״̬����ֹ�м�״̬�ĶԻ��ظ����֣�
    public void CheckBambooAndHeart()
    {
        if (PropManager.Instance.CheckPropIsOwn(PropType.Bamboo) && PropManager.Instance.CheckPropIsOwn(PropType.DuckHeart) && CheckIsOpen_Chap3())
        {
            unlockDoor_cantopen.SetActive(false);
            unlockDoor_open.SetActive(true);
        }
    }

    //�������ռ���������ʾ
    public void ShowChap3PropImg(int index)
    {
        currentOwnPropWithChap3++;
       // Debug.Log(" show  " + index + currentOwnPropWithChap3);
        //0 �׾� 1 ֽǮ 2 ���
        iconList[index].SetActive(true);
        if (currentOwnPropWithChap3 == 3)
        {
            unlockDoor_cantopen.SetActive(true);
            lockDoor.SetActive(false);
            CheckBambooAndHeart();
        }
    }
}
