using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Futilef.V2;
using Futilef.V2.Node;

public class V2Tester : MonoBehaviour {
	Node node = new Node();
	Container container = new Container();
	float[] v = Vec3.Create();

	void Start() {
		container.AddChild(node);
	}

	void Update() {
		container.x = transform.localPosition.x;
		container.y = transform.localPosition.y;
		container.z = transform.localPosition.z;
		container.rotX = transform.localRotation.eulerAngles.x;
		container.rotY = transform.localRotation.eulerAngles.y;
		container.rotZ = transform.localRotation.eulerAngles.z;
		container.sclX = transform.localScale.x;
		container.sclY = transform.localScale.y;
		container.sclZ = transform.localScale.z;

		node.x = 2;
		node.y = 3;
		node.z = 4;

		container.Redraw();

		Vec3.Zero(v);
		print("LocalToScreen: " + Vec3.Str(node.LocalToScreen(v)));

		Vec3.Zero(v);
		print("ScreenToLocal: " + Vec3.Str(node.ScreenToLocal(v)));
	}
}
