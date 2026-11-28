using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000283 RID: 643
	public static class GasUtility
	{
		// Token: 0x060012E2 RID: 4834 RVA: 0x0006B960 File Offset: 0x00069B60
		public static string GetLabel(this GasType gasType)
		{
			if (gasType <= GasType.ToxGas)
			{
				if (gasType == GasType.BlindSmoke)
				{
					return "BlindSmoke".Translate();
				}
				if (gasType == GasType.ToxGas)
				{
					return "ToxGas".Translate();
				}
			}
			else
			{
				if (gasType == GasType.RotStink)
				{
					return "RotStink".Translate();
				}
				if (gasType == GasType.DeadlifeDust)
				{
					return "DeadlifeDust".Translate();
				}
			}
			Log.ErrorOnce("Trying to get unknown gas type label.", 172091);
			return string.Empty;
		}

		// Token: 0x060012E3 RID: 4835 RVA: 0x0006B9DC File Offset: 0x00069BDC
		public static void AddGas(IntVec3 cell, Map map, GasType gasType, float radius)
		{
			int num = GenRadial.NumCellsInRadius(radius);
			map.gasGrid.AddGas(cell, gasType, 255 * num, true);
		}

		// Token: 0x060012E4 RID: 4836 RVA: 0x0006BA05 File Offset: 0x00069C05
		public static void AddGas(IntVec3 cell, Map map, GasType gasType, int amount)
		{
			map.gasGrid.AddGas(cell, gasType, amount, true);
		}

		// Token: 0x060012E5 RID: 4837 RVA: 0x0006BA16 File Offset: 0x00069C16
		public static void AddDeadifeGas(IntVec3 cell, Map map, Faction faction, int amount)
		{
			if (!ModsConfig.AnomalyActive)
			{
				return;
			}
			GasUtility.MarkDeadlifeCorpsesForFaction(cell, map, faction, amount);
			map.gasGrid.AddGas(cell, GasType.DeadlifeDust, amount, true);
		}

		// Token: 0x060012E6 RID: 4838 RVA: 0x0006BA3C File Offset: 0x00069C3C
		public static void MarkDeadlifeCorpsesForFaction(IntVec3 cell, Map map, Faction faction, int amount)
		{
			map.gasGrid.EstimateGasDiffusion(cell, GasType.DeadlifeDust, amount, delegate(IntVec3 c)
			{
				foreach (Thing thing in c.GetThingList(map))
				{
					Corpse corpse = thing as Corpse;
					if (corpse != null)
					{
						corpse.InnerPawn.MarkDeadlifeDustForFaction(faction);
					}
				}
			});
		}

		// Token: 0x060012E7 RID: 4839 RVA: 0x0006BA7D File Offset: 0x00069C7D
		public static byte GasDensity(this IntVec3 cell, Map map, GasType gasType)
		{
			return map.gasGrid.DensityAt(cell, gasType);
		}

		// Token: 0x060012E8 RID: 4840 RVA: 0x0006BA8C File Offset: 0x00069C8C
		public static bool AnyGas(this IntVec3 cell, Map map, GasType gasType)
		{
			return map.gasGrid.DensityAt(cell, gasType) > 0;
		}

		// Token: 0x060012E9 RID: 4841 RVA: 0x0006BAA0 File Offset: 0x00069CA0
		public static int RotStinkToSpawnForCorpse(Corpse corpse)
		{
			if (GenTemperature.RotRateAtTemperature((float)Mathf.RoundToInt(corpse.AmbientTemperature)) <= 0f)
			{
				return 0;
			}
			if (corpse.GetRotStage() == RotStage.Rotting)
			{
				float num = corpse.InnerPawn.BodySize;
				if (corpse.InnerPawn.RaceProps.Humanlike)
				{
					num *= 1.15f;
				}
				return Mathf.CeilToInt(num * 52f);
			}
			return 0;
		}

		// Token: 0x060012EA RID: 4842 RVA: 0x0006BB04 File Offset: 0x00069D04
		public static void PawnGasEffectsTickInterval(Pawn pawn, int delta)
		{
			if (!pawn.Spawned || !pawn.IsHashIntervalTick(50, delta))
			{
				return;
			}
			if (pawn.Position.GasDensity(pawn.Map, GasType.RotStink) > 0 && (pawn.RaceProps.Animal || pawn.RaceProps.Humanlike) && (!pawn.IsMutant || !pawn.mutant.Def.isImmuneToInfections) && !pawn.RaceProps.isImmuneToInfections && !pawn.health.hediffSet.HasHediff(HediffDefOf.LungRotExposure, false) && GasUtility.GetLungRotAffectedBodyParts(pawn).Any<BodyPartRecord>())
			{
				pawn.health.AddHediff(HediffDefOf.LungRotExposure, null, null, null);
			}
			if (ModsConfig.BiotechActive && pawn.Spawned)
			{
				byte b = pawn.Position.GasDensity(pawn.Map, GasType.ToxGas);
				if (b > 0)
				{
					float num = (float)b / 255f;
					Hediff firstHediffOfDef = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.ToxicBuildup, false);
					if (firstHediffOfDef != null && firstHediffOfDef.CurStageIndex == firstHediffOfDef.def.stages.Count - 1)
					{
						num *= 0.25f;
					}
					if (GasUtility.ShouldGetGasExposureHediff(pawn))
					{
						pawn.health.AddHediff(HediffDefOf.ToxGasExposure, null, null, null);
					}
					ToxicUtility.DoPawnToxicDamage(pawn, num);
				}
			}
			if (ModsConfig.AnomalyActive && pawn.Spawned && pawn.Position.GasDensity(pawn.Map, GasType.DeadlifeDust) > 0 && pawn.IsShambler)
			{
				Hediff firstHediffOfDef2 = pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Shambler, false);
				HediffComp_DisappearsAndKills hediffComp_DisappearsAndKills = (firstHediffOfDef2 != null) ? firstHediffOfDef2.TryGetComp<HediffComp_DisappearsAndKills>() : null;
				if (hediffComp_DisappearsAndKills != null)
				{
					hediffComp_DisappearsAndKills.ticksToDisappear = Mathf.Max(hediffComp_DisappearsAndKills.ticksToDisappear, 15000);
				}
			}
		}

		// Token: 0x060012EB RID: 4843 RVA: 0x0006BCCC File Offset: 0x00069ECC
		public static void CorpseGasEffectsTickRare(Corpse corpse)
		{
			if (!corpse.Spawned)
			{
				return;
			}
			if (corpse.GetRotStage() == RotStage.Rotting)
			{
				int num = GasUtility.RotStinkToSpawnForCorpse(corpse);
				if (num > 0)
				{
					GasUtility.AddGas(corpse.Position, corpse.Map, GasType.RotStink, num);
				}
			}
			if (ModsConfig.AnomalyActive && corpse.Position.GasDensity(corpse.Map, GasType.DeadlifeDust) > 0 && !corpse.InnerPawn.health.hediffSet.HasHediff(HediffDefOf.Shambler, false) && MutantUtility.CanResurrectAsShambler(corpse, true))
			{
				MutantUtility.ResurrectAsShambler(corpse.InnerPawn, 15000, corpse.InnerPawn.DeadlifeDustFaction);
			}
		}

		// Token: 0x060012EC RID: 4844 RVA: 0x0006BD68 File Offset: 0x00069F68
		public static void DoSteadyEffects(IntVec3 cell, Map map)
		{
			if (ModsConfig.AnomalyActive && cell.GasDensity(map, GasType.DeadlifeDust) > 0)
			{
				GasUtility.tempThingList.Clear();
				GasUtility.tempThingList.AddRange(cell.GetThingList(map));
				foreach (Thing thing in GasUtility.tempThingList)
				{
					Building_Grave building_Grave = thing as Building_Grave;
					if (building_Grave != null && building_Grave.Corpse != null && MutantUtility.CanResurrectAsShambler(building_Grave.Corpse, true))
					{
						Corpse corpse = building_Grave.Corpse;
						building_Grave.EjectContents();
						MutantUtility.ResurrectAsShambler(corpse.InnerPawn, 15000, corpse.InnerPawn.DeadlifeDustFaction);
					}
				}
			}
		}

		// Token: 0x060012ED RID: 4845 RVA: 0x0006BE30 File Offset: 0x0006A030
		public static IEnumerable<BodyPartRecord> GetLungRotAffectedBodyParts(Pawn pawn)
		{
			return from p in pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null)
			where p.def == BodyPartDefOf.Lung && !pawn.health.hediffSet.hediffs.Any((Hediff x) => x.Part == p && x.def.preventsLungRot)
			select p;
		}

		// Token: 0x060012EE RID: 4846 RVA: 0x0006BE74 File Offset: 0x0006A074
		private static bool ShouldGetGasExposureHediff(Pawn pawn)
		{
			return GasUtility.IsAffectedByExposure(pawn) && !pawn.health.hediffSet.HasHediff(HediffDefOf.ToxGasExposure, false);
		}

		// Token: 0x060012EF RID: 4847 RVA: 0x0006BE9C File Offset: 0x0006A09C
		public static bool IsAffectedByExposure(Pawn pawn)
		{
			if (pawn.RaceProps.Humanlike || pawn.RaceProps.Humanlike)
			{
				if (pawn.apparel != null)
				{
					using (List<Apparel>.Enumerator enumerator = pawn.apparel.WornApparel.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.def.apparel.immuneToToxGasExposure)
							{
								return false;
							}
						}
					}
				}
				if (pawn.genes != null)
				{
					using (List<Gene>.Enumerator enumerator2 = pawn.genes.GenesListForReading.GetEnumerator())
					{
						while (enumerator2.MoveNext())
						{
							if (enumerator2.Current.def.immuneToToxGasExposure)
							{
								return false;
							}
						}
					}
				}
				return true;
			}
			return false;
		}

		// Token: 0x040010D8 RID: 4312
		public const float BlindingGasAccuracyPenalty = 0.7f;

		// Token: 0x040010D9 RID: 4313
		public const int RotStinkPerRawMeatRotting = 2;

		// Token: 0x040010DA RID: 4314
		private const float RotStinkPerBodySize = 52f;

		// Token: 0x040010DB RID: 4315
		private const float RotStinkHumanlikeFactor = 1.15f;

		// Token: 0x040010DC RID: 4316
		private const int GasCheckInterval = 50;

		// Token: 0x040010DD RID: 4317
		private const float ToxGasEffectOnExtremeBuildupFactor = 0.25f;

		// Token: 0x040010DE RID: 4318
		private const int DeadlifeShamblerLifespanTicks = 15000;

		// Token: 0x040010DF RID: 4319
		private static List<Thing> tempThingList = new List<Thing>();
	}
}
