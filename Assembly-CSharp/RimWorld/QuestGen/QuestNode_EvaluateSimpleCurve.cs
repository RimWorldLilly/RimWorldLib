using System;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021B5 RID: 8629
	public class QuestNode_EvaluateSimpleCurve : QuestNode
	{
		// Token: 0x0600D6F9 RID: 55033 RVA: 0x004FCC50 File Offset: 0x004FAE50
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600D6FA RID: 55034 RVA: 0x004FCC5A File Offset: 0x004FAE5A
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600D6FB RID: 55035 RVA: 0x004FCC68 File Offset: 0x004FAE68
		private void SetVars(Slate slate)
		{
			float num = this.curve.GetValue(slate).Evaluate(this.value.GetValue(slate));
			if (this.roundRandom.GetValue(slate))
			{
				num = (float)GenMath.RoundRandom(num);
			}
			slate.Set<float>(this.storeAs.GetValue(slate), num, false);
		}

		// Token: 0x0400885A RID: 34906
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x0400885B RID: 34907
		public SlateRef<SimpleCurve> curve;

		// Token: 0x0400885C RID: 34908
		public SlateRef<float> value;

		// Token: 0x0400885D RID: 34909
		public SlateRef<bool> roundRandom;
	}
}
