using FridayNightFunkin.Editor.TimeLineEditor;
using FridayNightFunkin.GamePlay;
using FridayNightFunkin.Settings;
using UnityEngine;

namespace FridayNightFunkin.Editor
{
    [ExecuteInEditMode]
    public class EditModeArrowTaker : MonoBehaviour
    {
        public float distanceForTakearrow;
        public static EditModeArrowTaker instance;

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
        private ArrowTaker arrowTaker { 
            get { 
                if (!_arrowTaker) 
                    _arrowTaker = GetComponent<ArrowTaker>();
                return _arrowTaker; 
            }
        }

        private float arrowDetectRadiusCalcualted;

        private void Update()
        {
            arrowDetectRadiusCalcualted = arrowDetectRadius * (Camera.main.orthographicSize / 5);
            if (instance == null)
            {
                instance = this;
            }
            VisualiseTakingArrow();
        }

        private void OnDrawGizmos()
        {
            arrowDetectRadiusCalcualted = arrowDetectRadius * (Camera.main.orthographicSize / 5);
            DrawDetectRadius();
        }

        private void DrawDetectRadius()
        {
            Gizmos.color = arrowTaker.GetType() == typeof(ArrowTakerPlayer) ? Color.green : Color.red;
            Gizmos.DrawWireSphere(transform.position, arrowDetectRadiusCalcualted);
        }

        private void VisualiseTakingArrow()
        {
            animator.Update(Time.deltaTime);
            foreach (var arrow in ChartPlayBack.Instance.arrowsList[arrowTaker.roadSide])
            {
                if (arrow.isWork && arrow.gameObject.activeInHierarchy)
                {
                    var distance = Vector2.Distance(transform.position, arrow.transform.position);

                    if (distance <= arrowDetectRadiusCalcualted)
                    {
                        animator.Play("Pressed");
                        break;
                    }
                    else
                    {
                        if (arrow.distanceCount > 0 && arrow.tailDistanceToArrowTakerRaw > 0 && arrow.tailDistance > Mathf.Abs(arrow.tailDistanceToArrowTakerRaw))
                        {
                            animator.Play("Pressed");
                            break;
                        }

                        animator.Play("Idle");
                    }
                }
                else
                {
                    animator.Play("Idle");
                }
            }
        }
    }
}