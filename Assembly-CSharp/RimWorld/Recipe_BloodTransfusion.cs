using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200149E RID: 5278
	public class Recipe_BloodTransfusion : Recipe_Surgery
	{
		// Token: 0x0600815F RID: 33119 RVA: 0x002E2C8E File Offset: 0x002E0E8E
		public override bool CompletableEver(Pawn surgeryTarget)
		{
			return surgeryTarget.health.hediffSet.HasHediff(HediffDefOf.BloodLoss, false) && base.CompletableEver(surgeryTarget);
		}

		// Token: 0x06008160 RID: 33120 RVA: 0x002E2CB4 File Offset: 0x002E0EB4
		public override bool AvailableOnNow(Thing thing, BodyPartRecord part = null)
		{
			if (thing.MapHeld == null)
			{
				return false;
			}
			if (thing.MapHeld.listerThings.ThingsOfDef(ThingDefOf.HemogenPack).Count == 0)
			{
				return false;
			}
			Pawn pawn = thing as Pawn;
			return (pawn == null || pawn.health.hediffSet.HasHediff(HediffDefOf.BloodLoss, false)) && base.AvailableOnNow(thing, part);
		}

		// Token: 0x06008161 RID: 33121 RVA: 0x002E2D15 File Offset: 0x002E0F15
		public override void ConsumeIngredient(Thing ingredient, RecipeDef recipe, Map map)
		{
		}

		// Token: 0x06008162 RID: 33122 RVA: 0x002E2D18 File Offset: 0x002E0F18
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			if (!ModsConfig.BiotechActive)
			{
				return;
			}
			float num = 0f;
			float num2 = 0f;
			for (int i = 0; i < ingredients.Count; i++)
			{
				if (!ingredients[i].def.IsMedicine)
				{
					num += 0.35f * (float)ingredients[i].stackCount;
					num2 += JobGiver_GetHemogen.HemogenPackHemogenGain * (float)ingredients[i].stackCount;
				}
			}
			if (num > 0f)
			{
				Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BloodLoss, false);
				if (firstHediffOfDef != null)
				{
					firstHediffOfDef.Severity -= num;
				}
			}
			if (num2 > 0f)
			{
				Pawn_GeneTracker genes = pawn.genes;
				if (((genes != null) ? genes.GetFirstGeneOfType<Gene_Hemogen>() : null) != null)
				{
					GeneUtility.OffsetHemogen(pawn, num2, true);
				}
			}
			for (int j = 0; j < ingredients.Count; j++)
			{
				ingredients[j].Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06008163 RID: 33123 RVA: 0x002E2E08 File Offset: 0x002E1008
		public override float GetIngredientCount(IngredientCount ing, Bill bill)
		{
			BillStack billStack = bill.billStack;
			Pawn pawn = ((billStack != null) ? billStack.billGiver : null) as Pawn;
			if (pawn == null)
			{
				return base.GetIngredientCount(ing, bill);
			}
			Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.BloodLoss, false);
			if (firstHediffOfDef == null)
			{
				return base.GetIngredientCount(ing, bill);
			}
			return Mathf.Min((float)bill.Map.listerThings.ThingsOfDef(ThingDefOf.HemogenPack).Sum((Thing x) => x.stackCount), firstHediffOfDef.Severity / 0.35f);
		}

		// Token: 0x04005066 RID: 20582
		public const float BloodlossHealedPerPack = 0.35f;
	}
}
