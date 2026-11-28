using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020014AD RID: 5293
	public class Recipe_RemoveImplant : Recipe_Surgery
	{
		// Token: 0x060081A3 RID: 33187 RVA: 0x002E415F File Offset: 0x002E235F
		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			Recipe_RemoveImplant.<GetPartsToApplyOn>d__0 <GetPartsToApplyOn>d__ = new Recipe_RemoveImplant.<GetPartsToApplyOn>d__0(-2);
			<GetPartsToApplyOn>d__.<>3__pawn = pawn;
			<GetPartsToApplyOn>d__.<>3__recipe = recipe;
			return <GetPartsToApplyOn>d__;
		}

		// Token: 0x060081A4 RID: 33188 RVA: 0x002E4178 File Offset: 0x002E2378
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			MedicalRecipesUtility.IsClean(pawn, part);
			bool flag = this.IsViolationOnPawn(pawn, part, Faction.OfPlayer);
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
				if (!pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Contains(part))
				{
					return;
				}
				Hediff hediff = pawn.health.hediffSet.hediffs.FirstOrDefault((Hediff x) => x.def == this.recipe.removesHediff);
				if (hediff != null)
				{
					if (hediff.def.spawnThingOnRemoved != null)
					{
						GenSpawn.Spawn(hediff.def.spawnThingOnRemoved, billDoer.Position, billDoer.Map, WipeMode.Vanish);
					}
					pawn.health.RemoveHediff(hediff);
				}
			}
			if (flag)
			{
				base.ReportViolation(pawn, billDoer, pawn.HomeFaction, -70, null);
			}
		}
	}
}
