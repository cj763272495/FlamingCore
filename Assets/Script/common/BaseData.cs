using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameSettings {
    public bool bgAudio;
    public bool showJoyStick;
}
public class Vector3Data {
    public float X { get; set; }
    public float Y { get; set; }
    public float Z { get; set; }
}

public class LevelData {
    public string RobotName { get; set; }
    public Vector3Data PlayerStartPosition { get; set; }
    public Vector3Data CameraOffset { get; set; }
    public Vector3Data CameraRotation { get; set; }
    public int CamFOV { get; set; }
    public int EnemyNum { get; set; }
}

public class LevelConfig: Dictionary<string, LevelData> { 
}

[System.Serializable]
public class PlayerData {
    public int coin;
    public List<int> skin;
    public List<int> trail;
    public int energy;
    public int max_unLock_wave;
    public int cur_skin;
    public int cur_trail;
}

public class PlayerDataDic: Dictionary<string, PlayerData> { 
}