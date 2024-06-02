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

    private Dictionary<Vector2Int,GameObject> addedCells = new Dictionary<Vector2Int,GameObject>();
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
        EditorGUILayout.BeginVertical(EditorStyles.toolbar);
        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        GUILayout.Label("Width:");
        gridWidth = EditorGUILayout.IntField(gridWidth,GUILayout.Width(50));
        GUILayout.Space(20);
        GUILayout.Label("Height:");
        gridHeight = EditorGUILayout.IntField(gridHeight,GUILayout.Width(50));
        GUILayout.Space(20);

        selectedPrefabIndex = EditorGUILayout.Popup("选择预制体",selectedPrefabIndex,prefabNames);
        GUILayout.Space(5);
        if(GUILayout.Button("AddPrefab",GUILayout.Width(80))) {
            isCombining = false;
        }
        GUILayout.Space(10);
        if(GUILayout.Button("SelectAllToCombine",GUILayout.Width(120))) {
            combiningCells = addedCells.Keys.ToList();
        }

        GUILayout.Space(10);
        if(GUILayout.Button("SelectCombineGo",GUILayout.Width(140))) {
            isCombining = true;
        }
        if(GUILayout.Button("InstateCombineCollider",GUILayout.Width(140))) {
            InstateCombineCollider();
        }
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        Color oldColor = GUI.skin.button.normal.textColor;
        GUI.skin.button.normal.textColor = Color.red;
        if(GUILayout.Button("ClearAll",GUILayout.Width(80))) {
            foreach(GameObject instance in addedCells.Values) {
                DestroyImmediate(instance);
            }
            addedCells.Clear();
            completedCells.Clear();
            combiningCells.Clear();
            foreach(var item in completeGo) {
                DestroyImmediate(item);
            }
            Repaint();
        } 
        if(GUILayout.Button("ClearCombining",GUILayout.Width(100))) { 
            combiningCells.Clear();
            Repaint();
        }
        if(GUILayout.Button("ClearCompleted",GUILayout.Width(100))) {
            completedCells.Clear();
            Repaint();
        }
        GUILayout.Space(10); 
        GUI.skin.button.normal.textColor = oldColor; 
        EditorGUILayout.EndHorizontal();


        EditorGUILayout.EndVertical();
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
                    if(addedCells.ContainsKey(currentCell) && !combiningCells.Contains(currentCell)) {
                        combiningCells.Add(currentCell);
                    }
                } else {
                    if(!addedCells.ContainsKey(currentCell)) {
                        Vector3 worldPos = new Vector3(currentCell.x,0,-currentCell.y);
                        GameObject instance = InstantiateSelectedPrefab(path_front + prefabNames[selectedPrefabIndex],worldPos);
                        addedCells.Add(currentCell,instance);
                    }
                }
            } else if(Event.current.type == EventType.MouseDown || (Event.current.type == EventType.MouseDrag && Event.current.button == 1)) {
                if(addedCells.ContainsKey(currentCell)) {
                    DestroyImmediate(addedCells[currentCell]);
                    addedCells.Remove(currentCell);
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
                if(addedCells.ContainsKey(currentCell)) {
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
        Handles.DrawLine(new Vector3(0,0,0),new Vector3(position.width,0,0));
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

    // 添加碰撞体
    private void InstateCombineCollider() {
        GameObject parent = new GameObject("CombinedObjects");
        parent.transform.position = addedCells[combiningCells[0]].transform.position;
        parent.layer = 10;
        completeGo.Add(parent);
        foreach(Vector2Int cell in combiningCells) {
            GameObject instance = addedCells[cell];
            instance.transform.SetParent(parent.transform);
        }
        CombineMesh(parent);
        completedCells.AddRange(combiningCells);
        combiningCells.Clear();
    }
    private void CombineMesh(GameObject parent) {
        // 获取所有的MeshFilter
        MeshFilter[] meshFilters = parent.GetComponentsInChildren<MeshFilter>(); 
        CombineInstance[] combine = new CombineInstance[meshFilters.Length];

        for(int i = 0; i < meshFilters.Length; i++) {
            combine[i].mesh = meshFilters[i].sharedMesh;
            combine[i].transform = meshFilters[i].transform.localToWorldMatrix;
        } 
        Mesh newMesh = new Mesh();
        newMesh.CombineMeshes(combine);  
        Material material = Resources.Load<Material>("ResPack/Mesh/Materials/CubeEdge2");

        // 获取或添加MeshRenderer组件
        MeshRenderer meshRenderer = parent.GetComponent<MeshRenderer>();
        if(meshRenderer == null) {
            meshRenderer = parent.AddComponent<MeshRenderer>();
        }

        // 设置材质
        meshRenderer.material = material;
        MeshFilter meshFilter = parent.AddComponent<MeshFilter>();
        
        meshFilter.sharedMesh = newMesh; 
        MeshCollider meshCollider = parent.AddComponent<MeshCollider>(); 
        meshCollider.sharedMesh = parent.GetComponent<MeshFilter>().sharedMesh;
 
        int childCount = parent.transform.childCount;
        for(int i = childCount - 1; i >= 0; i--) { 
            Transform child = parent.transform.GetChild(i); 
            DestroyImmediate(child.gameObject);
        } 
    }

}

