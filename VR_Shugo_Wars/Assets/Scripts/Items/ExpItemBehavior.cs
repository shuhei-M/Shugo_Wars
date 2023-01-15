using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpItemBehavior : BaseItemBehavior
{
    #region define

    #endregion

    #region serialize field
    
    #endregion

    #region field
    /// <summary> ���g�ɂɃA�^�b�`���ꂽ�R���|�[�l���g���擾���邽�߂̕ϐ��Q </summary>
    private ExpItemSensorBehaviour _ExpItemSensor;
    private Rigidbody _Rigidbody;

    private int _AddExpPoint = 1;   // ���Z����o���l

    private Vector3 _GapVec;   // �P�ւ̃x�N�g��
    private float _Speed;   // �P�֌������ē����ۂ̃X�s�[�h

    private bool _IsMove;   // �A�C�e�����P�Ɍ������ē����Ă��邩
    #endregion

    #region property
    public bool IsMove { get { return _IsMove; } }
    #endregion

    #region Unity function
    // Start is called before the first frame update
    void Start()
    {
        SetUpBase();

        _ExpItemSensor = transform.Find("Sensor").gameObject.GetComponent<ExpItemSensorBehaviour>();
        _Rigidbody = GetComponent<Rigidbody>();

        _GapVec = Vector3.zero;
        _Speed = 0.0f;
        _IsMove = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(!TryHeightUpdate()) return;

        if (!TryTimeUpdate()) return;

        _IsMove = _ExpItemSensor.IsFindPlayer;

        if (_ExpItemSensor.IsFindPlayer)
        {
            _GapVec = GameModeController.Instance.Princess.transform.position - transform.position;
            _Rigidbody.useGravity = false;

            _GapVec = _GapVec.normalized;
            _Speed += (Time.deltaTime * 0.025f);
            transform.position += (_GapVec * _Speed);
        }
        else
        {
            // �P�����������ꍇ�̓X�s�[�h�����Z�b�g
            _Speed = 0.0f;
            _Rigidbody.useGravity = true;
        }
    }
    #endregion

    #region public function
    /// <summary>
    /// �o���l�|�C���g��ύX����
    /// �h���b�v���̓G�N���X����A�o���l�𒲐߂ł���悤�ɂ���B
    /// </summary>
    /// <param name="point"></param>
    public void SetAddExpPoint(int point)
    {
        _AddExpPoint = point;
    }
    #endregion

    #region protected function
    /// <summary>
    /// �A�C�e���̌��ʂ𔭓�������B
    /// �p����̊e��A�C�e���N���X�œ��e�����߂�
    /// </summary>
    protected override void ItemAbility(IPlayerGetItemComponent princess)
    {
        // �o���l��^����
        princess.AddExp(_AddExpPoint);

        // �G�t�F�N�g�𔭐�������
        EffectManager.Instance.Play(EffectManager.EffectID.Exp, transform.position);
    }
    #endregion

    #region private function

    #endregion
}
