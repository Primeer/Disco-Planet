using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaxBallBooster : Booster
{
	private int maxValue = 1;

	public override void OnReady()
	{
		foreach(GameObject ball in Creator.Instance.balls)
		{
			if(ball == Thrower.ball) continue;

			int value = ball.GetComponent<Ball>().Value;
			if(value > maxValue)
				maxValue = value;
		}
	}
	
	public override void Activate()
	{
		GameObject ball = Creator.Instance.Ball(maxValue);
		ball.GetComponent<Rigidbody2D>().simulated = false;
		GameManager.Instance.thrower.SetShell(ball);
		Thrower.onThrow += Finish;
	}

	private void Finish()
	{
		Thrower.onThrow -= Finish;
		Complete();
	}
}
