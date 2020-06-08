using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSystem : MonoBehaviour
{
	public delegate void InputMethod(Vector2 pos);
	public static event InputMethod onBallButtonTap;
	public static event InputMethod onBallButtonDrag;
	public static event InputMethod onBallButtonRelease;

	private bool isBallButton;
	private bool isBoosterButton;

	public void BallAreaTap(Vector2 pos)
	{
		if (onBallButtonTap != null && isBallButton)
			onBallButtonTap((Vector2)Camera.main.ScreenToWorldPoint((Vector3)pos));
	}

	public void BallAreaDrag(Vector2 pos)
	{
		if (onBallButtonDrag != null && isBallButton)
		{
			Vector2 posInArea = (Vector2)Camera.main.ScreenToWorldPoint((Vector3)pos);
			float x = Mathf.Clamp(posInArea.x, -1.4f, 1.4f);
			onBallButtonDrag(new Vector2(x, posInArea.y));
		}
	}

	public void BallAreaRelease(Vector2 pos)
	{
		if (onBallButtonRelease != null && isBallButton)
		{
			Vector2 posInArea = (Vector2)Camera.main.ScreenToWorldPoint((Vector3)pos);
			float x = Mathf.Clamp(posInArea.x, -1.4f, 1.4f);
			onBallButtonRelease(new Vector2(x, posInArea.y));
		}
	}

	public void VibroToggle(bool flag) => GameManager.Instance.isVibrationEnabled = flag;

	public void Booster1Click()
	{
		if(isBoosterButton)
			BoosterManager.Instance.ActivateBooster(0);
	}

	public void Booster2Click()
	{
		if(isBoosterButton)
			BoosterManager.Instance.ActivateBooster(1);
	}

	public void InputEnabled(bool flag)
	{
		BallButtonEnabled(flag);
		BoosterButtonEnabled(flag);
	}

	public void BallButtonEnabled(bool flag) => isBallButton = flag;
	
	public void BoosterButtonEnabled(bool flag) => isBoosterButton = flag;
}
