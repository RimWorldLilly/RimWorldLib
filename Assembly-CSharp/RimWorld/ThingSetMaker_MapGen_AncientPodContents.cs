using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B6A RID: 7018
	public class ThingSetMaker_MapGen_AncientPodContents : ThingSetMaker
	{
		// Token: 0x0600B41B RID: 46107 RVA: 0x003F7C40 File Offset: 0x003F5E40
		protected override void Generate(ThingSetMakerParams parms, List<Thing> outThings)
		{
			PodContentsType podContentsType = parms.podContentsType ?? Gen.RandomEnumValue<PodContentsType>(true);
			switch (podContentsType)
			{
			case PodContentsType.Empty:
				break;
			case PodContentsType.AncientFriendly:
				outThings.Add(this.GenerateFriendlyAncient_NewTemp(parms));
				return;
			case PodContentsType.AncientIncapped:
				outThings.Add(this.GenerateIncappedAncient_NewTemp(parms));
				return;
			case PodContentsType.AncientHalfEaten:
				outThings.Add(this.GenerateHalfEatenAncient_NewTemp(parms));
				outThings.AddRange(this.GenerateScarabs_NewTemp(parms));
				return;
			case PodContentsType.AncientHostile:
				outThings.Add(this.GenerateAngryAncient_NewTemp(parms));
				return;
			case PodContentsType.Slave:
				outThings.Add(this.GenerateSlave_NewTemp(parms));
				return;
			default:
				Log.Error("Pod contents type not handled: " + podContentsType.ToString());
				break;
			}
		}

		// Token: 0x0600B41C RID: 46108 RVA: 0x003F7CFC File Offset: 0x003F5EFC
		private Pawn GenerateFriendlyAncient()
		{
			return this.GenerateFriendlyAncient_NewTemp(default(ThingSetMakerParams));
		}

		// Token: 0x0600B41D RID: 46109 RVA: 0x003F7D18 File Offset: 0x003F5F18
		private Pawn GenerateFriendlyAncient_NewTemp(ThingSetMakerParams parms)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.AncientSoldier, Faction.OfAncients, PawnGenerationContext.NonPlayer, parms.tile, false, false, false, true, false, 1f, false, true, false, true, true, false, true, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, true, false, false, -1, 0, false));
			this.GiveRandomLootInventoryForTombPawn(pawn);
			return pawn;
		}

		// Token: 0x0600B41E RID: 46110 RVA: 0x003F7DC0 File Offset: 0x003F5FC0
		private Pawn GenerateIncappedAncient()
		{
			return this.GenerateIncappedAncient_NewTemp(default(ThingSetMakerParams));
		}

		// Token: 0x0600B41F RID: 46111 RVA: 0x003F7DDC File Offset: 0x003F5FDC
		private Pawn GenerateIncappedAncient_NewTemp(ThingSetMakerParams parms)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.AncientSoldier, Faction.OfAncients, PawnGenerationContext.NonPlayer, parms.tile, false, false, false, true, false, 1f, false, true, false, true, true, false, true, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, true, false, false, -1, 0, false));
			HealthUtility.DamageUntilDowned(pawn, true, null, null, null);
			this.GiveRandomLootInventoryForTombPawn(pawn);
			return pawn;
		}

		// Token: 0x0600B420 RID: 46112 RVA: 0x003F7E90 File Offset: 0x003F6090
		private Pawn GenerateSlave()
		{
			return this.GenerateSlave_NewTemp(default(ThingSetMakerParams));
		}

		// Token: 0x0600B421 RID: 46113 RVA: 0x003F7EAC File Offset: 0x003F60AC
		private Pawn GenerateSlave_NewTemp(ThingSetMakerParams parms)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.Slave, Faction.OfAncients, PawnGenerationContext.NonPlayer, parms.tile, false, false, false, true, false, 1f, false, true, false, true, true, false, true, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, true, false, false, -1, 0, false));
			HealthUtility.DamageUntilDowned(pawn, true, null, null, null);
			this.GiveRandomLootInventoryForTombPawn(pawn);
			if (Rand.Value < 0.5f)
			{
				HealthUtility.DamageUntilDead(pawn, null, null, null);
			}
			return pawn;
		}

		// Token: 0x0600B422 RID: 46114 RVA: 0x003F7F74 File Offset: 0x003F6174
		private Pawn GenerateAngryAncient()
		{
			return this.GenerateAngryAncient_NewTemp(default(ThingSetMakerParams));
		}

		// Token: 0x0600B423 RID: 46115 RVA: 0x003F7F90 File Offset: 0x003F6190
		private Pawn GenerateAngryAncient_NewTemp(ThingSetMakerParams parms)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.AncientSoldier, Faction.OfAncientsHostile, PawnGenerationContext.NonPlayer, parms.tile, false, false, false, true, false, 1f, false, true, false, true, true, false, true, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, true, false, false, -1, 0, false));
			this.GiveRandomLootInventoryForTombPawn(pawn);
			return pawn;
		}

		// Token: 0x0600B424 RID: 46116 RVA: 0x003F8038 File Offset: 0x003F6238
		private Pawn GenerateHalfEatenAncient()
		{
			return this.GenerateHalfEatenAncient_NewTemp(default(ThingSetMakerParams));
		}

		// Token: 0x0600B425 RID: 46117 RVA: 0x003F8054 File Offset: 0x003F6254
		private Pawn GenerateHalfEatenAncient_NewTemp(ThingSetMakerParams parms)
		{
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.AncientSoldier, Faction.OfAncients, PawnGenerationContext.NonPlayer, parms.tile, false, false, false, true, false, 1f, false, true, false, true, true, false, true, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, true, false, false, -1, 0, false));
			int num = Rand.Range(6, 10);
			for (int i = 0; i < num; i++)
			{
				pawn.TakeDamage(new DamageInfo(DamageDefOf.Bite, (float)Rand.Range(3, 8), 0f, -1f, pawn, null, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
			}
			this.GiveRandomLootInventoryForTombPawn(pawn);
			return pawn;
		}

		// Token: 0x0600B426 RID: 46118 RVA: 0x003F8144 File Offset: 0x003F6344
		private List<Thing> GenerateScarabs()
		{
			return this.GenerateScarabs_NewTemp(default(ThingSetMakerParams));
		}

		// Token: 0x0600B427 RID: 46119 RVA: 0x003F8160 File Offset: 0x003F6360
		private List<Thing> GenerateScarabs_NewTemp(ThingSetMakerParams parms)
		{
			List<Thing> list = new List<Thing>();
			int num = Rand.Range(3, 6);
			for (int i = 0; i < num; i++)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(PawnKindDefOf.Megascarab, null, parms.tile);
				pawn.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Manhunter, null, false, false, false, null, false, false, false);
				list.Add(pawn);
			}
			return list;
		}

		// Token: 0x0600B428 RID: 46120 RVA: 0x003F81C0 File Offset: 0x003F63C0
		private void GiveRandomLootInventoryForTombPawn(Pawn p)
		{
			if (Rand.Value < 0.65f)
			{
				this.MakeIntoContainer(p.inventory.innerContainer, ThingDefOf.Gold, Rand.Range(10, 50));
			}
			else
			{
				this.MakeIntoContainer(p.inventory.innerContainer, ThingDefOf.Plasteel, Rand.Range(10, 50));
			}
			if (Rand.Value < 0.7f)
			{
				this.MakeIntoContainer(p.inventory.innerContainer, ThingDefOf.ComponentIndustrial, Rand.Range(-2, 4));
				return;
			}
			this.MakeIntoContainer(p.inventory.innerContainer, ThingDefOf.ComponentSpacer, Rand.Range(-2, 4));
		}

		// Token: 0x0600B429 RID: 46121 RVA: 0x003F8264 File Offset: 0x003F6464
		private void MakeIntoContainer(ThingOwner container, ThingDef def, int count)
		{
			if (count <= 0)
			{
				return;
			}
			Thing thing = ThingMaker.MakeThing(def, null);
			thing.stackCount = count;
			container.TryAdd(thing, true);
		}

		// Token: 0x0600B42A RID: 46122 RVA: 0x003F828E File Offset: 0x003F648E
		protected override IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms)
		{
			return new ThingSetMaker_MapGen_AncientPodContents.<AllGeneratableThingsDebugSub>d__15(-2);
		}
	}
}
