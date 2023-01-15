using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// 保持するデータ
/// </summary>
class _Data
{
    // アクセスするようのキー
    public string Key;
    // リソース名
    public string ResName;
    // AudioClip
    public AudioClip Clip;

    // コンストラクタ
    public _Data(string key, string res)
    {
        Key = key;
        ResName = "BGM_SE/" + res;
        // AudioClip の取得
        Clip = Resources.Load(ResName) as AudioClip;
    }
}

// サウンド管理
public class Sound2D
{
    // SEチャンネル数
    const int SE_CHANNEL = 4;

    // サウンド種別
    enum eType
    {
        BGM, // BGM
        SE,  // SE
    }

    // シングルトン
    static Sound2D _Singleton = null;

    // インスタンス取得
    public static Sound2D GetInstance()
    {
        // null であれば右辺を返す
        return _Singleton ?? (_Singleton = new Sound2D());
    }

    // サウンド再生のためのゲームオブジェクト
    GameObject _Object = null;
    // サウンドリソース
    AudioSource _SourceBGM = null; // BGM
    AudioSource _SourceSeDefault = null; // SE（チャンネル）
    AudioSource[] _SourceSEArray; // SE（チャンネル）
    // BGM にアクセスするためのテーブル
    Dictionary<string, _Data> _PoolBGM = new Dictionary<string, _Data>();
    // SE にアクセスするためのテーブル
    Dictionary<string, _Data> _PoolSE = new Dictionary<string, _Data>();

    // コンストラクタ
    public Sound2D()
    {
        // チャンネル確保
        _SourceSEArray = new AudioSource[SE_CHANNEL];
    }

    // AudioSource を取得する
    AudioSource _GetAudioSource(eType type,int channel = -1)
    {
        if (_Object == null)
        {
            // GameObject がなければ作る
            _Object = new GameObject("Sound");
            // 破棄しないようにする
            GameObject.DontDestroyOnLoad(_Object);
            // AudioSource を作成
            _SourceBGM = _Object.AddComponent<AudioSource>();
            _SourceSeDefault = _Object.AddComponent<AudioSource>();
            for(int i = 0; i < SE_CHANNEL; i++)
            {
                _SourceSEArray[i] = _Object.AddComponent<AudioSource>();
            }
        }

        if (type == eType.BGM)
        {   // BGM
            return _SourceBGM;
        }
        else
        {   // SE
            if (0 <= channel && channel < SE_CHANNEL)
            {   // チャンネル指定
                return _SourceSEArray[channel];
            }
            else
            {   // デフォルト
                return _SourceSeDefault;
            }
        }
    }

    // サウンドのロード
    // ※Resources/Sounds フォルダに配置すること
    public static void LoadBGM(string key,string resName)
    {
        GetInstance()._LoadBGM(key, resName);
    }
    public static void LoadSE(string key,string resName)
    {
        GetInstance()._LoadSE(key, resName);
    }
    void _LoadBGM(string key,string resName)
    {
        if (_PoolBGM.ContainsKey(key))
        {   // すでに登録済みなのでいったん消す
            _PoolBGM.Remove(key);
        }
        _PoolBGM.Add(key, new _Data(key, resName));
    }
    void _LoadSE(string key, string resName)
    {
        if (_PoolSE.ContainsKey(key))
        {   // すでに登録済みなのでいったん消す
            _PoolSE.Remove(key);
        }
        _PoolSE.Add(key, new _Data(key, resName));
    }

    /// <summary>
    /// BGM の再生
    /// ※ 事前に LoadBGM でロードしておくこと
    /// </summary>
    public static bool PlayBGM(string key)
    {
        return GetInstance()._PlayBGM(key);
    }
    bool _PlayBGM(string key)
    {
        if (_PoolBGM.ContainsKey(key) == false)
        {   // 対応するキーがない
            return false;
        }

        // いったん止める
        _StopBGM();

        // リソースの取得
        var _data = _PoolBGM[key];

        // 再生
        var source = _GetAudioSource(eType.BGM);
        source.loop = true;
        source.clip = _data.Clip;
        source.Play();

        return true;
    }
    // BGM の停止
    public static bool StopBGM()
    {
        return GetInstance()._StopBGM();
    }
    bool _StopBGM()
    {
        _GetAudioSource(eType.BGM).Stop();

        return true;
    }

    /// <summary>
    /// SE の再生
    /// ※ 事前に LoadSEでロードしておくこと
    /// </summary>
    public static bool PlaySE(string key,int channel = -1)
    {
        return GetInstance()._PlaySE(key, channel);
    }
    bool _PlaySE(string key, int channel = -1)
    {
        if (_PoolSE.ContainsKey(key) == false)
        {   // 対応するキーがない
            return false;
        }

        // リソースの取得
        var _data = _PoolSE[key];

        if (0 <= channel && channel < SE_CHANNEL)
        {   // チャンネル指定
            var source = _GetAudioSource(eType.SE, channel);
            source.clip = _data.Clip;
            source.Play();
        }
        else
        {   // デフォルトで再生
            var source = _GetAudioSource(eType.SE);
            source.PlayOneShot(_data.Clip);
        }

        return true;
    }

    ///// <summary>
    ///// 音源の一括ロード
    ///// </summary>
    ///// <param name="BGMName"> BGMの名前</param>
    ///// <param name="SENames"> SEの名前（複数宣言可能だが、SE_CHANNELの数値と一致させる必要がある）</param>
    //public void AllSoundsLoad(string BGMName,string[] SENames)
    //{
    //    // BGMのロード
    //    LoadBGM(BGMName, BGMName);

    //    for(int i = 0; i < SENames.Length; i++)
    //    {
    //        LoadSE(SENames[i], SENames[i]);
    //    }
    //}

    /// <summary>
    /// BGMとSEのオブジェクトを削除しデータを初期化
    /// シーン遷移するときなどに定義しないとAudioSourceが圧迫する
    /// </summary>
    public static bool BGMAndSEResets()
    {
        return GetInstance()._BGMAndSEResets();
    }
    bool _BGMAndSEResets()
    {
        MonoBehaviour.Destroy(_Object);
        _PoolBGM.Clear();
        _PoolSE.Clear();

        return true;
    }
}
