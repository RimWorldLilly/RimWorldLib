using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000248 RID: 584
	public static class ContainmentUtility
	{
		// Token: 0x06001130 RID: 4400 RVA: 0x000628DC File Offset: 0x00060ADC
		public static bool SafeContainerExistsFor(Thing thing)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			using (List<Thing>.Enumerator enumerator = thing.MapHeld.listerThings.ThingsInGroup(ThingRequestGroup.EntityHolder).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (enumerator.Current.SafelyContains(thing))
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06001131 RID: 4401 RVA: 0x0006294C File Offset: 0x00060B4C
		public static bool SafelyContains(this Thing container, Thing thing)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			CompEntityHolder compEntityHolder = container.TryGetComp<CompEntityHolder>();
			return compEntityHolder != null && compEntityHolder.ContainmentStrength >= thing.GetStatValue(StatDefOf.MinimumContainmentStrength, true, -1);
		}

		// Token: 0x06001132 RID: 4402 RVA: 0x00062986 File Offset: 0x00060B86
		public static float GetEscapeIntervalMultiplier(Thing entity, CompEntityHolder entityHolder)
		{
			return ContainmentUtility.EscapeIntervalMultiplierFromContainmentStrengthCurve.Evaluate(entityHolder.ContainmentStrength - entity.GetStatValue(StatDefOf.MinimumContainmentStrength, true, -1));
		}

		// Token: 0x06001133 RID: 4403 RVA: 0x000629A6 File Offset: 0x00060BA6
		public static float GetStudyKnowledgeAmountMultiplier(Thing entity, CompEntityHolder entityHolder)
		{
			return ContainmentUtility.StudyKnowledgeMultiplierFromContainmentStrengthCurve.Evaluate(entityHolder.ContainmentStrength - entity.GetStatValue(StatDefOf.MinimumContainmentStrength, true, -1));
		}

		// Token: 0x06001134 RID: 4404 RVA: 0x000629C6 File Offset: 0x00060BC6
		public static IEnumerable<IntVec3> GetInhibitorAffectedCells(ThingDef def, IntVec3 center, Rot4 rot, Map map)
		{
			ContainmentUtility.<GetInhibitorAffectedCells>d__8 <GetInhibitorAffectedCells>d__ = new ContainmentUtility.<GetInhibitorAffectedCells>d__8(-2);
			<GetInhibitorAffectedCells>d__.<>3__def = def;
			<GetInhibitorAffectedCells>d__.<>3__center = center;
			<GetInhibitorAffectedCells>d__.<>3__rot = rot;
			<GetInhibitorAffectedCells>d__.<>3__map = map;
			return <GetInhibitorAffectedCells>d__;
		}

		// Token: 0x06001135 RID: 4405 RVA: 0x000629EB File Offset: 0x00060BEB
		public static IntVec3 GetLinearBuildingFrontCell(ThingDef def, IntVec3 center, Rot4 rot)
		{
			if (def.GetCompProperties<CompProperties_Facility>() == null)
			{
				return IntVec3.Invalid;
			}
			return center + IntVec3.South.RotatedBy(rot) * (def.size.z - 1);
		}

		// Token: 0x06001136 RID: 4406 RVA: 0x00062A20 File Offset: 0x00060C20
		public static bool IsLinearBuildingBlocked(ThingDef def, IntVec3 center, Rot4 rot, Map map, Thing thingToIgnore = null)
		{
			if (def.GetCompProperties<CompProperties_Facility>() == null)
			{
				return false;
			}
			Building edifice = ContainmentUtility.GetLinearBuildingFrontCell(def, center, rot).GetEdifice(map);
			return edifice != null && edifice != thingToIgnore;
		}

		// Token: 0x06001137 RID: 4407 RVA: 0x00062A54 File Offset: 0x00060C54
		public static bool ShowContainmentStats(Thing thing)
		{
			CompStudiable compStudiable = thing.TryGetComp<CompStudiable>();
			if (compStudiable != null && thing is Pawn)
			{
				return compStudiable.RequiresHoldingPlatform;
			}
			CompHoldingPlatformTarget compHoldingPlatformTarget = thing.TryGetComp<CompHoldingPlatformTarget>();
			return ((compHoldingPlatformTarget != null) ? compHoldingPlatformTarget.Props.heldPawnKind : null) != null;
		}

		// Token: 0x06001138 RID: 4408 RVA: 0x00062A98 File Offset: 0x00060C98
		public static bool TryGetColdContainmentBonus(Thing thing, out float modifier)
		{
			CompHoldingPlatformTarget compHoldingPlatformTarget;
			if (thing.TryGetComp(out compHoldingPlatformTarget) && compHoldingPlatformTarget.Props.getsColdContainmentBonus)
			{
				modifier = ContainmentUtility.ColdEscapeFactorCurve.Evaluate(thing.AmbientTemperature);
				return true;
			}
			modifier = 0f;
			return false;
		}

		// Token: 0x06001139 RID: 4409 RVA: 0x00062AD8 File Offset: 0x00060CD8
		public static float InitiateEscapeMtbDays(Pawn pawn, StringBuilder sb = null)
		{
			if (!ContainmentUtility.CanParticipateInEscape(pawn, sb))
			{
				return -1f;
			}
			CompHoldingPlatformTarget comp = pawn.GetComp<CompHoldingPlatformTarget>();
			float num = comp.Props.baseEscapeIntervalMtbDays;
			pawn.GetRoom(RegionType.Set_All);
			float num2 = 1f / Mathf.Clamp(pawn.health.capacities.GetLevel(PawnCapacityDefOf.Moving), 0.01f, 1f);
			num *= num2;
			if (sb != null && num2 > 1f)
			{
				sb.AppendLineIfNotEmpty();
				sb.Append("  - " + "FactorForMovement".Translate() + ": x" + num2.ToStringPercent());
			}
			float x = (float)pawn.mindState.entityTicksInCaptivity / 60000f;
			float num3 = ContainmentUtility.DaysInCaptivityEscapeFactorCurve.Evaluate(x);
			num *= num3;
			if (num3 > 1f && sb != null)
			{
				sb.AppendLineIfNotEmpty();
				sb.Append("  - " + "FactorForDaysInCaptivity".Translate() + ": x" + num3.ToStringPercent());
			}
			float escapeIntervalMultiplier = ContainmentUtility.GetEscapeIntervalMultiplier(pawn, comp.HeldPlatform.GetComp<CompEntityHolder>());
			num *= escapeIntervalMultiplier;
			if (sb != null)
			{
				sb.AppendLineIfNotEmpty();
				sb.Append("  - " + "FactorContainmentStrength".Translate() + ": x" + escapeIntervalMultiplier.ToStringPercent());
			}
			if (comp.Props.getsColdContainmentBonus)
			{
				float num4 = ContainmentUtility.ColdEscapeFactorCurve.Evaluate(pawn.AmbientTemperature);
				num *= num4;
				if (num4 > 1f && sb != null)
				{
					sb.AppendLineIfNotEmpty();
					sb.Append("  - " + "FactorForColdCaptivity".Translate() + ": x" + num4.ToStringPercent());
				}
			}
			return num;
		}

		// Token: 0x0600113A RID: 4410 RVA: 0x00062CBC File Offset: 0x00060EBC
		public static bool CanParticipateInEscape(Pawn pawn, StringBuilder sb = null)
		{
			if (pawn.Downed)
			{
				if (sb != null)
				{
					sb.AppendLineIfNotEmpty();
					sb.Append("  - " + "EntityDowned".Translate() + ": x0%");
				}
				return false;
			}
			CompHoldingPlatformTarget comp = pawn.GetComp<CompHoldingPlatformTarget>();
			if (comp == null)
			{
				return false;
			}
			if (!comp.CurrentlyHeldOnPlatform)
			{
				return false;
			}
			if (comp.Props.baseEscapeIntervalMtbDays <= 0f)
			{
				if (sb != null)
				{
					sb.AppendLineIfNotEmpty();
					sb.Append("  - " + "IncapableOfEscaping".Translate() + ": x0%");
				}
				return false;
			}
			return true;
		}

		// Token: 0x04000FBF RID: 4031
		private static readonly SimpleCurve DaysInCaptivityEscapeFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(15f, 1f),
				true
			},
			{
				new CurvePoint(60f, 1.2f),
				true
			},
			{
				new CurvePoint(120f, 1.5f),
				true
			},
			{
				new CurvePoint(240f, 2f),
				true
			}
		};

		// Token: 0x04000FC0 RID: 4032
		private static readonly SimpleCurve ColdEscapeFactorCurve = new SimpleCurve
		{
			{
				new CurvePoint(-30f, 1.5f),
				true
			},
			{
				new CurvePoint(0f, 1f),
				true
			}
		};

		// Token: 0x04000FC1 RID: 4033
		private static readonly SimpleCurve EscapeIntervalMultiplierFromContainmentStrengthCurve = new SimpleCurve
		{
			{
				new CurvePoint(-0.001f, 0.05f),
				true
			},
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(100f, 3f),
				true
			},
			{
				new CurvePoint(200f, 8f),
				true
			}
		};

		// Token: 0x04000FC2 RID: 4034
		private static readonly SimpleCurve StudyKnowledgeMultiplierFromContainmentStrengthCurve = new SimpleCurve
		{
			{
				new CurvePoint(-0.001f, 0.5f),
				true
			},
			{
				new CurvePoint(0f, 1f),
				true
			},
			{
				new CurvePoint(100f, 1.3f),
				true
			},
			{
				new CurvePoint(200f, 1.5f),
				true
			}
		};
	}
}
