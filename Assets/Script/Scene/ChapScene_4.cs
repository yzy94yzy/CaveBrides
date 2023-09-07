using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ChapScene_4 : MonoBehaviour
{
    //�����л�ʱ��
    public float sameChapSceneCrossTime;
    //��ҳ�ʼλ��
    public Vector3 chap4_River, chap4_ImaginationCave;
    
    //�����³���
    [SerializeField]
    private GameObject river_inside_scene, river_outside_scene, sisterroom_inside_scene, sisterroom_outside_scene, imagination_river, imagination_cave;
    //�����µ���
    [SerializeField]
    private GameObject closedoor, opendoor, sister, stoneStatue_close, stoneStatue_open, camera_foreground;
    //С�����
    [SerializeField]
    private Transform bird, birdCameraShot;
    [SerializeField]
    private AudioSource imaginationBGM;

    //�����������λ��

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //���ҵ��ӱ���ҵ�λ�ò�һ������Ҫ��Ļ����
    private CinemachineVirtualCamera cm;
    private Transform playerCameraSpot;
    private Transform player;
    public void Chap4ChangeToRiverDelay(CinemachineVirtualCamera cm, Transform playerCameraSpot, Transform player)
    {
        this.cm = cm;
        this.playerCameraSpot = playerCameraSpot;
        this.player = player;
        UIManager.Instance.BlackCross(sameChapSceneCrossTime);
        Invoke("Chap4ChangeToRiver", sameChapSceneCrossTime);
    }


    private void Chap4ChangeToRiver()
    {
        GameAudioManager.Instance.PlayOpenDoorAudio();
        if (PlayerManager.Instance.IsInside)
            river_inside_scene.SetActive(true);
        else
            river_outside_scene.SetActive(true);

        //��ֹ����л�˲��ȥ��������
        if (sisterroom_inside_scene.activeInHierarchy)
            sisterroom_inside_scene.SetActive(false);
        else
            sisterroom_outside_scene.SetActive(false);
        cm.Follow = playerCameraSpot;
        player.position = chap4_River;
    }

    public void HideScene_Reality_River()
    {
        river_inside_scene.SetActive(false);
        river_outside_scene.SetActive(false);
    }

    public void Chap4ChangeToImaginationRiver(CinemachineVirtualCamera cm, Transform player)
    {
        imaginationBGM.gameObject.SetActive(true);
        camera_foreground.SetActive(false);
        player.gameObject.SetActive(false);
        bird.gameObject.SetActive(true);
        cm.Follow = birdCameraShot;
        // ������ҳ��� �����ǳ���
        if (BirdManager.Instance.facingRight)
            BirdManager.Instance.Filp();
        imagination_river.SetActive(true);
    }

    public void Chap4ChangeToImaginationCaveDelay()
    {
        UIManager.Instance.BlackCross(sameChapSceneCrossTime);
        Invoke("Chap4ChangeToImaginationCave", sameChapSceneCrossTime);
    }


    private void Chap4ChangeToImaginationCave()
    {
        imagination_river.SetActive(false);
        imagination_cave.SetActive(true);
        bird.position = chap4_ImaginationCave;
    }

    public void HideScene_Chap4_River_Inside()
    {
        river_inside_scene.SetActive(false);
    }

    public void HideScene_Imagination_Cave()
    {
        imaginationBGM.gameObject.SetActive(false);
        imagination_cave.SetActive(false);
    }

    //���ҵĻ���
    public void CheckOpenDoorOrNot()
    {
        if (PropManager.Instance.CheckPropIsOwn(PropType.Book) && sister.GetComponent<BoxCollider2D>().enabled == false)
        {
            closedoor.SetActive(false);
            opendoor.SetActive(true);
        }
    }

    public void SwitchInsideOrOutside()
    {
        if (river_inside_scene.activeInHierarchy || river_outside_scene.activeInHierarchy)
        {
            if (PlayerManager.Instance.IsInside)
            {
                river_outside_scene.SetActive(false);
                river_inside_scene.SetActive(true);
            }
            else
            {
                river_inside_scene.SetActive(false);
                river_outside_scene.SetActive(true);
            }
        }
        else if (sisterroom_inside_scene.activeInHierarchy || sisterroom_outside_scene.activeInHierarchy)
        {
            if (PlayerManager.Instance.IsInside)
            {
                sisterroom_outside_scene.SetActive(false);
                sisterroom_inside_scene.SetActive(true);
            }
            else
            {
                sisterroom_inside_scene.SetActive(false);
                sisterroom_outside_scene.SetActive(true);
            }
        }
        else
        {
            Debug.LogError("��Ҳ��ڵ�����");
        }
    }

    //�����ӱ�������ʯ��
    public void StoneStatueInsideUnlock()
    {
        stoneStatue_close.SetActive(false);
        stoneStatue_open.SetActive(true);
    }
}
