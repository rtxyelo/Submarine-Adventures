using DG.Tweening;
using UnityEngine;


/// <summary>
/// Class for animating the movement of a school of fishes.
/// </summary>
public class SchoolFishAnimation : MonoBehaviour
{
    private float m_minSpeed = 0.5f;
    private float m_maxSpeed = 1.5f;
    private float m_distance = 1f;
    private float m_firstRotationXAxis;
    private float m_secondRotationXAxis;
    private float m_firstRotationZAxis;
    private float m_secondRotationZAxis;

    /// <summary>
    /// Initializes parameters and starts the animation when the object is instantiated.
    /// </summary>
    private void Start()
    {
        // X or Z axis to move
        int directionAxis = Random.Range(0, 2);

        // Positive or negative axis way direction
        bool axisWayDirection = Random.Range(0, 2) == 0 ? false: true;
        m_distance = axisWayDirection ? m_distance : -m_distance;

        // Axis rotation dependent by axisWayDirection
        m_firstRotationXAxis = axisWayDirection ? 180f : 0f;
        m_secondRotationXAxis = axisWayDirection ? 0f : -180f;
        m_firstRotationZAxis = axisWayDirection ? 90f : -90f;
        m_secondRotationZAxis = axisWayDirection ? -90f : 90f;

        if (gameObject.activeSelf)
            SchoolFishAnimationPlay(Random.Range(m_minSpeed, m_maxSpeed), directionAxis);

    }

    /// <summary>
    /// Starts an infinite animation of fish movement.
    /// </summary>
    /// <param name="speed">Animation speed.</param>
    /// <param name="dir">Movement direction (0 - along X, 1 - along Z).</param>
    private void SchoolFishAnimationPlay(float speed, int dir)
    {
        switch (dir)
        {
            case 0:
                {
                    Sequence moveAndRotate = DOTween.Sequence();
                    moveAndRotate.Append(gameObject.transform.DORotate(new Vector3(0f, m_firstRotationXAxis, 0f), 0.15f).SetEase(Ease.Linear));
                    moveAndRotate.Append(gameObject.transform.DOMoveX(gameObject.transform.position.x + m_distance, speed).SetEase(Ease.Linear));
                    moveAndRotate.Append(gameObject.transform.DORotate(new Vector3(0f, m_secondRotationXAxis, 0f), 0.15f).SetEase(Ease.Linear));
                    moveAndRotate.Append(gameObject.transform.DOMoveX(gameObject.transform.position.x, speed).SetEase(Ease.Linear));
                    moveAndRotate.SetLoops(-1, LoopType.Restart);
                    break;
                }
            case 1:
                {
                    Sequence moveAndRotate = DOTween.Sequence();
                    moveAndRotate.Append(gameObject.transform.DORotate(new Vector3(0f, m_firstRotationZAxis, 0f), 0.15f).SetEase(Ease.Linear));
                    moveAndRotate.Append(gameObject.transform.DOMoveZ(gameObject.transform.position.z + m_distance, speed).SetEase(Ease.Linear));
                    moveAndRotate.Append(gameObject.transform.DORotate(new Vector3(0f, m_secondRotationZAxis, 0f), 0.15f).SetEase(Ease.Linear));
                    moveAndRotate.Append(gameObject.transform.DOMoveZ(gameObject.transform.position.z, speed).SetEase(Ease.Linear));
                    moveAndRotate.SetLoops(-1, LoopType.Restart);
                    break;
                }
        }
    }

    /// <summary>
    /// Destroys the object and its child elements when the script is destroyed.
    /// </summary>
    void OnDestroy()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            Transform childTransform = gameObject.transform.GetChild(i);
            Destroy(childTransform.gameObject);
        }
        DOTween.Kill(gameObject.transform, true);
        DOTween.Clear();
    }

}
