using UnityEngine;
using DG.Tweening;


/// <summary>
/// Handles the animation of individual fish using DOTween.
/// </summary>
public class FishAnimation : MonoBehaviour
{
    public float tailRotationAmount = 30f;

    void Start()
    {
        if (gameObject.activeSelf)
            FishAnimationPlay();
    }

    private void FishAnimationPlay()
    {
        Sequence tailRotation = DOTween.Sequence();
        tailRotation.Append(gameObject.transform.DOLocalRotate(new Vector3(0f, gameObject.transform.localRotation.y + tailRotationAmount, 0f), 0.25f, RotateMode.FastBeyond360).SetEase(Ease.Linear));
        tailRotation.Append(gameObject.transform.DOLocalRotate(new Vector3(0f, gameObject.transform.localRotation.y - 2 * tailRotationAmount, 0f), 0.25f, RotateMode.FastBeyond360).SetEase(Ease.Linear));
        tailRotation.SetLoops(-1, LoopType.Yoyo);
    }

    void OnDestroy()
    {
        DOTween.Kill(gameObject.transform, true);
        DOTween.Clear();
    }
}