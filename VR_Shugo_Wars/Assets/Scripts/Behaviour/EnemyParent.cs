using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyParent : MonoBehaviour
{
    #region Protected field
    protected float blownPower = 100f;
    protected float awaySpeed = 1.0f;
    protected GameObject Target;
    private GameObject SpawnAir;
    protected SpawnArea_Air spawnArea_AirScript;
    private GameObject SpawnGround;
    protected SpawnArea_Ground spawnArea_GroundScript;
    private GameObject SpawnShip;
    protected SpawnArea_Ship spawnArea_ShipScript;
    private GameObject fallback_air;
    private GameObject fallback_ground_R;
    private GameObject fallback_ground_L;
    protected float fallback_speed = 0.001f;
    protected float gametime;
    #endregion


    #region Field
    public bool IsBlownAway = false;
    public Rigidbody rb;
    private NavMeshAgent navMeshAgent;
    public Vector3 vector;
    private Dictionary<string, _Data> _PoolSE = new Dictionary<string, _Data>();
    #endregion


    /// <summary>
    /// 吹っ飛ばされる処理のテンプレート
    /// 継承先でこの処理を書く
    /// </summary>
    #region Unity function
    //void Start()
    //{
    //    SetUp();
    //}

    //private void OnCollisionEnter(Collision hitcollision)
    //{
    //    // 手に当たった時の処理（手に当たったかも判定）
    //    HandHit(hitcollision.gameObject);

    //    // 敵にぶつかられた時の処理（敵に当たったかも判定）
    //    EnemyHit(hitcollision.gameObject);
    //}


    #endregion


    #region　Protected method
    // セットアップ処理
    protected void SetUp()
    {
        SetSE();

        rb = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        Target = GameObject.Find("Princess");

#if UNITY_EDITOR //デバック用　エディターのみ　スポーン系非表示の際のバグ対策
        if (GameObject.Find("Spawn_Air") != null)
        {
            SpawnAir = GameObject.Find("Spawn_Air");
            spawnArea_AirScript = SpawnAir.GetComponent<SpawnArea_Air>();
        }

        if (GameObject.Find("Spawn_Ship") != null)
        {
            SpawnShip = GameObject.Find("Spawn_Ship");
            spawnArea_ShipScript = SpawnShip.GetComponent<SpawnArea_Ship>();
        }

        if (GameObject.Find("Spawn_Ground") != null)
        {
            SpawnGround = GameObject.Find("Spawn_Ground");
            spawnArea_GroundScript = SpawnGround.GetComponent<SpawnArea_Ground>();
        }
#else //本番用
　　　　　　SpawnAir = GameObject.Find("Spawn_Air");
            spawnArea_AirScript = SpawnAir.GetComponent<SpawnArea_Air>();

　　　　　　SpawnShip = GameObject.Find("Spawn_Ship");
            spawnArea_ShipScript = SpawnShip.GetComponent<SpawnArea_Ship>();

　　　　　　SpawnGround = GameObject.Find("Spawn_Ground");
            spawnArea_GroundScript = SpawnGround.GetComponent<SpawnArea_Ground>();
#endif
    }

    // 手に当たったかを判定し、処理を行う
    protected void HandHit(GameObject other)
    {
        if (other.gameObject.tag == "HandCapsuleRigidbody" && !IsBlownAway)
        {
            ONClashEnemy();
            BlownAway(other.gameObject);
        }
    }

    // 敵に吹っ飛ばされたか判定し、処理を行う
    protected void EnemyHit(GameObject other)
    {
        // まず、接触したのが敵であるかを判定
        EnemyParent OtherData;
        bool Otherf;
        OtherData = other.gameObject.GetComponent<EnemyParent>();
        if (OtherData != null)
        {
            Otherf = OtherData.IsBlownAway;
        }
        else
        {
            return;
        }

        //Debug.Log(Otherf);

        if ((other.gameObject.tag == "enemy_ship_r" && !IsBlownAway && Otherf)
            || (other.gameObject.tag == "enemy_tank" && !IsBlownAway && Otherf)
            || (other.gameObject.tag == "enemy_soldier" && !IsBlownAway && Otherf)
            || (other.gameObject.tag == "enemy_ship_g" && !IsBlownAway && Otherf))
        {
            ONClashEnemy();
            BlownAway_Enemy(other.gameObject);
        }
    }

    // プレイヤーに飛ばされた時の処理
    protected void BlownAway(GameObject other)
    {
        var handObj = other.transform.parent.parent.gameObject;
        GameObject handRigitObj;
        if (handObj.name == "LeftOVRHandPrefab")
        {
            handRigitObj = GameObject.Find("LeftRigitCollider");
        }
        else if (handObj.name == "RightOVRHandPrefab")
        {
            handRigitObj = GameObject.Find("RightRigitCollider");
        }
        else
        {
            return;
        }
        var handRC = handRigitObj.gameObject.GetComponent<HandRightCollider>();

        // 手の速さが一定以上でなければ吹っ飛ばない
        if (handRC.GetSpeed() < awaySpeed)
            return;

        var vec = handRC.GetVelocity();
        vec.y = 1.0f;
        vector = vec;

        // ナビメッシュ使用の場合はここの処理をする
        if (gameObject.GetComponent<NavMeshAgent>() != null)
        {
            navMeshAgent.enabled = false;
            rb.isKinematic = false;
        }

        rb.useGravity = false;
        rb.AddForce(vec * blownPower);

        this.IsBlownAway = true;

        if (gameObject.tag != "TestEnemy")
            PlaySE("BlownAway");

        // 同じ種類の敵のカウンターを減らす
        EnemyAnalysis(this.gameObject);
        //Debug.Log("ship" + spawnArea_AirScript.enemy_count[0]);

        // アイテムを落とす
        ItemDrop(true);

        Destroy(this.gameObject, 1f);
    }

    //ゲーム終了時に机からはける
    protected void fallback()
    {

        //gametime = GameModeController.Instance.GameTime;
        //Debug.Log(gametime + "←←←←←←←←←←←");

        //if (gametime < 10f)
        //{
        //    return;
        //}

        fallback_air = GameObject.Find("fallback_air");
        fallback_ground_R = GameObject.Find("fallback_ground_R");
        fallback_ground_L = GameObject.Find("fallback_ground_L");

        //浮いてる敵は窓に向かって進む
        if (gameObject.tag == "enemy_ship_r" || gameObject.tag == "enemy_gatring")
        {
            transform.LookAt(fallback_air.transform.position);
            transform.position = Vector3.MoveTowards(transform.position, fallback_air.transform.position, fallback_speed);
        }

        //机の上の敵は机から落下
        if (gameObject.tag == "enemy_soldier" || gameObject.tag == "enemy_tank")
        {
            rb.useGravity = true;
            float fallback_ground_distance = Vector3.Distance(transform.position, fallback_ground_R.transform.position);

            //fallback_ground_R(机を正面で見て机の右端)から机の真ん中の距離の中にいれば机の右端から落下
            if (fallback_ground_distance < 1f)
            {
                transform.LookAt(fallback_ground_R.transform.position);
                transform.position = Vector3.MoveTowards(transform.position, fallback_ground_R.transform.position, fallback_speed);
            }
            //fallback_ground_R(机を正面で見て机の右端)から机の真ん中の距離の中にいなければ机の左端から落下
            else
            {
                transform.LookAt(fallback_ground_L.transform.position);
                transform.position = Vector3.MoveTowards(transform.position, fallback_ground_L.transform.position, fallback_speed);
            }

        }
    }
    #endregion

    #region Method
    void SetSE()
    {
        _PoolSE.Add("BlownAway", new _Data("BlownAway", "3D/BlownAwaySE"));
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

    // 吹っ飛んできた敵に巻き込まれた時の処理 
    void BlownAway_Enemy(GameObject other)
    {
        Vector3 Othervec;

        //敵の種類に合わせてデータ(other.vector)の取り込み。
        if (other.tag == "enemy_ship_r")
        {
            var OtherData = other.gameObject.GetComponent<Ship_RScript>();
            Othervec = OtherData.vector;
        }
        else if (other.tag == "enemy_soldier")
        {
            var OtherData = other.gameObject.GetComponent<SoldierMove>();
            Othervec = OtherData.vector;
        }
        else if (other.tag == "enemy_tank")
        {
            var OtherData = other.gameObject.GetComponent<TankMove>();
            Othervec = OtherData.vector;
        }
        else if (other.tag == "enemy_ship_g")
        {
            var OtherData = other.gameObject.GetComponent<GatringMove>();
            Othervec = OtherData.vector;
        }
        else
        {
            return;
        }

        //衝突時のベクトル計算
        var thisvec = new Vector3(Othervec.x, Othervec.y / 2, Othervec.z / 2);
        var othervec = new Vector3(Othervec.x / 2, Othervec.y / 2, Othervec.z);


        // ナビメッシュ使用の場合はここの処理をする
        if (gameObject.GetComponent<NavMeshAgent>() != null)
        {
            navMeshAgent.enabled = false;
            rb.isKinematic = false;
        }

        //ベクトルの反映、吹っ飛び。(自身と突っ込んできた対象)
        rb.useGravity = false;
        rb.AddForce(thisvec * blownPower);

        var Otherrb = other.GetComponent<Rigidbody>();
        Otherrb.AddForce(othervec * blownPower);

        this.IsBlownAway = true;

        // 同じ種類の敵のカウンターを減らす
        EnemyAnalysis(this.gameObject);
        Debug.Log("ship" + spawnArea_AirScript.enemy_count[0]);

        // アイテムを落とす
        ItemDrop(false);

        Destroy(this.gameObject, 1f);
    }

    //タグから敵の種類を察知し、その対象のカウントを減らす。
    void EnemyAnalysis(GameObject other)
    {
        if (other.tag == "enemy_ship_r")
        {
#if UNITY_EDITOR //デバック用　エディターのみ　スポーン系非表示の際のバグ対策
            if (spawnArea_AirScript != null)
            {
                spawnArea_AirScript.enemy_count[0]--;
            }

#else //本番用
                spawnArea_AirScript.enemy_count[0]--;
#endif
            //spawnArea_AirScript.enemyshiplist.Remove(this.gameObject);

        }
        else if (other.tag == "enemy_soldier")
        {
#if UNITY_EDITOR //デバック用　エディターのみ　スポーン系非表示の際のバグ対策
            if (spawnArea_ShipScript != null)
            {
                spawnArea_ShipScript.enemy_count[0]--;
            }

#else //本番用
                spawnArea_ShipScript.enemy_count[0]--;
#endif

        }
        else if (other.tag == "enemy_tank")
        {
#if UNITY_EDITOR //デバック用　エディターのみ　スポーン系非表示の際のバグ対策
            if (spawnArea_GroundScript != null)
            {
                spawnArea_GroundScript.enemy_count[0]--;
            }

#else //本番用
                spawnArea_GroundScript.enemy_count[0]--;
#endif

        }
        else if (other.tag == "enemy_ship_g")
        {
#if UNITY_EDITOR //デバック用　エディターのみ　スポーン系非表示の際のバグ対策
            if (spawnArea_AirScript != null)
            {
                spawnArea_AirScript.enemy_count[1]--;
            }

#else //本番用
                spawnArea_AirScript.enemy_count[1]--;
#endif
        }
        else
        {
            return;
        }
    }

    void ONClashEnemy()
    {
        if (this.tag == "enemy_ship_r")
        {
            var ship_sc = this.GetComponent<Ship_RScript>();
            ship_sc.rig_f = true;
        }

    }

    // 武器・アイテムドロップ処理
    void ItemDrop(bool isAttackPlayer)
    {
        var dropPos = new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z);
        GameObject weapon = null;

        if (isAttackPlayer)
        {   // プレイヤーとの接触で倒れた場合のみ武器ドロップ抽選を行う
            weapon = WeaponManager.Instance.GenerateWeapon(this.gameObject, dropPos);
        }

        if (weapon == null)
        {
            ItemManager.Instance.GenerateItem(dropPos);
        }
    }

    #endregion
}
