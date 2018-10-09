using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Futilef;

public static unsafe class Res {
	static readonly Dictionary<int, Texture2D> textureDict = new Dictionary<int, Texture2D>();

	static readonly Lstp *atlasMetaLst = Lstp.New();
	static readonly Lstp *spriteMetaLst = Lstp.New();
	static readonly Dictionary<int, int> spriteMetaLstIdxDict = new Dictionary<int, int>();

	public static Texture2D GetTexture(int id) {
		if (textureDict.ContainsKey(id)) return textureDict[id];

		var texture = new Texture2D(0, 0);
		Debug.Log("Load " + id + "i");
		texture.LoadImage(Resources.Load<TextAsset>(id + "i").bytes);
		textureDict.Add(id, texture);
		return texture;
	}

	public static void LoadAtlases(params int[] ids) {
		for (int i = 0, len = ids.Length; i < len; i += 1) {
			int id = ids[i];
			var atlasMeta = TpAtlasMeta.New(Resources.Load<TextAsset>(id.ToString()).text);
			Lstp.Push(atlasMetaLst, atlasMeta);
			for (int j = 0, jlen = atlasMeta->spriteCount; j < jlen; j += 1) {
				var spriteMeta = atlasMeta->sprites + j;
				spriteMetaLstIdxDict.Add(spriteMeta->name, spriteMetaLst->count);
				Lstp.Push(spriteMetaLst, spriteMeta);
			}
		}	
	}

	public static TpSpriteMeta *GetSpriteMeta(int id) {
		return (TpSpriteMeta *)spriteMetaLst->ptr[spriteMetaLstIdxDict[id]];
	}

	public static bool HasSpriteMeta(int id) {
		return spriteMetaLstIdxDict.ContainsKey(id);
	}
}

public class DrawBatch {
	const int VertExpandAmount = 60, TriExpandAmount = 30;
	const int VertUnusedLimit = 600, TriUnusedLimit = 300;

	public Shader shader;
	public Texture2D texture;

	public GameObject gameObject;

	public MeshFilter meshFilter;
	public Mesh mesh;

	public MeshRenderer meshRenderer;
	public Material material;

	public int vertLen = VertExpandAmount, vertCount;
	public Vector3[] verts = new Vector3[VertExpandAmount];
	public Color[] colors = new Color[VertExpandAmount];
	public Vector2[] uvs = new Vector2[VertExpandAmount];

	public int triLen = TriExpandAmount, triCount;
	public int[] tris = new int[TriExpandAmount];

	public DrawBatch(Shader shader, Texture2D texture) {
		this.shader = shader; this.texture = texture;

		gameObject = new GameObject(string.Format("Batch[{0}|{1}x{2}]", shader.name, texture.width, texture.height));

		meshFilter = gameObject.AddComponent<MeshFilter>();
		mesh = meshFilter.mesh;
		mesh.MarkDynamic();

		meshRenderer = gameObject.AddComponent<MeshRenderer>();
		material = new Material(shader);
		material.mainTexture = texture;
		meshRenderer.sharedMaterial = material;
	}

	public void Open(int queue) {
		verts[0].Set(50, 0, 1000000);  // special vertex to fill unused triangle
		vertCount = 1; triCount = 0;
		if (queue != material.renderQueue) material.renderQueue = queue;
	}

	public void RequestQuota(int v, int t) {
		if ((vertCount += v) >= vertLen) {
			vertLen = vertCount + VertExpandAmount;
			System.Array.Resize(ref verts, vertLen);
			System.Array.Resize(ref colors, vertLen);
			System.Array.Resize(ref uvs, vertLen);
		}

		if ((triCount += t) >= triLen) {
			triLen = triCount + TriExpandAmount;
			System.Array.Resize(ref tris, triLen);
		}
	}

