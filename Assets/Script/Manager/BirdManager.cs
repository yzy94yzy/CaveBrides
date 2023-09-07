using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdManager : MonoSingleton<BirdManager>
{
    Quaternion _rotation, _camRotation;

    //速度
    public float moveSpeed = 5;

    //防止转向影响相机跟随
    [SerializeField]
    private Transform cameraFollow;

    [SerializeField]
    private AudioSource birdFlyAudio;
    //判断是否面朝右面
    public bool facingRight = false;

    //动画控制
    [SerializeField]
    private Animator anim;

    //防止重复触发对话bug
    [SerializeField]
    private BoxCollider2D dialogueSystemTrigger;

    // Start is called before the first frame update
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

        //鸟儿扑腾翅膀的声音
        if (vec.magnitude * moveSpeed != 0 && !birdFlyAudio.isPlaying)
        {
            birdFlyAudio.Play();
        }
        else if (vec.magnitude * moveSpeed == 0 && birdFlyAudio.isPlaying)
        {
            birdFlyAudio.Pause();
        }
        //x轴 * 移动方向 * 移动速度 * 按照时间移动，按照世界坐标
        transform.Translate(Vector3.right * h * moveSpeed * Time.deltaTime, Space.World);

        //设置动画
        anim.SetFloat("Speed", vec.magnitude * moveSpeed);
        //设置转身功能
        if (h > 0 && !facingRight) { Filp(); }
        else if (h < 0 && facingRight)
        {
            Filp();
        }

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

    //防止玩家行走中触发对话的bug
    public void StopBirdAnim()
    {
        anim.SetFloat("Speed", 0);
        //birdFlyAudio.Pause();
    }
}
