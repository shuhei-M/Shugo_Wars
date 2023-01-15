/// <summary> ���� </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    /// <summary> �\�[�X�������Ƃ��̃����v���[�g </summary>

    #region define

    #endregion

    #region serialize field
    [SerializeField] private Sprite _BrankSprite;
    [SerializeField] private Sprite _HeartSprite;
    [SerializeField] private int _MaxHandHP = 50;

    [SerializeField] bool IsDebugMode = false;
    #endregion

    #region field
    /// <summary> �X�e�[�W�� </summary>
    int _CurrentPrincessLife = 4;
    int _PrevPrincessLife = 4;

    /// <summary> �f�t�H���g�p�l���ȉ��̃I�u�W�F�N�g�̕ϐ� </summary>
    private GameObject _DefaultPanel;
    private TextMeshProUGUI _MessageText;
    private TextMeshProUGUI _TimerText;

    /// <summary> ���C�t�p�l���ȉ��̃I�u�W�F�N�g�̕ϐ� </summary>
    private GameObject _PrincessLifePanel;
    private TextMeshProUGUI _LevelText;

    /// <summary> �Q�[�����[�h�̃X�e�[�g���擾 </summary>
    private GameModeStateEnum _CurrentInGameState;
    private GameModeStateEnum _PrevInGameState;

    /// <summary> ��̃��C�t���܂ނ��擾 </summary>
    private GameObject _HandLifePanel;
    private PlayerData _LeftHand;
    private PlayerData _RightHand;
    private Slider _LeftSlider;
    private Slider _RightSlider;

    private GameObject _Desk;
    private bool _IsFinishSetUp = false;

    /// <summary> �����ݒ�p�l���ȉ��̃I�u�W�F�N�g�̕ϐ� </summary>
    private GameObject _DirectionSettingPanel;

    #endregion

    #region property

    #endregion

    #region Unity function
    // Start is called before the first frame update
    /// <summary> GameModeController��Start�֐��ŃA�N�Z�X�ł���悤�ɂ��邽�� </summary>
    private void Awake()
    {
        /// <summary> �f�t�H���g�p�l���ȉ��̃I�u�W�F�N�g�̃Z�b�g�A�b�v </summary>
        _DefaultPanel = transform.GetChild(0).gameObject;
        var messageObj = _DefaultPanel.transform.GetChild(0).gameObject;
        _MessageText = messageObj.GetComponent<TextMeshProUGUI>();
        var timerObj = _DefaultPanel.transform.GetChild(1).gameObject;
        _TimerText = timerObj.GetComponent<TextMeshProUGUI>();
        _TimerText.text = "00 : 00";

        /// <summary> ���C�t�p�l���ȉ��̃I�u�W�F�N�g�̃Z�b�g�A�b�v </summary>
        _PrincessLifePanel = _DefaultPanel.transform.GetChild(2).gameObject;
        _LevelText = _PrincessLifePanel.transform.Find("LevelText").gameObject.GetComponent<TextMeshProUGUI>();

        /// <summary> ��̃��C�t���܂ނ��擾 </summary>
        _HandLifePanel = _DefaultPanel.transform.Find("HandLifePanel").gameObject;
        _LeftHand = GameObject.Find("LeftOVRHandPrefab").GetComponent<PlayerData>();
        _RightHand = GameObject.Find("RightOVRHandPrefab").GetComponent<PlayerData>();
        _LeftSlider = _HandLifePanel.transform.GetChild(0).gameObject.GetComponent<Slider>();
        _RightSlider = _HandLifePanel.transform.GetChild(1).gameObject.GetComponent<Slider>();

        _Desk = GameObject.Find("Desk");

        _DirectionSettingPanel = transform.Find("DirectionSettingPanel").gameObject;
        _DirectionSettingPanel.SetActive(false);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _CurrentInGameState = GameModeController.Instance.State;
        _CurrentPrincessLife = GameModeController.Instance.Princess.Life;
        if (_CurrentPrincessLife < 0) _CurrentPrincessLife = 0;

        UpdateUIState();
        DebugFunction();

        _PrevInGameState = _CurrentInGameState;
        _PrevPrincessLife = _CurrentPrincessLife;
    }
    #endregion

    #region public function

    #endregion

    #region private function
    /// <summary>
    /// ��Ԃ̕ύX
    /// </summary>
    private void ChangeUIState()
    {
        // ���O���o��
        //Debug.Log("ChangeState " + _PrevInGameState + "-> " + _CurrentInGameState);
        UI_ShowMessage(_CurrentInGameState);

        switch (_CurrentInGameState)
        {
            case GameModeStateEnum.None:
                {
                }
                break;
            case GameModeStateEnum.DirectionSetting:
                {
                    _DefaultPanel.SetActive(false);
                    _DirectionSettingPanel.SetActive(true);
                }
                break;
            case GameModeStateEnum.CountDown:
                {
                    _DirectionSettingPanel.SetActive(false);
                    _DefaultPanel.SetActive(true);
                }
                break;
            case GameModeStateEnum.HandsSetUp:
                {
                    _DefaultPanel.SetActive(false);
                }
                break;
            case GameModeStateEnum.Play:
                {
                    _DefaultPanel.SetActive(true);
                }
                break;
            case GameModeStateEnum.Clear:
                {
                    UI_ToGameClear();
                }
                break;
            case GameModeStateEnum.GameOver:
                {
                    UpdateLifeIcon();
                    UI_ToDead();
                }
                break;
        }

    }

    /// <summary>
    /// ��Ԗ��̖��t���[���Ă΂�鏈��
    /// </summary>
    private void UpdateUIState()
    {
        if (IsEntryThisState()) { ChangeUIState(); return; }

        switch (_CurrentInGameState)
        {
            case GameModeStateEnum.None:
                {
                }
                break;
            case GameModeStateEnum.DirectionSetting:
                {
                }
                break;
            case GameModeStateEnum.CountDown:
                {
                    // ���Ԃ̍X�V
                    UI_TimerUpdate(GameModeController.Instance.GameTime);
                }
                break;
            case GameModeStateEnum.HandsSetUp:
                {
                    // ��x�������̍����ɍ��킹��Canvas���ړ�������
                    if(!_IsFinishSetUp && _Desk.GetComponent<DeskBehaviour>().IsFinishSetUp)
                    {
                        SetPositon();
                        _IsFinishSetUp = true;
                    }
                }
                break;
            case GameModeStateEnum.Play:
                {
                    // ���Ԃ̍X�V
                    UI_TimerUpdate(GameModeController.Instance.GameTime);

                    // �P�̃��C�t�̍X�V
                    if (IsLifeChange()) UpdateLifeIcon();

                    // ����̃��C�t�̍X�V
                    UpdateHandLife();

                    // ���x���\���̍X�V
                    _LevelText.text = "Level : " + GameModeController.Instance.Princess.Level;
                        //+ "/" + GameModeController.Instance.Princess.Exp;
                    
                    if(IsDebugMode)
                    {
                        _MessageText.text =
                            "" + _CurrentInGameState + " : " + GameModeController.Instance.DebugMode;
                    }
                }
                break;
            case GameModeStateEnum.Clear:
                {
                }
                break;
            case GameModeStateEnum.GameOver:
                {
                }
                break;
        }
    }

    /// <summary>
    /// ���傤�ǂ��̃X�e�[�g�ɓ����������ǂ���
    /// </summary>
    /// <returns></returns>
    private bool IsEntryThisState()
    {
        return (_PrevInGameState != _CurrentInGameState);
    }

    /// <summary>
    /// ���̍����ɍ��킹��
    /// </summary>
    private void SetPositon()
    {
        Vector3 temp = new Vector3(
                        _Desk.transform.position.x,
                        _Desk.transform.position.y + 0.0111f,
                        _Desk.transform.position.z);

        RectTransform rectTransform = GetComponent<RectTransform>();

        rectTransform.position = temp;
    }

    /// <summary> �^�C�}�[�̍X�V </summary>
    public void UI_TimerUpdate(float timeF)
    {
        int time = 0;
        if(_CurrentInGameState == GameModeStateEnum.Play) time = GameModeController.Instance.LimitTime - (int)timeF;
        else time = (int)timeF;

        int seconds = 0;
        int minutes = 0;

        string secondsT = "0";
        string minutesT = "0";

        // �����E�b���̐U�蕪��
        if (time > 59)
        {
            minutes = time / 60;
            seconds = time - (minutes * 60);
        }
        else
        {
            seconds = time;
        }

        // �����\���p�e�L�X�g�쐬
        if (seconds > 9)
        {
            secondsT = seconds.ToString();
        }
        else
        {
            secondsT += seconds.ToString();
        }

        // �b���\���p�e�L�X�g�쐬
        if (minutes > 9)
        {
            minutesT = minutes.ToString();
        }
        else
        {
            minutesT += minutes.ToString();
        }

        _TimerText.text = minutesT + " : " + secondsT;
    }

    /// <summary>
    /// ���C�t�����������ǂ���
    /// </summary>
    /// <returns></returns>
    private bool IsLifeChange()
    {
        return (_PrevPrincessLife != _CurrentPrincessLife);
    }

    /// <summary>
    /// ���C�t�A�C�R���𑝌�������
    /// </summary>
    private void UpdateLifeIcon()
    {
        // ���C�t��0�ȉ��ɂȂ�����A�S�ău�����N�A�C�R���ɂ��āA���^�[��
        if (_CurrentPrincessLife <= 0)
        {
            for(int i = 0; i < 5; i++)
            {
                GameObject lifeIcon = _PrincessLifePanel.transform.GetChild(i).gameObject;
                Image lifeImage = lifeIcon.GetComponent<Image>();
                lifeImage.sprite = _BrankSprite;
            }
            return;
        }

        int index = _CurrentPrincessLife - 1;

        // ���C�t�A�C�R������čX�V
        for (int i = 0; i < 5; i++)
        {
            GameObject lifeIcon = _PrincessLifePanel.transform.GetChild(i).gameObject;
            Image lifeImage = lifeIcon.GetComponent<Image>();

            // ���C�t�ȉ��́A�n�[�g
            if(i <= index) lifeImage.sprite = _HeartSprite;
            // ���C�t����́A�u�����N
            else lifeImage.sprite = _BrankSprite;
        }
    }

    /// <summary>
    /// ����̃��C�t�Q�[�W���X�V����
    /// </summary>
    private void UpdateHandLife()
    {
        _LeftSlider.value = (float)_LeftHand.Life / (float)_MaxHandHP;
        _RightSlider.value = (float)_RightHand.Life / (float)_MaxHandHP;
    }

    /// <summary> UI��GameOver��ԂɑJ�ڂ����� </summary>
    private void UI_ShowMessage(GameModeStateEnum gameModeStateEnum)
    {
        _MessageText.text = "" + gameModeStateEnum;
    }

    /// <summary> UI��GameClear��ԂɑJ�ڂ����� </summary>
    private void UI_ToGameClear()
    {
        _MessageText.text = "Game Clear!";
    }

    /// <summary> UI��GameOver��ԂɑJ�ڂ����� </summary>
    private void UI_ToDead()
    {
        _MessageText.text = "Game Over!";
    }

    /// <summary> �f�o�b�O�p </summary>
    private void DebugFunction()
    {
        
    }
    #endregion
}
