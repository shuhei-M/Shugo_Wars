using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExpItemSensorBehaviour : MonoBehaviour
{
    /// <summary> �\�[�X�������Ƃ��̃����v���[�g </summary>

    #region define

    #endregion

    #region serialize field

    #endregion

    #region field
    private bool _IsFindPlayer;   // ���m�͈͓��Ƀv���C���[���͂�������
    #endregion

    #region property
    /// <summary> �e�I�u�W�F�N�g�̌o���l�A�C�e������擾�����B </summary>
    public bool IsFindPlayer { get { return _IsFindPlayer; } }
    #endregion

    #region Unity function
    // Start is called before the first frame update
    void Start()
    {
        _IsFindPlayer = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Princess")
        {
            _IsFindPlayer = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Princess")
        {
            _IsFindPlayer = false;
        }
    }
    #endregion

    #region public function

    #endregion

    #region private function

    #endregion
}
