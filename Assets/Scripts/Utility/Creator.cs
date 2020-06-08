using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Lean.Pool;

public class Creator : Singleton<Creator>
{
	[SerializeField] private GameObject ballPrefab;

	public List<GameObject> balls = new List<GameObject>();

	public delegate void CreatorMethod(GameObject ball);
	public static event CreatorMethod onBallCreate;
	public static event CreatorMethod onBallDelete;

	public GameObject Ball(int val)
	{
		GameObject ball = LeanPool.Spawn(ballPrefab);

		IInitializable initObj = ball.GetComponent<IInitializable>();
		initObj?.Initialize();

		IMergable mergeObj = ball.GetComponent<IMergable>();
		mergeObj.Value = val;

		ball.name = $"Ball {val}";

		balls.Add(ball);

		if(onBallCreate != null)
			onBallCreate(ball);

		return ball;
	}

	public void DeleteBall(GameObject ball)
	{
		LeanPool.Despawn(ball);
		balls.Remove(ball);

		if(onBallDelete != null)
			onBallDelete(ball);
	}

	public void DeleteAll()
	{
		foreach(GameObject ball in balls)
			LeanPool.Despawn(ball);

		balls.Clear();
	}
}
