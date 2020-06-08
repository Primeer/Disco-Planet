using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimLine : MonoBehaviour
{
    public float width = 0.02f;
    public float height = 7f;
    private SpriteRenderer render;
    private RaycastHit2D hit;
    private float len;

	public bool Visible
	{
		get { return render.enabled; }
		set { render.enabled = value; }
	}

    void Awake()
    {
        render = GetComponent<SpriteRenderer>();
    }

    private void Update() {
        if(Visible) 
            SetLength();
    }

    private void SetLength()
    {
        hit = Physics2D.Raycast(transform.position, Vector2.up, 8f, LayerMask.GetMask("Default"));
        len = height;
        
		if (hit.collider != null && hit.distance > 0)
            len = hit.distance;
       
        render.drawMode = SpriteDrawMode.Sliced;
        render.size = new Vector2(width, len);
        render.drawMode = SpriteDrawMode.Tiled;
    }
}
