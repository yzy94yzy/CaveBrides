using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class LoadManager : MonoSingleton<LoadManager>
{
    //��Ҫ���������
    int m_chap;/*
    bool m_isBird;
    bool m_isSmall;
    Vector3 m_playerPos;*/
    protected override void Init()
    {
        DontDestroyOnLoad(this);
    }
    //�������ڴ洢���࣬��������������Ҫ���������
    [System.Serializable]
    public class Save
    {
        //��ҵ�ǰ�����½�
        public int s_chap;/*
        //����ǲ�����
        public bool s_isBird;
        //����ǲ���Сʱ���״̬
        public bool s_isSmall;
        //������ڵ�λ��
        public Vector3 s_playerPos;*/
    }

    //�����򸲸Ǵ浵
    public void SaveGame()
    {
        //��ֵ��Ҫ���������
        m_chap = GameManager.Instance.MaxChap;
        //����Ѿ����ڴ浵������͵�ǰ�½�����Ǹ����󣬱���������Ľ���
        if (File.Exists(Application.persistentDataPath + "/CaveBride.save"))
        {
            BinaryFormatter bf_1 = new BinaryFormatter();
            FileStream file_1 = File.Open(Application.persistentDataPath + "/CaveBride.save", FileMode.Open);
            Save save_1 = (Save)bf_1.Deserialize(file_1);
            file_1.Close();
            //���浵�е�������ȡ����
            int have_chap = save_1.s_chap;
            //������������Ϸ���в���
            m_chap = have_chap > m_chap ? have_chap : m_chap;
        }
        //�����´浵����
        Save save = new Save();
        save.s_chap = m_chap;
        //Debug.Log("new save " + save.s_chap);
        //���ô���·��������
        string Save_Path = Application.persistentDataPath + "/CaveBride.save";
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Save_Path);
        bf.Serialize(file, save);
        file.Close();
    }
    //��ȡ�浵
    public int LoadGame()
    {
        //����Ƿ���ڴ浵�����������ȡ�浵
        if (File.Exists(Application.persistentDataPath + "/CaveBride.save"))
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/CaveBride.save", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();
            //���浵�е�������ȡ����
            m_chap = save.s_chap;
            //������������Ϸ���в���
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
        //����������һ��ʱ��
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
