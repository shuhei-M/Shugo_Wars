//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

///// <summary>
///// 保持するデータ
///// </summary>
//class _Data
//{
//    // アクセスするようのキー
//    public string Key;
//    // リソース名
//    public string ResName;
//    // AudioClip
//    public AudioClip Clip;

//    // コンストラクタ
//    public _Data(string key, string res)
//    {
//        Key = key;
//        ResName = "BGM_SE/" + res;
//        // AudioClip の取得
//        Clip = Resources.Load(ResName) as AudioClip;
//    }
//}



//public class Sound3D
//{
//    struct SoundObj
//    {
//        public GameObject Object;
//        public _AudioData AudioData;
//    }

//    // SEチャンネル数
//    const int SE_CHANNEL = 4;

//    List<SoundObj> _SoundList = new List<SoundObj>();

//    // インスタンス取得
//    public static Sound3D Sound3DGetInstance(GameObject soundObj)
//    {
//        // null であれば右辺を返す
//        return _Singleton ?? (_Singleton = new Sound3D());
//    }

//    class _AudioData
//    {
//        // サウンドリソース
//        public AudioSource _SourceSeDefault = null; // SE（チャンネル）
//        public AudioSource[] _SourceSEArray; // SE（チャンネル）
//                                             // SE にアクセスするためのテーブル
//        public Dictionary<string, _Data> _PoolSE = new Dictionary<string, _Data>();
//    }

//    // コンストラクタ
//    public Sound3D()
//    {
//        _audioData = new _AudioData();

//        // チャンネル確保
//        _audioData._SourceSEArray = new AudioSource[SE_CHANNEL];
//    }

//    // AudioSource を取得する
//    AudioSource _GetAudioSource(GameObject soundObj, int channel = -1)
//    {
//        if (soundObj.transform.Find("Sound") == null)
//        {
//            // GameObject がなければ作る
//            _Object = new GameObject("Sound");
//            // Sound3DController を追加
//            _Object.AddComponent<Sound3DController>();
//            // 鳴らしたいオブジェクトの子オブジェクトになる
//            _Object.transform.parent = soundObj.transform;
//            // AudioSource を作成
//            soundObj.GetComponent<Sound3DController>().audioData._SourceSeDefault = _Object.AddComponent<AudioSource>();
//            for (int i = 0; i < SE_CHANNEL; i++)
//            {
//                soundObj.GetComponent<Sound3DController>().audioData._SourceSEArray[i] = _Object.AddComponent<AudioSource>();
//            }
//        }

//        // SE
//        if (0 <= channel && channel < SE_CHANNEL)
//        {   // チャンネル指定
//            return soundObj.GetComponent<Sound3DController>().audioData._SourceSEArray[channel];
//        }
//        else
//        {   // デフォルト
//            return soundObj.GetComponent<Sound3DController>().audioData._SourceSeDefault;
//        }
//    }

//    // サウンドのロード
//    // ※Resources/Sounds フォルダに配置すること
//    public static void LoadSE(GameObject soundObj, string key, string resName)
//    {
//        Sound3DGetInstance(soundObj)._LoadSE(soundObj, key, resName);
//    }
//    void _LoadSE(GameObject soundObj, string key, string resName)
//    {
//        if (soundObj.GetComponent<Sound3DController>().audioData._PoolSE.ContainsKey(key))
//        {   // すでに登録済みなのでいったん消す
//            soundObj.GetComponent<Sound3DController>().audioData._PoolSE.Remove(key);
//        }
//        soundObj.GetComponent<Sound3DController>().audioData._PoolSE.Add(key, new _Data(key, resName));
//    }

//    /// <summary>
//    /// SE の再生
//    /// ※ 事前に LoadSEでロードしておくこと
//    /// </summary>
//    public static bool PlaySE(GameObject soundObj, string key, int channel = -1)
//    {
//        return Sound3DGetInstance(soundObj)._PlaySE(soundObj, key, channel);
//    }
//    bool _PlaySE(GameObject soundObj, string key, int channel = -1)
//    {
//        if (soundObj.GetComponent<Sound3DController>().audioData._PoolSE.ContainsKey(key) == false)
//        {   // 対応するキーがない
//            return false;
//        }

//        // リソースの取得
//        var _data = soundObj.GetComponent<Sound3DController>().audioData._PoolSE[key];

//        if (0 <= channel && channel < SE_CHANNEL)
//        {   // チャンネル指定
//            var source = _GetAudioSource(soundObj, channel);
//            source.clip = _data.Clip;
//            source.Play();
//        }
//        else
//        {   // デフォルトで再生
//            var source = _GetAudioSource(soundObj);
//            source.PlayOneShot(_data.Clip);
//        }

//        return true;
//    }
//}
