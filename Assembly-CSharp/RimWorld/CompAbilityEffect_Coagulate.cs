using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020013E1 RID: 5089
	public class CompAbilityEffect_Coagulate : CompAbilityEffect
	{
		// Token: 0x170015A5 RID: 5541
		// (get) Token: 0x06007E88 RID: 32392 RVA: 0x002D3621 File Offset: 0x002D1821
		private new CompProperties_AbilityCoagulate Props
		{
			get
			{
				return (CompProperties_AbilityCoagulate)this.props;
			}
		}

		// Token: 0x06007E89 RID: 32393 RVA: 0x002D3630 File Offset: 0x002D1830
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Pawn pawn = target.Pawn;
			if (pawn == null)
			{
				return;
			}
			int num = 0;
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = hediffs.Count - 1; i >= 0; i--)
			{
				if ((hediffs[i] is Hediff_Injury || hediffs[i] is Hediff_MissingPart) && hediffs[i].TendableNow(false))
				{
					hediffs[i].Tended(this.Props.tendQualityRange.RandomInRange, this.Props.tendQualityRange.TrueMax, 1);
					num++;
				}
			}
			if (num > 0)
			{
				MoteMaker.ThrowText(pawn.DrawPos, pawn.Map, "NumWoundsTended".Translate(num), 3.65f);
			}
			FleckMaker.AttachedOverlay(pawn, FleckDefOf.FlashHollow, Vector3.zero, 1.5f, -1f);
		}

		// Token: 0x06007E8A RID: 32394 RVA: 0x002D371C File Offset: 0x002D191C
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			Pawn pawn = target.Pawn;
			return (pawn == null || AbilityUtility.ValidateHasTendableWound(pawn, throwMessages, this.parent)) && base.Valid(target, throwMessages);
		}
	}
}
