using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour, IPushable, IInitializable, IMergable
{
	[SerializeField] private float startSpeed = 10f;
	[SerializeField] private float gravitationForce = 1f;
	[SerializeField] private float mass = 0.05f;
	[SerializeField] private float drag = 1f;
	
	public bool isGravitation;

	private Rigidbody2D rigidbody;
	private Transform mainBall;
	private Vector3 mainBallPos;
	private ContactFilter2D contactFilter = new ContactFilter2D().NoFilter();

	private State state;
	private enum State
	{
		Pushed,
		Attached,
		Free
	}

	private int value;
	public int Value
	{
		get { return value;  }
		set
		{
			this.value = value;

			SetScale(value);
			SetWeight(value);
			SetSprite(value);
		}
	}

	private void FixedUpdate()
	{
		if (isGravitation)
			rigidbody.AddForce(((Vector2)mainBallPos - rigidbody.position).normalized * gravitationForce);

		if (state == State.Pushed)
			if (rigidbody.position.y > mainBallPos.y - 0.5f)
			{
				Debuff();
				state = State.Free;
			}
	}

	private void OnCollisionEnter2D(Collision2D other) {
		if(!gameObject.activeSelf) return;
		if(!other.gameObject.activeSelf) return;
		
		if(state == State.Pushed)
		{
			Debuff();
			state = State.Free;
		}

		if(state != State.Attached)
		{
			transform.parent = mainBall;
			state = State.Attached;
		}

		IMergable ball2 = other.gameObject.GetComponent<IMergable>();

		if(ball2 != null && Value == ball2.Value)
		{
			if(other.gameObject.tag == "Main Ball")
				CollapseProvider.MainCollapse(gameObject);
			else
				CollapseProvider.Collapse(gameObject, other.gameObject);
		}
	}

	private void OnCollisionExit2D(Collision2D other) {
		if(!gameObject.activeSelf) return;

		if(!rigidbody.IsTouching(contactFilter)) 
		{
			// Debuff();
			transform.parent = null;
			state = State.Free;
		}
	}
	
	public void Initialize()
	{
		rigidbody = GetComponent<Rigidbody2D>();
		rigidbody.drag = drag;
        rigidbody.angularDrag = drag;
		rigidbody.velocity = Vector2.zero;
		isGravitation = true;

		mainBall = GameManager.Instance.mainBall.transform;
		mainBallPos = mainBall.position;

		state = State.Free;
	}
	
	public void Push()
	{
		transform.parent = null;
		rigidbody.simulated = true;

		rigidbody.velocity = Vector2.up * startSpeed;
		Buff();
		
		state = State.Pushed;
	}

	private void Buff()
	{
		rigidbody.mass *= 3;
        rigidbody.drag = 0;
        rigidbody.angularDrag = 0;
		isGravitation = false;
	}

	private void Debuff()
	{
		SetWeight(Value);
		rigidbody.drag = drag;
        rigidbody.angularDrag = drag;
		rigidbody.velocity = Vector2.ClampMagnitude(rigidbody.velocity, 5f);
		isGravitation = true;
	}

	private void SetScale(int val)
    {
        float koef = BallManager.Instance.GetScaleKoef(val);
        Transform par = transform.parent;
        transform.parent = null;
        transform.localScale = Vector3.one * koef;
        transform.parent = par;
    }

    private void SetWeight(int val)
    {
        rigidbody.mass = mass * BallManager.Instance.GetWeightKoef(val);
    }

	private void SetSprite(int val)
	{
		GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>($"Balls/{BallManager.Instance.GetSpriteNumber(val)}");
	}
}
