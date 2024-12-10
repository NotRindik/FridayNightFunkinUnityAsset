using UnityEditor;
using UnityEngine;
using FridayNightFunkin.Editor.TimeLineEditor;
using System.Collections.Generic;
using System;

public class LevelDataWindow : EditorWindow
{
    private string levelDataFileName = "LevelData.asset";
    public LevelData levelData { get; private set; }
    private ChartContainer chartContainer;
    private int selectedStageIndex = 0;
    private Dictionary<string, Vector2> scrollPositions = new Dictionary<string, Vector2>();
    private Vector2 windowScroll;
    private string tooltipText = null;
    private Rect tooltipRect;

    [MenuItem("Window/Level Data Editor")]
    public static void ShowWindow()
    {
        GetWindow<LevelDataWindow>("Level Data Editor");
    }

    private void OnGUI()
    {
        windowScroll = EditorGUILayout.BeginScrollView(windowScroll);
        levelData = Selection.activeObject as LevelData;
        if (Selection.activeGameObject != null)
        {
            if (Selection.activeGameObject.TryGetComponent(out ChartContainer chartContainer))
            {
                this.chartContainer = chartContainer;
                levelData = chartContainer.levelData;
            }
            else
            {
                this.chartContainer = null;
                levelData = null;
            }
        }

        if (levelData == null)
        {
            DrawNoLevelDataUI();
        }
        else
        {
            DrawLevelDataUI();
        }
        EditorGUILayout.EndScrollView();
    }

    private void DrawNoLevelDataUI()
    {
        EditorGUILayout.HelpBox("No Level Data selected. Please load or create a Level Data asset.", MessageType.Warning);

        if (GUILayout.Button("Load Level Data"))
        {
            string path = EditorUtility.OpenFilePanel("Select Level Data", "Assets/Levels", "asset");
            if (!string.IsNullOrEmpty(path))
            {
                LoadLevelData(path);
            }
        }

        if (GUILayout.Button("Create New Level Data"))
        {
            string path = EditorUtility.SaveFilePanel("Save New Level Data", "Assets", levelDataFileName, "asset");

            if (!string.IsNullOrEmpty(path))
            {
                CreateNewLevelData(path);
            }
        }
    }

    private void DrawLevelDataUI()
    {

        GUILayout.Label("Level Data Properties", EditorStyles.boldLabel);

        levelData.addMaxScore = (uint)EditorGUILayout.IntField("Add Max Score", (int)levelData.addMaxScore);

        levelData.addMaxScoreInLongArrow = (uint)EditorGUILayout.IntField("Add Max Score In Long Arrow", (int)levelData.addMaxScoreInLongArrow);

        EditorGUILayout.LabelField("Arrows Player Positions:");

        StageDraw();

        if (GUILayout.Button("Save Level Data"))
        {
            EditorUtility.SetDirty(levelData);
            AssetDatabase.SaveAssets();
        }
    }

    private void StageDraw()
    {
        if (levelData.stage != null && levelData.stage.Length > 0)
        {
            string[] stageNames = new string[levelData.stage.Length + 1];
            for (int i = 0; i < levelData.stage.Length; i++)
            {
                stageNames[i] = $"Stage {i + 1}";
            }
            stageNames[levelData.stage.Length] = $"+Create new Stage";
            selectedStageIndex = EditorGUILayout.Popup("Select Level Stage", selectedStageIndex, stageNames);

            if (selectedStageIndex != levelData.stage.Length)
                DrawLevelStage(levelData.stage[selectedStageIndex]);
            else
                AddNewStage();
        }
        else
        {
            EditorGUILayout.HelpBox("No Level Stages available in the Level Data.", MessageType.Info);

            if (GUILayout.Button("Add New Stage"))
            {
                AddNewStage();
            }
        }
    }

    private void DrawLevelStage(LevelStage stage)
    {

        float newChartSpeed = EditorGUILayout.FloatField("Chart Speed", stage.chartSpeed);
        if (!Mathf.Approximately(newChartSpeed, stage.chartSpeed))
        {
            stage.chartSpeed = newChartSpeed;
        }

        EditorGUILayout.Space();

        stage.BPM = EditorGUILayout.FloatField("BPM", stage.BPM);

        GUILayout.Label("PlayerIcons", EditorStyles.boldLabel);
        DrawIcons(stage.playerIcon);
        GUILayout.Label("EnemyIcons", EditorStyles.boldLabel);
        DrawIcons(stage.enemyIcon);

        EditorGUILayout.BeginVertical("Box");
        DrawTooltipLabel("Characters Prefabs", "If you wan't to spawn more than one chracter you can do this", EditorStyles.boldLabel);
        DrawArrayField(ref stage.playerPrefab, "Player prefab");
        DrawArrayField(ref stage.girlFriendPrefab, "GF prefab");
        DrawArrayField(ref stage.enemyPrefab, "Enemy prefab");
        RenderTooltip();
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("Box");
        DrawTooltipLabel("Characters positions","If you have more than one characters, you should set pos for all of them",EditorStyles.boldLabel);
        DrawArrayField(ref stage.playerPos,"Player pos",true);
        DrawArrayField(ref stage.girlPos,"GF pos", true);
        DrawArrayField(ref stage.enemyPos,"Enemy pos", true);
        RenderTooltip();
        EditorGUILayout.EndVertical();

        EditorGUILayout.BeginVertical("Box");
        DrawTooltipLabel("Chart Variants", "chats chartvariants for one stage, basicly it's need for difficults", EditorStyles.boldLabel);
        DrawArrayField(ref stage.chartVariants, "Player pos");
        RenderTooltip();
        EditorGUILayout.EndVertical();

    }

