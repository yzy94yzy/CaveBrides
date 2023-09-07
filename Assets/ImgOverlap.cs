using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImgOverlap : MonoBehaviour
{
    public List<GameObject> objList;
    public GameObject cursor;
    private RectTransform rtCursor;


    // Start is called before the first frame update
    void Start()
    {
        /*rt1 = GetComponent<RectTransform>();
        rt2 = GetComponent<RectTransform>();*/
        rtCursor = cursor.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject obj in objList)
        {
            RectTransform rt = obj.GetComponent<RectTransform>();

            if (RectTransToScreenPos(rt, null).Overlaps(RectTransToScreenPos(rtCursor, null)))
            {
                Button btn = obj.GetComponent<Button>();
                //Debug.Log("overlap" + obj.name);
                //Debug.Log(btn.enabled);

                if (Input.GetKeyDown(KeyCode.JoystickButton0) && btn.enabled && gameObject.GetComponent<CanvasGroup>().alpha == 1)
                {
                    //Debug.Log(obj.name);
                    btn.onClick.Invoke();
                }
            }
        }
    }

    public static Rect RectTransToScreenPos(RectTransform rt, Camera cam)
    {
        Vector3[] corners = new Vector3[4];
        rt.GetWorldCorners(corners);
        Vector2 v0 = RectTransformUtility.WorldToScreenPoint(cam, corners[0]);
        Vector2 v1 = RectTransformUtility.WorldToScreenPoint(cam, corners[2]);
        Rect rect = new Rect(v0, v1 - v0);
        //Debug.Log(rect);
        return rect;
    }
}
