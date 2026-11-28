using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02002154 RID: 8532
	public class SymbolStack
	{
		// Token: 0x170025F2 RID: 9714
		// (get) Token: 0x0600D53D RID: 54589 RVA: 0x004F17FF File Offset: 0x004EF9FF
		public bool Empty
		{
			get
			{
				return this.stack.Count == 0;
			}
		}

		// Token: 0x170025F3 RID: 9715
		// (get) Token: 0x0600D53E RID: 54590 RVA: 0x004F180F File Offset: 0x004EFA0F
		public int Count
		{
			get
			{
				return this.stack.Count;
			}
		}

		// Token: 0x0600D53F RID: 54591 RVA: 0x004F181C File Offset: 0x004EFA1C
		public void Push(string symbol, ResolveParams resolveParams, string customNameForPath = null)
		{
			string text = BaseGen.CurrentSymbolPath;
			if (!text.NullOrEmpty())
			{
				text += "_";
			}
			text += (customNameForPath ?? symbol);
			SymbolStack.Element item = default(SymbolStack.Element);
			item.symbol = symbol;
			item.resolveParams = resolveParams;
			item.symbolPath = text;
			this.stack.Push(item);
		}

		// Token: 0x0600D540 RID: 54592 RVA: 0x004F187C File Offset: 0x004EFA7C
		public void Push(string symbol, CellRect rect, string customNameForPath = null)
		{
			this.Push(symbol, new ResolveParams
			{
				rect = rect
			}, customNameForPath);
		}

		// Token: 0x0600D541 RID: 54593 RVA: 0x004F18A4 File Offset: 0x004EFAA4
		public void PushMany(ResolveParams resolveParams, params string[] symbols)
		{
			for (int i = 0; i < symbols.Length; i++)
			{
				this.Push(symbols[i], resolveParams, null);
			}
		}

		// Token: 0x0600D542 RID: 54594 RVA: 0x004F18CC File Offset: 0x004EFACC
		public void PushMany(CellRect rect, params string[] symbols)
		{
			for (int i = 0; i < symbols.Length; i++)
			{
				this.Push(symbols[i], rect, null);
			}
		}

		// Token: 0x0600D543 RID: 54595 RVA: 0x004F18F2 File Offset: 0x004EFAF2
		public SymbolStack.Element Pop()
		{
			return this.stack.Pop();
		}

		// Token: 0x0600D544 RID: 54596 RVA: 0x004F18FF File Offset: 0x004EFAFF
		public void Clear()
		{
			this.stack.Clear();
		}

		// Token: 0x04008727 RID: 34599
		private Stack<SymbolStack.Element> stack = new Stack<SymbolStack.Element>();

		// Token: 0x02003CEE RID: 15598
		public struct Element
		{
			// Token: 0x0400F0DA RID: 61658
			public string symbol;

			// Token: 0x0400F0DB RID: 61659
			public ResolveParams resolveParams;

			// Token: 0x0400F0DC RID: 61660
			public string symbolPath;
		}
	}
}
