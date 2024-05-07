/****************************************************
    �ļ���Constants.cs
	���ܣ���������
*****************************************************/

using UnityEngine;
using System.IO;

public class Constants {
    //С���ٶ�
    public const float PlayerSpeed = 12;
    public const float RotateSpeed = 2000;

    //���guideline����
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

    public static string ConfigPath = Path.Combine(Application.persistentDataPath, "config.json");// �û���������
    public static string PlayerDataPath = Path.Combine(Application.persistentDataPath, "playerData.json");//�û���������

    public const int ReviceCost = 80; //�������Ľ��

    public static int[] skinPrice = new int[] { 0, 100,200,300 };//Ƥ���۸�
    public static int[] trailPrice = new int[] { 0, 100,300,500 };//��β�۸�
}

public enum PageType {//��ҳҳ������
    Level,
    Store,
    Settings
}
