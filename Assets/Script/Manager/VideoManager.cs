using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using DG.Tweening;
using System.IO;

public class VideoManager : MonoSingleton<VideoManager>
{
    private bool isPlayingEnd;
    private VideoPlayer currentPlayVideo;
    //视频消失黑幕过渡时间
    public float blackPanelFadeTime;

    [SerializeField]
    private CanvasGroup videoGroup;
    [SerializeField]
    private GameObject earingEndTrigger, stick, cabinetMemoryEndTrigger, sisterShowEndTrigger, sisterSoulTrigger, sister_inside;
    [SerializeField]
    private VideoPlayer earingVideo, threeVideo, fourVideo, cabinetMemoryVideo, getMarriedVideo, blindDateVideo, sisterShowVideo,
        changeToBirdVideo, gameEndVideo, giveAllDollsVideo;



    // Start is called before the first frame update
    void Start()
    {
        isPlayingEnd = false;
        earingVideo.loopPointReached += OnEaringVideoEnd;
        threeVideo.loopPointReached += OnThreeVideoEnd;
        fourVideo.loopPointReached += OnFourVideoEnd;
        cabinetMemoryVideo.loopPointReached += OnCabinetMemoryEnd;
        getMarriedVideo.loopPointReached += OnGetMarriedEnd;
        blindDateVideo.loopPointReached += OnBlindDateEnd;
        sisterShowVideo.loopPointReached += OnSisterShowVideoEnd;
        changeToBirdVideo.loopPointReached += OnChangeToBirdVideoEnd;
        gameEndVideo.loopPointReached += OnGameEndVideoEnd;
        giveAllDollsVideo.loopPointReached += OnGameEndVideoEnd;

        earingVideo.url = Path.Combine(Application.streamingAssetsPath, "EaringShow") + ".mp4";
        threeVideo.url = Path.Combine(Application.streamingAssetsPath, "EndWithThreeSticks") + ".mp4";
        fourVideo.url = Path.Combine(Application.streamingAssetsPath, "EndWithFourSticks") + ".mp4";
        cabinetMemoryVideo.url = Path.Combine(Application.streamingAssetsPath, "CabinetMemory") + ".mp4";
        getMarriedVideo.url = Path.Combine(Application.streamingAssetsPath, "GetMarried") + ".mp4";
        blindDateVideo.url = Path.Combine(Application.streamingAssetsPath, "BlindDate") + ".mp4";
        sisterShowVideo.url = Path.Combine(Application.streamingAssetsPath, "SisterShow") + ".mp4";
        changeToBirdVideo.url = Path.Combine(Application.streamingAssetsPath, "ChangeToBird") + ".mp4";
        gameEndVideo.url = Path.Combine(Application.streamingAssetsPath, "GameEnd") + ".mp4";
        giveAllDollsVideo.url = Path.Combine(Application.streamingAssetsPath, "GiveAllDolls") + ".mp4";
    }

    // Update is called once per frame
    void Update()
    {
        //检测到播放结束以后要进行的操作
        if (isPlayingEnd)
        {
            //耳环动画结束的要激活对话
            if (currentPlayVideo == earingVideo)
                earingEndTrigger.SetActive(true);
            
        }
    }

    //通用的视频结束操作
    private void OnVideoEnd(VideoPlayer videoPlayer)
    {
        isPlayingEnd = true;
        currentPlayVideo = videoPlayer;

        currentPlayVideo.gameObject.SetActive(false);
        isPlayingEnd = false;
        //等一下再执行显示方法 防止移动过程被看到
        Invoke("FadeBlackPanel", blackPanelFadeTime);
    }

    //渐渐显示，防止掉帧问题
    private void FadeBlackPanel()
    {
        DOTween.Init();
        videoGroup.DOFade(0, blackPanelFadeTime);
        Invoke("HideBlackPanel", blackPanelFadeTime);
    }

    private void HideBlackPanel()
    {
        videoGroup.DOFade(1, 0f);
        videoGroup.gameObject.SetActive(false);
    }

    //耳环视频结束
    private void OnEaringVideoEnd(VideoPlayer videoPlayer)
    {
        earingEndTrigger.SetActive(true);
        OnVideoEnd(videoPlayer);
        GameAudioManager.Instance.Chap1GameSoundPlay();
    }

