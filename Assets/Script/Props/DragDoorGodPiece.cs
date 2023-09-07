using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragDoorGodPiece : DragProps
{
    public bool IsCorrect
    {
        get;
        set;
    }
    [SerializeField]
    private DoorGodPanel doorGodPanel;


    public override void CheckPosition()
    {
        if (Mathf.Sqrt((completePos.position - transform.position).magnitude) < 5)
        {
            if (!IsCorrect)
            {
                IsCorrect = true;
                doorGodPanel.CompleteOnePiece();
            }
            //Debug.Log("succeed");
            transform.position = completePos.position;
        }
        else
        {
            //Debug.Log("failed");
            if (IsCorrect)
            {
                IsCorrect = false;
                doorGodPanel.RemoveOnePiece();
            }

        }
    }
}
