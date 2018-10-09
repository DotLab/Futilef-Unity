using UnityEngine;
using Futilef;

public unsafe class Futile : MonoBehaviour {
	public TpAtlasMeta* atlas;

	void OnEnable() {
		atlas = TpAtlasMeta.Create(Resources.Load<TextAsset>("10").text);
		Debug.LogFormat("{0} {1}", atlas->sprites[0].pivot[0], atlas->sprites[0].pivot[1]);

		var img = stackalloc ImgObj[1];
		ImgObj.Init(img, atlas->sprites + 5);
		var ctx = new DrawContext();
		ctx.Start();
		ImgObj.Draw(img, ctx);
		ctx.Finish();
	}
	 
	void OnDisable() {
		Debug.Log("Clean up");
		TpAtlasMeta.Free(atlas);
	}
}
