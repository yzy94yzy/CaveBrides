using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ChapManager : MonoSingleton<ChapManager>
{
    //当前所处章节
    public int CurChap
    {
        get;
        set;
    }

    //章节的玩家开始位置重置
    public Vector3 chapStartPoint, chap2_Chamber, chap2_Lobbies, chap2_SisterRoom,
        chap3_River_Left, chap3_River_Right, chap3_SisterRoom;

    //同章节场景切换时间
    public float sameChapSceneCrossTime;

    //需要重置的东西
    [SerializeField]
    private Transform player, bird;
    //章节场景的摄像机位置
    [SerializeField]
    private Transform chamberCameraSpot, lobbiesCameraSpot, sisterRoomCameraSpot,
        forestCameraSpot, playerCameraSpot, sisterRoomOutsideCameraSpot, sisterRoomInsideCameraSpot,
        sisterRoomOutsideCameraSpot_chap4, sisterRoomInsideCameraSpot_chap4;
    //章节场景
    [SerializeField]
    private GameObject chapScene_1, chapScene_2, chapScene_3, chapScene_4;
    [SerializeField]
    private CinemachineVirtualCamera cm;

    //第二章大堂检查玩家是否拥有了所有道具再激活
    [SerializeField]
    private GameObject tablet_lock, tablet_unlock;
    //第二章场景
    [SerializeField]
    private GameObject chamber_scene, lobbies_scene, sisterRoom_scene;
    //第三章场景
    [SerializeField]
    private GameObject forest_scene, river_inside_scene, river_outside_scene, sisterroom_inside_scene, sisterroom_outside_scene;
    //第四章场景
    [SerializeField]
    private GameObject sisterroom_outside_chap4_scene, sisterroom_inside_chap4_scene;

    /*//玩家现在的状态是不是鸟
    private bool isBird;*/

    // Start is called before the first frame update
    void Start()
    {
        ///测试
        CurChap = 1;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //进入第二章
    public void ChangeToChap2()
    {
        CurChap = 2;
        GameManager.Instance.MaxChap = GameManager.Instance.MaxChap > CurChap ? GameManager.Instance.MaxChap : CurChap;
        //调整玩家朝向
        if (!PlayerManager.Instance.facingRight)
            PlayerManager.Instance.Filp();
        //自动保存玩家已解锁章节
        LoadManager.Instance.SaveGame();
        //摄像机跟随切换
        cm.Follow = chamberCameraSpot;
        player.position = chapStartPoint;
        PlayerManager.Instance.SwitchToChamber();
        //场景和UI切换
        chapScene_1.SetActive(false);
        chapScene_2.SetActive(true);
        //音效切换
        //GameAudioManager.Instance.PlayChamberAudio();
    }

    //第二章内部的场景切换
    //切换到闺房
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

    //切换到大堂
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

    //切换到姐姐家：因为不是显示的不需要黑幕过渡，有祠堂过渡。
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

    //切换到第三章场景
    public void ChangeToChap3()
    {
        CurChap = 3;
        GameManager.Instance.MaxChap = GameManager.Instance.MaxChap > CurChap ? GameManager.Instance.MaxChap : CurChap;
        // 调整玩家朝向
        if (!PlayerManager.Instance.facingRight)
            PlayerManager.Instance.Filp();
        LoadManager.Instance.SaveGame();
        //玩家状态这次好像没有要切换的
        //PlayerManager.Instance.SwitchToChamber();
        Invoke("MovePlayer", 0.5f);
        //场景和UI切换
        chapScene_3.SetActive(true);
        //摄像机跟随切换
        cm.Follow = forestCameraSpot;
        //道具栏修改
        PropManager.Instance.UseProp(PropType.Picture);
        PropManager.Instance.UseProp(PropType.SisterNote);
        PropManager.Instance.UseProp(PropType.Letter);
    }

    private void MovePlayer()
    {
        player.position = chapStartPoint;
    }


    //切换到河边：不需要黑幕过渡，有拍照过渡。
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

    //姐姐家到河边玩家的位置不一样且需要黑幕过渡
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

        //防止玩家切换瞬间去按有问题
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
        //防止玩家切换瞬间去按有问题
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

    //切换到第四章 
    public void ChangeToChap4()
    {
        CurChap = 4;
        GameManager.Instance.MaxChap = GameManager.Instance.MaxChap > CurChap ? GameManager.Instance.MaxChap : CurChap;
        // 调整玩家朝向
        if (!PlayerManager.Instance.facingRight)
            PlayerManager.Instance.Filp();
        LoadManager.Instance.SaveGame();
        //场景和UI切换
        chapScene_4.SetActive(true);
        //摄像机跟随切换
        cm.Follow = sisterRoomOutsideCameraSpot_chap4;
        player.position = chapStartPoint;
        //玩家状态切换到小时候
        PlayerManager.Instance.SwitchToSmall();
        //道具栏修改
        PropManager.Instance.UseProp(PropType.Knife);
    }

    public void Chap4ChangeToRiverDelay()
    {
        // 调整玩家朝向
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

    //切换玩家所在地的表里世界
    public void SwitchInsideOrOutside()
    {
        if (ChapManager.Instance.CurChap == 3)
            chapScene_3.GetComponent<ChapScene_3>().SwitchInsideOrOutside();
        else if (ChapManager.Instance.CurChap == 4)
            chapScene_4.GetComponent<ChapScene_4>().SwitchInsideOrOutside();
    }


    //复杂的读档加载对应场景
    public void LoadScene(Vector3 playerPos, bool isBird, bool isSmall)
    {

        //加载玩家
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
            //还原第一章道具栏
            PropManager.Instance.AddNewProp(PropType.Earing);
            PropManager.Instance.GetDolls();
            PropManager.Instance.UseDolls();
            PropManager.Instance.SetPropNum(2);

            PlayerManager.Instance.PropBarIsUnlock = true;
            //切换到第二章场景
            ChangeToChap2();
        }
        else if (chap == 3)
        {
            //还原前两章道具栏
            PropManager.Instance.AddNewProp(PropType.Earing);
            PropManager.Instance.GetDolls();
            PropManager.Instance.UseDolls();
            PropManager.Instance.UseDolls();
            PropManager.Instance.SetPropNum(2);

            PlayerManager.Instance.PropBarIsUnlock = true;
            //切换到第三章场景
            chapScene_1.gameObject.SetActive(false);
            cm.Follow = forestCameraSpot;
            chapScene_3.SetActive(true);
        }
        else if (chap == 4)
        {
            //还原前两章道具栏
            PropManager.Instance.AddNewProp(PropType.Earing);
            PropManager.Instance.GetDolls();
            PropManager.Instance.UseDolls();
            PropManager.Instance.UseDolls();
            PropManager.Instance.UseDolls();
            PropManager.Instance.AddNewProp(PropType.Bamboo);
            PropManager.Instance.AddNewProp(PropType.DuckHeart);
            PropManager.Instance.AddNewProp(PropType.Knife);
            PropManager.Instance.SetPropNum(5);
            //玩家解锁了背包和摇铃
            PlayerManager.Instance.PropBarIsUnlock = true;
            PlayerManager.Instance.SwitchWorldUnlock = true;
            //切换到第四章场景
            chapScene_1.gameObject.SetActive(false);
            cm.Follow = forestCameraSpot;
            chapScene_4.SetActive(true);
            //还原姐姐家神龛
            PropManager.Instance.SetCabinetSisterroomToChap4();
            ChangeToChap4();
        }
    }
}
