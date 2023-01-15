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
        // �������ł�Œ��������������̂���ȊO�ł���Έȉ��̏��������Ȃ�
        if (collision.gameObject.tag == "HandCapsuleRigidbody" && !IsBlownAway)
        {
            BlownAway(collision.gameObject);
        }
    }
}
