using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.AI.Group;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001680 RID: 5760
	public class RitualBehaviorWorker : IExposable
	{
		// Token: 0x06009064 RID: 36964 RVA: 0x0034FF85 File Offset: 0x0034E185
		public RitualBehaviorWorker()
		{
		}

		// Token: 0x06009065 RID: 36965 RVA: 0x0034FF8D File Offset: 0x0034E18D
		public RitualBehaviorWorker(RitualBehaviorDef def)
		{
			this.def = def;
		}

		// Token: 0x170018F4 RID: 6388
		// (get) Token: 0x06009066 RID: 36966 RVA: 0x0034FF9C File Offset: 0x0034E19C
		public virtual string descriptionOverride
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170018F5 RID: 6389
		// (get) Token: 0x06009067 RID: 36967 RVA: 0x0034FF9F File Offset: 0x0034E19F
		public virtual Sustainer SoundPlaying
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170018F6 RID: 6390
		// (get) Token: 0x06009068 RID: 36968 RVA: 0x0034FFA2 File Offset: 0x0034E1A2
		public virtual bool ChecksReservations
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06009069 RID: 36969 RVA: 0x0034FFA8 File Offset: 0x0034E1A8
		public virtual string CanStartRitualNow(TargetInfo target, Precept_Ritual ritual, Pawn selectedPawn = null, Dictionary<string, Pawn> forcedForRole = null)
		{
			if (target.IsValid && target.Map.Tile.Valid)
			{
				PlanetLayerDef layerDef = target.Map.Tile.LayerDef;
				if ((!ritual.layerWhitelist.NullOrEmpty<PlanetLayerDef>() && !ritual.layerWhitelist.Contains(layerDef)) || (!ritual.layerBlacklist.NullOrEmpty<PlanetLayerDef>() && ritual.layerBlacklist.Contains(layerDef)))
				{
					return "CantStartRitualLayer".Translate(ritual.Label.Named("RITUAL"), layerDef.gerundLabel.Named("GERUND"), layerDef.LabelCap.Named("LAYER")).CapitalizeFirst();
				}
			}
			if (!ritual.allowOtherInstances)
			{
				using (List<LordJob_Ritual>.Enumerator enumerator = Find.IdeoManager.GetActiveRituals(target.Map).GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.Ritual == ritual)
						{
							return "CantStartRitualAlreadyInProgress".Translate(ritual.Label).CapitalizeFirst();
						}
					}
				}
			}
			if (selectedPawn != null)
			{
				RitualBehaviorWorker behavior = ritual.behavior;
				if (((behavior != null) ? behavior.def.roles : null) != null)
				{
					foreach (RitualRole ritualRole in ritual.behavior.def.roles)
					{
						string text;
						if (ritualRole.defaultForSelectedColonist && !ritualRole.AppliesToPawn(selectedPawn, out text, target, null, null, ritual, false))
						{
							if (text.NullOrEmpty())
							{
								return "CantStartRitualSelectedPawnCannotBeRole".Translate(selectedPawn.Named("PAWN"), ritualRole.Label.Named("ROLE")).CapitalizeFirst();
							}
							return text;
						}
					}
				}
			}
			if (target.IsValid)
			{
				List<Pawn> list = target.Map.mapPawns.FreeColonistsAndPrisonersSpawned.ToList<Pawn>();
				list.AddRange(target.Map.mapPawns.SpawnedColonyAnimals);
				if (!ritual.behavior.def.roles.NullOrEmpty<RitualRole>())
				{
					using (List<RitualRole>.Enumerator enumerator2 = ritual.behavior.def.roles.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							RitualRole role = enumerator2.Current;
							if (role.required && !role.substitutable)
							{
								IEnumerable<RitualRole> source = (role.mergeId == null) ? Gen.YieldSingle<RitualRole>(role) : (from r in ritual.behavior.def.roles
								where r.mergeId == role.mergeId
								select r);
								if (list.Count(delegate(Pawn p)
								{
									string text2;
									return role.AppliesToPawn(p, out text2, target, null, null, null, true);
								}) < source.Count<RitualRole>() && (forcedForRole == null || !forcedForRole.ContainsKey(role.id)))
								{
									Precept precept = ritual.ideo.PreceptsListForReading.FirstOrDefault((Precept p) => p.def == role.precept);
									if (precept != null)
									{
										return "MessageNeedAssignedRoleToBeginRitual".Translate(role.missingDesc ?? Find.ActiveLanguageWorker.WithIndefiniteArticle(precept.LabelCap, false, false), ritual.Label);
									}
									if (!role.noCandidatesGizmoDesc.NullOrEmpty())
									{
										return role.noCandidatesGizmoDesc;
									}
									if (source.Count<RitualRole>() == 1)
									{
										return "MessageNoRequiredRolePawnToBeginRitual".Translate(role.missingDesc ?? Find.ActiveLanguageWorker.WithIndefiniteArticle(role.Label, false, false), ritual.Label);
									}
									return "MessageNoRequiredRolePawnToBeginRitual".Translate(source.Count<RitualRole>().ToString() + " " + (role.missingDesc ?? Find.ActiveLanguageWorker.Pluralize(role.Label, -1)), ritual.Label);
								}
							}
						}
					}
				}
			}
			return null;
		}

		// Token: 0x0600906A RID: 36970 RVA: 0x003504D0 File Offset: 0x0034E6D0
		public virtual string GetExplanation(Precept_Ritual ritual, RitualRoleAssignments assignments, float quality)
		{
			return null;
		}

		// Token: 0x0600906B RID: 36971 RVA: 0x003504D3 File Offset: 0x0034E6D3
		public virtual bool CanExecuteOn(TargetInfo target, RitualObligation obligation)
		{
			return obligation == null || obligation.precept.obligationTargetFilter.CanUseTarget(target, obligation).canUse;
		}

		// Token: 0x0600906C RID: 36972 RVA: 0x003504F4 File Offset: 0x0034E6F4
		public virtual void TryExecuteOn(TargetInfo target, Pawn organizer, Precept_Ritual ritual, RitualObligation obligation, RitualRoleAssignments assignments, bool playerForced = false)
		{
			if (this.CanStartRitualNow(target, ritual, null, assignments.ForcedRolesForReading) != null)
			{
				return;
			}
			if (!this.CanExecuteOn(target, obligation))
			{
				return;
			}
			if (playerForced)
			{
				foreach (Pawn pawn in assignments.Participants)
				{
					RitualRole ritualRole = assignments.RoleForPawn(pawn, true);
					if (ritualRole == null || !ritualRole.allowKeepLayingDown || pawn.GetPosture() != PawnPosture.LayingInBed)
					{
						pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, false, true);
					}
				}
			}
			foreach (Pawn pawn2 in assignments.Participants)
			{
				Lord lord = pawn2.GetLord();
				if (((lord != null) ? lord.LordJob : null) is LordJob_VoluntarilyJoinable)
				{
					pawn2.GetLord().Notify_PawnLost(pawn2, PawnLostCondition.LeftVoluntarily, null);
				}
			}
			LordJob_Ritual lordJob = (LordJob_Ritual)this.CreateLordJob(target, organizer, ritual, obligation, assignments);
			LordMaker.MakeNewLord(Faction.OfPlayer, lordJob, target.Map, assignments.Participants.Where(delegate(Pawn p)
			{
				RitualRole ritualRole2 = lordJob.RoleFor(p, false);
				return ritualRole2 == null || ritualRole2.addToLord;
			}));
			RitualOutcomeEffectWorker outcomeEffect = ritual.outcomeEffect;
			if (outcomeEffect != null)
			{
				outcomeEffect.ResetCompDatas();
			}
			lordJob.PreparePawns();
			AbilityGroupDef useCooldownFromAbilityGroupDef = ritual.def.useCooldownFromAbilityGroupDef;
			if (useCooldownFromAbilityGroupDef != null && useCooldownFromAbilityGroupDef.cooldownTicks > 0 && !useCooldownFromAbilityGroupDef.ritualRoleIds.NullOrEmpty<string>())
			{
				foreach (string roleId in useCooldownFromAbilityGroupDef.ritualRoleIds)
				{
					if (assignments.AnyPawnAssigned(roleId))
					{
						foreach (Pawn pawn3 in assignments.AssignedPawns(roleId))
						{
							foreach (Ability ability in pawn3.abilities.AllAbilitiesForReading)
							{
								ability.Notify_GroupStartedCooldown(useCooldownFromAbilityGroupDef, useCooldownFromAbilityGroupDef.cooldownTicks);
							}
						}
					}
				}
				ritual.Notify_CooldownFromAbilityStarted(useCooldownFromAbilityGroupDef.cooldownTicks);
			}
			this.PostExecute(target, organizer, ritual, obligation, assignments);
		}

		// Token: 0x0600906D RID: 36973 RVA: 0x0035078C File Offset: 0x0034E98C
		protected virtual void PostExecute(TargetInfo target, Pawn organizer, Precept_Ritual ritual, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			Messages.Message("RitualBegun".Translate(ritual.Label).CapitalizeFirst(), target, MessageTypeDefOf.NeutralEvent, true);
		}

		// Token: 0x0600906E RID: 36974 RVA: 0x003507CC File Offset: 0x0034E9CC
		protected virtual LordJob CreateLordJob(TargetInfo target, Pawn organizer, Precept_Ritual ritual, RitualObligation obligation, RitualRoleAssignments assignments)
		{
			return new LordJob_Ritual(target, ritual, obligation, this.def.stages, assignments, null, null);
		}

		// Token: 0x0600906F RID: 36975 RVA: 0x003507F9 File Offset: 0x0034E9F9
		public virtual void Tick(LordJob_Ritual ritual)
		{
		}

		// Token: 0x06009070 RID: 36976 RVA: 0x003507FC File Offset: 0x0034E9FC
		public bool SpectatorsRequired()
		{
			if (this.def.stages.NullOrEmpty<RitualStage>())
			{
				return true;
			}
			for (int i = 0; i < this.def.stages.Count; i++)
			{
				if (this.def.stages[i].spectatorsRequired)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009071 RID: 36977 RVA: 0x00350854 File Offset: 0x0034EA54
		public bool HasAnyRole(Pawn p)
		{
			if (this.def.stages.NullOrEmpty<RitualStage>())
			{
				return false;
			}
			for (int i = 0; i < this.def.stages.Count; i++)
			{
				if (this.def.stages[i].HasRole(p))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009072 RID: 36978 RVA: 0x003508AC File Offset: 0x0034EAAC
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<RitualBehaviorDef>(ref this.def, "def");
		}

		// Token: 0x06009073 RID: 36979 RVA: 0x003508BE File Offset: 0x0034EABE
		public virtual void Cleanup(LordJob_Ritual ritual)
		{
		}

		// Token: 0x06009074 RID: 36980 RVA: 0x003508C0 File Offset: 0x0034EAC0
		public virtual void PostCleanup(LordJob_Ritual ritual)
		{
		}

		// Token: 0x06009075 RID: 36981 RVA: 0x003508C2 File Offset: 0x0034EAC2
		public virtual string ExpectedDuration(Precept_Ritual ritual, RitualRoleAssignments assignments, float quality)
		{
			return this.def.durationTicks.max.ToStringTicksToPeriod(false, false, true, true, false);
		}

		// Token: 0x06009076 RID: 36982 RVA: 0x003508DE File Offset: 0x0034EADE
		public virtual bool ShouldInitAsSpectator(Pawn pawn, RitualRoleAssignments assignments)
		{
			return true;
		}

		// Token: 0x06009077 RID: 36983 RVA: 0x003508E1 File Offset: 0x0034EAE1
		public virtual void DrawPreviewOnTarget(TargetInfo targetInfo)
		{
		}

		// Token: 0x06009078 RID: 36984 RVA: 0x003508E3 File Offset: 0x0034EAE3
		public virtual bool TargetStillAllowed(TargetInfo selectedTarget, LordJob_Ritual ritual)
		{
			return true;
		}

		// Token: 0x06009079 RID: 36985 RVA: 0x003508E6 File Offset: 0x0034EAE6
		public virtual bool PawnCanFillRole(Pawn pawn, RitualRole role, out string s, TargetInfo ritualTarget)
		{
			s = null;
			return true;
		}

		// Token: 0x0400582F RID: 22575
		public RitualBehaviorDef def;
	}
}
