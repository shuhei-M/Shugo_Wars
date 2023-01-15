using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> インターフェイスをここに書く </summary>

#region IGrabableObject
/// <summary>
/// 摘まむ指先から、摘ままれたオブジェクトにアクセスるインターフェース。
/// 摘まむことが可能なオブジェクトには、必ず継承させる。
/// </summary>
public interface IGrabableComponent
{
    /// <summary> 摘ままれているかどうか </summary>
    public bool IsGrabed { get; set; }

    Transform Get_GrabedPoint();
}
#endregion


#region IBattleObject
/// <summary>
/// ダメージを与える等の、バトル系の機能を集めたインターフェース。
/// ダメージを受ける、、オブジェクトには、必ず継承させる。
/// </summary>
public interface IBattleComponent
{
    /// <summary>
    /// 残りライフ
    /// </summary>
    public int Life { get; }

    /// <summary>
    /// ダメージを与える関数。
    /// ダメージを与える側のオブジェクトが、この関数を呼び出す。
    /// 姫・両手・敵で、共通。ダメージのint, floatは継承側で設定（キャスト）
    /// </summary>
    /// <param name="damage"></param>
    public void ApplyDamage(int damage);
}
#endregion


#region IPlayerGetItem
/// <summary>
/// アイテムをゲットした時に発動させる関数群。
/// </summary>
public interface IPlayerGetItemComponent
{
    public void HealLifePoint(int healPoint);

    public void AddExp(int addExpPoint);

    public void EquipWeapon(WeaponType weaponType);
}
#endregion
