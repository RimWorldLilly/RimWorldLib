using System;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A9D RID: 2717
	public static class Toils_Interpersonal
	{
		// Token: 0x060051BE RID: 20926 RVA: 0x001E7B24 File Offset: 0x001E5D24
		public static Toil GotoInteractablePosition(TargetIndex target)
		{
			Toil toil = ToilMaker.MakeToil("GotoInteractablePosition");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Pawn pawn = (Pawn)((Thing)actor.CurJob.GetTarget(target));
				if (SocialInteractionUtility.IsGoodPositionForInteraction(actor, pawn))
				{
					actor.jobs.curDriver.ReadyForNextToil();
					return;
				}
				actor.pather.StartPath(pawn, PathEndMode.Touch);
			};
			toil.tickIntervalAction = delegate(int delta)
			{
				Pawn actor = toil.actor;
				Pawn pawn = (Pawn)((Thing)actor.CurJob.GetTarget(target));
				Map map = actor.Map;
				if (SocialInteractionUtility.IsGoodPositionForInteraction(actor, pawn) && actor.Position.InHorDistOf(pawn.Position, (float)Mathf.CeilToInt(3f)) && (!actor.pather.Moving || actor.pather.nextCell.GetDoor(map) == null))
				{
					actor.pather.StopDead();
					actor.jobs.curDriver.ReadyForNextToil();
					return;
				}
				if (!actor.pather.Moving)
				{
					IntVec3 c = SocialInteractionUtility.BestInteractableCell(actor, pawn);
					if (c.IsValid)
					{
						actor.pather.StartPath(c, PathEndMode.OnCell);
						return;
					}
					actor.jobs.curDriver.EndJobWith(JobCondition.Incompletable);
				}
			};
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			return toil;
		}

		// Token: 0x060051BF RID: 20927 RVA: 0x001E7B9C File Offset: 0x001E5D9C
		public static Toil GotoPrisoner(Pawn pawn, Pawn talkee, PrisonerInteractionModeDef mode)
		{
			Toil toil = ToilMaker.MakeToil("GotoPrisoner");
			toil.initAction = delegate()
			{
				pawn.pather.StartPath(talkee, PathEndMode.Touch);
			};
			toil.AddFailCondition(() => talkee.DestroyedOrNull() || (mode.mustBeAwake && !talkee.Awake()) || !talkee.IsPrisonerOfColony || (talkee.guest == null || talkee.guest.IsInteractionDisabled(mode)));
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return toil;
		}

		// Token: 0x060051C0 RID: 20928 RVA: 0x001E7C00 File Offset: 0x001E5E00
		public static Toil GotoGuiltyColonist(Pawn pawn, Pawn talkee)
		{
			Toil toil = ToilMaker.MakeToil("GotoGuiltyColonist");
			toil.initAction = delegate()
			{
				pawn.pather.StartPath(talkee, PathEndMode.Touch);
			};
			toil.AddFailCondition(() => talkee.DestroyedOrNull() || !talkee.guilt.IsGuilty);
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return toil;
		}

		// Token: 0x060051C1 RID: 20929 RVA: 0x001E7C60 File Offset: 0x001E5E60
		public static Toil GotoSlave(Pawn pawn, Pawn talkee)
		{
			Toil toil = ToilMaker.MakeToil("GotoSlave");
			toil.initAction = delegate()
			{
				pawn.pather.StartPath(talkee, PathEndMode.Touch);
			};
			toil.AddFailCondition(() => talkee.DestroyedOrNull() || !talkee.IsSlaveOfColony);
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.PatherArrival;
			return toil;
		}

		// Token: 0x060051C2 RID: 20930 RVA: 0x001E7CC0 File Offset: 0x001E5EC0
		public static Toil WaitToBeAbleToInteract(Pawn pawn)
		{
			Toil toil = ToilMaker.MakeToil("WaitToBeAbleToInteract");
			toil.initAction = delegate()
			{
				if (!pawn.interactions.InteractedTooRecentlyToInteract())
				{
					pawn.jobs.curDriver.ReadyForNextToil();
				}
			};
			toil.tickIntervalAction = delegate(int delta)
			{
				if (!pawn.interactions.InteractedTooRecentlyToInteract())
				{
					pawn.jobs.curDriver.ReadyForNextToil();
				}
			};
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Never;
			return toil;
		}

		// Token: 0x060051C3 RID: 20931 RVA: 0x001E7D18 File Offset: 0x001E5F18
		public static Toil ConvinceRecruitee(Pawn pawn, Pawn talkee, InteractionDef interactionDef = null)
		{
			Toil toil = ToilMaker.MakeToil("ConvinceRecruitee");
			toil.initAction = delegate()
			{
				if (!pawn.interactions.TryInteractWith(talkee, interactionDef ?? InteractionDefOf.BuildRapport))
				{
					pawn.jobs.curDriver.ReadyForNextToil();
					return;
				}
				pawn.records.Increment(RecordDefOf.PrisonersChatted);
			};
			toil.FailOn(() => !talkee.guest.ScheduledForInteraction);
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 350;
			toil.activeSkill = (() => SkillDefOf.Social);
			return toil;
		}

		// Token: 0x060051C4 RID: 20932 RVA: 0x001E7DB0 File Offset: 0x001E5FB0
		public static Toil Interrogate(Pawn pawn, Pawn talkee)
		{
			Toil toil = ToilMaker.MakeToil("Interrogate");
			toil.initAction = delegate()
			{
				if (!pawn.interactions.TryInteractWith(talkee, InteractionDefOf.InterrogateIdentity))
				{
					pawn.jobs.curDriver.ReadyForNextToil();
					return;
				}
				pawn.records.Increment(RecordDefOf.PrisonersChatted);
			};
			toil.FailOn(() => !talkee.guest.ScheduledForInteraction);
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 350;
			toil.activeSkill = (() => SkillDefOf.Social);
			return toil;
		}

		// Token: 0x060051C5 RID: 20933 RVA: 0x001E7E40 File Offset: 0x001E6040
		public static Toil ReduceWill(Pawn pawn, Pawn talkee)
		{
			Toil toil = ToilMaker.MakeToil("ReduceWill");
			toil.initAction = delegate()
			{
				if (!pawn.interactions.TryInteractWith(talkee, InteractionDefOf.ReduceWill))
				{
					pawn.jobs.curDriver.ReadyForNextToil();
					return;
				}
				pawn.records.Increment(RecordDefOf.PrisonersChatted);
			};
			toil.FailOn(() => !talkee.guest.ScheduledForInteraction);
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 350;
			toil.activeSkill = (() => SkillDefOf.Social);
			return toil;
		}

		// Token: 0x060051C6 RID: 20934 RVA: 0x001E7ED0 File Offset: 0x001E60D0
		public static Toil SetLastInteractTime(TargetIndex targetInd)
		{
			Toil toil = ToilMaker.MakeToil("SetLastInteractTime");
			toil.initAction = delegate()
			{
				Pawn pawn = (Pawn)toil.actor.jobs.curJob.GetTarget(targetInd).Thing;
				pawn.mindState.lastAssignedInteractTime = Find.TickManager.TicksGame;
				pawn.mindState.interactionsToday++;
			};
			toil.defaultCompleteMode = ToilCompleteMode.Instant;
			return toil;
		}

		// Token: 0x060051C7 RID: 20935 RVA: 0x001E7F24 File Offset: 0x001E6124
		public static Toil TryRecruit(TargetIndex recruiteeInd)
		{
			Toil toil = ToilMaker.MakeToil("TryRecruit");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Pawn pawn = (Pawn)actor.jobs.curJob.GetTarget(recruiteeInd).Thing;
				if (!pawn.Spawned || !pawn.Awake())
				{
					return;
				}
				InteractionDef intDef = pawn.AnimalOrWildMan() ? InteractionDefOf.TameAttempt : InteractionDefOf.RecruitAttempt;
				actor.interactions.TryInteractWith(pawn, intDef);
			};
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 350;
			toil.activeSkill = delegate()
			{
				if (!((Pawn)toil.actor.jobs.curJob.GetTarget(recruiteeInd).Thing).RaceProps.Animal)
				{
					return SkillDefOf.Social;
				}
				return SkillDefOf.Animals;
			};
			return toil;
		}

		// Token: 0x060051C8 RID: 20936 RVA: 0x001E7FAC File Offset: 0x001E61AC
		public static Toil TryTrain(TargetIndex traineeInd)
		{
			Toil toil = ToilMaker.MakeToil("TryTrain");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Pawn pawn = (Pawn)actor.jobs.curJob.GetTarget(traineeInd).Thing;
				if (pawn.Spawned && pawn.Awake() && actor.interactions.TryInteractWith(pawn, InteractionDefOf.TrainAttempt))
				{
					float num = actor.GetStatValue(StatDefOf.TrainAnimalChance, true, -1);
					num *= GenMath.LerpDouble(0f, 1f, 1.5f, 0.5f, pawn.GetStatValue(StatDefOf.Wildness, true, -1));
					if (actor.relations.DirectRelationExists(PawnRelationDefOf.Bond, pawn))
					{
						num *= 5f;
					}
					num = Mathf.Clamp01(num);
					TrainableDef trainableDef = pawn.training.NextTrainableToTrain();
					if (trainableDef == null)
					{
						Log.ErrorOnce("Attempted to train untrainable animal", 7842936);
						return;
					}
					string text;
					if (Rand.Value < num)
					{
						pawn.training.Train(trainableDef, actor, false);
						if (pawn.caller != null)
						{
							pawn.caller.DoCall(false);
						}
						text = "TextMote_TrainSuccess".Translate(trainableDef.LabelCap, num.ToStringPercent());
						RelationsUtility.TryDevelopBondRelation(actor, pawn, 0.007f);
						TaleRecorder.RecordTale(TaleDefOf.TrainedAnimal, new object[]
						{
							actor,
							pawn,
							trainableDef
						});
					}
					else
					{
						text = "TextMote_TrainFail".Translate(trainableDef.LabelCap, num.ToStringPercent());
					}
					text = string.Concat(new string[]
					{
						text,
						"\n",
						pawn.training.GetSteps(trainableDef).ToString(),
						" / ",
						trainableDef.steps.ToString()
					});
					MoteMaker.ThrowText((actor.DrawPos + pawn.DrawPos) / 2f, actor.Map, text, 5f);
				}
			};
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 100;
			toil.activeSkill = (() => SkillDefOf.Animals);
			return toil;
		}

		// Token: 0x060051C9 RID: 20937 RVA: 0x001E8038 File Offset: 0x001E6238
		public static Toil Interact(TargetIndex otherPawnInd, InteractionDef interaction)
		{
			Toil toil = ToilMaker.MakeToil("Interact");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Pawn pawn = (Pawn)actor.jobs.curJob.GetTarget(otherPawnInd).Thing;
				if (!pawn.Spawned)
				{
					return;
				}
				actor.interactions.TryInteractWith(pawn, interaction);
			};
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 60;
			return toil;
		}

		// Token: 0x060051CA RID: 20938 RVA: 0x001E80AC File Offset: 0x001E62AC
		public static Toil TryEnslave(TargetIndex prisonerInd)
		{
			Toil toil = ToilMaker.MakeToil("TryEnslave");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Pawn pawn = (Pawn)actor.jobs.curJob.GetTarget(prisonerInd).Thing;
				if (!pawn.Spawned || !pawn.Awake())
				{
					return;
				}
				actor.interactions.TryInteractWith(pawn, InteractionDefOf.EnslaveAttempt);
			};
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 350;
			toil.activeSkill = (() => SkillDefOf.Social);
			return toil;
		}

		// Token: 0x060051CB RID: 20939 RVA: 0x001E8148 File Offset: 0x001E6348
		public static Toil TryConvert(TargetIndex prisonerInd)
		{
			Toil toil = ToilMaker.MakeToil("TryConvert");
			toil.initAction = delegate()
			{
				Pawn actor = toil.actor;
				Pawn recipient = (Pawn)actor.jobs.curJob.GetTarget(prisonerInd).Thing;
				actor.interactions.TryInteractWith(recipient, InteractionDefOf.ConvertIdeoAttempt);
			};
			toil.socialMode = RandomSocialMode.Off;
			toil.defaultCompleteMode = ToilCompleteMode.Delay;
			toil.defaultDuration = 350;
			toil.activeSkill = (() => SkillDefOf.Social);
			return toil;
		}

		// Token: 0x060051CC RID: 20940 RVA: 0x001E81E4 File Offset: 0x001E63E4
		public static Toil ExtractHemogen(TargetIndex prisonerInd, float bloodLoss)
		{
			Toil toil = ToilMaker.MakeToil("ExtractHemogen");
			toil.initAction = delegate()
			{
				GenPlace.TryPlaceThing(ThingMaker.MakeThing(ThingDefOf.HemogenPack, null), toil.actor.Position, toil.actor.Map, ThingPlaceMode.Near, null, null, null, 1);
				Pawn pawn = (Pawn)toil.actor.jobs.curJob.GetTarget(prisonerInd).Thing;
				Hediff hediff = HediffMaker.MakeHediff(HediffDefOf.BloodLoss, pawn, null);
				hediff.Severity = bloodLoss;
				pawn.health.AddHediff(hediff, null, null, null);
			};
			return toil;
		}
	}
}
