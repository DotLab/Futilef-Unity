using UnityEngine;

public abstract class Benchmark : MonoBehaviour {
	System.Text.StringBuilder sb;
	System.Diagnostics.Stopwatch sw;

	void OnEnable() {
		sb = new System.Text.StringBuilder();
		sb.AppendFormat("{0} test start\n", GetTestName());
		var sw2 = new System.Diagnostics.Stopwatch(); sw2.Stop(); sw2.Reset();
		sw = new System.Diagnostics.Stopwatch(); sw.Stop(); sw.Reset(); sw.Start();
		RunTests();
		sb.AppendFormat("{0} test end, {1} ms\n", GetTestName(), sw2.ElapsedMilliseconds);
		Debug.Log(sb.ToString());
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
		sb.AppendFormat("\t{0} - {1}: {2:N0} ref\n\t{0} - {1}: {3:N0} {4}\n", GetTestName(), name, refTime, time, time <= refTime ? ":-)" : ":-(");
	}
}
