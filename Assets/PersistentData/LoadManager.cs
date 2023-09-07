using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class LoadManager : MonoSingleton<LoadManager>
{
    //需要储存的数据
    int m_chap;/*
    bool m_isBird;
    bool m_isSmall;
    Vector3 m_playerPos;*/
    protected override void Init()
    {
        DontDestroyOnLoad(this);
    }
    //创建用于存储的类，并在其中填入需要储存的数据
    [System.Serializable]
    public class Save
    {
        //玩家当前所在章节
        public int s_chap;/*
        //玩家是不是鸟
        public bool s_isBird;
        //玩家是不是小时候的状态
        public bool s_isSmall;
        //玩家所在的位置
        public Vector3 s_playerPos;*/
    }

    //创建或覆盖存档
    public void SaveGame()
    {
        //赋值想要储存的数据
        m_chap = GameManager.Instance.MaxChap;
        //如果已经存在存档，则检查和当前章节相比那个更大，保留更后面的进度
        if (File.Exists(Application.persistentDataPath + "/CaveBride.save"))
        {
            BinaryFormatter bf_1 = new BinaryFormatter();
            FileStream file_1 = File.Open(Application.persistentDataPath + "/CaveBride.save", FileMode.Open);
            Save save_1 = (Save)bf_1.Deserialize(file_1);
            file_1.Close();
            //将存档中的数据提取出来
            int have_chap = save_1.s_chap;
            //结束，重启游戏进行测试
            m_chap = have_chap > m_chap ? have_chap : m_chap;
        }
        //创建新存档覆盖
        Save save = new Save();
        save.s_chap = m_chap;
        //Debug.Log("new save " + save.s_chap);
        //设置储存路径并储存
        string Save_Path = Application.persistentDataPath + "/CaveBride.save";
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Save_Path);
        bf.Serialize(file, save);
        file.Close();
    }
    //读取存档
    public int LoadGame()
    {
        //检测是否存在存档，若存在则读取存档
        if (File.Exists(Application.persistentDataPath + "/CaveBride.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/CaveBride.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();
            //将存档中的数据提取出来
            m_chap = save.s_chap;
            //结束，重启游戏进行测试
            //Debug.Log("load game max chap " + save.s_chap);
            if (m_chap > 1)
            {
                GameManager.Instance.MaxChap = m_chap;
                return m_chap;
            }
        }
        return 1;
    }
    int loadChap = 1;
    public void ChangeSceneDelay(int loadChap)
    {
        this.loadChap = loadChap;
        GameManager.Instance.LoadSceneByIndex(2);
        //给场景加载一点时间
        //Invoke("ChangeScene", 0.8f);
        
    }

    public void ChangeScene()
    {
        if (loadChap == 1)
        {
            PropManager.Instance.SetPropNum(0);/*
            UIManager.Instance.CloseBlackImmediately();
            return;*/
        }

        ChapManager.Instance.LoadSceneByChap(loadChap);
    }
}
