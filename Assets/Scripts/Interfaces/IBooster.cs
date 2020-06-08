using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBooster
{
	float Cooldown { get; }

	void Activate();
}
