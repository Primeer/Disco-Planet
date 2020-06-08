using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour, IPushable
{
	[SerializeField] private float flashingTime;
	[SerializeField] private GameObject activeBeam;
	[SerializeField] private Collider2D laserCollider;

	public void Push()
	{
		transform.parent = null;

		StartCoroutine(Flashing());

		Strike();
	}

	private IEnumerator Flashing()
	{
		activeBeam.SetActive(true);
		yield return new WaitForSeconds(flashingTime);
		Destroy(gameObject);
	}

	private void Strike()
    {
        List<Collider2D> colliders = new List<Collider2D>();
        
		Physics2D.OverlapCollider(laserCollider, new ContactFilter2D().NoFilter(), colliders);
        
		foreach(Collider2D coll in colliders) {
			IMergable mergeObj = coll.GetComponent<IMergable>();
			if(mergeObj != null)
				BallEffects.IncreeseValue(mergeObj, Random.Range(1, 3));
		}

		BallEffects.Explosion(6);
	}
}
