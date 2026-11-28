using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E77 RID: 3703
	public class ThoughtWorker_ChildInGrowthVat : ThoughtWorker
	{
		// Token: 0x06006213 RID: 25107 RVA: 0x00234DE0 File Offset: 0x00232FE0
		public override string PostProcessLabel(Pawn p, string label)
		{
			int num = Mathf.RoundToInt(this.MoodMultiplier(p));
			if (num <= 1)
			{
				return base.PostProcessLabel(p, label);
			}
			return base.PostProcessLabel(p, label) + " x" + num.ToString();
		}

		// Token: 0x06006214 RID: 25108 RVA: 0x00234E20 File Offset: 0x00233020
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.BiotechActive)
			{
				return ThoughtState.Inactive;
			}
			if (this.ChildrenInGrowthVatCount(p) <= 0)
			{
				return ThoughtState.Inactive;
			}
			return ThoughtState.ActiveDefault;
		}

		// Token: 0x06006215 RID: 25109 RVA: 0x00234E44 File Offset: 0x00233044
		private int ChildrenInGrowthVatCount(Pawn pawn)
		{
			int num = 0;
			if (pawn.relations.ChildrenCount > 0)
			{
				foreach (Pawn pawn2 in pawn.relations.Children)
				{
					if (!pawn2.DevelopmentalStage.Adult() && pawn2.ParentHolder != null && pawn2.ParentHolder is Building_GrowthVat)
					{
						num++;
					}
				}
			}
			return num;
		}

		// Token: 0x06006216 RID: 25110 RVA: 0x00234EC8 File Offset: 0x002330C8
		public override float MoodMultiplier(Pawn p)
		{
			return (float)Mathf.Min(this.def.stackLimit, this.ChildrenInGrowthVatCount(p));
		}
	}
}
