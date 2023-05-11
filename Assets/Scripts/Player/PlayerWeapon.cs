using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerWeapon
{
    public string name = "M16A1";
    public int damage = 10;
    public float range = 100f;

    public float shootRate = 10f;   // 1����Դ���ٷ��ӵ��� ���С�ڵ���0����Ϊ����
    public float shootCoolDownTime = 0.75f; // ����ģʽ����ȴʱ��
    public float recoilForce = 2f;  // ������

    public int maxBullets = 30; // ��������
    public int bullets = 30;    // ��ǰ�ӵ�����
    public float reloadTime = 2f;   // ����ʱ��

    [HideInInspector]   // ʹ���б�������unity������չʾ
    public bool isReloading = false;    // �Ƿ����ڻ���

    public GameObject graphics;
}
