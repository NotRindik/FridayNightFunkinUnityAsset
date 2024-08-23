using DIALOGUE;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace FridayNightFunkin.Editor
{
    [ExecuteInEditMode]
    public class EditModeArrowTaker : MonoBehaviour
    {
        public Image saveArrow;
        public float distanceForTakearrow;
        private Collider2D[] overlapCircle;
        public static EditModeArrowTaker instance;

        public Sprite[] TakeSprite;
        public Sprite[] DefaultSprite;

        public List<Arrow> lastArrowRef = new List<Arrow>();
        private LevelSettings levelSettings => LevelSettings.instance;

        private void Start()
        {
            for (int i = 0; i < levelSettings.arrowsPlayer.Length; i++)
            {
                saveArrow = levelSettings.arrowsPlayer[i];

                saveArrow.sprite = DefaultSprite[i];

                if (levelSettings.arrowsPlayer.Length == levelSettings.arrowsEnemy.Length)
                {
                    saveArrow = levelSettings.arrowsEnemy[i];

                    saveArrow.sprite = DefaultSprite[i];
                }
            }
        }

#if UNITY_EDITOR
        [ExecuteInEditMode]

        private void Update()
        {
            if (instance == null)
            {
                instance = this;
            }

            if (!EditorApplication.isPlayingOrWillChangePlaymode && levelSettings != null)
            {
                VisualiseTakingArrow(levelSettings.arrowsPlayer);
                VisualiseTakingArrow(levelSettings.arrowsEnemy);
            }
        }

        private void OnDrawGizmos()
        {
            if (levelSettings != null)
            {
                for (int i = 0; i < levelSettings.arrowsEnemy.Length; i++)
                {
                    Gizmos.DrawWireSphere(levelSettings.arrowsEnemy[i].transform.position, levelSettings.arrowDetectRadiusCalcualted);
                }
            }
        }
        private void VisualiseTakingArrow(Image[] arrows)
        {
            for (int i = 0; i < arrows.Length; i++)
            {
                overlapCircle = Physics2D.OverlapCircleAll(arrows[i].transform.position, levelSettings.arrowDetectRadiusCalcualted, levelSettings.arrowLayer);

                if (overlapCircle.Length != 0)
                {
                    if (overlapCircle[0].TryGetComponent(out Arrow arrow) && TakeSprite != null)
                    {
                        float distance = arrows[i].transform.position.y - arrow.transform.position.y;
                        if (Mathf.Abs(distance) > 0 && Mathf.Abs(distance) < distanceForTakearrow)
                        {
                            saveArrow = arrows[i];

                            saveArrow.sprite = TakeSprite[i];   
                            lastArrowRef.Add(arrow);
                        }

                    }
                }
                else if (lastArrowRef.Count != 0 )
                {
                    for (int j = 0; j < lastArrowRef.Count; j++)
                    {
                        if (lastArrowRef[j].distanceCount == 0)
                        {
                            saveArrow = arrows[i];

                            saveArrow.sprite = DefaultSprite[i];
                            lastArrowRef.RemoveAt(lastArrowRef.Count - 1);
                        }
                        else if (!lastArrowRef[0].IsTakerUnderArrow())
                        {
                            saveArrow = arrows[i];

                            saveArrow.sprite = DefaultSprite[i];
                            lastArrowRef.RemoveAt(lastArrowRef.Count - 1);
                        }
                    }
                }
                else
                {
                    saveArrow = arrows[i];

                    saveArrow.sprite = DefaultSprite[i];
                }
            }
        }

        public void ResetLists()
        {
            lastArrowRef = new List<Arrow>();
        }
#endif
    }
}