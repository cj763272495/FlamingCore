using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class ResSvc : MonoBehaviour {
    public static ResSvc Instance = null;
    public TextAsset jsonConfig;

    public void InitSvc() {
        Instance = this;
        LoadMap();
        LoadPlayerData();
    }

    private Action prgCB = null;
    public void AsyncLoadScene(string sceneName, Action loaded) {
        GameRoot.Instance.loadingWnd.SetWndState(); 
        AsyncOperation sceneAsync = SceneManager.LoadSceneAsync(sceneName);
        prgCB = () => {
            float val = sceneAsync.progress;
            GameRoot.Instance.loadingWnd.SetProgress(val);
            if (val == 1) {
                //LoginSys.Instance.OpenLoginWnd();
                if (loaded != null) {
                    loaded();
                }
                prgCB = null;
                sceneAsync = null;
                GameRoot.Instance.loadingWnd.SetWndState(false);
            }
        };

    }

    private readonly Dictionary<string, GameObject> goDic = new();
    public GameObject LoadPrefab(string path, bool cache = false) {
        if (!goDic.TryGetValue(path, out GameObject prefab)) {
            prefab = Resources.Load<GameObject>(path);
            if (cache) {
                goDic.Add(path, prefab);
            }
        }
        GameObject go = null;
        if (prefab != null) {
            go = Instantiate(prefab);
        }
        return go;
    }

    private readonly Dictionary<string, AudioClip> adDic = new();
    public AudioClip LoadAudio(string path, bool cache = false) { 
        if (!adDic.TryGetValue(path, out AudioClip au)) {
            au = Resources.Load<AudioClip>(path);
            if (cache) {
                adDic.Add(path, au);
            }
        }
        return au;
    }

    private readonly Dictionary<string, Sprite> spDic = new();
    public Sprite LoadSprite(string path, bool cache = false) { 
        if (!spDic.TryGetValue(path, out Sprite sp)) {
            sp = Resources.Load<Sprite>(path);
            if (cache) {
                spDic.Add(path, sp);
            }
        }
        return sp;
    }

    #region SetCfg
    /// <summary>
    /// 游戏设置配置文件
    /// </summary>
    private GameSettings gameSetting;

    private void Update() {
        if (prgCB != null) {
            prgCB();
        }
    }
    public GameSettings LoadConf() {
        string filePath = Constants.ConfigPath;
        if (File.Exists(filePath)) {
            string textAsset = File.ReadAllText(filePath);
            gameSetting = JsonConvert.DeserializeObject<GameSettings>(textAsset);
        } else {
            gameSetting = new GameSettings { bgAudio = true, showJoyStick = true };
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
        File.WriteAllText(Constants.ConfigPath, JsonUtility.ToJson(gameSetting));
    } 
    #endregion

    #region MapCfg 
    /// <summary>
    /// 关卡文件
    /// </summary>
    private Dictionary<string, LevelData> mapCfgDataDic = new();
    public void LoadMap() {
        try {
            TextAsset textAsset = Resources.Load<TextAsset>(Constants.MapCfg);
            string jsonText = null;
            if (textAsset != null) {
                jsonText = textAsset.text; 
            } else {
                Debug.LogError("Failed to load resource: " + Constants.MapCfg);
            }
            LevelConfig levelConfig = JsonConvert.DeserializeObject<LevelConfig>(jsonText);
            if (levelConfig != null) {
                mapCfgDataDic = levelConfig;
            } else {
                Debug.LogError("Failed to parse JSON data.");
            }
        } catch (UnityException ex) {
            // 捕获并处理异常
            Debug.LogError("Exception occurred while loading resource: " + ex.Message);
        } catch (Exception ex) {
            // 捕获其他可能的异常
            Debug.LogError("An error occurred: " + ex.Message);
        }
    }
    public LevelData GetMapCfgData(string id) { 
        if (mapCfgDataDic.TryGetValue("level"+id, out LevelData data)) {
            return data;
        }
        return null;
    }
    #endregion


    #region PlayerData
    PlayerDataBase playerDataDic = new();
    public void  LoadPlayerData() {
        string filePath = Constants.PlayerDataPath;
        if (File.Exists(filePath)) {
            string json = File.ReadAllText(filePath);
            playerDataDic = JsonConvert.DeserializeObject<PlayerDataBase>(json);
        } else {
            PlayerData pd = new() { 
                coin = 0,
                skin = new List<int> { 0 },
                trail = new List<int> { 0 },
                energy = 5,
                current_wave = 1,
                cur_skin = 0,
                cur_trail = 0
            };
            SavePlayerData("11", pd);
        }
    }
    public PlayerData GetPlayerData(string Playerid) { 
        if (playerDataDic.TryGetValue(Playerid, out PlayerData data)) {
            return data;
        } else {
            Debug.Log("未获取到玩家数据");
            return null;
        }
    }

    public void SavePlayerData(string playerID, PlayerData pd) {
        if (playerDataDic.ContainsKey(playerID)) {
            playerDataDic[playerID] = pd;
        } else {
            playerDataDic.Add(playerID, pd);
        }
        string json = JsonConvert.SerializeObject(playerDataDic, Formatting.Indented);
        WriteJsonToFile(json, Constants.PlayerDataPath);
    }
    #endregion

    void WriteJsonToFile(string json, string filePath) {
        string directoryPath = Path.GetDirectoryName(filePath); // 确保文件路径的目录存在
        if (!string.IsNullOrEmpty(directoryPath) && !Directory.Exists(directoryPath)) {
            Directory.CreateDirectory(directoryPath);
        }
        File.WriteAllText(filePath, json);
    }
}
