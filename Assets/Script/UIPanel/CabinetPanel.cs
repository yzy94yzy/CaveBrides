using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class CabinetPanel : MonoBehaviour
{
    private bool isOpen;


    //判断谜题的变量 可以更改的
    private List<List<int>> rightNumList = new List<List<int>> { new List<int> { 4, 8 }, new List<int> { 6, 2 }, new List<int> { 2, 1, 3} , new List<int> { 4, 1, 2} };
    private List<int> rightNum;


    
    //可配置门移动距离和时间，娃娃掉落时间
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

    //新增代码
    public List<GameObject> cursor;
    //当前激活光标
    private int nowActCursor;

    //第一章的解锁方式
    [SerializeField]
    private List<Button> circle;
    //第二章的解锁方式
    [SerializeField]
    private List<Lock> lockList;
    //第三章的解锁方式
    private int currentOwnPropWithChap3 = 0;
    [SerializeField]
    private List<GameObject> iconList;
    //第三章的对话要切换
    [SerializeField]
    private GameObject lockDoor, unlockDoor_cantopen, unlockDoor_open;
    //第四章的解锁
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
        //第一章旋转解锁
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
        //第一章的解锁方式
        if (ChapManager.Instance.CurChap == 1 && !isOpen && CheckIsOpen_Chap1())
        {
            //防止对了以后玩家仍然继续操作
            circle[0].enabled = false;
            circle[1].enabled = false;
            circle[2].enabled = false;
            OpenDoorDelay();
            isOpen = true;
        }
        //第二章解锁方式
        else if (ChapManager.Instance.CurChap == 2 && !isOpen && CheckIsOpen_Chap2())
        {
            foreach(Lock l in lockList)
            {
                l.BanButton();
            }
            OpenDoorDelay();
            isOpen = true;
        }

        //第一章手柄操作轮盘
        if (ChapManager.Instance.CurChap == 1)
        {
            //按A键切换操作轮盘
            if (Input.GetKeyDown(KeyCode.JoystickButton1))
            {
                //当前有光标时，操作为切换轮盘
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
                //当前无光标时，操作显现当前光标
                else
                {
                    cursor[nowActCursor].SetActive(true);
                }
            }
            //按B键转动轮盘
            else if (Input.GetKeyDown(KeyCode.JoystickButton0) && circle[nowActCursor].enabled && cursor[nowActCursor].activeInHierarchy)
            {
                circle[nowActCursor].onClick.Invoke();
            }
            //鼠标移动时手柄光标消失
            else if (Input.GetAxis("Mouse X") != 0 || Input.GetAxis("Mouse Y") != 0)
            {
                cursor[nowActCursor].SetActive(false);
            }
        }
    }

    public void OnEnable()
    {
        //第三章满足所有条件后玩家进入面板直接解锁
        if (ChapManager.Instance.CurChap == 3 && unlockDoor_open.activeInHierarchy)
        {
            OpenDoor();
        }
    }

    //第四章点击按钮以后判断是否可以开门
    public void OnOpenDoorBtnClick()
    {
        //玩家使用过毛竹和鸭心唤魂以后
        if (PropManager.Instance.CheckPropIsOwn(PropType.Bamboo) == false && PropManager.Instance.CheckPropIsOwn(PropType.DuckHeart) == false)
        {
            openDoorBtn.gameObject.SetActive(false);
            OpenDoor();
        }
        else
            lockDoorAudio.Play();
    }

    //等到对准了再开门
    public void OpenDoorDelay()
    {
        //等最后一次旋转完成
        Invoke("OpenDoor", rotate_time);

        if (ChapManager.Instance.CurChap == 1)
        {
            foreach (Button c in circle)
            {
                c.enabled = false;
            }
        }
    }


    //开门 禁止按钮交互
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


    //开门以后掉落娃娃
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
        //先禁止一下防止出现bug
        circle[2].enabled = false;
        Invoke("ActiveCircle_3", rotate_time);
    }

    //激活按钮
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

    //现在没有细节面板了
    /*public void ShowFirepitPanel(int firepitIndex)
    {
        showFirepitPanel.gameObject.SetActive(true);
        showFirepitPanel.ShowFirepitDetailPanel(firepitIndex);
    }*/

    //防止重复触发全部黑掉的逻辑
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

    //禁止所有火焰台的交互
    public void BanAllFirepit()
    {
        foreach (GameObject g in firepitList)
        {
            g.GetComponent<Firepit>().BanInteractive();
        }
    }

    //打开全部火焰台，解谜成功
    public void FireAllFirepit()
    {
        foreach (GameObject g in firepitList)
        {
            g.GetComponent<Firepit>().SwitchStatue(true);
        }
        GameAudioManager.Instance.PlayFireAllAudio();
        //出现三哭提示
        if (ChapManager.Instance.CurChap == 1)
            ShowHintThree();
        //播放第二章结局cg
        else if (ChapManager.Instance.CurChap == 2)
        {
            VideoManager.Instance.PlayGetMarried();
        }
        //移动NPC位置
        else if (ChapManager.Instance.CurChap == 3)
            PropManager.Instance.Chap3RiverChangeNPCPosition();
        //出现阿姐和对话
        else if (ChapManager.Instance.CurChap == 4)
        {
            VideoManager.Instance.PlaySisterShowVideo();
            gameObject.SetActive(false);
        }
    }

    //关掉全部火焰台
    public void CloseAllFirepit()
    {
        curJudgeNum = 0;
        prepareCloseAll = false;
        for (int i = 0; i < firepitList.Count; i++)
        {
            ChangeFirepitState(i, false);
        }
    }



    //第一章开锁检查是否解谜正确
    public bool CheckIsOpen_Chap1()
    {
        if (Mathf.Abs(curRotate_1 - 360) < 0.01 && Mathf.Abs(curRotate_2 - 360) < 0.01 && Mathf.Abs(curRotate_3 - 360) < 0.01)
        {
            return true;
        }
        return false;
    }

    //显现木头字 一哭二哭三哭
    public void ShowHintThree()
    {
        isUnlockTrigger.SetActive(true);
        DOTween.Init();
        hintThree.DOFade(1, showThree_time);
        Invoke("ShowHintComplete", showThree_time);
    }

    //提示已经出现完了，该界面解谜结束
    public void ShowHintComplete()
    {
        exitBtn.gameObject.SetActive(true);
    }

    //第二章解锁谜题
    public bool CheckIsOpen_Chap2()
    {
        if (lockList[0].CurNum == 3 && lockList[1].CurNum == 4 && lockList[2].CurNum == 1)
            return true;
        return false;
    }

    //第三章解锁谜题
    public bool CheckIsOpen_Chap3()
    {
        //检查玩家是否已收集到鸭心和毛竹
        if (currentOwnPropWithChap3 == 3 && PropManager.Instance.CheckPropIsOwn(PropType.Bamboo) && PropManager.Instance.CheckPropIsOwn(PropType.DuckHeart))
            return true;
        else
            return false;
    }

    //第三章收集到毛竹和鸭心的时候检测是否收集完全 及时改变神龛状态（防止中间状态的对话重复出现）
    public void CheckBambooAndHeart()
    {
        if (PropManager.Instance.CheckPropIsOwn(PropType.Bamboo) && PropManager.Instance.CheckPropIsOwn(PropType.DuckHeart) && CheckIsOpen_Chap3())
        {
            unlockDoor_cantopen.SetActive(false);
            unlockDoor_open.SetActive(true);
        }
    }

    //第三章收集到道具显示
    public void ShowChap3PropImg(int index)
    {
        currentOwnPropWithChap3++;
       // Debug.Log(" show  " + index + currentOwnPropWithChap3);
        //0 白酒 1 纸钱 2 香火
        iconList[index].SetActive(true);
        if (currentOwnPropWithChap3 == 3)
        {
            unlockDoor_cantopen.SetActive(true);
            lockDoor.SetActive(false);
            CheckBambooAndHeart();
        }
    }
}
