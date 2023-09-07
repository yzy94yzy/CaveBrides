using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;



public class PlayerManager : MonoSingleton<PlayerManager>
{
    Quaternion _rotation, _camRotation;

    //����ϵͳ�Ƿ��Ѿ�����
    public bool PropBarIsUnlock
    {
        set;
        get;
    }   

    //���������л��Ƿ��Ѿ�����
    public bool SwitchWorldUnlock
    {
        set;
        get;
    }

    //�ٶ�
    public float moveSpeed = 3;

    //�Ų�����
    [SerializeField]
    private List<AudioSource> footStepsAudioList;

    //�������Ʊ���������л�
    public bool IsInside
    {
        get;
        set;
    }
    //��¼��ҵ�ǰ��Сʱ���ǳ���
    public bool IsSmall
    {
        set;
        get;
    }

    [SerializeField]
    private SpriteRenderer player_sprite;
    //��������
    [SerializeField]
    private Animator anim;
    [SerializeField]
    private Sprite player_inside_sprite, player_outside_sprite, player_small_outside_sprite, player_small_inside_sprite;
    [SerializeField]
    private AudioSource q_audio;

    //��ֹת��Ӱ���������
    [SerializeField]
    private Transform cameraFollow;
    //�ж��Ƿ��泯����
    public bool facingRight = true;

    [SerializeField]
    private GameObject fire_outside, fire_inside;

    //��ֹ�ظ������Ի�bug
    [SerializeField]
    private BoxCollider2D dialogueSystemTrigger;

    //����ʹ��
    [SerializeField]
    private JigsawPanel jigsawPanel;

    protected override void Init()
    {

        IsInside = false;
        IsSmall = false;
        ///�����޸�
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
        //ˮƽ������ƶ�,�������ֱ��ͼ���
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
        //x�� * �ƶ����� * �ƶ��ٶ� * ����ʱ���ƶ���������������
        transform.Translate(Vector3.right * h * moveSpeed * Time.deltaTime, Space.World);

        //���ö���
        anim.SetFloat("Speed",vec.magnitude * moveSpeed);
        //����ת����
        if (h > 0 && !facingRight) { Filp(); }
        else if (h < 0 && facingRight)
        {
            Filp();
        }


        //���������л�
        if ((Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.JoystickButton3)) && SwitchWorldUnlock)
        {
            SwitchWorld();
        }


        /*//����ʹ��
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


    //����������ڵ�ǰ������
    public void Filp()
    {
        facingRight = !facingRight;

        //��ת��ҵ�λ���Լ���֤����ͷ��Ҫ������ҽ�����ת
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

    //��ֹ��������д����Ի���bug
    public void StopPlayerAnim()
    {
        anim.SetFloat("Speed", 0);
        footStepsAudioList[ChapManager.Instance.CurChap - 1].Pause();
        /*
        //��������������
        if (UIManager.Instance.propBarIsOpen == true)
        {
            UIManager.Instance.OpenAndClosePropBar();
        }*/
    }

    //�л���������
    public void SwitchWorld()
    {
        q_audio.Play();

        //�л��������
        if (IsInside)
        {
            if (IsSmall)
                player_sprite.sprite = player_small_outside_sprite;
            else
                player_sprite.sprite = player_outside_sprite;
            //�ƹ��л�ֻ��ɽ�������� ֮��������ټ�
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
        //�л����ﶯ��
        anim.SetBool("IsInside", IsInside);


        //�л�����������������֣��ź��� ��Ȼ������û�ı䣩
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
