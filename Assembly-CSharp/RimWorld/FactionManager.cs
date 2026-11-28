using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001603 RID: 5635
	public class FactionManager : IExposable
	{
		// Token: 0x17001853 RID: 6227
		// (get) Token: 0x06008C54 RID: 35924 RVA: 0x003300FF File Offset: 0x0032E2FF
		public List<Faction> AllFactionsListForReading
		{
			get
			{
				return this.allFactions;
			}
		}

		// Token: 0x17001854 RID: 6228
		// (get) Token: 0x06008C55 RID: 35925 RVA: 0x00330107 File Offset: 0x0032E307
		public IEnumerable<Faction> AllFactions
		{
			get
			{
				return this.allFactions;
			}
		}

		// Token: 0x17001855 RID: 6229
		// (get) Token: 0x06008C56 RID: 35926 RVA: 0x0033010F File Offset: 0x0032E30F
		public IEnumerable<Faction> AllFactionsVisible
		{
			get
			{
				return from fa in this.allFactions
				where !fa.Hidden
				select fa;
			}
		}

		// Token: 0x17001856 RID: 6230
		// (get) Token: 0x06008C57 RID: 35927 RVA: 0x0033013B File Offset: 0x0032E33B
		public IEnumerable<Faction> AllFactionsVisibleInViewOrder
		{
			get
			{
				return FactionManager.GetInViewOrder(this.AllFactionsVisible);
			}
		}

		// Token: 0x17001857 RID: 6231
		// (get) Token: 0x06008C58 RID: 35928 RVA: 0x00330148 File Offset: 0x0032E348
		public IEnumerable<Faction> AllFactionsInViewOrder
		{
			get
			{
				return FactionManager.GetInViewOrder(this.AllFactions);
			}
		}

		// Token: 0x17001858 RID: 6232
		// (get) Token: 0x06008C59 RID: 35929 RVA: 0x00330155 File Offset: 0x0032E355
		public Faction OfPlayer
		{
			get
			{
				return this.ofPlayer;
			}
		}

		// Token: 0x17001859 RID: 6233
		// (get) Token: 0x06008C5A RID: 35930 RVA: 0x0033015D File Offset: 0x0032E35D
		public Faction OfMechanoids
		{
			get
			{
				return this.ofMechanoids;
			}
		}

		// Token: 0x1700185A RID: 6234
		// (get) Token: 0x06008C5B RID: 35931 RVA: 0x00330165 File Offset: 0x0032E365
		public Faction OfInsects
		{
			get
			{
				return this.ofInsects;
			}
		}

		// Token: 0x1700185B RID: 6235
		// (get) Token: 0x06008C5C RID: 35932 RVA: 0x0033016D File Offset: 0x0032E36D
		public Faction OfAncients
		{
			get
			{
				return this.ofAncients;
			}
		}

		// Token: 0x1700185C RID: 6236
		// (get) Token: 0x06008C5D RID: 35933 RVA: 0x00330175 File Offset: 0x0032E375
		public Faction OfAncientsHostile
		{
			get
			{
				return this.ofAncientsHostile;
			}
		}

		// Token: 0x1700185D RID: 6237
		// (get) Token: 0x06008C5E RID: 35934 RVA: 0x0033017D File Offset: 0x0032E37D
		public Faction OfEmpire
		{
			get
			{
				return this.empire;
			}
		}

		// Token: 0x1700185E RID: 6238
		// (get) Token: 0x06008C5F RID: 35935 RVA: 0x00330185 File Offset: 0x0032E385
		public Faction OfPirates
		{
			get
			{
				return this.ofPirates;
			}
		}

		// Token: 0x1700185F RID: 6239
		// (get) Token: 0x06008C60 RID: 35936 RVA: 0x0033018D File Offset: 0x0032E38D
		public Faction OfHoraxCult
		{
			get
			{
				return this.ofHoraxCult;
			}
		}

		// Token: 0x17001860 RID: 6240
		// (get) Token: 0x06008C61 RID: 35937 RVA: 0x00330195 File Offset: 0x0032E395
		public Faction OfEntities
		{
			get
			{
				return this.ofEntities;
			}
		}

		// Token: 0x17001861 RID: 6241
		// (get) Token: 0x06008C62 RID: 35938 RVA: 0x0033019D File Offset: 0x0032E39D
		public Faction OfTradersGuild
		{
			get
			{
				return this.ofTradersGuild;
			}
		}

		// Token: 0x17001862 RID: 6242
		// (get) Token: 0x06008C63 RID: 35939 RVA: 0x003301A5 File Offset: 0x0032E3A5
		public Faction OfSalvagers
		{
			get
			{
				return this.ofSalvagers;
			}
		}

		// Token: 0x06008C64 RID: 35940 RVA: 0x003301B0 File Offset: 0x0032E3B0
		public void ExposeData()
		{
			Scribe_Collections.Look<Faction>(ref this.allFactions, "allFactions", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<Faction>(ref this.toRemove, "toRemove", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				BackCompatibility.FactionManagerPostLoadInit();
				if (this.toRemove == null)
				{
					this.toRemove = new List<Faction>();
				}
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.ResolvingCrossRefs || Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.allFactions.RemoveAll((Faction x) => x == null || x.def == null) != 0)
				{
					Log.Error("Some factions were null after loading.");
				}
				this.RecacheFactions();
			}
		}

		// Token: 0x06008C65 RID: 35941 RVA: 0x00330260 File Offset: 0x0032E460
		public void Add(Faction faction)
		{
			if (this.allFactions.Contains(faction))
			{
				return;
			}
			this.allFactions.Add(faction);
			this.RecacheFactions();
			foreach (Map map in Find.Maps)
			{
				map.events.Notify_FactionAdded(faction);
			}
		}

		// Token: 0x06008C66 RID: 35942 RVA: 0x003302D8 File Offset: 0x0032E4D8
		private void Remove(Faction faction)
		{
			if (!faction.temporary)
			{
				Log.Error("Attempting to remove " + faction.Name + " which is not a temporary faction, only temporary factions can be removed");
				return;
			}
			if (!this.allFactions.Contains(faction))
			{
				return;
			}
			faction.RemoveAllRelations();
			this.allFactions.Remove(faction);
			List<Pawn> allMapsWorldAndTemporary_AliveOrDead = PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead;
			for (int i = 0; i < allMapsWorldAndTemporary_AliveOrDead.Count; i++)
			{
				if (allMapsWorldAndTemporary_AliveOrDead[i].Faction == faction)
				{
					allMapsWorldAndTemporary_AliveOrDead[i].SetFaction(null, null);
				}
			}
			for (int j = 0; j < Find.Maps.Count; j++)
			{
				Find.Maps[j].pawnDestinationReservationManager.Notify_FactionRemoved(faction);
				Find.Maps[j].listerBuildings.Notify_FactionRemoved(faction);
			}
			Find.LetterStack.Notify_FactionRemoved(faction);
			Find.PlayLog.Notify_FactionRemoved(faction);
			this.RecacheFactions();
			Find.QuestManager.Notify_FactionRemoved(faction);
			Find.IdeoManager.Notify_FactionRemoved(faction);
			Find.TaleManager.Notify_FactionRemoved(faction);
			foreach (Map map in Find.Maps)
			{
				map.events.Notify_FactionRemoved(faction);
			}
		}

		// Token: 0x06008C67 RID: 35943 RVA: 0x00330424 File Offset: 0x0032E624
		public void FactionManagerTick()
		{
			this.goodwillSituationManager.GoodwillManagerTick();
			SettlementProximityGoodwillUtility.CheckSettlementProximityGoodwillChange();
			for (int i = 0; i < this.allFactions.Count; i++)
			{
				this.allFactions[i].FactionTick();
			}
			for (int j = this.toRemove.Count - 1; j >= 0; j--)
			{
				Faction faction = this.toRemove[j];
				this.toRemove.Remove(faction);
				this.Remove(faction);
			}
		}

		// Token: 0x06008C68 RID: 35944 RVA: 0x003304A4 File Offset: 0x0032E6A4
		public Faction FirstFactionOfDef(FactionDef facDef)
		{
			for (int i = 0; i < this.allFactions.Count; i++)
			{
				if (this.allFactions[i].def == facDef)
				{
					return this.allFactions[i];
				}
			}
			return null;
		}

		// Token: 0x06008C69 RID: 35945 RVA: 0x003304EC File Offset: 0x0032E6EC
		public bool TryGetRandomNonColonyHumanlikeFaction(out Faction faction, bool tryMedievalOrBetter, bool allowDefeated = false, TechLevel minTechLevel = TechLevel.Undefined, TechLevel maxTechLevel = TechLevel.Undefined, bool allowTemporary = false, bool requireHostile = false)
		{
			return (from x in this.AllFactions
			where !x.IsPlayer && !x.Hidden && x.def.humanlikeFaction && (allowDefeated || !x.defeated) && (allowTemporary || !x.temporary) && (minTechLevel == TechLevel.Undefined || x.def.techLevel >= minTechLevel) && (maxTechLevel == TechLevel.Undefined || x.def.techLevel <= maxTechLevel) && (!requireHostile || x.HostileTo(Faction.OfPlayer))
			select x).TryRandomElementByWeight(delegate(Faction x)
			{
				if (tryMedievalOrBetter && x.def.techLevel < TechLevel.Medieval)
				{
					return 0.1f;
				}
				return 1f;
			}, out faction);
		}

		// Token: 0x06008C6A RID: 35946 RVA: 0x00330556 File Offset: 0x0032E756
		public IEnumerable<Faction> GetFactions(bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true, TechLevel minTechLevel = TechLevel.Undefined, bool allowTemporary = false)
		{
			FactionManager.<GetFactions>d__52 <GetFactions>d__ = new FactionManager.<GetFactions>d__52(-2);
			<GetFactions>d__.<>4__this = this;
			<GetFactions>d__.<>3__allowHidden = allowHidden;
			<GetFactions>d__.<>3__allowDefeated = allowDefeated;
			<GetFactions>d__.<>3__allowNonHumanlike = allowNonHumanlike;
			<GetFactions>d__.<>3__minTechLevel = minTechLevel;
			<GetFactions>d__.<>3__allowTemporary = allowTemporary;
			return <GetFactions>d__;
		}

		// Token: 0x06008C6B RID: 35947 RVA: 0x0033058C File Offset: 0x0032E78C
		public Faction RandomEnemyFaction(bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true, TechLevel minTechLevel = TechLevel.Undefined)
		{
			Faction result;
			if ((from x in this.GetFactions(allowHidden, allowDefeated, allowNonHumanlike, minTechLevel, false)
			where x.HostileTo(Faction.OfPlayer)
			select x).TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06008C6C RID: 35948 RVA: 0x003305D8 File Offset: 0x0032E7D8
		public Faction RandomRaidableEnemyFaction(bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true, TechLevel minTechLevel = TechLevel.Undefined)
		{
			Faction result;
			if ((from x in this.GetFactions(allowHidden, allowDefeated, allowNonHumanlike, minTechLevel, false)
			where x.HostileTo(Faction.OfPlayer) && !x.def.pawnGroupMakers.NullOrEmpty<PawnGroupMaker>()
			select x).TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06008C6D RID: 35949 RVA: 0x00330624 File Offset: 0x0032E824
		public Faction RandomNonHostileFaction(bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true, TechLevel minTechLevel = TechLevel.Undefined)
		{
			Faction result;
			if ((from x in this.GetFactions(allowHidden, allowDefeated, allowNonHumanlike, minTechLevel, false)
			where !x.HostileTo(Faction.OfPlayer)
			select x).TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06008C6E RID: 35950 RVA: 0x00330670 File Offset: 0x0032E870
		public Faction RandomAlliedFaction(bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true, TechLevel minTechLevel = TechLevel.Undefined)
		{
			Faction result;
			if ((from x in this.GetFactions(allowHidden, allowDefeated, allowNonHumanlike, minTechLevel, false)
			where x.PlayerRelationKind == FactionRelationKind.Ally
			select x).TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06008C6F RID: 35951 RVA: 0x003306BC File Offset: 0x0032E8BC
		public Faction RandomRoyalFaction(bool allowHidden = false, bool allowDefeated = false, bool allowNonHumanlike = true, TechLevel minTechLevel = TechLevel.Undefined)
		{
			Faction result;
			if ((from x in this.GetFactions(allowHidden, allowDefeated, allowNonHumanlike, minTechLevel, false)
			where x.def.HasRoyalTitles
			select x).TryRandomElement(out result))
			{
				return result;
			}
			return null;
		}

		// Token: 0x06008C70 RID: 35952 RVA: 0x00330708 File Offset: 0x0032E908
		public void LogKidnappedPawns()
		{
			Log.Message("Kidnapped pawns:");
			for (int i = 0; i < this.allFactions.Count; i++)
			{
				this.allFactions[i].kidnapped.LogKidnappedPawns();
			}
		}

		// Token: 0x06008C71 RID: 35953 RVA: 0x0033074C File Offset: 0x0032E94C
		public void LogAllFactions()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Faction faction in this.allFactions)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 3);
				defaultInterpolatedStringHandler.AppendLiteral("name: ");
				defaultInterpolatedStringHandler.AppendFormatted(faction.Name);
				defaultInterpolatedStringHandler.AppendLiteral(", temporary: ");
				defaultInterpolatedStringHandler.AppendFormatted<bool>(faction.temporary);
				defaultInterpolatedStringHandler.AppendLiteral(", can be deleted?: ");
				defaultInterpolatedStringHandler.AppendFormatted<bool>(this.FactionCanBeRemoved(faction));
				stringBuilder2.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			StringBuilder stringBuilder3 = stringBuilder;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(16, 1);
			defaultInterpolatedStringHandler2.AppendFormatted<int>(this.allFactions.Count);
			defaultInterpolatedStringHandler2.AppendLiteral(" factions found.");
			stringBuilder3.AppendLine(defaultInterpolatedStringHandler2.ToStringAndClear());
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06008C72 RID: 35954 RVA: 0x00330844 File Offset: 0x0032EA44
		public void LogFactionsToRemove()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (Faction faction in this.toRemove)
			{
				StringBuilder stringBuilder2 = stringBuilder;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 3);
				defaultInterpolatedStringHandler.AppendLiteral("name: ");
				defaultInterpolatedStringHandler.AppendFormatted(faction.Name);
				defaultInterpolatedStringHandler.AppendLiteral(", temporary: ");
				defaultInterpolatedStringHandler.AppendFormatted<bool>(faction.temporary);
				defaultInterpolatedStringHandler.AppendLiteral(", can be deleted?: ");
				defaultInterpolatedStringHandler.AppendFormatted<bool>(this.FactionCanBeRemoved(faction));
				stringBuilder2.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			StringBuilder stringBuilder3 = stringBuilder;
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(16, 1);
			defaultInterpolatedStringHandler2.AppendFormatted<int>(this.toRemove.Count);
			defaultInterpolatedStringHandler2.AppendLiteral(" factions found.");
			stringBuilder3.AppendLine(defaultInterpolatedStringHandler2.ToStringAndClear());
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06008C73 RID: 35955 RVA: 0x0033093C File Offset: 0x0032EB3C
		public void LogFactionsOnPawns()
		{
			StringBuilder stringBuilder = new StringBuilder();
			foreach (IGrouping<Faction, Pawn> grouping in from p in Find.WorldPawns.AllPawnsAliveOrDead
			group p by p.Faction)
			{
				if (grouping.Key == null)
				{
					StringBuilder stringBuilder2 = stringBuilder;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(25, 1);
					defaultInterpolatedStringHandler.AppendLiteral("no faction: ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(grouping.Count<Pawn>());
					defaultInterpolatedStringHandler.AppendLiteral(" pawns found.");
					stringBuilder2.AppendLine(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				else
				{
					StringBuilder stringBuilder3 = stringBuilder;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(15, 2);
					defaultInterpolatedStringHandler2.AppendFormatted<Faction>(grouping.Key);
					defaultInterpolatedStringHandler2.AppendLiteral(": ");
					defaultInterpolatedStringHandler2.AppendFormatted<int>(grouping.Count<Pawn>());
					defaultInterpolatedStringHandler2.AppendLiteral(" pawns found.");
					stringBuilder3.AppendLine(defaultInterpolatedStringHandler2.ToStringAndClear());
				}
			}
			Log.Message(stringBuilder.ToString());
		}

		// Token: 0x06008C74 RID: 35956 RVA: 0x00330A54 File Offset: 0x0032EC54
		public static IEnumerable<Faction> GetInViewOrder(IEnumerable<Faction> factions)
		{
			return from x in factions
			orderby x.defeated, x.def.listOrderPriority descending
			select x;
		}

		// Token: 0x06008C75 RID: 35957 RVA: 0x00330AAC File Offset: 0x0032ECAC
		private void RecacheFactions()
		{
			this.ofPlayer = null;
			for (int i = 0; i < this.allFactions.Count; i++)
			{
				if (this.allFactions[i].IsPlayer)
				{
					this.ofPlayer = this.allFactions[i];
					break;
				}
			}
			this.ofMechanoids = this.FirstFactionOfDef(FactionDefOf.Mechanoid);
			this.ofInsects = this.FirstFactionOfDef(FactionDefOf.Insect);
			this.ofAncients = this.FirstFactionOfDef(FactionDefOf.Ancients);
			this.ofAncientsHostile = this.FirstFactionOfDef(FactionDefOf.AncientsHostile);
			this.empire = this.FirstFactionOfDef(FactionDefOf.Empire);
			this.ofPirates = this.FirstFactionOfDef(FactionDefOf.Pirate);
			this.ofHoraxCult = this.FirstFactionOfDef(FactionDefOf.HoraxCult);
			this.ofEntities = this.FirstFactionOfDef(FactionDefOf.Entities);
			this.ofTradersGuild = this.FirstFactionOfDef(FactionDefOf.TradersGuild);
			this.ofSalvagers = this.FirstFactionOfDef(FactionDefOf.Salvagers);
		}

		// Token: 0x06008C76 RID: 35958 RVA: 0x00330BA8 File Offset: 0x0032EDA8
		public void Notify_QuestCleanedUp(Quest quest)
		{
			for (int i = this.allFactions.Count - 1; i >= 0; i--)
			{
				Faction faction = this.allFactions[i];
				if (this.FactionCanBeRemoved(faction))
				{
					this.QueueForRemoval(faction);
				}
			}
		}

		// Token: 0x06008C77 RID: 35959 RVA: 0x00330BEA File Offset: 0x0032EDEA
		public void Notify_PawnKilled(Pawn pawn)
		{
			this.TryQueuePawnFactionForRemoval(pawn);
		}

		// Token: 0x06008C78 RID: 35960 RVA: 0x00330BF3 File Offset: 0x0032EDF3
		public void Notify_PawnLeftMap(Pawn pawn)
		{
			this.TryQueuePawnFactionForRemoval(pawn);
		}

		// Token: 0x06008C79 RID: 35961 RVA: 0x00330BFC File Offset: 0x0032EDFC
		public void Notify_PawnLeftFaction(Faction oldFaction)
		{
			if (this.FactionCanBeRemoved(oldFaction))
			{
				this.QueueForRemoval(oldFaction);
			}
		}

		// Token: 0x06008C7A RID: 35962 RVA: 0x00330C0E File Offset: 0x0032EE0E
		public void Notify_WorldObjectDestroyed(WorldObject worldObject)
		{
			if (worldObject.Faction == null)
			{
				return;
			}
			if (this.FactionCanBeRemoved(worldObject.Faction))
			{
				this.QueueForRemoval(worldObject.Faction);
			}
		}

		// Token: 0x06008C7B RID: 35963 RVA: 0x00330C34 File Offset: 0x0032EE34
		private void TryQueuePawnFactionForRemoval(Pawn pawn)
		{
			if (pawn.Faction != null && this.FactionCanBeRemoved(pawn.Faction))
			{
				this.QueueForRemoval(pawn.Faction);
			}
			Faction extraHomeFaction = pawn.GetExtraHomeFaction(null);
			if (extraHomeFaction != null && this.FactionCanBeRemoved(extraHomeFaction))
			{
				this.QueueForRemoval(extraHomeFaction);
			}
			Faction extraMiniFaction = pawn.GetExtraMiniFaction(null);
			if (extraMiniFaction != null && this.FactionCanBeRemoved(extraMiniFaction))
			{
				this.QueueForRemoval(extraMiniFaction);
			}
			if (pawn.SlaveFaction != null && this.FactionCanBeRemoved(pawn.SlaveFaction))
			{
				this.QueueForRemoval(pawn.SlaveFaction);
			}
		}

		// Token: 0x06008C7C RID: 35964 RVA: 0x00330CBB File Offset: 0x0032EEBB
		private void QueueForRemoval(Faction faction)
		{
			if (!faction.temporary)
			{
				Log.Error("Cannot queue faction " + faction.Name + " for removal, only temporary factions can be removed");
				return;
			}
			if (!this.toRemove.Contains(faction))
			{
				this.toRemove.Add(faction);
			}
		}

		// Token: 0x06008C7D RID: 35965 RVA: 0x00330CFC File Offset: 0x0032EEFC
		private bool FactionCanBeRemoved(Faction faction)
		{
			FactionManager.<>c__DisplayClass71_0 CS$<>8__locals1;
			CS$<>8__locals1.faction = faction;
			if (!CS$<>8__locals1.faction.temporary || this.toRemove.Contains(CS$<>8__locals1.faction) || Find.QuestManager.IsReservedByAnyQuest(CS$<>8__locals1.faction))
			{
				return false;
			}
			if (!FactionManager.<FactionCanBeRemoved>g__CheckPawns|71_0(PawnsFinder.AllMaps_Spawned, ref CS$<>8__locals1))
			{
				return false;
			}
			if (!FactionManager.<FactionCanBeRemoved>g__CheckPawns|71_0(PawnsFinder.AllCaravansAndTravellingTransporters_Alive, ref CS$<>8__locals1))
			{
				return false;
			}
			using (List<WorldObject>.Enumerator enumerator = Find.WorldObjects.AllWorldObjects.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.Faction == CS$<>8__locals1.faction)
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06008C7F RID: 35967 RVA: 0x00330DE8 File Offset: 0x0032EFE8
		[CompilerGenerated]
		internal static bool <FactionCanBeRemoved>g__CheckPawns|71_0(IReadOnlyList<Pawn> pawns, ref FactionManager.<>c__DisplayClass71_0 A_1)
		{
			for (int i = 0; i < pawns.Count; i++)
			{
				Pawn pawn = pawns[i];
				if (!pawn.Dead && ((pawn.Faction != null && pawn.Faction == A_1.faction) || A_1.faction == pawn.GetExtraHomeFaction(null) || A_1.faction == pawn.GetExtraMiniFaction(null) || A_1.faction == pawn.SlaveFaction || (A_1.faction.leader == pawn && pawn.Faction != Faction.OfPlayer)))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x04005624 RID: 22052
		private List<Faction> allFactions = new List<Faction>();

		// Token: 0x04005625 RID: 22053
		private List<Faction> toRemove = new List<Faction>();

		// Token: 0x04005626 RID: 22054
		public GoodwillSituationManager goodwillSituationManager = new GoodwillSituationManager();

		// Token: 0x04005627 RID: 22055
		private Faction ofPlayer;

		// Token: 0x04005628 RID: 22056
		private Faction ofMechanoids;

		// Token: 0x04005629 RID: 22057
		private Faction ofInsects;

		// Token: 0x0400562A RID: 22058
		private Faction ofAncients;

		// Token: 0x0400562B RID: 22059
		private Faction ofAncientsHostile;

		// Token: 0x0400562C RID: 22060
		private Faction empire;

		// Token: 0x0400562D RID: 22061
		private Faction ofPirates;

		// Token: 0x0400562E RID: 22062
		private Faction ofHoraxCult;

		// Token: 0x0400562F RID: 22063
		private Faction ofEntities;

		// Token: 0x04005630 RID: 22064
		private Faction ofTradersGuild;

		// Token: 0x04005631 RID: 22065
		private Faction ofSalvagers;
	}
}
