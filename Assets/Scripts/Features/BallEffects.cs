using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallEffects : MonoBehaviour
{
	private static GameObject winEffect;
	private static GameObject defeatEffect;
	private static GameObject effect;

	public static void Explosion(float force)
	{
		Vector2 center = (Vector2)GameManager.Instance.mainBall.transform.position;

		foreach(GameObject ball in Creator.Instance.balls)
		{
			Rigidbody2D rig = ball.GetComponent<Rigidbody2D>();

			rig.AddForce((center - rig.position).normalized * force * rig.mass * 20f);
		}
	}

	public static void SetGravitationEnable(bool flag)
	{
		foreach(GameObject ball in Creator.Instance.balls)
		{
			Ball ballObj = ball.GetComponent<Ball>();

			ballObj.isGravitation = flag;
		}
	}

	public static void IncreeseValue(IMergable obj, int count)
	{
		if(obj == null) return;

		int maxValue = GameManager.Instance.Level;
		int value = obj.Value + count;
		value = Mathf.Clamp(value, 0, maxValue);
		obj.Value = value;
	}

	public static void StartLevelCompleteEffect(float force)
	{
		if(winEffect == null)
			winEffect = Resources.Load<GameObject>("Prefabs/Win");

		effect = GameObject.Instantiate(winEffect);
		int spriteIndex = BallManager.Instance.GetSpriteNumber(GameManager.Instance.Level);
		effect.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Balls/{spriteIndex}");

		SetGravitationEnable(false);
		Explosion(force);
	}

	public static void StopLevelCompleteEffect()
	{
		Destroy(effect);
		SetGravitationEnable(true);
	}

	public static void StartDefeatEffect(float force)
	{
		if(defeatEffect == null)
			defeatEffect = Resources.Load<GameObject>("Prefabs/Loose");

		effect = GameObject.Instantiate(defeatEffect);
		int spriteIndex = BallManager.Instance.GetSpriteNumber(GameManager.Instance.Level);
		effect.GetComponentInChildren<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Balls/{spriteIndex}");

		SetGravitationEnable(false);
		Explosion(force);
	}

	public static void StopDefeatEffect()
	{
		Destroy(effect);
		SetGravitationEnable(true);
	}
}
