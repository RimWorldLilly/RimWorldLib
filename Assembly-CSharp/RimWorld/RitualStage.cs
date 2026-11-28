using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001678 RID: 5752
	public class RitualStage : IExposable
	{
		// Token: 0x06009042 RID: 36930 RVA: 0x0034F72C File Offset: 0x0034D92C
		public RitualRoleBehavior BehaviorForRole(string roleId)
		{
			if (roleId == null)
			{
				return null;
			}
			RitualRoleBehavior result;
			if (!this.behaviorForRole.TryGetValue(roleId, out result) && this.roleBehaviors != null)
			{
				for (int i = 0; i < this.roleBehaviors.Count; i++)
				{
					RitualRoleBehavior ritualRoleBehavior = this.roleBehaviors[i];
					if (ritualRoleBehavior.roleId == roleId)
					{
						this.behaviorForRole[roleId] = ritualRoleBehavior;
						result = ritualRoleBehavior;
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x06009043 RID: 36931 RVA: 0x0034F79C File Offset: 0x0034D99C
		public DutyDef GetDuty(Pawn pawn, RitualRole forcedRole = null, LordJob_Ritual ritual = null)
		{
			if (this.parent.roles != null)
			{
				if (forcedRole != null)
				{
					RitualRoleBehavior ritualRoleBehavior = this.BehaviorForRole(forcedRole.id);
					if (ritualRoleBehavior == null)
					{
						return null;
					}
					return ritualRoleBehavior.dutyDef;
				}
				else
				{
					RitualRole ritualRole = ritual.assignments.RoleForPawn(pawn, true);
					if (ritualRole != null)
					{
						RitualRoleBehavior ritualRoleBehavior2 = this.BehaviorForRole(ritualRole.id);
						if (ritualRoleBehavior2 != null)
						{
							return ritualRoleBehavior2.dutyDef;
						}
					}
				}
			}
			return this.defaultDuty;
		}

		// Token: 0x06009044 RID: 36932 RVA: 0x0034F800 File Offset: 0x0034DA00
		public bool HasRole(Pawn p)
		{
			List<RitualRole> roles = this.parent.roles;
			for (int i = 0; i < roles.Count; i++)
			{
				string text;
				if (roles[i].AppliesToPawn(p, out text, TargetInfo.Invalid, null, null, null, true))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009045 RID: 36933 RVA: 0x0034F848 File Offset: 0x0034DA48
		public bool Applies(Precept_Ritual ritual, List<Pawn> participants)
		{
			foreach (RitualRole ritualRole in this.parent.RequiredRoles())
			{
				bool flag = false;
				Precept_Role precept_Role = (Precept_Role)Faction.OfPlayer.ideos.GetPrecept(ritualRole.precept);
				if (precept_Role == null)
				{
					return false;
				}
				foreach (Pawn p in participants)
				{
					if (precept_Role.Active && precept_Role.IsAssigned(p))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06009046 RID: 36934 RVA: 0x0034F914 File Offset: 0x0034DB14
		public PawnStagePosition GetPawnPosition(IntVec3 spot, Pawn pawn, LordJob_Ritual ritual, RitualRole forcedRole = null)
		{
			if (this.parent.roles != null)
			{
				if (forcedRole != null)
				{
					RitualRoleBehavior ritualRoleBehavior = this.BehaviorForRole(forcedRole.id);
					RitualPosition ritualPosition = (ritualRoleBehavior != null) ? ritualRoleBehavior.GetPosition(spot, pawn, ritual) : null;
					if (ritualPosition != null)
					{
						return ritualPosition.GetCell(spot, pawn, ritual);
					}
				}
				RitualRole ritualRole = ritual.assignments.RoleForPawn(pawn, true);
				if (ritualRole != null)
				{
					RitualRoleBehavior ritualRoleBehavior2 = this.BehaviorForRole(ritualRole.id);
					if (ritualRoleBehavior2 != null)
					{
						RitualPosition position = ritualRoleBehavior2.GetPosition(spot, pawn, ritual);
						if (position != null)
						{
							return position.GetCell(spot, pawn, ritual);
						}
					}
				}
			}
			return new PawnStagePosition(spot, null, Rot4.Invalid, false);
		}

		// Token: 0x06009047 RID: 36935 RVA: 0x0034F9A1 File Offset: 0x0034DBA1
		public virtual float ProgressPerTick(LordJob_Ritual ritual)
		{
			return 1f;
		}

		// Token: 0x06009048 RID: 36936 RVA: 0x0034F9A8 File Offset: 0x0034DBA8
		public virtual TargetInfo GetSecondFocus(LordJob_Ritual ritual)
		{
			return TargetInfo.Invalid;
		}

		// Token: 0x06009049 RID: 36937 RVA: 0x0034F9AF File Offset: 0x0034DBAF
		public virtual IEnumerable<RitualStagePawnSecondFocus> GetPawnSecondFoci(LordJob_Ritual ritual)
		{
			return null;
		}

		// Token: 0x0600904A RID: 36938 RVA: 0x0034F9B4 File Offset: 0x0034DBB4
		public virtual void ExposeData()
		{
			Scribe_Defs.Look<RitualBehaviorDef>(ref this.parent, "parent");
			Scribe_Defs.Look<DutyDef>(ref this.defaultDuty, "defaultDuty");
			Scribe_Collections.Look<StageEndTrigger>(ref this.endTriggers, "endTriggers", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.spectatorsRequired, "spectatorsRequired", false, false);
			Scribe_Values.Look<SpectateRectSide>(ref this.allowedSpectateSidesOverride, "allowedSpectateSidesOverride", SpectateRectSide.None, false);
			Scribe_Values.Look<IntRange>(ref this.spectateDistanceOverride, "spectateDistanceOverride", default(IntRange), false);
			Scribe_Values.Look<bool>(ref this.showProgressBar, "showProgressBar", false, false);
			Scribe_Collections.Look<StageFailTrigger>(ref this.failTriggers, "failTriggers", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<RitualRoleBehavior>(ref this.roleBehaviors, "roleBehaviors", LookMode.Deep, Array.Empty<object>());
			Scribe_Collections.Look<string>(ref this.highlightRolePositions, "highlightRolePositions", LookMode.Value, Array.Empty<object>());
			Scribe_Defs.Look<RitualVisualEffectDef>(ref this.visualEffectDef, "visualEffectDef");
			Scribe_Values.Look<bool>(ref this.ignoreDurationToFinishAfterStage, "ignoreDurationToFinishAfterStage", false, false);
			Scribe_Deep.Look<RitualStageAction>(ref this.postAction, "postAction", Array.Empty<object>());
			Scribe_Deep.Look<RitualStageAction>(ref this.preAction, "preAction", Array.Empty<object>());
			Scribe_Deep.Look<RitualStageAction>(ref this.interruptedAction, "interruptedAction", Array.Empty<object>());
			Scribe_Deep.Look<RitualStageAction>(ref this.pawnLeaveAction, "pawnLeaveAction", Array.Empty<object>());
		}

		// Token: 0x04005807 RID: 22535
		public RitualBehaviorDef parent;

		// Token: 0x04005808 RID: 22536
		public DutyDef defaultDuty;

		// Token: 0x04005809 RID: 22537
		public List<StageEndTrigger> endTriggers;

		// Token: 0x0400580A RID: 22538
		public List<StageFailTrigger> failTriggers;

		// Token: 0x0400580B RID: 22539
		public RitualStageAction preAction;

		// Token: 0x0400580C RID: 22540
		public RitualStageAction postAction;

		// Token: 0x0400580D RID: 22541
		public RitualStageAction interruptedAction;

		// Token: 0x0400580E RID: 22542
		public RitualStageAction pawnLeaveAction;

		// Token: 0x0400580F RID: 22543
		[NoTranslate]
		public List<string> highlightRolePositions = new List<string>();

		// Token: 0x04005810 RID: 22544
		[NoTranslate]
		public List<string> highlightRolePawns = new List<string>();

		// Token: 0x04005811 RID: 22545
		public RitualStageTickActionMaker tickActionMaker;

		// Token: 0x04005812 RID: 22546
		public RitualVisualEffectDef visualEffectDef;

		// Token: 0x04005813 RID: 22547
		public bool spectatorsRequired;

		// Token: 0x04005814 RID: 22548
		public bool essential;

		// Token: 0x04005815 RID: 22549
		public bool ignoreDurationToFinishAfterStage;

		// Token: 0x04005816 RID: 22550
		public SpectateRectSide allowedSpectateSidesOverride;

		// Token: 0x04005817 RID: 22551
		public IntRange spectateDistanceOverride = IntRange.Zero;

		// Token: 0x04005818 RID: 22552
		public bool showProgressBar = true;

		// Token: 0x04005819 RID: 22553
		public List<RitualRoleBehavior> roleBehaviors;

		// Token: 0x0400581A RID: 22554
		private Dictionary<string, RitualRoleBehavior> behaviorForRole = new Dictionary<string, RitualRoleBehavior>();
	}
}
