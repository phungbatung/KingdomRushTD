using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private Map _map;
    [SerializeField] private Camera _cam;
    private void Awake()
    {
        EventManager.Subscribe(EventID.POINTER_DOWN, PointerDown);
        EventManager.Subscribe(EventID.POINTER_UP, PointerUp);
        InputManager.Instance.cameraController.SetMapSize(_map.GetMapWidth(), _map.GetMapHeight());
    }
        
    public void PointerDown(EventData eventData)
    {
        Debug.Log($"PointerDown: {eventData.Get<Vector3>()}");
    }
    public void PointerUp(EventData eventData)
    {
        Debug.Log($"PointerUp: {eventData.Get<Vector3>()}");
    }

    public Camera GetMainCamera() => _cam;

}