    private void DrawArrayField<T>(ref T[] arr, string arrName,bool allowObjectFromScene = false) where T : UnityEngine.Object
    {
        EditorGUILayout.BeginVertical("Box");
        GUILayout.Label(arrName, EditorStyles.boldLabel);

        if (arr == null)
        {
            arr = new T[0];
        }

        var fontStyle = new GUIStyle(GUI.skin.button);
        fontStyle.fontStyle = FontStyle.Bold;

        if (!scrollPositions.ContainsKey(arrName))
        {
            scrollPositions.Add(arrName, Vector2.zero);
        }

        Vector2 localScrollPosition = scrollPositions[arrName];
        localScrollPosition = arr.Length < 3
            ? EditorGUILayout.BeginScrollView(localScrollPosition, GUILayout.Height(45*arr.Length))
            : EditorGUILayout.BeginScrollView(localScrollPosition, GUILayout.Height(80));

        scrollPositions[arrName] = localScrollPosition;

        if (arr != null)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                EditorGUILayout.BeginHorizontal();
                arr[i] = (T)EditorGUILayout.ObjectField(arr[i], typeof(T), allowObjectFromScene);

                if (GUILayout.Button("X", fontStyle, GUILayout.Width(30)))
                {
                    if (arr != null && arr.Length > 0)
                    {
                        var newList = new List<T>(arr);
                        newList.RemoveAt(i);
                        arr = newList.ToArray();
                    }
                }

                EditorGUILayout.EndHorizontal();
            }
        }

        if (arr.Length < 3)
            AddElement(ref arr, fontStyle);

        EditorGUILayout.EndScrollView();

        if (arr.Length >= 3)
            AddElement(ref arr, fontStyle);

        if (GUI.changed)
        {
            EditorUtility.SetDirty(levelData);
        }

        EditorGUILayout.EndVertical();
    }

    private static void AddElement<T>(ref T[] arr, GUIStyle fontStyle) where T : UnityEngine.Object
    {
        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("+", fontStyle, GUILayout.Width(30)))
        {
            Array.Resize(ref arr, arr.Length + 1);
            arr[arr.Length - 1] = null;
        }
        EditorGUILayout.EndHorizontal();
    }
    private void DrawTooltipLabel(string labelText, string tooltip, GUIStyle style = null, params GUILayoutOption[] options)
    {
        style ??= GUI.skin.label;

        GUILayout.Label(labelText, style, options);

        Rect lastRect = GUILayoutUtility.GetLastRect();

        if (lastRect.Contains(Event.current.mousePosition))
        {
            tooltipText = tooltip;
            tooltipRect = new Rect(Event.current.mousePosition + new Vector2(15, 15), new Vector2(200, 40));
        }
        else
        {
            tooltipRect = Rect.zero;
        }
    }
    private void RenderTooltip()
    {
        if (!string.IsNullOrEmpty(tooltipText))
        {
            GUIStyle tooltipStyle = new GUIStyle(EditorStyles.helpBox);
            tooltipStyle.normal.background = Texture2D.normalTexture;
            tooltipStyle.normal.textColor = Color.white; 
            tooltipStyle.alignment = TextAnchor.MiddleCenter;

            GUI.Label(tooltipRect, tooltipText, tooltipStyle);
        }
    }

    private void DrawIcons(Dictionary<IconProgressStatus, Sprite> icons)
    {
        GUILayout.BeginVertical("Box");
        int labelWidth = 100;

        foreach (IconProgressStatus key in Enum.GetValues(typeof(IconProgressStatus)))
        {
            EditorGUILayout.BeginHorizontal();

            if (!icons.ContainsKey(key))
            {
                icons.Add(key, null);
            }

            GUILayout.Label(key.ToString(), GUILayout.Width(labelWidth));

            icons[key] = (Sprite)EditorGUILayout.ObjectField(icons[key], typeof(Sprite), false);

            EditorGUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();
    }

    private void AddNewStage()
    {
        var stages = new System.Collections.Generic.List<LevelStage>(levelData.stage ?? new LevelStage[0]);
        stages.Add(new LevelStage());
        levelData.stage = stages.ToArray();
        selectedStageIndex = stages.Count - 1;
    }

    private void LoadLevelData(string path)
    {
        string relativePath = FileUtil.GetProjectRelativePath(path);
        LevelData loadedData = AssetDatabase.LoadAssetAtPath<LevelData>(relativePath);

        if (loadedData != null)
        {
            levelData = loadedData;
            Selection.activeObject = loadedData;
            Debug.Log("Level Data loaded successfully!");
        }
        else
        {
            Debug.LogError("Failed to load Level Data. Make sure the selected file is a LevelData asset.");
        }
    }

    private void CreateNewLevelData(string path)
    {
        string relativePath = FileUtil.GetProjectRelativePath(path);

        if (string.IsNullOrEmpty(relativePath))
        {
            Debug.LogError("Selected path is not in Unity project, it should be inside the Assets folder.");
            return;
        }

        LevelData newLevelData = ScriptableObject.CreateInstance<LevelData>();
        AssetDatabase.CreateAsset(newLevelData, relativePath);
        AssetDatabase.SaveAssets();

        Selection.activeObject = newLevelData;
        levelData = newLevelData;

        Debug.Log("New Level Data created successfully at " + relativePath);
    }
}
