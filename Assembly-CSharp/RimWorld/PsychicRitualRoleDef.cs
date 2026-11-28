using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CA6 RID: 3238
	public class PsychicRitualRoleDef : Def, ILordJobRole
	{
		// Token: 0x17000FED RID: 4077
		// (get) Token: 0x06005BA2 RID: 23458 RVA: 0x0021A736 File Offset: 0x00218936
		public virtual int MaxCount
		{
			get
			{
				return this.allowedCount.max;
			}
		}

		// Token: 0x17000FEE RID: 4078
		// (get) Token: 0x06005BA3 RID: 23459 RVA: 0x0021A743 File Offset: 0x00218943
		public virtual int MinCount
		{
			get
			{
				return this.allowedCount.min;
			}
		}

		// Token: 0x17000FEF RID: 4079
		// (get) Token: 0x06005BA4 RID: 23460 RVA: 0x0021A750 File Offset: 0x00218950
		public TaggedString Label
		{
			get
			{
				return this.label;
			}
		}

		// Token: 0x17000FF0 RID: 4080
		// (get) Token: 0x06005BA5 RID: 23461 RVA: 0x0021A75D File Offset: 0x0021895D
		public TaggedString CategoryLabel
		{
			get
			{
				return this.Label;
			}
		}

		// Token: 0x17000FF1 RID: 4081
		// (get) Token: 0x06005BA6 RID: 23462 RVA: 0x0021A765 File Offset: 0x00218965
		public TaggedString CategoryLabelCap
		{
			get
			{
				return this.LabelCap;
			}
		}

		// Token: 0x17000FF2 RID: 4082
		// (get) Token: 0x06005BA7 RID: 23463 RVA: 0x0021A76D File Offset: 0x0021896D
		public virtual PsychicRitualRoleDef.Condition AllowedConditions
		{
			get
			{
				return this.allowedConditions;
			}
		}

		// Token: 0x17000FF3 RID: 4083
		// (get) Token: 0x06005BA8 RID: 23464 RVA: 0x0021A775 File Offset: 0x00218975
		public virtual bool CanHandleOfferings
		{
			get
			{
				return this.canHandleOfferings;
			}
		}

		// Token: 0x06005BA9 RID: 23465 RVA: 0x0021A780 File Offset: 0x00218980
		public virtual bool ConditionAllowed(PawnLostCondition condition)
		{
			if (condition == PawnLostCondition.LordRejected)
			{
				return false;
			}
			PsychicRitualRoleDef.Condition condition2;
			if (!PsychicRitualRoleDef.PawnLostConditionToCondition.TryGetValue(condition, out condition2))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unknown PawnLostCondition ");
				defaultInterpolatedStringHandler.AppendFormatted<PawnLostCondition>(condition);
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return condition2 == PsychicRitualRoleDef.Condition.None || this.ConditionAllowed(condition2);
		}

		// Token: 0x06005BAA RID: 23466 RVA: 0x0021A7DA File Offset: 0x002189DA
		public virtual bool ConditionAllowed(PsychicRitualRoleDef.Condition condition)
		{
			return (this.AllowedConditions & condition) > PsychicRitualRoleDef.Condition.None;
		}

		// Token: 0x06005BAB RID: 23467 RVA: 0x0021A7E8 File Offset: 0x002189E8
		public static TaggedString ConditionToReason(Pawn pawn, PsychicRitualRoleDef.Condition disqualifier)
		{
			if (disqualifier > PsychicRitualRoleDef.Condition.NonPlayerFaction)
			{
				if (disqualifier <= PsychicRitualRoleDef.Condition.Child)
				{
					if (disqualifier <= PsychicRitualRoleDef.Condition.LeftVoluntarily)
					{
						if (disqualifier == PsychicRitualRoleDef.Condition.NotOnMap)
						{
							return "PsychicRitualLeaveReason_ExitedMap".Translate(pawn.Named("PAWN"));
						}
						if (disqualifier == PsychicRitualRoleDef.Condition.HasAnotherLord)
						{
							return "PsychicRitualLeaveReason_PawnBusyWithLord".Translate(pawn.Named("PAWN"));
						}
						if (disqualifier != PsychicRitualRoleDef.Condition.LeftVoluntarily)
						{
							goto IL_41D;
						}
						return "PsychicRitualLeaveReason_LeftVoluntarily".Translate(pawn.Named("PAWN"));
					}
					else
					{
						if (disqualifier == PsychicRitualRoleDef.Condition.Vanished)
						{
							return "PsychicRitualLeaveReason_Vanished".Translate(pawn.Named("PAWN"));
						}
						if (disqualifier != PsychicRitualRoleDef.Condition.Baby)
						{
							if (disqualifier != PsychicRitualRoleDef.Condition.Child)
							{
								goto IL_41D;
							}
							goto IL_35F;
							for (;;)
							{
								IL_35F:
								goto IL_35F;
							}
						}
					}
				}
				else if (disqualifier <= PsychicRitualRoleDef.Condition.SubHuman)
				{
					if (disqualifier != PsychicRitualRoleDef.Condition.Adult)
					{
						if (disqualifier == PsychicRitualRoleDef.Condition.NoPath)
						{
							return "PsychicRitualLeaveReason_CannotReachTarget".Translate(pawn.Named("PAWN"));
						}
						if (disqualifier != PsychicRitualRoleDef.Condition.SubHuman)
						{
							goto IL_41D;
						}
						return "PsychicRitualLeaveReason_Mutant".Translate(pawn.Named("PAWN"));
					}
				}
				else if (disqualifier <= PsychicRitualRoleDef.Condition.NoPsychicSensitivity)
				{
					if (disqualifier == PsychicRitualRoleDef.Condition.IdeoUnwilling)
					{
						return "PsychicRitualLeaveReason_IdeoUnwilling".Translate(pawn.Named("PAWN"), pawn.Ideo.Named("IDEO"));
					}
					if (disqualifier != PsychicRitualRoleDef.Condition.NoPsychicSensitivity)
					{
						goto IL_41D;
					}
					return "PsychicRitualLeaveReason_NoPsychicSensitivity".Translate(pawn.Named("PAWN"));
				}
				else
				{
					if (disqualifier == PsychicRitualRoleDef.Condition.TemporaryFactionMember)
					{
						return "PsychicRitualLeaveReason_TemporaryFactionMember".Translate(pawn.Named("PAWN"));
					}
					if (disqualifier == PsychicRitualRoleDef.Condition.Error)
					{
						goto IL_191;
					}
					goto IL_41D;
				}
				return "PsychicRitualLeaveReason_DevelopmentalStage".Translate(pawn.Named("PAWN"), pawn.DevelopmentalStage.ToString().Translate().Named("STAGE"));
			}
			if (disqualifier <= PsychicRitualRoleDef.Condition.Humanlike)
			{
				if (disqualifier <= PsychicRitualRoleDef.Condition.Burning)
				{
					switch (disqualifier)
					{
					case PsychicRitualRoleDef.Condition.None:
						break;
					case PsychicRitualRoleDef.Condition.Dead:
						return "PsychicRitualLeaveReason_Killed".Translate(pawn.Named("PAWN"));
					case PsychicRitualRoleDef.Condition.Downed:
						return "PsychicRitualLeaveReason_Incapped".Translate(pawn.Named("PAWN"));
					case PsychicRitualRoleDef.Condition.Dead | PsychicRitualRoleDef.Condition.Downed:
					case PsychicRitualRoleDef.Condition.Dead | PsychicRitualRoleDef.Condition.Drafted:
					case PsychicRitualRoleDef.Condition.Downed | PsychicRitualRoleDef.Condition.Drafted:
					case PsychicRitualRoleDef.Condition.Dead | PsychicRitualRoleDef.Condition.Downed | PsychicRitualRoleDef.Condition.Drafted:
						goto IL_41D;
					case PsychicRitualRoleDef.Condition.Drafted:
						return "PsychicRitualLeaveReason_Drafted".Translate(pawn.Named("PAWN"));
					case PsychicRitualRoleDef.Condition.Sleeping:
						return "PsychicRitualLeaveReason_Sleeping".Translate(pawn.Named("PAWN"));
					default:
						if (disqualifier == PsychicRitualRoleDef.Condition.Bleeding)
						{
							return "PsychicRitualLeaveReason_Bleeding".Translate(pawn.Named("PAWN"));
						}
						if (disqualifier != PsychicRitualRoleDef.Condition.Burning)
						{
							goto IL_41D;
						}
						return "PsychicRitualLeaveReason_Burning".Translate(pawn.Named("PAWN"));
					}
				}
				else
				{
					if (disqualifier == PsychicRitualRoleDef.Condition.MentalState)
					{
						return "PsychicRitualLeaveReason_InMentalState".Translate(pawn.Named("PAWN"));
					}
					if (disqualifier == PsychicRitualRoleDef.Condition.ExtremeTemperature)
					{
						return "PsychicRitualLeaveReason_ExtremeTemperature".Translate(pawn.Named("PAWN"));
					}
					if (disqualifier != PsychicRitualRoleDef.Condition.Humanlike)
					{
						goto IL_41D;
					}
					return "PsychicRitualLeaveReason_Humanlike".Translate(pawn.Named("PAWN"));
				}
			}
			else if (disqualifier <= PsychicRitualRoleDef.Condition.Slave)
			{
				if (disqualifier == PsychicRitualRoleDef.Condition.NonHumanlike)
				{
					return "PsychicRitualLeaveReason_NonHumanlike".Translate(pawn.Named("PAWN"));
				}
				if (disqualifier == PsychicRitualRoleDef.Condition.Prisoner)
				{
					return "PsychicRitualLeaveReason_Prisoner".Translate(pawn.Named("PAWN"));
				}
				if (disqualifier != PsychicRitualRoleDef.Condition.Slave)
				{
					goto IL_41D;
				}
				return "PsychicRitualLeaveReason_Slave".Translate(pawn.Named("PAWN"));
			}
			else
			{
				if (disqualifier == PsychicRitualRoleDef.Condition.Freeman)
				{
					return "PsychicRitualLeaveReason_Freeman".Translate(pawn.Named("PAWN"));
				}
				if (disqualifier == PsychicRitualRoleDef.Condition.InsideBuilding)
				{
					Building arg = (Building)pawn.SpawnedParentOrMe;
					return "PsychicRitualLeaveReason_InsideBuilding".Translate(pawn.Named("PAWN"), arg.Named("BUILDING"));
				}
				if (disqualifier != PsychicRitualRoleDef.Condition.NonPlayerFaction)
				{
					goto IL_41D;
				}
				return "PsychicRitualLeaveReason_WrongFaction".Translate(pawn.Named("PAWN"), Faction.OfPlayer.Named("FACTION"));
			}
			IL_191:
			return TaggedString.Empty;
			IL_41D:
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Unknown RoleDisqualifiersEnum ");
			defaultInterpolatedStringHandler.AppendFormatted<PsychicRitualRoleDef.Condition>(disqualifier);
			throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06005BAC RID: 23468 RVA: 0x0021AC3C File Offset: 0x00218E3C
		public static TaggedString PawnLostConditionToPsychicRitualReason(PsychicRitual psychicRitual, Pawn pawn, PawnLostCondition condition)
		{
			if (condition == PawnLostCondition.LordRejected)
			{
				throw new InvalidOperationException("Already informed player of pawn lost reason.");
			}
			PsychicRitualRoleDef.Condition disqualifier;
			if (!PsychicRitualRoleDef.PawnLostConditionToCondition.TryGetValue(condition, out disqualifier))
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(26, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unknown PawnLostCondition ");
				defaultInterpolatedStringHandler.AppendFormatted<PawnLostCondition>(condition);
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return PsychicRitualRoleDef.ConditionToReason(pawn, disqualifier);
		}

		// Token: 0x06005BAD RID: 23469 RVA: 0x0021AC9C File Offset: 0x00218E9C
		public bool PawnCanDo(PsychicRitualRoleDef.Context context, Pawn pawn, TargetInfo target, out PsychicRitualRoleDef.Reason reason)
		{
			AnyEnum reasonCode;
			if (!this.PawnCanDo(context, pawn, target, out reasonCode))
			{
				reason = new PsychicRitualRoleDef.Reason
				{
					context = context,
					def = this,
					pawn = pawn,
					reasonCode = reasonCode,
					target = target
				};
				return false;
			}
			reason = PsychicRitualRoleDef.Reason.None;
			return true;
		}

		// Token: 0x06005BAE RID: 23470 RVA: 0x0021AD00 File Offset: 0x00218F00
		protected virtual bool PawnCanDo(PsychicRitualRoleDef.Context context, Pawn pawn, TargetInfo target, out AnyEnum reason)
		{
			if (pawn == null)
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.Error);
				return false;
			}
			if (!this.ConditionAllowed(PsychicRitualRoleDef.Condition.Dead) && pawn.Dead)
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.Dead);
				return false;
			}
			if (ModsConfig.AnomalyActive && !this.ConditionAllowed(PsychicRitualRoleDef.Condition.SubHuman) && pawn.IsSubhuman)
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.SubHuman);
				return false;
			}
			if (!this.ConditionAllowed(PsychicRitualRoleDef.Condition.Humanlike) && pawn.RaceProps.Humanlike)
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.Humanlike);
				return false;
			}
			if (!this.ConditionAllowed(PsychicRitualRoleDef.Condition.NonHumanlike) && !pawn.RaceProps.Humanlike)
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.NonHumanlike);
				return false;
			}
			PsychicRitualRoleDef.Condition condition = PsychicRitualRoleDef.DevelopmentalStageToCondition[pawn.DevelopmentalStage];
			if (!this.ConditionAllowed(condition))
			{
				if (context == PsychicRitualRoleDef.Context.InvokePsychicRitualGizmo)
				{
					reason = AnyEnum.None;
					return false;
				}
				if (context != PsychicRitualRoleDef.Context.Runtime && context == PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual)
				{
					reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(condition);
					return false;
				}
			}
			if (context != PsychicRitualRoleDef.Context.NonPlayerFaction && context != PsychicRitualRoleDef.Context.Runtime && !pawn.IsPrisoner && !this.ConditionAllowed(PsychicRitualRoleDef.Condition.NonPlayerFaction) && !pawn.Faction.IsPlayerSafe())
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.NonPlayerFaction);
				return false;
			}
			if (!this.ConditionAllowed(PsychicRitualRoleDef.Condition.HasAnotherLord) && pawn.GetLord() != null && context != PsychicRitualRoleDef.Context.Runtime)
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.HasAnotherLord);
				return false;
			}
			if (!this.ConditionAllowed(PsychicRitualRoleDef.Condition.Slave) && pawn.IsSlave)
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.Slave);
				return false;
			}
			if (!this.ConditionAllowed(PsychicRitualRoleDef.Condition.Prisoner) && pawn.IsPrisoner)
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.Prisoner);
				return false;
			}
			if (!this.ConditionAllowed(PsychicRitualRoleDef.Condition.Freeman) && pawn.IsFreeman)
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.Freeman);
				return false;
			}
			if (!this.ConditionAllowed(PsychicRitualRoleDef.Condition.Downed) && pawn.Downed && (!pawn.health.capacities.CanBeAwake || pawn.Awake()))
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.Downed);
				return false;
			}
			if (!this.ConditionAllowed(PsychicRitualRoleDef.Condition.MentalState) && pawn.InMentalState)
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.MentalState);
				return false;
			}
			if (!this.ConditionAllowed(PsychicRitualRoleDef.Condition.Bleeding) && pawn.health.hediffSet.BleedRateTotal > 0f)
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.Bleeding);
				return false;
			}
			if (!this.ConditionAllowed(PsychicRitualRoleDef.Condition.Burning) && pawn.IsBurning())
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.Burning);
				return false;
			}
			if (!this.ConditionAllowed(PsychicRitualRoleDef.Condition.ExtremeTemperature) && target.IsValid)
			{
				Map map = target.Map;
				if (map != null && pawn.Faction == Faction.OfPlayer && !pawn.SafeTemperatureAtCell(target.Cell, map))
				{
					reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.ExtremeTemperature);
					return false;
				}
			}
			if (!this.ConditionAllowed(PsychicRitualRoleDef.Condition.InsideBuilding) && pawn.SpawnedParentOrMe is Building)
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.InsideBuilding);
				return false;
			}
			if (!this.ConditionAllowed(PsychicRitualRoleDef.Condition.Sleeping) && !pawn.Awake() && context != PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual)
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.Sleeping);
				return false;
			}
			if (!this.ConditionAllowed(PsychicRitualRoleDef.Condition.Drafted) && pawn.Drafted && context != PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual)
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.Drafted);
				return false;
			}
			if (!this.ConditionAllowed(PsychicRitualRoleDef.Condition.NoPath) && target.IsValid && context != PsychicRitualRoleDef.Context.Runtime && !this.CanReach(pawn, target) && !pawn.IsPrisoner)
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.NoPath);
				return false;
			}
			if (!this.ConditionAllowed(PsychicRitualRoleDef.Condition.IdeoUnwilling) && context == PsychicRitualRoleDef.Context.Dialog_BeginPsychicRitual && pawn.Ideo != null && !pawn.Ideo.MemberWillingToDo(new HistoryEvent(HistoryEventDefOf.InvolvedInPsychicRitual, pawn.Named(HistoryEventArgsNames.Doer))))
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.IdeoUnwilling);
				return false;
			}
			if (!this.ConditionAllowed(PsychicRitualRoleDef.Condition.NoPsychicSensitivity) && pawn.GetStatValue(StatDefOf.PsychicSensitivity, true, -1) <= 0f)
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.NoPsychicSensitivity);
				return false;
			}
			if (!this.ConditionAllowed(PsychicRitualRoleDef.Condition.TemporaryFactionMember) && pawn.IsQuestLodger())
			{
				reason = AnyEnum.FromEnum<PsychicRitualRoleDef.Condition>(PsychicRitualRoleDef.Condition.TemporaryFactionMember);
				return false;
			}
			reason = AnyEnum.None;
			return true;
		}

		// Token: 0x06005BAF RID: 23471 RVA: 0x0021B15C File Offset: 0x0021935C
		public virtual TaggedString PawnCannotDoReason(AnyEnum reason, PsychicRitualRoleDef.Context context, Pawn pawn, TargetInfo target)
		{
			if (reason == AnyEnum.None)
			{
				return TaggedString.Empty;
			}
			PsychicRitualRoleDef.Condition? condition = reason.As<PsychicRitualRoleDef.Condition>();
			if (condition != null)
			{
				PsychicRitualRoleDef.Condition valueOrDefault = condition.GetValueOrDefault();
				return PsychicRitualRoleDef.ConditionToReason(pawn, valueOrDefault);
			}
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(75, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Unknown status reason type ");
			defaultInterpolatedStringHandler.AppendFormatted(reason.enumType.ToStringSafe<Type>());
			defaultInterpolatedStringHandler.AppendLiteral(".  ");
			defaultInterpolatedStringHandler.AppendLiteral("Did you forget to override ");
			defaultInterpolatedStringHandler.AppendFormatted("PawnCannotDoReason");
			defaultInterpolatedStringHandler.AppendLiteral(" in a child class?");
			throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x06005BB0 RID: 23472 RVA: 0x0021B203 File Offset: 0x00219403
		public IEnumerable<string> BlockingIssues(Pawn pawn, TargetInfo target)
		{
			PsychicRitualRoleDef.<BlockingIssues>d__33 <BlockingIssues>d__ = new PsychicRitualRoleDef.<BlockingIssues>d__33(-2);
			<BlockingIssues>d__.<>4__this = this;
			<BlockingIssues>d__.<>3__pawn = pawn;
			<BlockingIssues>d__.<>3__target = target;
			return <BlockingIssues>d__;
		}

		// Token: 0x06005BB1 RID: 23473 RVA: 0x0021B221 File Offset: 0x00219421
		public virtual IEnumerable<Gizmo> GetPawnGizmos(PsychicRitual psychicRitual, Pawn pawn)
		{
			PsychicRitualRoleDef.<GetPawnGizmos>d__34 <GetPawnGizmos>d__ = new PsychicRitualRoleDef.<GetPawnGizmos>d__34(-2);
			<GetPawnGizmos>d__.<>3__psychicRitual = psychicRitual;
			<GetPawnGizmos>d__.<>3__pawn = pawn;
			return <GetPawnGizmos>d__;
		}

		// Token: 0x06005BB2 RID: 23474 RVA: 0x0021B238 File Offset: 0x00219438
		public virtual bool CanReach(Pawn pawn, TargetInfo target)
		{
			return pawn.CanReachNonLocal(target, PathEndMode.OnCell, Danger.Deadly, false, TraverseMode.ByPawn);
		}

		// Token: 0x06005BB3 RID: 23475 RVA: 0x0021B245 File Offset: 0x00219445
		public virtual void Notify_PawnJobDone(PsychicRitual psychicRitual, Pawn pawn, JobCondition condition)
		{
			if (condition == JobCondition.ErroredPather && !this.ConditionAllowed(PsychicRitualRoleDef.Condition.NoPath) && !this.CanReach(pawn, psychicRitual.assignments.Target))
			{
				psychicRitual.LeaveOrCancelPsychicRitual(this, pawn, PsychicRitualRoleDef.ConditionToReason(pawn, PsychicRitualRoleDef.Condition.NoPath));
			}
		}

		// Token: 0x04003472 RID: 13426
		public static IReadOnlyDictionary<DevelopmentalStage, PsychicRitualRoleDef.Condition> DevelopmentalStageToCondition = new Dictionary<DevelopmentalStage, PsychicRitualRoleDef.Condition>
		{
			{
				DevelopmentalStage.None,
				PsychicRitualRoleDef.Condition.None
			},
			{
				DevelopmentalStage.Newborn,
				PsychicRitualRoleDef.Condition.Baby
			},
			{
				DevelopmentalStage.Baby,
				PsychicRitualRoleDef.Condition.Baby
			},
			{
				DevelopmentalStage.Child,
				PsychicRitualRoleDef.Condition.Child
			},
			{
				DevelopmentalStage.Adult,
				PsychicRitualRoleDef.Condition.Adult
			}
		};

		// Token: 0x04003473 RID: 13427
		public static IReadOnlyDictionary<PawnLostCondition, PsychicRitualRoleDef.Condition> PawnLostConditionToCondition = new Dictionary<PawnLostCondition, PsychicRitualRoleDef.Condition>
		{
			{
				PawnLostCondition.ChangedFaction,
				PsychicRitualRoleDef.Condition.NonPlayerFaction
			},
			{
				PawnLostCondition.Drafted,
				PsychicRitualRoleDef.Condition.Drafted
			},
			{
				PawnLostCondition.ExitedMap,
				PsychicRitualRoleDef.Condition.NotOnMap
			},
			{
				PawnLostCondition.ForcedByPlayerAction,
				PsychicRitualRoleDef.Condition.None
			},
			{
				PawnLostCondition.ForcedByQuest,
				PsychicRitualRoleDef.Condition.None
			},
			{
				PawnLostCondition.ForcedToJoinOtherLord,
				PsychicRitualRoleDef.Condition.HasAnotherLord
			},
			{
				PawnLostCondition.Incapped,
				PsychicRitualRoleDef.Condition.Downed
			},
			{
				PawnLostCondition.InMentalState,
				PsychicRitualRoleDef.Condition.MentalState
			},
			{
				PawnLostCondition.Killed,
				PsychicRitualRoleDef.Condition.Dead
			},
			{
				PawnLostCondition.LeftVoluntarily,
				PsychicRitualRoleDef.Condition.LeftVoluntarily
			},
			{
				PawnLostCondition.MadePrisoner,
				PsychicRitualRoleDef.Condition.Prisoner
			},
			{
				PawnLostCondition.MadeSlave,
				PsychicRitualRoleDef.Condition.Slave
			},
			{
				PawnLostCondition.NoLongerEnteringTransportPods,
				PsychicRitualRoleDef.Condition.None
			},
			{
				PawnLostCondition.Undefined,
				PsychicRitualRoleDef.Condition.Error
			},
			{
				PawnLostCondition.Vanished,
				PsychicRitualRoleDef.Condition.Vanished
			}
		};

		// Token: 0x04003474 RID: 13428
		protected IntRange allowedCount = new IntRange(0, 1);

		// Token: 0x04003475 RID: 13429
		protected PsychicRitualRoleDef.Condition allowedConditions = PsychicRitualRoleDef.Condition.Default;

		// Token: 0x04003476 RID: 13430
		protected bool canHandleOfferings;

		// Token: 0x04003477 RID: 13431
		public bool psychicRitualWaitsForArrival = true;

		// Token: 0x04003478 RID: 13432
		public bool showInvocationVfx;

		// Token: 0x04003479 RID: 13433
		public bool applyPowerOffset = true;

		// Token: 0x0400347A RID: 13434
		public bool removeOnLost = true;

		// Token: 0x02002E93 RID: 11923
		[Flags]
		public enum Condition : uint
		{
			// Token: 0x0400BCBE RID: 48318
			None = 0U,
			// Token: 0x0400BCBF RID: 48319
			Dead = 1U,
			// Token: 0x0400BCC0 RID: 48320
			Downed = 2U,
			// Token: 0x0400BCC1 RID: 48321
			Drafted = 4U,
			// Token: 0x0400BCC2 RID: 48322
			Sleeping = 8U,
			// Token: 0x0400BCC3 RID: 48323
			Bleeding = 16U,
			// Token: 0x0400BCC4 RID: 48324
			Burning = 32U,
			// Token: 0x0400BCC5 RID: 48325
			MentalState = 64U,
			// Token: 0x0400BCC6 RID: 48326
			ExtremeTemperature = 128U,
			// Token: 0x0400BCC7 RID: 48327
			Humanlike = 256U,
			// Token: 0x0400BCC8 RID: 48328
			NonHumanlike = 512U,
			// Token: 0x0400BCC9 RID: 48329
			Prisoner = 1024U,
			// Token: 0x0400BCCA RID: 48330
			Slave = 2048U,
			// Token: 0x0400BCCB RID: 48331
			Freeman = 4096U,
			// Token: 0x0400BCCC RID: 48332
			InsideBuilding = 8192U,
			// Token: 0x0400BCCD RID: 48333
			NonPlayerFaction = 16384U,
			// Token: 0x0400BCCE RID: 48334
			NotOnMap = 32768U,
			// Token: 0x0400BCCF RID: 48335
			HasAnotherLord = 65536U,
			// Token: 0x0400BCD0 RID: 48336
			LeftVoluntarily = 131072U,
			// Token: 0x0400BCD1 RID: 48337
			Vanished = 262144U,
			// Token: 0x0400BCD2 RID: 48338
			Baby = 524288U,
			// Token: 0x0400BCD3 RID: 48339
			Child = 1048576U,
			// Token: 0x0400BCD4 RID: 48340
			Adult = 2097152U,
			// Token: 0x0400BCD5 RID: 48341
			NoPath = 4194304U,
			// Token: 0x0400BCD6 RID: 48342
			SubHuman = 8388608U,
			// Token: 0x0400BCD7 RID: 48343
			IdeoUnwilling = 16777216U,
			// Token: 0x0400BCD8 RID: 48344
			NoPsychicSensitivity = 33554432U,
			// Token: 0x0400BCD9 RID: 48345
			TemporaryFactionMember = 67108864U,
			// Token: 0x0400BCDA RID: 48346
			Error = 134217728U,
			// Token: 0x0400BCDB RID: 48347
			All = 4294967295U,
			// Token: 0x0400BCDC RID: 48348
			Default = 70260992U,
			// Token: 0x0400BCDD RID: 48349
			DefaultInvoker = 3152128U
		}

		// Token: 0x02002E94 RID: 11924
		public enum Context
		{
			// Token: 0x0400BCDF RID: 48351
			InvokePsychicRitualGizmo,
			// Token: 0x0400BCE0 RID: 48352
			Dialog_BeginPsychicRitual,
			// Token: 0x0400BCE1 RID: 48353
			Runtime,
			// Token: 0x0400BCE2 RID: 48354
			NonPlayerFaction
		}

		// Token: 0x02002E95 RID: 11925
		public struct Reason
		{
			// Token: 0x060119BA RID: 72122 RVA: 0x0060AF57 File Offset: 0x00609157
			public Reason(PsychicRitualRoleDef def, PsychicRitualRoleDef.Context context, Pawn pawn, TargetInfo target, AnyEnum reasonCode)
			{
				this.def = def;
				this.context = context;
				this.pawn = pawn;
				this.target = target;
				this.reasonCode = reasonCode;
			}

			// Token: 0x17002EC0 RID: 11968
			// (get) Token: 0x060119BB RID: 72123 RVA: 0x0060AF80 File Offset: 0x00609180
			public static PsychicRitualRoleDef.Reason None
			{
				get
				{
					return default(PsychicRitualRoleDef.Reason);
				}
			}

			// Token: 0x060119BC RID: 72124 RVA: 0x0060AF96 File Offset: 0x00609196
			public TaggedString ToPlayerReadable()
			{
				PsychicRitualRoleDef psychicRitualRoleDef = this.def;
				if (psychicRitualRoleDef == null)
				{
					return TaggedString.Empty;
				}
				return psychicRitualRoleDef.PawnCannotDoReason(this.reasonCode, this.context, this.pawn, this.target);
			}

			// Token: 0x0400BCE3 RID: 48355
			public PsychicRitualRoleDef def;

			// Token: 0x0400BCE4 RID: 48356
			public PsychicRitualRoleDef.Context context;

			// Token: 0x0400BCE5 RID: 48357
			public Pawn pawn;

			// Token: 0x0400BCE6 RID: 48358
			public TargetInfo target;

			// Token: 0x0400BCE7 RID: 48359
			public AnyEnum reasonCode;
		}
	}
}
