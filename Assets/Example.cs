using UnityEngine;
using Futilef;

using ImgAttr = Futilef.GpController.ImgAttr;

public unsafe class Example : MonoBehaviour {
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
			gpc.SetImgAttr(1, ImgAttr.Alpha, 1f);
			gpc.SetImgAttrEased(1, ImgAttr.Scale, 1f, EsType.ElasticOut, 0.01f, 0.01f);
			gpc.Wait(.5f);
			gpc.SetImgAttrEased(1, ImgAttr.Tint, 1.5f, EsType.ElasticOut, 1f, 0.5f, 1f);
			gpc.SetImgAttrEased(1, ImgAttr.Position, 2f, EsType.ElasticOut, 2f, -1f, 0f);
			gpc.Wait();
			gpc.SetImgAttrEased(1, ImgAttr.Tint, 1.5f, EsType.ElasticOut, 1f, 1f, 1f);
			gpc.SetImgAttrEased(1, ImgAttr.Position, 2f, EsType.ElasticOut, -2f, 2f, 0f);
			gpc.SetImgAttrEased(1, ImgAttr.Rotation, 1.5f, EsType.ElasticOut, -10f);

			gpc.Wait(.5f);
			gpc.AddImg(2, 10001);
			gpc.SetImgAttr(2, ImgAttr.Position, 0f, 0f, -5f);
			gpc.SetImgAttr(2, ImgAttr.Alpha, 1f);
			gpc.SetImgAttrEased(2, ImgAttr.Scale, 1f, EsType.ElasticOut, 0.006f, 0.006f);
			gpc.SetImgAttrEased(2, ImgAttr.Position, 4f, EsType.ElasticOut, -2f, 2f, 0f);

			gpc.Wait();
			gpc.SetImgAttrEased(1, ImgAttr.Tint, 1f, EsType.ElasticOut, 1.5f, 1.5f, 1.5f);

			gpc.Wait();
			gpc.RmImg(1);
			gpc.Wait(.5f);
			gpc.RmImg(2);
			gpc.Wait(.5f);
		}

//		Application.targetFrameRate = 60;
		lastCounterTime = Time.time;
		frameCounter = 0;

		var sw = new System.Diagnostics.Stopwatch();
		var refDict = new System.Collections.Generic.Dictionary<uint, uint>();
		sw.Stop();
		sw.Reset();
		sw.Start();
		for (uint i = 2; i < 1000000; i += 1) {
			refDict.Add(i, i);
		}
		sw.Stop();
		UnityEngine.Debug.LogFormat("refDict Test: {0:N0}", sw.ElapsedTicks);

		var dict = stackalloc Dict[1]; Dict.Init(dict);
		sw.Reset();
		sw.Start();
		for (uint i = 2; i < 1000000; i += 1) {
			Dict.Set(dict, i, (void *)i, (void *)i, eq);
		}
		sw.Stop();
		UnityEngine.Debug.LogFormat("dict Test: {0:N0}", sw.ElapsedTicks);

		sw.Reset();
		sw.Start();
		for (uint i = 2; i < 1000000; i += 1) {
			refDict.Remove(i);
		}
		sw.Stop();
		UnityEngine.Debug.LogFormat("refDict Test: {0:N0}", sw.ElapsedTicks);

		sw.Reset();
		sw.Start();
		for (uint i = 2; i < 1000000; i += 1) {
			Dict.Remove(dict, i, (void *)i, eq);
		}
		sw.Stop();
		UnityEngine.Debug.LogFormat("dict Test: {0:N0}", sw.ElapsedTicks);

		sw.Reset();
		sw.Start();
		for (uint i = 2; i < 1000000; i += 1) {
			refDict.Add(i, i);
		}
		sw.Stop();
		UnityEngine.Debug.LogFormat("refDict Test: {0:N0}", sw.ElapsedTicks);

		sw.Reset();
		sw.Start();
		for (uint i = 2; i < 1000000; i += 1) {
			Dict.Set(dict, i, (void *)i, (void *)i, eq);
		}
		sw.Stop();
		UnityEngine.Debug.LogFormat("dict Test: {0:N0}", sw.ElapsedTicks);

		uint k = 0;
		sw.Reset();
		sw.Start();
		for (uint i = 2; i < 1000000; i += 1) {
			k += refDict[i];
		}
		sw.Stop();
		UnityEngine.Debug.LogFormat("refDict Test: {0:N0}", sw.ElapsedTicks);

		k = 0;
		sw.Reset();
		sw.Start();
		for (uint i = 2; i < 1000000; i += 1) {
			k += (uint)Dict.Get(dict, i, (void *)i, eq);
		}
		sw.Stop();
		UnityEngine.Debug.LogFormat("dict Test: {0:N0}", sw.ElapsedTicks);
	}
	static unsafe bool eq(void *a, void *b) {
		return (uint)a == (uint)b;
	}

	void Update() {
		frameCounter += 1;
		if (frameCounter >= frameCounterSize) {
//			Fdb.Log("{0:F2} fps", frameCounterSize / (Time.time - lastCounterTime));
			lastCounterTime = Time.time;
			frameCounter = 0;
		}

		gpc.Update(Time.deltaTime);
	}
	 
	void OnDisable() {
		Debug.Log("Clean up ");
		gpc.Dispose();
	}
}
