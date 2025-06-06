using UnityEditor;
using UnityEngine;
using UnityEditor.Animations;
using System.Collections.Generic;

public class SpritePivotEditorWindow : EditorWindow {
    GameObject selectedObject;
    SpriteRenderer spriteRenderer;
    Animator animator;

    bool pivotEditMode = false;

    AnimationClip selectedClip;
    List<AnimationClip> clips = new List<AnimationClip>();
    List<FrameData> spriteKeyframes = new List<FrameData>();
    int currentFrameIndex = 0;
    private static SpritePivotEditorWindow currentWindow;

    private bool showPreviousOnionSkin;
    private bool showNextOnionSkin;
    
    [MenuItem("Tools/Sprite Pivot Editor")]
    public static void ShowWindow() {
        currentWindow = GetWindow<SpritePivotEditorWindow>("Pivot Editor");
        SceneView.duringSceneGui += OnSceneGUI;
    }
    private void OnDestroy()
    {
        SceneView.duringSceneGui -= OnSceneGUI;
        if (currentWindow == this)
            currentWindow = null;
    }

    void OnSelectionChange() {
        Repaint();
    }

    void OnGUI() {
        selectedObject = Selection.activeGameObject;
        if (selectedObject == null) {
            EditorGUILayout.HelpBox("Выделите объект на сцене", MessageType.Info);
            return;
        }

        spriteRenderer = selectedObject.GetComponent<SpriteRenderer>();
        animator = selectedObject.GetComponent<Animator>();

        if (spriteRenderer == null) {
            if (GUILayout.Button("Добавить SpriteRenderer")) {
                selectedObject.AddComponent<SpriteRenderer>();
            }
            return;
        }

        if (animator != null && animator.runtimeAnimatorController != null) {
            DrawAnimatorMode();
        } else {
            DrawSimpleMode();
        }
    }
    private bool initialized = false;
    void DrawSimpleMode() {
        EditorGUILayout.LabelField("Текущий спрайт:", spriteRenderer.sprite?.name ?? "None");
        if (!initialized) {
            spriteKeyframes.Clear();
            spriteKeyframes.Add(new FrameData(Vector2.zero));
            currentFrameIndex = 0;
            initialized = true;
        }

        if (!pivotEditMode) {
            if (GUILayout.Button("Войти в режим PivotEdit")) {
                pivotEditMode = true;
                if (spriteRenderer.sprite != null)
                {
                    Vector2 normalizedPivot = new Vector2(
                        spriteRenderer.sprite.pivot.x / spriteRenderer.sprite.rect.width,
                        spriteRenderer.sprite.pivot.y / spriteRenderer.sprite.rect.height
                    );
                    spriteKeyframes[currentFrameIndex].PivotOffset = normalizedPivot;
                }
                else
                {
                    spriteKeyframes[currentFrameIndex].PivotOffset = Vector2.zero;
                }
                Repaint();
            }
        } else {
            spriteKeyframes[currentFrameIndex].PivotOffset = EditorGUILayout.Vector2Field("Смещение пивота", spriteKeyframes[currentFrameIndex].PivotOffset);
            if (GUILayout.Button("Применить и выйти")) {
                ApplyPivot(spriteRenderer.sprite, spriteKeyframes[0].PivotOffset);
                pivotEditMode = false;
                initialized = false;
            }
            if (GUILayout.Button("Отмена")) {
                pivotEditMode = false;
                initialized = false;
            }
            Repaint();
        }
    }

