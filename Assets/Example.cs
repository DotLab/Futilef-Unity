using UnityEngine;
using Futilef;

using ImgAttr = Futilef.GpController.ImgAttr;

public unsafe class Example : MonoBehaviour {
	GpController gpc;

	Pool *nodePool;
	PtrLst *nodeLst;

	void OnEnable() {
		Res.LoadAtlases(10);
		
		#if GPC_TEST
		nodePool = Pool.New();

		nodeLst = PtrLst.New();
		PtrLst.Push(&nodePool->dependentLst, nodeLst);

		var sprite = (TpSprite *)Pool.Alloc(nodePool, sizeof(TpSprite));
		TpSprite.Init(sprite, Res.GetSpriteMeta(10001));

		var container = (Group *)Pool.Alloc(nodePool, sizeof(Group));
		Group.Init(container);
		PtrLst.Push(&nodePool->dependentLst, &container->childLst);

		PtrLst.Push(nodeLst, sprite);
		Vec4.Set(sprite->color, 1, 1, 1, 1);
		Vec2.Set(sprite->scl, .015f, .015f);

		DrawCtx.Start();
		var arr = (TpSprite **)nodeLst->arr;
		for (int i = 0, len = nodeLst->count; i < len; i += 1) {
			Node.Draw(arr[i], null, false);
		}
		DrawCtx.Finish();

		return;
		#endif

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
			gpc.RmImg(2);
		}

		Application.targetFrameRate = Screen.currentResolution.refreshRate;

		#if FDB
		Fdb.Test();
		#endif
	}

	void Update() {
		if (gpc != null) gpc.Update(Time.deltaTime);
	}
	 
	void OnDisable() {
		DrawCtx.Dispose();
		if (gpc != null) gpc.Dispose();
	}
}
