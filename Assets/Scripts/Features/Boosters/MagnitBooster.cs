using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnitBooster : Booster
{
	[SerializeField] private float explosion;
	[SerializeField] private float delay;

	public override void Activate()
	{
		StartCoroutine(Execution());
	}

	private IEnumerator Execution()
	{
		BallEffects.Explosion(explosion);

		yield return new WaitForSeconds(delay);

		Merge();

		Complete();
	}

	private void Merge()
    {
		List<GameObject> list = new List<GameObject>(Creator.Instance.balls);
		list.Remove(Thrower.ball);

		for (int i = 0; i < list.Count - 1; i++)
			for (int j = i + 1; j < list.Count; j++)
			{
				if(list[i].GetComponent<IMergable>().Value == list[j].GetComponent<IMergable>().Value)
				{
					CollapseProvider.Collapse(list[i], list[j]);
					
					list.RemoveAt(j);
					list.RemoveAt(i);
					
					j = i;
				}
			}
	}
}