    void DrawAnimatorMode() {
        AnimatorController controller = animator.runtimeAnimatorController as AnimatorController;
        if (controller == null) {
            EditorGUILayout.HelpBox("AnimatorController не найден", MessageType.Warning);
            return;
        }

        clips.Clear();
        clips.AddRange(controller.animationClips);
        
        EditorGUILayout.LabelField("Выберите AnimationClip:");
        foreach (var clip in clips) {
            if (GUILayout.Button(clip.name)) {
                selectedClip = clip;
                ExtractSpriteKeyframes();
            }
        }

        if (selectedClip != null && spriteKeyframes.Count > 0) {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Ключевой кадр:", currentFrameIndex.ToString());
            EditorGUI.BeginChangeCheck();
            showPreviousOnionSkin = GUILayout.Toggle(showPreviousOnionSkin, "Prev Onion Skin");
            if (EditorGUI.EndChangeCheck())
            {
                Repaint();
                SceneView.RepaintAll();
            }
            
            if (GUILayout.Button("Предыдущий кадр") && currentFrameIndex > 0) {
                currentFrameIndex--;
                spriteRenderer.sprite = spriteKeyframes[currentFrameIndex].Keyframe.value as Sprite;
            }

            if (GUILayout.Button("Следующий кадр") && currentFrameIndex < spriteKeyframes.Count - 1) {
                currentFrameIndex++;
                spriteRenderer.sprite = spriteKeyframes[currentFrameIndex].Keyframe.value as Sprite;
            }
            EditorGUI.BeginChangeCheck();
            showNextOnionSkin = GUILayout.Toggle(showNextOnionSkin, "Prev Onion Skin");
            if (EditorGUI.EndChangeCheck())
            {
                Repaint();
                SceneView.RepaintAll();
            }

            if (spriteRenderer.sprite != null)
                EditorGUILayout.LabelField("Текущий спрайт:", spriteRenderer.sprite.name);

            EditorGUILayout.Space();
            
            if (!pivotEditMode) {
                if (GUILayout.Button("Войти в режим PivotEdit")) {
                    pivotEditMode = true;
                    if (spriteRenderer.sprite != null)
                    {
                        Vector2 normalizedPivot = new Vector2(
                            spriteRenderer.sprite.pivot.x / spriteRenderer.sprite.rect.width,
                            spriteRenderer.sprite.pivot.y / spriteRenderer.sprite.rect.height
                        );
                        spriteKeyframes[currentFrameIndex].PivotOffset = normalizedPivot;
                    }
                    else
                    {
                        spriteKeyframes[currentFrameIndex].PivotOffset = Vector2.zero;
                    }
                    Repaint();
                    SceneView.RepaintAll();
                }
            } else {
                spriteKeyframes[currentFrameIndex].PivotOffset = EditorGUILayout.Vector2Field("Смещение пивота", spriteKeyframes[currentFrameIndex].PivotOffset);
                
            
                if (GUILayout.Button("Применить и выйти")) {
                    ApplyPivot(spriteKeyframes);
                    Repaint();
                    pivotEditMode = false;
                    SceneView.RepaintAll();
                }
                if (GUILayout.Button("Отмена")) {
                    pivotEditMode = false;
                    Repaint();
                    SceneView.RepaintAll();
                }
            }
        }
    }

    void ExtractSpriteKeyframes() {
        spriteKeyframes.Clear();
        currentFrameIndex = 0;

        var bindings = AnimationUtility.GetObjectReferenceCurveBindings(selectedClip);
        foreach (var binding in bindings) {
            if (binding.propertyName == "m_Sprite") {
                var keyframes = AnimationUtility.GetObjectReferenceCurve(selectedClip, binding);
                foreach (var frame in keyframes)
                {
                    Vector2 normalizedPivot = new Vector2(
                        spriteRenderer.sprite.pivot.x / spriteRenderer.sprite.rect.width,
                        spriteRenderer.sprite.pivot.y / spriteRenderer.sprite.rect.height
                    );
                    spriteKeyframes.Add(new FrameData(frame,normalizedPivot));   
                }
                if (spriteKeyframes.Count > 0)
                    spriteRenderer.sprite = spriteKeyframes[0].Keyframe.value as Sprite;
                break;
            }
        }
    }


