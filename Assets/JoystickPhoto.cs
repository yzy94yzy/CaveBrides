using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class JoystickPhoto : MonoBehaviour
{
    public float showAll_time, white_time, showShootPhoto_time;
    private float moveSpeed;
    [SerializeField]
    private Transform photo, startRiverTrigger;
    private Vector3 photoFixPos;
    [SerializeField]
    private GameObject completePos;
    [SerializeField]
    private Image photo_all, whiteCross;
    [SerializeField]
    private CanvasGroup shootImg;
    // Start is called before the first frame update
    void Start()
    {
        moveSpeed = 600;
        photoFixPos = photo.position;
    }

    // Update is called once per frame
    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal3");
        float v = Input.GetAxisRaw("Vertical3");
        if (h + v != 0)
        {
            transform.Translate(Vector3.right * h * moveSpeed * Time.deltaTime);
            transform.Translate(Vector3.up * v * moveSpeed * Time.deltaTime);
            photo.position = photoFixPos;
            if (Mathf.Sqrt((completePos.transform.position - transform.position).magnitude) < 5)
            {
                photo_all.gameObject.SetActive(true);
                photo_all.DOFade(1, showAll_time);
                Invoke("WhiteCross", showAll_time);
                gameObject.SetActive(false);
            }
        }

        
    }

    public void WhiteCross()
    {
        whiteCross.DOFade(1, white_time);
        Invoke("WhiteFade", white_time);
    }

    public void WhiteFade()
    {
        whiteCross.DOFade(0, white_time);
        Invoke("ShowShootPhoto", white_time);
    }

    public void ShowShootPhoto()
    {
        ChapManager.Instance.Chap3ForestChangeToRiver();
        shootImg.gameObject.SetActive(true);
        shootImg.DOFade(1, showShootPhoto_time);
        Invoke("HideShootPhoto", showShootPhoto_time);
    }

    public void HideShootPhoto()
    {
        shootImg.transform.parent.GetComponent<CanvasGroup>().DOFade(0, showShootPhoto_time);
        Invoke("PhotoGamePanelFalse", showShootPhoto_time);
    }

    public void PhotoGamePanelFalse()
    {
        shootImg.transform.parent.gameObject.SetActive(false);
        startRiverTrigger.gameObject.SetActive(true);
    }
}
