using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatringMove : EnemyParent
{
    Transform RHandTarget;
    GameObject RHand;
    Transform LHandTarget;
    GameObject LHand;

    public float ExitDistance = 0.001f;

    public float stopdistance = 0.4f;//姫に近づいたときに止まる距離
    public float speed = 0.01f;//追尾する速度

    public float ChargeTime = 4f;//チャージ時間
    float ChargeCount = 0f;

    [SerializeField] GameObject Shotpos;
    [SerializeField] GameObject Bullet;
    float ShotCount = 0f;
    public float Autofire = 0.1f;//一発弾を撃つまでの時間
    int BulletCount = 0;
    public int MaxBullet = 20;//弾を撃つ最大装填数

    int state;

    private Dictionary<string, _Data> _PoolSE = new Dictionary<string, _Data>();


    // Start is called before the first frame update
    void Start()
    {
        SetUp();

        state = 0;

        //右手のオブジェクト（名前）
        RHand = GameObject.Find("RightOVRHandPrefab");
        RHandTarget = RHand.transform;

        //左手のオブジェクト（名前）
        LHand = GameObject.Find("LeftOVRHandPrefab");
        LHandTarget = LHand.transform;

        SetSE();
    }

    void SetSE()
    {
        _PoolSE.Add("Missile2", new _Data("Missile2", "3D/Missile2"));
        
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
            fallback();
            return;
        }

        //手ととの距離(Rが右手、Lが左手)
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

        switch (state)
        {
            case 0:
                Chase();
                break;
            case 1:
                Charge();
                break;
            default:
                Shot();
                break;
        }


    }


    //ひめを中心に半径40cmに入ると止まる
    void Chase()
    {
        float distance = Vector3.Distance(transform.position, Target.transform.position);

        if (distance < stopdistance)
        {
            state = 1;
        }

        transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, speed);

    }


    //4秒くらいチャージ
    void Charge()
    {
        ChargeCount += Time.deltaTime;

        if (ChargeCount > ChargeTime)
        {
            state = 2;
            ChargeCount = 0f;
        }
    }


    //弾の発射
    void Shot()
    {
        ShotCount += Time.deltaTime;

        if (ShotCount > Autofire)
        {
            Instantiate(Bullet, Shotpos.transform.position, Quaternion.identity);
            BulletCount++;
            ShotCount = 0f;
            PlaySE("Missile2");
        }

        if (BulletCount > 20)
        {
            state = 0;
            ShotCount = 0f;
            BulletCount = 0;
        }
    }

    private void OnCollisionEnter(Collision hitcollision)
    {
        // hit がいらなくなり次第if 文なくす
        HandHit(hitcollision.gameObject);

        // 敵にぶつかられた時の処理（敵に当たったかも判定）
        EnemyHit(hitcollision.gameObject);
    }
}

