using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpItemBehavior : MonoBehaviour, IGrabableComponent
{
    #region define

    #endregion

    #region serialize field
    [SerializeField, Range(5.0f, 10.0f)] float _LifeTime = 10.0f;
    #endregion

    #region field
    /// <summary> ���g�ɂɃA�^�b�`���ꂽ�R���|�[�l���g���擾���邽�߂̕ϐ��Q </summary>
    private Transform _GrabedPoint;
    private ExpItemSensorBehaviour _ExpItemSensor;
    private Rigidbody _Rigidbody;

    private int _AddExpPoint = 1;   // ���Z����o���l

    private Vector3 _GapVec;   // �P�ւ̃x�N�g��
    private float _Speed;   // �P�֌������ē����ۂ̃X�s�[�h

    private bool _IsMove;   // �A�C�e�����P�Ɍ������ē����Ă��邩

    private float time;
    #endregion

    #region property
    public bool IsMove { get { return _IsMove; } }
    #endregion

    #region Unity function
    // Start is called before the first frame update
    void Start()
    {
        _GrabedPoint = transform.Find("GrabedPoint").gameObject.transform;
        _ExpItemSensor = transform.Find("Sensor").gameObject.GetComponent<ExpItemSensorBehaviour>();
        _Rigidbody = GetComponent<Rigidbody>();

        _GapVec = Vector3.zero;
        _Speed = 0.0f;
        _IsMove = false;

        time = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -1) DestroyThisItem();
        _IsMove = _ExpItemSensor.IsFindPlayer;

        if(time > _LifeTime) DestroyThisItem();

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

    private void OnCollisionEnter(Collision collision)
    {
        IPlayerGetItemComponent princess = collision.gameObject.GetComponent<IPlayerGetItemComponent>();

        // �P�łȂ���Έȉ��̏����͍s��Ȃ��B
        if (princess == null) return;

        // �o���l��^����
        princess.AddExp(_AddExpPoint);

        // �G�t�F�N�g�𔭐�������
        EffectManager.Instance.Play(EffectManager.EffectID.Exp, transform.position);

        // �A�C�e������
        DestroyThisItem();
    }
    #endregion

    #region public function
    /// <summary>
    /// �񕜃|�C���g��ύX����
    /// �h���b�v���̓G�N���X����A�񕜗ʂ𒲐߂ł���悤�ɂ���B
    /// </summary>
    /// <param name="point"></param>
    public void SetAddExpPoint(int point)
    {
        _AddExpPoint = point;
    }
    #endregion

    #region private function
    /// <summary>
    /// �A�C�e������������
    /// </summary>
    private void DestroyThisItem()
    {
        ItemManager.Instance.AliveItemCount--;

        // �A�C�e������
        Destroy(this.gameObject);
    }
    #endregion

    /// <summary> �E�܂ގw�悩��A�E�܂܂ꂽ�I�u�W�F�N�g�ɃA�N�Z�X���邽�߂̃C���^�[�t�F�[�X </summary>
    #region IGrabableObject
    /// <summary> �E�܂܂�Ă��邩�ǂ��� </summary>
    public bool IsGrabed { get; set; }

    /// <summary> �͂ލ��W��n�� </summary>
    public Transform Get_GrabedPoint()
    {
        return _GrabedPoint;
    }
    #endregion
}
