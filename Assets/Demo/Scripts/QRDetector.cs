using System;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public sealed class QRDetector : MonoBehaviour
{

    public event Action Changed;

    [SerializeField] private ARTrackedImageManager _trackedImageManager;

    public ARTrackedImage QRA;
    public ARTrackedImage QRB;

    private void OnEnable()
    {
        _trackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        _trackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }

    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs change)
    {
        var added = change.added;
        var removed = change.removed;

        bool changed = false;

        foreach (var image in change.added)
        {
            if (QRA == null)
            {
                QRA = image;
                changed = true;
            }
            else if (QRB == null)
            {
                QRB = image;
                changed = true;
            }
        }

        if (change.updated.Count > 0)
        {
            changed = true;
        }

        foreach (var image in change.removed)
        {
            if (image == QRA)
            {
                QRA = null;
            }
            else if (image == QRB)
            {
                QRB = null;
            }
        }

        if (changed)
        {
            Changed?.Invoke();
        }
    }

    private void Update()
    {
        Debug.Log(
            $"There are {_trackedImageManager.trackables.count} images being tracked.");

        foreach (var trackedImage in _trackedImageManager.trackables)
        {
            Debug.Log($"Image: {trackedImage.referenceImage.name} is at " +
                      $"{trackedImage.transform.position}");
        }
    }

}
