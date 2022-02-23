using UnityEngine;

public class LoadingSpinner : MonoBehaviour
{
    [SerializeField] private RectTransform loadingSpinnerTransform;
    [SerializeField] private float spinSpeed = 180f;

    private void Update()
    {
        loadingSpinnerTransform.Rotate(Vector3.forward, spinSpeed * Time.deltaTime);
    }
}