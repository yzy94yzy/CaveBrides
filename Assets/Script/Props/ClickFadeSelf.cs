using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


//挂载在需要点击后渐变消失的物体上
public class ClickFadeSelf : MonoBehaviour
{

    public float fade_time;
    [SerializeField]
    private Button selfBtn, nextBtn;
    [SerializeField]
    private GameObject hidePanel;
    // Start is called before the first frame update
    void Start()
    {
        selfBtn.onClick.AddListener(OnSelfBtnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnSelfBtnClick()
    {
        gameObject.GetComponent<Image>().DOFade(0, fade_time);
        Invoke("HideSelf", fade_time);
    }

    private void HideSelf()
    {
        gameObject.SetActive(false);
        //下一个要激活的按钮（不一定会有）
        if (nextBtn != null)
            nextBtn.enabled = true;
        else if (hidePanel != null)
            hidePanel.SetActive(false);
    }
}
