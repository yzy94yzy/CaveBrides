using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChapterBtn : MonoBehaviour
{
    [SerializeField]
    private int chapterNum;
    [SerializeField]
    private Image lockImg;
    [SerializeField]
    private GameObject chapterTitle;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(() => LoadManager.Instance.ChangeSceneDelay(chapterNum));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Unlock()
    {
        gameObject.GetComponent<Button>().enabled = true;
        lockImg.gameObject.SetActive(false);
        chapterTitle.SetActive(true);
        this.GetComponent<Image>().color = Color.white;
    }

    public void Lock()
    {
        gameObject.GetComponent<Button>().enabled = false;
        lockImg.gameObject.SetActive(true);
        chapterTitle.SetActive(false);

        //        this.GetComponent<Image>().color = new Color(188, 181, 160);
        this.GetComponent<Image>().color = Color.grey;
    }
}
