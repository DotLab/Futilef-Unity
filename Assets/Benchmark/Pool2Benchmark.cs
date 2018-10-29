using Futilef;

public unsafe class Pool2Benchmark : Benchmark {
	protected override string GetTestName() {
		return "Pool2";
	}

	protected override void RunTests() {
		var pool = stackalloc Pool2[1]; Pool2.Init(pool);

		const int len = 10000;
		var sizes = stackalloc int[len];
		for (int i = 0; i < len; i += 1) {
			sizes[i] = UnityEngine.Random.Range(1, 1000);
		}

		var refPtrs = stackalloc byte *[len];
		var ptrs = stackalloc byte *[len];

		StartCase();
		for (int i = 0; i < len; i += 1) {
			refPtrs[i] = (byte *)Mem.Malloc(sizes[i]);
		}
		RefCase();
		StartCase();
		for (int i = 0; i < len; i += 1) {
			ptrs[i] = (byte *)Pool2.Alloc(pool, sizes[i]);
			if (pool->shift != 0) {
				long shift = pool->shift;
				for (int j = 0; j < i; j += 1) {
					ptrs[j] += shift;
				}
				pool->shift = 0;
			}
		}
		LogCase("rand alloc");

		StartCase();
		for (int i = 0; i < len; i += 1) {
			Mem.Free(refPtrs[i]);
		}
		RefCase();
		StartCase();
		for (int i = 0; i < len; i += 1) {
			Pool2.Free(pool, ptrs[i]);
		}
		LogCase("free");

		StartCase();
		for (int i = 0; i < len; i += 1) {
			refPtrs[i] = (byte *)Mem.Malloc(sizes[i]);
		}
		RefCase();
		StartCase();
		for (int i = 0; i < len; i += 1) {
			ptrs[i] = (byte *)Pool2.Alloc(pool, sizes[i]);
			if (pool->shift != 0) {
				long shift = pool->shift;
				for (int j = 0; j < i; j += 1) {
					ptrs[j] += shift;
				}
				pool->shift = 0;
			}
		}
		LogCase("rand alloc 2");

		StartCase();
		for (int i = 0; i < len; i += 1) {
			Mem.Free(refPtrs[i]);
		}
		RefCase();
		StartCase();
		for (int i = 0; i < len; i += 1) {
			Pool2.Free(pool, ptrs[i]);
		}
		LogCase("free 2");
	}
}
