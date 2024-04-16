using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEngine.SceneManagement;


[System.Serializable]
public class GameSettings {
    public bool bgAudio;
    public bool showJoyStick;
}

public class ResSvc : MonoBehaviour {
    public static ResSvc Instance = null;
    public TextAsset jsonConfig;

    public void InitSvc() {
        Instance = this;
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

    private GameSettings gameSetting;

    private void Update() {
        if (prgCB != null) {
            prgCB();
        }
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




    public GameSettings LoadConf() {
        gameSetting = JsonUtility.FromJson<GameSettings>(jsonConfig.text);
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
}
