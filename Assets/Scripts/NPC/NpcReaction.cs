using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NpcReaction : MonoBehaviour
{
    private float horribleTime = 0f;
    SkinnedMeshRenderer skinned;
    // Start is called before the first frame update
    void Start()
    {
         skinned = GetComponentInChildren<SkinnedMeshRenderer>();
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
    }
}
