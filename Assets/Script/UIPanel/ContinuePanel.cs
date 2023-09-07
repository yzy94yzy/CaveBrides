using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ContinuePanel : MonoBehaviour
{
    [SerializeField]
    private List<ChapterBtn> chapterBtnList;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnEnable()
    {
        int m_chap = LoadManager.Instance.LoadGame();
        //Debug.Log("active button before " + m_chap);
        for (int i = 0; i < 4; i++)
        {
            if (i < m_chap)
                chapterBtnList[i].Unlock();
            else
                chapterBtnList[i].Lock();
        }
    }
}
