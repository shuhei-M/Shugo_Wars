using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : SingletonMonoBehaviour<ItemManager>
{
	/// <summary> ソースを書くときのレンプレート </summary>

	#region define
	/// <summary>
	/// アイテムID ( 手動で追加する)
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
	//private int DefeatePoint;   // 敵を打ち負かした数を計測
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
		// キー入力でアイテム生成
		//DebugFunction();
	}
	#endregion

	#region public function
	/// <summary>
	/// アイテムを生成
	/// </summary>
	/// <param name="id">アイテムID</param>
	/// <param name="position">生成ポジション</param>
	/// <returns></returns>
	public GameObject GenerateItem(Vector3 position)
	{
		ItemID id = JudgeGenerateItem();
		
		if (id == ItemID.None)
		{
			return null;
		}

		// 場にあるアイテムが10個以上であれば生成しない
		if (_AliveItemCount >= _MaxAliveItemCount) return null;

		var index = (int)id;
		var prefab = _ItemList[index];
		if (index < 0 || _ItemList.Count <= index)
		{
			Debug.Log("indexが不正な値です!!!!!!!!!!!!!!!!!!!!!!");
			return null;
		}
		if (prefab == null)
		{
			Debug.Log("prefabが設定されていません!!!!!!!!!!!!!!!");
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
		int rnd = Random.Range(0, _DropEstablishment);   // 0 ～ 4

		// 乱数で外れた場合
		if(rnd > 0)
        {
			_DropEstablishment--;
			return id;
		}

		id = ItemID.Heal;

		// 姫のライフからドロップするアイテムを考える
		if (GameModeController.Instance.Princess.Life < 3) rnd = 0;
		else rnd = Random.Range(GameModeController.Instance.Princess.Life, 6);   // 1 ～ 5

		if (rnd == 5) id = ItemID.Exp;

		_DropEstablishment = _MaxDropEstablishment;

		return id;
	}

	private void DebugFunction()
    {
		// アイテムを生成
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
