using UnityEngine;
using Futilef;

using ImgAttr = Futilef.GpController.ImgAttr;

public class Example : MonoBehaviour {
	GpController gpc;

	void OnEnable() {
		Res.LoadAtlases(10);

		gpc = new GpController();
		gpc.Init();

		gpc.AddImg(1, 10001);
		gpc.SetImgAttr(1, ImgAttr.Alpha, 1f);
		gpc.SetImgAttrEased(1, ImgAttr.Scale, 5f, EsType.BackInOut, 0.01f, 0.01f);
		gpc.Wait(.5f);
		gpc.SetImgAttr(1, ImgAttr.Alpha, 0f);
		gpc.Wait(.5f);
		gpc.SetImgAttr(1, ImgAttr.Alpha, 1f);
		gpc.Wait(1f);
		gpc.SetImgAttrEased(1, ImgAttr.Alpha, 2f, EsType.BounceOut, 0f);
		gpc.Wait();
		gpc.SetImgAttrEased(1, ImgAttr.Alpha, 3f, EsType.BounceOut, 1f);
		gpc.SetImgAttrEased(1, ImgAttr.Tint, 3f, EsType.BounceOut, 1f, 0.5f, 1f);
		gpc.SetImgAttrEased(1, ImgAttr.Position, 4f, EsType.ElasticOut, 2f, -1f, 0f);
		gpc.Wait();
		gpc.SetImgAttrEased(1, ImgAttr.Tint, 3f, EsType.BounceOut, 1f, 1f, 1f);
		gpc.SetImgAttrEased(1, ImgAttr.Position, 4f, EsType.ElasticOut, -2f, 2f, 0f);
		gpc.SetImgAttrEased(1, ImgAttr.Rotation, 3f, EsType.ElasticOut, -10f);
		gpc.Wait();
		gpc.SetImgAttrEased(1, ImgAttr.Alpha, 4f, EsType.CubicOut, 0f);
		gpc.Wait();
		gpc.RmImg(1);

		Input.simulateMouseWithTouches = true;
		Debug.Log(Screen.currentResolution);
	}

	void Update() {
		Debug.LogFormat("{0} at {1} ({2})", "mouse", Input.mousePosition, Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0)));

		foreach (var t in Input.touches) {
			Debug.LogFormat("{0} at {1} ({2})", t.fingerId, t.position, t.rawPosition);
		}

		gpc.Update(Time.deltaTime);
	}
	 
	void OnDisable() {
		gpc.Dispose();
		Debug.Log("Clean up ");
	}
}
