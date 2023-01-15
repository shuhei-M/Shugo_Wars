using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankMove : EnemyParent
{
    GameObject pos0;
    GameObject pos1;
    GameObject pos2;
    GameObject pos3;
    GameObject pos4;
    GameObject pos5;


    //private GameObject Target;
    //public string himename;

    public float movespeed = 0.01f;//行き先まで動くスピード
    public float rotationSpeed = 0.01f;//姫の方向を向く回転するスピード

    GameObject course;

    int posmove;
    float _shottime = 0f;
    bool move = true;//trueのとき行先まで動く
    bool rol = false;//falseのとき行き先をさがす
    public static bool cannon = false;//trueで大砲を撃つ

    GameObject Desk;
    public float upspeed = 0.01f;
    bool spawn = false;

    public float ShotTime = 10f;
    public float StopDistance = 0.04f;

    private Dictionary<string, _Data> _PoolSE = new Dictionary<string, _Data>();

    void Start()
    {
        SetUp();

        pos0 = GameObject.Find("pos0");
        pos1 = GameObject.Find("pos1");
        pos2 = GameObject.Find("pos2");
        pos3 = GameObject.Find("pos3");
        pos4 = GameObject.Find("pos4");
        pos5 = GameObject.Find("pos5");

        //Target = GameObject.Find(himename);
        Randomrol();

        spawn = true;
        Desk = GameObject.Find("Desk");

        SetSE();
    }

    void SetSE()
    {
        _PoolSE.Add("Tank_Shot", new _Data("Tank_Shot", "3D/Tank_Shot"));

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

        //スポーンしてから机の上まで移動
        if (spawn == true)
        {
            transform.position += transform.up * upspeed * Time.deltaTime;

            if (transform.position.y > Desk.transform.position.y + 0.005f)
            {
                upspeed = 0f;
                spawn = false;
            };
            return;
        }

        //int posmove = Random.Range(0, 6);

        if (rol == false)
        {
            //行き先がONの場合再検索
            if (course.gameObject.tag == "ON")
            {
                Randomrol();
                return;
            }
            rol = true;
            transform.LookAt(course.transform.position);
            Debug.Log("行き先" + course);
        }

        if (move == true)
        {
            Debug.Log("タンク走る");
            float distance = Vector3.Distance(transform.position, course.transform.position);

            run();
            if (distance <= StopDistance)
            {
                move = false;
            }
        }
        else
        {
            Debug.Log("タンク回転");
            course.tag = "ON";

            Vector3 targetPosition = Target.transform.position;

            if (transform.position.y != Target.transform.position.y)
            {
                targetPosition = new Vector3(Target.transform.position.x, transform.position.y, Target.transform.position.z);
            }

            Quaternion TargetRot = Quaternion.LookRotation(targetPosition - transform.position);

            float step = rotationSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Slerp(transform.rotation, TargetRot, step * 0.1f);//ひめに向かって回転

            shot();
        }
    }

    private void OnCollisionEnter(Collision hitcollision)
    {
        // 手に当たった時の処理（手に当たったかも判定）
        HandHit(hitcollision.gameObject);

        // 敵にぶつかられた時の処理（敵に当たったかも判定）
        EnemyHit(hitcollision.gameObject);
    }

    void Randomrol()
    {
        posmove = Random.Range(0, 6);

        switch (posmove)
        {
            case 0:
                course = pos0;
                break;
            case 1:
                course = pos1;
                break;
            case 2:
                course = pos2;
                break;
            case 3:
                course = pos3;
                break;
            case 4:
                course = pos4;
                break;
            default:
                course = pos5;
                break;

        }
        Debug.Log("行き先探し中");
    }

    void run()
    {
        //行き先に向かって走る
        transform.position = Vector3.MoveTowards(transform.position, course.transform.position, movespeed * 0.01f);
    }

    void shot()
    {
        //10秒ごとに弾を生成
        _shottime = _shottime + Time.deltaTime;
        if (_shottime > ShotTime)
        {
            _shottime = 0f;

            Debug.Log("発射");

            cannon = true;

            PlaySE("Tank_Shot");

        }
    }
    void OnDestroyed()
    {
        Debug.Log("破壊された");

        course.tag = "OFF";
    }

}
