using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class JigsawPanel : MonoBehaviour
{
    //�����±겼�� x = num / 3 ; y = num % 3; num = 3x + y
    // 0 1 2 
    // 3 4 5
    // 6 7 8

    /* ���̿�ʼ״̬    ����״̬
     * 4     3         0     2
     * 7  6  2         3  4  5
     * 0  8  5         6  7  8
     */

    //��¼Ŀǰ��ȱ��λ���±����ȷ�Ŀ�ȱ�±� 
    private int indexOfNull, correctIndexOfNull;

    //ƴͼ�ƶ�ʱ���ת������ʱ��
    public float move_time, pictureShow_time;

    //��¼���е�ƴͼ
    [SerializeField]
    private List<Jigsaw> jigsawList;
    //ƴ��ƴͼת��
    [SerializeField]
    private CanvasGroup tabletPanel, sisterWallPanel;
    //�����ɹ���Ч
    [SerializeField]
    private AudioSource completeAudio;
    //����ƴͼ
    [SerializeField]
    private Button resetBtn;
    //��¼ƴͼҪ�ƶ��ľ���
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

    //����ƴͼ��ʼλ��
    public void SetOriginPosCurIndex()
    {
        jigsawList[0].CurPosIndex = 0;
        //��Ϊ�ڶ����ǿ��ŵ�
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


    //ȷ��ƴͼ�Ƿ�����ƶ�
    public void SetJigsawCanMoveOrNot()
    {
        BanAllJigsawMove();
        resetBtn.enabled = true;
        int curNull_x = indexOfNull / 3;
        int curNull_y = indexOfNull % 3;

        //�ж��Ϸ�ƴͼ
        if (curNull_x - 1 >= 0)
        {
            Jigsaw upJigsaw = FindJigsawInIndex(3 * (curNull_x - 1) + curNull_y);
            if (upJigsaw != null)
            {
                upJigsaw.MoveDirection = 2;
                upJigsaw.CanMove = true;
            }
        }
        //�ж��·�ƴͼ
        if (curNull_x + 1 <= 2)
        {
            Jigsaw downJigsaw = FindJigsawInIndex(3 * (curNull_x + 1) + curNull_y);
            if (downJigsaw != null)
            {
                downJigsaw.MoveDirection = 1;
                downJigsaw.CanMove = true;
            }
        }
        //�ж���ƴͼ
        if (curNull_y - 1 >= 0)
        {
            Jigsaw leftJigsaw = FindJigsawInIndex(3 * curNull_x + curNull_y - 1);
            if (leftJigsaw != null)
            {
                leftJigsaw.MoveDirection = 4;
                leftJigsaw.CanMove = true;
            }
        }
        //�ж��ҷ�ƴͼ
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


    //��ҷ������ƶ�����
    public void MoveOperation(Jigsaw jigsaw, int moveDirection)
    {
        int temp = jigsaw.CurPosIndex;
        jigsaw.CurPosIndex = indexOfNull;
        indexOfNull = temp;
        float moveDis = Mathf.Abs(leftPos.position.x - rightPos.position.x);
        //�����ƶ�
        if (moveDirection == 1)
        {
            float y = jigsaw.transform.position.y;
            y +=  moveDis;
            jigsaw.transform.DOMoveY(y, move_time);
        }
        //�����ƶ�
        else if (moveDirection == 2)
        {
            float y = jigsaw.transform.position.y;
            y -= moveDis;
            jigsaw.transform.DOMoveY(y, move_time);
        }
        //�����ƶ�
        else if (moveDirection == 3)
        {
            float x = jigsaw.transform.position.x;
            x -= moveDis;
            jigsaw.transform.DOMoveX(x, move_time);
        }
        //�����ƶ�
        else if (moveDirection == 4)
        {
            float x = jigsaw.transform.position.x;
            x += moveDis;
            jigsaw.transform.DOMoveX(x, move_time);
        }
        else
        {
            Debug.LogError("moveDirection���������⣡  jigsaw correct index = " + jigsaw.correctIndex) ;
        }
        BanAllJigsawMove();
        Invoke("JudgeIsCorrectOrNot", move_time);
    }

    //��ֹ����ƴͼ�ƶ�
    public void BanAllJigsawMove()
    {
        resetBtn.enabled = false;
        foreach (Jigsaw j in jigsawList)
            j.CanMove = false;
    }



    //�жϽ���Ƿ���ȷ
    public void JudgeIsCorrectOrNot()
    {
        //��ȱλ�ö���һ������ֱ�Ӵ��󣬼�������
        if (indexOfNull != correctIndexOfNull)
        {
            SetJigsawCanMoveOrNot();
            return;
        }
        foreach (Jigsaw j in jigsawList)
        {
            //����
            if (j.CurPosIndex != j.correctIndex)
            {
                SetJigsawCanMoveOrNot();
                return;
            }
        }
        //��ȷ
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


    //���Ҷ�Ӧλ���ϵ�ƴͼ
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
