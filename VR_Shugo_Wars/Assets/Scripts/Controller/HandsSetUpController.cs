using UnityEngine;

public class HandsSetUpController : MonoBehaviour
{
    /// <summary> �\�[�X�������Ƃ��̃����v���[�g </summary>

    #region define

    #endregion


    #region serialize field
    [Header("�e�X�g�p�I�u�W�F�N�g�֌W")]
    [SerializeField] private GameObject _testObjPrefab;
    [SerializeField] private Transform _TestSpawn;

    [Header("�{�^��UI")]
    [SerializeField] private GameObject _leftHandSetResetButton;
    [SerializeField] private GameObject _rightHandSetResetButton;
    [SerializeField] private GameObject _leftHandOKButton;
    [SerializeField] private GameObject _rightHandOKButton;
    [SerializeField] private GameObject _GameStartButton;
    [SerializeField] private GameObject _RespawnButton;

    [Header("�{�^��UI�̓����蔻��I�u�W�F�N�g")]
    [SerializeField] private GameObject _HandSetUpSpheres;
    [SerializeField] private GameObject _leftHandSetResetSphere;
    [SerializeField] private GameObject _rightHandSetResetSphere;
    [SerializeField] private GameObject _leftHandOKSphere;
    [SerializeField] private GameObject _rightHandOKSphere;
    [SerializeField] private GameObject _GameStartSphere;
    [SerializeField] private GameObject _RespawnSphere;
    #endregion


    #region field
    private bool _IsStart = false;
    private bool _IsFinish = false;
    private bool _IsLeftHandSet = false;
    private bool _IsRightHandSet = false;
    private bool _IsLeftHandOK = false;
    private bool _IsRightHandOK = false;
    private bool _IsGameStart = false;
    private bool _IsRespawn = false;

    private GameObject _TestObj;

    /// <summary> �Q�[�����[�h�̃X�e�[�g���擾 </summary>
    private GameModeStateEnum _CurrentInGameState;
    private GameModeStateEnum _PrevInGameState;
    #endregion


    #region property
    public bool IsFnish { get { return _IsFinish; } }
    public bool IsLeftHandSet { get { return _IsLeftHandSet; } set { _IsLeftHandSet = value; } }
    public bool IsRightHandSet { get { return _IsRightHandSet; } set { _IsRightHandSet = value; } }
    public bool IsLeftHandOK { get { return _IsLeftHandOK; } set { _IsLeftHandOK = value; } }
    public bool IsRightHandOK { get { return _IsRightHandOK; } set { _IsRightHandOK = value; } }
    public bool IsGameStart { get { return _IsGameStart; } set { _IsGameStart = value; } }
    public bool IsRespawn { get { return _IsRespawn; } set { _IsRespawn = value; } }
    #endregion


    #region Unity function
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // �ŏ��Ɍ��݂̃X�e�[�g���擾
        _CurrentInGameState = GameModeController.Instance.State;


        if (_CurrentInGameState != GameModeStateEnum.HandsSetUp)
            return;

        if (!_IsStart)
        {
            _HandSetUpSpheres.SetActive(true);
            _IsStart = true;
        }

        // �e�X�g�I�u�W�F�N�g�̐�������
        SetTestObj();


        // �G�𐁂���΂����珀�������̃{�^����\��
        ApplyDamageCheck();


        // �Z�b�g�A�b�v�������������`�F�b�N
        SetUpCheck();


        // ��������̏���
        // ����̓f�o�b�O�p�iH�L�[��������Play�X�e�[�g�ɐi�ށj
        //DebugFunction();


        // �Ō�ɂЂƂO�̃X�e�[�g�Ƃ��ĕۑ�
        _PrevInGameState = _CurrentInGameState;
    }
    #endregion


    #region public function

    #endregion


    #region private function
    private void DebugFunction()
    {
        // �n���h�Z�b�g�A�b�v���łȂ���Έȉ��̏����͍s��Ȃ��B
        if (_CurrentInGameState != GameModeStateEnum.HandsSetUp) return;

        if (Input.GetKeyDown(KeyCode.H)) _IsFinish = true;
    }

    void SetTestObj()
    {
        if (_TestObj != null && !_IsRespawn)
            return;

        if (_IsRespawn)
        {
            Destroy(_TestObj);
            _IsRespawn = false;
        }

        _TestObj = Instantiate(_testObjPrefab, _TestSpawn.position, _TestSpawn.rotation);
    }

    private void ApplyDamageCheck()
    {
        if (_IsLeftHandSet)
        {
            _leftHandOKButton.SetActive(true);
            _leftHandOKSphere.SetActive(true);
        }

        if (_IsRightHandSet)
        {
            _rightHandOKButton.SetActive(true);
            _rightHandOKSphere.SetActive(true);
        }
    }

    private void SetUpCheck()
    {
        // �n���h�Z�b�g�A�b�v���łȂ���Έȉ��̏����͍s��Ȃ��B
        if (_CurrentInGameState != GameModeStateEnum.HandsSetUp) return;

        // ����̃Z�b�g�A�b�v���I�������
        if (_IsLeftHandOK)
        {
            _leftHandOKButton.SetActive(false);
            _leftHandOKSphere.SetActive(false);
            _leftHandSetResetButton.SetActive(false);
            _leftHandSetResetSphere.SetActive(false);
        }

        // �E��̃Z�b�g�A�b�v���I�������
        if (_IsRightHandOK)
        {
            _rightHandOKButton.SetActive(false);
            _rightHandOKSphere.SetActive(false);
            _rightHandSetResetButton.SetActive(false);
            _rightHandSetResetSphere.SetActive(false);
        }

        // ����̎�̃Z�b�g�A�b�v���I�������
        if (_IsLeftHandOK && _IsRightHandOK)
        {
            Destroy(_TestObj);
            _RespawnButton.SetActive(false);
            _RespawnSphere.SetActive(false);
            _GameStartButton.SetActive(true);
            _GameStartSphere.SetActive(true);
        }

        // �Q�[���X�^�[�g
        if(_IsGameStart)
        {
            _IsFinish = true;
            _HandSetUpSpheres.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }
    #endregion
}
