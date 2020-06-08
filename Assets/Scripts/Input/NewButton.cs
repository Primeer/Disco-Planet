using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class NewButton : MonoBehaviour
{
	[SerializeField] private Vector2Event onTap;
	[SerializeField] private Vector2Event onDrag;
	[SerializeField] private Vector2Event onRelease;

	private void Start() {
		BoxCollider2D collider = GetComponent<BoxCollider2D>();

		if(!collider)
		{
			collider = gameObject.AddComponent<BoxCollider2D>();
			collider.isTrigger = true;
		}

		RectTransform rectTransform = GetComponent<RectTransform>();
		
		if (rectTransform)
		{
			Vector2 newSize = new Vector2(rectTransform.rect.width, rectTransform.rect.height);
			collider.size = newSize;
		}
	}
	
	private void OnMouseDown() {
		onTap?.Invoke(Input.mousePosition);
	}

	private void OnMouseDrag() {
		onDrag?.Invoke(Input.mousePosition);
	}

	private void OnMouseUp(){
		onRelease?.Invoke(Input.mousePosition);
	}
}
