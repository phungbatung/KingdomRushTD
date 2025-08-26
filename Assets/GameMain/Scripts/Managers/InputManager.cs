using UnityEngine;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance;

    public CameraController cameraController;

    private bool _isZooming;
    private bool _canMove;
    private float _touchDistance;
    private Vector3 _touchPosition;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
        _canMove = true;
    }


    private void Update()
    {
#if UNITY_ANDROID
        HandleInputMobile(); 
#endif

#if UNITY_EDITOR
        HandleInputPC();
#endif
    }


    private void SetCanMove(bool canMove)
    {
        _canMove = canMove;
    }
    private void HandleMoveCamera()
    {
        Vector3 diff = _touchPosition - cameraController.GetCamera().ScreenToWorldPoint(Input.mousePosition);
        cameraController.MoveCamera(diff);
        _touchPosition = cameraController.GetCamera().ScreenToWorldPoint(Input.mousePosition);

    }

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
    private void HandleInputPC()
    {
        HandleZoomOnPC();
        if (Input.GetMouseButtonDown(0))
        {
            HandleTouchInputPC();
            EventManager.TriggerEvent(EventID.POINTER_DOWN, _touchPosition);
        }

        if (Input.GetMouseButton(0))
        {
            if (_canMove)
            {
                HandleMoveCamera();
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            HandleTouchInputPC();
            _canMove = true;
            EventManager.TriggerEvent(EventID.POINTER_UP, _touchPosition);
        }
    }
    private void HandleZoomOnPC()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        cameraController.ScaleCamera(1f - scroll);
    }
    private void HandleTouchInputPC()
    {
        _touchPosition = cameraController.GetCamera().ScreenToWorldPoint(Input.mousePosition);
    }

#endif
   
//#if UNITY_ANDROID
    private void HandleInputMobile()
    {
        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (touch.phase == TouchPhase.Began)
            {
                // X? lý khi touch b?t ??u
                HandleTouchInputMobile();
                EventManager.TriggerEvent(EventID.POINTER_DOWN, _touchPosition);
            }
        }
        //if (Input.GetMouseButtonDown(0))
        //{
        //    HandleTouchInputMobile();
        //    EventManager.TriggerEvent(EventID.POINTER_DOWN, _touchPosition);
        //}

        if (Input.touchCount>0)
        {
            if (_isZooming)
            {
                HandleZoomOnMobile();
            }
            if (Input.touchCount == 1 && _canMove)
            {
                HandleMoveCamera();
            }
        }
        if (Input.GetMouseButtonUp(0))
        {
            HandleTouchInputMobile();
            EventManager.TriggerEvent(EventID.POINTER_UP, _touchPosition);
        }
    }
    private void HandleTouchInputMobile()
    {
        if (Input.touchCount == 1)
        {
            _touchPosition = cameraController.GetCamera().ScreenToWorldPoint(Input.GetTouch(0).position);
            _canMove = true;
        }

        if (Input.touchCount == 2)
        {
            _isZooming = true;
            _touchDistance = Vector3.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
        }
        else
        {
            _isZooming = false;
        }
    }
    private void HandleZoomOnMobile()
    {
        float tempTouchDistance = Vector3.Distance(Input.GetTouch(0).position, Input.GetTouch(1).position);
        float scale = tempTouchDistance / _touchDistance;

        cameraController.ScaleCamera(scale);
    }
//#endif
    
}
