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

    private Dictionary<string, GameObject> goDic = new Dictionary<string, GameObject>();
    public GameObject LoadPrefab(string path, bool cache = false) {
        GameObject prefab = null;
        if (!goDic.TryGetValue(path, out prefab)) {
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

    private Dictionary<string, AudioClip> adDic = new Dictionary<string, AudioClip>();
    public AudioClip LoadAudio(string path, bool cache = false) {
        AudioClip au = null;
        if (!adDic.TryGetValue(path, out au)) {
            au = Resources.Load<AudioClip>(path);
            if (cache) {
                adDic.Add(path, au);
            }
        }
        return au;
    }

    private Dictionary<string, Sprite> spDic = new Dictionary<string, Sprite>();
    public Sprite LoadSprite(string path, bool cache = false) {
        Sprite sp = null;
        if (!spDic.TryGetValue(path, out sp)) {
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
    private Dictionary<string, LevelData> mapCfgDataDic = new Dictionary<string, LevelData>();
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
        LevelData data;
        if (mapCfgDataDic.TryGetValue("level"+id, out data)) {
            return data;
        }
        return null;
    }
    #endregion


    #region PlayerData
    PlayerDataBase playerDataDic = new PlayerDataBase();
    public void  LoadPlayerData() {
        string filePath = Constants.PlayerDataPath;
        if (File.Exists(filePath)) {
            string json = File.ReadAllText(filePath);
            playerDataDic = JsonConvert.DeserializeObject<PlayerDataBase>(json);
        } else {
            PlayerData pd = new PlayerData {
                coin = 0,
                skin = new int[] {1},
                trail = new int[] {1},
                energy = 5,
                current_wave = 1
            };
            SavePlayerData("11", pd);
        }
    }
    public PlayerData GetPlayerData(string Playerid) {
        PlayerData data;
        if (playerDataDic.TryGetValue(Playerid, out data)) {
            return data;
        }
        return null;
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
