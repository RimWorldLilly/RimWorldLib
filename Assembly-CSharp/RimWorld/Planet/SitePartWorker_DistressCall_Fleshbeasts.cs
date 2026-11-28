using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.BaseGen;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020023C7 RID: 9159
	public class SitePartWorker_DistressCall_Fleshbeasts : SitePartWorker_DistressCall
	{
		// Token: 0x0600E465 RID: 58469 RVA: 0x0054DC24 File Offset: 0x0054BE24
		public override void PostMapGenerate(Map map)
		{
			Site site = map.Parent as Site;
			Faction faction = site.Faction ?? Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			if (faction.IsPlayer)
			{
				faction = Find.FactionManager.RandomEnemyFaction(false, false, true, TechLevel.Undefined);
			}
			List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
			{
				faction = faction,
				groupKind = PawnGroupKindDefOf.Settlement,
				points = SymbolResolver_Settlement.DefaultPawnsPoints.RandomInRange * 0.33f,
				tile = map.Tile
			}, true).ToList<Pawn>();
			float num = Faction.OfEntities.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Fleshbeasts, null) * 1.05f;
			float num2 = Mathf.Max(SitePartWorker_DistressCall_Fleshbeasts.FleshbeastsPointsModifierCurve.Evaluate(site.desiredThreatPoints), num);
			List<Pawn> list2 = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
			{
				groupKind = PawnGroupKindDefOf.Fleshbeasts,
				points = Rand.Range(num, num2 * 0.33f),
				faction = Faction.OfEntities,
				raidStrategy = RaidStrategyDefOf.ImmediateAttack
			}, true).ToList<Pawn>();
			List<Pawn> list3 = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
			{
				groupKind = PawnGroupKindDefOf.Fleshbeasts,
				points = num2,
				faction = Faction.OfEntities,
				raidStrategy = RaidStrategyDefOf.ImmediateAttack
			}, true).ToList<Pawn>();
			this.SplitFleshbeasts(ref list2);
			DistressCallUtility.SpawnCorpses(map, list2, list, map.Center, 20);
			DistressCallUtility.SpawnCorpses(map, list, list2.Concat(list3), map.Center, 20);
			DistressCallUtility.SpawnPawns(map, list3, map.Center, 20);
			this.ScatterCorpsesAroundPitBurrows(map, list);
		}

		// Token: 0x0600E466 RID: 58470 RVA: 0x0054DDB8 File Offset: 0x0054BFB8
		private void ScatterCorpsesAroundPitBurrows(Map map, IEnumerable<Pawn> killers)
		{
			foreach (Thing thing in map.listerThings.ThingsOfDef(ThingDefOf.PitBurrow))
			{
				if (Rand.Chance(0.4f))
				{
					int randomInRange = SitePartWorker_DistressCall_Fleshbeasts.BurrowCorpseCountRange.RandomInRange;
					List<Pawn> list = new List<Pawn>();
					for (int i = 0; i < randomInRange; i++)
					{
						list.Add(PawnGenerator.GeneratePawn(FleshbeastUtility.AllFleshbeasts.RandomElement<PawnKindDef>(), Faction.OfEntities, null));
					}
					this.SplitFleshbeasts(ref list);
					DistressCallUtility.SpawnCorpses(map, list, killers, thing.Position, 3);
				}
			}
		}

		// Token: 0x0600E467 RID: 58471 RVA: 0x0054DE7C File Offset: 0x0054C07C
		private void SplitFleshbeasts(ref List<Pawn> fleshbeasts)
		{
			List<Pawn> list = new List<Pawn>();
			foreach (Pawn fleshbeast in fleshbeasts)
			{
				foreach (Pawn item in FleshbeastUtility.SplitFleshbeast(fleshbeast))
				{
					list.Add(item);
				}
			}
			fleshbeasts = list;
		}

		// Token: 0x04009159 RID: 37209
		private const float CorpsePointFactor = 0.33f;

		// Token: 0x0400915A RID: 37210
		private const int SpawnRadius = 20;

		// Token: 0x0400915B RID: 37211
		private static readonly SimpleCurve FleshbeastsPointsModifierCurve = new SimpleCurve
		{
			{
				new CurvePoint(100f, 100f),
				true
			},
			{
				new CurvePoint(500f, 250f),
				true
			},
			{
				new CurvePoint(1000f, 400f),
				true
			},
			{
				new CurvePoint(5000f, 800f),
				true
			}
		};

		// Token: 0x0400915C RID: 37212
		private const float ChanceForFleshbeastCorpsesAroundBurrow = 0.4f;

		// Token: 0x0400915D RID: 37213
		private static readonly IntRange BurrowCorpseCountRange = new IntRange(1, 2);
	}
}
