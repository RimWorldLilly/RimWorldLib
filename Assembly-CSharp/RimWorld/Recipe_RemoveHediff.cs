using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020014AC RID: 5292
	public class Recipe_RemoveHediff : Recipe_Surgery
	{
		// Token: 0x0600819F RID: 33183 RVA: 0x002E3F00 File Offset: 0x002E2100
		public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
		{
			if (!base.AvailableOnNow(thing, part))
			{
				return false;
			}
			Pawn pawn = thing as Pawn;
			if (pawn == null)
			{
				return false;
			}
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				if ((!this.recipe.targetsBodyPart || hediffs[i].Part != null) && hediffs[i].def == this.recipe.removesHediff && hediffs[i].Visible)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x060081A0 RID: 33184 RVA: 0x002E3F8C File Offset: 0x002E218C
		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			Recipe_RemoveHediff.<GetPartsToApplyOn>d__1 <GetPartsToApplyOn>d__ = new Recipe_RemoveHediff.<GetPartsToApplyOn>d__1(-2);
			<GetPartsToApplyOn>d__.<>3__pawn = pawn;
			<GetPartsToApplyOn>d__.<>3__recipe = recipe;
			return <GetPartsToApplyOn>d__;
		}

		// Token: 0x060081A1 RID: 33185 RVA: 0x002E3FA4 File Offset: 0x002E21A4
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if (billDoer != null)
			{
				if (base.CheckSurgeryFail(billDoer, pawn, ingredients, part, bill))
				{
					return;
				}
				TaleRecorder.RecordTale(TaleDefOf.DidSurgery, new object[]
				{
					billDoer,
					pawn
				});
				if (PawnUtility.ShouldSendNotificationAbout(pawn) || PawnUtility.ShouldSendNotificationAbout(billDoer))
				{
					string text;
					if (!this.recipe.successfullyRemovedHediffMessage.NullOrEmpty())
					{
						text = this.recipe.successfullyRemovedHediffMessage.Formatted(billDoer.LabelShort, pawn.LabelShort);
					}
					else
					{
						text = "MessageSuccessfullyRemovedHediff".Translate(billDoer.LabelShort, pawn.LabelShort, this.recipe.removesHediff.label.Named("HEDIFF"), billDoer.Named("SURGEON"), pawn.Named("PATIENT"));
					}
					Messages.Message(text, pawn, MessageTypeDefOf.PositiveEvent, true);
				}
			}
			if (this.recipe.targetsBodyPart)
			{
				Hediff hediff = pawn.health.hediffSet.hediffs.Find((Hediff x) => x.def == this.recipe.removesHediff && x.Part == part && x.Visible);
				if (hediff != null)
				{
					pawn.health.RemoveHediff(hediff);
					return;
				}
			}
			else
			{
				for (int i = pawn.health.hediffSet.hediffs.Count - 1; i >= 0; i--)
				{
					Hediff hediff2 = pawn.health.hediffSet.hediffs[i];
					if (hediff2.def == this.recipe.removesHediff && hediff2.Visible)
					{
						pawn.health.RemoveHediff(hediff2);
					}
				}
			}
		}
	}
}
