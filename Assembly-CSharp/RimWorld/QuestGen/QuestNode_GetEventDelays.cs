using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021BD RID: 8637
	public class QuestNode_GetEventDelays : QuestNode
	{
		// Token: 0x0600D71A RID: 55066 RVA: 0x004FD317 File Offset: 0x004FB517
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600D71B RID: 55067 RVA: 0x004FD321 File Offset: 0x004FB521
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600D71C RID: 55068 RVA: 0x004FD330 File Offset: 0x004FB530
		private void SetVars(Slate slate)
		{
			if (this.intervalTicksRange.GetValue(slate).max <= 0)
			{
				Log.Error("intervalTicksRange with max <= 0");
				return;
			}
			int num = 0;
			int num2 = 0;
			for (;;)
			{
				num += this.intervalTicksRange.GetValue(slate).RandomInRange;
				if (num > this.durationTicks.GetValue(slate))
				{
					break;
				}
				slate.Set<int>(this.storeDelaysAs.GetValue(slate).Formatted(num2.Named("INDEX")), num, false);
				num2++;
			}
			slate.Set<int>(this.storeCountAs.GetValue(slate), num2, false);
		}

		// Token: 0x04008871 RID: 34929
		public SlateRef<int> durationTicks;

		// Token: 0x04008872 RID: 34930
		public SlateRef<IntRange> intervalTicksRange;

		// Token: 0x04008873 RID: 34931
		[NoTranslate]
		public SlateRef<string> storeCountAs;

		// Token: 0x04008874 RID: 34932
		[NoTranslate]
		public SlateRef<string> storeDelaysAs;
	}
}
