using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealItemBehaviour : BaseItemBehavior
{
    #region define

    #endregion

    #region serialize field
    
    #endregion

    #region field
    /// <summary> 自身ににアタッチされたコンポーネントを取得するための変数群 </summary>
    private int _HealPoint = 1;
    #endregion

    #region property

    #endregion

    #region Unity function
    // Start is called before the first frame update
    void Start()
    {
        SetUpBase();
    }

    // Update is called once per frame
    void Update()
    {
        if(!TryHeightUpdate()) return;

        if(!TryTimeUpdate()) return;
    }
    #endregion

    #region public function
    /// <summary>
    /// 回復ポイントを変更する
    /// ドロップ元の敵クラスから、回復量を調節できるようにする。
    /// </summary>
    /// <param name="point"></param>
    public void SetHealPoint(int point)
    {
        _HealPoint = point;
    }
    #endregion

    #region protected function
    /// <summary>
    /// アイテムの効果を発動させる。
    /// 継承先の各種アイテムクラスで内容を決める
    /// </summary>
    protected override void ItemAbility(IPlayerGetItemComponent princess)
    {
        // 回復させる
        princess.HealLifePoint(_HealPoint);

        // エフェクトを発生させる
        EffectManager.Instance.Play(EffectManager.EffectID.Heal, transform.position);
    }
    #endregion

    #region private function

    #endregion
}
