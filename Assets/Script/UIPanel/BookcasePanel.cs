using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookcasePanel : MonoBehaviour
{
    [SerializeField]
    private Canvas translateBook, letter, sisterNote;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ChangeTranslateToTop()
    {
        if (translateBook.sortingOrder == 2)
            return;
        //Debug.Log("translate top");
        translateBook.sortingOrder = 2;
        if (letter.sortingOrder > 0)
            letter.sortingOrder--;
        if (sisterNote.sortingOrder > 0)
            sisterNote.sortingOrder--;
    }
    public void ChangeSisterNoteToTop()
    {
        if (sisterNote.sortingOrder == 2)
            return;
        //Debug.Log("sister top");
        sisterNote.sortingOrder = 2;
        if (letter.sortingOrder > 0)
            letter.sortingOrder--;
        if (translateBook.sortingOrder > 0)
            translateBook.sortingOrder--;
    }
    public void ChangeLetterToTop()
    {
        if (letter.sortingOrder == 2)
            return;
        //Debug.Log("letter top");
        letter.sortingOrder = 2;
        if (sisterNote.sortingOrder > 0)
            sisterNote.sortingOrder--;
        if (translateBook.sortingOrder > 0)
            translateBook.sortingOrder--;
    }

}
