using UnityEngine;

public class HandsSetUpController : MonoBehaviour
{
    /// <summary> ソースを書くときのレンプレート </summary>

    #region define

    #endregion


    #region serialize field
    [Header("テスト用オブジェクト関係")]
    [SerializeField] private GameObject _testObjPrefab;
    [SerializeField] private Transform _TestSpawn;

    [Header("ボタンUI")]
    [SerializeField] private GameObject _leftHandSetResetButton;
    [SerializeField] private GameObject _rightHandSetResetButton;
    [SerializeField] private GameObject _leftHandOKButton;
    [SerializeField] private GameObject _rightHandOKButton;
    [SerializeField] private GameObject _GameStartButton;
    [SerializeField] private GameObject _RespawnButton;

    [Header("ボタンUIの当たり判定オブジェクト")]
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

    /// <summary> ゲームモードのステートを取得 </summary>
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
        // 最初に現在のステートを取得
        _CurrentInGameState = GameModeController.Instance.State;


        if (_CurrentInGameState != GameModeStateEnum.HandsSetUp)
            return;

        if (!_IsStart)
        {
            _HandSetUpSpheres.SetActive(true);
            _IsStart = true;
        }

        // テストオブジェクトの生成処理
        SetTestObj();


        // 敵を吹っ飛ばしたら準備完了のボタンを表示
        ApplyDamageCheck();


        // セットアップが完了したかチェック
        SetUpCheck();


        // 何かしらの処理
        // 今回はデバッグ用（Hキーを押すとPlayステートに進む）
        //DebugFunction();


        // 最後にひとつ前のステートとして保存
        _PrevInGameState = _CurrentInGameState;
    }
    #endregion


    #region public function

    #endregion


    #region private function
    private void DebugFunction()
    {
        // ハンドセットアップ中でなければ以下の処理は行わない。
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
        // ハンドセットアップ中でなければ以下の処理は行わない。
        if (_CurrentInGameState != GameModeStateEnum.HandsSetUp) return;

        // 左手のセットアップが終わったか
        if (_IsLeftHandOK)
        {
            _leftHandOKButton.SetActive(false);
            _leftHandOKSphere.SetActive(false);
            _leftHandSetResetButton.SetActive(false);
            _leftHandSetResetSphere.SetActive(false);
        }

        // 右手のセットアップが終わったか
        if (_IsRightHandOK)
        {
            _rightHandOKButton.SetActive(false);
            _rightHandOKSphere.SetActive(false);
            _rightHandSetResetButton.SetActive(false);
            _rightHandSetResetSphere.SetActive(false);
        }

        // 両手の手のセットアップが終わったか
        if (_IsLeftHandOK && _IsRightHandOK)
        {
            Destroy(_TestObj);
            _RespawnButton.SetActive(false);
            _RespawnSphere.SetActive(false);
            _GameStartButton.SetActive(true);
            _GameStartSphere.SetActive(true);
        }

        // ゲームスタート
        if(_IsGameStart)
        {
            _IsFinish = true;
            _HandSetUpSpheres.SetActive(false);
            this.gameObject.SetActive(false);
        }
    }
    #endregion
}
