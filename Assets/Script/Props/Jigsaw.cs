using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Jigsaw : MonoBehaviour
{
    [SerializeField]
    private RectTransform resetPos;

    public int correctIndex;

    public int CurPosIndex
    {
        get;
        set;
    }

    public bool CanMove
    {
        get;
        set;
    }
    
    //上下左右 1234
    public int MoveDirection
    {
        get;
        set;
    }

    [SerializeField]
    private Button selfBtn;
    [SerializeField]
    private JigsawPanel jigsawPanel;
    [SerializeField]
    private AudioSource moveAudio, cantMoveAudio;

    // Start is called before the first frame update
    void Start()
    {
        selfBtn.onClick.AddListener(OnSelfBtnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSelfBtnClick()
    {
        if (CanMove)
        {
            moveAudio.Play();
            jigsawPanel.MoveOperation(this, MoveDirection);
        }
        else
            cantMoveAudio.Play();
    }

    //重置拼图位置
    public void ResetPos()
    {
        transform.position = resetPos.position;
    }
}
