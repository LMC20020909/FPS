using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class WeaponManager : NetworkBehaviour
{
    [SerializeField]
    private PlayerWeapon primaryWeapon;

    [SerializeField]
    private PlayerWeapon secondaryWeapon;

    [SerializeField]
    private GameObject weaponHolder;

    private PlayerWeapon currentWeapon;
    private WeaponGraphics currentGraphics;
    private AudioSource currentAudioSource;

    // Start is called before the first frame update
    void Start()
    {
        EquipWeapon(primaryWeapon);
    }

    public void EquipWeapon(PlayerWeapon weapon)
    {
        currentWeapon = weapon;

        // ����һ��������ͼ��ɾ������ֹ�ص�
        while (weaponHolder.transform.childCount > 0)
        {
            DestroyImmediate(weaponHolder.transform.GetChild(0).gameObject);
        }

        // ʵ����currentWeapon��ͼ����������ص�weaponHolder������Ⱦ
        GameObject weaponObject = Instantiate(currentWeapon.graphics, weaponHolder.transform.position, weaponHolder.transform.rotation);
        weaponObject.transform.SetParent(weaponHolder.transform);

        currentGraphics = weaponObject.GetComponent<WeaponGraphics>();
        currentAudioSource = weaponObject.GetComponent<AudioSource>();

        // ������Լ��Ļ�����Ч��Ϊ2D����Ϊǹ��ģ������ҵ��ұߣ�3D��ʹ����ƫ�ң�
        if (IsLocalPlayer)
        {
            currentAudioSource.spatialBlend = 0f;
        }
    }

    public PlayerWeapon GetCurrentWeapon()
    {
        return currentWeapon;
    }

    public WeaponGraphics GetCurrentGraphics()
    {
        return currentGraphics;
    }

    public AudioSource GetCurrentAudioSource()
    {
        return currentAudioSource;
    }

    private void ToggleWeapon()
    {
        if (currentWeapon == primaryWeapon)
        {
            EquipWeapon(secondaryWeapon);
        } else
        {
            EquipWeapon(primaryWeapon);
        }
    }

    [ClientRpc]
    private void ToggleWeaponClientRpc()
    {
        ToggleWeapon();
    }

    [ServerRpc]
    private void ToggleWeaponServerRpc()
    {
        if (!IsHost)
        {
            ToggleWeapon();
        }
        ToggleWeaponClientRpc();
    }

    // Update is called once per frame
    void Update()
    {
        if (IsLocalPlayer)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                ToggleWeaponServerRpc();
            }
        }
    }
}
