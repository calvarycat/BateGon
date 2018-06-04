using System;
using UnityEngine;
using UnityEngine.EventSystems;

[AddComponentMenu("Kinect/Kinect Input Module")]
[RequireComponent(typeof(EventSystem))]
public class KinectInputModule : BaseInputModule
{
    private static KinectInputModule _instance;

    public static KinectInputModule Instance
    {
        get
        {
            if (!_instance)
            {
                _instance = FindObjectOfType(typeof(KinectInputModule)) as KinectInputModule;

                if (!_instance)
                {
                    if (EventSystem.current)
                    {
                        EventSystem.current.gameObject.AddComponent<KinectInputModule>();
                        Debug.LogWarning("Add Kinect Input Module to your EventSystem!");
                    }
                    else
                    {
                        Debug.LogWarning("Create your UI first");
                    }
                }
            }

            return _instance;
        }
    }

    public KinectInputData[] InputData = new KinectInputData[0];

    [SerializeField]
    public bool AllowUpdate = true;

    [SerializeField]
    public Canvas TargetCanvas;

    [SerializeField]
    private float _scrollTreshold = 0.5f;

    [SerializeField]
    private float _scrollSpeed = 3.5f;

    [SerializeField]
    private float _waitOverTime = 3f;

    private PointerEventData _handPointerData;

    // get a pointer event data for a screen position
    private PointerEventData GetLookPointerEventData(Vector3 componentPosition)
    {
        if (_handPointerData == null)
        {
            _handPointerData = new PointerEventData(eventSystem);
        }

        _handPointerData.Reset();
        _handPointerData.delta = Vector2.zero;
        _handPointerData.position = componentPosition;
        _handPointerData.scrollDelta = Vector2.zero;

        eventSystem.RaycastAll(_handPointerData, m_RaycastResultCache);
        _handPointerData.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
        m_RaycastResultCache.Clear();

        return _handPointerData;
    }

    protected virtual void Update()
    {
        if (AllowUpdate)
            UpdateHandState();
    }

    public override void Process()
    {
        if (AllowUpdate)
        {
            ProcessHover();
            ProcessPress();
            ProcessDrag();
            ProcessWaitOver();
        }
    }

    private void UpdateHandState()
    {
        for (int i = 0; i < InputData.Length; i++)
        {
            InputData[i].UpdateComponent();
        }
    }

    /// <summary>
    ///     Process hovering over component, sends pointer enter exit event to gameobject
    /// </summary>
    private void ProcessHover()
    {
        for (int i = 0; i < InputData.Length; i++)
        {
            PointerEventData pointer = GetLookPointerEventData(InputData[i].GetScreenPosition());
            var obj = _handPointerData.pointerCurrentRaycast.gameObject;
            HandlePointerExitAndEnter(pointer, obj);

            // Hover update
            InputData[i].IsHovering = obj != null;
            InputData[i].HoveringObject = obj;
        }
    }

    /// <summary>
    ///     Process pressing, event click trigered on button by closing and opening hand,sends submit event to gameobject
    /// </summary>
    private void ProcessPress()
    {
        for (int i = 0; i < InputData.Length; i++)
        {
            //Check if we are tracking hand state not wait over
            if (!InputData[i].IsHovering || InputData[i].ClickGesture != KinectUIClickGesture.HandState)
                continue;

            // If hand state is not tracked reset properties
            if (InputData[i].CurrentHandState == KinectInterop.HandState.NotTracked)
            {
                if (InputData[i].CurrentBufferTime <= 0)
                {
                    InputData[i].IsPressing = false;
                    InputData[i].IsDraging = false;
                }
                else
                {
                    continue;
                }
            }

            // When we close hand and we are not pressing set property as pressed
            if (!InputData[i].IsPressing && InputData[i].CurrentHandState == KinectInterop.HandState.Closed)
            {
                InputData[i].IsPressing = true;
            }
            // If hand state is opened and is pressed, make click action
            else if (InputData[i].IsPressing && InputData[i].CurrentHandState == KinectInterop.HandState.Open)
            {
                PointerEventData lookData = GetLookPointerEventData(InputData[i].GetScreenPosition());
                eventSystem.SetSelectedGameObject(null);

                if (lookData.pointerCurrentRaycast.gameObject != null && !InputData[i].IsDraging)
                {
                    GameObject go = lookData.pointerCurrentRaycast.gameObject;
                    ExecuteEvents.ExecuteHierarchy(go, lookData, ExecuteEvents.submitHandler);
                }

                InputData[i].IsPressing = false;
            }
        }
    }

