using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GetPropPanel : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeSelf()
    {
        UIManager.Instance.ClosePanelSlowly(gameObject.GetComponent<CanvasGroup>());
    }
}
