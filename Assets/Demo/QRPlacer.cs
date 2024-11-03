using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

public class QRPlacer : MonoBehaviour
{

    public event Action Changed;

    [SerializeField] private QR _prefab;
    [SerializeField] private Texture2D _qrTextureA;
    [SerializeField] private Texture2D _qrTextureB;
    [SerializeField] private ARRaycastManager _arRaycastManager;
    [SerializeField] private ARPlaneManager _arPlaneManager;
    [SerializeField] private QRDemoDebug _debugger;

    private readonly List<ARRaycastHit> _hits = new List<ARRaycastHit>();

    private GameObject[] _qrs = new GameObject[2];
    private int _nextInteractionIndex = 0;

    public GameObject QRA => _qrs[0];
    public GameObject QRB => _qrs[1];

    private void OnEnable()
    {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += OnFingerDown;
    }

    private void OnDisable()
    {
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
        EnhancedTouch.Touch.onFingerDown -= OnFingerDown;
    }

    private void OnFingerDown(EnhancedTouch.Finger finger)
    {
        _debugger.Push("OnFingerDown");

        //if (finger.index != 0)
        //    return;
        //
        //_debugger.Push("Index is not zero");

        if (!_arRaycastManager.Raycast(finger.currentTouch.screenPosition, _hits, TrackableType.PlaneWithinPolygon))
            return;

        _debugger.Push($"Raycast hit ({_hits.Count})");

        foreach (var hit in _hits)
        {
            bool isFloor = _arPlaneManager.GetPlane(hit.trackableId).alignment == PlaneAlignment.HorizontalUp;

            if (!isFloor)
                continue;

            Interact(hit.pose);
            break;
        }

        _hits.Clear();
    }

    private void Interact(Pose pose)
    {
        _debugger.Push($"Interact");

        bool isSpawned = _qrs[_nextInteractionIndex] != null;

        if (isSpawned)
        {
            _debugger.Push($"Moved");
            _qrs[_nextInteractionIndex].transform.SetPositionAndRotation(pose.position, pose.rotation);
        }
        else
        {
            _debugger.Push($"Spawned");
            var qrCode = Instantiate(_prefab, pose.position, pose.rotation);
            var qrTexture = _nextInteractionIndex == 0 ? _qrTextureA : _qrTextureB;
            qrCode.Setup(qrTexture);
            _qrs[_nextInteractionIndex] = qrCode.gameObject;
        }

        _nextInteractionIndex += 1;
        if (_nextInteractionIndex > _hits.Count) 
            _nextInteractionIndex = 0;

        Changed?.Invoke();
    }

}
