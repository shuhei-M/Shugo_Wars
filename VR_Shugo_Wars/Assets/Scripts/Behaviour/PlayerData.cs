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
        // 姫のレベルが上がった際に回復
        HimeLevelUp();

        // 手の当たり判定のあるオブジェクトを取得
        if (hand.capsulesObj == null && meshRenderer.IsInitialized)
        {
            hand.capsulesObj = transform.GetChild(2).gameObject;
        }
        else if(_capsulesInstansTime < 2.5f)
        {
            _capsulesInstansTime += Time.deltaTime;
        }

        // スタン中
        if (_stan)
        {
            IsStan();
        }
    }
    #endregion


    #region Method
    // SEのロード
    void SetSE()
    {
        _PoolSE.Add("Hand_Hit", new _Data("Hand_Hit", "3D/Hand_Hit"));
        _PoolSE.Add("Hand_Stan", new _Data("Hand_Stan", "3D/Hand_Stan"));
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

    // プレイヤーがダメージを受けた時の処理
    void DamagePlayer(int damage)
    {
        if (_life <= 0) return;

        // デバッグモード：姫が無敵　ならすぐリターン
        if (GameModeController.Instance.DebugMode == GameModeController.DebugModeEnum.Invincible)
            return;

        //  デバッグモード：Safety　なら、Life=1ならリターン
        if (GameModeController.Instance.DebugMode == GameModeController.DebugModeEnum.Safety
            && _life == 1)
            return;

        _life -= damage;
        Debug.Log("プレイヤーが攻撃を受けた！\n" +
            handType + " = " + _life);

        // 手のライフが 0 になったら非表示
        // （そのうち一時的に物理判定をなくす処理に変更）
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

    // 掴む判定のSphearのオブジェクトを取得
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

    // スタン中の処理
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

    // 姫のレベルが上がったか取得
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
    /// 担当者：松島
    /// 姫とライドエリアの親子関係を切る
    /// </summary>
    private void ResetPrincessTransform()
    {
        GameModeController.Instance.Princess.transform.parent = null;
    }
    #endregion


    /// <summary> ダメージを与える等の、バトル系の機能を集めたインターフェース。 </summary>
    #region IBattleObject
    /// <summary>
    /// 残りライフ
    /// </summary>
    public int Life { get { return _life; } }

    /// <summary>
    /// ダメージを与える関数。
    /// ダメージを与える側のオブジェクトが、この関数を呼び出す。
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
