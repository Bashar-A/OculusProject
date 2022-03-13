using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractionManager : MonoBehaviour
{
    [SerializeField] private GameObject rightHandObject = null;
    [SerializeField] private GameObject leftHandObject = null;
    
    [SerializeField] private MeshRenderer indexR = null;
    [SerializeField] private MeshRenderer middleR = null;
    [SerializeField] private MeshRenderer ringR = null;
    [SerializeField] private MeshRenderer pinkyR = null;
    
    [SerializeField] private MeshRenderer indexL = null;
    [SerializeField] private MeshRenderer middleL = null;
    [SerializeField] private MeshRenderer ringL = null;
    [SerializeField] private MeshRenderer pinkyL = null;
    
    private OVRHand _rightHand = null;
    private OVRHand _leftHand = null;
    // Start is called before the first frame update

    enum Fingers
    {
        Index  = OVRPlugin.HandFinger.Index,
        Middle = OVRPlugin.HandFinger.Middle,
        Ring   = OVRPlugin.HandFinger.Ring,
        Pinky  = OVRPlugin.HandFinger.Pinky
    }
    void Start()
    {
        _rightHand = rightHandObject.GetComponent<OVRHand>();
        _leftHand = leftHandObject.GetComponent<OVRHand>();
        Debug.LogWarning($"InteractionManager started");
    }
    
    void LateUpdate()
    {
        var rightHandPinchFinger = -1;
        var leftHandPinchFinger = -1;

        foreach (OVRHand.HandFinger finger in Enum.GetValues(typeof(Fingers)))
        {
            if (rightHandPinchFinger == -1 && _rightHand.GetFingerIsPinching(finger))
            {
                OVRHand.TrackingConfidence confidence = _rightHand.GetFingerConfidence(finger);
                if (confidence == OVRHand.TrackingConfidence.High)
                {
                    rightHandPinchFinger = (int) finger;
                }
            }

            if (leftHandPinchFinger == -1 && _leftHand.GetFingerIsPinching(finger))
            {
                OVRHand.TrackingConfidence confidence = _leftHand.GetFingerConfidence(finger);
                if (confidence == OVRHand.TrackingConfidence.High)
                {
                    leftHandPinchFinger = (int) finger;
                }
            }

            if (leftHandPinchFinger != -1 && rightHandPinchFinger != -1)
            {
                return;
            }
        }
        
        if (leftHandPinchFinger == -1 && rightHandPinchFinger == -1)
        {
            return;
        }

        UpdateSpheres(rightHandPinchFinger, leftHandPinchFinger);

        Debug.LogWarning($"Pinching: {rightHandPinchFinger} AND {leftHandPinchFinger}");
    }

    void UpdateSpheres(int rightHandFinger, int leftHandFinger)
    {
        indexR.material.SetColor("_Color", Color.white);
        middleR.material.SetColor("_Color", Color.white);
        ringR.material.SetColor("_Color", Color.white);
        pinkyR.material.SetColor("_Color", Color.white);
        indexL.material.SetColor("_Color", Color.white);
        middleL.material.SetColor("_Color", Color.white);
        ringL.material.SetColor("_Color", Color.white);
        pinkyL.material.SetColor("_Color", Color.white);

        switch (rightHandFinger)
        {
            case (int) Fingers.Index:
                indexR.material.SetColor("_Color", Color.green);
                break;
            case (int) Fingers.Middle:
                middleR.material.SetColor("_Color", Color.green);
                break;
            case (int) Fingers.Ring:
                ringR.material.SetColor("_Color", Color.green);
                break;
            case (int) Fingers.Pinky:
                pinkyR.material.SetColor("_Color", Color.green);
                break;
            default:
                indexR.material.SetColor("_Color", Color.white);
                middleR.material.SetColor("_Color", Color.white);
                ringR.material.SetColor("_Color", Color.white);
                pinkyR.material.SetColor("_Color", Color.white);
                break;
        }

        switch (leftHandFinger)
        {
            case (int) Fingers.Index:
                indexL.material.SetColor("_Color", Color.green);
                break;
            case (int) Fingers.Middle:
                middleL.material.SetColor("_Color", Color.green);
                break;
            case (int) Fingers.Ring:
                ringL.material.SetColor("_Color", Color.green);
                break;
            case (int) Fingers.Pinky:
                pinkyL.material.SetColor("_Color", Color.green);
                break;
            default:
                indexL.material.SetColor("_Color", Color.white);
                middleL.material.SetColor("_Color", Color.white);
                ringL.material.SetColor("_Color", Color.white);
                pinkyL.material.SetColor("_Color", Color.white);
                break;
        }
    }
}
