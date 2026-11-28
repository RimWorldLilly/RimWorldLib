using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x020014A3 RID: 5283
	public class Recipe_ImplantEmbryo : Recipe_Surgery
	{
		// Token: 0x06008179 RID: 33145 RVA: 0x002E34D9 File Offset: 0x002E16D9
		public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
		{
			return false;
		}

		// Token: 0x0600817A RID: 33146 RVA: 0x002E34DC File Offset: 0x002E16DC
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if (!ModsConfig.BiotechActive)
			{
				return;
			}
			HumanEmbryo humanEmbryo = (HumanEmbryo)ingredients.FirstOrDefault((Thing t) => t.def == ThingDefOf.HumanEmbryo);
			if (humanEmbryo == null)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(85, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Tried to perform implant embryo surgery on ");
				defaultInterpolatedStringHandler.AppendFormatted<Pawn>(pawn);
				defaultInterpolatedStringHandler.AppendLiteral(" but no embryo was in the ingredients list");
				Log.Warning(defaultInterpolatedStringHandler.ToStringAndClear());
				return;
			}
			if (base.CheckSurgeryFail(billDoer, pawn, ingredients, part, bill))
			{
				return;
			}
			if (Rand.Chance(PregnancyUtility.PregnancyChanceImplantEmbryo(pawn)))
			{
				Hediff_Pregnant hediff_Pregnant = (Hediff_Pregnant)HediffMaker.MakeHediff(HediffDefOf.PregnantHuman, pawn, null);
				hediff_Pregnant.SetParents(humanEmbryo.Mother, humanEmbryo.Father, humanEmbryo.GeneSet);
				pawn.health.AddHediff(hediff_Pregnant, null, null, null);
				return;
			}
			Messages.Message("ImplantFailedMessage".Translate(humanEmbryo.Label, pawn), pawn, MessageTypeDefOf.NegativeHealthEvent, true);
		}

		// Token: 0x0600817B RID: 33147 RVA: 0x002E35E8 File Offset: 0x002E17E8
		public override string LabelFromUniqueIngredients(Bill bill)
		{
			Bill_Medical bill_Medical = bill as Bill_Medical;
			if (bill_Medical != null && !bill_Medical.uniqueRequiredIngredients.NullOrEmpty<Thing>())
			{
				foreach (Thing thing in bill_Medical.uniqueRequiredIngredients)
				{
					HumanEmbryo humanEmbryo = thing as HumanEmbryo;
					if (humanEmbryo != null && !humanEmbryo.DestroyedOrNull())
					{
						return "ImplantSurgeryLabel".Translate(humanEmbryo.Label);
					}
				}
			}
			return null;
		}

		// Token: 0x0600817C RID: 33148 RVA: 0x002E367C File Offset: 0x002E187C
		public override bool CompletableEver(Pawn surgeryTarget)
		{
			return base.CompletableEver(surgeryTarget) && !surgeryTarget.Sterile();
		}
	}
}
