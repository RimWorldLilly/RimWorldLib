using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020011B8 RID: 4536
	public abstract class IncidentWorker_Raid : IncidentWorker_PawnsArrive
	{
		// Token: 0x06007149 RID: 29001
		protected abstract bool TryResolveRaidFaction(IncidentParms parms);

		// Token: 0x0600714A RID: 29002
		public abstract void ResolveRaidStrategy(IncidentParms parms, PawnGroupKindDef groupKind);

		// Token: 0x0600714B RID: 29003
		protected abstract string GetLetterLabel(IncidentParms parms);

		// Token: 0x0600714C RID: 29004
		protected abstract string GetLetterText(IncidentParms parms, List<Pawn> pawns);

		// Token: 0x0600714D RID: 29005
		protected abstract LetterDef GetLetterDef();

		// Token: 0x0600714E RID: 29006
		protected abstract string GetRelatedPawnsInfoLetterText(IncidentParms parms);

		// Token: 0x0600714F RID: 29007
		protected abstract void ResolveRaidPoints(IncidentParms parms);

		// Token: 0x06007150 RID: 29008 RVA: 0x0027A6F8 File Offset: 0x002788F8
		public virtual bool TryResolveRaidArriveMode(IncidentParms parms)
		{
			if (parms.raidArrivalMode != null)
			{
				return false;
			}
			if (parms.raidArrivalModeForQuickMilitaryAid && !(from mode in DefDatabase<PawnsArrivalModeDef>.AllDefs
			where mode.forQuickMilitaryAid && mode.Worker.CanUseWith(parms)
			select mode).Any((PawnsArrivalModeDef mode) => IncidentWorker_Raid.ModeWeight(parms, mode) > 0f))
			{
				parms.raidArrivalMode = ((Rand.Value < 0.6f) ? PawnsArrivalModeDefOf.EdgeDrop : PawnsArrivalModeDefOf.CenterDrop);
				return true;
			}
			if (parms.raidStrategy == null)
			{
				Log.Error("parms raidStrategy was null but shouldn't be. Defaulting to ImmediateAttack.");
				parms.raidStrategy = RaidStrategyDefOf.ImmediateAttack;
			}
			return (from mode in parms.raidStrategy.arriveModes
			where mode.Worker.CanUseWith(parms)
			select mode).TryRandomElementByWeight((PawnsArrivalModeDef mode) => IncidentWorker_Raid.ModeWeight(parms, mode), out parms.raidArrivalMode);
		}

		// Token: 0x06007151 RID: 29009 RVA: 0x0027A7E0 File Offset: 0x002789E0
		private static float ModeWeight(IncidentParms parms, PawnsArrivalModeDef mode)
		{
			Map map = parms.target as Map;
			if (map != null && map.Tile.Valid && map.Tile.LayerDef.isSpace)
			{
				return Mathf.Max(mode.Worker.GetSelectionWeight(parms), mode.minSpaceSelectionWeight);
			}
			return mode.Worker.GetSelectionWeight(parms);
		}

		// Token: 0x06007152 RID: 29010 RVA: 0x0027A848 File Offset: 0x00278A48
		public virtual void ResolveRaidArriveMode(IncidentParms parms)
		{
			if (!this.TryResolveRaidArriveMode(parms))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(73, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Could not resolve arrival mode for raid. Defaulting to EdgeWalkIn. parms=");
				defaultInterpolatedStringHandler.AppendFormatted<IncidentParms>(parms);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				parms.raidArrivalMode = PawnsArrivalModeDefOf.EdgeWalkIn;
			}
		}

		// Token: 0x06007153 RID: 29011 RVA: 0x0027A893 File Offset: 0x00278A93
		public virtual void ResolveRaidAgeRestriction(IncidentParms parms)
		{
		}

		// Token: 0x06007154 RID: 29012 RVA: 0x0027A895 File Offset: 0x00278A95
		protected virtual void GenerateRaidLoot(IncidentParms parms, float raidLootPoints, List<Pawn> pawns)
		{
		}

		// Token: 0x06007155 RID: 29013 RVA: 0x0027A898 File Offset: 0x00278A98
		public bool TryGenerateRaidInfo(IncidentParms parms, out List<Pawn> pawns, bool debugTest = false)
		{
			pawns = null;
			this.ResolveRaidPoints(parms);
			if (!this.TryResolveRaidFaction(parms))
			{
				return false;
			}
			PawnGroupKindDef groupKind = parms.pawnGroupKind ?? PawnGroupKindDefOf.Combat;
			this.ResolveRaidStrategy(parms, groupKind);
			if (parms.raidArrivalMode == null && !this.TryResolveRaidArriveMode(parms))
			{
				return false;
			}
			this.ResolveRaidAgeRestriction(parms);
			if (!debugTest)
			{
				parms.raidStrategy.Worker.TryGenerateThreats(parms);
			}
			if (!debugTest && !parms.raidArrivalMode.Worker.TryResolveRaidSpawnCenter(parms))
			{
				return false;
			}
			float points = parms.points;
			parms.points = IncidentWorker_Raid.AdjustedRaidPoints(parms.points, parms.raidArrivalMode, parms.raidStrategy, parms.faction, groupKind, parms.target, parms.raidAgeRestriction);
			if (!debugTest)
			{
				pawns = parms.raidStrategy.Worker.SpawnThreats(parms);
			}
			if (pawns == null)
			{
				PawnGroupMakerParms defaultPawnGroupMakerParms = IncidentParmsUtility.GetDefaultPawnGroupMakerParms(groupKind, parms, false);
				pawns = PawnGroupMakerUtility.GeneratePawns(defaultPawnGroupMakerParms, true).ToList<Pawn>();
				if (pawns.Count == 0)
				{
					if (debugTest)
					{
						Log.Error("Got no pawns spawning raid from parms " + ((parms != null) ? parms.ToString() : null));
					}
					return false;
				}
				if (!debugTest)
				{
					parms.raidArrivalMode.Worker.Arrive(pawns, parms);
				}
			}
			parms.pawnCount = pawns.Count;
			this.PostProcessSpawnedPawns(parms, pawns);
			if (debugTest)
			{
				parms.target.StoryState.lastRaidFaction = parms.faction;
			}
			else
			{
				this.GenerateRaidLoot(parms, points, pawns);
			}
			return true;
		}

		// Token: 0x06007156 RID: 29014 RVA: 0x0027A9FC File Offset: 0x00278BFC
		protected virtual void PostProcessSpawnedPawns(IncidentParms parms, List<Pawn> pawns)
		{
		}

		// Token: 0x06007157 RID: 29015 RVA: 0x0027AA00 File Offset: 0x00278C00
		protected override bool TryExecuteWorker(IncidentParms parms)
		{
			List<Pawn> list;
			if (!this.TryGenerateRaidInfo(parms, out list, false))
			{
				return false;
			}
			TaggedString baseLetterLabel = this.GetLetterLabel(parms);
			TaggedString baseLetterText = this.GetLetterText(parms, list);
			PawnRelationUtility.Notify_PawnsSeenByPlayer_Letter(list, ref baseLetterLabel, ref baseLetterText, this.GetRelatedPawnsInfoLetterText(parms), true, true);
			List<TargetInfo> list2 = new List<TargetInfo>();
			if (parms.pawnGroups != null)
			{
				List<List<Pawn>> list3 = IncidentParmsUtility.SplitIntoGroups(list, parms.pawnGroups);
				List<Pawn> list4 = list3.MaxBy((List<Pawn> x) => x.Count);
				if (list4.Any<Pawn>())
				{
					list2.Add(list4[0]);
				}
				for (int i = 0; i < list3.Count; i++)
				{
					if (list3[i] != list4 && list3[i].Any<Pawn>())
					{
						list2.Add(list3[i][0]);
					}
				}
			}
			else if (list.Any<Pawn>())
			{
				foreach (Pawn t in list)
				{
					list2.Add(t);
				}
			}
			base.SendStandardLetter(baseLetterLabel, baseLetterText, this.GetLetterDef(), parms, list2, Array.Empty<NamedArgument>());
			if (parms.controllerPawn == null || parms.controllerPawn.Faction != Faction.OfPlayer)
			{
				parms.raidStrategy.Worker.MakeLords(parms, list);
			}
			LessonAutoActivator.TeachOpportunity(ConceptDefOf.EquippingWeapons, OpportunityType.Critical);
			if (!PlayerKnowledgeDatabase.IsComplete(ConceptDefOf.ShieldBelts))
			{
				for (int j = 0; j < list.Count; j++)
				{
					Pawn pawn = list[j];
					if (pawn.apparel != null)
					{
						if (pawn.apparel.WornApparel.Any((Apparel ap) => ap.def == ThingDefOf.Apparel_ShieldBelt))
						{
							LessonAutoActivator.TeachOpportunity(ConceptDefOf.ShieldBelts, OpportunityType.Critical);
							break;
						}
					}
				}
			}
			if (DebugSettings.logRaidInfo)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(17, 6);
				defaultInterpolatedStringHandler.AppendLiteral("Raid: ");
				defaultInterpolatedStringHandler.AppendFormatted(parms.faction.Name);
				defaultInterpolatedStringHandler.AppendLiteral(" (");
				defaultInterpolatedStringHandler.AppendFormatted(parms.faction.def.defName);
				defaultInterpolatedStringHandler.AppendLiteral(") ");
				defaultInterpolatedStringHandler.AppendFormatted(parms.raidArrivalMode.defName);
				defaultInterpolatedStringHandler.AppendLiteral(" ");
				defaultInterpolatedStringHandler.AppendFormatted(parms.raidStrategy.defName);
				defaultInterpolatedStringHandler.AppendLiteral(" c=");
				defaultInterpolatedStringHandler.AppendFormatted<IntVec3>(parms.spawnCenter);
				defaultInterpolatedStringHandler.AppendLiteral(" p=");
				defaultInterpolatedStringHandler.AppendFormatted<float>(parms.points);
				Log.Message(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return true;
		}

		// Token: 0x06007158 RID: 29016 RVA: 0x0027ACE0 File Offset: 0x00278EE0
		public static float AdjustedRaidPoints(float points, PawnsArrivalModeDef raidArrivalMode, RaidStrategyDef raidStrategy, Faction faction, PawnGroupKindDef groupKind, IIncidentTarget target, RaidAgeRestrictionDef ageRestriction = null)
		{
			if (raidArrivalMode.pointsFactorCurve != null)
			{
				points *= raidArrivalMode.pointsFactorCurve.Evaluate(points);
			}
			if (raidStrategy.pointsFactorCurve != null)
			{
				points *= raidStrategy.pointsFactorCurve.Evaluate(points);
			}
			if (ageRestriction != null)
			{
				points *= ageRestriction.threatPointsFactor;
			}
			if (target.Tile.Valid)
			{
				points *= target.Tile.LayerDef.raidPointsFactor;
			}
			points = Mathf.Max(points, raidStrategy.Worker.MinimumPoints(faction, groupKind) * 1.05f);
			return points;
		}

		// Token: 0x06007159 RID: 29017 RVA: 0x0027AD74 File Offset: 0x00278F74
		public void DoTable_RaidFactionSampled()
		{
			int ticksGame = Find.TickManager.TicksGame;
			Find.TickManager.DebugSetTicksGame(36000000);
			Faction lastRaidFaction = Find.CurrentMap.StoryState.lastRaidFaction;
			List<TableDataGetter<Faction>> list = new List<TableDataGetter<Faction>>();
			list.Add(new TableDataGetter<Faction>("name", (Faction f) => f.Name + " (" + f.def.defName + ")"));
			foreach (float points in DebugActionsUtility.PointsOptions(false))
			{
				Dictionary<Faction, int> factionCount = new Dictionary<Faction, int>();
				foreach (Faction key in Find.FactionManager.AllFactions)
				{
					factionCount.Add(key, 0);
				}
				for (int i = 0; i < 500; i++)
				{
					IncidentParms incidentParms = new IncidentParms();
					incidentParms.target = Find.CurrentMap;
					incidentParms.points = points;
					if (this.TryResolveRaidFaction(incidentParms))
					{
						Dictionary<Faction, int> factionCount2 = factionCount;
						Faction faction = incidentParms.faction;
						int num = factionCount2[faction];
						factionCount2[faction] = num + 1;
						Find.CurrentMap.StoryState.lastRaidFaction = incidentParms.faction;
					}
				}
				list.Add(new TableDataGetter<Faction>(points.ToString("F0"), (Faction str) => ((float)factionCount[str] / 500f).ToStringPercent()));
			}
			Find.TickManager.DebugSetTicksGame(ticksGame);
			Find.CurrentMap.StoryState.lastRaidFaction = lastRaidFaction;
			DebugTables.MakeTablesDialog<Faction>(Find.FactionManager.AllFactions, list.ToArray());
		}

		// Token: 0x0600715A RID: 29018 RVA: 0x0027AF64 File Offset: 0x00279164
		public void DoTable_RaidStrategySampled(Faction fac)
		{
			int ticksGame = Find.TickManager.TicksGame;
			Find.TickManager.DebugSetTicksGame(36000000);
			FactionRelationKind? factionRelationKind = null;
			int? num = null;
			if (fac != null && !fac.HostileTo(Faction.OfPlayer))
			{
				if (fac.HasGoodwill)
				{
					num = new int?(fac.GoodwillToMakeHostile(Faction.OfPlayer));
					fac.ChangeGoodwill_Debug(Faction.OfPlayer, num.Value);
				}
				else
				{
					factionRelationKind = new FactionRelationKind?(fac.RelationKindWith(Faction.OfPlayer));
					fac.SetRelationDirect(Faction.OfPlayer, FactionRelationKind.Hostile, false, null, null);
				}
			}
			List<TableDataGetter<RaidStrategyDef>> list = new List<TableDataGetter<RaidStrategyDef>>();
			list.Add(new TableDataGetter<RaidStrategyDef>("defName", (RaidStrategyDef d) => d.defName));
			foreach (float num2 in DebugActionsUtility.PointsOptions(false))
			{
				if (fac != null && num2 < fac.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat, null))
				{
					list.Add(new TableDataGetter<RaidStrategyDef>(num2.ToString("F0"), (RaidStrategyDef str) => "0%"));
				}
				else
				{
					Dictionary<RaidStrategyDef, int> strats = new Dictionary<RaidStrategyDef, int>();
					foreach (RaidStrategyDef key in DefDatabase<RaidStrategyDef>.AllDefs)
					{
						strats.Add(key, 0);
					}
					for (int i = 0; i < 500; i++)
					{
						IncidentParms incidentParms = new IncidentParms();
						incidentParms.target = Find.CurrentMap;
						incidentParms.points = num2;
						incidentParms.faction = fac;
						if (this.TryResolveRaidFaction(incidentParms))
						{
							this.ResolveRaidStrategy(incidentParms, PawnGroupKindDefOf.Combat);
							Find.CurrentMap.StoryState.lastRaidFaction = incidentParms.faction;
							if (incidentParms.raidStrategy != null)
							{
								Dictionary<RaidStrategyDef, int> strats2 = strats;
								RaidStrategyDef raidStrategy = incidentParms.raidStrategy;
								int num3 = strats2[raidStrategy];
								strats2[raidStrategy] = num3 + 1;
							}
						}
					}
					list.Add(new TableDataGetter<RaidStrategyDef>(num2.ToString("F0"), (RaidStrategyDef str) => ((float)strats[str] / 500f).ToStringPercent()));
				}
			}
			Find.TickManager.DebugSetTicksGame(ticksGame);
			if (factionRelationKind != null)
			{
				fac.SetRelationDirect(Faction.OfPlayer, factionRelationKind.Value, false, null, null);
			}
			else if (num != null)
			{
				fac.ChangeGoodwill_Debug(Faction.OfPlayer, -num.Value);
			}
			DebugTables.MakeTablesDialog<RaidStrategyDef>(DefDatabase<RaidStrategyDef>.AllDefs, list.ToArray());
		}

		// Token: 0x0600715B RID: 29019 RVA: 0x0027B264 File Offset: 0x00279464
		public void DoTable_RaidArrivalModeSampled(Faction fac)
		{
			int ticksGame = Find.TickManager.TicksGame;
			Find.TickManager.DebugSetTicksGame(36000000);
			Faction lastRaidFaction = Find.CurrentMap.StoryState.lastRaidFaction;
			FactionRelationKind? factionRelationKind = null;
			int? num = null;
			if (fac != null && !fac.HostileTo(Faction.OfPlayer))
			{
				if (fac.HasGoodwill)
				{
					num = new int?(fac.GoodwillToMakeHostile(Faction.OfPlayer));
					fac.ChangeGoodwill_Debug(Faction.OfPlayer, num.Value);
				}
				else
				{
					factionRelationKind = new FactionRelationKind?(fac.RelationKindWith(Faction.OfPlayer));
					fac.SetRelationDirect(Faction.OfPlayer, FactionRelationKind.Hostile, false, null, null);
				}
			}
			List<TableDataGetter<PawnsArrivalModeDef>> list = new List<TableDataGetter<PawnsArrivalModeDef>>();
			list.Add(new TableDataGetter<PawnsArrivalModeDef>("mode", (PawnsArrivalModeDef f) => f.defName));
			foreach (float num2 in DebugActionsUtility.PointsOptions(false))
			{
				if (num2 < fac.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Combat, null))
				{
					list.Add(new TableDataGetter<PawnsArrivalModeDef>(num2.ToString("F0"), (PawnsArrivalModeDef str) => "0%"));
				}
				else
				{
					Dictionary<PawnsArrivalModeDef, int> modeCount = new Dictionary<PawnsArrivalModeDef, int>();
					foreach (PawnsArrivalModeDef key in DefDatabase<PawnsArrivalModeDef>.AllDefs)
					{
						modeCount.Add(key, 0);
					}
					for (int i = 0; i < 500; i++)
					{
						IncidentParms incidentParms = new IncidentParms();
						incidentParms.target = Find.CurrentMap;
						incidentParms.points = num2;
						incidentParms.faction = fac;
						if (this.TryResolveRaidFaction(incidentParms))
						{
							Find.CurrentMap.storyState.lastRaidFaction = incidentParms.faction;
							this.ResolveRaidStrategy(incidentParms, PawnGroupKindDefOf.Combat);
							if (incidentParms.raidStrategy != null)
							{
								this.ResolveRaidArriveMode(incidentParms);
								Dictionary<PawnsArrivalModeDef, int> modeCount2 = modeCount;
								PawnsArrivalModeDef raidArrivalMode = incidentParms.raidArrivalMode;
								int num3 = modeCount2[raidArrivalMode];
								modeCount2[raidArrivalMode] = num3 + 1;
							}
						}
					}
					list.Add(new TableDataGetter<PawnsArrivalModeDef>(num2.ToString("F0"), (PawnsArrivalModeDef str) => ((float)modeCount[str] / 500f).ToStringPercent()));
				}
			}
			Find.TickManager.DebugSetTicksGame(ticksGame);
			Find.CurrentMap.storyState.lastRaidFaction = lastRaidFaction;
			if (factionRelationKind != null)
			{
				fac.SetRelationDirect(Faction.OfPlayer, factionRelationKind.Value, false, null, null);
			}
			else if (num != null)
			{
				fac.ChangeGoodwill_Debug(Faction.OfPlayer, -num.Value);
			}
			DebugTables.MakeTablesDialog<PawnsArrivalModeDef>(DefDatabase<PawnsArrivalModeDef>.AllDefs, list.ToArray());
		}
	}
}
