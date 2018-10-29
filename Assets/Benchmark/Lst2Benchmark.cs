using UnityEngine;
using Futilef;

public unsafe class Lst2Benchmark : Benchmark {
	struct Item {
		public uint i, j, k;
	}

	protected override string GetTestName() {
		return "Lst2";
	}

	protected override void RunTests() {
		var refList = new System.Collections.Generic.List<Item>();
		var lst = stackalloc Lst2[1]; Lst2.Init(lst, sizeof(Item));

		var item = stackalloc Item[1];
		item->j = 10; item->k = 100;

		StartCase();
		for (uint i = 0; i < 1000000; i += 1) {
			item->i = i;
			refList.Add(*item);
		}
		RefCase();
		StartCase();
		for (uint i = 0; i < 1000000; i += 1) {
			item->i = i;
			*(Item *)Lst2.Push(lst) = *item;
		}
		LogCase("push");

		var idx = new int[1000000];
		for (int i = 0; i < 1000000; i += 1) {
			idx[i] = Random.Range(0, 1000000);
		}

		uint t = 0;
		StartCase();
		for (uint i = 0; i < 1000000; i += 1) {
			t = refList[idx[i]].i;
		}
		RefCase();
		StartCase();
		for (uint i = 0; i < 1000000; i += 1) {
			t = ((Item *)lst->arr + idx[i])->i;
		}
		LogCase("rand get");

		StartCase();
		for (uint i = 0; i < 100; i += 1) {
			if (idx[i] < (1000000 - i)) {
				refList.RemoveAt(idx[i]);
			}
		}
		RefCase();
		StartCase();
		for (uint i = 0; i < 100; i += 1) {
			if (idx[i] < (1000000 - i)) {
				Lst2.RemoveAt(lst, idx[i]);
			}
		}
		LogCase("rand remove");

		StartCase();
		for (uint i = 0; i < 50000; i += 1) {
			item->i = t + i;
			refList.Add(*item);
		}
		RefCase();
		StartCase();
		for (uint i = 0; i < 50000; i += 1) {
			item->i = t + i;
			*(Item *)Lst2.Push(lst) = *item;
		}
		LogCase("re push");
	}
}