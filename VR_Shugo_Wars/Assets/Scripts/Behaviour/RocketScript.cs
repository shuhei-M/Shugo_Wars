using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//担当 中薗 昂太
public class RocketScript : EnemyWeapon
{
    //public GameObject Front;
    private Vector3 dir;
    //public float rotio = 10f;
    public float speed = 0.1f;
    //public float homingTime = 3f;
    public float homingdistance = 2f;//〇分の一の距離まで追尾
    private float homingrange;
    public float lifeTime = 5f;

    private bool homingf;
    private bool movef;
    //private bool rocketf;
    public GameObject Ship;
    //private bool canJudge = true;

    // Start is called before the first frame update
    void Start()
    {
        SetUp();

        movef = true;
        homingf = false;

        Invoke(nameof(FireTime),1.0f);//
        Invoke(nameof(LifeTime), lifeTime);//○○秒後に消滅

        homingrange = Vector3.Distance(Target.transform.position, this.gameObject.transform.position) / homingdistance;
    }

    // Update is called once per frame
    void Update()
    {
        //ある程度の距離に近づくと追尾機能　OFF
        if (homingrange > Vector3.Distance(Target.transform.position, this.gameObject.transform.position))
        {
            homingf = true;
        }

        if (movef)
        {
            if (!homingf)
            {
                if (Target != null)
                {
                    LookHoming();
                    Homing();
                }
                else
                {
                    StraightForward();
                }
            }
            else
            {
                StraightForward();
            }
        }
        //else
        //{
        //    StraightForward();
        //}
    }

    //発射時
    private void FireTime()
    {
        movef = true;
    }
    //捕捉
    private void LookHoming()
    {
        dir = new Vector3(Target.transform.position.x, Target.transform.position.y + neck, Target.transform.position.z) - transform.position;
        dir.Normalize();
        transform.rotation = Quaternion.LookRotation(dir); //向きを変更する
    }
    //追尾
    private void Homing()
    {
        transform.position = Vector3.MoveTowards(
                        transform.position, new Vector3(
                            Target.transform.position.x,
                            Target.transform.position.y + neck,
                            Target.transform.position.z), speed * Time.deltaTime);//追尾
    }
    //直進
    private void StraightForward()
    {
        this.transform.position += transform.TransformDirection(Vector3.forward) * speed * Time.deltaTime;
    }
    //寿命
    private void LifeTime()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision hitcollision)
    {
        if (hitcollision.gameObject.name == Ship.name + "(Clone)" || hitcollision.gameObject.tag == "Desk")
        {
            Destroy(gameObject);//敵(船)に当たると自壊
        }

        DamageJudgment(hitcollision);
    }
}

