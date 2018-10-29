using UnityEngine;
using Futilef;

public unsafe class PtrLst2Benchmark : Benchmark {
	protected override string GetTestName() {
		return "PtrLst2";
	}

	protected override void RunTests() {
		var refList = new System.Collections.Generic.List<long>();
		var lst = stackalloc PtrLst2[1]; PtrLst2.Init(lst);

		StartCase();
		for (long i = 0; i < 1000000; i += 1) {
			refList.Add(i);
		}
		RefCase();
		StartCase();
		for (long i = 0; i < 1000000; i += 1) {
			PtrLst2.Push(lst, (void *)i);
		}
		LogCase("push");

		var idx = new int[1000000];
		for (int i = 0; i < 1000000; i += 1) {
			idx[i] = Random.Range(0, 1000000);
		}

		long t = 0;
		StartCase();
		for (long i = 0; i < 1000000; i += 1) {
			t = refList[idx[i]];
		}
		RefCase();
		StartCase();
		for (long i = 0; i < 1000000; i += 1) {
			t = (long)lst->arr[idx[i]];
		}
		LogCase("rand get");

		StartCase();
		for (long i = 0; i < 100; i += 1) {
			if (idx[i] < (1000000 - i)) {
				refList.RemoveAt(idx[i]);
			}
		}
		RefCase();
		StartCase();
		for (long i = 0; i < 100; i += 1) {
			if (idx[i] < (1000000 - i)) {
				PtrLst2.RemoveAt(lst, idx[i]);
			}
		}
		LogCase("rand remove");

		StartCase();
		for (long i = 0; i < 50000; i += 1) {
			refList.Add(t + i);
		}
		RefCase();
		StartCase();
		for (long i = 0; i < 50000; i += 1) {
			PtrLst2.Push(lst, (void *)(t + i));
		}
		LogCase("re push");

		StartCase();
		refList.Sort(0, 10000, new Cmpr());
		RefCase();
		StartCase();
		Algo.MergeSort(lst->arr, 10000, Cmp);
		LogCase("sort");
	}

	sealed class Cmpr : System.Collections.Generic.IComparer<long> {
		public int Compare(long x, long y) {
			return (int)x - (int)y;
		}
	}

	static int Cmp(void *a, void *b){
		return (int)a - (int)b;
	}
}