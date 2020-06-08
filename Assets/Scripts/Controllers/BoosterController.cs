using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class BoosterController : MonoBehaviour
{
	public GameObject prefab;
	public GameObject gameObj;
	public Booster booster;
	public State state;
	public enum State 
	{
		Disable,
		Ready,
		Active
	}

	public Image maskImage;
	public Button button;
	public bool ButtonEnabled
	{
		get { return button.interactable; }
		set { button.interactable = value && (state != State.Disable) && Enabled; }
	}
	public bool enabled;
	public bool Enabled 
	{
		get { return enabled; }
		set 
		{ 
			enabled = value;
			ButtonEnabled = value;
		}
	}

	private void Awake() {
		maskImage = GetComponent<Image>();
		button = GetComponent<Button>();
	}
	
	public void SetBooster(GameObject prefab)
	{
		this.prefab = prefab;
		
		if (booster != null) Destroy(gameObj);
		
		state = State.Disable;

		CreateBooster();

		StartCoroutine(Timer(booster.Cooldown));
	}

	private IEnumerator Timer(float time)
	{
		ButtonEnabled = false;

		float cd = time;
        while(cd > 0 && state != State.Ready) {
            yield return null;
            cd -= Time.deltaTime;
            maskImage.fillAmount = (time - cd) / time;
        }
		maskImage.fillAmount = 1;
		state = State.Ready;
		ButtonEnabled = true;
		Repaint(Color.white);
		booster.OnReady();
	}

	public void Activate()
	{
		if (state == State.Ready)
		{
			state = State.Active;
			Repaint(Color.yellow);
			booster.Activate();
		}
	}
	
	public void Complete() => BoosterManager.Instance.OnComplete(this);

	private void CreateBooster()
	{
		gameObj = Instantiate(prefab, transform);
		booster = gameObj.GetComponent<Booster>();
		booster.Controller = this;

		button.targetGraphic = gameObj.GetComponent<Image>();
	}

	private void Repaint(Color color)
	{
		ColorBlock colorBlock = button.colors;
		colorBlock.normalColor = color;
		button.colors = colorBlock;
	}
}
