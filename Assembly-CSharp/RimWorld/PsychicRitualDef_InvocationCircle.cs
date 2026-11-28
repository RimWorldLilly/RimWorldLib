using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000C99 RID: 3225
	public class PsychicRitualDef_InvocationCircle : PsychicRitualDef
	{
		// Token: 0x17000FE6 RID: 4070
		// (get) Token: 0x06005B5C RID: 23388 RVA: 0x002194EE File Offset: 0x002176EE
		public virtual PsychicRitualRoleDef InvokerRole
		{
			get
			{
				return this.invokerRole;
			}
		}

		// Token: 0x17000FE7 RID: 4071
		// (get) Token: 0x06005B5D RID: 23389 RVA: 0x002194F6 File Offset: 0x002176F6
		public virtual PsychicRitualRoleDef ChanterRole
		{
			get
			{
				return this.chanterRole;
			}
		}

		// Token: 0x17000FE8 RID: 4072
		// (get) Token: 0x06005B5E RID: 23390 RVA: 0x002194FE File Offset: 0x002176FE
		public virtual PsychicRitualRoleDef TargetRole
		{
			get
			{
				return this.targetRole;
			}
		}

		// Token: 0x17000FE9 RID: 4073
		// (get) Token: 0x06005B5F RID: 23391 RVA: 0x00219506 File Offset: 0x00217706
		public virtual PsychicRitualRoleDef DefenderRole
		{
			get
			{
				return this.defenderRole;
			}
		}

		// Token: 0x17000FEA RID: 4074
		// (get) Token: 0x06005B60 RID: 23392 RVA: 0x0021950E File Offset: 0x0021770E
		public virtual IngredientCount RequiredOffering
		{
			get
			{
				return this.requiredOffering;
			}
		}

		// Token: 0x17000FEB RID: 4075
		// (get) Token: 0x06005B61 RID: 23393 RVA: 0x00219516 File Offset: 0x00217716
		public TaggedString CooldownLabel
		{
			get
			{
				return "PsychicRitualCooldownLabel".Translate() + ": " + (this.cooldownHours * 2500).ToStringTicksToPeriod(true, false, true, true, false);
			}
		}

		// Token: 0x17000FEC RID: 4076
		// (get) Token: 0x06005B62 RID: 23394 RVA: 0x00219548 File Offset: 0x00217748
		public override List<PsychicRitualRoleDef> Roles
		{
			get
			{
				List<PsychicRitualRoleDef> roles = base.Roles;
				if (this.InvokerRole != null)
				{
					roles.Add(this.InvokerRole);
				}
				if (this.TargetRole != null)
				{
					roles.Add(this.TargetRole);
				}
				if (this.ChanterRole != null)
				{
					roles.Add(this.ChanterRole);
				}
				if (this.DefenderRole != null)
				{
					roles.Add(this.DefenderRole);
				}
				return roles;
			}
		}

		// Token: 0x06005B63 RID: 23395 RVA: 0x002195B0 File Offset: 0x002177B0
		public override void ResolveReferences()
		{
			base.ResolveReferences();
			IngredientCount ingredientCount = this.requiredOffering;
			if (ingredientCount != null)
			{
				ingredientCount.ResolveReferences();
			}
			this.invokerRole = (this.invokerRole ?? PsychicRitualRoleDefOf.Invoker);
			this.chanterRole = (this.chanterRole ?? PsychicRitualRoleDefOf.Chanter);
		}

		// Token: 0x06005B64 RID: 23396 RVA: 0x00219600 File Offset: 0x00217800
		public override List<PsychicRitualToil> CreateToils(PsychicRitual psychicRitual, PsychicRitualGraph parent)
		{
			float randomInRange = this.hoursUntilOutcome.RandomInRange;
			IReadOnlyDictionary<PsychicRitualRoleDef, List<IntVec3>> readOnlyDictionary = this.GenerateRolePositions(psychicRitual.assignments);
			return new List<PsychicRitualToil>
			{
				new PsychicRitualToil_GatherForInvocation(psychicRitual, this, readOnlyDictionary),
				new PsychicRitualToil_InvokeHorax(this.InvokerRole, readOnlyDictionary.TryGetValue(this.InvokerRole, null), this.TargetRole, readOnlyDictionary.TryGetValue(this.TargetRole, null), this.ChanterRole, readOnlyDictionary.TryGetValue(this.ChanterRole, null), this.DefenderRole, readOnlyDictionary.TryGetValue(this.DefenderRole, null), this.RequiredOffering)
				{
					hoursUntilHoraxEffect = this.hoursUntilHoraxEffect.RandomInRange,
					hoursUntilOutcome = randomInRange
				},
				new PsychicRitualToil_Wait(120)
			};
		}

		// Token: 0x06005B65 RID: 23397 RVA: 0x002196BC File Offset: 0x002178BC
		public override bool IsValidTarget(TargetInfo target, out AnyEnum reason)
		{
			using (IEnumerator<IntVec3> enumerator = GenRadial.RadialCellsAround(target.Cell, this.invocationCircleRadius, true).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.Standable(target.Map))
					{
						reason = AnyEnum.FromEnum<PsychicRitualDef_InvocationCircle.InvalidTargetReasonEnum>(PsychicRitualDef_InvocationCircle.InvalidTargetReasonEnum.AreaNotClear);
						return false;
					}
				}
			}
			reason = AnyEnum.None;
			return true;
		}

		// Token: 0x06005B66 RID: 23398 RVA: 0x0021973C File Offset: 0x0021793C
		public override TaggedString InvalidTargetReason(AnyEnum reason)
		{
			PsychicRitualDef_InvocationCircle.InvalidTargetReasonEnum? invalidTargetReasonEnum = reason.As<PsychicRitualDef_InvocationCircle.InvalidTargetReasonEnum>();
			if (invalidTargetReasonEnum == null)
			{
				return base.InvalidTargetReason(reason);
			}
			PsychicRitualDef_InvocationCircle.InvalidTargetReasonEnum valueOrDefault = invalidTargetReasonEnum.GetValueOrDefault();
			if (valueOrDefault == PsychicRitualDef_InvocationCircle.InvalidTargetReasonEnum.None)
			{
				return TaggedString.Empty;
			}
			if (valueOrDefault != PsychicRitualDef_InvocationCircle.InvalidTargetReasonEnum.AreaNotClear)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(15, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unknown reason ");
				defaultInterpolatedStringHandler.AppendFormatted<PsychicRitualDef_InvocationCircle.InvalidTargetReasonEnum>(valueOrDefault);
				throw new InvalidOperationException(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return "PsychicRitualDef_InvocationCircle_AreaMustBeClear".Translate();
		}

		// Token: 0x06005B67 RID: 23399 RVA: 0x002197AE File Offset: 0x002179AE
		public override TaggedString OutcomeDescription(FloatRange qualityRange, string qualityNumber, PsychicRitualRoleAssignments assignments)
		{
			return this.outcomeDescription.Formatted(Array.Empty<NamedArgument>());
		}

		// Token: 0x06005B68 RID: 23400 RVA: 0x002197C0 File Offset: 0x002179C0
		public override IEnumerable<TaggedString> OutcomeWarnings(PsychicRitualRoleAssignments assignments)
		{
			PsychicRitualDef_InvocationCircle.<OutcomeWarnings>d__33 <OutcomeWarnings>d__ = new PsychicRitualDef_InvocationCircle.<OutcomeWarnings>d__33(-2);
			<OutcomeWarnings>d__.<>4__this = this;
			<OutcomeWarnings>d__.<>3__assignments = assignments;
			return <OutcomeWarnings>d__;
		}

		// Token: 0x06005B69 RID: 23401 RVA: 0x002197D8 File Offset: 0x002179D8
		public override TaggedString TimeAndOfferingLabel()
		{
			if (this.timeAndOfferingLabelCached != null)
			{
				return this.timeAndOfferingLabelCached;
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.DurationLabel());
			stringBuilder.Append(this.CooldownLabel);
			if (!this.OfferingLabel().NullOrEmpty())
			{
				stringBuilder.AppendLine();
				stringBuilder.Append(this.OfferingLabel());
			}
			this.timeAndOfferingLabelCached = stringBuilder.ToString();
			return this.timeAndOfferingLabelCached;
		}

		// Token: 0x06005B6A RID: 23402 RVA: 0x00219864 File Offset: 0x00217A64
		private TaggedString OfferingLabel()
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (this.RequiredOffering != null)
			{
				stringBuilder.Append("PsychicRitualRequiredOffering".Translate().CapitalizeFirst());
				stringBuilder.Append(": ");
				stringBuilder.Append(this.RequiredOffering.SummaryFilterFirst);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06005B6B RID: 23403 RVA: 0x002198C8 File Offset: 0x00217AC8
		public TaggedString DurationLabel()
		{
			string value = ((int)(this.hoursUntilOutcome.Average * 2500f)).ToStringTicksToPeriod(true, false, true, true, false);
			TaggedString taggedString;
			if (this.hoursUntilOutcome.min == this.hoursUntilOutcome.max)
			{
				taggedString = "PsychicRitualExpectedDurationLabel".Translate().CapitalizeFirst();
			}
			else
			{
				taggedString = "ExpectedLordJobDuration".Translate().CapitalizeFirst();
			}
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(taggedString);
			stringBuilder.Append(": ");
			stringBuilder.Append(value);
			return stringBuilder.ToString();
		}

		// Token: 0x06005B6C RID: 23404 RVA: 0x00219964 File Offset: 0x00217B64
		private IReadOnlyDictionary<PsychicRitualRoleDef, List<IntVec3>> GenerateRolePositions(PsychicRitualRoleAssignments assignments)
		{
			PsychicRitualDef_InvocationCircle.tmpParticipants.ClearAndPoolValueLists<PsychicRitualRoleDef, IntVec3>();
			foreach (PsychicRitualRoleDef key in this.Roles)
			{
				PsychicRitualDef_InvocationCircle.tmpParticipants[key] = SimplePool<List<IntVec3>>.Get();
			}
			int num = assignments.RoleAssignedCount(this.ChanterRole) + assignments.RoleAssignedCount(this.InvokerRole);
			int num2 = 0;
			foreach (Pawn pawn in assignments.AssignedPawns(this.InvokerRole))
			{
				int num3 = 0;
				IntVec3 intVec;
				do
				{
					intVec = assignments.Target.Cell;
					intVec += IntVec3.FromPolar(360f * (float)num2++ / (float)num, this.invocationCircleRadius);
				}
				while (!intVec.Walkable(assignments.Target.Map) && num3++ <= 10);
				if (num3 >= 10)
				{
					intVec = assignments.Target.Cell;
				}
				PsychicRitualDef_InvocationCircle.tmpParticipants[this.InvokerRole].Add(intVec);
			}
			foreach (Pawn pawn2 in assignments.AssignedPawns(this.ChanterRole))
			{
				IntVec3 intVec2 = assignments.Target.Cell;
				intVec2 += IntVec3.FromPolar(360f * (float)num2++ / (float)num, this.invocationCircleRadius);
				PsychicRitualDef_InvocationCircle.tmpParticipants[this.ChanterRole].Add(intVec2);
			}
			foreach (Pawn pawn3 in assignments.AssignedPawns(this.TargetRole))
			{
				PsychicRitualDef_InvocationCircle.tmpParticipants[this.TargetRole].Add(assignments.Target.Cell);
			}
			if (this.DefenderRole != null)
			{
				num2 = 0;
				int num4 = assignments.RoleAssignedCount(this.DefenderRole);
				bool playerRitual = assignments.AllAssignedPawns.Any((Pawn x) => x.Faction == Faction.OfPlayer);
				foreach (Pawn pawn4 in assignments.AssignedPawns(this.DefenderRole))
				{
					IntVec3 intVec3 = assignments.Target.Cell;
					intVec3 += IntVec3.FromPolar(360f * (float)num2++ / (float)num4, this.invocationCircleRadius + 5f);
					intVec3 = this.GetBestStandableRolePosition(playerRitual, intVec3, assignments.Target.Cell, assignments.Target.Map, 8f);
					PsychicRitualDef_InvocationCircle.tmpParticipants[this.DefenderRole].Add(intVec3);
				}
			}
			return PsychicRitualDef_InvocationCircle.tmpParticipants;
		}

		// Token: 0x06005B6D RID: 23405 RVA: 0x00219CB8 File Offset: 0x00217EB8
		public override IEnumerable<string> BlockingIssues(PsychicRitualRoleAssignments assignments, Map map)
		{
			PsychicRitualDef_InvocationCircle.<BlockingIssues>d__40 <BlockingIssues>d__ = new PsychicRitualDef_InvocationCircle.<BlockingIssues>d__40(-2);
			<BlockingIssues>d__.<>4__this = this;
			<BlockingIssues>d__.<>3__assignments = assignments;
			<BlockingIssues>d__.<>3__map = map;
			return <BlockingIssues>d__;
		}

		// Token: 0x06005B6E RID: 23406 RVA: 0x00219CD8 File Offset: 0x00217ED8
		public override void CalculateMaxPower(PsychicRitualRoleAssignments assignments, List<QualityFactor> powerFactorsOut, out float power)
		{
			power = 0f;
			foreach (Pawn pawn in assignments.AssignedPawns(this.InvokerRole))
			{
				float statValue = pawn.GetStatValue(StatDefOf.PsychicSensitivity, true, -1);
				float num = PsychicRitualDef_InvocationCircle.PsychicSensitivityToPowerFactor.Evaluate(statValue);
				num *= this.psychicSensitivityPowerFactor;
				if (powerFactorsOut != null)
				{
					powerFactorsOut.Add(new QualityFactor
					{
						label = "PsychicRitualDef_InvocationCircle_QualityFactor_PsychicSensitivity".Translate(pawn.Named("PAWN")),
						positive = (statValue >= 1f),
						count = statValue.ToStringPercent(),
						quality = num,
						toolTip = "PsychicRitualDef_InvocationCircle_QualityFactor_PsychicSensitivity_Tooltip".Translate(pawn.Named("PAWN"))
					});
				}
				power += num;
			}
			float num2;
			base.CalculateMaxPower(assignments, powerFactorsOut, out num2);
			power += num2;
			Building building = assignments.Target.Thing as Building;
			if (building != null)
			{
				PsychicRitualDef_InvocationCircle.CalculateFacilityQualityOffset(powerFactorsOut, ref power, building);
			}
			power = Mathf.Clamp01(power);
		}

		// Token: 0x06005B6F RID: 23407 RVA: 0x00219E10 File Offset: 0x00218010
		private static void CalculateFacilityQualityOffset(List<QualityFactor> powerFactorsOut, ref float power, Building building)
		{
			Dictionary<ThingDef, PsychicRitualDef_InvocationCircle.RitualQualityOffsetCount> dictionary = new Dictionary<ThingDef, PsychicRitualDef_InvocationCircle.RitualQualityOffsetCount>();
			List<Thing> linkedFacilitiesListForReading = building.GetComp<CompAffectedByFacilities>().LinkedFacilitiesListForReading;
			for (int i = 0; i < linkedFacilitiesListForReading.Count; i++)
			{
				Thing thing = linkedFacilitiesListForReading[i];
				CompFacility compFacility = thing.TryGetComp<CompFacility>();
				if (((compFacility != null) ? compFacility.StatOffsets : null) != null)
				{
					for (int j = 0; j < compFacility.StatOffsets.Count; j++)
					{
						StatModifier statModifier = compFacility.StatOffsets[j];
						if (statModifier.stat == StatDefOf.PsychicRitualQuality)
						{
							PsychicRitualDef_InvocationCircle.RitualQualityOffsetCount ritualQualityOffsetCount;
							if (dictionary.TryGetValue(thing.def, out ritualQualityOffsetCount))
							{
								ritualQualityOffsetCount.count++;
								ritualQualityOffsetCount.offset += statModifier.value;
							}
							else
							{
								dictionary.Add(thing.def, new PsychicRitualDef_InvocationCircle.RitualQualityOffsetCount(1, statModifier.value));
							}
						}
					}
				}
			}
			foreach (KeyValuePair<ThingDef, PsychicRitualDef_InvocationCircle.RitualQualityOffsetCount> keyValuePair in dictionary)
			{
				if (powerFactorsOut != null)
				{
					powerFactorsOut.Add(new QualityFactor
					{
						label = Find.ActiveLanguageWorker.Pluralize(keyValuePair.Key.label, -1).CapitalizeFirst(),
						positive = true,
						count = keyValuePair.Value.count.ToString() + " / " + keyValuePair.Key.GetCompProperties<CompProperties_Facility>().maxSimultaneous.ToString(),
						quality = keyValuePair.Value.offset,
						toolTip = "PsychicRitualDef_InvocationCircle_QualityFactor_Increase_Tooltip".Translate().CapitalizeFirst().EndWithPeriod()
					});
				}
				power += keyValuePair.Value.offset;
			}
		}

		// Token: 0x06005B70 RID: 23408 RVA: 0x00219FF4 File Offset: 0x002181F4
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats(StatRequest req)
		{
			PsychicRitualDef_InvocationCircle.<SpecialDisplayStats>d__43 <SpecialDisplayStats>d__ = new PsychicRitualDef_InvocationCircle.<SpecialDisplayStats>d__43(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			<SpecialDisplayStats>d__.<>3__req = req;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x06005B71 RID: 23409 RVA: 0x0021A00C File Offset: 0x0021820C
		public override void CheckPsychicRitualCancelConditions(PsychicRitual psychicRitual)
		{
			base.CheckPsychicRitualCancelConditions(psychicRitual);
			if (psychicRitual.canceled)
			{
				return;
			}
			if (this.invokerRole != null)
			{
				Pawn pawn = psychicRitual.assignments.FirstAssignedPawn(this.InvokerRole);
				if (pawn != null && pawn.DeadOrDowned)
				{
					psychicRitual.CancelPsychicRitual("PsychicRitualDef_InvocationCircle_InvokerLost".Translate(pawn.Named("PAWN")));
				}
			}
		}

		// Token: 0x04003452 RID: 13394
		public FloatRange hoursUntilHoraxEffect;

		// Token: 0x04003453 RID: 13395
		public FloatRange hoursUntilOutcome;

		// Token: 0x04003454 RID: 13396
		public float invocationCircleRadius = 3.9f;

		// Token: 0x04003455 RID: 13397
		[MustTranslate]
		public string outcomeDescription;

		// Token: 0x04003456 RID: 13398
		public float psychicSensitivityPowerFactor = 0.25f;

		// Token: 0x04003457 RID: 13399
		protected PsychicRitualRoleDef invokerRole;

		// Token: 0x04003458 RID: 13400
		protected PsychicRitualRoleDef chanterRole;

		// Token: 0x04003459 RID: 13401
		protected PsychicRitualRoleDef targetRole;

		// Token: 0x0400345A RID: 13402
		protected PsychicRitualRoleDef defenderRole;

		// Token: 0x0400345B RID: 13403
		protected IngredientCount requiredOffering;

		// Token: 0x0400345C RID: 13404
		protected string timeAndOfferingLabelCached;

		// Token: 0x0400345D RID: 13405
		public static readonly SimpleCurve PsychicSensitivityToPowerFactor = new SimpleCurve
		{
			{
				new CurvePoint(0f, 0f),
				true
			},
			{
				new CurvePoint(1f, 0.5f),
				true
			},
			{
				new CurvePoint(2f, 0.9f),
				true
			},
			{
				new CurvePoint(3f, 1f),
				true
			}
		};

		// Token: 0x0400345E RID: 13406
		protected const int DurationTicksWaitPostEffect = 120;

		// Token: 0x0400345F RID: 13407
		private static Dictionary<PsychicRitualRoleDef, List<IntVec3>> tmpParticipants = new Dictionary<PsychicRitualRoleDef, List<IntVec3>>(8);

		// Token: 0x04003460 RID: 13408
		private List<Pawn> tmpGatheringPawns = new List<Pawn>(8);

		// Token: 0x02002E86 RID: 11910
		public enum InvalidTargetReasonEnum
		{
			// Token: 0x0400BC6E RID: 48238
			None,
			// Token: 0x0400BC6F RID: 48239
			AreaNotClear
		}

		// Token: 0x02002E87 RID: 11911
		private class RitualQualityOffsetCount
		{
			// Token: 0x06011961 RID: 72033 RVA: 0x00609B27 File Offset: 0x00607D27
			public RitualQualityOffsetCount(int count, float offset)
			{
				this.count = count;
				this.offset = offset;
			}

			// Token: 0x0400BC70 RID: 48240
			public float offset;

			// Token: 0x0400BC71 RID: 48241
			public int count;
		}
	}
}
