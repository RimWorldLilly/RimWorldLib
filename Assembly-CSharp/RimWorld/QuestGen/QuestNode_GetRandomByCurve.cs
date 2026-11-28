using System;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021D4 RID: 8660
	public class QuestNode_GetRandomByCurve : QuestNode
	{
		// Token: 0x0600D781 RID: 55169 RVA: 0x004FFD8B File Offset: 0x004FDF8B
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600D782 RID: 55170 RVA: 0x004FFD95 File Offset: 0x004FDF95
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600D783 RID: 55171 RVA: 0x004FFDA4 File Offset: 0x004FDFA4
		private void SetVars(Slate slate)
		{
			float num = Rand.ByCurve(this.curve.GetValue(slate));
			if (this.roundRandom.GetValue(slate))
			{
				num = (float)GenMath.RoundRandom(num);
			}
			if (this.min.GetValue(slate) != null)
			{
				num = Mathf.Max(num, this.min.GetValue(slate).Value);
			}
			if (this.max.GetValue(slate) != null)
			{
				num = Mathf.Min(num, this.max.GetValue(slate).Value);
			}
			slate.Set<float>(this.storeAs.GetValue(slate), num, false);
		}

		// Token: 0x040088E7 RID: 35047
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x040088E8 RID: 35048
		public SlateRef<SimpleCurve> curve;

		// Token: 0x040088E9 RID: 35049
		public SlateRef<bool> roundRandom;

		// Token: 0x040088EA RID: 35050
		public SlateRef<float?> min;

		// Token: 0x040088EB RID: 35051
		public SlateRef<float?> max;
	}
}
