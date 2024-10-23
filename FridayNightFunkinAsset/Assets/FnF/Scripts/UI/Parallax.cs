using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] Transform followingTarget;
    [SerializeField, Range(-1f, 2f)] float parallaxStrenghtOnX;
    [SerializeField, Range(-1f, 2f)] float parallaxStrenghtOnY;
    Vector3 targetPreviousPosition;
    void Start()
    {
        targetPreviousPosition = followingTarget.position;
    }
    void Update()
    {
        var delta = followingTarget.position - targetPreviousPosition;
        targetPreviousPosition = followingTarget.position;
        transform.position += new Vector3(delta.x * parallaxStrenghtOnX, delta.y * parallaxStrenghtOnY, 0);
    }
}