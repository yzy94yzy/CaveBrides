using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowPropDetail : MonoBehaviour
{
    [SerializeField]
    private int propIndex;
    [SerializeField]
    private Button selfBtn;
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
        PropManager.Instance.ShowPropDetailPanel(propIndex, gameObject.GetComponent<Image>().sprite);
    }
}
