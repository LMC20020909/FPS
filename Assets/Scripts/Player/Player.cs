using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.VisualScripting;
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
    private NetworkVariable<bool> isInvincible = new NetworkVariable<bool>();
    private NetworkVariable<float> invincibleTime = new NetworkVariable<float>();

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
            isInvincible.Value = true;
            invincibleTime.Value = 0f;
        }
    }

    private void Update()
    {
        if (IsServer && isInvincible.Value)
        {
            invincibleTime.Value += Time.deltaTime;
            if (invincibleTime.Value > 5f)
            {
                isInvincible.Value = false;
                invincibleTime.Value = 0f;
            }
        }
    }

    public bool IsDead()
    {
        return isDead.Value;
    }

    // �ܵ����˺�, ֻ���ڷ������˱�����
    public void TakeDamage(int damage)
    {
        if (isDead.Value || isInvincible.Value) return;   // ���˾Ͳ����ܵ��˺�

        currentHealth.Value -= damage;

        if (currentHealth.Value <= 0)
        {
            currentHealth.Value = 0;
            isDead.Value = true;

            if (!IsHost)
            {
                DieOnServer();
            }
            DieClientRpc();
        }
    }

    private IEnumerator Respawn() // ����
    {
        yield return new WaitForSeconds(GameManager.Singleton.matchingSettings.respawnTime);    // �ӳ�3�������

        SetDefaults();  // �ָ���ʼ״̬

        GetComponentInChildren<Animator>().SetInteger("direction", 0);
        GetComponent<Rigidbody>().useGravity = true;

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
        GetComponentInChildren<Animator>().SetInteger("direction", -1);
        GetComponentInChildren<Animator>().SetBool("isHitted", false);
        GetComponent<Rigidbody>().useGravity = false;

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