    //三根香结束不需要再播放水滴那些声音了
    private void OnThreeVideoEnd(VideoPlayer videoPlayer)
    {
        //后面有对话可以恢复玩家行动
        //GameObject.FindWithTag("Player").GetComponent<PlayerManager>().enabled = true;
        ChapManager.Instance.ChangeToChap2();
        OnVideoEnd(videoPlayer);
    }
    private void OnFourVideoEnd(VideoPlayer videoPlayer)
    {
        //GameObject.FindWithTag("Player").GetComponent<PlayerManager>().enabled = true;
        stick.SetActive(true);
        OnVideoEnd(videoPlayer);
        GameAudioManager.Instance.Chap1GameSoundPlay();
    }

    private void OnCabinetMemoryEnd(VideoPlayer videoPlayer)
    {
        cabinetMemoryEndTrigger.SetActive(true);
        OnVideoEnd(videoPlayer);
    }
    private void OnGetMarriedEnd(VideoPlayer videoPlayer)
    {
        ChapManager.Instance.ChangeToChap3();
        //播放下一章声音
       // GameAudioManager.Instance.Chap3GameSoundPlay();
        OnVideoEnd(videoPlayer);
    }
    private void OnBlindDateEnd(VideoPlayer videoPlayer)
    {
        ChapManager.Instance.ChangeToChap4();
        //播放下一章声音
        //GameAudioManager.Instance.Chap4GameSoundPlay();
        OnVideoEnd(videoPlayer);
    }

    private void OnSisterShowVideoEnd(VideoPlayer videoPlayer)
    {
        sisterShowEndTrigger.SetActive(true);
        sisterSoulTrigger.SetActive(true);
        sister_inside.SetActive(true);
        OnVideoEnd(videoPlayer);
    }
    private void OnChangeToBirdVideoEnd(VideoPlayer videoPlayer)
    {
        OnVideoEnd(videoPlayer);
    }
    private void OnGameEndVideoEnd(VideoPlayer videoPlayer)
    {
        OnVideoEnd(videoPlayer);
        GameManager.Instance.LoadSceneByIndex(1);
    }

    public void PlayEaringVideo()
    {
        videoGroup.gameObject.SetActive(true);
        earingVideo.gameObject.SetActive(true);
        GameAudioManager.Instance.Chap1GameSoundStop();
        earingVideo.Play();
    }

    public void PlayThreeSticks()
    {
        videoGroup.gameObject.SetActive(true);
        threeVideo.gameObject.SetActive(true);
        GameAudioManager.Instance.Chap1GameSoundStop();
        threeVideo.Play();
    }

    public void PlayFourSticks()
    {
        videoGroup.gameObject.SetActive(true);
        fourVideo.gameObject.SetActive(true);
        GameAudioManager.Instance.Chap1GameSoundStop();
        fourVideo.Play();
    }

    public void PlayCabinetMemory()
    {
        videoGroup.gameObject.SetActive(true);
        cabinetMemoryVideo.gameObject.SetActive(true);
        cabinetMemoryVideo.Play();
    }

    public void PlayGetMarried()
    {
        //隐藏第二章场景 并在播放结束后激活第三章场景
        ChapManager.Instance.HideScene_2();
        videoGroup.gameObject.SetActive(true);
        getMarriedVideo.gameObject.SetActive(true);
        getMarriedVideo.Play();
    }

    public void PlayBlineDate()
    {
        videoGroup.gameObject.SetActive(true);
        blindDateVideo.gameObject.SetActive(true);
        blindDateVideo.Play();
    }
    public void PlaySisterShowVideo()
    {
        PlayerManager.Instance.SwitchWorld();
        videoGroup.gameObject.SetActive(true);
        sisterShowVideo.gameObject.SetActive(true);
        sisterShowVideo.Play();
    }
    public void PlayChangeToBirdVideo()
    {
        //隐藏现实河边场景 防止BGM影响
        ChapManager.Instance.HideScene_Reality_River();
        //直接切换 CG要和下一个场景的BGM一样
        ChapManager.Instance.Chap4ChangeToImaginationRiver();
        videoGroup.gameObject.SetActive(true);
        changeToBirdVideo.gameObject.SetActive(true);
        changeToBirdVideo.Play();
    }

    public void PlayGameEndVideo()
    {
        //隐藏现实河边场景 防止BGM影响
        ChapManager.Instance.HideScene_Imagination_Cave();
        videoGroup.gameObject.SetActive(true);
        gameEndVideo.gameObject.SetActive(true);
        gameEndVideo.Play();
    }

    public void PlayGiveAllDollsVideo()
    {
        ChapManager.Instance.HideScene_Chap4_River_Inside();
        videoGroup.gameObject.SetActive(true);
        giveAllDollsVideo.gameObject.SetActive(true);
        giveAllDollsVideo.Play();
    }
}
