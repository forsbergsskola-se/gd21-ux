using TMPro;
using UnityEngine;

public class FieldOfViewSlider : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private TextMeshProUGUI fieldOfViewText;

    public void SetFieldOfView(float fieldOfView)
    {
        mainCamera.fieldOfView = fieldOfView;
        fieldOfViewText.text = fieldOfView.ToString();
    }
}