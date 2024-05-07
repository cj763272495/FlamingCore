/****************************************************
    文件：Constants.cs
	功能：常量配置
*****************************************************/

using UnityEngine;
using System.IO;

public class Constants {
    //小球速度
    public const float PlayerSpeed = 12;
    public const float RotateSpeed = 2000;

    //最大guideline长度
    public const float MaxGuideLineLen = 7;

    public const string ButtonClip = "AudioClip/Glitch_High_01";
    public const string EarnMoneyClip = "AudioClip/Glitch_High_01";

    public const string HitWallClip = "AudioClip/wall";
    public const string HitWall2Clip = "AudioClip/hitwall01";
    public const string HitEnenmyClip = "AudioClip/hit2";
    public const string DeadClip = "AudioClip/Dead"; 

    public const string BGMainCity = "AudioClip/Tiero - Action Dubstep";
    public const string BGGame = "AudioClip/Tiero - Dubstep A";


    public const string MapCfg = "Conf/map";

    public static string ConfigPath = Path.Combine(Application.persistentDataPath, "config.json");// 用户本地设置
    public static string PlayerDataPath = Path.Combine(Application.persistentDataPath, "playerData.json");//用户本地数据

    public const int ReviceCost = 80; //重生消耗金额

    public static int[] skinPrice = new int[] { 0, 100,200,300 };//皮肤价格
    public static int[] trailPrice = new int[] { 0, 100,300,500 };//拖尾价格
}

public enum PageType {//主页页面类型
    Level,
    Store,
    Settings
}
