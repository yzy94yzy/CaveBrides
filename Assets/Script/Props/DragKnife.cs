using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class DragKnife : DragProps
{

    [SerializeField]
    private DuckPanel duckPanel;
    [SerializeField]
    private AudioSource cutMeat;
    void Update()
    {
        
    }

    public override void CheckPosition()
    {
        if (Mathf.Sqrt((completePos.transform.position - transform.position).magnitude) < 15)
        {
            cutMeat.Play();
            duckPanel.MoveKnife();
        }
        else
        {
            transform.position = startPos.position;
        }
    }
    
}
