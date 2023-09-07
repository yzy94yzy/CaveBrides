using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;



public class PlayerManager : MonoSingleton<PlayerManager>
{
    Quaternion _rotation, _camRotation;

    //背包系统是否已经解锁
    public bool PropBarIsUnlock
    {
        set;
        get;
    }   

    //表里世界切换是否已经解锁
    public bool SwitchWorldUnlock
    {
        set;
        get;
    }

    //速度
    public float moveSpeed = 3;

    //脚步控制
    [SerializeField]
    private List<AudioSource> footStepsAudioList;

    //用来控制表里世界的切换
    public bool IsInside
    {
        get;
        set;
    }
    //记录玩家当前是小时候还是长大
    public bool IsSmall
    {
        set;
        get;
    }

    [SerializeField]
    private SpriteRenderer player_sprite;
    //动画控制
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Sprite player_inside_sprite, player_outside_sprite, player_small_outside_sprite, player_small_inside_sprite;
    [SerializeField]
    private AudioSource q_audio;

    //防止转向影响相机跟随
    [SerializeField]
    private Transform cameraFollow;
    //判断是否面朝右面
    public bool facingRight = true;

    [SerializeField]
    private GameObject fire_outside, fire_inside;

    //防止重复触发对话bug
    [SerializeField]
    private BoxCollider2D dialogueSystemTrigger;

    //测试使用
    [SerializeField]
    private JigsawPanel jigsawPanel;

    protected override void Init()
    {

        IsInside = false;
        IsSmall = false;
        ///测试修改
        PropBarIsUnlock = false;
        SwitchWorldUnlock = false;
    }

    void Start()
    {

        _rotation = this.gameObject.transform.rotation;
        _camRotation = cameraFollow.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        //水平方向的移动,适用于手柄和键盘
        float h = 0;
        if (Input.GetAxisRaw("Horizontal2") != 0)
        {
            h = Input.GetAxisRaw("Horizontal2");
        }
        else
        {
            h = Input.GetAxisRaw("Horizontal");
        }
        Vector3 vec = new Vector3(h, 0f, 0f);

        if (vec.magnitude * moveSpeed != 0 && !footStepsAudioList[ChapManager.Instance.CurChap - 1].isPlaying)
        {
            footStepsAudioList[ChapManager.Instance.CurChap - 1].Play();
        }
        else if (vec.magnitude * moveSpeed == 0 && footStepsAudioList[ChapManager.Instance.CurChap - 1].isPlaying)
        {
            footStepsAudioList[ChapManager.Instance.CurChap - 1].Pause();
        }
        //x轴 * 移动方向 * 移动速度 * 按照时间移动，按照世界坐标
        transform.Translate(Vector3.right * h * moveSpeed * Time.deltaTime, Space.World);

        //设置动画
        anim.SetFloat("Speed",vec.magnitude * moveSpeed);
        //设置转身功能
        if (h > 0 && !facingRight) { Filp(); }
        else if (h < 0 && facingRight)
        {
            Filp();
        }


        //表里世界切换
        if ((Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.JoystickButton3)) && SwitchWorldUnlock)
        {
            SwitchWorld();
        }


        /*//测试使用
        if (Input.GetKeyDown(KeyCode.V))
            moveSpeed = 15;
        if (Input.GetKeyDown(KeyCode.J))
            jigsawPanel.CompleteJigsaw();
        if (Input.GetKeyDown(KeyCode.U))
        {
            SwitchWorldUnlock = true;
            PropBarIsUnlock = true;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ChapManager.Instance.LoadSceneByChap(2);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ChapManager.Instance.LoadSceneByChap(3);
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ChapManager.Instance.LoadSceneByChap(4);

        }*/
    }

    private void OnEnable()
    {
        dialogueSystemTrigger.enabled = true;
    }

    private void OnDisable()
    {
        dialogueSystemTrigger.enabled = false;
    }


    //调整玩家现在的前进方向
    public void Filp()
    {
        facingRight = !facingRight;

        //旋转玩家的位置以及保证摄像头不要跟着玩家进行旋转
        _rotation.y += 180;
        _camRotation.y = cameraFollow.rotation.y - ((_rotation.y % 180) % 2) * 180;

        if (_rotation.y >= 360) { _rotation.y -= 360; }
        transform.rotation = _rotation;

        cameraFollow.rotation = _camRotation;
    }

    public void UnlockPropBarPanel(bool isUnlock)
    {
        PropBarIsUnlock = true;
    }

    //防止玩家行走中触发对话的bug
    public void StopPlayerAnim()
    {
        anim.SetFloat("Speed", 0);
        footStepsAudioList[ChapManager.Instance.CurChap - 1].Pause();
        /*
        //道具栏会有问题
        if (UIManager.Instance.propBarIsOpen == true)
        {
            UIManager.Instance.OpenAndClosePropBar();
        }*/
    }

    //切换表里世界
    public void SwitchWorld()
    {
        q_audio.Play();

        //切换人物表现
        if (IsInside)
        {
            if (IsSmall)
                player_sprite.sprite = player_small_outside_sprite;
            else
                player_sprite.sprite = player_outside_sprite;
            //灯光切换只有山洞里面有 之后第四章再加
            if (fire_inside.activeInHierarchy || fire_outside.activeInHierarchy)
            {
                fire_outside.SetActive(false);
                fire_inside.SetActive(true);
            }
            IsInside = false;
        }
        else
        {
            if (IsSmall)
                player_sprite.sprite = player_small_inside_sprite;
            else
                player_sprite.sprite = player_inside_sprite;
            if (fire_inside.activeInHierarchy || fire_outside.activeInHierarchy)
            {
                fire_inside.SetActive(false);
                fire_outside.SetActive(true);
            }
            IsInside = true;
        }
        //切换人物动画
        anim.SetBool("IsInside", IsInside);


        //切换背景场景和人物表现（放后面 不然变量还没改变）
        ChapManager.Instance.SwitchInsideOrOutside();
    }

    public void SwitchToChamber()
    {
        fire_outside.SetActive(false);
    }

    public void SwitchToSmall()
    {
        IsSmall = true;
        moveSpeed = 2;
        anim.SetBool("IsSmall", true);
        player_sprite.sprite = player_small_outside_sprite;
    }
}
