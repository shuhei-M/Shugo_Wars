using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatringBullet : EnemyWeapon
{
    public float speed = 1f;

    // Start is called before the first frame update
    void Start()
    {
        SetUp();

        transform.LookAt(Target.transform);

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;


        Destroy(this.gameObject, 4f);
    }

    private void OnCollisionEnter(Collision hitcollision)
    {
        DamageJudgment(hitcollision);
    }
}
