/// <summary> 松島 </summary>
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    /// <summary> ソースを書くときのレンプレート </summary>

    #region define

    #endregion

    #region serialize field
    [SerializeField] private Sprite _BrankSprite;
    [SerializeField] private Sprite _HeartSprite;
    [SerializeField] private int _MaxHandHP = 50;

    [SerializeField] bool IsDebugMode = false;
    #endregion

    #region field
    /// <summary> ステージ上 </summary>
    int _CurrentPrincessLife = 4;
    int _PrevPrincessLife = 4;

    /// <summary> デフォルトパネル以下のオブジェクトの変数 </summary>
    private GameObject _DefaultPanel;
    private TextMeshProUGUI _MessageText;
    private TextMeshProUGUI _TimerText;

    /// <summary> ライフパネル以下のオブジェクトの変数 </summary>
    private GameObject _PrincessLifePanel;
    private TextMeshProUGUI _LevelText;

    /// <summary> ゲームモードのステートを取得 </summary>
    private GameModeStateEnum _CurrentInGameState;
    private GameModeStateEnum _PrevInGameState;

    /// <summary> 手のライフを含むを取得 </summary>
    private GameObject _HandLifePanel;
    private PlayerData _LeftHand;
    private PlayerData _RightHand;
    private Slider _LeftSlider;
    private Slider _RightSlider;

    private GameObject _Desk;
    private bool _IsFinishSetUp = false;

    /// <summary> 方向設定パネル以下のオブジェクトの変数 </summary>
    private GameObject _DirectionSettingPanel;

    #endregion

    #region property

    #endregion

    #region Unity function
    // Start is called before the first frame update
    /// <summary> GameModeControllerのStart関数でアクセスできるようにするため </summary>
    private void Awake()
    {
        /// <summary> デフォルトパネル以下のオブジェクトのセットアップ </summary>
        _DefaultPanel = transform.GetChild(0).gameObject;
        var messageObj = _DefaultPanel.transform.GetChild(0).gameObject;
        _MessageText = messageObj.GetComponent<TextMeshProUGUI>();
        var timerObj = _DefaultPanel.transform.GetChild(1).gameObject;
        _TimerText = timerObj.GetComponent<TextMeshProUGUI>();
        _TimerText.text = "00 : 00";

        /// <summary> ライフパネル以下のオブジェクトのセットアップ </summary>
        _PrincessLifePanel = _DefaultPanel.transform.GetChild(2).gameObject;
        _LevelText = _PrincessLifePanel.transform.Find("LevelText").gameObject.GetComponent<TextMeshProUGUI>();

        /// <summary> 手のライフを含むを取得 </summary>
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
    /// 状態の変更
    /// </summary>
    private void ChangeUIState()
    {
        // ログを出す
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
    /// 状態毎の毎フレーム呼ばれる処理
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
                    // 時間の更新
                    UI_TimerUpdate(GameModeController.Instance.GameTime);
                }
                break;
            case GameModeStateEnum.HandsSetUp:
                {
                    // 一度だけ机の高さに合わせてCanvasを移動させる
                    if(!_IsFinishSetUp && _Desk.GetComponent<DeskBehaviour>().IsFinishSetUp)
                    {
                        SetPositon();
                        _IsFinishSetUp = true;
                    }
                }
                break;
            case GameModeStateEnum.Play:
                {
                    // 時間の更新
                    UI_TimerUpdate(GameModeController.Instance.GameTime);

                    // 姫のライフの更新
                    if (IsLifeChange()) UpdateLifeIcon();

                    // 両手のライフの更新
                    UpdateHandLife();

                    // レベル表示の更新
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
    /// ちょうどそのステートに入った所かどうか
    /// </summary>
    /// <returns></returns>
    private bool IsEntryThisState()
    {
        return (_PrevInGameState != _CurrentInGameState);
    }

    /// <summary>
    /// 机の高さに合わせる
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

    /// <summary> タイマーの更新 </summary>
    public void UI_TimerUpdate(float timeF)
    {
        int time = 0;
        if(_CurrentInGameState == GameModeStateEnum.Play) time = GameModeController.Instance.LimitTime - (int)timeF;
        else time = (int)timeF;

        int seconds = 0;
        int minutes = 0;

        string secondsT = "0";
        string minutesT = "0";

        // 分数・秒数の振り分け
        if (time > 59)
        {
            minutes = time / 60;
            seconds = time - (minutes * 60);
        }
        else
        {
            seconds = time;
        }

        // 分数表示用テキスト作成
        if (seconds > 9)
        {
            secondsT = seconds.ToString();
        }
        else
        {
            secondsT += seconds.ToString();
        }

        // 秒数表示用テキスト作成
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
    /// ライフが減ったかどうか
    /// </summary>
    /// <returns></returns>
    private bool IsLifeChange()
    {
        return (_PrevPrincessLife != _CurrentPrincessLife);
    }

    /// <summary>
    /// ライフアイコンを増減させる
    /// </summary>
    private void UpdateLifeIcon()
    {
        // ライフが0以下になったら、全てブランクアイコンにして、リターン
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

        // ライフアイコンを一斉更新
        for (int i = 0; i < 5; i++)
        {
            GameObject lifeIcon = _PrincessLifePanel.transform.GetChild(i).gameObject;
            Image lifeImage = lifeIcon.GetComponent<Image>();

            // ライフ以下は、ハート
            if(i <= index) lifeImage.sprite = _HeartSprite;
            // ライフより上は、ブランク
            else lifeImage.sprite = _BrankSprite;
        }
    }

    /// <summary>
    /// 両手のライフゲージを更新する
    /// </summary>
    private void UpdateHandLife()
    {
        _LeftSlider.value = (float)_LeftHand.Life / (float)_MaxHandHP;
        _RightSlider.value = (float)_RightHand.Life / (float)_MaxHandHP;
    }

    /// <summary> UIをGameOver状態に遷移させる </summary>
    private void UI_ShowMessage(GameModeStateEnum gameModeStateEnum)
    {
        _MessageText.text = "" + gameModeStateEnum;
    }

    /// <summary> UIをGameClear状態に遷移させる </summary>
    private void UI_ToGameClear()
    {
        _MessageText.text = "Game Clear!";
    }

    /// <summary> UIをGameOver状態に遷移させる </summary>
    private void UI_ToDead()
    {
        _MessageText.text = "Game Over!";
    }

    /// <summary> デバッグ用 </summary>
    private void DebugFunction()
    {
        
    }
    #endregion
}
