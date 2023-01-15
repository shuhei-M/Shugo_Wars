using UnityEngine;

public class RideAreaBehaviour : MonoBehaviour
{
    /// <summary> �\�[�X�������Ƃ��̃����v���[�g </summary>

    #region define
    /// <summary> �ǂ���̎肩 </summary>
    public enum HandTypeEnum : int
    {
        Left,
        Right,
        None,
    }
    #endregion

    #region serialize field
    [SerializeField] public HandTypeEnum _HandType;
    #endregion

    #region field
    private bool _IsRided;   // �P�ɏ���Ă��邩
    private GameObject _HandAnchor;   // �e�I�u�W�F�N�g�B��̊p�x�擾�p�B
    private GameObject _OVRHandPrefab;   // �����̑���OVRHandPrefab;
    private OVRSkeleton _OVRSkeleton;   // �����̑���OVRHandPrefab;

    private Vector3 _HandDirection;   // ��̌���

    private GameObject TestObj;
    #endregion

    #region property
    public bool IsRided { get { return _IsRided; } }
    public HandTypeEnum HandType { get { return _HandType; } }

    public Vector3 HandDirection { get { return _HandDirection; } }
    #endregion

    #region Unity function
    // Start is called before the first frame update
    void Start()
    {
        _IsRided = false;
        _HandAnchor = transform.parent.gameObject;
        _OVRHandPrefab = _HandAnchor.transform.GetChild(1).gameObject;
        _OVRSkeleton = _OVRHandPrefab.GetComponent<OVRSkeleton>();

        TestObj = GameObject.Find("Test");
    }

    // Update is called once per frame
    void Update()
    {
        // �o�O�h�~
        if (_OVRSkeleton.Bones.Count <= 0) return;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "PrincessFoot")
        {
            _IsRided = true;
            GameModeController.Instance.Princess.SetRideArea(this);
            GameModeController.Instance.Princess.ToRideState();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "PrincessFoot")
        {
            if(GameModeController.Instance.Princess.PrincessState != PrincessBehaviour.StateEnum.Ride)
            {
                GameModeController.Instance.Princess.SetRideArea(this);
                GameModeController.Instance.Princess.ToRideState();
            }
            _IsRided = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "PrincessFoot")
        {
            _IsRided = false;
            if (GameModeController.Instance.Princess.PrincessState == PrincessBehaviour.StateEnum.Fall) return;
            GameModeController.Instance.Princess.ToFallState();
            GameModeController.Instance.Princess.ResetRideArea();
        }
    }
    #endregion

    #region public function

    #endregion

    #region private function
    
    #endregion
}
