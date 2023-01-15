using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    #region
    private GameModeStateEnum _CurrentInGameState;
    private GameModeStateEnum _PrevInGameState;
    private bool _ChangeState = false;
    #endregion


    #region Unity function
    // Start is called before the first frame update
    void Start()
    {
        Sound2D.LoadBGM("PlayBGM", "2D/PlayBGM");
        Sound2D.LoadSE("GameClearSE", "2D/GameClear");
        Sound2D.LoadSE("GameOverSE", "2D/GameOver");

        // ����
        Sound2D.LoadSE("P_DamagedSE", "2D/P_Damaged");
        Sound2D.LoadSE("P_DeadSE", "2D/P_Dead");
        Sound2D.LoadSE("HealSE", "2D/Heal");
    }

    // Update is called once per frame
    void Update()
    {
        // �ŏ��Ɍ��݂̃X�e�[�g���擾
        _CurrentInGameState = GameModeController.Instance.State;

        // �X�e�[�g���ς���������X�V
        _ChangeState = ChangStateChack();

        if (_ChangeState && _CurrentInGameState == GameModeStateEnum.Play)
        {
            Sound2D.PlayBGM("PlayBGM");
        }
        if (_ChangeState && _CurrentInGameState == GameModeStateEnum.Clear)
        {
            Sound2D.StopBGM();
            Sound2D.PlaySE("GameClearSE");
        }
        if (_ChangeState && _CurrentInGameState == GameModeStateEnum.GameOver)
        {
            Sound2D.StopBGM();
            Sound2D.PlaySE("GameOverSE");
        }

        // �X�e�[�g���X�V
        _PrevInGameState = _CurrentInGameState;
    }
    #endregion


    #region Private Method
    bool ChangStateChack()
    {
        if (_CurrentInGameState != _PrevInGameState)
        {
            return true;
        }

        return false;
    }
    #endregion
}
