/****************************************************
    �ļ���Constants.cs
	���ܣ���������
*****************************************************/

using UnityEngine;
using System.IO;

public class Constants{

    //public const float BulletTimeVelocity = 0.5f;

    //�����ٶ�
    public const float PlayerNormalSpeed = 15;
    public const float NormalRotateSpeed = 2000;

    //ʱ�ռ����ٶ�
    public const float SlowDownSpeed = 0.8f; 
    public const float SlowRotateSpeed = 4;


    //���guideline����
    public const float MaxGuideLineLen = 6;
    //��������
    public const float MaxReflections = 5;

    public const string ButtonClip = "AudioClip/Glitch_High_01";
    public const string EarnMoneyClip = "AudioClip/Glitch_High_01";

    public const string HitWallClip = "AudioClip/wall";
    public const string HitWallSlowlyClip = "AudioClip/hitwall01";
    public const string HitEnenmyClip = "AudioClip/hit2";
    public const string DeadClip = "AudioClip/Dead";

    public const string BG1 = "AudioClip/Tiero - Action Dubstep";
    public const string BG2 = "AudioClip/Tiero - Dubstep A";


    public const string MapCfg = "Conf/map";

    public static string ConfigPath = Path.Combine(Application.persistentDataPath, "config.json");
    public static string PlayerDataPath = Path.Combine(Application.persistentDataPath,"playerData.json");

    public const int ReviceCost = 80;
}

public enum PageType {
    Level,
    Store,
    Settings
}
