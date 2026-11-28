using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;
using Verse;

namespace LudeonTK
{
	// Token: 0x02000044 RID: 68
	public class Dialog_DebugOptionListLister : Dialog_DebugOptionLister
	{
		// Token: 0x17000051 RID: 81
		// (get) Token: 0x060001C5 RID: 453 RVA: 0x0000CBBC File Offset: 0x0000ADBC
		protected override int HighlightedIndex
		{
			get
			{
				if (this.options.NullOrEmpty<DebugMenuOption>())
				{
					return base.HighlightedIndex;
				}
				if (base.FilterAllows(this.options[this.prioritizedHighlightedIndex].label))
				{
					return this.prioritizedHighlightedIndex;
				}
				if (this.filter.NullOrEmpty())
				{
					return 0;
				}
				for (int i = 0; i < this.options.Count; i++)
				{
					if (base.FilterAllows(this.options[i].label))
					{
						this.currentHighlightIndex = i;
						break;
					}
				}
				return this.currentHighlightIndex;
			}
		}

		// Token: 0x060001C6 RID: 454 RVA: 0x0000CC4F File Offset: 0x0000AE4F
		public Dialog_DebugOptionListLister(IEnumerable<DebugMenuOption> options, string header = null)
		{
			this.options = options.ToList<DebugMenuOption>();
			this.header = header;
		}

		// Token: 0x060001C7 RID: 455 RVA: 0x0000CC6C File Offset: 0x0000AE6C
		protected override void DoListingItems(Rect inRect, float columnWidth)
		{
			if (KeyBindingDefOf.Dev_ChangeSelectedDebugAction.KeyDownEvent)
			{
				this.ChangeHighlightedOption();
			}
			if (!string.IsNullOrEmpty(this.header))
			{
				base.DebugLabel(this.header, columnWidth);
			}
			int highlightedIndex = this.HighlightedIndex;
			for (int i = 0; i < this.options.Count; i++)
			{
				DebugMenuOption debugMenuOption = this.options[i];
				bool highlight = highlightedIndex == i;
				DebugMenuOptionMode mode = debugMenuOption.mode;
				if (mode != DebugMenuOptionMode.Action)
				{
					if (mode == DebugMenuOptionMode.Tool)
					{
						base.DebugToolMap(debugMenuOption.label, columnWidth, debugMenuOption.method, highlight);
					}
				}
				else
				{
					base.DebugAction(debugMenuOption.label, columnWidth, debugMenuOption.method, highlight);
				}
			}
		}

		// Token: 0x060001C8 RID: 456 RVA: 0x0000CD14 File Offset: 0x0000AF14
		protected override void ChangeHighlightedOption()
		{
			int highlightedIndex = this.HighlightedIndex;
			for (int i = 0; i < this.options.Count; i++)
			{
				int num = (highlightedIndex + i + 1) % this.options.Count;
				if (base.FilterAllows(this.options[num].label))
				{
					this.prioritizedHighlightedIndex = num;
					return;
				}
			}
		}

		// Token: 0x060001C9 RID: 457 RVA: 0x0000CD74 File Offset: 0x0000AF74
		public static void ShowSimpleDebugMenu<T>(IEnumerable<T> elements, Func<T, string> label, Action<T> chosen)
		{
			List<DebugMenuOption> list = new List<DebugMenuOption>();
			using (IEnumerator<T> enumerator = elements.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					T t = enumerator.Current;
					list.Add(new DebugMenuOption(label(t), DebugMenuOptionMode.Action, delegate()
					{
						chosen(t);
					}));
				}
			}
			Find.WindowStack.Add(new Dialog_DebugOptionListLister(list, null));
		}

		// Token: 0x060001CA RID: 458 RVA: 0x0000CE10 File Offset: 0x0000B010
		public override void OnAcceptKeyPressed()
		{
			if (GUI.GetNameOfFocusedControl() == "DebugFilter")
			{
				int highlightedIndex = this.HighlightedIndex;
				if (highlightedIndex >= 0)
				{
					this.Close(true);
					if (this.options[highlightedIndex].mode == DebugMenuOptionMode.Action)
					{
						this.options[highlightedIndex].method();
					}
					else
					{
						DebugTools.curTool = new DebugTool(this.options[highlightedIndex].label, this.options[highlightedIndex].method, null);
					}
				}
				Event.current.Use();
			}
		}

		// Token: 0x040000DA RID: 218
		protected List<DebugMenuOption> options;

		// Token: 0x040000DB RID: 219
		protected string header;
	}
}
