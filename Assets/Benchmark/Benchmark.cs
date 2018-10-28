using UnityEngine;

public abstract class Benchmark : MonoBehaviour {
	System.Text.StringBuilder sb = new System.Text.StringBuilder();
	System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

	void OnEnable() {
		#if !FDB
		sb = new System.Text.StringBuilder();
		sb.AppendFormat("{0} benchmark start\n", GetTestName());
		var sw2 = new System.Diagnostics.Stopwatch(); sw2.Stop(); sw2.Reset(); sw2.Start();
		sw = new System.Diagnostics.Stopwatch(); sw.Stop(); sw.Reset(); sw.Start();
		RunTests();
		sb.AppendFormat("{0} benchmark end {1} ms\n", GetTestName(), sw2.ElapsedMilliseconds);
		Debug.Log(sb.ToString());
		#else
		Debug.Log(GetTestName() + " benchmark canceled");
		#endif
	}

	protected abstract string GetTestName();
	protected abstract void RunTests();

	protected void StartCase() {
		sw.Stop();
		sw.Reset();
		sw.Start();
	}

	long refTime;
	protected void RefCase() {
		refTime = sw.ElapsedTicks;
	}
	protected void LogCase(string name) {
		long time = sw.ElapsedTicks;
		sb.AppendFormat("\t{0:N0} {1} {2}\n\t{3:N0} {4}\n", refTime, GetTestName(), name, time, time <= refTime ? ":-)" : ":-(");
	}
}
