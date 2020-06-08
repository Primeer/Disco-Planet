using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainBall : MonoBehaviour, IMergable
{
	[SerializeField] private float rotationSpeed;

	private SpriteRenderer renderer;
	private int value;
	public int Value
	{
		get { return value; }
		set
		{
			this.value = value;

			SetScale(value);
			SetSprite(value);
		}
	}

	public bool Visible
	{
		get { return renderer.enabled; }
		set 
		{ 
			renderer.enabled = value;
			Rotation = value;
		}
	}
	public bool Rotation { get; set; }

	private void Awake() {
		renderer = GetComponent<SpriteRenderer>();
		Rotation = true;
	}
    
    void Update()
    {
		if(Rotation)
			transform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
	}

	private void SetScale(int val)
    {
        float koef = BallManager.Instance.GetScaleKoef(val);
        transform.localScale = Vector3.one * 1.5f * koef;
    }

	private void SetSprite(int val)
	{
		renderer.sprite = Resources.Load<Sprite>($"Balls/{BallManager.Instance.GetSpriteNumber(val)}");
	}
}
