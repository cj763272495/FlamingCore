using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class GameSettings {
    public bool bgAudio;
    public bool showJoyStick;
}
public class Vector3Data {
    public float x { get; set; }
    public float y { get; set; }
    public float z { get; set; }
}

public class LevelData {
    public Vector3Data playerStartPosition { get; set; }
    public Vector3Data cameraOffset { get; set; }
    public Vector3Data cameraRotation { get; set; }
    public int enemyNum { get; set; }
}

public class LevelConfig: Dictionary<string, LevelData> { 
}

[System.Serializable]
public class PlayerData {
    public int coin;
    public int[] skin;
    public int[] trail;
    public int energy;
    public int current_wave;
    public int cur_skin;
    public int cur_trail;
}

public class PlayerDataBase: Dictionary<string, PlayerData> { 
}