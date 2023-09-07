using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//���ڵ�����������İ�ť
public class DestroyBtn : MonoBehaviour
{
    [SerializeField]
    private GameObject destroyGameocject;

    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(OnBtnClick);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnBtnClick()
    {
        Destroy(destroyGameocject);
        Destroy(gameObject);
    }
}
