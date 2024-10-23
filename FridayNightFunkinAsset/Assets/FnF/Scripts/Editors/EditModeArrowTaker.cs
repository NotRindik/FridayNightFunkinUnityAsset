using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace FridayNightFunkin.Editor
{
    [ExecuteInEditMode]
    public class EditModeArrowTaker : MonoBehaviour
    {
        public float distanceForTakearrow;
        public static EditModeArrowTaker instance;

        public Sprite[] TakeSprite;
        public Sprite[] DefaultSprite;

        private LevelSettings levelSettings => LevelSettings.instance;

        [SerializeField] private float arrowDetectRadius = 0.8f;

        private float arrowDetectRadiusCalcualted;

#if UNITY_EDITOR
        [ExecuteInEditMode]

        private void Update()
        {
            arrowDetectRadiusCalcualted = arrowDetectRadius * (Camera.main.orthographicSize / 5);
            if (instance == null)
            {
                instance = this;
            }

            if (!EditorApplication.isPlayingOrWillChangePlaymode && levelSettings != null)
            {
                VisualiseTakingArrow(levelSettings.arrowsPlayer, CharacterSide.Player);
                VisualiseTakingArrow(levelSettings.arrowsEnemy, CharacterSide.Enemy);
            }
        }

        private void OnDrawGizmos()
        {
            arrowDetectRadiusCalcualted = arrowDetectRadius * (Camera.main.orthographicSize / 5);
            if (levelSettings != null)
            {
                DrawDetectRadius(levelSettings.arrowsPlayer,Color.green);
                DrawDetectRadius(levelSettings.arrowsEnemy,Color.red);
            }
        }

        private void DrawDetectRadius(Image[] arrowTakers,Color color)
        {
            for (int i = 0; i < arrowTakers.Length; i++)
            {
                Gizmos.color = color;
                Gizmos.DrawWireSphere(arrowTakers[i].transform.position, arrowDetectRadiusCalcualted);
            }
        }

        private void VisualiseTakingArrow(Image[] arrowTakers, CharacterSide characterSide)
        {
            for (int i = 0; i < arrowTakers.Length; i++)
            {
                foreach (var arrow in levelSettings.arrowsList)
                {
                    if (arrow.characterSide != characterSide) continue;

                    if (i == (int)arrow.arrowSide && arrow.isWork && arrow.gameObject.activeInHierarchy)
                    { 
                        var distance = Vector2.Distance(arrowTakers[i].transform.position, arrow.transform.position);

                        if (distance <= arrowDetectRadiusCalcualted)
                        {
                            arrowTakers[i].sprite = TakeSprite[i];
                            break;
                        }
                        else
                        {
                            if (arrow.distanceCount > 0 && arrow.tailDistanceToArrowTakerRaw > 0 && arrow.tailDistance > Mathf.Abs(arrow.tailDistanceToArrowTakerRaw))
                            {
                                arrowTakers[i].sprite = TakeSprite[i];
                                break;
                            }

                            arrowTakers[i].sprite = DefaultSprite[i];
                        }
                    }
                    else
                    {
                        arrowTakers[i].sprite = DefaultSprite[i];
                    }
                }
            }
        }
#endif
    }
}