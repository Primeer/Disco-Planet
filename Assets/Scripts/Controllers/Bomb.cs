using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour, IPushable
{
	[SerializeField] private float startSpeed = 10f;
	[SerializeField] private float explodeForce = 30f;
	[SerializeField] private float range = 1f;
	[SerializeField] private float drag = 0.8f;
	[SerializeField] private GameObject effect;

	private Rigidbody2D rigidbody;
	private Vector3 mainBallPos;
	private bool isGravitation;
	private bool isPushed;

	public void Push()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		mainBallPos = GameManager.Instance.mainBall.transform.position;

		transform.parent = null;
		rigidbody.simulated = true;

		Buff();
		rigidbody.velocity = Vector2.up * startSpeed;

		isPushed = true;
	}

	private void FixedUpdate()
	{
		if (isGravitation)
			rigidbody.AddForce(((Vector2)mainBallPos - rigidbody.position).normalized);

		if (isPushed)
			if (rigidbody.position.y > mainBallPos.y - 0.5f)
			{
				Debuff();
				isPushed = false;
			}
	}

	private void OnCollisionEnter2D(Collision2D other) {
		Explode();
		Destroy(gameObject);
	}

	private void Explode()
	{
		foreach(GameObject ball in Creator.Instance.balls)
		{
			Vector2 direction = (Vector2)ball.transform.position - rigidbody.position;
			float distance = direction.magnitude;
			ball.GetComponent<Rigidbody2D>().AddForce(direction.normalized * explodeForce / distance);

			if(distance <= range)
				BallEffects.IncreeseValue(ball.GetComponent<IMergable>(), Random.Range(1, 3));
		}

		effect?.SetActive(true);
		effect?.transform.SetParent(null);
	}

	private void Buff()
	{
        rigidbody.drag = 0;
        rigidbody.angularDrag = 0;
		isGravitation = false;
	}

	private void Debuff()
	{
		rigidbody.drag = drag;
        rigidbody.angularDrag = drag;
		rigidbody.velocity = Vector2.ClampMagnitude(rigidbody.velocity, 5f);
		isGravitation = true;
	}
}
