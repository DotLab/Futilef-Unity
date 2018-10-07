using UnityEngine;
using Futilef;

public unsafe class Futile : MonoBehaviour {
	public Texture2D texture;
	public TpAtlasMeta* atlas;

	void OnEnable() {
		atlas = TpAtlasMeta.Create(Resources.Load<TextAsset>("10").text);
		Debug.LogFormat("{0} {1}", atlas->sprites[0].pivot[0], atlas->sprites[0].pivot[1]);
		var textureBytes = Resources.Load<TextAsset>("10i").bytes;
		Debug.Log(textureBytes.Length);
		texture = new Texture2D(0, 0, TextureFormat.RGBA4444, false);
		texture.LoadImage(textureBytes);
	}

	void OnDisable() {
		Debug.Log("Clean up");
		TpAtlasMeta.Free(atlas);
		Destroy(texture);
	}
}
