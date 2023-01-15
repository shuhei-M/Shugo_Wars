using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary> �C���^�[�t�F�C�X�������ɏ��� </summary>

#region IGrabableObject
/// <summary>
/// �E�܂ގw�悩��A�E�܂܂ꂽ�I�u�W�F�N�g�ɃA�N�Z�X��C���^�[�t�F�[�X�B
/// �E�܂ނ��Ƃ��\�ȃI�u�W�F�N�g�ɂ́A�K���p��������B
/// </summary>
public interface IGrabableComponent
{
    /// <summary> �E�܂܂�Ă��邩�ǂ��� </summary>
    public bool IsGrabed { get; set; }

    Transform Get_GrabedPoint();
}
#endregion


#region IBattleObject
/// <summary>
/// �_���[�W��^���铙�́A�o�g���n�̋@�\���W�߂��C���^�[�t�F�[�X�B
/// �_���[�W���󂯂�A�A�I�u�W�F�N�g�ɂ́A�K���p��������B
/// </summary>
public interface IBattleComponent
{
    /// <summary>
    /// �c�胉�C�t
    /// </summary>
    public int Life { get; }

    /// <summary>
    /// �_���[�W��^����֐��B
    /// �_���[�W��^���鑤�̃I�u�W�F�N�g���A���̊֐����Ăяo���B
    /// �P�E����E�G�ŁA���ʁB�_���[�W��int, float�͌p�����Őݒ�i�L���X�g�j
    /// </summary>
    /// <param name="damage"></param>
    public void ApplyDamage(int damage);
}
#endregion


#region IPlayerGetItem
/// <summary>
/// �A�C�e�����Q�b�g�������ɔ���������֐��Q�B
/// </summary>
public interface IPlayerGetItemComponent
{
    public void HealLifePoint(int healPoint);

    public void AddExp(int addExpPoint);

    public void EquipWeapon(WeaponType weaponType);
}
#endregion
