using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020012FA RID: 4858
	public class GenStep_SitePawns : GenStep
	{
		// Token: 0x1700149F RID: 5279
		// (get) Token: 0x0600787C RID: 30844 RVA: 0x002AA36A File Offset: 0x002A856A
		public override int SeedPart
		{
			get
			{
				return 897875812;
			}
		}

		// Token: 0x0600787D RID: 30845 RVA: 0x002AA374 File Offset: 0x002A8574
		private PawnGroupMakerParms GroupMakerParms(PlanetTile tile, Faction faction, float points, int seed)
		{
			PawnGroupKindDef settlement = this.pawnGroupKindDef;
			if (!faction.def.pawnGroupMakers.Any((PawnGroupMaker maker) => maker.kindDef == this.pawnGroupKindDef))
			{
				settlement = PawnGroupKindDefOf.Settlement;
			}
			PawnGroupMakerParms pawnGroupMakerParms = new PawnGroupMakerParms
			{
				groupKind = settlement,
				tile = tile,
				faction = faction,
				inhabitants = true,
				generateFightersOnly = true,
				seed = new int?(seed)
			};
			pawnGroupMakerParms.points = Mathf.Max(points, faction.def.MinPointsToGeneratePawnGroup(pawnGroupMakerParms.groupKind, pawnGroupMakerParms));
			return pawnGroupMakerParms;
		}

		// Token: 0x0600787E RID: 30846 RVA: 0x002AA404 File Offset: 0x002A8604
		public override void Generate(Map map, GenStepParams parms)
		{
			GenStep_SitePawns.<>c__DisplayClass8_0 CS$<>8__locals1 = new GenStep_SitePawns.<>c__DisplayClass8_0();
			CS$<>8__locals1.map = map;
			CellRect cellRect;
			IntVec3 baseCenter = MapGenerator.TryGetVar<CellRect>("RectOfInterest", out cellRect) ? cellRect.CenterCell : CS$<>8__locals1.map.Center;
			Faction faction = Find.FactionManager.FirstFactionOfDef(this.factionDef);
			Lord lord = LordMaker.MakeNewLord(faction, new LordJob_SitePawns(faction, baseCenter, GenStep_SitePawns.Delay.RandomInRange), CS$<>8__locals1.map, null);
			int pawnGroupMakerSeed = OutpostSitePartUtility.GetPawnGroupMakerSeed(parms.sitePart.parms);
			PawnGroupMakerParms parms2 = this.GroupMakerParms(CS$<>8__locals1.map.Tile, faction, parms.sitePart.parms.threatPoints, pawnGroupMakerSeed);
			CellRect var = MapGenerator.GetVar<CellRect>("SpawnRect");
			LookTargets lookTargets = new LookTargets();
			foreach (Pawn pawn in PawnGroupMakerUtility.GeneratePawns(parms2, true))
			{
				IntVec3 loc;
				if (var.TryFindRandomCell(out loc, new Predicate<IntVec3>(CS$<>8__locals1.<Generate>g__Validator|0)))
				{
					GenSpawn.Spawn(pawn, loc, CS$<>8__locals1.map, WipeMode.Vanish);
					lord.AddPawn(pawn);
					lookTargets.targets.Add(pawn);
				}
			}
			if (lookTargets.Any)
			{
				Find.LetterStack.ReceiveLetter(this.letterLabel, this.letterDesc, LetterDefOf.ThreatSmall, lookTargets, null, null, null, null, 0, true);
			}
		}

		// Token: 0x04004B8B RID: 19339
		public FactionDef factionDef;

		// Token: 0x04004B8C RID: 19340
		public PawnGroupKindDef pawnGroupKindDef;

		// Token: 0x04004B8D RID: 19341
		[MustTranslate]
		public string letterLabel;

		// Token: 0x04004B8E RID: 19342
		[MustTranslate]
		public string letterDesc;

		// Token: 0x04004B8F RID: 19343
		private static readonly IntRange Delay = new IntRange(2500, 5000);
	}
}
