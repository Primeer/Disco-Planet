using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoosterManager : Singleton<BoosterManager>
{
	[SerializeField] private List<GameObject> boosterPrefabs;
	[SerializeField] private List<BoosterController> controllers;
	

	private void Start() {
		RefreshBoosters();
	}
	
	public void RefreshBoosters()
	{
		EnableBoosters();

		foreach(BoosterController ctrl in controllers)
		{
			ctrl.SetBooster(GetRandomPrefab());
		}
	}

	public void ActivateBooster(int index)
	{
		DisableBoosters(controllers[index]);
		controllers[index].Activate();
	}

	public void OnComplete(BoosterController ctrl)
	{
		ctrl.SetBooster(GetRandomPrefab());
		EnableBoosters();
	}

	public void EnableBoosters()
	{
		foreach(BoosterController ctrl in controllers)
		{
			ctrl.Enabled = true;
		}
	}

	private void DisableBoosters(BoosterController exclude)
	{
		foreach(BoosterController ctrl in controllers)
		{
			if(ctrl != exclude)
				ctrl.Enabled = false;
		}
	}

	public GameObject GetRandomPrefab()
	{
		GameObject prefab = boosterPrefabs[Random.Range(0, boosterPrefabs.Count)];
		
		while(HasBooster(prefab))
			prefab = boosterPrefabs[Random.Range(0, boosterPrefabs.Count)];

		return prefab;
	}

	private bool HasBooster(GameObject prefab)
	{
		foreach(BoosterController ctrl in controllers)
		{
			if(ctrl.prefab == prefab)
				return true;
		}
		return false;
	}

	public void SetBoostersState(BoosterController.State state)
	{
		foreach(BoosterController ctrl in controllers)
		{
			ctrl.state = state;
		}
	}
}
