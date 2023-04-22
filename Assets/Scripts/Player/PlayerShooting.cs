using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerShooting : NetworkBehaviour
{
    private const string PLAYER_TAG = "Player";

    private WeaponManager weaponManager;
    private PlayerWeapon currentWeapon;

    private float ShootCoolDownTime = 0f;   // �����ϴο�ǹ���˶�ã��룩
    private int autoShootCount = 0; // ��ǰһ����������ǹ

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

        if (currentWeapon.shootRate <= 0)   // ����
        {
            if (Input.GetButtonDown("Fire1") && ShootCoolDownTime >= currentWeapon.shootCoolDownTime)
            {
                autoShootCount = 0;
                Shoot();
                ShootCoolDownTime = 0f; // ������ȴʱ��
            }
        } else    // ����
        {
            if (Input.GetButtonDown("Fire1"))
            {
                autoShootCount = 0;
                // �ظ�ִ��һ����������һ������Ϊ���������ڶ�������Ϊʲôʱ��ִ�е�һ�Σ� ����������Ϊ����ִ�е�ʱ����
                InvokeRepeating("Shoot", 0f, 1f / currentWeapon.shootRate);
            } else if (Input.GetButtonUp("Fire1") || Input.GetKeyDown(KeyCode.Q))   // �ɿ������������л�������ֹͣ����
            {
                CancelInvoke("Shoot");
            }
        }
        
    }

    private void OnHit(Vector3 pos, Vector3 normal, HitEffectMaterial material) // ���е����Ч
    {
        GameObject hitEffectPrefab;
        if (material == HitEffectMaterial.Metal)
        {
            hitEffectPrefab = weaponManager.GetCurrentGraphics().metalHitEffectPrefab;
        } else
        {
            hitEffectPrefab = weaponManager.GetCurrentGraphics().stoneHitEffectPrefab;
        }

        GameObject hitEffectObject = Instantiate(hitEffectPrefab, pos, Quaternion.LookRotation(normal));    // ������Ч����Ϊ���߷�����
        ParticleSystem particleSystem = hitEffectObject.GetComponent<ParticleSystem>();
        particleSystem.Emit(1); // ֱ�Ӵ�����û���ӳ�
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

    private void Onshoot(float recoilForce)  // ÿ�������ص��߼���������Ч��������
    {
        weaponManager.GetCurrentGraphics().muzzleFlash.Play();
        weaponManager.GetCurrentAudioSource().Play();

        if (IsLocalPlayer)  //ʩ�Ӻ�����
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

        RaycastHit hit; // ���߻��еĵ�һ������
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
