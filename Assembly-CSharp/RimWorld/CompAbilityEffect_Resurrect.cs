using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200143B RID: 5179
	public class CompAbilityEffect_Resurrect : CompAbilityEffect
	{
		// Token: 0x170015D8 RID: 5592
		// (get) Token: 0x06007F9B RID: 32667 RVA: 0x002D8D26 File Offset: 0x002D6F26
		public new CompProperties_Resurrect Props
		{
			get
			{
				return (CompProperties_Resurrect)this.props;
			}
		}

		// Token: 0x06007F9C RID: 32668 RVA: 0x002D8D34 File Offset: 0x002D6F34
		public override void Apply(LocalTargetInfo target, LocalTargetInfo dest)
		{
			base.Apply(target, dest);
			Pawn innerPawn = ((Corpse)target.Thing).InnerPawn;
			if (ResurrectionUtility.TryResurrectWithSideEffects(innerPawn))
			{
				Messages.Message("MessagePawnResurrected".Translate(innerPawn), innerPawn, MessageTypeDefOf.PositiveEvent, true);
				MoteMaker.MakeAttachedOverlay(innerPawn, ThingDefOf.Mote_ResurrectFlash, Vector3.zero, 1f, -1f);
			}
		}

		// Token: 0x06007F9D RID: 32669 RVA: 0x002D8DA4 File Offset: 0x002D6FA4
		public override bool Valid(LocalTargetInfo target, bool throwMessages = false)
		{
			if (target.HasThing)
			{
				Corpse corpse = target.Thing as Corpse;
				if (corpse != null && corpse.GetRotStage() == RotStage.Dessicated)
				{
					if (throwMessages)
					{
						Messages.Message("MessageCannotResurrectDessicatedCorpse".Translate(), corpse, MessageTypeDefOf.RejectInput, false);
					}
					return false;
				}
			}
			return base.Valid(target, throwMessages);
		}
	}
}
