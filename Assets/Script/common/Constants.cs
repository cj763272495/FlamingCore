/****************************************************
    文件：Constants.cs
	功能：常量配置
*****************************************************/

using UnityEngine;

public class Constants{

    //public const float BulletTimeVelocity = 0.5f;

    //正常速度
    public const float PlayerNormalSpeed = 15;
    public const float NormalRotateSpeed = 2000;

    //时空减速速度
    public const float SlowDownSpeed = 0.8f; 
    public const float SlowRotateSpeed = 4;


    //最大guideline长度
    public const float MaxGuideLineLen = 6;
    //最大反射次数
    public const float MaxReflections = 5;

    public const string ButtonClip = "AudioClip/Glitch_High_01";
    public const string EarnMoneyClip = "AudioClip/Glitch_High_01";

    public const string HitWallClip = "AudioClip/wall";
    public const string HitWallSlowlyClip = "AudioClip/hitwall01";
    public const string HitEnenmyClip = "AudioClip/hit2";
    public const string DeadClip = "AudioClip/Dead";

    public const string BG1 = "AudioClip/Tiero - Action Dubstep";
    public const string BG2 = "AudioClip/Tiero - Dubstep A";

    public const string ConfigPath = "Assets/Conf/config.json";
    
}

public enum PageType {
    Level,
    Store,
    Settings
}
