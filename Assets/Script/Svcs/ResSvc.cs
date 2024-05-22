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
    }

    private Action prgCB = null;
    public void AsyncLoadScene(string sceneName, Action loaded) {
        UIManager uIManager = UIManager.Instance;
        uIManager.FadeIn().onComplete += () => {  
            AsyncOperation sceneAsync = SceneManager.LoadSceneAsync(sceneName);
            prgCB = () => {
                float val = sceneAsync.progress;
                //uIManager.loadingWnd.SetProgress(val); 
                if(val == 1) {
                    //LoginSys.Instance.OpenLoginWnd();
                    if(loaded != null) {
                        loaded();
                    }
                    prgCB = null;
                    sceneAsync = null;
                    //uIManager.loadingWnd.SetWndState(false);
                    //DG.Tweening.Sequence seq = DOTween.Sequence();
                    //seq.AppendInterval(1f); // 等待1秒
                    //seq.AppendCallback(() => uIManager.FadeOut());
                    //seq.SetUpdate(UpdateType.Normal,true);
                    //seq.Play();
                    uIManager.FadeOut();
                }
            };
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
        return prefab != null ? Instantiate(prefab) : null;
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
        TextAsset textAsset = Resources.Load<TextAsset>(Constants.MapCfg);
        if(textAsset != null) {
            LevelConfig levelConfig = JsonConvert.DeserializeObject<LevelConfig>(textAsset.text);
            if(levelConfig != null) {
                mapCfgDataDic = levelConfig;
            } else {
                Debug.LogError("Failed to parse JSON data.");
            }
        } else {
            Debug.LogError("Failed to load resource: " + Constants.MapCfg);
        }
    }
    public LevelData GetMapCfgData(string waveName) { 
        if (mapCfgDataDic.TryGetValue(waveName, out LevelData data)) {
            return data;
        }
        return null;
    }
    #endregion


    #region PlayerData
    public void LoadPlayerData( out PlayerDataDic playerDataDic) {
        string filePath = Constants.PlayerDataPath;
        if(File.Exists(filePath)) {
            string json = File.ReadAllText(filePath);
            playerDataDic = JsonConvert.DeserializeObject<PlayerDataDic>(json);
        } else {
            playerDataDic = null;
        }
    }
    

    public bool SavePlayerData(PlayerDataDic playerDataDic) {
        try {
            string json = JsonConvert.SerializeObject(playerDataDic,Formatting.Indented);
            WriteJsonToFile(json,Constants.PlayerDataPath);
            return true;
        } catch(Exception ex) {
            Debug.LogError("An error occurred while saving player data: " + ex.Message);
            return false;
        }
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
