using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class JigsawPanel : MonoBehaviour
{
    //棋盘下标布局 x = num / 3 ; y = num % 3; num = 3x + y
    // 0 1 2 
    // 3 4 5
    // 6 7 8

    /* 棋盘开始状态    结束状态
     * 4     3         0     2
     * 7  6  2         3  4  5
     * 0  8  5         6  7  8
     */

    //记录目前空缺的位置下标和正确的空缺下标 
    private int indexOfNull, correctIndexOfNull;

    //拼图移动时间和转场动画时间
    public float move_time, pictureShow_time;

    //记录所有的拼图
    [SerializeField]
    private List<Jigsaw> jigsawList;
    //拼好拼图转场
    [SerializeField]
    private CanvasGroup tabletPanel, sisterWallPanel;
    //解锁成功音效
    [SerializeField]
    private AudioSource completeAudio;
    //重置拼图
    [SerializeField]
    private Button resetBtn;
    //记录拼图要移动的距离
    [SerializeField]
    private RectTransform leftPos, rightPos;

    // Start is called before the first frame update
    void Start()
    {
        SetOriginPosCurIndex();
        resetBtn.onClick.AddListener(ResetJigsawPos);
    }

    // Update is called once per frame
    void Update()
    {

    }

    //赋予拼图初始位置
    public void SetOriginPosCurIndex()
    {
        jigsawList[0].CurPosIndex = 0;
        //因为第二格是空着的
        for (int i = 1; i < jigsawList.Count; i++)
            jigsawList[i].CurPosIndex = i + 1;
        indexOfNull = 1;
        correctIndexOfNull = 1;
        SetJigsawCanMoveOrNot();
    }

    public void ResetJigsawPos()
    {
        foreach (Jigsaw j in jigsawList)
            j.ResetPos();
        SetOriginPosCurIndex();
    }


    //确定拼图是否可以移动
    public void SetJigsawCanMoveOrNot()
    {
        BanAllJigsawMove();
        resetBtn.enabled = true;
        int curNull_x = indexOfNull / 3;
        int curNull_y = indexOfNull % 3;

        //判断上方拼图
        if (curNull_x - 1 >= 0)
        {
            Jigsaw upJigsaw = FindJigsawInIndex(3 * (curNull_x - 1) + curNull_y);
            if (upJigsaw != null)
            {
                upJigsaw.MoveDirection = 2;
                upJigsaw.CanMove = true;
            }
        }
        //判断下方拼图
        if (curNull_x + 1 <= 2)
        {
            Jigsaw downJigsaw = FindJigsawInIndex(3 * (curNull_x + 1) + curNull_y);
            if (downJigsaw != null)
            {
                downJigsaw.MoveDirection = 1;
                downJigsaw.CanMove = true;
            }
        }
        //判断左方拼图
        if (curNull_y - 1 >= 0)
        {
            Jigsaw leftJigsaw = FindJigsawInIndex(3 * curNull_x + curNull_y - 1);
            if (leftJigsaw != null)
            {
                leftJigsaw.MoveDirection = 4;
                leftJigsaw.CanMove = true;
            }
        }
        //判断右方拼图
        if (curNull_y + 1 <= 2)
        {
            Jigsaw rightJigsaw = FindJigsawInIndex(3 * curNull_x + curNull_y + 1);
            if (rightJigsaw != null)
            {
                rightJigsaw.MoveDirection = 3;
                rightJigsaw.CanMove = true;
            }
        }


    }


    //玩家发生了移动操作
    public void MoveOperation(Jigsaw jigsaw, int moveDirection)
    {
        int temp = jigsaw.CurPosIndex;
        jigsaw.CurPosIndex = indexOfNull;
        indexOfNull = temp;
        float moveDis = Mathf.Abs(leftPos.position.x - rightPos.position.x);
        //向上移动
        if (moveDirection == 1)
        {
            float y = jigsaw.transform.position.y;
            y +=  moveDis;
            jigsaw.transform.DOMoveY(y, move_time);
        }
        //向下移动
        else if (moveDirection == 2)
        {
            float y = jigsaw.transform.position.y;
            y -= moveDis;
            jigsaw.transform.DOMoveY(y, move_time);
        }
        //向左移动
        else if (moveDirection == 3)
        {
            float x = jigsaw.transform.position.x;
            x -= moveDis;
            jigsaw.transform.DOMoveX(x, move_time);
        }
        //向右移动
        else if (moveDirection == 4)
        {
            float x = jigsaw.transform.position.x;
            x += moveDis;
            jigsaw.transform.DOMoveX(x, move_time);
        }
        else
        {
            Debug.LogError("moveDirection设置有问题！  jigsaw correct index = " + jigsaw.correctIndex) ;
        }
        BanAllJigsawMove();
        Invoke("JudgeIsCorrectOrNot", move_time);
    }

    //禁止所有拼图移动
    public void BanAllJigsawMove()
    {
        resetBtn.enabled = false;
        foreach (Jigsaw j in jigsawList)
            j.CanMove = false;
    }



    //判断结果是否正确
    public void JudgeIsCorrectOrNot()
    {
        //空缺位置都不一样可以直接错误，减少消耗
        if (indexOfNull != correctIndexOfNull)
        {
            SetJigsawCanMoveOrNot();
            return;
        }
        foreach (Jigsaw j in jigsawList)
        {
            //错误
            if (j.CurPosIndex != j.correctIndex)
            {
                SetJigsawCanMoveOrNot();
                return;
            }
        }
        //正确
        CompleteJigsaw();
    }

    public void CompleteJigsaw()
    {
        completeAudio.Play();
        tabletPanel.DOFade(0, pictureShow_time);
        sisterWallPanel.gameObject.SetActive(true);
        sisterWallPanel.DOFade(1, pictureShow_time);
        ChapManager.Instance.Chap2ChangeToSisterRoom();
        Invoke("SetSelfFalse", pictureShow_time);
    }

    public void SetSelfFalse()
    {
        sisterWallPanel.gameObject.GetComponent<SisterWallPanel>().MoveToDown();
        GameAudioManager.Instance.PlayEaringRingAudio();
        transform.parent.gameObject.SetActive(false);
    }


    //查找对应位置上的拼图
    public Jigsaw FindJigsawInIndex(int findIndex)
    {
        foreach(Jigsaw j in jigsawList)
        {
            if (findIndex == j.CurPosIndex)
                return j;
        }
        return null; 
    }
}