    private static void OnSceneGUI(SceneView sceneView) {
        if (currentWindow == null || !currentWindow.pivotEditMode || currentWindow.spriteRenderer == null || currentWindow.spriteRenderer.sprite == null)
            return;

        void DrawSpriteGUI(Sprite sprite, Vector2 pivotOffset, Vector3 worldPos, Color color) {
            if (sprite == null || sprite.texture == null) return;

            var texture = sprite.texture;
            float pixelsPerUnit = sprite.pixelsPerUnit;
            Vector2 offsetUnits = pivotOffset / pixelsPerUnit;
            Vector3 previewWorldPos = worldPos + (Vector3)offsetUnits;

            float unitWidth = sprite.rect.width / pixelsPerUnit;
            float unitHeight = sprite.rect.height / pixelsPerUnit;
            Vector3 bottomLeft = previewWorldPos - new Vector3(unitWidth / 2, unitHeight / 2, 0);

            Vector2 guiCenter = HandleUtility.WorldToGUIPoint(previewWorldPos);
            float scale = currentWindow.spriteRenderer.transform.lossyScale.x * 80;
            float handleSize = HandleUtility.GetHandleSize(previewWorldPos);
            float guiScale = scale / handleSize;

            Vector2 pivotOffsetFromCenterPixels = sprite.pivot - (sprite.rect.size / 2f);
            Vector2 pivotOffsetFromCenterGUI = new Vector2(
                pivotOffsetFromCenterPixels.x * (guiScale / pixelsPerUnit),
                -pivotOffsetFromCenterPixels.y * (guiScale / pixelsPerUnit)
            );

            float guiWidth = sprite.rect.width * (guiScale / pixelsPerUnit);
            float guiHeight = sprite.rect.height * (guiScale / pixelsPerUnit);

            Rect drawRect = new Rect(
                guiCenter.x - guiWidth / 2,
                guiCenter.y - guiHeight / 2,
                guiWidth,
                guiHeight
            );
            Rect correctedDrawRect = new Rect(
                drawRect.x - pivotOffsetFromCenterGUI.x,
                drawRect.y - pivotOffsetFromCenterGUI.y,
                drawRect.width,
                drawRect.height
            );

            Rect uvRect = new Rect(
                sprite.rect.x / texture.width,
                sprite.rect.y / texture.height,
                sprite.rect.width / texture.width,
                sprite.rect.height / texture.height
            );

            Color oldColor = GUI.color;
            GUI.color = color;
            GUI.DrawTextureWithTexCoords(correctedDrawRect, texture, uvRect);
            GUI.color = oldColor;
        }

        Handles.BeginGUI();

        Vector3 objectWorldPos = currentWindow.selectedObject.transform.position;
        float pixelsPerUnitMain = currentWindow.spriteRenderer.sprite.pixelsPerUnit;

        // Onion skin — предыдущий ключ
        if (currentWindow.showPreviousOnionSkin && currentWindow.currentFrameIndex > 0) {
            var prevFrame = currentWindow.spriteKeyframes[currentWindow.currentFrameIndex - 1];
            DrawSpriteGUI(currentWindow.spriteRenderer.sprite, prevFrame.PivotOffset, objectWorldPos, Color.red);
        }

        // Onion skin — следующий ключ
        if (currentWindow.showNextOnionSkin && currentWindow.currentFrameIndex < currentWindow.spriteKeyframes.Count - 1) {
            var nextFrame = currentWindow.spriteKeyframes[currentWindow.currentFrameIndex + 1];
            DrawSpriteGUI(currentWindow.spriteRenderer.sprite, nextFrame.PivotOffset, objectWorldPos, Color.green);
        }

        // Отрисовка текущего ключа
        var currentFrame = currentWindow.spriteKeyframes[currentWindow.currentFrameIndex];
        DrawSpriteGUI(currentWindow.spriteRenderer.sprite, currentFrame.PivotOffset, objectWorldPos, new Color(1,1,1,0.5f));
        Handles.EndGUI();
        // Pivot Handle
        Vector2 pivotOffsetWorld = currentFrame.PivotOffset / pixelsPerUnitMain;
        Vector3 worldPivotPos = objectWorldPos + (Vector3)pivotOffsetWorld;

        EditorGUI.BeginChangeCheck();
        var fmh = Quaternion.identity;
        Vector3 newWorldPivot = Handles.FreeMoveHandle(
            worldPivotPos,
            HandleUtility.GetHandleSize(worldPivotPos) * 0.1f,
            Vector3.zero,
            Handles.CircleHandleCap
        );

        if (EditorGUI.EndChangeCheck()) {
            Undo.RecordObject(currentWindow, "Move Pivot Handle");

            Vector2 localOffset = newWorldPivot - objectWorldPos;
            currentWindow.spriteKeyframes[currentWindow.currentFrameIndex].PivotOffset = localOffset * pixelsPerUnitMain;
            sceneView.Repaint();
            currentWindow.Repaint();
        }

        Handles.Label(worldPivotPos + Vector3.up * 0.1f, "Pivot");
        
    }

    
    void ApplyPivot(Sprite sprite, Vector2 pixelPivot)
    {
        string path = AssetDatabase.GetAssetPath(sprite);
        TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(path);

        Rect rect = sprite.rect;
        Vector2 pivot = pixelPivot;
        Vector2 normalizedPivot = new Vector2(
            pivot.x / rect.width,
            1f - (pivot.y / rect.height) // ← вот здесь инверсия
        );

        if (importer.spriteImportMode == SpriteImportMode.Single)
        {
            importer.spritePivot = normalizedPivot;
        }
        else if (importer.spriteImportMode == SpriteImportMode.Multiple)
        {
            SpriteMetaData[] metas = importer.spritesheet;
            for (int i = 0; i < metas.Length; i++)
            {
                if (metas[i].name == sprite.name)
                {
                    metas[i].alignment = (int)SpriteAlignment.Custom;
                    metas[i].pivot = normalizedPivot;
                    break;
                }
            }
            importer.spritesheet = metas;
        }

        EditorUtility.SetDirty(importer);
        importer.SaveAndReimport();
    }
    
