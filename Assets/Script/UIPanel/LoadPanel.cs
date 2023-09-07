using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LoadPanel : BasePanel
{
    public float change_time, scale_big;
    [SerializeField]
    private Button startBtn;
    [SerializeField]
    private RectTransform hand;
    // Start is called before the first frame update
    void Start()
    {
        startBtn.onClick.AddListener(OnStartBtnClick);
        //Debug.Log("demo 2.5 9/5 9:29");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0) && startBtn.enabled)
        {
            startBtn.onClick.Invoke();
        }
    }

    private void OnStartBtnClick()
    {
        Vector3 vec = hand.localScale;
        vec.x *= scale_big;
        vec.y *= scale_big;
        vec.z *= scale_big;
        DOTween.Init();
        hand.DOScale(vec, change_time);
        Invoke("ChangeScene", change_time);
    }

    private void ChangeScene()
    {
        DOTween.Kill(hand);
        GameManager.Instance.LoadSceneToStartScene(1);
    }
}
