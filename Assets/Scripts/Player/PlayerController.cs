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

    private float cameraRotationTotal = 0f; // �ۼ�ת�˶��ٶ�
    [SerializeField]
    private float cameraRotationLimit = 85f;

    private Vector3 thrusterForce = Vector3.zero; //���ϵ�����

    public void Move(Vector3 _velocity)
    {
        velocity = _velocity;
    }

    public void Rotate(Vector3 _yRotation, Vector3 _xRotation)
    {
        yRotation = _yRotation;
        xRotation = _xRotation;
    }

    public void Thrust(Vector3 _thrusterForce)
    {
        thrusterForce = _thrusterForce;
    }

    private void PerformMovement()
    {
        if (velocity != Vector3.zero)
        {
            rb.MovePosition(rb.position + velocity * Time.fixedDeltaTime);
        }

        if (thrusterForce != Vector3.zero)
        {
            rb.AddForce(thrusterForce); // ����Time.FixedDeltaTime�룺0.02��
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
            cameraRotationTotal += xRotation.x;
            cameraRotationTotal = Mathf.Clamp(cameraRotationTotal, -cameraRotationLimit, cameraRotationLimit);  //С����Сֵ�͸���Ϊ��Сֵ�� �������ֵ�͸���Ϊ���ֵ
            cam.transform.localEulerAngles = new Vector3(cameraRotationTotal, 0f, 0f);
        }
    }

 

    private void FixedUpdate()
    {
        PerformMovement();
        PerformRotation();
    }
}
