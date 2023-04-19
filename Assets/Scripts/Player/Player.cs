using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Player : NetworkBehaviour
{
    [SerializeField]
    private int maxHealth = 100;
    [SerializeField]
    private Behaviour[] componentsToDisable;    //�����������Ҫ�����õ����
    private bool[] componentsEnabled;   //������״̬�ĳ�ʼֵ�����ڸ����ͳһ�ָ�
    private bool colliderEnabled;   // collider���Ǽ̳���Behaviour��������Ҫ����

    // ֻ����server���޸ģ�֮���Զ�ͬ����ÿһ���ͻ���
    private NetworkVariable<int> currentHealth = new NetworkVariable<int>();
    private NetworkVariable<bool> isDead = new NetworkVariable<bool>();

    public void Setup()
    {
        // ��¼������ĳ�ʼ״̬����������������ָ�
        componentsEnabled = new bool[componentsToDisable.Length];
        for (int i = 0; i < componentsToDisable.Length; i ++ )
        {
            componentsEnabled[i] = componentsToDisable[i].enabled;
        }
        Collider col = GetComponent<Collider>();
        colliderEnabled = col.enabled;

        SetDefaults();
    }

    private void SetDefaults()
    {
        // ��Ҹ����֮ǰ�����õ�����ָ���ʼ״̬
        for (int i = 0; i < componentsToDisable.Length; i ++ )
        {
            componentsToDisable[i].enabled = componentsEnabled[i];
        }
        Collider col = GetComponent<Collider>();
        col.enabled = colliderEnabled;

        if (IsServer)
        {
            currentHealth.Value = maxHealth;
            isDead.Value = false;
        }
    }

    // �ܵ����˺�, ֻ���ڷ������˱�����
    public void TakeDamage(int damage)
    {
        if (isDead.Value) return;   // ���˾Ͳ����ܵ��˺�

        currentHealth.Value -= damage;

        if (currentHealth.Value <= 0)
        {
            currentHealth.Value = 0;
            isDead.Value = true;

            DieOnServer();
            DieClientRpc();
        }
    }

    private IEnumerator Respawn() // ����
    {
        yield return new WaitForSeconds(GameManager.Singleton.matchingSettings.respawnTime);    // �ӳ�3�������

        SetDefaults();  // �ָ���ʼ״̬
        
        if (IsLocalPlayer)
        {
            transform.position = new Vector3(0f, 10f, 0f);  // ����λ��������
        }
    }

    private void DieOnServer()
    {
        Die();
    }

    [ClientRpc]
    private void DieClientRpc()
    {
        Die();
    }

    private void Die()
    {
        for (int i = 0; i < componentsToDisable.Length; i ++ )
        {
            componentsToDisable[i].enabled = false;
        }
        Collider col = GetComponent<Collider>();
        col.enabled = false;

        StartCoroutine(Respawn());  // ����һ���µ��߳�ִ����������
    }

    public int GetHealth()
    {
        return currentHealth.Value;
    }
}
