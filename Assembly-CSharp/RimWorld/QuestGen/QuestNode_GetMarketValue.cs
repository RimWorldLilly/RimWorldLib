using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021C7 RID: 8647
	public class QuestNode_GetMarketValue : QuestNode
	{
		// Token: 0x0600D745 RID: 55109 RVA: 0x004FDE6C File Offset: 0x004FC06C
		protected override bool TestRunInt(Slate slate)
		{
			return this.DoWork(slate);
		}

		// Token: 0x0600D746 RID: 55110 RVA: 0x004FDE75 File Offset: 0x004FC075
		protected override void RunInt()
		{
			this.DoWork(QuestGen.slate);
		}

		// Token: 0x0600D747 RID: 55111 RVA: 0x004FDE84 File Offset: 0x004FC084
		private bool DoWork(Slate slate)
		{
			float num = 0f;
			if (this.things.GetValue(slate) != null)
			{
				foreach (Thing thing in this.things.GetValue(slate))
				{
					num += thing.MarketValue * (float)thing.stackCount;
				}
			}
			slate.Set<float>(this.storeAs.GetValue(slate), num, false);
			return true;
		}

		// Token: 0x0400889C RID: 34972
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400889D RID: 34973
		public SlateRef<IEnumerable<Thing>> things;
	}
}
