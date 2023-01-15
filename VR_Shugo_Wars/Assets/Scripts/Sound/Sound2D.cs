using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// �ێ�����f�[�^
/// </summary>
class _Data
{
    // �A�N�Z�X����悤�̃L�[
    public string Key;
    // ���\�[�X��
    public string ResName;
    // AudioClip
    public AudioClip Clip;

    // �R���X�g���N�^
    public _Data(string key, string res)
    {
        Key = key;
        ResName = "BGM_SE/" + res;
        // AudioClip �̎擾
        Clip = Resources.Load(ResName) as AudioClip;
    }
}

// �T�E���h�Ǘ�
public class Sound2D
{
    // SE�`�����l����
    const int SE_CHANNEL = 4;

    // �T�E���h���
    enum eType
    {
        BGM, // BGM
        SE,  // SE
    }

    // �V���O���g��
    static Sound2D _Singleton = null;

    // �C���X�^���X�擾
    public static Sound2D GetInstance()
    {
        // null �ł���ΉE�ӂ�Ԃ�
        return _Singleton ?? (_Singleton = new Sound2D());
    }

    // �T�E���h�Đ��̂��߂̃Q�[���I�u�W�F�N�g
    GameObject _Object = null;
    // �T�E���h���\�[�X
    AudioSource _SourceBGM = null; // BGM
    AudioSource _SourceSeDefault = null; // SE�i�`�����l���j
    AudioSource[] _SourceSEArray; // SE�i�`�����l���j
    // BGM �ɃA�N�Z�X���邽�߂̃e�[�u��
    Dictionary<string, _Data> _PoolBGM = new Dictionary<string, _Data>();
    // SE �ɃA�N�Z�X���邽�߂̃e�[�u��
    Dictionary<string, _Data> _PoolSE = new Dictionary<string, _Data>();

    // �R���X�g���N�^
    public Sound2D()
    {
        // �`�����l���m��
        _SourceSEArray = new AudioSource[SE_CHANNEL];
    }

    // AudioSource ���擾����
    AudioSource _GetAudioSource(eType type,int channel = -1)
    {
        if (_Object == null)
        {
            // GameObject ���Ȃ���΍��
            _Object = new GameObject("Sound");
            // �j�����Ȃ��悤�ɂ���
            GameObject.DontDestroyOnLoad(_Object);
            // AudioSource ���쐬
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
            {   // �`�����l���w��
                return _SourceSEArray[channel];
            }
            else
            {   // �f�t�H���g
                return _SourceSeDefault;
            }
        }
    }

    // �T�E���h�̃��[�h
    // ��Resources/Sounds �t�H���_�ɔz�u���邱��
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
        {   // ���łɓo�^�ς݂Ȃ̂ł����������
            _PoolBGM.Remove(key);
        }
        _PoolBGM.Add(key, new _Data(key, resName));
    }
    void _LoadSE(string key, string resName)
    {
        if (_PoolSE.ContainsKey(key))
        {   // ���łɓo�^�ς݂Ȃ̂ł����������
            _PoolSE.Remove(key);
        }
        _PoolSE.Add(key, new _Data(key, resName));
    }

    /// <summary>
    /// BGM �̍Đ�
    /// �� ���O�� LoadBGM �Ń��[�h���Ă�������
    /// </summary>
    public static bool PlayBGM(string key)
    {
        return GetInstance()._PlayBGM(key);
    }
    bool _PlayBGM(string key)
    {
        if (_PoolBGM.ContainsKey(key) == false)
        {   // �Ή�����L�[���Ȃ�
            return false;
        }

        // ��������~�߂�
        _StopBGM();

        // ���\�[�X�̎擾
        var _data = _PoolBGM[key];

        // �Đ�
        var source = _GetAudioSource(eType.BGM);
        source.loop = true;
        source.clip = _data.Clip;
        source.Play();

        return true;
    }
    // BGM �̒�~
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
    /// SE �̍Đ�
    /// �� ���O�� LoadSE�Ń��[�h���Ă�������
    /// </summary>
    public static bool PlaySE(string key,int channel = -1)
    {
        return GetInstance()._PlaySE(key, channel);
    }
    bool _PlaySE(string key, int channel = -1)
    {
        if (_PoolSE.ContainsKey(key) == false)
        {   // �Ή�����L�[���Ȃ�
            return false;
        }

        // ���\�[�X�̎擾
        var _data = _PoolSE[key];

        if (0 <= channel && channel < SE_CHANNEL)
        {   // �`�����l���w��
            var source = _GetAudioSource(eType.SE, channel);
            source.clip = _data.Clip;
            source.Play();
        }
        else
        {   // �f�t�H���g�ōĐ�
            var source = _GetAudioSource(eType.SE);
            source.PlayOneShot(_data.Clip);
        }

        return true;
    }

    ///// <summary>
    ///// �����̈ꊇ���[�h
    ///// </summary>
    ///// <param name="BGMName"> BGM�̖��O</param>
    ///// <param name="SENames"> SE�̖��O�i�����錾�\�����ASE_CHANNEL�̐��l�ƈ�v������K�v������j</param>
    //public void AllSoundsLoad(string BGMName,string[] SENames)
    //{
    //    // BGM�̃��[�h
    //    LoadBGM(BGMName, BGMName);

    //    for(int i = 0; i < SENames.Length; i++)
    //    {
    //        LoadSE(SENames[i], SENames[i]);
    //    }
    //}

    /// <summary>
    /// BGM��SE�̃I�u�W�F�N�g���폜���f�[�^��������
    /// �V�[���J�ڂ���Ƃ��Ȃǂɒ�`���Ȃ���AudioSource����������
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
