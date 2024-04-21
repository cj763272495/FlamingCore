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
    };// Ԥ������������

    private int selectedPrefabIndex = 0; // ѡ�е�Ԥ��������
    private int selectedDirIndex = 0; // ѡ�еķ�������

    // ���������̬��������ȡ��ǰ���ڵ�ʵ������ʾ��
    [MenuItem("Window/MapMaker")]
    public static void ShowWindow() {
        GetWindow<MapEditorWindow>("MapMaker"); 
    }
    private void OnEnable() {
        curPos = new Vector3(0,1,0);//Ĭ��λ��
        lastPos = curPos;
        instanceNum = 0;
        prefabNames = new string[] {
            "CubeEdge.prefab",
            "CubeEdge2Angle.prefab"
        };// Ԥ������������
    }


    void OnGUI() {
        EditorGUILayout.TextField("��ǰ�α�λ�ã�", "("+curPos.x+"," + curPos.y + "," + curPos.z + "," + ")");
        selectedPrefabIndex = EditorGUILayout.Popup("ѡ��Ԥ����", selectedPrefabIndex, prefabNames);

        selectedDirIndex = EditorGUILayout.Popup("ѡ�����ɷ���", selectedDirIndex, dir);
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

        // ���һ����ť�����ʱ����Ԥ����ʵ��
        if (GUILayout.Button("����ѡ�е�Ԥ����")) {
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
            EditorGUILayout.ObjectField("��ǰѡ�еĶ���", Selection.activeGameObject, typeof(GameObject), true);
            return true;
        } else {
            EditorGUILayout.HelpBox("û��ѡ���κζ���", MessageType.Info);
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
    private void OnSelectionChange() {//������ǰѡ������仯
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
            Debug.LogError("Ԥ����δ�ҵ�������·���Ƿ���ȷ��");
            return null;
        }
    
    }

    private bool IsPositionClear(Vector3 position) { 
        Collider[] colliders = Physics.OverlapSphere(position, 0.4f); // 0.1f�Ǽ��뾶
        return colliders.Length == 0;
    }
}
