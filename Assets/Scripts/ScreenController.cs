using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenController : MonoBehaviour
{
    public float sinkSpeed = 1.0f;
    public float maxSinkDepth = 1f;
    Vector3 destination;
    void Start()
    {
        destination = transform.position - Vector3.up * maxSinkDepth;
    }

    void Update()
    {
        if (destination.y < transform.position.y)
        {
            transform.position += -Vector3.up * sinkSpeed * Time.deltaTime;
        }
    }
}
