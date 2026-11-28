using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DF9 RID: 3577
	public class Thought_OpinionOfMyLover : Thought_Situational
	{
		// Token: 0x170010A6 RID: 4262
		// (get) Token: 0x060060D8 RID: 24792 RVA: 0x00230BA4 File Offset: 0x0022EDA4
		public override string LabelCap
		{
			get
			{
				DirectPawnRelation directPawnRelation = LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(this.pawn, false);
				string text = base.CurStage.label.Formatted(directPawnRelation.def.GetGenderSpecificLabel(directPawnRelation.otherPawn), directPawnRelation.otherPawn.LabelShort, directPawnRelation.otherPawn).CapitalizeFirst();
				if (this.def.Worker != null)
				{
					text = this.def.Worker.PostProcessLabel(this.pawn, text);
				}
				return text;
			}
		}

		// Token: 0x170010A7 RID: 4263
		// (get) Token: 0x060060D9 RID: 24793 RVA: 0x00230C34 File Offset: 0x0022EE34
		protected override float BaseMoodOffset
		{
			get
			{
				float num = 0.1f * (float)this.pawn.relations.OpinionOf(LovePartnerRelationUtility.ExistingMostLikedLovePartnerRel(this.pawn, false).otherPawn);
				if (num < 0f)
				{
					return Mathf.Min(num, -1f);
				}
				return Mathf.Max(num, 1f);
			}
		}
	}
}
