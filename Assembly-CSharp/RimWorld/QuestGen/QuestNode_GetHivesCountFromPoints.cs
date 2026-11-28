using System;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021C2 RID: 8642
	public class QuestNode_GetHivesCountFromPoints : QuestNode
	{
		// Token: 0x0600D730 RID: 55088 RVA: 0x004FD90E File Offset: 0x004FBB0E
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			return true;
		}

		// Token: 0x0600D731 RID: 55089 RVA: 0x004FD918 File Offset: 0x004FBB18
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600D732 RID: 55090 RVA: 0x004FD928 File Offset: 0x004FBB28
		private void SetVars(Slate slate)
		{
			float num = slate.Get<float>("points", 0f, false);
			num *= IncidentWorker_Infestation.PointsFactorCurve.Evaluate(num);
			int num2 = Mathf.RoundToInt(num / 220f);
			if (num2 < 1)
			{
				num2 = 1;
			}
			slate.Set<int>(this.storeAs.GetValue(slate), num2, false);
		}

		// Token: 0x0400888D RID: 34957
		[NoTranslate]
		public SlateRef<string> storeAs;
	}
}
