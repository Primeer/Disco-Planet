using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBooster : Booster
{
	public GameObject bombPrefab;

	public override void Activate()
	{
		GameObject bomb = Instantiate(bombPrefab);
		bomb.GetComponent<Rigidbody2D>().simulated = false;
		GameManager.Instance.thrower.SetShell(bomb);
		Thrower.onThrow += Finish;
	}

	private void Finish()
	{
		Thrower.onThrow -= Finish;
		Complete();
	}
}
