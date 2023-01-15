using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : SingletonMonoBehaviour<ItemManager>
{
	/// <summary> �\�[�X�������Ƃ��̃����v���[�g </summary>

	#region define
	/// <summary>
	/// �A�C�e��ID ( �蓮�Œǉ�����)
	/// </summary>
	public enum ItemID : int
	{
		None = -1,
		Heal,
		Exp,
	}
	#endregion

	#region serialize field
	[SerializeField] private List<GameObject> _ItemList = new List<GameObject>();

	[SerializeField, Range(5, 10)] private int _MaxDropEstablishment = 7;

	[SerializeField, Range(5, 10)] private int _MaxAliveItemCount = 10;
	#endregion

	#region field
	//private int DefeatePoint;   // �G��ł��������������v��
	private int _DropEstablishment;

	private int _AliveItemCount;
    #endregion

    #region property
    public int AliveItemCount 
	{
		get { return _AliveItemCount; }
		set { _AliveItemCount = value; }
	}
    #endregion

    #region Unity function
    // Start is called before the first frame update
    void Start()
    {
		_DropEstablishment = _MaxDropEstablishment;

		_AliveItemCount = 0;
	}

    // Update is called once per frame
    void Update()
    {
		// �L�[���͂ŃA�C�e������
		//DebugFunction();
	}
	#endregion

	#region public function
	/// <summary>
	/// �A�C�e���𐶐�
	/// </summary>
	/// <param name="id">�A�C�e��ID</param>
	/// <param name="position">�����|�W�V����</param>
	/// <returns></returns>
	public GameObject GenerateItem(Vector3 position)
	{
		ItemID id = JudgeGenerateItem();
		
		if (id == ItemID.None)
		{
			return null;
		}

		// ��ɂ���A�C�e����10�ȏ�ł���ΐ������Ȃ�
		if (_AliveItemCount >= _MaxAliveItemCount) return null;

		var index = (int)id;
		var prefab = _ItemList[index];
		if (index < 0 || _ItemList.Count <= index)
		{
			Debug.Log("index���s���Ȓl�ł�!!!!!!!!!!!!!!!!!!!!!!");
			return null;
		}
		if (prefab == null)
		{
			Debug.Log("prefab���ݒ肳��Ă��܂���!!!!!!!!!!!!!!!");
			return null;
		}
		var obj = Instantiate(prefab, position, Quaternion.identity);
		obj.transform.SetParent(transform);

		_AliveItemCount++;

		return obj;
	}
	#endregion

	#region private function
	private ItemID JudgeGenerateItem()
    {
		ItemID id = ItemID.None;
		int rnd = Random.Range(0, _DropEstablishment);   // 0 �` 4

		// �����ŊO�ꂽ�ꍇ
		if(rnd > 0)
        {
			_DropEstablishment--;
			return id;
		}

		id = ItemID.Heal;

		// �P�̃��C�t����h���b�v����A�C�e�����l����
		if (GameModeController.Instance.Princess.Life < 3) rnd = 0;
		else rnd = Random.Range(GameModeController.Instance.Princess.Life, 6);   // 1 �` 5

		if (rnd == 5) id = ItemID.Exp;

		_DropEstablishment = _MaxDropEstablishment;

		return id;
	}

	private void DebugFunction()
    {
		// �A�C�e���𐶐�
		if(Input.GetKeyDown(KeyCode.G))
        {
			Vector3 vector3 = new Vector3(
				-0.25f,
				GameModeController.Instance.StartHight,
				-0.5f);
			GenerateItem(vector3);
		}
	}
	#endregion
}
