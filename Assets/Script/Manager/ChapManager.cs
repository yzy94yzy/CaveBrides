using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChapManager : MonoSingleton<ChapManager>
{
    //��ǰ�����½�
    public int CurChap
    {
        get;
        set;
    }

    //�½ڵ���ҿ�ʼλ������
    public Vector3 chapStartPoint, chap2_Chamber, chap2_Lobbies, chap2_SisterRoom,
        chap3_River_Left, chap3_River_Right, chap3_SisterRoom;

    //ͬ�½ڳ����л�ʱ��
    public float sameChapSceneCrossTime;

    //��Ҫ���õĶ���
    [SerializeField]
    private Transform player, bird;
    //�½ڳ����������λ��
    [SerializeField]
    private Transform chamberCameraSpot, lobbiesCameraSpot, sisterRoomCameraSpot,
        forestCameraSpot, playerCameraSpot, sisterRoomOutsideCameraSpot, sisterRoomInsideCameraSpot,
        sisterRoomOutsideCameraSpot_chap4, sisterRoomInsideCameraSpot_chap4;
    //�½ڳ���
    [SerializeField]
    private GameObject chapScene_1, chapScene_2, chapScene_3, chapScene_4;
    [SerializeField]
    private CinemachineVirtualCamera cm;

    //�ڶ��´��ü������Ƿ�ӵ�������е����ټ���
    [SerializeField]
    private GameObject tablet_lock, tablet_unlock;
    //�ڶ��³���
    [SerializeField]
    private GameObject chamber_scene, lobbies_scene, sisterRoom_scene;
    //�����³���
    [SerializeField]
    private GameObject forest_scene, river_inside_scene, river_outside_scene, sisterroom_inside_scene, sisterroom_outside_scene;
    //�����³���
    [SerializeField]
    private GameObject sisterroom_outside_chap4_scene, sisterroom_inside_chap4_scene;

    /*//������ڵ�״̬�ǲ�����
    private bool isBird;*/

    // Start is called before the first frame update
    void Start()
    {
        ///����
        CurChap = 1;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //����ڶ���
    public void ChangeToChap2()
    {
        CurChap = 2;
        GameManager.Instance.MaxChap = GameManager.Instance.MaxChap > CurChap ? GameManager.Instance.MaxChap : CurChap;
        //������ҳ���
        if (!PlayerManager.Instance.facingRight)
            PlayerManager.Instance.Filp();
        //�Զ���������ѽ����½�
        LoadManager.Instance.SaveGame();
        //����������л�
        cm.Follow = chamberCameraSpot;
        player.position = chapStartPoint;
        PlayerManager.Instance.SwitchToChamber();
        //������UI�л�
        chapScene_1.SetActive(false);
        chapScene_2.SetActive(true);
        //��Ч�л�
        //GameAudioManager.Instance.PlayChamberAudio();
    }

    //�ڶ����ڲ��ĳ����л�
    //�л����뷿
    public void Chap2ChangeToChamberDelay()
    {
        UIManager.Instance.BlackCross(sameChapSceneCrossTime);
        Invoke("Chap2ChangeToChamber", sameChapSceneCrossTime);
    }

    private void Chap2ChangeToChamber()
    {
        GameAudioManager.Instance.PlayOpenDoorAudio();
        //GameAudioManager.Instance.PlayChamberAudio();
        cm.Follow = chamberCameraSpot;
        player.position = chap2_Chamber;
        chamber_scene.SetActive(true);
        lobbies_scene.SetActive(false);
    }

    //�л�������
    public void Chap2ChangeToLobbiesDelay()
    {
        if (PropManager.Instance.CheckPropIsOwn(PropType.Letter) && PropManager.Instance.CheckPropIsOwn(PropType.Picture))
        {
            tablet_lock.SetActive(false);
            tablet_unlock.SetActive(true);
        }
        UIManager.Instance.BlackCross(sameChapSceneCrossTime);
        Invoke("Chap2ChangeToLobbies", sameChapSceneCrossTime);
    }

    private void Chap2ChangeToLobbies()
    {
        GameAudioManager.Instance.PlayOpenDoorAudio();
        //GameAudioManager.Instance.ChangeToLobbiesAudio();
        lobbies_scene.SetActive(true);
        chamber_scene.SetActive(false);
        cm.Follow = lobbiesCameraSpot;
        player.position = chap2_Lobbies;
    }

    //�л������ң���Ϊ������ʾ�Ĳ���Ҫ��Ļ���ɣ������ù��ɡ�
    public void Chap2ChangeToSisterRoom()
    {
        //UIManager.Instance.BlackCross(sameChapSceneCrossTime);
        sisterRoom_scene.SetActive(true);
        lobbies_scene.SetActive(false);
        cm.Follow = sisterRoomCameraSpot;
        player.position = chap2_SisterRoom;
    }

    public void HideScene_2()
    {
        chapScene_2.SetActive(false);
    }

    //�л��������³���
    public void ChangeToChap3()
    {
        CurChap = 3;
        GameManager.Instance.MaxChap = GameManager.Instance.MaxChap > CurChap ? GameManager.Instance.MaxChap : CurChap;
        // ������ҳ���
        if (!PlayerManager.Instance.facingRight)
            PlayerManager.Instance.Filp();
        LoadManager.Instance.SaveGame();
        //���״̬��κ���û��Ҫ�л���
        //PlayerManager.Instance.SwitchToChamber();
        Invoke("MovePlayer", 0.5f);
        //������UI�л�
        chapScene_3.SetActive(true);
        //����������л�
        cm.Follow = forestCameraSpot;
        //�������޸�
        PropManager.Instance.UseProp(PropType.Picture);
        PropManager.Instance.UseProp(PropType.SisterNote);
        PropManager.Instance.UseProp(PropType.Letter);
    }

    private void MovePlayer()
    {
        player.position = chapStartPoint;
    }


    //�л����ӱߣ�����Ҫ��Ļ���ɣ������չ��ɡ�
    public void Chap3ForestChangeToRiver()
    {
        //UIManager.Instance.BlackCross(sameChapSceneCrossTime);
        forest_scene.SetActive(false);
        if (PlayerManager.Instance.IsInside)
            river_inside_scene.SetActive(true);
        else
            river_outside_scene.SetActive(true);
        cm.Follow = playerCameraSpot;
        player.position = chap3_River_Left;
    }

    //���ҵ��ӱ���ҵ�λ�ò�һ������Ҫ��Ļ����
    public void Chap3SisterRoomChangeToRiverDelay()
    {
        UIManager.Instance.BlackCross(sameChapSceneCrossTime);
        Invoke("Chap3SisterRoomChangeToRiver", sameChapSceneCrossTime);
    }


    private void Chap3SisterRoomChangeToRiver()
    {
        GameAudioManager.Instance.PlayOpenDoorAudio();
        UIManager.Instance.BlackCross(sameChapSceneCrossTime);
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
        player.position = chap3_River_Right;
    }

    public void Chap3ChangeToSisterRoomDelay()
    {
        UIManager.Instance.BlackCross(sameChapSceneCrossTime);
        Invoke("Chap3ChangeToSisterRoom", sameChapSceneCrossTime);
    }

    private void Chap3ChangeToSisterRoom()
    {
        GameAudioManager.Instance.PlayOpenDoorAudio();
        if (PlayerManager.Instance.IsInside)
        {
            sisterroom_inside_scene.SetActive(true);
            cm.Follow = sisterRoomInsideCameraSpot;
        }
        else
        {
            sisterroom_outside_scene.SetActive(true);
            cm.Follow = sisterRoomOutsideCameraSpot;
        }
        //��ֹ����л�˲��ȥ��������
        if (river_inside_scene.activeInHierarchy)
            river_inside_scene.SetActive(false);
        else
            river_outside_scene.SetActive(false);
        player.position = chap3_SisterRoom;
    }

    public void HideScene_3()
    {
        chapScene_3.SetActive(false);
    }

    //�л��������� 
    public void ChangeToChap4()
    {
        CurChap = 4;
        GameManager.Instance.MaxChap = GameManager.Instance.MaxChap > CurChap ? GameManager.Instance.MaxChap : CurChap;
        // ������ҳ���
        if (!PlayerManager.Instance.facingRight)
            PlayerManager.Instance.Filp();
        LoadManager.Instance.SaveGame();
        //������UI�л�
        chapScene_4.SetActive(true);
        //����������л�
        cm.Follow = sisterRoomOutsideCameraSpot_chap4;
        player.position = chapStartPoint;
        //���״̬�л���Сʱ��
        PlayerManager.Instance.SwitchToSmall();
        //�������޸�
        PropManager.Instance.UseProp(PropType.Knife);
    }

    public void Chap4ChangeToRiverDelay()
    {
        // ������ҳ���
        if (PlayerManager.Instance.facingRight)
            PlayerManager.Instance.Filp();
        chapScene_4.GetComponent<ChapScene_4>().Chap4ChangeToRiverDelay(cm, playerCameraSpot, player);
    }

    public void HideScene_Reality_River()
    {
        chapScene_4.GetComponent<ChapScene_4>().HideScene_Reality_River();
    }

    public void Chap4ChangeToImaginationRiver()
    {
        chapScene_4.GetComponent<ChapScene_4>().Chap4ChangeToImaginationRiver(cm, player);
    }
    public void Chap4ChangeToImaginationCaveDelay()
    {
        chapScene_4.GetComponent<ChapScene_4>().Chap4ChangeToImaginationCaveDelay();
    }

    public void HideScene_Chap4_River_Inside()
    {
        chapScene_4.GetComponent<ChapScene_4>().HideScene_Chap4_River_Inside();
    }

    public void HideScene_Imagination_Cave()
    {
        chapScene_4.GetComponent<ChapScene_4>().HideScene_Imagination_Cave();
    }

    //�л�������ڵصı�������
    public void SwitchInsideOrOutside()
    {
        if (ChapManager.Instance.CurChap == 3)
            chapScene_3.GetComponent<ChapScene_3>().SwitchInsideOrOutside();
        else if (ChapManager.Instance.CurChap == 4)
            chapScene_4.GetComponent<ChapScene_4>().SwitchInsideOrOutside();
    }


    //���ӵĶ������ض�Ӧ����
    public void LoadScene(Vector3 playerPos, bool isBird, bool isSmall)
    {

        //�������
        if (isBird)
        {
            player.gameObject.SetActive(false);
            bird.gameObject.SetActive(true);
            bird.position = playerPos;
        }
        else
        {
            if (isSmall)
                PlayerManager.Instance.SwitchToSmall();
            player.position = playerPos;
        }

    }

    public void LoadSceneByChap(int chap)
    {
        UIManager.Instance.BlackMaskCross(sameChapSceneCrossTime);
        if (chap == 1)
        {
            chapScene_1.gameObject.SetActive(true);
            return;
        }
        CurChap = chap;
        GameManager.Instance.MaxChap = GameManager.Instance.MaxChap > chap ? GameManager.Instance.MaxChap : chap;
        LoadManager.Instance.SaveGame();
        GameAudioManager.Instance.Chap1GameSoundStop();
        if (chap == 2)
        {
            //��ԭ��һ�µ�����
            PropManager.Instance.AddNewProp(PropType.Earing);
            PropManager.Instance.GetDolls();
            PropManager.Instance.UseDolls();
            PropManager.Instance.SetPropNum(2);

            PlayerManager.Instance.PropBarIsUnlock = true;
            //�л����ڶ��³���
            ChangeToChap2();
        }
        else if (chap == 3)
        {
            //��ԭǰ���µ�����
            PropManager.Instance.AddNewProp(PropType.Earing);
            PropManager.Instance.GetDolls();
            PropManager.Instance.UseDolls();
            PropManager.Instance.UseDolls();
            PropManager.Instance.SetPropNum(2);

            PlayerManager.Instance.PropBarIsUnlock = true;
            //�л��������³���
            chapScene_1.gameObject.SetActive(false);
            cm.Follow = forestCameraSpot;
            chapScene_3.SetActive(true);
        }
        else if (chap == 4)
        {
            //��ԭǰ���µ�����
            PropManager.Instance.AddNewProp(PropType.Earing);
            PropManager.Instance.GetDolls();
            PropManager.Instance.UseDolls();
            PropManager.Instance.UseDolls();
            PropManager.Instance.UseDolls();
            PropManager.Instance.AddNewProp(PropType.Bamboo);
            PropManager.Instance.AddNewProp(PropType.DuckHeart);
            PropManager.Instance.AddNewProp(PropType.Knife);
            PropManager.Instance.SetPropNum(5);
            //��ҽ����˱�����ҡ��
            PlayerManager.Instance.PropBarIsUnlock = true;
            PlayerManager.Instance.SwitchWorldUnlock = true;
            //�л��������³���
            chapScene_1.gameObject.SetActive(false);
            cm.Follow = forestCameraSpot;
            chapScene_4.SetActive(true);
            //��ԭ��������
            PropManager.Instance.SetCabinetSisterroomToChap4();
            ChangeToChap4();
        }
    }
}
