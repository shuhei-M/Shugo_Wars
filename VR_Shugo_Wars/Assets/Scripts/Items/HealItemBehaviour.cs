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
    /// <summary> ���g�ɂɃA�^�b�`���ꂽ�R���|�[�l���g���擾���邽�߂̕ϐ��Q </summary>
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
    /// �񕜃|�C���g��ύX����
    /// �h���b�v���̓G�N���X����A�񕜗ʂ𒲐߂ł���悤�ɂ���B
    /// </summary>
    /// <param name="point"></param>
    public void SetHealPoint(int point)
    {
        _HealPoint = point;
    }
    #endregion

    #region protected function
    /// <summary>
    /// �A�C�e���̌��ʂ𔭓�������B
    /// �p����̊e��A�C�e���N���X�œ��e�����߂�
    /// </summary>
    protected override void ItemAbility(IPlayerGetItemComponent princess)
    {
        // �񕜂�����
        princess.HealLifePoint(_HealPoint);

        // �G�t�F�N�g�𔭐�������
        EffectManager.Instance.Play(EffectManager.EffectID.Heal, transform.position);
    }
    #endregion

    #region private function

    #endregion
}
