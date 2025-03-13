/*using FridayNightFunkin.CHARACTERS;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelData))]
public class LevelDataEditor : Editor
{
    public override void OnInspectorGUI()
    {
        LevelData levelData = (LevelData)target;

        EditorGUILayout.LabelField("General Level Data", EditorStyles.boldLabel);
        levelData.addMaxScore = (uint)EditorGUILayout.IntField("Max Score (Arrow)", (int)levelData.addMaxScore);
        levelData.addMaxScoreInLongArrow = (uint)EditorGUILayout.IntField("Max Score (Long Arrow)", (int)levelData.addMaxScoreInLongArrow);
        levelData.arrowLayer = EditorGUILayout.LayerField("Arrow Layer", levelData.arrowLayer);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Stages", EditorStyles.boldLabel);

        if (levelData.stage != null && levelData.stage.Length > 0)
        {
            for (int i = 0; i < levelData.stage.Length; i++)
            {
                EditorGUILayout.BeginVertical("box");

                EditorGUILayout.LabelField($"Stage {i + 1}", EditorStyles.boldLabel);
                LevelStage stage = levelData.stage[i];

                stage.BPM = EditorGUILayout.FloatField("BPM", stage.BPM);
                stage.chartSpeed = EditorGUILayout.FloatField("Chart Speed", stage.chartSpeed);
                stage.playerForce = EditorGUILayout.FloatField("Player Force", stage.playerForce);
                stage.missForce = EditorGUILayout.FloatField("Miss Force", stage.missForce);
                stage.enemyForce = EditorGUILayout.FloatField("Enemy Force", stage.enemyForce);

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Player Prefabs", EditorStyles.boldLabel);
                for (int j = 0; j < stage.playerPrefab.Length; j++)
                {
                    stage.playerPrefab[j] = (Character_Fnf_PlayAble)EditorGUILayout.ObjectField($"Player Prefab {j + 1}", stage.playerPrefab[j], typeof(Character_Fnf_PlayAble), false);
                }

                EditorGUILayout.LabelField("Enemy Prefabs", EditorStyles.boldLabel);
                for (int j = 0; j < stage.enemyPrefab.Length; j++)
                {
                    stage.enemyPrefab[j] = (Character_Fnf_Enemy)EditorGUILayout.ObjectField($"Enemy Prefab {j + 1}", stage.enemyPrefab[j], typeof(Character_Fnf_Enemy), false);
                }

                EditorGUILayout.LabelField("Positions", EditorStyles.boldLabel);
                EditorGUILayout.PropertyField(serializedObject.FindProperty($"stage[{i}].playerPos"), new GUIContent("Player Positions"));
                EditorGUILayout.PropertyField(serializedObject.FindProperty($"stage[{i}].enemyPos"), new GUIContent("Enemy Positions"));

                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Icon Progress Sprites", EditorStyles.boldLabel);
                foreach (var status in stage.playerIcon.Keys)
                {
                    stage.playerIcon[status] = (Sprite)EditorGUILayout.ObjectField($"Player Icon ({status})", stage.playerIcon[status], typeof(Sprite), false);
                }
                foreach (var status in stage.enemyIcon.Keys)
                {
                    stage.enemyIcon[status] = (Sprite)EditorGUILayout.ObjectField($"Enemy Icon ({status})", stage.enemyIcon[status], typeof(Sprite), false);
                }

                EditorGUILayout.EndVertical();
            }
        }
        else
        {
            EditorGUILayout.HelpBox("No stages found.", MessageType.Warning);
        }

        if (GUILayout.Button("Add Stage"))
        {
            System.Array.Resize(ref levelData.stage, levelData.stage.Length + 1);
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(target);
        }
    }
}*/
