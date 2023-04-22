using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerShooting : NetworkBehaviour
{
    private const string PLAYER_TAG = "Player";

    private WeaponManager weaponManager;
    private PlayerWeapon currentWeapon;

    private float ShootCoolDownTime = 0f;   // 距离上次开枪过了多久（秒）
    private int autoShootCount = 0; // 当前一共连开多少枪

    [SerializeField]
    private LayerMask mask;

    private Camera cam;
    private PlayerController playerController;

    enum HitEffectMaterial
    {
        Metal,
        Stone,
    }

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponentInChildren<Camera>();
        weaponManager = GetComponent<WeaponManager>();
        playerController = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        ShootCoolDownTime += Time.deltaTime;

        if (!IsLocalPlayer) return;

        currentWeapon = weaponManager.GetCurrentWeapon();

        if (currentWeapon.shootRate <= 0)   // 单发
        {
            if (Input.GetButtonDown("Fire1") && ShootCoolDownTime >= currentWeapon.shootCoolDownTime)
            {
                autoShootCount = 0;
                Shoot();
                ShootCoolDownTime = 0f; // 重置冷却时间
            }
        } else    // 连发
        {
            if (Input.GetButtonDown("Fire1"))
            {
                autoShootCount = 0;
                // 重复执行一个函数，第一个参数为函数名，第二个参数为什么时候执行第一次， 第三个参数为两次执行的时间间隔
                InvokeRepeating("Shoot", 0f, 1f / currentWeapon.shootRate);
            } else if (Input.GetButtonUp("Fire1") || Input.GetKeyDown(KeyCode.Q))   // 松开鼠标左键或者切换武器，停止连发
            {
                CancelInvoke("Shoot");
            }
        }
        
    }

    private void OnHit(Vector3 pos, Vector3 normal, HitEffectMaterial material) // 击中点的特效
    {
        GameObject hitEffectPrefab;
        if (material == HitEffectMaterial.Metal)
        {
            hitEffectPrefab = weaponManager.GetCurrentGraphics().metalHitEffectPrefab;
        } else
        {
            hitEffectPrefab = weaponManager.GetCurrentGraphics().stoneHitEffectPrefab;
        }

        GameObject hitEffectObject = Instantiate(hitEffectPrefab, pos, Quaternion.LookRotation(normal));    // 击中特效方向为法线反方向
        ParticleSystem particleSystem = hitEffectObject.GetComponent<ParticleSystem>();
        particleSystem.Emit(1); // 直接触发，没有延迟
        particleSystem.Play();
        Destroy(hitEffectObject, 1f);
    }

    [ServerRpc]
    private void OnHitServerRpc(Vector3 pos, Vector3 normal, HitEffectMaterial material)
    {
        if (!IsHost)
        {
            OnHit(pos, normal, material);
        }
        OnHitClientRpc(pos, normal, material);
    }

    [ClientRpc]
    private void OnHitClientRpc(Vector3 pos, Vector3 normal, HitEffectMaterial material)
    {
        OnHit(pos, normal, material);
    }

    private void Onshoot(float recoilForce)  // 每次射击相关的逻辑，包括特效、声音等
    {
        weaponManager.GetCurrentGraphics().muzzleFlash.Play();
        weaponManager.GetCurrentAudioSource().Play();

        if (IsLocalPlayer)  //施加后坐力
        {
            playerController.AddRecoilForce(recoilForce);
        }
    }

    [ClientRpc]
    private void OnshootClientRpc(float recoilForce)
    {
        Onshoot(recoilForce);
    }

    [ServerRpc]
    private void OnshootServerRpc(float recoilForce)
    {
        if (!IsHost)
        {
            Onshoot(recoilForce);
        }
        OnshootClientRpc(recoilForce);
    }

    private void NpcOnHit(GameObject npc)
    {
        if (npc.GetComponentInChildren<SkinnedMeshRenderer>().GetBlendShapeWeight(8) == 0)
        {
            npc.GetComponentInChildren<SkinnedMeshRenderer>().SetBlendShapeWeight(8, 100f);
            npc.GetComponentInChildren<SkinnedMeshRenderer>().SetBlendShapeWeight(39, 100f);
        }
    }

    private void Shoot()
    {
        autoShootCount++;
        float recoilForce = currentWeapon.recoilForce;

        if (autoShootCount <= 3)
        {
            recoilForce *= 0.2f;
        }

        OnshootServerRpc(recoilForce);

        RaycastHit hit; // 射线击中的第一个物体
        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out hit, currentWeapon.range, mask))
        {
            if (hit.collider.tag == PLAYER_TAG)
            {
                ShootServerRpc(hit.collider.name, currentWeapon.damage);
                OnHitServerRpc(hit.point, hit.normal, HitEffectMaterial.Metal);
            }
            else
            {
                OnHitServerRpc(hit.point, hit.normal, HitEffectMaterial.Stone);
            }
            if (hit.collider.tag == "NPC")
            {
                NpcOnHit(hit.collider.gameObject);
            }
        }
    }

    [ServerRpc]
    private void ShootServerRpc(string name, int damage)
    {
        Player player = GameManager.Singleton.GetPlayer(name);
        player.TakeDamage(damage);
    }
}
