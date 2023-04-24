using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcReaction : MonoBehaviour
{
    private float horribleTime = 0f;
    SkinnedMeshRenderer skinned;
    private float idleTime = 0f;
    private Animator animator;
    private int npcNum = 0;
    // Start is called before the first frame update
    void Start()
    {
        skinned = GetComponentInChildren<SkinnedMeshRenderer>();
        animator = GetComponent<Animator>();
        switch (transform.name)
        {
            case "Keli": npcNum = 1;break;
            case "Qiuqiuren": npcNum = 2;break;
        }

        animator.SetInteger("npc", npcNum);
    }

    private void AlterIdle()
    {
        idleTime += Time.deltaTime;
        int idle = 0;
        if (idleTime > 8f)
        {
            idleTime = 0f;
            idle = 1;
        }
        animator.SetInteger("idle", idle);
    }

    // Update is called once per frame
    void Update()
    {
        if (skinned.GetBlendShapeWeight(8) > 0)
        {
            horribleTime += Time.deltaTime;
            if (horribleTime > 3f)
            {
                skinned.SetBlendShapeWeight(8, 0f);
                skinned.SetBlendShapeWeight(39, 0f);
                horribleTime = 0f;
            }
        }

        AlterIdle();
    }
}
