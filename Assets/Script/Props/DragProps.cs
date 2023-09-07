using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragProps : MonoBehaviour
{
    protected Vector3 offsetPos;
    [SerializeField]
    protected Transform completePos;
    [SerializeField]
    protected Transform startPos;

    public virtual void GetOffsetPos()
    {
        offsetPos = Input.mousePosition - transform.position;
    }

    public virtual void DragPropsMethod()
    {
        transform.position = Input.mousePosition - offsetPos;
    }

    public virtual void CheckPosition()
    {
        if (Mathf.Sqrt((completePos.position - transform.position).magnitude) < 5)
        {
            transform.position = completePos.position;
        }
        else
        {
            transform.position = startPos.position;
        }
    }

}
