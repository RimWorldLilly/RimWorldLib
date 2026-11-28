using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200172E RID: 5934
	public static class RitualUtility
	{
		// Token: 0x060093F7 RID: 37879 RVA: 0x0035EF0B File Offset: 0x0035D10B
		public static bool IsRitualTarget(this Thing thing)
		{
			return thing.TargetOfRitual() != null;
		}

		// Token: 0x060093F8 RID: 37880 RVA: 0x0035EF18 File Offset: 0x0035D118
		public static LordJob_Ritual TargetOfRitual(this Thing thing)
		{
			if (!thing.Spawned)
			{
				return null;
			}
			foreach (Lord lord in thing.Map.lordManager.lords)
			{
				LordJob_Ritual lordJob_Ritual = lord.LordJob as LordJob_Ritual;
				if (lordJob_Ritual != null && lordJob_Ritual.selectedTarget.Thing == thing)
				{
					LordToilData_Gathering lordToilData_Gathering = lordJob_Ritual.lord.CurLordToil.data as LordToilData_Gathering;
					if (lordToilData_Gathering != null && lordToilData_Gathering.presentForTicks.Any<KeyValuePair<Pawn, int>>())
					{
						return lordJob_Ritual;
					}
				}
			}
			return null;
		}

		// Token: 0x060093F9 RID: 37881 RVA: 0x0035EFC0 File Offset: 0x0035D1C0
		public static bool GoodSpectateCellForRitual(IntVec3 spot, Pawn p, Map map)
		{
			return !spot.ContainsStaticFire(map) && !spot.GetTerrain(map).avoidWander && !PawnUtility.KnownDangerAt(spot, map, p);
		}

		// Token: 0x060093FA RID: 37882 RVA: 0x0035EFE8 File Offset: 0x0035D1E8
		public static float CalculateQualityAbstract(Precept_Ritual ritual, TargetInfo ritualTarget, RitualRoleAssignments assignments, RitualObligation obligation = null)
		{
			RitualOutcomeEffectDef def = ritual.outcomeEffect.def;
			float num = def.startingQuality;
			foreach (RitualOutcomeComp ritualOutcomeComp in def.comps)
			{
				QualityFactor qualityFactor = ritualOutcomeComp.GetQualityFactor(ritual, ritualTarget, obligation, assignments, null);
				if (qualityFactor != null)
				{
					if (qualityFactor.uncertainOutcome)
					{
						Log.Warning("Tried to calculate abstract quality for a ritual with uncertain quality offset, that can't be calculated without actually executing the ritual.");
					}
					else
					{
						num += qualityFactor.quality;
					}
				}
			}
			return num;
		}

		// Token: 0x060093FB RID: 37883 RVA: 0x0035F070 File Offset: 0x0035D270
		public static string QualityBreakdownAbstract(Precept_Ritual ritual, TargetInfo ritualTarget, RitualRoleAssignments assignments, RitualObligation obligation = null)
		{
			RitualOutcomeEffectDef def = ritual.outcomeEffect.def;
			float f = RitualUtility.CalculateQualityAbstract(ritual, ritualTarget, assignments, obligation);
			TaggedString taggedString = "RitualOutcomeQualitySpecific".Translate(ritual.Label, f.ToStringPercent()).CapitalizeFirst() + ":\n";
			if (def.startingQuality > 0f)
			{
				taggedString += "\n  - " + "StartingRitualQuality".Translate(def.startingQuality.ToStringPercent()) + ".";
			}
			foreach (RitualOutcomeComp ritualOutcomeComp in def.comps)
			{
				QualityFactor qualityFactor = ritualOutcomeComp.GetQualityFactor(ritual, ritualTarget, obligation, assignments, null);
				if (qualityFactor != null)
				{
					taggedString += "\n  - " + qualityFactor.label;
					taggedString += ": " + ritualOutcomeComp.GetDescAbstract(qualityFactor.positive, qualityFactor.quality) + ".";
				}
			}
			if (ritual.RepeatPenaltyActive)
			{
				taggedString += "\n  - " + "RitualOutcomePerformedRecently".Translate() + ": " + ritual.RepeatQualityPenalty.ToStringPercent();
			}
			Tuple<ExpectationDef, float> expectationsOffset = RitualOutcomeEffectWorker_FromQuality.GetExpectationsOffset(ritualTarget.Map, ritual.def);
			if (expectationsOffset != null)
			{
				taggedString += "\n  - " + "RitualQualityExpectations".Translate(expectationsOffset.Item1.LabelCap) + ": " + expectationsOffset.Item2.ToStringPercent();
			}
			return taggedString;
		}

		// Token: 0x060093FC RID: 37884 RVA: 0x0035F240 File Offset: 0x0035D440
		public static void AddArrivalTag(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			LordJob_Ritual lordJob_Ritual = ((lord != null) ? lord.LordJob : null) as LordJob_Ritual;
			if (lordJob_Ritual != null && !lordJob_Ritual.PawnTagSet(pawn, "Arrived"))
			{
				lordJob_Ritual.AddTagForPawn(pawn, "Arrived");
			}
		}

		// Token: 0x060093FD RID: 37885 RVA: 0x0035F282 File Offset: 0x0035D482
		public static IEnumerable<LordJob_Ritual> GetActiveRitualsForPrecept(Precept_Ritual precept)
		{
			RitualUtility.<GetActiveRitualsForPrecept>d__6 <GetActiveRitualsForPrecept>d__ = new RitualUtility.<GetActiveRitualsForPrecept>d__6(-2);
			<GetActiveRitualsForPrecept>d__.<>3__precept = precept;
			return <GetActiveRitualsForPrecept>d__;
		}

		// Token: 0x060093FE RID: 37886 RVA: 0x0035F294 File Offset: 0x0035D494
		public static string RoleChangeConfirmation(Pawn pawn, Precept_Role oldRole, Precept_Role newRole)
		{
			string text = "";
			if (oldRole != null)
			{
				text += " " + "ChooseRoleConfirmAssignHasOtherRole".Translate(pawn.Named("PAWN"), oldRole.Named("ROLE"));
			}
			Pawn pawn2 = newRole.ChosenPawns().FirstOrDefault<Pawn>();
			if (pawn2 != null && newRole is Precept_RoleSingle)
			{
				text += " " + "ChooseRoleConfirmAssignReplace".Translate(pawn2.Named("PAWN"));
			}
			else if (newRole.def.leaderRole)
			{
				foreach (Ideo ideo in Faction.OfPlayer.ideos.AllIdeos)
				{
					foreach (Precept precept in ideo.PreceptsListForReading)
					{
						if (precept != oldRole)
						{
							Precept_Role precept_Role = precept as Precept_Role;
							if (precept_Role != null)
							{
								Pawn pawn3 = precept_Role.ChosenPawnSingle();
								if (precept_Role.def.leaderRole && pawn3 != null && pawn3 != pawn && pawn3.IsFreeColonist)
								{
									text += " " + "ChooseRoleConfirmAssignReplaceLeader".Translate(pawn3.Named("PAWN"), newRole.Named("ROLE"), precept_Role.Named("OTHERROLE"));
									break;
								}
							}
						}
					}
				}
			}
			IEnumerable<WorkTypeDef> disabledWorkTypes = newRole.DisabledWorkTypes;
			if (disabledWorkTypes.Any<WorkTypeDef>())
			{
				if (!text.NullOrEmpty())
				{
					text += "\n\n";
				}
				text += "ChooseRoleListWorkTypeRestrictions".Translate(pawn.Named("PAWN")) + ": \n" + (from x in disabledWorkTypes
				select x.labelShort.CapitalizeFirst()).ToLineList("  - ", false);
			}
			if (!newRole.def.grantedAbilities.NullOrEmpty<AbilityDef>())
			{
				if (!text.NullOrEmpty())
				{
					text += "\n\n";
				}
				text += "ChooseRoleListAbilities".Translate(pawn.Named("PAWN")) + ": \n" + (from x in newRole.def.grantedAbilities
				select x.LabelCap.ToString()).ToLineList("  - ", false);
			}
			if (!newRole.apparelRequirements.NullOrEmpty<PreceptApparelRequirement>())
			{
				if (!text.NullOrEmpty())
				{
					text += "\n\n";
				}
				text += "ChooseRoleListApparelDemands".Translate(newRole.Named("ROLE")) + ": \n" + newRole.AllApparelRequirementLabels(pawn.gender, pawn).ToLineList("  - ");
			}
			if (newRole.def.roleRequiredWorkTags != WorkTags.None)
			{
				List<string> list = new List<string>();
				foreach (WorkTags workTags in newRole.def.roleRequiredWorkTags.GetAllSelectedItems<WorkTags>())
				{
					if (pawn.WorkTagIsDisabled(workTags))
					{
						list.Add(workTags.LabelTranslated().CapitalizeFirst());
					}
				}
				if (list.Any<string>())
				{
					if (!text.NullOrEmpty())
					{
						text += "\n\n";
					}
					text += "ChooseRoleRequiredWorkTagsDisabled".Translate(pawn.Named("PAWN"), newRole.Named("ROLE")) + ": \n" + list.ToLineList("  - ");
				}
			}
			else if (newRole.def.roleRequiredWorkTagAny != WorkTags.None)
			{
				bool flag = false;
				List<string> list2 = new List<string>();
				foreach (WorkTags workTags2 in newRole.def.roleRequiredWorkTagAny.GetAllSelectedItems<WorkTags>())
				{
					if (!pawn.WorkTagIsDisabled(workTags2))
					{
						flag = true;
						break;
					}
					list2.Add(workTags2.LabelTranslated().CapitalizeFirst());
				}
				if (!flag)
				{
					if (!text.NullOrEmpty())
					{
						text += "\n\n";
					}
					text += "ChooseRoleRequiredWorkTagsDisabled".Translate(pawn.Named("PAWN"), newRole.Named("ROLE")) + ": \n" + list2.ToLineList("  - ");
				}
			}
			if (newRole.ChosenPawnSingle() == null && newRole is Precept_RoleSingle)
			{
				if (!text.NullOrEmpty())
				{
					text += "\n\n";
				}
				text += "ChooseRoleHint".Translate();
			}
			return text;
		}

		// Token: 0x060093FF RID: 37887 RVA: 0x0035F7C8 File Offset: 0x0035D9C8
		public static IEnumerable<Precept_Role> AllRolesForPawn(Pawn p)
		{
			RitualUtility.<AllRolesForPawn>d__8 <AllRolesForPawn>d__ = new RitualUtility.<AllRolesForPawn>d__8(-2);
			<AllRolesForPawn>d__.<>3__p = p;
			return <AllRolesForPawn>d__;
		}

		// Token: 0x06009400 RID: 37888 RVA: 0x0035F7D8 File Offset: 0x0035D9D8
		public static IntVec3 RitualCrowdCenterFor(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			LordJob_Ritual lordJob_Ritual = ((lord != null) ? lord.LordJob : null) as LordJob_Ritual;
			if (lordJob_Ritual != null && pawn.CurJob != null && pawn.CurJob.speechFaceSpectatorsIfPossible)
			{
				return lordJob_Ritual.CurrentSpectatorCrowdCenter();
			}
			return IntVec3.Invalid;
		}
	}
}
