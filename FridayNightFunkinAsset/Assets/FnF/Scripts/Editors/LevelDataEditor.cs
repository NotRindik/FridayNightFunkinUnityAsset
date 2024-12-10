using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelData))]
public class LevelDataEditor : Editor
{
    private SerializedProperty addMaxScore;
    private SerializedProperty addMaxScoreInLongArrow;
    private SerializedProperty arrowsPlayerPos;
    private SerializedProperty arrowsEnemyPos;
    private SerializedProperty arrowsList;
    private SerializedProperty currentPlayer;
    private SerializedProperty currentGirlFriend;
    private SerializedProperty currentEnemy;
    private SerializedProperty levelStage;

    Texture2D icon;

    private void OnEnable()
    {
        // Ссылаемся на свойства ScriptableObject
        addMaxScore = serializedObject.FindProperty("addMaxScore");
        addMaxScoreInLongArrow = serializedObject.FindProperty("addMaxScoreInLongArrow");
        arrowsPlayerPos = serializedObject.FindProperty("arrowsPlayerPos");
        arrowsEnemyPos = serializedObject.FindProperty("arrowsEnemyPos");
        arrowsList = serializedObject.FindProperty("arrowsList");
        currentPlayer = serializedObject.FindProperty("currentPlayer");
        currentGirlFriend = serializedObject.FindProperty("currentGirlFriend");
        currentEnemy = serializedObject.FindProperty("currentEnemy");
        levelStage = serializedObject.FindProperty("levelStage");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        // Пример редактирования данных с помощью инспектора
        EditorGUILayout.PropertyField(addMaxScore);
        EditorGUILayout.PropertyField(addMaxScoreInLongArrow);

        if(icon == null)
             icon = AssetDatabase.LoadAssetAtPath<Texture2D>("Assets/FnF/Editor/eventArrow.png");

        if (icon)
        {
            EditorGUIUtility.SetIconForObject(target, icon);
        }
    }
}
