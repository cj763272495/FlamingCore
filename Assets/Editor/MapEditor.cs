using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EditorWindow:UnityEditor.EditorWindow {
    private int gridWidth = 20;
    private int gridHeight = 20; 

    private int instanceNum;
    readonly string path_front = "Assets/Resources/Prefab/Env/";  
    private string[] prefabNames;
    private int selectedPrefabIndex = 0; // 选中的预制体索引

    private Dictionary<Vector2Int,GameObject> selectedCells = new Dictionary<Vector2Int,GameObject>();
    private List<Vector2Int> combiningCells = new List<Vector2Int>(); // 添加这行代码来存储被组合的网格 
    private List<Vector2Int> completedCells = new List<Vector2Int>();
    private List<GameObject> completeGo = new List<GameObject>();

    Rect toolbarRect;
    private bool isCombining = false;

    [MenuItem("Window/Map Editor")]
    public static void ShowWindow() {
        GetWindow<EditorWindow>("Map Editor");
    }

    private void OnEnable() {
        instanceNum = 0;
        prefabNames = new string[] {
            "CubeEdge.prefab",
            "CubeEdge2Angle.prefab"
        };// 预制体名称数组
    }
    void OnGUI() { 
        DrawGrid();

        // Toolbar 
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        GUILayout.Label("Width:");
        gridWidth = EditorGUILayout.IntField(gridWidth,GUILayout.Width(50));
        GUILayout.Space(20);
        GUILayout.Label("Height:");
        gridHeight = EditorGUILayout.IntField(gridHeight,GUILayout.Width(50));
        GUILayout.Space(20);

        selectedPrefabIndex = EditorGUILayout.Popup("选择预制体",selectedPrefabIndex,prefabNames);
        GUILayout.Space(20);
        if(GUILayout.Button("AddPrefab",GUILayout.Width(80))) {
            isCombining = false;
        }
        GUILayout.Space(20);
        Color oldColor = GUI.skin.button.normal.textColor;
        GUI.skin.button.normal.textColor = Color.red;
        if(GUILayout.Button("ClearAll",GUILayout.Width(80))) {
            foreach(GameObject instance in selectedCells.Values) {
                DestroyImmediate(instance);
            }
            selectedCells.Clear();
            completedCells.Clear();
            combiningCells.Clear();
            foreach(var item in completeGo) {
                DestroyImmediate(item);
            }
            Repaint();
        }
        GUI.skin.button.normal.textColor = Color.red;
        if(GUILayout.Button("ClearCombining",GUILayout.Width(80))) { 
            combiningCells.Clear();
            Repaint();
        }
        if(GUILayout.Button("ClearCompleted",GUILayout.Width(80))) {
            completedCells.Clear();
            Repaint();
        }
        GUILayout.Space(20);

        GUI.skin.button.normal.textColor = oldColor;
        if(GUILayout.Button("SelectCombineGo",GUILayout.Width(160))) {
            isCombining = true;
        }
        if(GUILayout.Button("InstateCombineCollider",GUILayout.Width(160))) {
            InstateCombineCollider();
        }

        EditorGUILayout.EndHorizontal();
        toolbarRect = GUILayoutUtility.GetLastRect();


        //点击事件
        if(Event.current != null) {
            if(Event.current.type == EventType.ScrollWheel) {
                gridWidth += (int)Event.current.delta.y;
                gridHeight += (int)Event.current.delta.y;
                gridWidth = Mathf.Max(1,gridWidth);
                gridHeight = Mathf.Max(1,gridHeight);
                Repaint();
                return;
            }

            Vector2 mousePosition = Event.current.mousePosition;

            Vector2Int currentCell = new Vector2Int((int)(mousePosition.x / (position.width / gridWidth)),
                (int)(mousePosition.y / (position.height / gridHeight)));
            if((Event.current.type == EventType.MouseDown || Event.current.type == EventType.MouseDrag) && Event.current.button == 0) {
                if(toolbarRect.Contains(mousePosition)) {
                    return;
                }
                if(isCombining) {
                    if(selectedCells.ContainsKey(currentCell) && !combiningCells.Contains(currentCell)) {
                        combiningCells.Add(currentCell);
                    }
                } else {
                    if(!selectedCells.ContainsKey(currentCell)) {
                        Vector3 worldPos = new Vector3(currentCell.x,0,-currentCell.y);
                        GameObject instance = InstantiateSelectedPrefab(path_front + prefabNames[selectedPrefabIndex],worldPos);
                        selectedCells.Add(currentCell,instance);
                    }
                }
            } else if(Event.current.type == EventType.MouseDown || (Event.current.type == EventType.MouseDrag && Event.current.button == 1)) {
                if(selectedCells.ContainsKey(currentCell)) {
                    DestroyImmediate(selectedCells[currentCell]);
                    selectedCells.Remove(currentCell);
                }
            }
            Repaint();
        }



    }
    void DrawGrid() {
        float cellWidth = position.width / gridWidth; // Change this to change the cell width
        float cellHeight = position.height / gridHeight; // Change this to change the cell height

        Handles.color = Color.gray;
        for(int i = 0; i < gridWidth; i++) {
            for(int j = 0; j < gridHeight; j++) {
                Vector2Int currentCell = new Vector2Int(i,j);
                if(selectedCells.ContainsKey(currentCell)) {
                    // 如果当前的网格在 combinedCells 列表中，就将其颜色设置为红色
                    // 如果当前的网格在 completedCells 列表中，就将其颜色设置为黑色
                    if(completedCells.Contains(currentCell)) {
                        Handles.DrawSolidRectangleWithOutline(new Rect(i * cellWidth,j * cellHeight ,cellWidth,cellHeight),Color.black,Color.gray);
                    } else if(combiningCells.Contains(currentCell)) {
                        Handles.DrawSolidRectangleWithOutline(new Rect(i * cellWidth,j * cellHeight,cellWidth,cellHeight),Color.red,Color.gray);
                    } else {
                        Handles.DrawSolidRectangleWithOutline(new Rect(i * cellWidth,j * cellHeight,cellWidth,cellHeight),Color.white,Color.gray);
                    }
                } else {
                    Handles.DrawLine(new Vector3(i * cellWidth,j * cellHeight,0),new Vector3((i + 1) * cellWidth,j * cellHeight ,0));
                    Handles.DrawLine(new Vector3(i * cellWidth,j * cellHeight,0),new Vector3(i * cellWidth,(j + 1) * cellHeight ,0));
                }
            }
        }

        //画xy轴
        Handles.color = Color.red;
        Handles.DrawLine(new Vector3(0,position.height - 1,0),new Vector3(position.width,position.height - 1,0));
        Handles.DrawLine(new Vector3(0,0,0),new Vector3(0,position.height,0));
    }

    // 实例化预制体
    private GameObject InstantiateSelectedPrefab(string path,Vector3 worldPos) {
        GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
        if(prefab != null) {
            GameObject instance = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
            instance.name = prefab.name + "_" + instanceNum.ToString(); 
            instance.transform.position = worldPos;
            instance.isStatic = true;
            instanceNum++;
            return instance;
        } else {
            Debug.LogError("预制体未找到，请检查路径是否正确。");
            return null;
        }
    }

    // 添加这个方法来实例化组合的碰撞体
    private void InstateCombineCollider() {
        GameObject parent = new GameObject("CombinedObjects");
        parent.transform.position = selectedCells[combiningCells[0]].transform.position;
        parent.layer = 10;
        completeGo.Add(parent);
        foreach(Vector2Int cell in combiningCells) {
            GameObject instance = selectedCells[cell];
            instance.transform.SetParent(parent.transform);
        }
        Bounds bounds = new Bounds(parent.transform.position,Vector3.zero);
        foreach(MeshRenderer renderer in parent.GetComponentsInChildren<MeshRenderer>()) {
            bounds.Encapsulate(renderer.bounds);
        }
        BoxCollider boxCollider = parent.AddComponent<BoxCollider>();
        boxCollider.center = bounds.center - parent.transform.position;
        boxCollider.size = bounds.size;
        completedCells.AddRange(combiningCells);
        combiningCells.Clear();
    }
}

