using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour {
    public static PoolManager Instance { get; private set; }
    //ͨ�����������ҵ���Ӧ�ĳ���
    private Dictionary<Object, Queue<Object>> poolsDict;

    private void Awake() {
        Instance = this;
        poolsDict = new Dictionary<Object, Queue<Object>>();
    }
    /// <summary>
    /// ��ʼ��ĳһ�������
    /// </summary>
    /// <param name="prefab">ָ��Ԥ���������</param>
    /// <param name="size">��ǰ��Ӧ���͵Ķ���صĳ���</param>
    public void InitPool(Object prefab, int size) {
        if (poolsDict.ContainsKey(prefab)) {
            return;
        }
        Queue<Object> queue = new Queue<Object>();
        for (int i = 0; i < size; i++) {
            Object go = Instantiate(prefab);
            CreateGameObjectAndSetActive(go, false);
            queue.Enqueue(go);
        }
        poolsDict[prefab] = queue;
    }

    private void CreateGameObjectAndSetActive(Object obj, bool active) {
        GameObject itemGo = null;
        if (obj is Component) {
            Component component = obj as Component;
            itemGo = component.gameObject;
        } else {
            itemGo = obj as GameObject;
        }
        itemGo.transform.SetParent(transform);
        itemGo.SetActive(active);
    }

    public T GetInstance<T>(Object prefab) where T : Object {
        Queue<Object> queue;
        if (poolsDict.TryGetValue(prefab, out queue)) {
            Object obj;
            if (queue.Count > 0) {
                obj = queue.Dequeue();
            } else {
                obj = Instantiate(prefab);
            }
            CreateGameObjectAndSetActive(obj, true);
            queue.Enqueue(obj);
            return obj as T;
        }
        Debug.LogError("��û�е�ǰ���͵���Դ�ر�ʵ����");
        return null;
    }
}
