using System;
using System.Collections.Generic;
using FridayNightFunkin.Editor.TimeLineEditor;
using UnityEditor;
using UnityEngine;
#if UNITY_EDITOR
namespace FnF.Scripts.Editors
{
    public class LevelDataWindow : EditorWindow
    {
        private string levelDataFileName = "LevelData.asset";
        public LevelData levelData { get; private set; }
        private ChartPlayBack chartPlayback;
        private int preveusSelectedChartVar;
        private Dictionary<string, Vector2> scrollPositions = new Dictionary<string, Vector2>();
        private string tooltipText = null;
        private Rect tooltipRect;
        private bool isLocked;
        
        private Rect windowRect = new Rect(50, 50, 400, 300);

        private Vector2 scroll;

        public Action OnGUIUpdate;

        [MenuItem("Window/FNF Level Data Editor")]
        public static void ShowWindow()
        {
            var window = GetWindow<LevelDataWindow>("FNF Level Data Editor");
        }
        private void OnEnable()
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
        }
        private void OnGUI()
        {
            DrawHeader();

            if (!isLocked)
            {
                levelData = Selection.activeObject as LevelData;
                if (Selection.activeGameObject != null)
                {
                    if (Selection.activeGameObject.TryGetComponent(out ChartPlayBack chartPlayback))
                    {
                        InitChartPlayback(chartPlayback);
                    }
                    else
                    {
                        this.chartPlayback = null;
                        levelData = null;
                    }
                }
            }
            if (levelData == null)
            {
                DrawNoLevelDataUI();
            }
            else
            {
                scroll = EditorGUILayout.BeginScrollView(scroll);
                DrawLevelDataUI();
                EditorGUILayout.EndScrollView();
            }
        }
        private void InitChartPlayback(ChartPlayBack chartPlayback)
        {
            this.chartPlayback = chartPlayback;
            levelData = chartPlayback.levelData;
        }

