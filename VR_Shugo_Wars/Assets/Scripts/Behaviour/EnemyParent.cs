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
    /// ������΂���鏈���̃e���v���[�g
    /// �p����ł��̏���������
    /// </summary>
    #region Unity function
    //void Start()
    //{
    //    SetUp();
    //}

    //private void OnCollisionEnter(Collision hitcollision)
    //{
    //    // ��ɓ����������̏����i��ɓ���������������j
    //    HandHit(hitcollision.gameObject);

    //    // �G�ɂԂ���ꂽ���̏����i�G�ɓ���������������j
    //    EnemyHit(hitcollision.gameObject);
    //}


    #endregion


    #region�@Protected method
    // �Z�b�g�A�b�v����
    protected void SetUp()
    {
        SetSE();

        rb = GetComponent<Rigidbody>();
        navMeshAgent = GetComponent<NavMeshAgent>();
        Target = GameObject.Find("Princess");

#if UNITY_EDITOR //�f�o�b�N�p�@�G�f�B�^�[�̂݁@�X�|�[���n��\���̍ۂ̃o�O�΍�
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
#else //�{�ԗp
�@�@�@�@�@�@SpawnAir = GameObject.Find("Spawn_Air");
            spawnArea_AirScript = SpawnAir.GetComponent<SpawnArea_Air>();

�@�@�@�@�@�@SpawnShip = GameObject.Find("Spawn_Ship");
            spawnArea_ShipScript = SpawnShip.GetComponent<SpawnArea_Ship>();

�@�@�@�@�@�@SpawnGround = GameObject.Find("Spawn_Ground");
            spawnArea_GroundScript = SpawnGround.GetComponent<SpawnArea_Ground>();
#endif
    }

    // ��ɓ����������𔻒肵�A�������s��
    protected void HandHit(GameObject other)
    {
        if (other.gameObject.tag == "HandCapsuleRigidbody" && !IsBlownAway)
        {
            ONClashEnemy();
            BlownAway(other.gameObject);
        }
    }

    // �G�ɐ�����΂��ꂽ�����肵�A�������s��
    protected void EnemyHit(GameObject other)
    {
        // �܂��A�ڐG�����̂��G�ł��邩�𔻒�
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

    // �v���C���[�ɔ�΂��ꂽ���̏���
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

        // ��̑��������ȏ�łȂ���ΐ�����΂Ȃ�
        if (handRC.GetSpeed() < awaySpeed)
            return;

        var vec = handRC.GetVelocity();
        vec.y = 1.0f;
        vector = vec;

        // �i�r���b�V���g�p�̏ꍇ�͂����̏���������
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

        // ������ނ̓G�̃J�E���^�[�����炷
        EnemyAnalysis(this.gameObject);
        //Debug.Log("ship" + spawnArea_AirScript.enemy_count[0]);

        // �A�C�e���𗎂Ƃ�
        ItemDrop(true);

        Destroy(this.gameObject, 1f);
    }

    //�Q�[���I�����Ɋ�����͂���
    protected void fallback()
    {

        //gametime = GameModeController.Instance.GameTime;
        //Debug.Log(gametime + "����������������������");

        //if (gametime < 10f)
        //{
        //    return;
        //}

        fallback_air = GameObject.Find("fallback_air");
        fallback_ground_R = GameObject.Find("fallback_ground_R");
        fallback_ground_L = GameObject.Find("fallback_ground_L");

        //�����Ă�G�͑��Ɍ������Đi��
        if (gameObject.tag == "enemy_ship_r" || gameObject.tag == "enemy_gatring")
        {
            transform.LookAt(fallback_air.transform.position);
            transform.position = Vector3.MoveTowards(transform.position, fallback_air.transform.position, fallback_speed);
        }

        //���̏�̓G�͊����痎��
        if (gameObject.tag == "enemy_soldier" || gameObject.tag == "enemy_tank")
        {
            rb.useGravity = true;
            float fallback_ground_distance = Vector3.Distance(transform.position, fallback_ground_R.transform.position);

            //fallback_ground_R(���𐳖ʂŌ��Ċ��̉E�[)������̐^�񒆂̋����̒��ɂ���Ί��̉E�[���痎��
            if (fallback_ground_distance < 1f)
            {
                transform.LookAt(fallback_ground_R.transform.position);
                transform.position = Vector3.MoveTowards(transform.position, fallback_ground_R.transform.position, fallback_speed);
            }
            //fallback_ground_R(���𐳖ʂŌ��Ċ��̉E�[)������̐^�񒆂̋����̒��ɂ��Ȃ���Ί��̍��[���痎��
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

    // �w���SE���P��Đ�
    void PlaySE(string key)
    {
        // ���\�[�X�̎擾
        var _data = _PoolSE[key];
        var source = GetComponent<AudioSource>();
        source.clip = _data.Clip;
        source.Play();
    }

    // �������ł����G�Ɋ������܂ꂽ���̏��� 
    void BlownAway_Enemy(GameObject other)
    {
        Vector3 Othervec;

        //�G�̎�ނɍ��킹�ăf�[�^(other.vector)�̎�荞�݁B
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

        //�Փˎ��̃x�N�g���v�Z
        var thisvec = new Vector3(Othervec.x, Othervec.y / 2, Othervec.z / 2);
        var othervec = new Vector3(Othervec.x / 2, Othervec.y / 2, Othervec.z);


        // �i�r���b�V���g�p�̏ꍇ�͂����̏���������
        if (gameObject.GetComponent<NavMeshAgent>() != null)
        {
            navMeshAgent.enabled = false;
            rb.isKinematic = false;
        }

        //�x�N�g���̔��f�A������сB(���g�Ɠ˂�����ł����Ώ�)
        rb.useGravity = false;
        rb.AddForce(thisvec * blownPower);

        var Otherrb = other.GetComponent<Rigidbody>();
        Otherrb.AddForce(othervec * blownPower);

        this.IsBlownAway = true;

        // ������ނ̓G�̃J�E���^�[�����炷
        EnemyAnalysis(this.gameObject);
        Debug.Log("ship" + spawnArea_AirScript.enemy_count[0]);

        // �A�C�e���𗎂Ƃ�
        ItemDrop(false);

        Destroy(this.gameObject, 1f);
    }

    //�^�O����G�̎�ނ��@�m���A���̑Ώۂ̃J�E���g�����炷�B
    void EnemyAnalysis(GameObject other)
    {
        if (other.tag == "enemy_ship_r")
        {
#if UNITY_EDITOR //�f�o�b�N�p�@�G�f�B�^�[�̂݁@�X�|�[���n��\���̍ۂ̃o�O�΍�
            if (spawnArea_AirScript != null)
            {
                spawnArea_AirScript.enemy_count[0]--;
            }

#else //�{�ԗp
                spawnArea_AirScript.enemy_count[0]--;
#endif
            //spawnArea_AirScript.enemyshiplist.Remove(this.gameObject);

        }
        else if (other.tag == "enemy_soldier")
        {
#if UNITY_EDITOR //�f�o�b�N�p�@�G�f�B�^�[�̂݁@�X�|�[���n��\���̍ۂ̃o�O�΍�
            if (spawnArea_ShipScript != null)
            {
                spawnArea_ShipScript.enemy_count[0]--;
            }

#else //�{�ԗp
                spawnArea_ShipScript.enemy_count[0]--;
#endif

        }
        else if (other.tag == "enemy_tank")
        {
#if UNITY_EDITOR //�f�o�b�N�p�@�G�f�B�^�[�̂݁@�X�|�[���n��\���̍ۂ̃o�O�΍�
            if (spawnArea_GroundScript != null)
            {
                spawnArea_GroundScript.enemy_count[0]--;
            }

#else //�{�ԗp
                spawnArea_GroundScript.enemy_count[0]--;
#endif

        }
        else if (other.tag == "enemy_ship_g")
        {
#if UNITY_EDITOR //�f�o�b�N�p�@�G�f�B�^�[�̂݁@�X�|�[���n��\���̍ۂ̃o�O�΍�
            if (spawnArea_AirScript != null)
            {
                spawnArea_AirScript.enemy_count[1]--;
            }

#else //�{�ԗp
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

    // ����E�A�C�e���h���b�v����
    void ItemDrop(bool isAttackPlayer)
    {
        var dropPos = new Vector3(transform.position.x, transform.position.y + 0.01f, transform.position.z);
        GameObject weapon = null;

        if (isAttackPlayer)
        {   // �v���C���[�Ƃ̐ڐG�œ|�ꂽ�ꍇ�̂ݕ���h���b�v���I���s��
            weapon = WeaponManager.Instance.GenerateWeapon(this.gameObject, dropPos);
        }

        if (weapon == null)
        {
            ItemManager.Instance.GenerateItem(dropPos);
        }
    }

    #endregion
}
