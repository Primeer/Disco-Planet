using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thrower : MonoBehaviour
{
	[SerializeField] private float cooldown = 0.3f;
	[SerializeField] private float shellOffset = 0.74f;

	public delegate void ThrowerMethod();
	public event ThrowerMethod onThrowerReady;
	public static event ThrowerMethod onThrow;
	public static GameObject ball;

	private IPushable shell;
	private WaitForSeconds cdTime;
	private AimLine line;

	private void Awake() {
		cdTime = new WaitForSeconds(cooldown);
		line = GetComponentInChildren<AimLine>();
	}

	private void Start() {
		StartCoroutine(Cooldown());
	}

	public void Move(Vector2 pos)
	{
		transform.position = new Vector3(pos.x, transform.position.y, 0f);

		line.Visible = !(pos == Vector2.zero);
	}

	public void Push()
	{
		shell.Push();
		ball = null;

		if(onThrow != null)
			onThrow();

		StartCoroutine(Cooldown());
	}

	public void SetShell(GameObject shl)
	{
		IPushable pushObj = shl.GetComponent<IPushable>();

		if(pushObj != null)
		{
			DeleteShell();

			ball = shl;
			ball.transform.parent = transform;
			ball.transform.localPosition = new Vector3(0f, shellOffset, 0f);

			shell = pushObj;
		}
	}

	private IEnumerator Cooldown()
	{
		yield return cdTime;

		if(onThrowerReady != null)
			onThrowerReady();
	}

	private void DeleteShell()
	{
		if(ball == null) return;
		if(!ball.activeSelf) return;

		if(ball.GetComponent<Ball>())
			Creator.Instance.DeleteBall(ball);
		else
			Destroy(ball);
	}
}
