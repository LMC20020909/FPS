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
    private Behaviour[] componentsToDisable;    //玩家死亡后需要被禁用的组件
    private bool[] componentsEnabled;   //存放组件状态的初始值，便于复活后统一恢复
    private bool colliderEnabled;   // collider不是继承于Behaviour，所以需要特判

    // 只能在server上修改，之后自动同步到每一个客户端
    private NetworkVariable<int> currentHealth = new NetworkVariable<int>();
    private NetworkVariable<bool> isDead = new NetworkVariable<bool>();
    private NetworkVariable<bool> isInvincible = new NetworkVariable<bool>();
    private NetworkVariable<float> invincibleTime = new NetworkVariable<float>();

    public void Setup()
    {
        // 记录下组件的初始状态，方便死亡重生后恢复
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
        // 玩家复活后之前被禁用的组件恢复初始状态
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

    // 受到了伤害, 只会在服务器端被调用
    public void TakeDamage(int damage)
    {
        if (isDead.Value || isInvincible.Value) return;   // 死了就不再受到伤害

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

    private IEnumerator Respawn() // 重生
    {
        yield return new WaitForSeconds(GameManager.Singleton.matchingSettings.respawnTime);    // 延迟3秒后重生

        SetDefaults();  // 恢复初始状态

        GetComponentInChildren<Animator>().SetInteger("direction", 0);
        GetComponent<Rigidbody>().useGravity = true;

        if (IsLocalPlayer)
        {
            transform.position = new Vector3(0f, 10f, 0f);  // 重生位置在天上
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

        StartCoroutine(Respawn());  // 开启一个新的线程执行重生函数
    }

    public int GetHealth()
    {
        return currentHealth.Value;
    }
}
