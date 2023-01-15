using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoldierMove : EnemyParent
{

    public float chaseSpeed = 0.1f;//追いかけるスピード
    public float MaxRange = 0.8f;//手が届くであろう距離
    public float CoolTime = 3f;//攻撃のクールタイム
    public float ExitDistance = 0.3f;//手が近くに来たら逃げる距離

    //Transform Target;
    //GameObject Hime;
    Transform RHandTarget;
    GameObject RHand;
    Transform LHandTarget;
    GameObject LHand;

    private int state = 0;
    private float time;
    float speed;
    float StopDistance = 0;
    public static bool gun = false;


    float removespeed;
    int turn = 0;
    float moveRange = 0f;

    float repostime;

    public static bool remove = false;

    public float Desk_height = 0.4f;

    bool spawn = true;
    public Animator animator;

    GameObject Point;

    private Dictionary<string, _Data> _PoolSE = new Dictionary<string, _Data>();


    // Start is called before the first frame update
    void Start()
    {
        SetUp();

        animator = GetComponent<Animator>();

        //Hime = GameObject.FindWithTag("Princess");
        //Target = Hime.transform;
        speed = chaseSpeed;

        //右手のオブジェクト（名前）
        RHand = GameObject.Find("RightOVRHandPrefab");
        RHandTarget = RHand.transform;

        //左手のオブジェクト（名前）
        LHand = GameObject.Find("LeftOVRHandPrefab");
        LHandTarget = LHand.transform;

        Point = GameObject.Find("soldierAtanPoint");

        SetSE();


    }
    void SetSE()
    {
        _PoolSE.Add("Gun_Shot", new _Data("Gun_Shot", "3D/Gun_Shot"));
        
    }
    // 指定のSEを１回再生
    void PlaySE(string key)
    {
        // リソースの取得
        var _data = _PoolSE[key];
        var source = GetComponent<AudioSource>();
        source.clip = _data.Clip;
        source.Play();
    }


    // Update is called once per frame
    void Update()
    {
        if (GameModeController.Instance.GameTime > 180f)
        {

            animator.SetTrigger("Run");
            fallback();
            return;
        }

        if (spawn == true)
        {
            animator.SetTrigger("Pilot");
        }


        float distance = Vector3.Distance(transform.position, Target.transform.position);

        //手と銃歩兵との距離(Rが右手、Lが左手)
        float Rhanddistance = Vector3.Distance(transform.position, RHandTarget.position);
        float Lhanddistance = Vector3.Distance(transform.position, LHandTarget.position);

        //姫と手の距離がExitDistanceより小さい時に返す
        if (Rhanddistance < ExitDistance || Lhanddistance < ExitDistance)
        {
            if (IsBlownAway)
            {
                
                return;
            }

        }
        Vector3 TarPos = Target.transform.position;
        TarPos.y = transform.position.y;

        transform.LookAt(TarPos);

        float StopRange = Random.Range(0.3f, 0.5f);//姫から0.3～0.5のランダム距離

        if (StopDistance == 0)
        {
            StopDistance = StopRange;
        }
        if (gun == true)
        {
            gun = false;
        }



        //敵と姫の距離が0.3～0.5の距離になったら停止、それ以上の時は追いかける
        if (distance > StopDistance)
        {

            Chase();

        }
        else
        {
            StopDistance = MaxRange;
            //手の届かない距離になったら追いかける
            if (distance > StopDistance)
            {
                StopDistance = 0;
            }

            Debug.Log("停止");
            chaseSpeed = 0;

            switch (state)
            {
                case 0:
                    chaseSpeed = 0;
                    stop();
                    break;
                case 1:
                    chaseSpeed = 0;
                    shot();
                    break;
                default:
                    reposition();
                    break;

            }


        }

        void Chase()
        {
            if (distance > MaxRange)
            {
                chaseSpeed = speed;
            }
            Debug.Log("追いかける");
            transform.position += transform.forward * chaseSpeed * Time.deltaTime;

            animator.SetTrigger("Run");
        }

        void stop()
        {
            animator.SetTrigger("Stop");

            time = time + Time.deltaTime;

            if (time > CoolTime)
            {
                time = 0;
                state = 1;
            }
        }

        void shot()
        {
            animator.SetTrigger("Shoot");

            Debug.Log("うった");
            gun = true;
            state = 2;
            remove = true;

            PlaySE("Gun_Shot");
        }
    }
    void reposition()
    {
        animator.SetTrigger("Run");

        repostime += Time.deltaTime;


        float x = Point.transform.position.x - this.transform.position.x;
        float z = Point.transform.position.z - this.transform.position.z;

        float rad = Mathf.Atan2(z, x) * Mathf.Rad2Deg;

        Debug.Log(rad + "向いてる角度！！！！！！！！！！！！！！！！！！");

        float distance = Vector3.Distance(transform.position, Target.transform.position);

        if (remove == true)
        {
            turn = Random.Range(0, 2);
            moveRange = Random.Range(-0.3f, 0.3f);
            remove = false;
        }
        chaseSpeed = speed * 0.05f;

        if (distance < 0.4f)
        {
            turn = 0;
        }

        //机の手前側の時に机の内側に入る
        if (transform.position.z <= -0.3f)
        {
            //廊下の方向を向いているとき
            if (rad < 90 && rad > -10)
            {

                turn = 1;
                moveRange = Random.Range(-0.3f, 0);
            }
            //窓の方向を向いているとき
            else if (rad >= 90 && rad < 190)
            {
                Debug.Log("窓の方向向いてるよ！！！！！！！！！");
                turn = 1;
                moveRange = Random.Range(0, 0.3f);
            }
        }//机の奥側の時に机の内側に入る
        else if (transform.position.z >= 0.4f)
        {
            //廊下の方向を向いているとき
            if (rad < 90 && rad > -10)
            {

                turn = 0;
                moveRange = Random.Range(0, 0.3f);
            }
            //窓の方向を向いているとき
            else if (rad >= 90 && rad < 190)
            {
                Debug.Log("窓の方向向いてるよ！！！！！！！！！");
                turn = 0;
                moveRange = Random.Range(-0.3f, 0);
            }
        }

        switch (turn)
        {
            case 0://左右にずれる
                Vector3 repos_x = new Vector3(transform.position.x + moveRange, transform.position.y, transform.position.z);
                transform.position = Vector3.MoveTowards(transform.position, repos_x, chaseSpeed);
                break;
            default://前後にずれる
                Vector3 repos_z = new Vector3(transform.position.x, transform.position.y, transform.position.z + moveRange);
                transform.position = Vector3.MoveTowards(transform.position, repos_z, chaseSpeed);
                break;
        }

        Debug.Log("うったあとにうごいてる");
        if (repostime > 0.5f)
        {
            state = 0;
            repostime = 0f;
        }


    }

    private void OnCollisionEnter(Collision hitcollision)
    {
        // hit がいらなくなり次第if 文なくす
        HandHit(hitcollision.gameObject);

        // 敵にぶつかられた時の処理（敵に当たったかも判定）
        EnemyHit(hitcollision.gameObject);

        if (hitcollision.gameObject.tag == "Desk")
        {
            spawn = false;
            animator.SetTrigger("Wait");
        }

    }
}
