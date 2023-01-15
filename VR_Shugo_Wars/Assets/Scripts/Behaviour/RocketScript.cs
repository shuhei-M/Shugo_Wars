using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//íSìñ íÜâí çVëæ
public class RocketScript : EnemyWeapon
{
    //public GameObject Front;
    private Vector3 dir;
    //public float rotio = 10f;
    public float speed = 0.1f;
    //public float homingTime = 3f;
    public float homingdistance = 2f;//ÅZï™ÇÃàÍÇÃãóó£Ç‹Ç≈í«îˆ
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
        Invoke(nameof(LifeTime), lifeTime);//ÅõÅõïbå„Ç…è¡ñ≈

        homingrange = Vector3.Distance(Target.transform.position, this.gameObject.transform.position) / homingdistance;
    }

    // Update is called once per frame
    void Update()
    {
        //Ç†ÇÈíˆìxÇÃãóó£Ç…ãﬂÇ√Ç≠Ç∆í«îˆã@î\Å@OFF
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

    //î≠éÀéû
    private void FireTime()
    {
        movef = true;
    }
    //ïﬂë®
    private void LookHoming()
    {
        dir = new Vector3(Target.transform.position.x, Target.transform.position.y + neck, Target.transform.position.z) - transform.position;
        dir.Normalize();
        transform.rotation = Quaternion.LookRotation(dir); //å¸Ç´ÇïœçXÇ∑ÇÈ
    }
    //í«îˆ
    private void Homing()
    {
        transform.position = Vector3.MoveTowards(
                        transform.position, new Vector3(
                            Target.transform.position.x,
                            Target.transform.position.y + neck,
                            Target.transform.position.z), speed * Time.deltaTime);//í«îˆ
    }
    //íºêi
    private void StraightForward()
    {
        this.transform.position += transform.TransformDirection(Vector3.forward) * speed * Time.deltaTime;
    }
    //éıñΩ
    private void LifeTime()
    {
        Destroy(gameObject);
    }

    private void OnCollisionEnter(Collision hitcollision)
    {
        if (hitcollision.gameObject.name == Ship.name + "(Clone)" || hitcollision.gameObject.tag == "Desk")
        {
            Destroy(gameObject);//ìG(ëD)Ç…ìñÇΩÇÈÇ∆é©âÛ
        }

        DamageJudgment(hitcollision);
    }
}

