using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class XRControllerInput : MonoBehaviour
{
    public InputActionProperty leftHandPositionAction;
    public InputActionProperty leftHandRotationAction;
    public InputActionProperty rightHandPositionAction;
    public InputActionProperty rightHandRotationAction;

    public Transform leftHand;
    public Transform rightHand;

    void Update()
    {
        // Update left hand position and rotation
        leftHand.localPosition = leftHandPositionAction.action.ReadValue<Vector3>();
        leftHand.localRotation = leftHandRotationAction.action.ReadValue<Quaternion>();

        // Update right hand position and rotation
        rightHand.localPosition = rightHandPositionAction.action.ReadValue<Vector3>();
        rightHand.localRotation = rightHandRotationAction.action.ReadValue<Quaternion>();
    }
}
