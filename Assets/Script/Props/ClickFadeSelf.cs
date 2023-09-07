using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


//��������Ҫ����󽥱���ʧ��������
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
        //��һ��Ҫ����İ�ť����һ�����У�
        if (nextBtn != null)
            nextBtn.enabled = true;
        else if (hidePanel != null)
            hidePanel.SetActive(false);
    }
}
