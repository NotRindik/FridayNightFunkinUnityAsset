using FridayNightFunkin.Editor.TimeLineEditor;
using FridayNightFunkin.GamePlay;
using FridayNightFunkin.Settings;
using UnityEngine;

namespace FridayNightFunkin.Editor
{
    [ExecuteInEditMode]
    public class EditModeArrowTaker : MonoBehaviour
    {

        [SerializeField] private float arrowDetectRadius = 0.8f;

        private Animator _animator;
        private Animator animator
        {
            get
            {
                if (!_animator)
                    _animator = GetComponent<Animator>();
                return _animator;
            }
        }

        private ArrowTaker _arrowTaker;
        private ArrowTaker arrowTaker
        {
            get
            {
                if (!_arrowTaker)
                    _arrowTaker = GetComponent<ArrowTaker>();
                return _arrowTaker;
            }
        }

        public ChartPlayBack chartPlayBack;

        private float arrowDetectRadiusCalcualted => arrowDetectRadius * (Camera.main.orthographicSize / 5);

        private string lastAnim;
        private string currentAnim;

        private void Update()
        {
            if (chartPlayBack == null)
            {
                chartPlayBack = FindAnyObjectByType<ChartPlayBack>();
            }
            else
            {
                VisualiseTakingArrow();
                animator.Update(1f / 60f);
                if (currentAnim != lastAnim)
                {
                    lastAnim = currentAnim;
                    animator.CrossFade(currentAnim, 0.3f);
                }
            }
        }
        private void OnDrawGizmos()
        {
            DrawDetectRadius();
        }

        private void DrawDetectRadius()
        {
            Gizmos.color = arrowTaker.GetType() == typeof(ArrowTakerPlayer) ? Color.green : Color.red;
            Gizmos.DrawWireSphere(transform.position, arrowDetectRadiusCalcualted);
        }

        private void VisualiseTakingArrow()
        {
            foreach (var arrow in chartPlayBack.arrowsList[arrowTaker.roadSide])
            {
                if (arrow.isWork && arrow.gameObject.activeInHierarchy && arrow.arrowSide == arrowTaker.arrowSide)
                {
                    var distance = Vector2.Distance(transform.position, arrow.transform.position);

                    if (distance <= arrowDetectRadiusCalcualted)
                    {
                        currentAnim = "Pressed";
                        break;
                    }
                    else
                    {
                        if (arrow.distanceCount > 0 && arrow.tailDistanceToArrowTakerRaw > 0 && arrow.tailDistance > Mathf.Abs(arrow.tailDistanceToArrowTakerRaw))
                        {
                            currentAnim = "Pressed";
                            break;
                        }

                        currentAnim = "Idle";
                    }
                }
                else
                {
                    currentAnim = "Idle";
                }
            }
        }
    }
}