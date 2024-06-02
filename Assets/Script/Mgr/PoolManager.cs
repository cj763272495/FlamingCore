using System.Collections.Generic;
using UnityEngine;

public class PoolManager:MonoBehaviour {
    public static PoolManager Instance { get; private set; }
    //通过传入类型找到对应的池子
    private Dictionary<string,Queue<Object>> poolsDict;

    private void Awake() {
        Instance = this;
        poolsDict = new();
    }
    /// <summary>
    /// 初始化某一个对象池
    /// </summary>
    /// <param name="prefab">指定预制体的类型</param>
    /// <param name="size">当前对应类型的对象池的长度</param>
    public void InitPool(Object prefab,int size,Transform parent = null) {
        if(!prefab || poolsDict.ContainsKey(prefab.name)) {
            return;
        }
        Queue<Object> queue = new();
        for(int i = 0; i < size; i++) {
            Object go = Instantiate(prefab);
            GameObject itemGo = go as GameObject ?? (go as Component).gameObject;
            itemGo.name = prefab.name;
            CreateGameObjectAndSetActive(go,false,parent);
            queue.Enqueue(go);
        }
        poolsDict[prefab.name] = queue;
    }

    private void CreateGameObjectAndSetActive(Object obj,bool active,Transform parent = null,bool changeParent = true) {
        GameObject itemGo = obj as GameObject ?? (obj as Component).gameObject;
        if(changeParent) {
            itemGo.transform.SetParent(parent ?? transform);
        }
        itemGo.SetActive(active);
    }

    public class PoolableObject:MonoBehaviour {
        public Object OriginalPrefab { get; set; }
    }


    public T GetInstance<T>(Object prefab,Transform parent = null) where T : Object {
        if(!prefab) {
            return null;
        }
        if(poolsDict.TryGetValue(prefab.name,out Queue<Object> queue)) {
            Object obj;
            GameObject itemGo;
            while(queue.Count > 0) {
                obj = queue.Dequeue();
                if(obj == null) {
                    continue;
                }
                itemGo = obj as GameObject ?? (obj as Component).gameObject;
                CreateGameObjectAndSetActive(itemGo,true,parent,true);
                return itemGo as T;
            }

            // 如果队列为空，则实例化一个新的对象
            Object newObj = Instantiate(prefab);
            itemGo = newObj as GameObject ?? (newObj as Component).gameObject;
            itemGo.name = prefab.name;
            CreateGameObjectAndSetActive(itemGo,true,parent,true);
            return itemGo as T;
        }
        Debug.LogError("还没有当前类型的资源池被实例化");
        return null;
    }

    public void ReturnToPool(Object obj) {
        if(obj == null) {
            return;
        }
        if(poolsDict.TryGetValue(obj.name,out Queue<Object> queue)) {
            CreateGameObjectAndSetActive(obj,false,null,false);
            queue.Enqueue(obj);
        } else {
            Debug.LogError("还没有当前类型的资源池被实例化");
        }
    }
}
