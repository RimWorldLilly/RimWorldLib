using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x020015B1 RID: 5553
	public class Pawn_PlayerSettings : IExposable
	{
		// Token: 0x170017B0 RID: 6064
		// (get) Token: 0x06008989 RID: 35209 RVA: 0x0031CF22 File Offset: 0x0031B122
		// (set) Token: 0x0600898A RID: 35210 RVA: 0x0031CF2C File Offset: 0x0031B12C
		public Pawn Master
		{
			get
			{
				return this.master;
			}
			set
			{
				if (this.master == value)
				{
					return;
				}
				if (value != null && !this.pawn.training.HasLearned(TrainableDefOf.Obedience))
				{
					Log.ErrorOnce("Attempted to set master for non-obedient pawn", 73908573);
					return;
				}
				bool flag = ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(this.pawn);
				this.master = value;
				if (this.pawn.Spawned && (flag || ThinkNode_ConditionalShouldFollowMaster.ShouldFollowMaster(this.pawn)))
				{
					this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
			}
		}

		// Token: 0x170017B1 RID: 6065
		// (get) Token: 0x0600898B RID: 35211 RVA: 0x0031CFB4 File Offset: 0x0031B1B4
		public Area EffectiveAreaRestrictionInPawnCurrentMap
		{
			get
			{
				if (!this.RespectsAllowedArea)
				{
					return null;
				}
				Area result;
				if (!this.allowedAreas.TryGetValue(this.pawn.MapHeld, out result))
				{
					return null;
				}
				return result;
			}
		}

		// Token: 0x170017B2 RID: 6066
		// (get) Token: 0x0600898C RID: 35212 RVA: 0x0031CFE8 File Offset: 0x0031B1E8
		// (set) Token: 0x0600898D RID: 35213 RVA: 0x0031D020 File Offset: 0x0031B220
		public Area AreaRestrictionInPawnCurrentMap
		{
			get
			{
				Area result;
				if (this.pawn.MapHeld == null || !this.allowedAreas.TryGetValue(this.pawn.MapHeld, out result))
				{
					return null;
				}
				return result;
			}
			set
			{
				Map map = this.pawn.MapHeld;
				if (map == null && this.pawn.DevelopmentalStage == DevelopmentalStage.Baby)
				{
					Pawn firstDirectRelationPawn = this.pawn.relations.GetFirstDirectRelationPawn(PawnRelationDefOf.Parent, null);
					map = ((firstDirectRelationPawn != null) ? firstDirectRelationPawn.MapHeld : null);
				}
				if (map == null)
				{
					return;
				}
				this.allowedAreas.SetOrAdd(map, value);
				if (this.pawn.Spawned && !this.pawn.Drafted && value != null && value == this.EffectiveAreaRestrictionInPawnCurrentMap && value.TrueCount > 0)
				{
					Pawn_JobTracker jobs = this.pawn.jobs;
					if (((jobs != null) ? jobs.curJob : null) != null && this.pawn.jobs.curJob.AnyTargetOutsideArea(value))
					{
						this.pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
				}
			}
		}

		// Token: 0x170017B3 RID: 6067
		// (get) Token: 0x0600898E RID: 35214 RVA: 0x0031D0F2 File Offset: 0x0031B2F2
		public bool RespectsAllowedArea
		{
			get
			{
				return this.SupportsAllowedAreas && this.pawn.GetLord() == null && this.pawn.Faction == Faction.OfPlayer && this.pawn.HostFaction == null;
			}
		}

		// Token: 0x170017B4 RID: 6068
		// (get) Token: 0x0600898F RID: 35215 RVA: 0x0031D12F File Offset: 0x0031B32F
		public bool SupportsAllowedAreas
		{
			get
			{
				return !this.pawn.Roamer && !this.pawn.RaceProps.disableAreaControl;
			}
		}

		// Token: 0x170017B5 RID: 6069
		// (get) Token: 0x06008990 RID: 35216 RVA: 0x0031D153 File Offset: 0x0031B353
		public bool RespectsMaster
		{
			get
			{
				return this.Master != null && this.pawn.Faction == Faction.OfPlayer && this.Master.Faction == this.pawn.Faction;
			}
		}

		// Token: 0x170017B6 RID: 6070
		// (get) Token: 0x06008991 RID: 35217 RVA: 0x0031D18B File Offset: 0x0031B38B
		public Pawn RespectedMaster
		{
			get
			{
				if (!this.RespectsMaster)
				{
					return null;
				}
				return this.Master;
			}
		}

		// Token: 0x170017B7 RID: 6071
		// (get) Token: 0x06008992 RID: 35218 RVA: 0x0031D1A0 File Offset: 0x0031B3A0
		public bool UsesConfigurableHostilityResponse
		{
			get
			{
				return (this.pawn.IsColonist || (this.pawn.IsColonySubhuman && !this.pawn.mutant.Def.disableHostilityResponse)) && this.pawn.HostFaction == null;
			}
		}

		// Token: 0x06008993 RID: 35219 RVA: 0x0031D1F0 File Offset: 0x0031B3F0
		public Pawn_PlayerSettings(Pawn pawn)
		{
			this.pawn = pawn;
			if (Current.ProgramState == ProgramState.Playing)
			{
				this.joinTick = Find.TickManager.TicksGame;
			}
			else
			{
				this.joinTick = 0;
			}
			this.Notify_FactionChanged();
		}

		// Token: 0x06008994 RID: 35220 RVA: 0x0031D264 File Offset: 0x0031B464
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.joinTick, "joinTick", 0, false);
			Scribe_Values.Look<bool>(ref this.animalsReleased, "animalsReleased", false, false);
			Scribe_Values.Look<MedicalCareCategory>(ref this.medCare, "medCare", MedicalCareCategory.NoCare, false);
			Scribe_Collections.Look<Map, Area>(ref this.allowedAreas, "allowedAreas", LookMode.Reference, LookMode.Reference, ref this.allowedAreasKeys, ref this.allowedAreasValues, true, false, false);
			Scribe_References.Look<Pawn>(ref this.master, "master", false);
			Scribe_Values.Look<bool>(ref this.followDrafted, "followDrafted", false, false);
			Scribe_Values.Look<bool>(ref this.followFieldwork, "followFieldwork", false, false);
			Scribe_Values.Look<HostilityResponseMode>(ref this.hostilityResponse, "hostilityResponse", HostilityResponseMode.Flee, false);
			Scribe_Values.Look<bool>(ref this.selfTend, "selfTend", false, false);
			Scribe_Values.Look<int>(ref this.displayOrder, "displayOrder", 0, false);
			Scribe_Values.Look<bool>(ref this.animalForage, "animalForage", true, false);
			Scribe_Values.Look<bool>(ref this.animalDig, "animalDig", true, false);
			if (Scribe.mode == LoadSaveMode.Saving && this.allowedAreas != null)
			{
				this.allowedAreas.RemoveAll((KeyValuePair<Map, Area> kvp) => kvp.Key == null || kvp.Value == null);
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.pawn.Roamer)
			{
				this.allowedAreas.Clear();
			}
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs && this.allowedAreas == null)
			{
				this.allowedAreas = new Dictionary<Map, Area>();
			}
			if (Scribe.mode == LoadSaveMode.LoadingVars || Scribe.mode == LoadSaveMode.ResolvingCrossRefs || Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				Area area = null;
				Scribe_References.Look<Area>(ref area, "areaAllowed", false);
				if (area != null && Find.AnyPlayerHomeMap != null)
				{
					this.allowedAreas.Add(Find.AnyPlayerHomeMap, area);
				}
			}
		}

		// Token: 0x06008995 RID: 35221 RVA: 0x0031D40F File Offset: 0x0031B60F
		public IEnumerable<Gizmo> GetGizmos()
		{
			Pawn_PlayerSettings.<GetGizmos>d__36 <GetGizmos>d__ = new Pawn_PlayerSettings.<GetGizmos>d__36(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x06008996 RID: 35222 RVA: 0x0031D41F File Offset: 0x0031B61F
		public void Notify_FactionChanged()
		{
			this.ResetMedicalCare();
			this.allowedAreas.Clear();
		}

		// Token: 0x06008997 RID: 35223 RVA: 0x0031D434 File Offset: 0x0031B634
		public void ResetMedicalCare()
		{
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				return;
			}
			if (this.pawn.Faction == Faction.OfPlayer)
			{
				if (ModsConfig.AnomalyActive && this.pawn.IsGhoul)
				{
					this.medCare = Find.PlaySettings.defaultCareForGhouls;
					return;
				}
				if (ModsConfig.AnomalyActive && this.pawn.IsEntity)
				{
					this.medCare = Find.PlaySettings.defaultCareForEntities;
					return;
				}
				if (!this.pawn.RaceProps.Animal)
				{
					this.medCare = (this.pawn.IsSlave ? Find.PlaySettings.defaultCareForSlave : Find.PlaySettings.defaultCareForColonist);
					return;
				}
				this.medCare = Find.PlaySettings.defaultCareForTamedAnimal;
				return;
			}
			else
			{
				if (ModsConfig.AnomalyActive && this.pawn.IsEntity)
				{
					this.medCare = Find.PlaySettings.defaultCareForEntities;
					return;
				}
				if (this.pawn.IsPrisoner)
				{
					this.medCare = Find.PlaySettings.defaultCareForPrisoner;
					return;
				}
				if (this.pawn.Faction == null && this.pawn.RaceProps.Animal)
				{
					this.medCare = Find.PlaySettings.defaultCareForWildlife;
					return;
				}
				if (this.pawn.Faction != null)
				{
					FactionRelationKind kind = this.pawn.Faction.RelationWith(Faction.OfPlayer, false).kind;
					if (kind == FactionRelationKind.Ally)
					{
						this.medCare = Find.PlaySettings.defaultCareForFriendlyFaction;
						return;
					}
					if (kind == FactionRelationKind.Neutral)
					{
						this.medCare = Find.PlaySettings.defaultCareForNeutralFaction;
						return;
					}
					if (kind == FactionRelationKind.Hostile)
					{
						this.medCare = Find.PlaySettings.defaultCareForHostileFaction;
						return;
					}
				}
				else
				{
					if (!this.pawn.Faction.HostileTo(Faction.OfPlayer))
					{
						this.medCare = Find.PlaySettings.defaultCareForNoFaction;
						return;
					}
					this.medCare = Find.PlaySettings.defaultCareForHostileFaction;
				}
				return;
			}
		}

		// Token: 0x06008998 RID: 35224 RVA: 0x0031D608 File Offset: 0x0031B808
		public void Notify_AreaRemoved(Area area)
		{
			foreach (Map key in this.allowedAreas.Keys.ToList<Map>())
			{
				if (this.allowedAreas[key] == area)
				{
					this.allowedAreas.Remove(key);
				}
			}
		}

		// Token: 0x06008999 RID: 35225 RVA: 0x0031D67C File Offset: 0x0031B87C
		public void Notify_MapRemoved(Map map)
		{
			if (this.allowedAreas.ContainsKey(map))
			{
				this.allowedAreas.Remove(map);
			}
		}

		// Token: 0x040054B5 RID: 21685
		public const int UnsetDisplayOrder = -9999999;

		// Token: 0x040054B6 RID: 21686
		private Pawn pawn;

		// Token: 0x040054B7 RID: 21687
		private Dictionary<Map, Area> allowedAreas = new Dictionary<Map, Area>();

		// Token: 0x040054B8 RID: 21688
		public int joinTick;

		// Token: 0x040054B9 RID: 21689
		private Pawn master;

		// Token: 0x040054BA RID: 21690
		public bool followDrafted;

		// Token: 0x040054BB RID: 21691
		public bool followFieldwork;

		// Token: 0x040054BC RID: 21692
		public bool animalsReleased;

		// Token: 0x040054BD RID: 21693
		public MedicalCareCategory medCare = MedicalCareCategory.NoMeds;

		// Token: 0x040054BE RID: 21694
		public HostilityResponseMode hostilityResponse = HostilityResponseMode.Flee;

		// Token: 0x040054BF RID: 21695
		public bool selfTend;

		// Token: 0x040054C0 RID: 21696
		public int displayOrder = -9999999;

		// Token: 0x040054C1 RID: 21697
		public bool animalForage = true;

		// Token: 0x040054C2 RID: 21698
		public bool animalDig = true;

		// Token: 0x040054C3 RID: 21699
		private List<Map> allowedAreasKeys;

		// Token: 0x040054C4 RID: 21700
		private List<Area> allowedAreasValues;
	}
}