    private void ProcessDrag()
    {
        for (int i = 0; i < InputData.Length; i++)
        {
            // if not pressing we can't drag
            if (!InputData[i].IsPressing)
                continue;

            // Check if we reach drag treshold for any axis, temporary position set when we press an object
            if (Mathf.Abs(InputData[i].TempHandPosition.x - InputData[i].HandPosition.x) > _scrollTreshold ||
                Mathf.Abs(InputData[i].TempHandPosition.y - InputData[i].HandPosition.y) > _scrollTreshold)
            {
                InputData[i].IsDraging = true;
            }
            else
            {
                InputData[i].IsDraging = false;
            }

            // If dragging use unit's eventhandler to send an event to a scrollview like component
            if (InputData[i].IsDraging)
            {
                PointerEventData lookData = GetLookPointerEventData(InputData[i].GetScreenPosition());
                eventSystem.SetSelectedGameObject(null);
                GameObject go = lookData.pointerCurrentRaycast.gameObject;

                PointerEventData pEvent = new PointerEventData(eventSystem);
                pEvent.dragging = true;
                pEvent.scrollDelta = (InputData[i].TempHandPosition - InputData[i].HandPosition) * _scrollSpeed;
                pEvent.useDragThreshold = true;

                ExecuteEvents.ExecuteHierarchy(go, pEvent, ExecuteEvents.scrollHandler);
            }
        }
    }

    /// <summary>
    ///     Processes waitint over componens, if hovererd buttons click type is waitover, process it!
    /// </summary>
    private void ProcessWaitOver()
    {
        for (int i = 0; i < InputData.Length; i++)
        {
            if (!InputData[i].IsHovering || InputData[i].ClickGesture != KinectUIClickGesture.WaitOver)
                continue;

            float waitOverTime = _waitOverTime;
            if (InputData[i].OverrideWaitOverTime > 0)
                waitOverTime = InputData[i].OverrideWaitOverTime;

            InputData[i].WaitOverAmount = (Time.time - InputData[i].HoverTime) / waitOverTime;

            if (Time.time >= InputData[i].HoverTime + waitOverTime)
            {
                PointerEventData lookData = GetLookPointerEventData(InputData[i].GetScreenPosition());
                GameObject go = lookData.pointerCurrentRaycast.gameObject;
                ExecuteEvents.ExecuteHierarchy(go, lookData, ExecuteEvents.submitHandler);

                // reset time
                InputData[i].HoverTime = Time.time;
            }
        }
    }

    /// <summary>
    ///     Used from UI hand cursor components
    /// </summary>
    /// <param name="handType"></param>
    /// <returns></returns>
    public KinectInputData GetHandData(KinectUIHandType handType)
    {
        for (int i = 0; i < InputData.Length; i++)
        {
            if (InputData[i].HandType == handType)
                return InputData[i];
        }

        return null;
    }
}

[Serializable]
public class KinectInputData
{
    public readonly float BufferTime = 0.1f;
    public readonly float SmoothTime = 0.05f;

    // Which hand we are tracking
    public KinectUIHandType HandType = KinectUIHandType.Right;

