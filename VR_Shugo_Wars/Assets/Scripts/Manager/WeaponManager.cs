using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : SingletonMonoBehaviour<WeaponManager>
{
    #region define
    enum Weapon
    {
		None = -1,
		Gun,
		Missile,
    }
	#endregion

	#region serialize field
	[SerializeField] private List<GameObject> _EnemyList = new List<GameObject>();
	[SerializeField] private List<GameObject> _WeaponList = new List<GameObject>();
	#endregion

	#region field
	GameObject gunObj;
	GameObject launcharObj;
	float dropProbability = 10; // 武器が落ちる確率
	#endregion

	#region property

	#endregion

	#region Unity function
	// Start is called before the first frame update
	void Start()
	{

	}

	// Update is called once per frame
	void Update()
	{
		// キー入力でアイテム生成
		DebugFunction();
	}
    #endregion

    #region public function
    public GameObject GenerateWeapon(GameObject Enemy, Vector3 spawnPos)
    {
		int weaponNum;

		// 確率計算
		int value = Random.Range(0, 100 + 1);
        if (value % dropProbability != 0)
        {
			return null;
        }

		// 自分が何の武器を落とすかを判定
		GameObject weapon;
		if (Enemy.tag == "enemy_soldier" && gunObj == null)
		{
			weaponNum = 0;
			weapon = _WeaponList[weaponNum];
			gunObj = Instantiate(weapon, spawnPos, Quaternion.identity);
		}
		else if (Enemy.tag == "enemy_ship_r")
		{
			weaponNum = 1;
			weapon = _WeaponList[weaponNum];
			launcharObj = Instantiate(weapon, spawnPos, Quaternion.identity);
		}
		else
		{
			return null;
		}

		//GameObject weapon = _WeaponList[0];
		//var obj = Instantiate(weapon, spawnPos, Quaternion.identity);
		return weapon;
    }
    #endregion

    #region private function
    private void DebugFunction()
	{
		// アイテムを生成
		if (Input.GetKeyDown(KeyCode.B))
		{
			Vector3 vector3 = new Vector3(
				-0.25f,
				GameModeController.Instance.StartHight,
				-0.5f);
			GenerateWeapon(_EnemyList[0], vector3);
		}
	}
	#endregion
}
