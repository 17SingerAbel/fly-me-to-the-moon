using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaverMovement : MonoBehaviour
{
    public float speed = 12f;
    public CharacterController controller;

    float gravity = 9.8f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal") * speed;
        float z = Input.GetAxis("Vertical") * speed;
        Vector3 move = transform.right * x + transform.forward * z;
        if (!controller.isGrounded)
        {
            move = move + new Vector3(0, -gravity, 0f);
        }
        controller.Move(move * speed);
    }
}
