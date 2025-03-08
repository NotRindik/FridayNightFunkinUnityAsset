using FridayNightFunkin.Editor.TimeLineEditor;
using FridayNightFunkin.GamePlay;
using FridayNightFunkin.Settings;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

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

        private Image _image;
        public Sprite inactiveState;
        public Sprite activeState;

        private void Update()
        {
            if (!_image)
            {
                _image = GetComponent<Image>();
            }
            
            if (chartPlayBack == null)
            {
                chartPlayBack = FindAnyObjectByType<ChartPlayBack>();
            }
            else
            {
                VisualiseTakingArrow();
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
            foreach (var arrow in chartPlayBack.ChartContainer.arrowsList[arrowTaker.roadSide])
            {
                if (arrow)
                {
                    if (arrow.isWork && arrow.gameObject.activeInHierarchy && arrow.arrowSide == arrowTaker.arrowSide)
                    {
                        var distance = Vector2.Distance(transform.position, arrow.transform.position);

                        if (distance <= arrowDetectRadiusCalcualted)
                        {
                            _image.sprite = activeState;
                            break;
                        }
                        else
                        {
                            if (arrow.distanceCount > 0 && arrow.tailDistanceToArrowTakerRaw > 0 && arrow.tailDistance > Mathf.Abs(arrow.tailDistanceToArrowTakerRaw))
                            {
                                _image.sprite = activeState;
                                break;
                            }

                            _image.sprite = inactiveState;
                        }
                    }
                    else
                    {
                        _image.sprite = inactiveState;
                    }
                }
            }
        }
    }
}