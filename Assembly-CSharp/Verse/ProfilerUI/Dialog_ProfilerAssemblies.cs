using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace Verse.ProfilerUI
{
	// Token: 0x02000786 RID: 1926
	public sealed class Dialog_ProfilerAssemblies : Window
	{
		// Token: 0x17000BD2 RID: 3026
		// (get) Token: 0x06003CA1 RID: 15521 RVA: 0x0017E439 File Offset: 0x0017C639
		public override Vector2 InitialSize
		{
			get
			{
				return new Vector2(560f, 640f);
			}
		}

		// Token: 0x06003CA2 RID: 15522 RVA: 0x0017E44C File Offset: 0x0017C64C
		public Dialog_ProfilerAssemblies(Func<string> getDraft, Action<string> setDraft) : base(null)
		{
			this.getDraft = getDraft;
			this.setDraft = setDraft;
			this.layer = WindowLayer.Super;
			this.doCloseX = true;
			this.draggable = true;
			this.closeOnClickedOutside = false;
			this.absorbInputAroundWindow = false;
			this.preventCameraMotion = false;
			this.optionalTitle = "Instrumented assemblies";
			this.BuildRows();
		}

		// Token: 0x06003CA3 RID: 15523 RVA: 0x0017E4D0 File Offset: 0x0017C6D0
		private void BuildRows()
		{
			Dictionary<Assembly, List<string>> patches = Dialog_ProfilerAssemblies.HarmonyPatchLines();
			HashSet<Assembly> seen = new HashSet<Assembly>();
			this.AddRow("Game", typeof(Root).Assembly, patches, seen);
			foreach (ModContentPack modContentPack in LoadedModManager.RunningModsListForReading)
			{
				foreach (Assembly assembly in modContentPack.assemblies.loadedAssemblies)
				{
					this.AddRow(modContentPack.Name, assembly, patches, seen);
				}
			}
		}

		// Token: 0x06003CA4 RID: 15524 RVA: 0x0017E598 File Offset: 0x0017C798
		private void AddRow(string group, Assembly assembly, Dictionary<Assembly, List<string>> patches, HashSet<Assembly> seen)
		{
			if (!assembly.IsDynamic && !assembly.IsCollectible && seen.Add(assembly))
			{
				string name = assembly.GetName().Name;
				if (name != null)
				{
					List<string> patchLines;
					patches.TryGetValue(assembly, out patchLines);
					this.rows.Add(new Dialog_ProfilerAssemblies.Row
					{
						group = group,
						assembly = name,
						patchLines = patchLines
					});
					return;
				}
			}
		}

		// Token: 0x06003CA5 RID: 15525 RVA: 0x0017E600 File Offset: 0x0017C800
		private static Dictionary<Assembly, List<string>> HarmonyPatchLines()
		{
			Dictionary<Assembly, List<string>> dictionary = new Dictionary<Assembly, List<string>>();
			HashSet<MethodBase> hashSet = new HashSet<MethodBase>();
			try
			{
				Type typeInAnyAssembly = GenTypes.GetTypeInAnyAssembly("HarmonyLib.Harmony", null);
				MethodInfo methodInfo = (typeInAnyAssembly != null) ? typeInAnyAssembly.GetMethod("GetAllPatchedMethods", BindingFlags.Static | BindingFlags.Public) : null;
				MethodInfo methodInfo2 = (typeInAnyAssembly != null) ? typeInAnyAssembly.GetMethod("GetPatchInfo", BindingFlags.Static | BindingFlags.Public) : null;
				if (methodInfo == null || methodInfo2 == null)
				{
					return dictionary;
				}
				foreach (object obj in ((IEnumerable)methodInfo.Invoke(null, null)))
				{
					MethodBase methodBase = (MethodBase)obj;
					object obj2 = methodInfo2.Invoke(null, new object[]
					{
						methodBase
					});
					if (obj2 != null)
					{
						foreach (string name in new string[]
						{
							"Prefixes",
							"Postfixes",
							"Finalizers"
						})
						{
							IEnumerable enumerable = Dialog_ProfilerAssemblies.Member(obj2, name) as IEnumerable;
							if (enumerable != null)
							{
								foreach (object target in enumerable)
								{
									MethodInfo methodInfo3 = Dialog_ProfilerAssemblies.Member(target, "PatchMethod") as MethodInfo;
									if (methodInfo3 != null)
									{
										Type declaringType = methodInfo3.DeclaringType;
										string text = (declaringType != null) ? declaringType.FullName : null;
										if (text != null && hashSet.Add(methodInfo3))
										{
											Assembly assembly = methodInfo3.DeclaringType.Assembly;
											List<string> list;
											if (!dictionary.TryGetValue(assembly, out list))
											{
												list = (dictionary[assembly] = new List<string>());
											}
											string item = string.Concat(new string[]
											{
												assembly.GetName().Name,
												"!",
												text,
												"::",
												methodInfo3.Name
											});
											if (!list.Contains(item))
											{
												list.Add(item);
											}
										}
									}
								}
							}
						}
					}
				}
			}
			catch (Exception ex)
			{
				Log.Warning("Could not list harmony patches for the profiler: " + ex.Message);
			}
			return dictionary;
		}

		// Token: 0x06003CA6 RID: 15526 RVA: 0x0017E880 File Offset: 0x0017CA80
		private static object Member(object target, string name)
		{
			Type type = target.GetType();
			FieldInfo field = type.GetField(name, BindingFlags.Instance | BindingFlags.Public);
			object result;
			if ((result = ((field != null) ? field.GetValue(target) : null)) == null)
			{
				PropertyInfo property = type.GetProperty(name, BindingFlags.Instance | BindingFlags.Public);
				if (property == null)
				{
					return null;
				}
				result = property.GetValue(target);
			}
			return result;
		}

		// Token: 0x06003CA7 RID: 15527 RVA: 0x0017E8C4 File Offset: 0x0017CAC4
		private void SyncLines()
		{
			string text = this.getDraft();
			if (text == this.linesSource)
			{
				return;
			}
			this.linesSource = text;
			this.lines.Clear();
			foreach (string text2 in text.Split(Dialog_ProfilerAssemblies.LineSeparators, StringSplitOptions.RemoveEmptyEntries))
			{
				this.lines.Add(text2.Trim());
			}
		}

		// Token: 0x06003CA8 RID: 15528 RVA: 0x0017E930 File Offset: 0x0017CB30
		private void SetLines(IEnumerable<string> toggled, bool on)
		{
			List<string> list = new List<string>();
			string[] array = this.getDraft().Split(Dialog_ProfilerAssemblies.LineSeparators, StringSplitOptions.RemoveEmptyEntries);
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i].Trim();
				if (text.Length > 0)
				{
					list.Add(text);
				}
			}
			using (IEnumerator<string> enumerator = toggled.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					string line = enumerator.Current;
					list.RemoveAll((string existing) => string.Equals(existing, line, StringComparison.OrdinalIgnoreCase));
					if (on)
					{
						list.Add(line);
					}
				}
			}
			this.setDraft(string.Join("\n", list));
		}

		// Token: 0x06003CA9 RID: 15529 RVA: 0x0017EA04 File Offset: 0x0017CC04
		private bool AllPresent(List<string> wanted)
		{
			foreach (string item in wanted)
			{
				if (!this.lines.Contains(item))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06003CAA RID: 15530 RVA: 0x0017EA60 File Offset: 0x0017CC60
		public override void DoWindowContents(Rect inRect)
		{
			this.SyncLines();
			Text.Font = GameFont.Small;
			this.search = Widgets.TextField(new Rect(inRect.x, inRect.y, inRect.width, 28f), this.search);
			List<Dialog_ProfilerAssemblies.Row> list = new List<Dialog_ProfilerAssemblies.Row>();
			foreach (Dialog_ProfilerAssemblies.Row row in this.rows)
			{
				if (this.search.Length == 0 || row.assembly.IndexOf(this.search, StringComparison.OrdinalIgnoreCase) >= 0 || row.group.IndexOf(this.search, StringComparison.OrdinalIgnoreCase) >= 0)
				{
					list.Add(row);
				}
			}
			float num = 0f;
			string b = null;
			foreach (Dialog_ProfilerAssemblies.Row row2 in list)
			{
				if (row2.group != b)
				{
					num += 24f;
				}
				b = row2.group;
				num += 24f;
			}
			Rect outRect = new Rect(inRect.x, inRect.y + 34f, inRect.width, inRect.height - 34f);
			Rect viewRect = new Rect(0f, 0f, outRect.width - 16f, num);
			Widgets.BeginScrollView(outRect, ref this.scroll, viewRect, true);
			float num2 = 0f;
			b = null;
			foreach (Dialog_ProfilerAssemblies.Row row3 in list)
			{
				if (row3.group != b)
				{
					GUI.color = Color.gray;
					Widgets.Label(new Rect(0f, num2 + 2f, viewRect.width, 24f), row3.group);
					GUI.color = Color.white;
					num2 += 24f;
					b = row3.group;
				}
				if (num2 + 24f >= this.scroll.y && num2 <= this.scroll.y + outRect.height)
				{
					this.DrawRow(new Rect(12f, num2, viewRect.width - 12f, 24f), row3);
				}
				num2 += 24f;
			}
			Widgets.EndScrollView();
		}

		// Token: 0x06003CAB RID: 15531 RVA: 0x0017ECF8 File Offset: 0x0017CEF8
		private void DrawRow(Rect rect, Dialog_ProfilerAssemblies.Row row)
		{
			string text = row.assembly + "!*";
			bool flag = this.lines.Contains(text);
			bool flag2 = flag;
			bool flag3 = row.patchLines != null;
			Widgets.CheckboxLabeled(new Rect(rect.x, rect.y, rect.width - (flag3 ? 118f : 0f), rect.height), row.assembly, ref flag, false, null, null, false, false);
			if (flag != flag2)
			{
				this.SetLines(new string[]
				{
					text
				}, flag);
			}
			if (!flag3)
			{
				return;
			}
			bool flag4 = this.AllPresent(row.patchLines);
			bool flag5 = flag4;
			Widgets.CheckboxLabeled(new Rect(rect.xMax - 110f, rect.y, 110f, rect.height), row.patchLines.Count.ToString() + " patches", ref flag4, false, null, null, false, false);
			if (flag4 != flag5)
			{
				this.SetLines(row.patchLines, flag4);
			}
		}

		// Token: 0x0400276A RID: 10090
		private const float RowHeight = 24f;

		// Token: 0x0400276B RID: 10091
		private const float PatchesWidth = 110f;

		// Token: 0x0400276C RID: 10092
		private static readonly char[] LineSeparators = new char[]
		{
			'\n',
			'\r'
		};

		// Token: 0x0400276D RID: 10093
		private readonly Func<string> getDraft;

		// Token: 0x0400276E RID: 10094
		private readonly Action<string> setDraft;

		// Token: 0x0400276F RID: 10095
		private readonly List<Dialog_ProfilerAssemblies.Row> rows = new List<Dialog_ProfilerAssemblies.Row>();

		// Token: 0x04002770 RID: 10096
		private readonly HashSet<string> lines = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

		// Token: 0x04002771 RID: 10097
		private string linesSource;

		// Token: 0x04002772 RID: 10098
		private string search = "";

		// Token: 0x04002773 RID: 10099
		private Vector2 scroll;

		// Token: 0x02002A91 RID: 10897
		private sealed class Row
		{
			// Token: 0x0400AE61 RID: 44641
			public string group;

			// Token: 0x0400AE62 RID: 44642
			public string assembly;

			// Token: 0x0400AE63 RID: 44643
			public List<string> patchLines;
		}
	}
}
