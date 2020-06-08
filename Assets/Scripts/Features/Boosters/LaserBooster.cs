using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBooster : Booster
{
	public GameObject laserbPrefab;

    public override void Activate()
	{
		GameObject laser = Instantiate(laserbPrefab);
		GameManager.Instance.thrower.SetShell(laser);
		Thrower.onThrow += Finish;
	}

	private void Finish()
	{
		Thrower.onThrow -= Finish;
		Complete();
	}
}