    public KinectInterop.JointType HandTypeKinect
    {
        get
        {
            if (HandType == KinectUIHandType.Right)
                return KinectInterop.JointType.HandRight;
            return KinectInterop.JointType.HandLeft;
        }
    }

    // Is hand in pressing condition
    private bool _isPressing;

    // Hovering Gameobject, needed for WaitOver like clicking detection
    private GameObject _hoveringObject;

    // Hovering Gameobject getter setter, needed for WaitOver like clicking detection
    public GameObject HoveringObject
    {
        get { return _hoveringObject; }
        set
        {
            if (value != _hoveringObject)
            {
                HoverTime = Time.time;
                _hoveringObject = value;

                if (_hoveringObject == null)
                {
                    OverrideWaitOverTime = 0;
                    return;
                }

                KinectUIWaitOverButton overButton = _hoveringObject.GetComponent<KinectUIWaitOverButton>();

                if (overButton)
                {
                    ClickGesture = KinectUIClickGesture.WaitOver;
                    OverrideWaitOverTime = overButton.OverrideWaitOverTime;
                }
                else
                {
                    ClickGesture = KinectUIClickGesture.HandState;
                    OverrideWaitOverTime = 0;
                }

                WaitOverAmount = 0f;
            }
        }
    }

    public KinectInterop.HandState CurrentHandState { get; private set; }

    // Click gesture of button
    public KinectUIClickGesture ClickGesture { get; private set; }

    // Is this hand over a UI component
    public bool IsHovering { get; set; }

    // Is hand dragging a component
    public bool IsDraging { get; set; }

    // Is hand pressing a button
    public bool IsPressing
    {
        get { return _isPressing; }
        set
        {
            _isPressing = value;
            if (_isPressing)
                TempHandPosition = HandPosition;
        }
    }

    // Global position of tracked hand
    public Vector3 HandPosition { get; private set; }

    // Temporary hand position of hand, used for draging check
    public Vector3 TempHandPosition { get; private set; }

    // Hover start time, used for waitover type buttons
    public float HoverTime { get; set; }

    // Amout of wait over , between 1 - 0 , when reaches 1 button is clicked
    public float WaitOverAmount { get; set; }

    public float OverrideWaitOverTime { get; private set; }

    public float CurrentBufferTime { get; private set; }

    // Must be called for each hand 
    public void UpdateComponent()
    {
        long userId = KinectManager.Instance.GetUserIdByIndex(0);

        if (HandType == KinectUIHandType.Left)
        {
            CurrentHandState = KinectManager.Instance.GetLeftHandState(userId);
        }
        else if (HandType == KinectUIHandType.Right)
        {
            CurrentHandState = KinectManager.Instance.GetRightHandState(userId);
        }

        if (CurrentHandState != KinectInterop.HandState.NotTracked)
        {
            Vector3 newHandPosition = KinectManager.Instance.GetJointPosColorOverlay(userId,
                (int)HandTypeKinect, Camera.main, KinectInputModule.Instance.TargetCanvas.pixelRect);

            if (newHandPosition != Vector3.zero)
            {
                Vector3 velocity = Vector3.zero;
                HandPosition = Vector3.SmoothDamp(HandPosition, newHandPosition, ref velocity, SmoothTime);
            }

            CurrentBufferTime = BufferTime;
        }
        else
        {
            if (CurrentBufferTime > 0)
                CurrentBufferTime -= Time.deltaTime;
        }
    }

    // Converts hand position to screen coordinates
    public Vector3 GetScreenPosition()
    {
        return Camera.main.WorldToScreenPoint(HandPosition);
    }

    // Converts hand position to canvas coordinates
    public Vector3 GetCanvasPosition()
    {
        return Utility.ConvertScreenPositionToCanvasPosition(KinectInputModule.Instance.TargetCanvas,
            GetScreenPosition());
    }
}

public enum KinectUIClickGesture
{
    HandState,
    Push,
    WaitOver
}

public enum KinectUIHandType
{
    Right,
    Left
}