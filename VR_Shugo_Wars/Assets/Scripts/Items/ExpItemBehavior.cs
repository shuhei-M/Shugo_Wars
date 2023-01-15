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
    /// <summary> 自身ににアタッチされたコンポーネントを取得するための変数群 </summary>
    private ExpItemSensorBehaviour _ExpItemSensor;
    private Rigidbody _Rigidbody;

    private int _AddExpPoint = 1;   // 加算する経験値

    private Vector3 _GapVec;   // 姫へのベクトル
    private float _Speed;   // 姫へ向かって動く際のスピード

    private bool _IsMove;   // アイテムが姫に向かって動いているか
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
            // 姫を見失った場合はスピードをリセット
            _Speed = 0.0f;
            _Rigidbody.useGravity = true;
        }
    }
    #endregion

    #region public function
    /// <summary>
    /// 経験値ポイントを変更する
    /// ドロップ元の敵クラスから、経験値を調節できるようにする。
    /// </summary>
    /// <param name="point"></param>
    public void SetAddExpPoint(int point)
    {
        _AddExpPoint = point;
    }
    #endregion

    #region protected function
    /// <summary>
    /// アイテムの効果を発動させる。
    /// 継承先の各種アイテムクラスで内容を決める
    /// </summary>
    protected override void ItemAbility(IPlayerGetItemComponent princess)
    {
        // 経験値を与える
        princess.AddExp(_AddExpPoint);

        // エフェクトを発生させる
        EffectManager.Instance.Play(EffectManager.EffectID.Exp, transform.position);
    }
    #endregion

    #region private function

    #endregion
}
