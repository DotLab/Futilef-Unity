using UnityEngine;
using Futilef;

public unsafe class PtrLstBenchmark : Benchmark {
	protected override string GetTestName() {
		return "PtrLst";
	}

	protected override void RunTests() {
//		var refList = new System.Collections.Generic.List<uint>();
//		var lst = stackalloc PtrLst[1]; PtrLst.Init(lst);

//		StartCase();
//		for (uint i = 0; i < 1000000; i += 1) {
//			refList.Add(i);
//		}
//		RefCase();
//		StartCase();
//		for (uint i = 0; i < 1000000; i += 1) {
//			PtrLst.Push(lst, (void *)i);
//		}
//		LogCase("push");
//
//		var idx = new int[1000000];
//		for (int i = 0; i < 1000000; i += 1) {
//			idx[i] = Random.Range(0, 1000000);
//		}
//
//		uint t = 0;
//		StartCase();
//		for (uint i = 0; i < 1000000; i += 1) {
//			t = refList[idx[i]];
//		}
//		RefCase();
//		StartCase();
//		for (uint i = 0; i < 1000000; i += 1) {
//			t = (uint)lst->arr[idx[i]];
//		}
//		LogCase("rand get");
//
//		StartCase();
//		for (uint i = 0; i < 100; i += 1) {
//			if (idx[i] < (1000000 - i)) {
//				refList.RemoveAt(idx[i]);
//			}
//		}
//		RefCase();
//		StartCase();
//		for (uint i = 0; i < 100; i += 1) {
//			if (idx[i] < (1000000 - i)) {
//				PtrLst.Remove(lst, );
//			}
//		}
//		LogCase("rand remove");
//
//		StartCase();
//		for (uint i = 0; i < 50000; i += 1) {
//			item->i = t + i;
//			refList.Add(*item);
//		}
//		RefCase();
//		StartCase();
//		for (uint i = 0; i < 50000; i += 1) {
//			item->i = t + i;
//			*(Item *)Lst2.Push(lst) = *item;
//		}
//		LogCase("re push");
	}
}