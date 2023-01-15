//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

///// <summary>
///// �ێ�����f�[�^
///// </summary>
//class _Data
//{
//    // �A�N�Z�X����悤�̃L�[
//    public string Key;
//    // ���\�[�X��
//    public string ResName;
//    // AudioClip
//    public AudioClip Clip;

//    // �R���X�g���N�^
//    public _Data(string key, string res)
//    {
//        Key = key;
//        ResName = "BGM_SE/" + res;
//        // AudioClip �̎擾
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

//    // SE�`�����l����
//    const int SE_CHANNEL = 4;

//    List<SoundObj> _SoundList = new List<SoundObj>();

//    // �C���X�^���X�擾
//    public static Sound3D Sound3DGetInstance(GameObject soundObj)
//    {
//        // null �ł���ΉE�ӂ�Ԃ�
//        return _Singleton ?? (_Singleton = new Sound3D());
//    }

//    class _AudioData
//    {
//        // �T�E���h���\�[�X
//        public AudioSource _SourceSeDefault = null; // SE�i�`�����l���j
//        public AudioSource[] _SourceSEArray; // SE�i�`�����l���j
//                                             // SE �ɃA�N�Z�X���邽�߂̃e�[�u��
//        public Dictionary<string, _Data> _PoolSE = new Dictionary<string, _Data>();
//    }

//    // �R���X�g���N�^
//    public Sound3D()
//    {
//        _audioData = new _AudioData();

//        // �`�����l���m��
//        _audioData._SourceSEArray = new AudioSource[SE_CHANNEL];
//    }

//    // AudioSource ���擾����
//    AudioSource _GetAudioSource(GameObject soundObj, int channel = -1)
//    {
//        if (soundObj.transform.Find("Sound") == null)
//        {
//            // GameObject ���Ȃ���΍��
//            _Object = new GameObject("Sound");
//            // Sound3DController ��ǉ�
//            _Object.AddComponent<Sound3DController>();
//            // �炵�����I�u�W�F�N�g�̎q�I�u�W�F�N�g�ɂȂ�
//            _Object.transform.parent = soundObj.transform;
//            // AudioSource ���쐬
//            soundObj.GetComponent<Sound3DController>().audioData._SourceSeDefault = _Object.AddComponent<AudioSource>();
//            for (int i = 0; i < SE_CHANNEL; i++)
//            {
//                soundObj.GetComponent<Sound3DController>().audioData._SourceSEArray[i] = _Object.AddComponent<AudioSource>();
//            }
//        }

//        // SE
//        if (0 <= channel && channel < SE_CHANNEL)
//        {   // �`�����l���w��
//            return soundObj.GetComponent<Sound3DController>().audioData._SourceSEArray[channel];
//        }
//        else
//        {   // �f�t�H���g
//            return soundObj.GetComponent<Sound3DController>().audioData._SourceSeDefault;
//        }
//    }

//    // �T�E���h�̃��[�h
//    // ��Resources/Sounds �t�H���_�ɔz�u���邱��
//    public static void LoadSE(GameObject soundObj, string key, string resName)
//    {
//        Sound3DGetInstance(soundObj)._LoadSE(soundObj, key, resName);
//    }
//    void _LoadSE(GameObject soundObj, string key, string resName)
//    {
//        if (soundObj.GetComponent<Sound3DController>().audioData._PoolSE.ContainsKey(key))
//        {   // ���łɓo�^�ς݂Ȃ̂ł����������
//            soundObj.GetComponent<Sound3DController>().audioData._PoolSE.Remove(key);
//        }
//        soundObj.GetComponent<Sound3DController>().audioData._PoolSE.Add(key, new _Data(key, resName));
//    }

//    /// <summary>
//    /// SE �̍Đ�
//    /// �� ���O�� LoadSE�Ń��[�h���Ă�������
//    /// </summary>
//    public static bool PlaySE(GameObject soundObj, string key, int channel = -1)
//    {
//        return Sound3DGetInstance(soundObj)._PlaySE(soundObj, key, channel);
//    }
//    bool _PlaySE(GameObject soundObj, string key, int channel = -1)
//    {
//        if (soundObj.GetComponent<Sound3DController>().audioData._PoolSE.ContainsKey(key) == false)
//        {   // �Ή�����L�[���Ȃ�
//            return false;
//        }

//        // ���\�[�X�̎擾
//        var _data = soundObj.GetComponent<Sound3DController>().audioData._PoolSE[key];

//        if (0 <= channel && channel < SE_CHANNEL)
//        {   // �`�����l���w��
//            var source = _GetAudioSource(soundObj, channel);
//            source.clip = _data.Clip;
//            source.Play();
//        }
//        else
//        {   // �f�t�H���g�ōĐ�
//            var source = _GetAudioSource(soundObj);
//            source.PlayOneShot(_data.Clip);
//        }

//        return true;
//    }
//}
