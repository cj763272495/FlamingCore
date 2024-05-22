/****************************************************
    文件：Constants.cs
	功能：常量配置
*****************************************************/

using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class Constants {
    //小球速度
    public const float PlayerSpeed = 12;
    public const float RotateSpeed = 100;

    //获得加速时的速度
    public const float OverloadSpeed = 20;
    public const float overloadDuration = 2.0f;
    public static readonly Vector3 DefaultCamOffset = new Vector3(0,27,-26);
    //超载模式fov
    public const float OverloadFov = 45;

    //最大guideline长度
    public const float MaxGuideLineLen = 7;

    public const string ButtonClip = "AudioClip/Glitch_High_01";
    public const string EarnMoneyClip = "AudioClip/Glitch_High_01";

    public const string HitWallClip = "AudioClip/wall";
    public const string HitWall2Clip = "AudioClip/hitwall01";
    public const string HitEnenmyClip = "AudioClip/Click_Heavy_00";
    public const string DeadClip = "AudioClip/Dead";

    public const string BGMainCity = "AudioClip/Tiero - Action Dubstep";
    public const string BGGame = "AudioClip/Tiero - Dubstep A";


    public const string MapCfg = "Conf/map";

    public static string ConfigPath = Path.Combine(Application.persistentDataPath,"config.json");// 用户本地设置
    public static string PlayerDataPath = Path.Combine(Application.persistentDataPath,"playerData.json");//用户本地数据

    public const int ReviceCost = 80; //重生消耗金额

    public static int[] trailPrice = new int[] { 0,100,300,500,700,900 };//拖尾价格

    public const string DashTargetPosSprite = "Sprite/Huan";

    public static Dictionary<CoreType,CoreInfo> CoresInfo = new Dictionary<CoreType,CoreInfo> {
        { CoreType.Core_Normal, new CoreInfo { descript = "让你的子弹时间变得更慢", price = 0 } },
        { CoreType.Core_Immune_Explosion, new CoreInfo { descript = "免疫爆炸伤害", price = 100 } },
        { CoreType.Core_Not_Rebound, new CoreInfo { descript = "摧毁敌人时不会产生反弹", price = 200 } },
        { CoreType.Core_Clear_Bullet, new CoreInfo { descript = "摧毁敌人时，清除周围子弹", price = 300 } },
        { CoreType.Core_Shield, new CoreInfo { descript = "撞击会产生护盾，进行操作会让这个护盾消失", price = 500 } },
        { CoreType.Core_Dash, new CoreInfo { descript = "以闪现的方式进行移动", price = 800 } }
    };
     
}

public enum PageType {//主页页面类型
    Level,
    Store,
    Settings
}

public enum CoreType {//核心类型
    Core_Normal,
    Core_Immune_Explosion,
    Core_Not_Rebound,
    Core_Clear_Bullet,
    Core_Shield,
    Core_Dash
}
public struct CoreInfo {//核心信息
    public string descript;
    public int price;
}
