using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour, IBattleComponent
{
    #region define
    private enum HandType
    {
        LeftHand,
        Righthand,
    }
    #endregion


    #region serializefield
    [SerializeField] private HandType handType = HandType.LeftHand;
    [SerializeField] private int _life = 50;
    [SerializeField] private float _stanTime = 5.0f;
    #endregion


    #region field
    private Dictionary<string, _Data> _PoolSE = new Dictionary<string, _Data>();
    private Hand hand;
    private OVRMeshRenderer meshRenderer;
    private Material material;
    private Color _handMatColor;
    private int _startLife;
    private int _oldHimeLevel = 1;
    private float _nowStanTime = 0f;
    private float _capsulesInstansTime = 0f;
    private float _stanFlashSpeed = 6f;
    private bool _stan = false;
    bool _stanInMat = false;
    #endregion


    #region Unity function
    // Start is called before the first frame update
    void Start()
    {
        hand = new Hand();
        meshRenderer = GetComponent<OVRMeshRenderer>();
        material = GetComponent<SkinnedMeshRenderer>().material;
        _handMatColor = material.GetColor("_MyColor");

        GetHand();

        SetSE();

        _startLife = _life;
    }

    // Update is called once per frame
    void Update()
    {
        // �P�̃��x�����オ�����ۂɉ�
        HimeLevelUp();

        // ��̓����蔻��̂���I�u�W�F�N�g���擾
        if (hand.capsulesObj == null && meshRenderer.IsInitialized)
        {
            hand.capsulesObj = transform.GetChild(2).gameObject;
        }
        else if(_capsulesInstansTime < 2.5f)
        {
            _capsulesInstansTime += Time.deltaTime;
        }

        // �X�^����
        if (_stan)
        {
            IsStan();
        }
    }
    #endregion


    #region Method
    // SE�̃��[�h
    void SetSE()
    {
        _PoolSE.Add("Hand_Hit", new _Data("Hand_Hit", "3D/Hand_Hit"));
        _PoolSE.Add("Hand_Stan", new _Data("Hand_Stan", "3D/Hand_Stan"));
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

    // �v���C���[���_���[�W���󂯂����̏���
    void DamagePlayer(int damage)
    {
        if (_life <= 0) return;

        // �f�o�b�O���[�h�F�P�����G�@�Ȃ炷�����^�[��
        if (GameModeController.Instance.DebugMode == GameModeController.DebugModeEnum.Invincible)
            return;

        //  �f�o�b�O���[�h�FSafety�@�Ȃ�ALife=1�Ȃ烊�^�[��
        if (GameModeController.Instance.DebugMode == GameModeController.DebugModeEnum.Safety
            && _life == 1)
            return;

        _life -= damage;
        Debug.Log("�v���C���[���U�����󂯂��I\n" +
            handType + " = " + _life);

        // ��̃��C�t�� 0 �ɂȂ������\��
        // �i���̂����ꎞ�I�ɕ���������Ȃ��������ɕύX�j
        if (_life <= 0)
        {
            //this.gameObject.SetActive(false);
            ResetPrincessTransform();
            hand.ChangeActive(false);
            _stan = true;
            PlaySE("Hand_Stan");
        }
        else
        {
            PlaySE("Hand_Hit");
        }
    }

    // �͂ޔ����Sphear�̃I�u�W�F�N�g���擾
    void GetHand()
    {
        if (handType == HandType.LeftHand)
        {
            hand.grabSencor = GameObject.Find("LeftGrabSencor");
            hand.rideArea = GameObject.Find("LeftRideArea");
        }
        else if(handType == HandType.Righthand)
        {
            hand.grabSencor = GameObject.Find("RightGrabSencor");
            hand.rideArea = GameObject.Find("RightRideArea");
        }
    }

    // �X�^�����̏���
    void IsStan()
    {
        _nowStanTime += Time.deltaTime;

        if (_nowStanTime >= _stanTime)
        {
            material.SetColor("_MyColor", _handMatColor);
            hand.ChangeActive(true);
            _stan = false;
            _life = _startLife;
            _nowStanTime = 0f;
        }
        else
        {
            StanMaterial();
        }
    }

    void StanMaterial()
    {
        var col = material.GetColor("_MyColor");
        var red = col.r;
        var green = col.g;
        var blue = col.b;
        var alpha = col.a;

        if (_stanInMat)
        {
            green += _stanFlashSpeed / 256f;
            blue += _stanFlashSpeed / 256f;
            if (green >= _handMatColor.g || blue >= _handMatColor.b)
            {
                material.SetColor("_MyColor", _handMatColor);
                _stanInMat = !_stanInMat;
            }
            else
            {
                material.SetColor("_MyColor", new Color(red, green, blue, alpha));
            }
        }
        else
        {
            green -= _stanFlashSpeed / 256f;
            blue -= _stanFlashSpeed / 256f;
            if (green <= 0f)
            {
                green = 0;

                material.SetColor("_MyColor", new Color(red, green, blue, alpha));
                _stanInMat = !_stanInMat;
            }
            else if(blue <= 0f)
            {
                blue = 0;

                material.SetColor("_MyColor", new Color(red, green, blue, alpha));
                _stanInMat = !_stanInMat;
            }
            else
            {
                material.SetColor("_MyColor", new Color(red, green, blue, alpha));
            }
        }
    }

    // �P�̃��x�����オ�������擾
    void HimeLevelUp()
    {
        int nowLevel = GameModeController.Instance.Princess.Level;

        if (_oldHimeLevel < nowLevel)
        {
            _life = _startLife;
            _oldHimeLevel = nowLevel;
        }
    }

    /// <summary>
    /// �S���ҁF����
    /// �P�ƃ��C�h�G���A�̐e�q�֌W��؂�
    /// </summary>
    private void ResetPrincessTransform()
    {
        GameModeController.Instance.Princess.transform.parent = null;
    }
    #endregion


    /// <summary> �_���[�W��^���铙�́A�o�g���n�̋@�\���W�߂��C���^�[�t�F�[�X�B </summary>
    #region IBattleObject
    /// <summary>
    /// �c�胉�C�t
    /// </summary>
    public int Life { get { return _life; } }

    /// <summary>
    /// �_���[�W��^����֐��B
    /// �_���[�W��^���鑤�̃I�u�W�F�N�g���A���̊֐����Ăяo���B
    /// </summary>
    /// <param name="damage"></param>
    public void ApplyDamage(int damage)
    {
        DamagePlayer(damage);
    }
    #endregion
}

public class Hand
{
    #region public field
    public OVRHand hand;
    public GameObject capsulesObj;
    public GameObject grabSencor;
    public GameObject rideArea;
    #endregion


    #region Method
    public void ChangeActive(bool flag)
    {
        capsulesObj.SetActive(flag);
        grabSencor.SetActive(flag);
        rideArea.SetActive(flag);
    }
    #endregion
}
