using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollapseProvider
{
    public delegate void EventMethod(int value, GameObject ball);
	public static event EventMethod onCollapse;

	private static GameObject particles;

	public static void Collapse(GameObject ball1, GameObject ball2)
	{
		if(GameManager.Instance.IsMainCollapsed) return;

		Vector3 avgPos = (ball1.transform.position + ball2.transform.position) / 2f;
		int oldValue = ball1.GetComponent<IMergable>().Value;
		int newValue = oldValue + 1;

		Vector2 velocity = (ball1.GetComponent<Rigidbody2D>().velocity + ball2.GetComponent<Rigidbody2D>().velocity) / 2f;		

		Creator.Instance.DeleteBall(ball1);
		Creator.Instance.DeleteBall(ball2);

		ShowParticles(avgPos);

		if(GameManager.Instance.isVibrationEnabled) VibratorWrapper.Vibrate(0.3f);

		GameObject ball3 = Creator.Instance.Ball(newValue);
		ball3.transform.position = avgPos;
		ball3.GetComponent<Rigidbody2D>().velocity = velocity;

		if(onCollapse != null)
			onCollapse(newValue, ball3);
	}

	public static void MainCollapse(GameObject ball)
	{
		if(GameManager.Instance.IsMainCollapsed) return;
		
		Creator.Instance.DeleteBall(ball);
		GameManager.Instance.OnMainCollapse();
	}

	private static void ShowParticles(Vector3 pos)
	{
		if(particles == null)
			particles = Resources.Load<GameObject>("Prefabs/CollapseParticles");

		GameObject gameObject = GameObject.Instantiate(particles);
		gameObject.transform.position = pos;
	}
}
