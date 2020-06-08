using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshPro))]
public class Overflow : MonoBehaviour
{
    [SerializeField] private float max = 20f;
    private float count = 0f;
    private TextMeshPro textMeshPro;
    private Vector3 scale = Vector3.one;
    private float koef = 1f;  
    private bool isBlinked;
    private WaitForSeconds timeout;
    private bool isOver;

	private bool Visible
	{
		get { return textMeshPro.enabled; }
		set { textMeshPro.enabled = value; }
	}

    void Awake()
    {
        textMeshPro = GetComponent<TextMeshPro>();
        timeout = new WaitForSeconds(1.5f);
        Paint();
    }

	private void OnEnable() {
		Creator.onBallCreate += IncreeseCount;
		Creator.onBallDelete += DecreeseCount;

		GameManager.onMainCollapse += () => Visible = false;
		GameManager.onNextLevel += () => Visible = true;
		
		GameManager.onOverflow += () =>
		{ 
			ResetCounter();
			Visible = false;
		};
		GameManager.onRestart += () => Visible = true;
	}

	private void OnDisable() {
		Creator.onBallCreate -= IncreeseCount;
		Creator.onBallDelete -= DecreeseCount;

		GameManager.onMainCollapse -= () =>
		{ 
			ResetCounter();
			Visible = false;
		};

		GameManager.onNextLevel -= () => Visible = true;
		
		GameManager.onOverflow -= () =>
		{ 
			ResetCounter();
			Visible = false;
		};

		GameManager.onRestart -= () => Visible = true;
	}

	public void IncreeseCount(GameObject ball)
	{
		count++;
		Paint();
		Check();
	}

	public void DecreeseCount(GameObject ball)
	{
		count--;
		Paint();
		Check();
	}
    
    public void Check()
	{	
		if(count == max) {
            isBlinked = true;
            StartCoroutine(Blink());
            isOver = false;
        }
        else {
            isBlinked = false;
            StopCoroutine(Blink());
        }
        if(count == max + 1) {
            StartCoroutine(Over());
        }
        else if(count > max + 1) {
            isOver = false;
            StopCoroutine(Over());
            GameManager.Instance.OnOverflow();
        }
    }

    private void Paint()
    {
        float koef = count / max;
        Color color = Color.Lerp(Color.green, Color.red, koef);
		textMeshPro.color = color;
		textMeshPro.text = $"{count}/{max}";
    }

    private IEnumerator Blink()
    {
        Color color = textMeshPro.color;
        bool flag = false;
        float koef = 0;
        while(isBlinked) {
            yield return null;
            if(!flag) {
                if(koef < 0.5f) {
                    koef += Time.deltaTime;
                }
                else {
                    flag = true;
                }
            }
            else {
                if(koef > 0) {
                    koef -= Time.deltaTime;
                }
                else {
                    flag = false;
                }
            }
            color.a = koef;
            textMeshPro.color = color;
        }
        Paint();
    }

    private IEnumerator Over()
    {
        isOver = true;
        yield return new WaitForSeconds(1.5f);
        if(count == max + 1 && isOver) {
            GameManager.Instance.OnOverflow();
        }
    }

	private void ResetCounter() 
	{
		count = 0;
		Paint();
	} 
}
