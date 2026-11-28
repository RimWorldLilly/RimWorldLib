using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020014D5 RID: 5333
	public static class PawnTechHediffsGenerator
	{
		// Token: 0x06008267 RID: 33383 RVA: 0x002EBDD8 File Offset: 0x002E9FD8
		public static void GenerateTechHediffsFor(Pawn pawn)
		{
			float partsMoney = pawn.kindDef.techHediffsMoney.RandomInRange;
			int num = pawn.kindDef.techHediffsMaxAmount;
			if (pawn.kindDef.techHediffsRequired != null)
			{
				foreach (ThingDef thingDef in pawn.kindDef.techHediffsRequired)
				{
					partsMoney -= thingDef.BaseMarketValue;
					num--;
					PawnTechHediffsGenerator.InstallPart(pawn, thingDef);
				}
			}
			if (pawn.kindDef.techHediffsTags == null || pawn.kindDef.techHediffsChance <= 0f)
			{
				return;
			}
			PawnTechHediffsGenerator.tmpGeneratedTechHediffsList.Clear();
			Func<ThingDef, bool> <>9__0;
			for (int i = 0; i < num; i++)
			{
				if (Rand.Value <= pawn.kindDef.techHediffsChance)
				{
					IEnumerable<ThingDef> allDefs = DefDatabase<ThingDef>.AllDefs;
					Func<ThingDef, bool> predicate;
					if ((predicate = <>9__0) == null)
					{
						predicate = (<>9__0 = ((ThingDef x) => x.isTechHediff && !PawnTechHediffsGenerator.tmpGeneratedTechHediffsList.Contains(x) && x.BaseMarketValue <= partsMoney && x.techHediffsTags != null && pawn.kindDef.techHediffsTags.Any((string tag) => x.techHediffsTags.Contains(tag)) && (!pawn.WorkTagIsDisabled(WorkTags.Violent) || !x.violentTechHediff) && (pawn.kindDef.techHediffsDisallowTags == null || !pawn.kindDef.techHediffsDisallowTags.Any((string tag) => x.techHediffsTags.Contains(tag)))));
					}
					IEnumerable<ThingDef> source = allDefs.Where(predicate);
					if (source.Any<ThingDef>())
					{
						ThingDef thingDef2 = source.RandomElementByWeight((ThingDef w) => w.BaseMarketValue);
						partsMoney -= thingDef2.BaseMarketValue;
						PawnTechHediffsGenerator.InstallPart(pawn, thingDef2);
						PawnTechHediffsGenerator.tmpGeneratedTechHediffsList.Add(thingDef2);
					}
				}
			}
		}

		// Token: 0x06008268 RID: 33384 RVA: 0x002EBF8C File Offset: 0x002EA18C
		private static void InstallPart(Pawn pawn, ThingDef partDef)
		{
			IEnumerable<RecipeDef> source = from x in DefDatabase<RecipeDef>.AllDefs
			where x.IsIngredient(partDef) && pawn.def.AllRecipes.Contains(x)
			select x;
			if (source.Any<RecipeDef>())
			{
				RecipeDef recipeDef = source.RandomElement<RecipeDef>();
				if (!recipeDef.targetsBodyPart)
				{
					recipeDef.Worker.ApplyOnPawn(pawn, null, null, PawnTechHediffsGenerator.emptyIngredientsList, null);
					return;
				}
				if (recipeDef.Worker.GetPartsToApplyOn(pawn, recipeDef).Any<BodyPartRecord>())
				{
					recipeDef.Worker.ApplyOnPawn(pawn, recipeDef.Worker.GetPartsToApplyOn(pawn, recipeDef).RandomElement<BodyPartRecord>(), null, PawnTechHediffsGenerator.emptyIngredientsList, null);
					return;
				}
			}
			else
			{
				CompProperties_UseEffectInstallImplant compProperties = partDef.GetCompProperties<CompProperties_UseEffectInstallImplant>();
				if (compProperties != null)
				{
					List<BodyPartRecord> partsWithDef = pawn.RaceProps.body.GetPartsWithDef(compProperties.bodyPart);
					pawn.health.AddHediff(compProperties.hediffDef, partsWithDef.NullOrEmpty<BodyPartRecord>() ? null : partsWithDef.RandomElement<BodyPartRecord>(), null, null);
				}
			}
		}

		// Token: 0x040050CC RID: 20684
		private static List<Thing> emptyIngredientsList = new List<Thing>();

		// Token: 0x040050CD RID: 20685
		private static List<ThingDef> tmpGeneratedTechHediffsList = new List<ThingDef>();
	}
}
