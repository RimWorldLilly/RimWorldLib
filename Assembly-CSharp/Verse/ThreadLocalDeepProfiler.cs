using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;

namespace Verse
{
	// Token: 0x020006BD RID: 1725
	public class ThreadLocalDeepProfiler
	{
		// Token: 0x060036BB RID: 14011 RVA: 0x0015F3DC File Offset: 0x0015D5DC
		static ThreadLocalDeepProfiler()
		{
			for (int i = 0; i < 50; i++)
			{
				ThreadLocalDeepProfiler.Prefixes[i] = "";
				for (int j = 0; j < i; j++)
				{
					string[] prefixes = ThreadLocalDeepProfiler.Prefixes;
					int num = i;
					prefixes[num] += " -";
				}
			}
		}

		// Token: 0x060036BC RID: 14012 RVA: 0x0015F432 File Offset: 0x0015D632
		public void Start(string label = null)
		{
			if (!Prefs.LogVerbose)
			{
				return;
			}
			this.watchers.Push(new ThreadLocalDeepProfiler.Watcher(label));
		}

		// Token: 0x060036BD RID: 14013 RVA: 0x0015F450 File Offset: 0x0015D650
		public void End()
		{
			if (!Prefs.LogVerbose)
			{
				return;
			}
			if (this.watchers.Count == 0)
			{
				Log.Error("Ended deep profiling while not profiling.");
				return;
			}
			ThreadLocalDeepProfiler.Watcher watcher = this.watchers.Pop();
			watcher.Watch.Stop();
			if (this.watchers.Count > 0)
			{
				this.watchers.Peek().AddChildResult(watcher);
				return;
			}
			this.Output(watcher);
		}

		// Token: 0x060036BE RID: 14014 RVA: 0x0015F4BC File Offset: 0x0015D6BC
		private void Output(ThreadLocalDeepProfiler.Watcher root)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (UnityData.IsInMainThread)
			{
				stringBuilder.AppendLine("--- Main thread ---");
			}
			else
			{
				stringBuilder.AppendLine("--- Thread " + Thread.CurrentThread.ManagedThreadId.ToString() + " ---");
			}
			List<ThreadLocalDeepProfiler.Watcher> list = new List<ThreadLocalDeepProfiler.Watcher>();
			list.Add(root);
			this.AppendStringRecursive(stringBuilder, root.Label, root.Children, root.ElapsedMilliseconds, 0, list, -1.0);
			stringBuilder.AppendLine();
			stringBuilder.AppendLine();
			this.HotspotAnalysis(stringBuilder, list);
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x060036BF RID: 14015 RVA: 0x0015F560 File Offset: 0x0015D760
		private void HotspotAnalysis(StringBuilder sb, List<ThreadLocalDeepProfiler.Watcher> allWatchers)
		{
			List<ThreadLocalDeepProfiler.LabelTimeTuple> list = new List<ThreadLocalDeepProfiler.LabelTimeTuple>();
			foreach (IGrouping<string, ThreadLocalDeepProfiler.Watcher> grouping in from w in allWatchers
			group w by w.Label)
			{
				double num = 0.0;
				double num2 = 0.0;
				int num3 = 0;
				foreach (ThreadLocalDeepProfiler.Watcher watcher in grouping)
				{
					num3++;
					num += watcher.ElapsedMilliseconds;
					if (watcher.Children != null)
					{
						foreach (ThreadLocalDeepProfiler.Watcher watcher2 in watcher.Children)
						{
							num2 += watcher2.ElapsedMilliseconds;
						}
					}
				}
				list.Add(new ThreadLocalDeepProfiler.LabelTimeTuple
				{
					label = num3.ToString() + "x " + grouping.Key,
					totalTime = num,
					selfTime = num - num2
				});
			}
			sb.AppendLine("Hotspot analysis");
			sb.AppendLine("----------------------------------------");
			foreach (ThreadLocalDeepProfiler.LabelTimeTuple labelTimeTuple in from e in list
			orderby e.selfTime descending
			select e)
			{
				sb.AppendLine(string.Concat(new string[]
				{
					labelTimeTuple.label,
					" -> ",
					labelTimeTuple.selfTime.ToString("0.000"),
					" ms (total (w/children): ",
					labelTimeTuple.totalTime.ToString("0.000"),
					" ms)"
				}));
			}
		}