        private void DrawHeader()
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);


            GUILayout.FlexibleSpace();
            if (chartPlayback != null)
            {
                if (chartPlayback.levelData == null)
                {
                    GUI.enabled = false;
                }
                else
                {
                    GUI.enabled = true;

                }
            }
            else
            {
                GUI.enabled = false;
            }
            if (GUILayout.Button("Save Level Data"))
            {
                EditorUtility.SetDirty(levelData);
                AssetDatabase.SaveAssets();
            }
            isLocked = GUILayout.Toggle(isLocked, isLocked ? EditorGUIUtility.IconContent("LockIcon-On") : EditorGUIUtility.IconContent("LockIcon"),
                "ToolbarButton", GUILayout.Width(30));
            GUI.enabled = true;
            GUILayout.EndHorizontal();
        }
        private void DrawNoLevelDataUI()
        {
            var selectedGameObject = Selection.activeGameObject;
            if (selectedGameObject != null)
            {
                if (GUILayout.Button("Create New Level Data"))
                {
                    string path = EditorUtility.SaveFilePanel("Save New Level Data", "Assets", levelDataFileName, "asset");

                    if (!string.IsNullOrEmpty(path))
                    {
                        CreateNewLevelData(path, selectedGameObject);
                    }
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Please Select GameObject", MessageType.Warning);
            }
        }

        private void DrawLevelDataUI()
        {
            GUIStyle headerStyle = new GUIStyle(EditorStyles.boldLabel);
            headerStyle.fontSize = 16;
            GUILayout.Label("LEVEL DATA BASE SETTINGS", headerStyle);

            levelData.addMaxScore = (uint)EditorGUILayout.IntField("Add Max Score", (int)levelData.addMaxScore);

            levelData.addMaxScoreInLongArrow = (uint)EditorGUILayout.IntField("Add Max Score In Long Arrow", (int)levelData.addMaxScoreInLongArrow);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("STAGE SETTINGS",  headerStyle);
            EditorGUILayout.Space();
            StageDraw();
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
                levelData.selectedStageIndex = EditorGUILayout.Popup("Select Level Stage", levelData.selectedStageIndex, stageNames);

                if (levelData.selectedStageIndex != levelData.stage.Length)
                    DrawLevelStage(levelData.stage[levelData.selectedStageIndex]);
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
            stage.name = EditorGUILayout.TextField("Stage Name",stage.name);
            stage.icon = (Sprite)EditorGUILayout.ObjectField("Stage Sprite", stage.icon, typeof(Sprite), false);
            
            float newChartSpeed = EditorGUILayout.FloatField("Chart Speed", stage.ChartSpeed);
            if (!Mathf.Approximately(newChartSpeed, stage.ChartSpeed))
            {
                stage.ChartSpeed = newChartSpeed;
            }
            
            stage.chartSpawnDistance = EditorGUILayout.FloatField("Chart Spawn Distance", stage.chartSpawnDistance);

            EditorGUILayout.Space();
            
            EditorGUILayout.LabelField("Music");
            
            stage.BPM = EditorGUILayout.FloatField("BPM", stage.BPM);
            
            EditorGUILayout.Space();
            
            EditorGUILayout.LabelField("Health bar settings");

            stage.startHealth = EditorGUILayout.FloatField("Start health", stage.startHealth);
            
            stage.maxHealth = EditorGUILayout.FloatField("Max health", stage.maxHealth);
            
            stage.minHealth = EditorGUILayout.FloatField("Min health", stage.minHealth);
            
            EditorGUILayout.Space();
            
            EditorGUILayout.LabelField("Damages/Forces");

            stage.playerForce = EditorGUILayout.FloatField("Player Damage", stage.playerForce);
            
            stage.enemyForce = EditorGUILayout.FloatField("Enemy Damage", stage.enemyForce);
            
            stage.missForce = EditorGUILayout.FloatField("Miss Damage", stage.missForce);
            
            DrawTooltipLabel("Map objects","You can add here prefabs with your map.It will spawn on zero coordinates", EditorStyles.boldLabel);
            DrawArrayField(ref stage.mapGameObjects,allowObjectFromScene:true);

            RenderTooltip();
            GUILayout.Label("PlayerIcons", EditorStyles.boldLabel);
            DrawIconsField(stage.playerIcon);
            GUILayout.Label("EnemyIcons", EditorStyles.boldLabel);
            DrawIconsField(stage.enemyIcon);

            EditorGUILayout.BeginVertical("Box");
            DrawTooltipLabel("Characters Prefabs", "If you want to spawn more than one character you can do this", EditorStyles.boldLabel);
            DrawArrayField(ref stage.playerPrefab, "Player prefab");
            DrawArrayField(ref stage.girlFriendPrefab, "GF prefab");
            DrawArrayField(ref stage.enemyPrefab, "Enemy prefab");
            RenderTooltip();
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical("Box");
            DrawTooltipLabel("Chart Variants", "Chart variants for one stage, basically it's needed for difficulties.(FROM EASY TO HARD)", EditorStyles.boldLabel);
            DrawArrayField(ref stage.chartVariants);

            if (stage.chartVariants != null && stage.chartVariants.Length > 0)
            {
                string[] arrElementShowing = new string[stage.chartVariants.Length];
                for (int i = 0; i < stage.chartVariants.Length; i++)
                {
                    if (stage.chartVariants[i] != null)
                        arrElementShowing[i] = $"{stage.chartVariants[i].name}";
                }
                levelData.selectedChartVar = EditorGUILayout.Popup("Current chart variant", levelData.selectedChartVar, arrElementShowing);
                if (levelData.selectedChartVar != preveusSelectedChartVar)
                {
                    OnGUIUpdate?.Invoke();
                    preveusSelectedChartVar = levelData.selectedChartVar;
                }
            }
            else
            {
                EditorGUILayout.HelpBox("No Variants", MessageType.Info);
            }
            RenderTooltip();
            EditorGUILayout.EndVertical();
        
            var isDelete = GUILayout.Button("Delete current stage");
            if (isDelete)
            {
                DeleteStage(stage);
            }
        }

        private void DrawArrayField<T>(ref T[] arr, string arrName = "", bool allowObjectFromScene = false) where T : UnityEngine.Object
        {
            EditorGUILayout.BeginVertical("Box");
            if (arrName != "") GUILayout.Label(arrName, EditorStyles.boldLabel);

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
                ? EditorGUILayout.BeginScrollView(localScrollPosition, GUILayout.Height(45 * arr.Length))
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

        private void DrawIconsField(Dictionary<IconProgressStatus, Sprite> icons)
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
            var stages = new List<LevelStage>(levelData.stage ?? new LevelStage[0]);
            stages.Add(new LevelStage());
            levelData.stage = stages.ToArray();
            levelData.selectedStageIndex = stages.Count - 1;
        }
        private void DeleteStage(LevelStage stage)
        {
            var stages = new List<LevelStage>(levelData.stage);
            stages.Remove(stage);
            levelData.stage = stages.ToArray();
            levelData.selectedStageIndex = stages.Count-1;
            if (levelData.selectedStageIndex == -1)
                levelData.selectedStageIndex = 0;
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

        private void CreateNewLevelData(string path, GameObject selectedGameObject)
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
            ChartPlayBack chartPlayBack;
            if (!selectedGameObject.TryGetComponent(out ChartPlayBack playback))
            {
                chartPlayBack = selectedGameObject.AddComponent<ChartPlayBack>();
            }
            else
            {
                chartPlayBack = playback;
            }
            levelData = newLevelData;
            chartPlayBack.levelData = levelData;
            InitChartPlayback(chartPlayBack);
            Debug.Log("New Level Data created successfully at " + relativePath);
        }
    }
}
#endif