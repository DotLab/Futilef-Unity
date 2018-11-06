using UnityEngine;
using Futilef;

public unsafe class NumDictBenchmark : Benchmark {
	protected override string GetTestName() {
		return "NumDict";
	}

	protected override void RunTests() {
		var refDict = new System.Collections.Generic.Dictionary<int, int>();
		var dict = stackalloc PtrIntDict[1]; PtrIntDict.Init(dict);

		StartCase();
		for (int i = 2; i < 1000000; i += 1) {
			refDict.Add(i, i);
		}
		RefCase();

		StartCase();
		for (int i = 2; i < 1000000; i += 1) {
			PtrIntDict.Set(dict, i, (void *)i);
		}
		LogCase("set");

		StartCase();
		for (int i = 2; i < 1000000; i += 1) {
			refDict.Remove(i);
		}
		RefCase();
		StartCase();
		for (int i = 2; i < 1000000; i += 1) {
			PtrIntDict.Remove(dict, i);
		}
		LogCase("remove");

		StartCase();
		for (int i = 2; i < 1000000; i += 1) {
			refDict.Add(i, i);
		}
		RefCase();
		StartCase();
		for (int i = 2; i < 1000000; i += 1) {
			PtrIntDict.Set(dict, i, (void *)i);
		}
		LogCase("re set");

		int k = 0;
		StartCase();
		for (int i = 2; i < 1000000; i += 1) {
			k += refDict[i];
		}
		RefCase();
		k = 0;
		StartCase();
		for (int i = 2; i < 1000000; i += 1) {
			k += (int)PtrIntDict.Get(dict, i);
		}
		LogCase("get");
	}
}