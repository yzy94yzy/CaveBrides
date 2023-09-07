using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class ChapScene_4 : MonoBehaviour
{
    //场景切换时间
    public float sameChapSceneCrossTime;
    //玩家初始位置
    public Vector3 chap4_River, chap4_ImaginationCave;
    
    //第四章场景
    [SerializeField]
    private GameObject river_inside_scene, river_outside_scene, sisterroom_inside_scene, sisterroom_outside_scene, imagination_river, imagination_cave;
    //第四章道具
    [SerializeField]
    private GameObject closedoor, opendoor, sister, stoneStatue_close, stoneStatue_open, camera_foreground;
    //小鸟控制
    [SerializeField]
    private Transform bird, birdCameraShot;
    [SerializeField]
    private AudioSource imaginationBGM;

    //第四章摄像机位置

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //姐姐家到河边玩家的位置不一样且需要黑幕过渡
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

        //防止玩家切换瞬间去按有问题
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
        // 调整玩家朝向 这里是朝左
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

    //姐姐家的互动
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
            Debug.LogError("玩家不在第四章");
        }
    }

    //解锁河边里世界石像
    public void StoneStatueInsideUnlock()
    {
        stoneStatue_close.SetActive(false);
        stoneStatue_open.SetActive(true);
    }
}
