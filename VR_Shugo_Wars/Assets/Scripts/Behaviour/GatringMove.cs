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

    public float stopdistance = 0.4f;//�P�ɋ߂Â����Ƃ��Ɏ~�܂鋗��
    public float speed = 0.01f;//�ǔ����鑬�x

    public float ChargeTime = 4f;//�`���[�W����
    float ChargeCount = 0f;

    [SerializeField] GameObject Shotpos;
    [SerializeField] GameObject Bullet;
    float ShotCount = 0f;
    public float Autofire = 0.1f;//�ꔭ�e�����܂ł̎���
    int BulletCount = 0;
    public int MaxBullet = 20;//�e�����ő呕�U��

    int state;

    private Dictionary<string, _Data> _PoolSE = new Dictionary<string, _Data>();


    // Start is called before the first frame update
    void Start()
    {
        SetUp();

        state = 0;

        //�E��̃I�u�W�F�N�g�i���O�j
        RHand = GameObject.Find("RightOVRHandPrefab");
        RHandTarget = RHand.transform;

        //����̃I�u�W�F�N�g�i���O�j
        LHand = GameObject.Find("LeftOVRHandPrefab");
        LHandTarget = LHand.transform;

        SetSE();
    }

    void SetSE()
    {
        _PoolSE.Add("Missile2", new _Data("Missile2", "3D/Missile2"));
        
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

    // Update is called once per frame
    void Update()
    {

        if (GameModeController.Instance.GameTime > 180f)
        {
            fallback();
            return;
        }

        //��ƂƂ̋���(R���E��AL������)
        float Rhanddistance = Vector3.Distance(transform.position, RHandTarget.position);
        float Lhanddistance = Vector3.Distance(transform.position, LHandTarget.position);

        //�P�Ǝ�̋�����ExitDistance��菬�������ɕԂ�
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


    //�Ђ߂𒆐S�ɔ��a40cm�ɓ���Ǝ~�܂�
    void Chase()
    {
        float distance = Vector3.Distance(transform.position, Target.transform.position);

        if (distance < stopdistance)
        {
            state = 1;
        }

        transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, speed);

    }


    //4�b���炢�`���[�W
    void Charge()
    {
        ChargeCount += Time.deltaTime;

        if (ChargeCount > ChargeTime)
        {
            state = 2;
            ChargeCount = 0f;
        }
    }


    //�e�̔���
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
        // hit ������Ȃ��Ȃ莟��if ���Ȃ���
        HandHit(hitcollision.gameObject);

        // �G�ɂԂ���ꂽ���̏����i�G�ɓ���������������j
        EnemyHit(hitcollision.gameObject);
    }
}

