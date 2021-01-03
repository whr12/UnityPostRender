using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;
    public float speed;
    public float range = 2;

    private float current;
    private float direction = 1;

    private void Start()
    {
        if (target)
        {
            transform.position = target.position - new Vector3(10, 0, 0);
            transform.LookAt(target);
        }
    }

    private void Update()
    {
        if (target)
        {
            transform.position += direction * Time.deltaTime * speed * transform.right;

            current += direction * Time.deltaTime * speed;
            if (current > range)
            {
                direction = -1;
            }

            if (current < -range)
            {
                direction = 1;
            }
        }
    }
}
