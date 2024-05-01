/****************************************************
    文件：Constants.cs
	功能：常量配置
*****************************************************/

using UnityEngine;
using System.IO;

public class Constants {

    //public const float BulletTimeVelocity = 0.5f;

    //正常速度
    public const float PlayerNormalSpeed = 15;
    public const float NormalRotateSpeed = 2000;

    //时空减速速度
    public const float SlowDownSpeed = 0.8f;
    public const float SlowRotateSpeed = 4;

    //最大guideline长度
    public const float MaxGuideLineLen = 7;

    public const string ButtonClip = "AudioClip/Glitch_High_01";
    public const string EarnMoneyClip = "AudioClip/Glitch_High_01";

    public const string HitWallClip = "AudioClip/wall";
    public const string HitWallSlowlyClip = "AudioClip/hitwall01";
    public const string HitEnenmyClip = "AudioClip/hit2";
    public const string DeadClip = "AudioClip/Dead";

    public const string BGMainCity = "AudioClip/Tiero - Action Dubstep";
    public const string BGGame = "AudioClip/Tiero - Dubstep A";


    public const string MapCfg = "Conf/map";

    public static string ConfigPath = Path.Combine(Application.persistentDataPath, "config.json");
    public static string PlayerDataPath = Path.Combine(Application.persistentDataPath, "playerData.json");

    public const int ReviceCost = 80;
    public static int[] skinPrice = new int[] { 0, 100,200,300 };
    public static int[] trailPrice = new int[] { 0, 100,300,500 };
}

public enum PageType {
    Level,
    Store,
    Settings
}
