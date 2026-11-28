using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DDA RID: 3546
	public class ThoughtWorker_Precept_ChildNotInGrowthVat : ThoughtWorker_Precept
	{
		// Token: 0x0600608E RID: 24718 RVA: 0x0022FAE0 File Offset: 0x0022DCE0
		public override string PostProcessLabel(Pawn p, string label)
		{
			int num = Mathf.RoundToInt(this.MoodMultiplier(p));
			if (num <= 1)
			{
				return base.PostProcessLabel(p, label);
			}
			return base.PostProcessLabel(p, label) + " x" + num.ToString();
		}

		// Token: 0x0600608F RID: 24719 RVA: 0x0022FB20 File Offset: 0x0022DD20
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!ModsConfig.IdeologyActive || !ModsConfig.BiotechActive)
			{
				return ThoughtState.Inactive;
			}
			if (this.ChildrenNotInGrowthVatCount(p) <= 0)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveDefault;
		}

		// Token: 0x06006090 RID: 24720 RVA: 0x0022FB4C File Offset: 0x0022DD4C
		private int ChildrenNotInGrowthVatCount(Pawn pawn)
		{
			int num = 0;
			if (pawn.relations.ChildrenCount > 0)
			{
				foreach (Pawn pawn2 in pawn.relations.Children)
				{
					if (!pawn2.Dead && pawn2.MapHeld == pawn.MapHeld && !pawn2.DevelopmentalStage.Adult() && !(pawn2.ParentHolder is Building_GrowthVat))
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06006091 RID: 24721 RVA: 0x0022FBDC File Offset: 0x0022DDDC
		public override float MoodMultiplier(Pawn p)
		{
			return (float)Mathf.Min(this.def.stackLimit, this.ChildrenNotInGrowthVatCount(p));
		}
	}
}
