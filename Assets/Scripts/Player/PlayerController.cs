using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Rigidbody rb;
    [SerializeField]
    private Camera cam;

    private Vector3 velocity = Vector3.zero; // �ٶȣ�ÿ�����ƶ���λ��
    private Vector3 yRotation = Vector3.zero; // ��ת��ɫ
    private Vector3 xRotation = Vector3.zero; // ��תcamera

    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    public void Rotate(Vector3 _yRotation, Vector3 _xRotation)
    {
        yRotation = _yRotation;
        xRotation = _xRotation;
    }

    private void PerformMovement()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }
    }

    private void PerformRotation()
    {
        if (yRotation != Vector3.zero)
        {
            //rb.MoveRotation() ��Ҫ��һ����Ԫ��
            rb.transform.Rotate(yRotation);
        }
        
        if (xRotation != Vector3.zero)
        {
            cam.transform.Rotate(xRotation);
        }
    }

    private void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }
}
