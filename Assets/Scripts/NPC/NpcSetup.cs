using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class NpcSetup : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Singleton.RegisterNpc(transform.name, GetComponent<Npc>());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
