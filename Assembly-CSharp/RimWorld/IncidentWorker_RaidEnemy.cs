using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020011B3 RID: 4531
	public class IncidentWorker_RaidEnemy : IncidentWorker_Raid
	{
		// Token: 0x06007124 RID: 28964 RVA: 0x002797B0 File Offset: 0x002779B0
		public override bool FactionCanBeGroupSource(Faction f, IncidentParms parms, bool desperate = false)
		{
			Map map = (Map)parms.target;
			if (!base.FactionCanBeGroupSource(f, parms, desperate))
			{
				return false;
			}
			if (!f.HostileTo(Faction.OfPlayer) || f.deactivated)
			{
				return false;
			}
			if (!desperate && (float)GenDate.DaysPassedSinceSettle < f.def.earliestRaidDays)
			{
				return false;
			}
			if (!f.def.raidArrivalLayerWhitelist.NullOrEmpty<PlanetLayerDef>() && !f.def.raidArrivalLayerWhitelist.Contains(map.Tile.LayerDef))
			{
				return false;
			}
			if (!f.def.raidArrivalLayerBlacklist.NullOrEmpty<PlanetLayerDef>() && f.def.raidArrivalLayerBlacklist.Contains(map.Tile.LayerDef))
			{
				return false;
			}
			Faction faction = parms.faction;
			parms.faction = f;
			if (!DefDatabase<RaidStrategyDef>.AllDefs.Any((RaidStrategyDef strategy) => strategy.Worker.CanUseWith(parms, null)))
			{
				parms.faction = faction;
				return false;
			}
			parms.faction = faction;
			return true;
		}

		// Token: 0x06007125 RID: 28965 RVA: 0x002798D0 File Offset: 0x00277AD0
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			if (!base.TryExecuteWorker(parms))
			{
				return false;
			}
			if (!parms.silent)
			{
				Find.TickManager.slower.SignalForceNormalSpeedShort();
			}
			Find.StoryWatcher.statsRecord.numRaidsEnemy++;
			parms.target.StoryState.lastRaidFaction = parms.faction;
			return true;
		}

		// Token: 0x06007126 RID: 28966 RVA: 0x00279930 File Offset: 0x00277B30
		protected override bool TryResolveRaidFaction(IncidentParms parms)
		{
			return (parms.faction != null && parms.faction.HostileTo(Faction.OfPlayer) && (!parms.faction.deactivated || parms.forced)) || PawnGroupMakerUtility.TryGetRandomFactionForCombatPawnGroupWeighted(parms, out parms.faction, (Faction f) => this.FactionCanBeGroupSource(f, parms, false), true, true, true, true) || PawnGroupMakerUtility.TryGetRandomFactionForCombatPawnGroupWeighted(parms, out parms.faction, (Faction f) => this.FactionCanBeGroupSource(f, parms, true), true, true, true, true);
		}

		// Token: 0x06007127 RID: 28967 RVA: 0x002799EB File Offset: 0x00277BEB
		public override void ResolveRaidAgeRestriction(IncidentParms parms)
		{
			if (ModsConfig.BiotechActive && RaidAgeRestrictionDefOf.Children.Worker.CanUseWith(parms) && Rand.Chance(RaidAgeRestrictionDefOf.Children.chance))
			{
				parms.raidAgeRestriction = RaidAgeRestrictionDefOf.Children;
			}
		}

		// Token: 0x06007128 RID: 28968 RVA: 0x00279A22 File Offset: 0x00277C22
		protected override void ResolveRaidPoints(IncidentParms parms)
		{
			if (parms.points > 0f)
			{
				return;
			}
			Log.Error("RaidEnemy is resolving raid points. They should always be set before initiating the incident.");
			parms.points = StorytellerUtility.DefaultThreatPointsNow(parms.target);
		}

		// Token: 0x06007129 RID: 28969 RVA: 0x00279A50 File Offset: 0x00277C50
		public override void ResolveRaidStrategy(IncidentParms parms, PawnGroupKindDef groupKind)
		{
			IncidentWorker_RaidEnemy.<>c__DisplayClass5_0 CS$<>8__locals1 = new IncidentWorker_RaidEnemy.<>c__DisplayClass5_0();
			CS$<>8__locals1.parms = parms;
			CS$<>8__locals1.groupKind = groupKind;
			if (CS$<>8__locals1.parms.raidStrategy != null)
			{
				return;
			}
			CS$<>8__locals1.map = (Map)CS$<>8__locals1.parms.target;
			if (!DefDatabase<RaidStrategyDef>.AllDefs.Where(new Func<RaidStrategyDef, bool>(CS$<>8__locals1.<ResolveRaidStrategy>g__CanUseStrategy|0)).TryRandomElementByWeight((RaidStrategyDef d) => d.Worker.SelectionWeightForFaction(CS$<>8__locals1.map, CS$<>8__locals1.parms.faction, CS$<>8__locals1.parms.points), out CS$<>8__locals1.parms.raidStrategy))
			{
				string[] array = new string[8];
				array[0] = "No raid strategy found, defaulting to ImmediateAttack. Faction=";
				array[1] = CS$<>8__locals1.parms.faction.def.defName;
				array[2] = ", points=";
				array[3] = CS$<>8__locals1.parms.points.ToString();
				array[4] = ", groupKind=";
				int num = 5;
				PawnGroupKindDef groupKind2 = CS$<>8__locals1.groupKind;
				array[num] = ((groupKind2 != null) ? groupKind2.ToString() : null);
				array[6] = ", parms=";
				int num2 = 7;
				IncidentParms parms2 = CS$<>8__locals1.parms;
				array[num2] = ((parms2 != null) ? parms2.ToString() : null);
				Log.Error(string.Concat(array));
				CS$<>8__locals1.parms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
			}
		}

		// Token: 0x0600712A RID: 28970 RVA: 0x00279B61 File Offset: 0x00277D61
		protected override string GetLetterLabel(IncidentParms parms)
		{
			return parms.raidStrategy.letterLabelEnemy + ": " + parms.faction.Name;
		}

		// Token: 0x0600712B RID: 28971 RVA: 0x00279B84 File Offset: 0x00277D84
		protected override string GetLetterText(IncidentParms parms, List<Pawn> pawns)
		{
			string text = string.Format(parms.raidArrivalMode.textEnemy, parms.faction.def.pawnsPlural, parms.faction.Name.ApplyTag(parms.faction)).CapitalizeFirst();
			text += "\n\n";
			text += parms.raidStrategy.arrivalTextEnemy;
			Pawn pawn = pawns.Find((Pawn x) => x.Faction.leader == x);
			if (pawn != null)
			{
				text += "\n\n";
				text += "EnemyRaidLeaderPresent".Translate(pawn.Faction.def.pawnsPlural, pawn.LabelShort, pawn.Named("LEADER")).Resolve();
			}
			if (parms.raidAgeRestriction != null && !parms.raidAgeRestriction.arrivalTextExtra.NullOrEmpty())
			{
				text += "\n\n";
				text += parms.raidAgeRestriction.arrivalTextExtra.Formatted(parms.faction.def.pawnsPlural.Named("PAWNSPLURAL")).Resolve();
			}
			return text;
		}

		// Token: 0x0600712C RID: 28972 RVA: 0x00279CC9 File Offset: 0x00277EC9
		protected override LetterDef GetLetterDef()
		{
			return LetterDefOf.ThreatBig;
		}

		// Token: 0x0600712D RID: 28973 RVA: 0x00279CD0 File Offset: 0x00277ED0
		protected override string GetRelatedPawnsInfoLetterText(IncidentParms parms)
		{
			return "LetterRelatedPawnsRaidEnemy".Translate(Faction.OfPlayer.def.pawnsPlural, parms.faction.def.pawnsPlural);
		}

		// Token: 0x0600712E RID: 28974 RVA: 0x00279D0C File Offset: 0x00277F0C
		protected override void GenerateRaidLoot(IncidentParms parms, float raidLootPoints, List<Pawn> pawns)
		{
			if (parms.faction.def.raidLootMaker == null || !pawns.Any<Pawn>())
			{
				return;
			}
			raidLootPoints *= Find.Storyteller.difficulty.EffectiveRaidLootPointsFactor;
			float num = parms.faction.def.raidLootValueFromPointsCurve.Evaluate(raidLootPoints);
			if (parms.raidStrategy != null)
			{
				num *= parms.raidStrategy.raidLootValueFactor;
			}
			ThingSetMakerParams parms2 = default(ThingSetMakerParams);
			parms2.totalMarketValueRange = new FloatRange?(new FloatRange(num, num));
			parms2.makingFaction = parms.faction;
			List<Thing> loot = parms.faction.def.raidLootMaker.root.Generate(parms2);
			new RaidLootDistributor(parms, pawns, loot).DistributeLoot();
		}
	}
}