	public void Close() {
		if (vertCount < vertLen - VertUnusedLimit) {
			vertLen = vertCount + VertExpandAmount;
			System.Array.Resize(ref verts, vertLen);
			System.Array.Resize(ref colors, vertLen);
			System.Array.Resize(ref uvs, vertLen);
		}

		if (triCount < triLen - TriUnusedLimit) {
			triLen = triCount + TriExpandAmount;
			System.Array.Resize(ref tris, triLen);
		}

		if (triCount < triLen - 1) {  // fill unused triangles
			System.Array.Clear(tris, triCount, triLen - triCount - 1);
		}

		mesh.vertices = verts;
		mesh.colors = colors;
		mesh.uv = uvs;

		mesh.triangles = tris;

		mesh.RecalculateBounds();
	}

	public void Activate() {
		gameObject.SetActive(true);
	}

	public void Deactivate() {
		gameObject.SetActive(false);
	}

	public void Dispose() {
		Debug.Log("Dispose " + gameObject.name);
		Object.Destroy(material);
		Object.Destroy(mesh);
		Object.Destroy(gameObject);
	}
}

public class DrawContext {
	const int BaseQueue = 3001;
	static readonly Shader DefaultShader = Shader.Find("Futilef/Basic");

	public LinkedList<DrawBatch> prevBatches = new LinkedList<DrawBatch>();
	public LinkedList<DrawBatch> activeBatches = new LinkedList<DrawBatch>();
	public readonly LinkedList<DrawBatch> inactiveBatches = new LinkedList<DrawBatch>();

	int curQueue;
	DrawBatch curBatch;

	public void Start() {
		curQueue = BaseQueue;
		curBatch = null;
	}

	public void Finish() {
		for (var node = prevBatches.First; prevBatches.Count > 0; node = prevBatches.First) {
			node.Value.Deactivate();
			inactiveBatches.AddLast(node.Value);
			prevBatches.RemoveFirst();
		}

		// Same as prevBatches.AddRange(activeBatches); activeBatches.Clear();
		var swap = prevBatches;
		prevBatches = activeBatches;
		activeBatches = swap;

		if (curBatch != null) {
			curBatch.Close();
			curBatch = null;
		}
	}

	public DrawBatch GetBatch(int textureId) {
		return GetBatch(DefaultShader, Res.GetTexture(textureId));
	}

	public DrawBatch GetBatch(Shader shader, Texture2D texture) {
		if (curBatch != null) {
			if (curBatch.shader == shader && curBatch.texture == texture) return curBatch;
			curBatch.Close();
		}

		curQueue += 1;

		// if there is a prevBatch that matches
		for (var node = prevBatches.First; node != null; node = node.Next) {
			curBatch = node.Value;
			if (curBatch.shader == shader && curBatch.texture == texture) {
				prevBatches.Remove(node);
				curBatch.Open(curQueue);
				activeBatches.AddLast(curBatch);
				return curBatch;
			}
		}

		// if there is an inactiveBatch that matches
		for (var node = inactiveBatches.First; node != null; node = node.Next) {
			curBatch = node.Value;
			if (curBatch.shader == shader && curBatch.texture == texture) {
				inactiveBatches.Remove(node);
				curBatch.Activate();
				curBatch.Open(curQueue);
				activeBatches.AddLast(curBatch);
				return curBatch;
			}
		}

		// create a new batch
		curBatch = new DrawBatch(shader, texture);
		curBatch.Open(curQueue);
		activeBatches.AddLast(curBatch);
		return curBatch;
	}

	public void Dispose() {
		foreach (var batch in activeBatches) batch.Dispose();
		foreach (var batch in prevBatches) batch.Dispose();
		foreach (var batch in inactiveBatches) batch.Dispose();
	}
}

public unsafe class GamePresentationController : MonoBehaviour {
	class Cmd {  }

	class WaitCmd : Cmd { public int time; }

	class ImgCmd : Cmd { public int id; }
	class AddImgCmd : ImgCmd { public int imgId; }
	class RmImgCmd : ImgCmd { }
	class SetImgAttrCmd : ImgCmd { public int imgAttrId; public object[] args; }
	class SetImgAttrEasedCmd : SetImgAttrCmd { public float duration; public int esType; }

	class SetCamAttrCmd : Cmd { public int camAttrId; public object[] args; }
	class SetCamAttrEasedCmd : SetCamAttrCmd { public float duration; public int esType; }

	readonly Queue<Cmd> cmdQueue = new Queue<Cmd>();

