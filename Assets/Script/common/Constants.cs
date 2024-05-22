/****************************************************
    �ļ���Constants.cs
	���ܣ���������
*****************************************************/

using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class Constants {
    //С���ٶ�
    public const float PlayerSpeed = 12;
    public const float RotateSpeed = 100;

    //��ü���ʱ���ٶ�
    public const float OverloadSpeed = 20;
    public const float overloadDuration = 2.0f;
    public static readonly Vector3 DefaultCamOffset = new Vector3(0,27,-26);
    //����ģʽfov
    public const float OverloadFov = 45;

    //���guideline����
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

    public static string ConfigPath = Path.Combine(Application.persistentDataPath,"config.json");// �û���������
    public static string PlayerDataPath = Path.Combine(Application.persistentDataPath,"playerData.json");//�û���������

    public const int ReviceCost = 80; //�������Ľ��

    public static int[] trailPrice = new int[] { 0,100,300,500,700,900 };//��β�۸�

    public const string DashTargetPosSprite = "Sprite/Huan";

    public static Dictionary<CoreType,CoreInfo> CoresInfo = new Dictionary<CoreType,CoreInfo> {
        { CoreType.Core_Normal, new CoreInfo { descript = "������ӵ�ʱ���ø���", price = 0 } },
        { CoreType.Core_Immune_Explosion, new CoreInfo { descript = "���߱�ը�˺�", price = 100 } },
        { CoreType.Core_Not_Rebound, new CoreInfo { descript = "�ݻٵ���ʱ�����������", price = 200 } },
        { CoreType.Core_Clear_Bullet, new CoreInfo { descript = "�ݻٵ���ʱ�������Χ�ӵ�", price = 300 } },
        { CoreType.Core_Shield, new CoreInfo { descript = "ײ����������ܣ����в����������������ʧ", price = 500 } },
        { CoreType.Core_Dash, new CoreInfo { descript = "�����ֵķ�ʽ�����ƶ�", price = 800 } }
    };
     
}

public enum PageType {//��ҳҳ������
    Level,
    Store,
    Settings
}

public enum CoreType {//��������
    Core_Normal,
    Core_Immune_Explosion,
    Core_Not_Rebound,
    Core_Clear_Bullet,
    Core_Shield,
    Core_Dash
}
public struct CoreInfo {//������Ϣ
    public string descript;
    public int price;
}
