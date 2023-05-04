using UnityEngine;

public class CameraAspectController : MonoBehaviour
{
    private Camera cameraComponent;
    private readonly float fixedAspectRatio = 16f / 9f;

    private void Awake()
    {
        cameraComponent = GetComponent<Camera>();
        cameraComponent.clearFlags = CameraClearFlags.SolidColor;
    }

    private void OnDestroy()
    {
        cameraComponent.clearFlags = CameraClearFlags.SolidColor;
    }

    private void Start()
    {
        AdjustViewportRect();
    }

    private void OnPreCull()
    {
        AdjustViewportRect();
    }

    private void AdjustViewportRect()
    {
        cameraComponent.clearFlags = CameraClearFlags.SolidColor;


        float currentAspectRatio = (float)Screen.width / Screen.height;
        float targetAspectRatio = fixedAspectRatio;

        if (currentAspectRatio > targetAspectRatio)
        {
            float newWidth = targetAspectRatio / currentAspectRatio;
            float xOffset = (1f - newWidth) / 2f;
            cameraComponent.rect = new Rect(xOffset, 0f, newWidth, 1f);
        }
        else
        {
            float newHeight = currentAspectRatio / targetAspectRatio;
            float yOffset = (1f - newHeight) / 2f;
            cameraComponent.rect = new Rect(0f, yOffset, 1f, newHeight);
        }
    }
}
