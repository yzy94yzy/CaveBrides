using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class StoneStatuePanel_Chap4 : MonoBehaviour
{
    public float change_time;
    [SerializeField]
    private Image dolls_hand;
    [SerializeField]
    private CanvasGroup stoneStatue_open, stoneStatue_angry, stoneStatue_close;
    [SerializeField]
    private AudioSource stoneAngryAudio, eatAudio, laughAudio;
    [SerializeField]
    private RectTransform dollsEndPos;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void HideHandDoll()
    {
        dolls_hand.DOFade(0, change_time);
        Invoke("ChangeStoneStatueToAngry", change_time);
    }

    private void ChangeStoneStatueToAngry()
    {
        stoneAngryAudio.Play();
        dolls_hand.gameObject.SetActive(false);
        stoneStatue_open.DOFade(0, change_time);
        stoneStatue_angry.DOFade(1, change_time);
        Invoke("SetSelfFalse", change_time);
    }

    public void GiveHandDoll()
    {
        dolls_hand.rectTransform.DOMoveY(dollsEndPos.position.y, change_time);
        Invoke("HideDolls", change_time);
    }

    private void HideDolls()
    {
        dolls_hand.DOFade(0, change_time);
        Invoke("ChangeStoneStatueToClose", change_time);
    }

    private void ChangeStoneStatueToClose()
    {
        eatAudio.Play();
        laughAudio.PlayDelayed(0.5f);
        dolls_hand.gameObject.SetActive(false);
        stoneStatue_open.DOFade(0, change_time);
        stoneStatue_close.DOFade(1, change_time);
        Invoke("GameEnd", change_time);
    }
    private void GameEnd()
    {
        VideoManager.Instance.PlayGiveAllDollsVideo();
    }

    private void SetSelfFalse()
    {
        UIManager.Instance.ClosePanelSlowly(gameObject.GetComponent<CanvasGroup>());
    }
}
