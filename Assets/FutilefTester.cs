using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Futilef;
using Futilef.Core;

public class FutilefTester : MonoBehaviour {
	void Start() {
		FutilefBehaviour.Instance.Init();
	}

	Container c;

	[ContextMenu("Add")]
	void Add() {
		FutilefBehaviour.Stage.AddChild(c = new Container());
	}

	[ContextMenu("Remove")]
	void Remove() {
		FutilefBehaviour.Stage.RemoveChild(c);
		c = null;
	}
}
