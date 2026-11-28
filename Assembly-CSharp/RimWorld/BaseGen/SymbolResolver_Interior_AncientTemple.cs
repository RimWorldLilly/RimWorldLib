using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.BaseGen
{
	// Token: 0x02002141 RID: 8513
	public class SymbolResolver_Interior_AncientTemple : SymbolResolver
	{
		// Token: 0x0600D50C RID: 54540 RVA: 0x004EF9A4 File Offset: 0x004EDBA4
		public override void Resolve(ResolveParams rp)
		{
			List<Thing> list = ThingSetMakerDefOf.MapGen_AncientTempleContents.root.Generate();
			list.SortByDescending((Thing t) => t.MarketValue * (float)t.stackCount);
			for (int i = 0; i < list.Count; i++)
			{
				ResolveParams resolveParams = rp;
				if (ModsConfig.IdeologyActive && i == 0)
				{
					resolveParams.singleThingDef = ThingDefOf.AncientHermeticCrate;
					resolveParams.singleThingInnerThings = new List<Thing>
					{
						list[0]
					};
				}
				else
				{
					resolveParams.singleThingToSpawn = list[i];
				}
				BaseGen.symbolStack.Push("thing", resolveParams, null);
			}
			if (!Find.Storyteller.difficulty.peacefulTemples)
			{
				if (ModsConfig.AnomalyActive && Rand.Chance(0.5f) && Faction.OfEntities != null)
				{
					ResolveParams resolveParams2 = rp;
					resolveParams2.fleshbeastsCount = new int?(rp.fleshbeastsCount ?? SymbolResolver_Interior_AncientTemple.FleshbeastsCountRange.RandomInRange);
					BaseGen.symbolStack.Push("randomFleshbeastGroup", resolveParams2, null);
				}
				else if (Rand.Chance(0.65f) && Faction.OfMechanoids != null)
				{
					ResolveParams resolveParams3 = rp;
					resolveParams3.mechanoidsCount = new int?(rp.mechanoidsCount ?? SymbolResolver_Interior_AncientTemple.MechanoidCountRange.RandomInRange);
					resolveParams3.disableSinglePawn = new bool?(rp.disableSinglePawn.GetValueOrDefault(true));
					resolveParams3.wakeSleepingMechanoidsOnUnfogged = new bool?(rp.wakeSleepingMechanoidsOnUnfogged.GetValueOrDefault(true));
					BaseGen.symbolStack.Push("randomMechanoidGroup", resolveParams3, null);
				}
				else if (Faction.OfInsects != null)
				{
					ResolveParams resolveParams4 = rp;
					resolveParams4.hivesCount = new int?(rp.hivesCount ?? SymbolResolver_Interior_AncientTemple.HivesCountRange.RandomInRange);
					resolveParams4.disableHives = new bool?(rp.disableHives.GetValueOrDefault(true));
					BaseGen.symbolStack.Push("hives", resolveParams4, null);
				}
			}
			if (rp.rect.Width >= SymbolResolver_Interior_AncientTemple.MinSizeForShrines.x && rp.rect.Height >= SymbolResolver_Interior_AncientTemple.MinSizeForShrines.z)
			{
				BaseGen.symbolStack.Push("ancientShrinesGroup", rp, null);
			}
			if (ModsConfig.IdeologyActive)
			{
				ResolveParams resolveParams5 = rp;
				resolveParams5.singleThingDef = ThingDefOf.AncientBarrel;
				BaseGen.symbolStack.Push("edgeThing", resolveParams5, null);
			}
		}

		// Token: 0x0400871A RID: 34586
		private const float FleshbeastsChance = 0.5f;

		// Token: 0x0400871B RID: 34587
		private const float MechanoidsChance = 0.65f;

		// Token: 0x0400871C RID: 34588
		private static readonly IntRange MechanoidCountRange = new IntRange(1, 5);

		// Token: 0x0400871D RID: 34589
		private static readonly IntRange HivesCountRange = new IntRange(1, 2);

		// Token: 0x0400871E RID: 34590
		private static readonly IntRange FleshbeastsCountRange = new IntRange(2, 5);

		// Token: 0x0400871F RID: 34591
		private static readonly IntVec2 MinSizeForShrines = new IntVec2(4, 3);
	}
}
