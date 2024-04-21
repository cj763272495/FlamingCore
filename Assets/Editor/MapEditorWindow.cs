using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MapEditorWindow : EditorWindow { 
    public Vector3 curPos;
    private Vector3 lastPos;
    string path_front = "Assets/Resources/Prefab/Env/";
    private int instanceNum;
    private Vector3 instateDir;

    //private GameObject selectedObject = null;
    private string[] prefabNames;

    private string[] dir ={
        "X+",
        "X-",
        "Y+",
        "Z+",
        "Z-"
    };// 预制体名称数组

    private int selectedPrefabIndex = 0; // 选中的预制体索引
    private int selectedDirIndex = 0; // 选中的方向索引

    // 调用这个静态方法来获取当前窗口的实例并显示它
    [MenuItem("Window/MapMaker")]
    public static void ShowWindow() {
        GetWindow<MapEditorWindow>("MapMaker"); 
    }
    private void OnEnable() {
        curPos = new Vector3(0,1,0);//默认位置
        lastPos = curPos;
        instanceNum = 0;
        prefabNames = new string[] {
            "CubeEdge.prefab",
            "CubeEdge2Angle.prefab"
        };// 预制体名称数组
    }


    void OnGUI() {
        EditorGUILayout.TextField("当前游标位置：", "("+curPos.x+"," + curPos.y + "," + curPos.z + "," + ")");
        selectedPrefabIndex = EditorGUILayout.Popup("选择预制体", selectedPrefabIndex, prefabNames);

        selectedDirIndex = EditorGUILayout.Popup("选择生成方向", selectedDirIndex, dir);
        switch (selectedDirIndex) {
            case 0:
                instateDir = Vector3.right;
                break;
            case 1:
                instateDir = Vector3.left;
                break;
            case 2: 
                instateDir = Vector3.up;
                break;
            case 3:
                instateDir = Vector3.forward;
                break;
            case 4:
                instateDir = Vector3.back;
                break; 
            default:
                return;
        }

        // 添加一个按钮，点击时生成预制体实例
        if (GUILayout.Button("生成选中的预制体")) {
            if (HasSelectGameObject()) {
                curPos = Selection.activeGameObject.transform.position;
            }
            while (!IsPositionClear(curPos)) {
                curPos += instateDir;
            }
            GameObject go = InstantiateSelectedPrefab(path_front+prefabNames[selectedPrefabIndex], curPos);
            if (go != null) {
                Selection.activeGameObject = go;
                lastPos = curPos;
            }
        }

    }

    private bool HasSelectGameObject() {
        if (Selection.activeGameObject != null) {
            EditorGUILayout.ObjectField("当前选中的对象", Selection.activeGameObject, typeof(GameObject), true);
            return true;
        } else {
            EditorGUILayout.HelpBox("没有选中任何对象。", MessageType.Info);
            return false;
        }
    }
    private void SelectNewObject() { 
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects) {
            if (obj.transform.parent == null) {
                Selection.activeGameObject = obj;
                break;
            }
        }
    }
    private void OnSelectionChange() {//监听当前选中物体变化
        if (Selection.activeGameObject) {
            curPos = Selection.activeGameObject.transform.position;
        }
    }

    private GameObject InstantiateSelectedPrefab(string path, Vector3 pos) {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        if (prefab != null) {
            GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            instance.name = prefab.name +"_" +instanceNum.ToString();
            instance.transform.position = pos;
            instance.isStatic = true;
            instanceNum++;
            return instance;
        } else {
            Debug.LogError("预制体未找到，请检查路径是否正确。");
            return null;
        }
    
    }

    private bool IsPositionClear(Vector3 position) { 
        Collider[] colliders = Physics.OverlapSphere(position, 0.4f); // 0.1f是检测半径
        return colliders.Length == 0;
    }
}
