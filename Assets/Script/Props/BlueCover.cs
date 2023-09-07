using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BlueCover : MonoBehaviour
{
    [SerializeField]
    private RectTransform coverEndPos;
    [SerializeField]
    private RectTransform dollScary, blueCover;
    [SerializeField]
    private Button button;

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(OnButtonClick);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.JoystickButton0) && button.enabled)
        {
            button.onClick.Invoke();
        }
    }

    public void OnButtonClick()
    {
        blueCover.DOMoveX(coverEndPos.position.x, 2f);
        Invoke("SetSelfFase", 2f);
        Vector3 vec = dollScary.localScale;
        vec.x *= 2;
        vec.y *= 2;
        vec.z *= 2;
        dollScary.gameObject.SetActive(true);
        dollScary.DOScale(vec, 3f);
    }

    public void SetSelfFase()
    {
        gameObject.SetActive(false);
    }
}
