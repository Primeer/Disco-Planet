using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QualityBooster : Booster
{
	[SerializeField] private float throwCount;

	private float count;

	public override void Activate()
	{
		BallManager.Instance.SetQuality(true);
		
		InputSystem.onBallButtonRelease += Count;
	}

	private void Count(Vector2 pos)
	{
		count++;

		Controller.maskImage.fillAmount = (throwCount - count) / throwCount;

		if (count == throwCount)
		{
			BallManager.Instance.SetQuality(false);
			InputSystem.onBallButtonRelease -= Count;
			Complete();
		}
	}
}
