using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Lock : MonoBehaviour
{
    public float move_time;
    //能停留的最上方和下方边界；重置另一张的位置；求diff的差距
    [SerializeField]
    private RectTransform max_up, max_down, resetLocknumPos_up, resetLocknumPos_down, num_1, num_2;
    //现在这个锁停在几号
    public int CurNum
    {
        get;
        set;
    }

    [SerializeField]
    private Button upBtn, downBtn;
    [SerializeField]
    private Transform lockNum_1, lockNum_2;
    // Start is called before the first frame update
    void Start()
    {
        CurNum = 0;

        upBtn.onClick.AddListener(OnUpBtnClick);
        downBtn.onClick.AddListener(OnDownBtnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnUpBtnClick()
    {
        float diff = Mathf.Abs(num_1.position.y - num_2.position.y);
        if (CurNum == 0)
        {
            CurNum = 4;
            //确定目前视野内的是哪一张，并将另一张放到前面去
            if (Mathf.Abs(lockNum_1.position.y - max_up.position.y) < 0.01)
            {
                Vector3 resetPos = new Vector3(lockNum_2.position.x, resetLocknumPos_up.position.y, lockNum_2.position.z);
                lockNum_2.position = resetPos;
            }
            else
            {
                Vector3 resetPos = new Vector3(lockNum_1.position.x, resetLocknumPos_up.position.y, lockNum_1.position.z);
                lockNum_1.position = resetPos;
            }
        }
        else
            CurNum--;
        lockNum_1.DOMoveY(lockNum_1.position.y - diff, move_time);
        lockNum_2.DOMoveY(lockNum_2.position.y - diff, move_time);

        BanButton();
        Invoke("ActiveButton", move_time);
    }

    public void OnDownBtnClick()
    {
        float diff = Mathf.Abs(num_1.position.y - num_2.position.y);
        if (CurNum == 4)
        {
            CurNum = 0;
            //确定目前视野内的是哪一张，并将另一张放到后面去
            if (Mathf.Abs(lockNum_1.position.y - max_down.position.y) < 0.01)
            {
                Vector3 resetPos = new Vector3(lockNum_2.position.x, resetLocknumPos_down.position.y, lockNum_2.position.z);
                lockNum_2.position = resetPos;
            }
            else
            {
                Vector3 resetPos = new Vector3(lockNum_1.position.x, resetLocknumPos_down.position.y, lockNum_1.position.z);
                lockNum_1.position = resetPos;
            }
        }
        else
            CurNum++;
        //移动后重置已经出视野的那张
        lockNum_1.DOMoveY(lockNum_1.position.y + diff, move_time);
        lockNum_2.DOMoveY(lockNum_2.position.y + diff, move_time);

        BanButton();
        Invoke("ActiveButton", move_time);
    }

    public void BanButton()
    {
        upBtn.enabled = false;
        downBtn.enabled = false;
    }

    public void ActiveButton()
    {
        upBtn.enabled = true;
        downBtn.enabled = true;
    }
}
