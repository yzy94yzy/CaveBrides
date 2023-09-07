using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragWindowpaper : DragProps
{
    [SerializeField]
    private DoorGodPanel doorGodPanel;


    // Update is called once per frame
    void Update()
    {
        
    }

    public override void CheckPosition()
    {
        if (Mathf.Sqrt((completePos.transform.position - transform.position).magnitude) < 5)
        {
            doorGodPanel.CompleteDoorGod();
            transform.position = completePos.position;
        }
        else
        {
            transform.position = startPos.position;
        }
    }
}
