using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E89 RID: 3721
	public class ThoughtWorker_BabySick : ThoughtWorker
	{
		// Token: 0x06006258 RID: 25176 RVA: 0x00235908 File Offset: 0x00233B08
		public override string PostProcessLabel(Pawn p, string label)
		{
			IEnumerable<Pawn> source = p.relations.Children.Where(new Func<Pawn, bool>(this.IsSickBaby));
			if (source.Count<Pawn>() > 1)
			{
				return base.PostProcessLabel(p, label) + " x" + Mathf.RoundToInt(this.MoodMultiplier(p)).ToString();
			}
			Pawn arg = source.First<Pawn>();
			return label.Formatted(p.Named("PAWN"), arg.Named("BABY"));
		}

		// Token: 0x06006259 RID: 25177 RVA: 0x0023598C File Offset: 0x00233B8C
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!ModsConfig.BiotechActive)
			{
				return ThoughtState.Inactive;
			}
			int num = this.BabiesSickCount(p);
			if (num > 1)
			{
				return ThoughtState.ActiveAtStage(1);
			}
			if (num == 1)
			{
				return ThoughtState.ActiveAtStage(0);
			}
			return ThoughtState.Inactive;
		}

		// Token: 0x0600625A RID: 25178 RVA: 0x002359CC File Offset: 0x00233BCC
		private int BabiesSickCount(Pawn pawn)
		{
			int num = 0;
			foreach (Pawn pawn2 in pawn.relations.Children)
			{
				if (!pawn2.Dead && this.IsSickBaby(pawn2))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x0600625B RID: 25179 RVA: 0x00235A30 File Offset: 0x00233C30
		private bool IsSickBaby(Pawn pawn)
		{
			return pawn.DevelopmentalStage.Baby() && pawn.health.hediffSet.HasHediff(HediffDefOf.InfantIllness, false);
		}

		// Token: 0x0600625C RID: 25180 RVA: 0x00235A57 File Offset: 0x00233C57
		public override float MoodMultiplier(Pawn p)
		{
			return (float)Mathf.Min(this.def.stackLimit, this.BabiesSickCount(p));
		}

		// Token: 0x04003625 RID: 13861
		private const int SingleBabySickStage = 0;

		// Token: 0x04003626 RID: 13862
		private const int SeveralBabiesSickStage = 1;
	}
}
