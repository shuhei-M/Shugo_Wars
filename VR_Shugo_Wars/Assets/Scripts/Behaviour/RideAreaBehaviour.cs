using UnityEngine;

public class RideAreaBehaviour : MonoBehaviour
{
    /// <summary> ソースを書くときのレンプレート </summary>

    #region define
    /// <summary> どちらの手か </summary>
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
    private bool _IsRided;   // 姫に乗られているか
    private GameObject _HandAnchor;   // 親オブジェクト。手の角度取得用。
    private GameObject _OVRHandPrefab;   // 自分の側のOVRHandPrefab;
    private OVRSkeleton _OVRSkeleton;   // 自分の側のOVRHandPrefab;

    private Vector3 _HandDirection;   // 手の向き

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
        // バグ防止
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
