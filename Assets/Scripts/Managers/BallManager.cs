using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : Singleton<BallManager>
{
	private int max_diff_balls_per_round;
	private int tir;
	private int difTir;
	private int minValue;
	private int maxValue;
	private int maxSpawnValue;

	private Creator creator;
	private Thrower thrower;

	private bool isQuality;

	private void OnEnable() {
		thrower = GameManager.Instance.thrower;
		thrower.onThrowerReady += CreateThrowerBall;
	}

	private void OnDisable() {
		thrower.onThrowerReady -= CreateThrowerBall;
	}

	public void CreateThrowerBall()
	{
		int value = GetRandomBallValue();
		GameObject ball = Creator.Instance.Ball(value);

		ball.GetComponent<Rigidbody2D>().simulated = false;

		thrower.SetShell(ball);
	}

	public void RecalculateValueBounds(int mainBallValue)
	{
		max_diff_balls_per_round = Random.Range(9, 12);
		tir = Random.Range(1, 7);
		
		if (tir == 1 && max_diff_balls_per_round == 11) tir++;
		difTir = mainBallValue - (tir + 9);
		maxValue = mainBallValue;
		minValue = mainBallValue - max_diff_balls_per_round + 1;
		if (minValue < 1) minValue = 1;
		maxSpawnValue = minValue + 4;
	}

	public int GetRandomBallValue()
	{
		int val = isQuality ? 1 : 0;
		int raw = isQuality ? Random.Range(0, 6) + 3 : Random.Range(0, 9);

		if (raw <= 2) val += minValue;
		else if (raw <= 5) val += minValue + 1;
		else if (raw <= 7) val += minValue + 2;
		else if (raw == 8) val += minValue + 3;
		return val;
	}

	public float GetScaleKoef(int val)
	{
		return Mathf.Pow(1.1f, val - minValue);
	}

	public float GetWeightKoef(int val)
	{
		int pow = GameManager.Instance.Level > 15 ? val - difTir - 1 : val - 1;
		pow *= 2;

		return Mathf.Pow(1.1f, pow);
	}

	public int GetSpriteNumber(int val)
	{
		return val - minValue + 2;
	}

	public void SetQuality(bool flag) => isQuality = flag;
}
