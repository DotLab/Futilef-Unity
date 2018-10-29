using UnityEngine;
using Futilef;

using ImgAttr = Futilef.GpController.ImgAttr;

public class Example : MonoBehaviour {
	const int frameCounterSize = 500;
	int frameCounter;
	float lastCounterTime;

	GpController gpc;

	void OnEnable() {
		Res.LoadAtlases(10);

		gpc = new GpController();

		for (int i = 0; i < 5; i += 1) {
			gpc.AddImg(1, 10001);
			gpc.SetImgAttr(1, ImgAttr.Position, 0f, 0f, 0f);
			gpc.SetImgAttr(1, ImgAttr.Rotation, 0f);
			gpc.SetImgAttr(1, ImgAttr.Alpha, 1f);
			gpc.SetImgAttrEased(1, ImgAttr.Scale, 1f, EsType.ElasticOut, 0.01f, 0.01f);
			gpc.Wait(.5f);
			gpc.SetImgAttrEased(1, ImgAttr.Tint, 1.5f, EsType.ElasticOut, 1f, 0.5f, 1f);
			gpc.SetImgAttrEased(1, ImgAttr.Position, 2f, EsType.ElasticOut, 2f, -1f, 0f);
			gpc.Wait();
			gpc.SetImgAttrEased(1, ImgAttr.Tint, 1.5f, EsType.ElasticOut, 1f, 1f, 1f);
			gpc.SetImgAttrEased(1, ImgAttr.Position, 2f, EsType.ElasticOut, -2f, 2f, 0f);
			gpc.SetImgAttrEased(1, ImgAttr.Rotation, 1.5f, EsType.ElasticOut, Mathf.PI * 2.5f);

			gpc.Wait(.5f);
			gpc.AddImg(2, 10001);
			gpc.SetImgAttr(2, ImgAttr.Position, 0f, 0f, -5f);
			gpc.SetImgAttr(2, ImgAttr.Rotation, 0f);
			gpc.SetImgAttr(2, ImgAttr.Scale, 0.1f, 0.1f);
			gpc.SetImgAttr(2, ImgAttr.Alpha, 1f);
			gpc.SetImgAttrEased(2, ImgAttr.Scale, 1f, EsType.ElasticOut, 0.006f, 0.006f);
			gpc.SetImgAttrEased(2, ImgAttr.Position, 4f, EsType.ElasticOut, -2f, 2f, 0f);

			gpc.Wait();
			gpc.SetImgAttrEased(1, ImgAttr.Tint, 1f, EsType.ElasticOut, 1.5f, 1.5f, 1.5f);

			gpc.Wait();
			gpc.RmImg(1);
//			gpc.Wait(.5f);
			gpc.RmImg(2);
//			gpc.Wait(.5f);
		}

		#if UNITY_EDITOR
		Application.targetFrameRate = 60;
		#endif

		#if FDB
		Fdb.Test();
		#endif
	}

	void Update() {
		if (gpc != null) gpc.Update(Time.deltaTime);
	}
	 
	void OnDisable() {
		Debug.Log("Clean up ");
		gpc.Dispose();
	}
}
