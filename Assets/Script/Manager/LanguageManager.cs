using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void LanguageChangeHandler(bool isChinese);

public class LanguageManager : MonoSingleton<LanguageManager>
{
    public LanguageChangeHandler LanguageChange;
    public bool IsChinese
    {
        get;
        set;
    }
    protected override void Init()
    {
        IsChinese = true;
        DontDestroyOnLoad(this);
    }

    public void ChangeLanguage(bool isChinese)
    {
        IsChinese = isChinese;
        LanguageChange?.Invoke(isChinese);
    }
}