	float waitEndTime = -1, lastEsEndTime;

	void OnEnable() {
	
	}

	void OnDisable() {

	}

	void Update() {
		if (Time.time <= waitEndTime) return;

		while (Time.time > waitEndTime && cmdQueue.Count > 0) {
			var cmd = cmdQueue.Dequeue();
			float endTime;

			switch (cmd.GetType().Name) {
			case "WaitCmd":
				var waitCmd = cmd as WaitCmd;
				if (waitCmd.time < 0) {  // wait for all animation to finish
					if (lastEsEndTime > waitEndTime) waitEndTime = lastEsEndTime;
				} else {
					endTime = Time.time + waitCmd.time;
					if (endTime > waitEndTime) waitEndTime = endTime;
				}
				break;
			case "SetImgAttrEasedCmd":
				var setImgAttrEasedCmd = cmd as SetImgAttrEasedCmd;
				endTime = Time.time + setImgAttrEasedCmd.duration;
				if (endTime > lastEsEndTime) lastEsEndTime = endTime;
				break;
			case "AddImgCmd":
				var addImgCmd = cmd as AddImgCmd;

				break;
			}
		}
	}

	public void Wait(int time = -1) {
		cmdQueue.Enqueue(new WaitCmd{ time = time });
	}

	public void AddImg(int id, int imgId) {
		if (Res.HasSpriteMeta(imgId)) {
			cmdQueue.Enqueue(new AddImgCmd{ id = id, imgId = imgId });
		}
	}

	public void RmImg(int id) {
		cmdQueue.Enqueue(new RmImgCmd{ id = id });
	}

	public void SetImgAttr(int id, int imgAttrId, params object[] args) {
		cmdQueue.Enqueue(new SetImgAttrCmd{ id = id, imgAttrId = imgAttrId, args = args });
	}

	public void SetImgAttrEased(int id, int imgAttrId, float duration, int esType, params object[] args) {
		cmdQueue.Enqueue(new SetImgAttrEasedCmd{ id = id, imgAttrId = imgAttrId, duration = duration, esType = esType, args = args });
	}

	public void SetCamAttr(int camAttrId, params object[] args) {
		cmdQueue.Enqueue(new SetCamAttrCmd{ camAttrId = camAttrId, args = args });
	}

	public void SetCamAttrEased(int camAttrId, float duration, int esType, params object[] args) {
		cmdQueue.Enqueue(new SetCamAttrEasedCmd{ camAttrId = camAttrId, duration = duration, esType = esType, args = args });
	}
}

public unsafe struct ImgObj {
	public int type;

	bool interactable;
	bool shouldRebuild;
	// bool resetTouch;

	fixed float pos[2];
	fixed float scl[2];
	float rot;

	fixed float color[4];

	fixed float verts[4 * 2];
	fixed float uvs[4 * 2];

	TpSpriteMeta *spriteMeta;

	public static ImgObj *New(TpSpriteMeta *spriteMeta) {
		return Init((ImgObj *)Mem.Alloc(sizeof(ImgObj)), spriteMeta);
	}

	public static ImgObj *Init(ImgObj *self, TpSpriteMeta *spriteMeta) {
		self->type = 1;
		self->interactable = false;
		self->shouldRebuild = true;

		Vec2.Zero(self->pos);
		Vec2.Set(self->scl, 1, 1);
		Vec4.Set(self->color, 1, 1, 1, 1);
		self->spriteMeta = spriteMeta;

		return self;
	}

	public static void SetInteractable(ImgObj *self, bool val) {
		self->interactable = val;
		// self->resetTouch = true;
	}

	public static void SetPosition(ImgObj *self, float *position) {
		Vec2.Copy(self->pos, position);
		self->shouldRebuild = true;
	}

	public static void SetRotation(ImgObj *self, float rotation) {
		self->rot = rotation;
		self->shouldRebuild = true;
	}

	public static void SetScale(ImgObj *self, float *scale) {
		Vec2.Copy(self->pos, scale);
		self->shouldRebuild = true;
	}

	public static void SetOpacity(ImgObj *self, float opacity) {
		self->color[3] = opacity;
	}

