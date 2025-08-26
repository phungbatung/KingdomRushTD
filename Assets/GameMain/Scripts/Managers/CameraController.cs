using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _cam;

    private float _mapWidth;
    private float _mapHeight;

    private float _currentScale;
    private float _maxScale;
    private float _minScale;
    private float _padding = 0.01f;
    private void Awake()
    {
        _cam = GetComponent<Camera>();
        _currentScale = 1f;
        _minScale = 0.75f;

    }

    public void SetMapSize(float mapWidth, float mapHeight)
    {
        _mapWidth = mapWidth;
        _mapHeight = mapHeight;
        _currentScale = 1f;
        _minScale = 0.75f;
        _maxScale = Mathf.Min((_mapWidth - 2.0f * _padding) / GetCamWidth(), (_mapHeight - 2.0f * _padding) / GetCamHeight());
        Debug.Log($"Cam: W:{(_mapWidth - 2.0f * _padding)}, H:{_mapHeight - 2.0f * _padding}");
        Debug.Log($"Map: W:{GetCamWidth()}, H:{GetCamHeight()}");

        float height = _cam.orthographicSize * 2f;
        float width = height * _cam.aspect;
        Debug.Log($"CamWidth: {width}, CamHeight: {height}");
    }

    public Camera GetCamera() => _cam;

    public float GetCamHeight() => GetCamera().orthographicSize * 2f;
    public float GetCamWidth() => GetCamera().aspect * GetCamHeight();


    public void ScaleCamera(float scale)
    {
        float tempScale = _currentScale * scale;
        tempScale = Mathf.Clamp(tempScale, _minScale + 0.01f, _maxScale-0.01f);

        GetCamera().orthographicSize = GetCamera().orthographicSize * (tempScale / _currentScale);
        _currentScale = tempScale;
        ClampPosition();
    }

    public void MoveCamera(Vector3 direction)
    {
        GetCamera().transform.position += direction;
        //Debug.Log($"Before clamp: {GetCamera().transform.position}");
        ClampPosition();
        //Debug.Log($"After clamp: {GetCamera().transform.position}");
    }    

    public void ClampPosition()
    {
        float right = _mapWidth / 2f - (_padding + GetCamWidth() / 2f);
        float left = -right;
        float top = _mapHeight / 2f - (_padding + GetCamHeight() / 2f);
        float bottom = -top;

        if (right < left || top < bottom)
        {
            Debug.LogError($"[CameraController]Camera size is bigger than Map size!!!");
            return;
        }    

        Vector3 camPosition = GetCamera().transform.position;
        camPosition.x = Mathf.Clamp(camPosition.x, left, right);
        camPosition.y = Mathf.Clamp(camPosition.y, bottom, top);

        GetCamera().transform.position = camPosition;
    }
}
