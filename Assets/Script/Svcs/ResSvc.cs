using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class ResSvc:MonoBehaviour {
    public static ResSvc Instance = null;
    public TextAsset jsonConfig;

    public void InitSvc() {
        Instance = this;
        LoadMap();
    }

    private Action prgCB = null;
    public void AsyncLoadScene(string sceneName,Action loaded) {
        UIManager uIManager = UIManager.Instance;
        uIManager.loadingWnd.StartLoading();
        AsyncOperation sceneAsync = SceneManager.LoadSceneAsync(sceneName,LoadSceneMode.Single); 
        prgCB = () => {
            float val = sceneAsync.progress; 
            if(val == 1) {
                if(loaded != null) {
                    loaded();
                }
                prgCB = null;
                uIManager.loadingWnd.LoadingEnd();
            }
        };
    }

    private void Update() {
        if(prgCB != null) {
            prgCB();
        }
    }

    private readonly Dictionary<string,GameObject> goDic = new();
    public GameObject LoadPrefab(string path,bool cache = false) {
        if(!goDic.TryGetValue(path,out GameObject prefab)) {
            prefab = Resources.Load<GameObject>(path);
            if(cache) {
                goDic.Add(path,prefab);
            }
        }
        return prefab != null ? Instantiate(prefab) : null;
    }

    private readonly Dictionary<string,AudioClip> adDic = new();
    public AudioClip LoadAudio(string path,bool cache = false) {
        if(!adDic.TryGetValue(path,out AudioClip au)) {
            au = Resources.Load<AudioClip>(path);
            if(cache) {
                adDic.Add(path,au);
            }
        }
        return au;
    }

    private readonly Dictionary<string,Sprite> spDic = new();
    public Sprite LoadSprite(string path,bool cache = false) {
        if(!spDic.TryGetValue(path,out Sprite sp)) {
            sp = Resources.Load<Sprite>(path);
            if(cache) {
                spDic.Add(path,sp);
            }
        }
        return sp;
    }

    #region SetCfg
    /// <summary>
    /// 游戏设置配置文件
    /// </summary>
    private GameSettings gameSetting;


    public GameSettings LoadConf() {
        string filePath = Constants.ConfigPath;
        if(File.Exists(filePath)) {
            string textAsset = File.ReadAllText(filePath);
            gameSetting = JsonConvert.DeserializeObject<GameSettings>(textAsset);
        } else {
            gameSetting = new GameSettings { bgAudio = true,showJoyStick = true };
            SaveConf();
        }
        return gameSetting;
    }
    public void ChangeBgAudioConf(bool isOn) {
        gameSetting.bgAudio = isOn;
        SaveConf();
    }

    public void ChangeShowJoyStickConf(bool isShow) {
        gameSetting.showJoyStick = isShow;
        SaveConf();
    }

    public void SaveConf() {
        File.WriteAllText(Constants.ConfigPath,JsonUtility.ToJson(gameSetting));
    }
    #endregion

    #region MapCfg 
    /// <summary>
    /// 关卡文件
    /// </summary>
    private Dictionary<string,LevelData> mapCfgDataDic = new();
    public void LoadMap() {
        TextAsset textAsset = Resources.Load<TextAsset>(Constants.MapCfg);
        if(textAsset != null) {
            LevelConfig levelConfig = JsonConvert.DeserializeObject<LevelConfig>(textAsset.text);
            if(levelConfig != null) {
                mapCfgDataDic = levelConfig;
            } else {
                ToolClass.PrintLog("Failed to parse JSON data.");
            }
        } else {
            ToolClass.PrintLog("Failed to load resource: " + Constants.MapCfg);
        }
    }
    public LevelData GetMapCfgData(string waveName) {
        if(mapCfgDataDic.TryGetValue(waveName,out LevelData data)) {
            return data;
        }
        return null;
    }
    #endregion


    #region PlayerData
    private PlayerDataDic playerDataDic = new();
    public PlayerDataDic LoadPlayerData() {
        string filePath = Constants.PlayerDataPath;
        if(File.Exists(filePath)) {
            string json = File.ReadAllText(filePath);
            playerDataDic = JsonConvert.DeserializeObject<PlayerDataDic>(json);
        }
        return playerDataDic;
    }


    public bool SavePlayerData(PlayerDataDic playerDataDic) {
        try {
            string json = JsonConvert.SerializeObject(playerDataDic,Formatting.Indented);
            WriteJsonToFile(json,Constants.PlayerDataPath);
            return true;
        } catch(Exception ex) {
            ToolClass.PrintLog("An error occurred while saving player data: " + ex.Message);
            return false;
        }
    }
    #endregion

    void WriteJsonToFile(string json,string filePath) {
        string directoryPath = Path.GetDirectoryName(filePath); // 确保文件路径的目录存在
        if(!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath)) {
            Directory.CreateDirectory(directoryPath);
        }
        File.WriteAllText(filePath,json);
    }
}