		// Token: 0x060036C0 RID: 14016 RVA: 0x0015F794 File Offset: 0x0015D994
		private void AppendStringRecursive(StringBuilder sb, string label, List<ThreadLocalDeepProfiler.Watcher> children, double elapsedTime, int depth, List<ThreadLocalDeepProfiler.Watcher> allWatchers, double parentElapsedTime)
		{
			string text = ThreadLocalDeepProfiler.Prefixes[depth] + " " + elapsedTime.ToString("0.000") + "ms";
			if (depth > 0)
			{
				text = text + " (" + ((float)(elapsedTime / parentElapsedTime)).ToStringPercent() + ")";
			}
			if (children != null)
			{
				double num = elapsedTime;
				foreach (ThreadLocalDeepProfiler.Watcher watcher in children)
				{
					num -= watcher.ElapsedMilliseconds;
				}
				sb.AppendLine(string.Concat(new string[]
				{
					text,
					" (self: ",
					num.ToString("0.000"),
					" ms) ",
					label
				}));
			}
			else
			{
				sb.AppendLine(text + " " + label);
			}
			if (children != null)
			{
				allWatchers.AddRange(children);
				foreach (ValueTuple<List<ThreadLocalDeepProfiler.Watcher>, double> valueTuple in from x in (from c in children
				group c by c.Label).Select(delegate(IGrouping<string, ThreadLocalDeepProfiler.Watcher> x)
				{
					double num4 = 0.0;
					foreach (ThreadLocalDeepProfiler.Watcher watcher3 in x)
					{
						num4 += watcher3.ElapsedMilliseconds;
					}
					return new ValueTuple<List<ThreadLocalDeepProfiler.Watcher>, double>(x.ToList<ThreadLocalDeepProfiler.Watcher>(), num4);
				})
				orderby x.Item2 descending
				select x)
				{
					List<ThreadLocalDeepProfiler.Watcher> list = new List<ThreadLocalDeepProfiler.Watcher>();
					double num2 = 0.0;
					int num3 = 0;
					foreach (ThreadLocalDeepProfiler.Watcher watcher2 in valueTuple.Item1)
					{
						if (watcher2.Children != null)
						{
							foreach (ThreadLocalDeepProfiler.Watcher item in watcher2.Children)
							{
								list.Add(item);
							}
						}
						num2 += watcher2.ElapsedMilliseconds;
						num3++;
					}
					string label2 = valueTuple.Item1[0].Label;
					if (num3 <= 1)
					{
						this.AppendStringRecursive(sb, label2, list, num2, depth + 1, allWatchers, elapsedTime);
					}
					else
					{
						this.AppendStringRecursive(sb, num3.ToString() + "x " + label2, list, num2, depth + 1, allWatchers, elapsedTime);
					}
				}
			}
		}

		// Token: 0x04002401 RID: 9217
		private Stack<ThreadLocalDeepProfiler.Watcher> watchers = new Stack<ThreadLocalDeepProfiler.Watcher>();

		// Token: 0x04002402 RID: 9218
		private static readonly string[] Prefixes = new string[50];

		// Token: 0x04002403 RID: 9219
		private const int MaxDepth = 50;

		// Token: 0x02002A27 RID: 10791
		private class Watcher
		{
			// Token: 0x17002BBE RID: 11198
			// (get) Token: 0x06010579 RID: 66937 RVA: 0x005C3E50 File Offset: 0x005C2050
			public double ElapsedMilliseconds
			{
				get
				{
					return this.watch.Elapsed.TotalMilliseconds;
				}
			}

			// Token: 0x17002BBF RID: 11199
			// (get) Token: 0x0601057A RID: 66938 RVA: 0x005C3E70 File Offset: 0x005C2070
			public string Label
			{
				get
				{
					return this.label;
				}
			}

			// Token: 0x17002BC0 RID: 11200
			// (get) Token: 0x0601057B RID: 66939 RVA: 0x005C3E78 File Offset: 0x005C2078
			public Stopwatch Watch
			{
				get
				{
					return this.watch;
				}
			}

			// Token: 0x17002BC1 RID: 11201
			// (get) Token: 0x0601057C RID: 66940 RVA: 0x005C3E80 File Offset: 0x005C2080
			public List<ThreadLocalDeepProfiler.Watcher> Children
			{
				get
				{
					return this.children;
				}
			}

			// Token: 0x0601057D RID: 66941 RVA: 0x005C3E88 File Offset: 0x005C2088
			public Watcher(string label)
			{
				this.label = label;
				this.watch = Stopwatch.StartNew();
				this.children = null;
			}

			// Token: 0x0601057E RID: 66942 RVA: 0x005C3EA9 File Offset: 0x005C20A9
			public Watcher(string label, Stopwatch stopwatch)
			{
				this.label = label;
				this.watch = stopwatch;
				this.children = null;
			}

			// Token: 0x0601057F RID: 66943 RVA: 0x005C3EC6 File Offset: 0x005C20C6
			public void AddChildResult(ThreadLocalDeepProfiler.Watcher w)
			{
				if (this.children == null)
				{
					this.children = new List<ThreadLocalDeepProfiler.Watcher>();
				}
				this.children.Add(w);
			}

			// Token: 0x0400ACA3 RID: 44195
			private string label;

			// Token: 0x0400ACA4 RID: 44196
			private Stopwatch watch;

			// Token: 0x0400ACA5 RID: 44197
			private List<ThreadLocalDeepProfiler.Watcher> children;
		}

		// Token: 0x02002A28 RID: 10792
		private struct LabelTimeTuple
		{
			// Token: 0x0400ACA6 RID: 44198
			public string label;

			// Token: 0x0400ACA7 RID: 44199
			public double totalTime;

			// Token: 0x0400ACA8 RID: 44200
			public double selfTime;
		}
	}
}
