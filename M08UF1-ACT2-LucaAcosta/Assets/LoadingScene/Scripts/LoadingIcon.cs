using UnityEngine;
using DG.Tweening;

public class LoadingIcon : MonoBehaviour
{
    void Start()
    {
        transform.DORotate(Vector3.forward * 360f, 3, RotateMode.FastBeyond360).
            SetEase(Ease.OutBounce).
            SetLoops(-1); // Para que rote infinitamente
    }
}
