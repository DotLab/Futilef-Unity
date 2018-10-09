using UnityEngine;
using Futilef;

public unsafe class Futile : MonoBehaviour {
	DrawContext ctx;
	ImgObj *img;

	void OnEnable() {
		Res.LoadAtlases(10);
		img = ImgObj.New(Res.GetSpriteMeta(10001));
		ctx = new DrawContext();
	}

	void Update() {
		var vec2 = stackalloc float[2];
		ImgObj.SetPosition(img, Vec2.Set(vec2, Mathf.Sin(Time.time) * 100, 0));
		ImgObj.SetRotation(img, Mathf.Cos(Time.time));

		ctx.Start();
		ImgObj.Draw(img, ctx);
		ctx.Finish();		
	}
	 
	void OnDisable() {
		Mem.Free(img);
		ctx.Dispose();

		Debug.Log("Clean up ");
	}
}
