using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class GameManager : Singleton<GameManager>
{

	[SerializeField] private int startLevel = 7;
	[SerializeField] private float levelCooldown = 3f;
	[SerializeField] private Text levelText;
	
	public int Level
	{
		get { return mainBall.Value; }
		set 
		{
			if (value != mainBall.Value)
			{
				BallManager.Instance.RecalculateValueBounds(value);
				mainBall.Value = value;
				levelText.text = $"{value - startLevel + 1}";
			}
		}
	}
	public Thrower thrower;
	public MainBall mainBall;
	public bool IsMainCollapsed
	{
		get { return !mainBall.Visible; }
		set {}
	}
	public bool isVibrationEnabled;

	public delegate void EventMethod();
	public static event EventMethod onMainCollapse;
	public static event EventMethod onNextLevel;
	public static event EventMethod onOverflow;
	public static event EventMethod onRestart;

	private InputSystem inputSystem;
	private WaitForSeconds lvlCooldown;

	private void Start() {
		lvlCooldown = new WaitForSeconds(levelCooldown);
		inputSystem = GetComponent<InputSystem>();
		Level = startLevel;
	}

	private void OnEnable() {
		InputSystem.onBallButtonTap += BeginAiming;
		InputSystem.onBallButtonDrag += Aiming;
		InputSystem.onBallButtonRelease += EndAiming;

		thrower.onThrowerReady += OnThrowerReady;
	}

	private void OnDisable() {
		InputSystem.onBallButtonTap -= BeginAiming;
		InputSystem.onBallButtonDrag -= Aiming;
		InputSystem.onBallButtonRelease -= EndAiming;

		thrower.onThrowerReady -= OnThrowerReady;
	}

	private void BeginAiming(Vector2 pos)
	{
		thrower.Move(pos);
	}

	private void Aiming(Vector2 pos)
	{
		thrower.Move(pos);
	}

	private void EndAiming(Vector2 pos)
	{
		inputSystem.InputEnabled(false);
		
		thrower.Move(pos);
		thrower.Push();
		thrower.Move(Vector2.zero);
	}

	private void OnThrowerReady()
	{
		inputSystem.InputEnabled(true);
	}

	public void OnMainCollapse()
	{
		mainBall.Visible = false;

		BallEffects.StartLevelCompleteEffect(30f);

		inputSystem.InputEnabled(false);

		if(onMainCollapse != null)
			onMainCollapse();

		StartCoroutine(NextLevel());
	}

	private IEnumerator NextLevel()
	{
		yield return lvlCooldown;

		Level++;
		
		mainBall.Visible = true;

		BallEffects.StopLevelCompleteEffect();

		inputSystem.InputEnabled(true);

		if(onNextLevel != null)
			onNextLevel();
	}

	public void OnOverflow()
	{
		mainBall.Rotation = false;

		BallEffects.StartDefeatEffect(30f);

		inputSystem.InputEnabled(false);

		if(onOverflow != null)
			onOverflow();

		StartCoroutine(Restart());
	}

	private IEnumerator Restart()
	{
		yield return lvlCooldown;

		mainBall.Rotation = true;

		BallEffects.StopDefeatEffect();

		Creator.Instance.DeleteAll();
		thrower.Push();

		inputSystem.InputEnabled(true);

		if(onRestart != null)
			onRestart();
	}
}
