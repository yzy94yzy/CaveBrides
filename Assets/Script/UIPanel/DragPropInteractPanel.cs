using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DragPropInteractPanel : MonoBehaviour
{
    public float change_time;
    //拼凑完成前后图片
    [SerializeField]
    private CanvasGroup change_before, change_after;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void CompleteDrag()
    {
        DOTween.Init();
        change_before.DOFade(0, change_time);
        change_after.gameObject.SetActive(true);
        change_after.DOFade(1, change_time);
        Invoke("ClosePanel", change_time);
    }

    public void ClosePanel()
    {
        UIManager.Instance.ClosePanelSlowly(gameObject.GetComponent<CanvasGroup>());
    }
}
