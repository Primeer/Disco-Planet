using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Booster : MonoBehaviour 
{
	[SerializeField] private float maxCooldown;
	[SerializeField] private float minCooldown;

	public BoosterController Controller { get; set; }

	private float cooldown;
	public float Cooldown
	{
		get 
		{ 
			if(cooldown == 0) cooldown = Random.Range(minCooldown, maxCooldown);
			return cooldown;
		}
	}

	public virtual void OnReady() {}

	public abstract void Activate();

	protected void Complete()
	{
		Controller.Complete();
	}
}
