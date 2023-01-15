using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierBullet : EnemyWeapon
{
    public float speed = 0.5f;
    public GameObject Front;

    //private GameObject Princess;
    //private GameObject RightHand;
    //private GameObject LeftHand;
    //public string targetname;
    //public string handname_R;
    //public string handname_L;

    public GameObject Soldier;

    //Transform Target;
    //GameObject Hime;


    // Start is called before the first frame update
    void Start()
    {
        SetUp();

        //Princess = GameObject.Find(targetname);
        //RightHand = GameObject.Find(handname_R);
        //LeftHand = GameObject.Find(handname_L);

        //Hime = GameObject.FindWithTag("Princess");
        //Target = Hime.transform;

        transform.LookAt(Target.transform);
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(
            transform.position, Front.transform.position, speed * Time.deltaTime);

        Destroy(this.gameObject, 10f);
    }

    private void OnCollisionEnter(Collision hitcollision)
    {
        //if (Princess.name == hitcollision.gameObject.name || this.gameObject.name == hitcollision.gameObject.name)
        //{
        //    Destroy(gameObject);//ターゲットに当たると自壊
        //}


        //// null参照が無いように
        //if (hitcollision.transform.parent == null || hitcollision.transform.parent.parent == null)
        //    return;

        //if (RightHand == hitcollision.transform.parent.parent.gameObject || LeftHand == hitcollision.transform.parent.parent.gameObject)
        //{
        //    Destroy(gameObject);//ターゲットに当たると自壊
        //}

        DamageJudgment(hitcollision);
    }
}