        void ApplyPivot(List<FrameData> frameDatas)
        {
            var importerMap = new Dictionary<string, FrameData>();

            foreach (var frameData in frameDatas)
            {
                string path = AssetDatabase.GetAssetPath(frameData.Keyframe.value);
                if (!importerMap.ContainsKey(path))
                    importerMap[path] = frameData;
            }
            
            foreach (var kvp in importerMap)
            {
                string path = kvp.Key;
                FrameData frames = kvp.Value;

                TextureImporter importer = (TextureImporter)AssetImporter.GetAtPath(path);
                

                if (importer.spriteImportMode == SpriteImportMode.Single)
                {
                    Sprite sprite = (Sprite)frames.Keyframe.value;
                    Rect rect = sprite.rect;
                    Vector2 pivot = frames.PivotOffset;
                    Vector2 normalizedPivot = new Vector2(
                        pivot.x / rect.width,
                        1f - (pivot.y / rect.height)
                    );
                    importer.spritePivot = normalizedPivot;
                }
                else if (importer.spriteImportMode == SpriteImportMode.Multiple)
                {
                    SpriteMetaData[] metas = importer.spritesheet;

                    Sprite sprite = (Sprite)frames.Keyframe.value;
                    Rect rect = sprite.rect;
                    Vector2 pivot = frames.PivotOffset;
                    Vector2 normalizedPivot = new Vector2(pivot.x / rect.width, pivot.y / rect.height);

                    for (int i = 0; i < metas.Length; i++)
                    {
                        if (metas[i].name == sprite.name)
                        {
                            metas[i].alignment = (int)SpriteAlignment.Custom;
                            metas[i].pivot = normalizedPivot;
                            break;
                        }
                    }

                    importer.spritesheet = metas;
                }

                EditorUtility.SetDirty(importer);
                importer.SaveAndReimport();
            }
        }
}


public class FrameData
{
    public ObjectReferenceKeyframe Keyframe;

    private Vector2 _PivotOffset;
    public Vector2 PivotOffset
    {
        get => _PivotOffset;
        set
        {
            _PivotOffset = value;
            SceneView.RepaintAll();
        }
    }
    public FrameData(ObjectReferenceKeyframe objectReferenceKeyframe, Vector2 pivotOffset)
    {
        Keyframe = objectReferenceKeyframe;
        PivotOffset = pivotOffset;
    }
    
    public FrameData( Vector2 pivotOffset)
    {
        PivotOffset = pivotOffset;
    }
}