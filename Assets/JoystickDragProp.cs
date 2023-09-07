using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JoystickDragProp : MonoBehaviour
{
    public RectTransform completePos;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool CheckPosition()
    {
        if (Mathf.Sqrt((completePos.transform.position - transform.position).magnitude) < 15)
            return true;
        else
            return false;
    }
}
