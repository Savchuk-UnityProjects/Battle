using UnityEngine;

public class SettingCameraSize : MonoBehaviour
{
    [SerializeField] private Camera TheCamera;
    [SerializeField] private GameField GameField;
    [SerializeField] private float WidthOfAllContent;
    
    private float HeightOfAllContent;

    private void Start()
    {
        HeightOfAllContent = GameField.SizeOfTheWholeMap.y;

        TheCamera.orthographicSize = 1;
        Vector2 SizeOfCameraWindow = TheCamera.ViewportToWorldPoint(new Vector3(1, 1, TheCamera.farClipPlane)) * 2;
        float NewSizeOfCamera;
        if (WidthOfAllContent / HeightOfAllContent < SizeOfCameraWindow.x / SizeOfCameraWindow.y)
        {
            NewSizeOfCamera = HeightOfAllContent / SizeOfCameraWindow.y;
        }
        else
        {
            NewSizeOfCamera = WidthOfAllContent / SizeOfCameraWindow.x;
        }
        TheCamera.orthographicSize = NewSizeOfCamera + 0.3f;
    }
}