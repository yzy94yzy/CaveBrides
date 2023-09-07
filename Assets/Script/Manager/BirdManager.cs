using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdManager : MonoSingleton<BirdManager>
{
    Quaternion _rotation, _camRotation;

    //�ٶ�
    public float moveSpeed = 5;

    //��ֹת��Ӱ���������
    [SerializeField]
    private Transform cameraFollow;

    [SerializeField]
    private AudioSource birdFlyAudio;
    //�ж��Ƿ��泯����
    public bool facingRight = false;

    //��������
    [SerializeField]
    private Animator anim;

    //��ֹ�ظ������Ի�bug
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

        //������ڳ�������
        if (vec.magnitude * moveSpeed != 0 && !birdFlyAudio.isPlaying)
        {
            birdFlyAudio.Play();
        }
        else if (vec.magnitude * moveSpeed == 0 && birdFlyAudio.isPlaying)
        {
            birdFlyAudio.Pause();
        }
        //x�� * �ƶ����� * �ƶ��ٶ� * ����ʱ���ƶ���������������
        transform.Translate(Vector3.right * h * moveSpeed * Time.deltaTime, Space.World);

        //���ö���
        anim.SetFloat("Speed", vec.magnitude * moveSpeed);
        //����ת����
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

    //��ֹ��������д����Ի���bug
    public void StopBirdAnim()
    {
        anim.SetFloat("Speed", 0);
        //birdFlyAudio.Pause();
    }
}
