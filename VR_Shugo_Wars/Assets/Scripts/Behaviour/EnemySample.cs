using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySample : EnemyParent
{
    //[SerializeField] float blownPower = 10f;

    // Start is called before the first frame update
    void Start()
    {
        SetUp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        // 吹っ飛んでる最中か当たったものが手以外であれば以下の処理をしない
        if (collision.gameObject.tag == "HandCapsuleRigidbody" && !IsBlownAway)
        {
            BlownAway(collision.gameObject);
        }
    }
}
