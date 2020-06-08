using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Score : MonoBehaviour
{
    [SerializeField] private Text curText;
    [SerializeField] private Text bestText;
    [SerializeField] private GameObject addingTextPrefab;
    [SerializeField] private Transform canvas;
    private int curScore = 0;
    private int maxScore = 0;
    private static char[] liters;
    private WaitForSeconds mSec;
    private WaitForSeconds second;
    private int adding;
    private bool isAdding;

    void Awake()
    {
        mSec = new WaitForSeconds(0.05f);
        second = new WaitForSeconds(1f);
    }

	private void OnEnable() {
		CollapseProvider.onCollapse += IncreeseScore;

		GameManager.onRestart += ResetScore;
	}

	private void OnDisable() {
		CollapseProvider.onCollapse -= IncreeseScore;

		GameManager.onRestart -= ResetScore;
	}

    void Start()
    {
        RefreshCurrent(0);
    }

    public void IncreeseScore(int x, GameObject ball)
    {
        curScore += x;
        
        if(curScore > maxScore) {
            maxScore = curScore;
        }
        StartCoroutine(RefreshCurrent(x));
        StartCoroutine(ShowAdding(x, ball));
    }

    public void ResetScore()
    {
        curScore = 0;
        StartCoroutine(RefreshCurrent(0));
    }

    private IEnumerator RefreshCurrent(int x)
    {
        adding += x;
        if(!isAdding) {
            isAdding = true;
            string curBase = curText.text;
            string bestBase = bestText.text;
            for(int i = 1; i <= adding; i++) {
                curText.text = curBase + "+" + i;
                if(maxScore == curScore) {
                    bestText.text = bestBase + "+" + i;
                }
                yield return mSec;
            }
            yield return second;
            adding = 0;
            isAdding = false;
        

            string curStr = curScore > 999999 ? (curScore / 1000000).ToString("N0") + liters[1] : curScore.ToString("N0");

            curText.text = curStr;
            if(maxScore == curScore) {
                string maxStr = maxScore > 999999 ? (maxScore / 1000000).ToString("N0") + liters[1] : maxScore.ToString("N0");
                bestText.text = maxStr;
            }
        }
    }

    private IEnumerator ShowAdding(int x, GameObject ball)
    {
        GameObject aObj = Lean.Pool.LeanPool.Spawn(addingTextPrefab);

		aObj.transform.SetParent(canvas);
		aObj.transform.position = ball.transform.position;

        TextMeshPro aText = aObj.GetComponent<TextMeshPro>();
        aText.text = "+" + x.ToString();

        aObj.transform.localScale = ball.transform.localScale;

        Color color = aText.color;
        float t = 0.3f;
        
		while (t < 1) {
			color.a = t;
			aText.color = color;
            yield return null;
            t += Time.deltaTime;
        }

        t = 1;

        while (t > 0) {
            color.a = t;
			aText.color = color;
            yield return null;
            t -= Time.deltaTime;
        }

        Lean.Pool.LeanPool.Despawn(aObj);
    }
}
