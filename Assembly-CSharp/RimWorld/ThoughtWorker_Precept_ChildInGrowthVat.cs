using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DDB RID: 3547
	public class ThoughtWorker_Precept_ChildInGrowthVat : ThoughtWorker_Precept
	{
		// Token: 0x06006093 RID: 24723 RVA: 0x0022FC00 File Offset: 0x0022DE00
		public override string PostProcessLabel(Pawn p, string label)
		{
			int num = Mathf.RoundToInt(this.MoodMultiplier(p));
			if (num <= 1)
			{
				return base.PostProcessLabel(p, label);
			}
			return base.PostProcessLabel(p, label) + " x" + num.ToString();
		}

		// Token: 0x06006094 RID: 24724 RVA: 0x0022FC40 File Offset: 0x0022DE40
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!ModsConfig.IdeologyActive || !ModsConfig.BiotechActive)
			{
				return ThoughtState.Inactive;
			}
			if (this.ChildrenInGrowthVatCount(p) <= 0)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveDefault;
		}

		// Token: 0x06006095 RID: 24725 RVA: 0x0022FC6C File Offset: 0x0022DE6C
		private int ChildrenInGrowthVatCount(Pawn pawn)
		{
			int num = 0;
			if (pawn.relations.ChildrenCount > 0)
			{
				foreach (Pawn pawn2 in pawn.relations.Children)
				{
					if (pawn2.MapHeld == pawn.MapHeld && !pawn2.DevelopmentalStage.Adult() && pawn2.ParentHolder is Building_GrowthVat)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06006096 RID: 24726 RVA: 0x0022FCF4 File Offset: 0x0022DEF4
		public override float MoodMultiplier(Pawn p)
		{
			return (float)Mathf.Min(this.def.stackLimit, this.ChildrenInGrowthVatCount(p));
		}
	}
}
