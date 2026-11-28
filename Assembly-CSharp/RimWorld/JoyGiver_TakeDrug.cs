using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B81 RID: 2945
	public class JoyGiver_TakeDrug : JoyGiver_Ingest
	{
		// Token: 0x06005583 RID: 21891 RVA: 0x001FCDDC File Offset: 0x001FAFDC
		protected override Thing BestIngestItem(Pawn pawn, Predicate<Thing> extraValidator)
		{
			JoyGiver_TakeDrug.<>c__DisplayClass1_0 CS$<>8__locals1 = new JoyGiver_TakeDrug.<>c__DisplayClass1_0();
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.pawn = pawn;
			CS$<>8__locals1.extraValidator = extraValidator;
			if (CS$<>8__locals1.pawn.drugs == null)
			{
				return null;
			}
			ThingOwner<Thing> innerContainer = CS$<>8__locals1.pawn.inventory.innerContainer;
			for (int i = 0; i < innerContainer.Count; i++)
			{
				if (CS$<>8__locals1.<BestIngestItem>g__Validator|0(innerContainer[i]))
				{
					return innerContainer[i];
				}
			}
			bool flag = CS$<>8__locals1.pawn.story != null && (CS$<>8__locals1.pawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire) > 0 || CS$<>8__locals1.pawn.InMentalState);
			JoyGiver_TakeDrug.takeableDrugs.Clear();
			DrugPolicy currentPolicy = CS$<>8__locals1.pawn.drugs.CurrentPolicy;
			for (int j = 0; j < currentPolicy.Count; j++)
			{
				if (flag || currentPolicy[j].allowedForJoy)
				{
					JoyGiver_TakeDrug.takeableDrugs.Add(currentPolicy[j].drug);
				}
			}
			JoyGiver_TakeDrug.takeableDrugs.Shuffle<ThingDef>();
			for (int k = 0; k < JoyGiver_TakeDrug.takeableDrugs.Count; k++)
			{
				List<Thing> list = CS$<>8__locals1.pawn.Map.listerThings.ThingsOfDef(JoyGiver_TakeDrug.takeableDrugs[k]);
				if (list.Count > 0)
				{
					Thing thing = GenClosest.ClosestThing_Global_Reachable(CS$<>8__locals1.pawn.Position, CS$<>8__locals1.pawn.Map, list, PathEndMode.OnCell, TraverseParms.For(CS$<>8__locals1.pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), 9999f, new Predicate<Thing>(CS$<>8__locals1.<BestIngestItem>g__Validator|0), null, false);
					if (thing != null)
					{
						return thing;
					}
				}
			}
			return null;
		}

		// Token: 0x06005584 RID: 21892 RVA: 0x001FCF88 File Offset: 0x001FB188
		public override float GetChance(Pawn pawn)
		{
			int num = 0;
			if (pawn.story != null)
			{
				num = pawn.story.traits.DegreeOfTrait(TraitDefOf.DrugDesire);
			}
			if (num < 0)
			{
				return 0f;
			}
			float num2 = this.def.baseChance;
			if (num == 1)
			{
				num2 *= 2f;
			}
			if (num == 2)
			{
				num2 *= 5f;
			}
			return num2;
		}

		// Token: 0x06005585 RID: 21893 RVA: 0x001FCFE4 File Offset: 0x001FB1E4
		protected override Job CreateIngestJob(Thing ingestible, Pawn pawn)
		{
			return DrugAIUtility.IngestAndTakeToInventoryJob(ingestible, pawn, 9999);
		}

		// Token: 0x040032E3 RID: 13027
		private static readonly List<ThingDef> takeableDrugs = new List<ThingDef>();
	}
}
