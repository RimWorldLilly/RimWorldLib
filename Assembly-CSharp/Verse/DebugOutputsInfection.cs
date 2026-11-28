using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using LudeonTK;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x020005CC RID: 1484
	public static class DebugOutputsInfection
	{
		// Token: 0x06002EC1 RID: 11969 RVA: 0x00124690 File Offset: 0x00122890
		private static List<Pawn> GenerateDoctorArray()
		{
			PawnGenerationRequest request = new PawnGenerationRequest(PawnKindDefOf.Colonist, Faction.OfPlayer, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, (Pawn p) => !p.WorkTypeIsDisabled(WorkTypeDefOf.Doctor) && p.health.hediffSet.hediffs.Count == 0, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false);
			List<Pawn> list = new List<Pawn>();
			for (int i = 0; i <= 20; i++)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(request);
				pawn.skills.GetSkill(SkillDefOf.Medicine).Level = i;
				list.Add(pawn);
			}
			return list;
		}

		// Token: 0x06002EC2 RID: 11970 RVA: 0x00124790 File Offset: 0x00122990
		private static IEnumerable<HediffDef> InfectionList()
		{
			return from hediff in DefDatabase<HediffDef>.AllDefs
			where hediff.tendable && hediff.HasComp(typeof(HediffComp_TendDuration)) && hediff.HasComp(typeof(HediffComp_Immunizable)) && hediff.lethalSeverity > 0f
			select hediff;
		}

		// Token: 0x06002EC3 RID: 11971 RVA: 0x001247BC File Offset: 0x001229BC
		[DebugOutput]
		public static void Infections()
		{
			DebugOutputsInfection.<>c__DisplayClass3_0 CS$<>8__locals1 = new DebugOutputsInfection.<>c__DisplayClass3_0();
			CS$<>8__locals1.ilc = delegate(DebugOutputsInfection.InfectionLuck il)
			{
				float result = 1f;
				if (il == DebugOutputsInfection.InfectionLuck.Bad)
				{
					result = 0.8f;
				}
				if (il == DebugOutputsInfection.InfectionLuck.Good)
				{
					result = 1.2f;
				}
				return result;
			};
			CS$<>8__locals1.stringizeWithLuck = ((Func<DebugOutputsInfection.InfectionLuck, float> func) => string.Format("{0:F2} / {1:F2}", func(DebugOutputsInfection.InfectionLuck.Bad), func(DebugOutputsInfection.InfectionLuck.Good)));
			CS$<>8__locals1.baseImmunityIncrease = ((HediffDef d, DebugOutputsInfection.InfectionLuck il) => d.CompProps<HediffCompProperties_Immunizable>().immunityPerDaySick * CS$<>8__locals1.ilc(il));
			CS$<>8__locals1.tendedSeverityIncrease = ((HediffDef d, float tend) => DebugOutputsInfection.<Infections>g__baseSeverityIncrease|3_3(d) + d.CompProps<HediffCompProperties_TendDuration>().severityPerDayTended * tend);
			CS$<>8__locals1.immunityIncrease = delegate(HediffDef d, DebugOutputsInfection.InfectionLuck il, bool bedridden)
			{
				float b = DebugOutputsInfection.<Infections>g__isAnimal|3_2(d) ? 1f : ThingDefOf.Bed.GetStatValueAbstract(StatDefOf.ImmunityGainSpeedFactor, null);
				float num = Mathf.Lerp(1f, b, bedridden ? 1f : 0.3f) * StatDefOf.ImmunityGainSpeed.GetStatPart<StatPart_Resting>().factor;
				return CS$<>8__locals1.baseImmunityIncrease(d, il) * num;
			};
			CS$<>8__locals1.immunityOnLethality = delegate(HediffDef d, DebugOutputsInfection.InfectionLuck il, float tend)
			{
				if (CS$<>8__locals1.tendedSeverityIncrease(d, tend) <= 0f)
				{
					return float.PositiveInfinity;
				}
				return d.lethalSeverity / CS$<>8__locals1.tendedSeverityIncrease(d, tend) * CS$<>8__locals1.immunityIncrease(d, il, true);
			};
			List<TableDataGetter<HediffDef>> list = new List<TableDataGetter<HediffDef>>();
			list.Add(new TableDataGetter<HediffDef>("defName", (HediffDef d) => d.defName + (d.stages.Any((HediffStage stage) => stage.capMods.Any((PawnCapacityModifier cap) => cap.capacity == PawnCapacityDefOf.BloodFiltration)) ? " (inaccurate)" : "")));
			list.Add(new TableDataGetter<HediffDef>("lethal\nseverity", (HediffDef d) => d.lethalSeverity.ToString("F2")));
			list.Add(new TableDataGetter<HediffDef>("base\nseverity\nincrease", (HediffDef d) => DebugOutputsInfection.<Infections>g__baseSeverityIncrease|3_3(d).ToString("F2")));
			list.Add(new TableDataGetter<HediffDef>("base\nimmunity\nincrease", (HediffDef d) => CS$<>8__locals1.stringizeWithLuck((DebugOutputsInfection.InfectionLuck il) => CS$<>8__locals1.baseImmunityIncrease(d, il))));
			List<Pawn> source = DebugOutputsInfection.GenerateDoctorArray();
			float tendquality;
			for (tendquality = 0f; tendquality <= 1.01f; tendquality += 0.1f)
			{
				tendquality = Mathf.Clamp01(tendquality);
				Pawn arg = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, null) >= Mathf.Clamp01(tendquality - 0.25f), null);
				Pawn arg2 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineHerbal) >= Mathf.Clamp01(tendquality - 0.25f), null);
				Pawn arg3 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineIndustrial) >= Mathf.Clamp01(tendquality - 0.25f), null);
				Pawn arg4 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineUltratech) >= Mathf.Clamp01(tendquality - 0.25f), null);
				Pawn arg5 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, null) >= tendquality, null);
				Pawn arg6 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineHerbal) >= tendquality, null);
				Pawn arg7 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineIndustrial) >= tendquality, null);
				Pawn arg8 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineUltratech) >= tendquality, null);
				Pawn arg9 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, null) >= Mathf.Clamp01(tendquality + 0.25f), null);
				Pawn arg10 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineHerbal) >= Mathf.Clamp01(tendquality + 0.25f), null);
				Pawn arg11 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineIndustrial) >= Mathf.Clamp01(tendquality + 0.25f), null);
				Pawn arg12 = source.FirstOrFallback((Pawn doc) => TendUtility.CalculateBaseTendQuality(doc, null, ThingDefOf.MedicineUltratech) >= Mathf.Clamp01(tendquality + 0.25f), null);
				Func<Pawn, Pawn, Pawn, string> func2 = delegate(Pawn low, Pawn exp, Pawn high)
				{
					string arg13 = (low != null) ? low.skills.GetSkill(SkillDefOf.Medicine).Level.ToString() : "X";
					string arg14 = (exp != null) ? exp.skills.GetSkill(SkillDefOf.Medicine).Level.ToString() : "X";
					string arg15 = (high != null) ? high.skills.GetSkill(SkillDefOf.Medicine).Level.ToString() : "X";
					return string.Format("{0}-{1}-{2}", arg13, arg14, arg15);
				};
				string text = func2(arg, arg5, arg9);
				string text2 = func2(arg2, arg6, arg10);
				string text3 = func2(arg3, arg7, arg11);
				string text4 = func2(arg4, arg8, arg12);
				float tq = tendquality;
				list.Add(new TableDataGetter<HediffDef>(string.Format("survival chance at\ntend quality {0}\n\ndoc skill needed:\nno meds:  {1}\nherbal:  {2}\nnormal:  {3}\nglitter:  {4}", new object[]
				{
					tq.ToStringPercent(),
					text,
					text2,
					text3,
					text4
				}), delegate(HediffDef d)
				{
					float num = CS$<>8__locals1.immunityOnLethality(d, DebugOutputsInfection.InfectionLuck.Bad, tq);
					float num2 = CS$<>8__locals1.immunityOnLethality(d, DebugOutputsInfection.InfectionLuck.Good, tq);
					if (num == float.PositiveInfinity)
					{
						return float.PositiveInfinity.ToString();
					}
					return Mathf.Clamp01((num2 - 1f) / (num2 - num)).ToStringPercent();
				}));
			}
			DebugTables.MakeTablesDialog<HediffDef>(DebugOutputsInfection.InfectionList(), list.ToArray());
		}

		// Token: 0x06002EC4 RID: 11972 RVA: 0x00124B9C File Offset: 0x00122D9C
		[DebugOutput]
		public static void InfectionSimulator()
		{
			LongEventHandler.QueueLongEvent(DebugOutputsInfection.InfectionSimulatorWorker(), "Simulating . . .", null, true, false);
		}

		// Token: 0x06002EC5 RID: 11973 RVA: 0x00124BB0 File Offset: 0x00122DB0
		private static IEnumerable InfectionSimulatorWorker()
		{
			return new DebugOutputsInfection.<InfectionSimulatorWorker>d__6(-2);
		}

		// Token: 0x06002EC6 RID: 11974 RVA: 0x00124BB9 File Offset: 0x00122DB9
		[CompilerGenerated]
		internal static bool <Infections>g__isAnimal|3_2(HediffDef d)
		{
			return d.defName.Contains("Animal");
		}

		// Token: 0x06002EC7 RID: 11975 RVA: 0x00124BCB File Offset: 0x00122DCB
		[CompilerGenerated]
		internal static float <Infections>g__baseSeverityIncrease|3_3(HediffDef d)
		{
			return d.CompProps<HediffCompProperties_Immunizable>().severityPerDayNotImmune;
		}

		// Token: 0x0200292D RID: 10541
		private enum InfectionLuck
		{
			// Token: 0x0400A7D0 RID: 42960
			Bad,
			// Token: 0x0400A7D1 RID: 42961
			Normal,
			// Token: 0x0400A7D2 RID: 42962
			Good
		}

		// Token: 0x0200292E RID: 10542
		private struct InfectionSimRow
		{
			// Token: 0x0400A7D3 RID: 42963
			public HediffDef illness;

			// Token: 0x0400A7D4 RID: 42964
			public int skill;

			// Token: 0x0400A7D5 RID: 42965
			public ThingDef medicine;

			// Token: 0x0400A7D6 RID: 42966
			public float deathChance;

			// Token: 0x0400A7D7 RID: 42967
			public float recoveryTimeDays;

			// Token: 0x0400A7D8 RID: 42968
			public float medicineUsed;
		}
	}
}
