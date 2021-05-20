using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Vuforia;

public class ARTouchController : MonoBehaviour
{
    public float rotateSpeed = 10f;
    private Vector2 _startingPosition;
    private TrackableBehaviour m_activeTrackable;
    private Rotatable m_interactable;
    private bool hasInteractable;
    private StateManager sm;
    
    // private void Update()
    // {
    //     StateManager sm = TrackerManager.Instance.GetStateManager ();
    //
    //     // Query the StateManager to retrieve the list of
    //     // currently 'active' trackables 
    //     //(i.e. the ones currently being tracked by Vuforia)
    //     var activeTrackable = sm.GetTrackableBehaviours().FirstOrDefault();
    //     if (m_activeTrackable != activeTrackable)
    //     {
    //         m_activeTrackable = activeTrackable;
    //         if(m_activeTrackable != null)
    //             m_interactable = m_activeTrackable.GetComponentInChildren<Rotatable>();
    //         hasInteractable = m_interactable != null;
    //     }
    //     
    //     if (!hasInteractable) return;
    //
    //     if (Input.touchCount <= 0) return;
    //     Touch touch = Input.GetTouch(0);
    //     switch (touch.phase)
    //     {
    //         case TouchPhase.Moved:
    //             var mPosDelta = touch.position - _startingPosition;
    //             transform.Rotate(transform.up, Vector3.Dot(mPosDelta, Camera.main.transform.right), Space.World);
    //             _startingPosition = touch.position;
    //             break;
    //
    //     }
    //
    // }
    
    [SerializeField] private LayerMask targetLayer;
    [SerializeField] private float rotationRate = 3.0f;
    [SerializeField] private bool xRotation;
    [SerializeField] private bool yRotation;
    [SerializeField] private bool touchAnywhere;
    private float m_previousX;
    private float m_previousY;
    private Camera m_camera;
    private bool m_rotating = false;

    private void Awake()
    {
        m_camera = Camera.main;
        sm = TrackerManager.Instance.GetStateManager ();
    }

    private void Update ()
    {
        try
        {
            var activeTrackable = sm.GetTrackableBehaviours().FirstOrDefault();
            m_interactable = activeTrackable.GetComponentInChildren<Rotatable>();
        }
        catch (NullReferenceException)
        {
            return;
        }
        
        if (!touchAnywhere)
        {
            //No need to check if already rotating
            if (!m_rotating)
            {
                RaycastHit hit;
                Ray ray = m_camera.ScreenPointToRay(Input.mousePosition);
                if (!Physics.Raycast(ray, out hit, 1000, targetLayer))
                {
                    return;
                }
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            m_rotating = true;
            m_previousX = Input.mousePosition.x;
            m_previousY = Input.mousePosition.y;
        }
        // get the user touch input
        if(Input.GetMouseButton(0)) 
        {
            var touch = Input.mousePosition;
            var deltaX = -(Input.mousePosition.y - m_previousY) * rotationRate;
            var deltaY = -(Input.mousePosition.x - m_previousX) * rotationRate;
            if(!yRotation) deltaX = 0;
            if(!xRotation) deltaY = 0;

            m_interactable.transform.Rotate (deltaX, deltaY, 0, Space.Self);
            
            m_previousX = Input.mousePosition.x;
            m_previousY = Input.mousePosition.y;
        }
        if (Input.GetMouseButtonUp(0))
            m_rotating = false;
    }
    
}
