using System;
using UnityEngine;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x020021B9 RID: 8633
	public class QuestNode_GetColonistCountFromColonyPercentage : QuestNode
	{
		// Token: 0x0600D709 RID: 55049 RVA: 0x004FCF80 File Offset: 0x004FB180
		protected override void RunInt()
		{
			this.SetVars(QuestGen.slate);
		}

		// Token: 0x0600D70A RID: 55050 RVA: 0x004FCF90 File Offset: 0x004FB190
		private void SetVars(Slate slate)
		{
			string value = this.storeAs.GetValue(slate);
			float minAgeResolved = this.minAge.GetValue(slate).GetValueOrDefault();
			int num = PawnsFinder.AllMaps_FreeColonistsSpawned.Count((Pawn c) => this.ColonistCounts(c, minAgeResolved));
			int var = Mathf.Clamp((int)((float)num * this.colonyPercentage.GetValue(slate)), 1, num - 1);
			slate.Set<int>(value, var, false);
		}

		// Token: 0x0600D70B RID: 55051 RVA: 0x004FD00C File Offset: 0x004FB20C
		protected override bool TestRunInt(Slate slate)
		{
			this.SetVars(slate);
			float num = (float)this.mustHaveFreeColonistsAvailableCount.GetValue(slate);
			if (num > 0f)
			{
				float minAgeResolved = this.minAge.GetValue(slate).GetValueOrDefault();
				return (float)PawnsFinder.AllMaps_FreeColonistsSpawned.Count((Pawn c) => this.ColonistCounts(c, minAgeResolved)) >= num;
			}
			return true;
		}

		// Token: 0x0600D70C RID: 55052 RVA: 0x004FD07C File Offset: 0x004FB27C
		private bool ColonistCounts(Pawn pawn, float minAge)
		{
			return !pawn.IsQuestLodger() && pawn.ageTracker.AgeBiologicalYearsFloat >= minAge;
		}

		// Token: 0x04008865 RID: 34917
		[NoTranslate]
		public SlateRef<string> storeAs;

		// Token: 0x04008866 RID: 34918
		public SlateRef<float> colonyPercentage;

		// Token: 0x04008867 RID: 34919
		public SlateRef<int> mustHaveFreeColonistsAvailableCount;

		// Token: 0x04008868 RID: 34920
		public SlateRef<float?> minAge;
	}
}
