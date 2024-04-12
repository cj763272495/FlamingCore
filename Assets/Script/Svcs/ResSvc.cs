using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

using UnityEngine.SceneManagement;

public class ResSvc : MonoBehaviour {
    public static ResSvc Instance = null;

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
}
