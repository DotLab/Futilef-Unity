using UnityEngine;
using Futilef;

public unsafe class DictBenchmark : Benchmark {
	protected override string GetTestName() {
		return "Dict";
	}

	protected override void RunTests() {
		var refDict = new System.Collections.Generic.Dictionary<uint, uint>();
		var dict = stackalloc Dict[1]; Dict.Init(dict);

		StartCase();
		for (uint i = 2; i < 1000000; i += 1) {
			refDict.Add(i, i);
		}
		RefCase();

		StartCase();
		for (uint i = 2; i < 1000000; i += 1) {
			Dict.Set(dict, i, (void *)i, (void *)i, Eq);
		}
		LogCase("set");

		StartCase();
		for (uint i = 2; i < 1000000; i += 1) {
			refDict.Remove(i);
		}
		RefCase();
		StartCase();
		for (uint i = 2; i < 1000000; i += 1) {
			Dict.Remove(dict, i, (void *)i, Eq);
		}
		LogCase("remove");

		StartCase();
		for (uint i = 2; i < 1000000; i += 1) {
			refDict.Add(i, i);
		}
		RefCase();
		StartCase();
		for (uint i = 2; i < 1000000; i += 1) {
			Dict.Set(dict, i, (void *)i, (void *)i, Eq);
		}
		LogCase("re set");

		uint k = 0;
		StartCase();
		for (uint i = 2; i < 1000000; i += 1) {
			k += refDict[i];
		}
		RefCase();
		k = 0;
		StartCase();
		for (uint i = 2; i < 1000000; i += 1) {
			k += (uint)Dict.Get(dict, i, (void *)i, Eq);
		}
		LogCase("get");
	}

	static unsafe bool Eq(void *a, void *b) {
		return (uint)a == (uint)b;
	}
}