	public static void SetColor(ImgObj *self, float *color) {
		Vec3.Copy(self->color, color);
	}

	public static void Draw(ImgObj *self, DrawContext ctx) {
		var batch = ctx.GetBatch(self->spriteMeta->atlas->name);
		int vertIdx = batch.vertCount, triIdx = batch.triCount;
		batch.RequestQuota(4, 6);
		
		float *verts = self->verts;
		float *uvs = self->uvs;

		// v0 - v1
		// |  \ |
		// v3 - v2
		if (self->shouldRebuild) {
			float *mat = stackalloc float[6];
			Mat2D.FromScalingRotationTranslation(mat, self->pos, self->scl, self->rot);

			float *pivot = self->spriteMeta->pivot;
			float pivotX = pivot[0], pivotY = pivot[1];
			float *quad = self->spriteMeta->quad;
			float quadX = quad[0], quadY = quad[1], quadW = quad[2], quadH = quad[3];
			Vec2.TransformMat2D(verts,     mat, -pivotX + quadX,         pivotY - quadY);
			Vec2.TransformMat2D(verts + 2, mat, -pivotX + quadX + quadW, pivotY - quadY);
			Vec2.TransformMat2D(verts + 4, mat, -pivotX + quadX + quadW, pivotY - quadY - quadH);
			Vec2.TransformMat2D(verts + 6, mat, -pivotX + quadX,         pivotY - quadY - quadH);

			float *size = self->spriteMeta->atlas->size;
			float invSizeX = 1 / size[0], invSizeY = 1 / size[1];
			float *uv = self->spriteMeta->uv;
			float uvX = uv[0], uvY = uv[1], uvW = uv[2], uvH = uv[3];
			if (self->spriteMeta->rotated) {
				Vec2.Set(uvs    , (uvX + uvW) * invSizeX,  -uvY        * invSizeY);
				Vec2.Set(uvs + 2, (uvX + uvW) * invSizeX, (-uvY - uvH) * invSizeY);
				Vec2.Set(uvs + 4,  uvX        * invSizeX, (-uvY - uvH) * invSizeY);
				Vec2.Set(uvs + 6,  uvX        * invSizeX,  -uvY        * invSizeY);
			} else {
				Vec2.Set(uvs    ,  uvX        * invSizeX,  -uvY        * invSizeY);
				Vec2.Set(uvs + 2, (uvX + uvW) * invSizeX,  -uvY        * invSizeY);
				Vec2.Set(uvs + 4, (uvX + uvW) * invSizeX, (-uvY - uvH) * invSizeY);
				Vec2.Set(uvs + 6,  uvX        * invSizeX, (-uvY - uvH) * invSizeY);
			}
		}

		var bVerts = batch.verts; var bUvs = batch.uvs; 
		bVerts[vertIdx    ].Set(verts[0], verts[1], 0);
		bVerts[vertIdx + 1].Set(verts[2], verts[3], 0);
		bVerts[vertIdx + 2].Set(verts[4], verts[5], 0);
		bVerts[vertIdx + 3].Set(verts[6], verts[7], 0);

		bUvs[vertIdx    ].Set(uvs[0], uvs[1]);
		bUvs[vertIdx + 1].Set(uvs[2], uvs[3]);
		bUvs[vertIdx + 2].Set(uvs[4], uvs[5]);
		bUvs[vertIdx + 3].Set(uvs[6], uvs[7]);

		float *color = self->color;
		var bColor = new Color(color[0], color[1], color[2], color[3]);
		var bColors = batch.colors;
		bColors[vertIdx    ] = bColor;
		bColors[vertIdx + 1] = bColor;
		bColors[vertIdx + 2] = bColor;
		bColors[vertIdx + 3] = bColor;

		var bTris = batch.tris;
		bTris[triIdx]     = vertIdx;
		bTris[triIdx + 1] = vertIdx + 1;
		bTris[triIdx + 2] = vertIdx + 2;
		bTris[triIdx + 3] = vertIdx;
		bTris[triIdx + 4] = vertIdx + 2;
		bTris[triIdx + 5] = vertIdx + 3;
	}
}