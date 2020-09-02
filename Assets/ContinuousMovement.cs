using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
public class ContinuousMovement : MonoBehaviour
{
    // Start is called before the first frame update
    public XRNode rotationInputSource;
    public XRNode movementInputSource;
    public float mouseSensitivity = 100f;
    public float gravity = 9.8f;
    public float speed = 1f;
    public float additionalHeight = 0.2f;
    float xRotation = 0f;
    private Vector2 movementInputAxis;
    private Vector2 rotationInputAxis;
    private XRRig rig;
    private CharacterController character;
    private float fallingSpeed;
    void Start()
    {
        character = GetComponent<CharacterController>();
        rig = GetComponent<XRRig>();
    }

    // Update is called once per frame
    void Update()
    {
        InputDevice movementDevice = InputDevices.GetDeviceAtXRNode(movementInputSource);
        movementDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out movementInputAxis);

        InputDevice rotationDevice = InputDevices.GetDeviceAtXRNode(rotationInputSource);
        rotationDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out rotationInputAxis);

    }

    private void FixedUpdate()
    {
        CapsuleFollowHeadset();
        // // rotation
        // float mouseX = rotationInputAxis.x * mouseSensitivity * Time.deltaTime;
        // transform.Rotate(Vector3.up * mouseX);


        // movement
        Quaternion headYaw = Quaternion.Euler(0, rig.cameraGameObject.transform.eulerAngles.y, 0);

        Vector3 direction = headYaw * new Vector3(movementInputAxis.x, 0, movementInputAxis.y);
        character.Move(direction * Time.fixedDeltaTime * speed);

        if (!character.isGrounded)
        {
            //gravity
            fallingSpeed -= gravity * Time.fixedDeltaTime;
        }
        else
        {
            fallingSpeed = 0.0f;
        }
        character.Move(Vector3.up * fallingSpeed * Time.fixedDeltaTime);
    }

    void CapsuleFollowHeadset()
    {
        character.height = rig.cameraInRigSpaceHeight + additionalHeight;
        Vector3 capsuleCenter = transform.InverseTransformPoint(rig.cameraGameObject.transform.position);
        character.center = new Vector3(capsuleCenter.x, character.height / 2 + character.skinWidth, capsuleCenter.z);
    }
}
