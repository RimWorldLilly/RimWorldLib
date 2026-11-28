using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02001561 RID: 5473
	public static class MutantUtility
	{
		// Token: 0x06008612 RID: 34322 RVA: 0x00307090 File Offset: 0x00305290
		public static bool CanResurrectAsShambler(Corpse corpse, bool ignoreIndoors = false)
		{
			if (((corpse != null) ? corpse.InnerPawn : null) == null)
			{
				return false;
			}
			if (!corpse.InnerPawn.RaceProps.IsFlesh)
			{
				return false;
			}
			if (!corpse.InnerPawn.RaceProps.canBecomeShambler)
			{
				return false;
			}
			if (corpse.InnerPawn.IsMutant)
			{
				return false;
			}
			if (corpse is UnnaturalCorpse)
			{
				return false;
			}
			Room room = corpse.PositionHeld.GetRoom(corpse.MapHeld);
			if (room != null && !ignoreIndoors && corpse.PositionHeld.Roofed(corpse.MapHeld) && (room.ProperRoom || room.IsDoorway))
			{
				return false;
			}
			if (!Find.Storyteller.difficulty.childShamblersAllowed && !corpse.InnerPawn.ageTracker.Adult)
			{
				return false;
			}
			Hediff_DeathRefusal firstHediff = corpse.InnerPawn.health.hediffSet.GetFirstHediff<Hediff_DeathRefusal>();
			return firstHediff == null || (!firstHediff.InProgress && firstHediff.UsesLeft <= 0);
		}

		// Token: 0x06008613 RID: 34323 RVA: 0x00307180 File Offset: 0x00305380
		public static void ResurrectAsShambler(Pawn pawn, int lifespanTicks = -1, Faction faction = null)
		{
			RotStage rotStage = pawn.Corpse.GetRotStage();
			pawn.mutant = new Pawn_MutantTracker(pawn, MutantDefOf.Shambler, rotStage);
			Hediff_Shambler hediff_Shambler = pawn.health.AddHediff(MutantDefOf.Shambler.hediff, null, null, null) as Hediff_Shambler;
			HediffComp_DisappearsAndKills hediffComp_DisappearsAndKills = hediff_Shambler.TryGetComp<HediffComp_DisappearsAndKills>();
			if (hediffComp_DisappearsAndKills != null)
			{
				if (lifespanTicks > 0)
				{
					hediffComp_DisappearsAndKills.disappearsAfterTicks = lifespanTicks;
					hediffComp_DisappearsAndKills.ticksToDisappear = lifespanTicks;
				}
				else
				{
					hediffComp_DisappearsAndKills.disabled = true;
				}
			}
			if (hediff_Shambler != null)
			{
				hediff_Shambler.StartRising(lifespanTicks);
			}
			if (faction == null && MutantDefOf.Shambler.defaultFaction != null)
			{
				faction = Find.FactionManager.FirstFactionOfDef(MutantDefOf.Shambler.defaultFaction);
			}
			if (faction != null && pawn.Faction != faction)
			{
				pawn.SetFaction(faction, null);
			}
		}

		// Token: 0x06008614 RID: 34324 RVA: 0x0030723C File Offset: 0x0030543C
		public static void SetFreshPawnAsMutant(Pawn pawn, MutantDef mutant)
		{
			RotStage rotStage = mutant.useCorpseGraphics ? Gen.RandomEnumValue<RotStage>(false) : RotStage.Fresh;
			Pawn_EquipmentTracker equipment = pawn.equipment;
			if (((equipment != null) ? equipment.Primary : null) != null)
			{
				pawn.equipment.DestroyEquipment(pawn.equipment.Primary);
			}
			if (ModsConfig.BiotechActive && pawn.genes != null && pawn.genes.Xenotype != XenotypeDefOf.Baseliner && rotStage == RotStage.Dessicated)
			{
				rotStage = RotStage.Fresh;
			}
			if (rotStage == RotStage.Dessicated)
			{
				Pawn_ApparelTracker apparel = pawn.apparel;
				if (apparel != null)
				{
					apparel.DestroyAll(DestroyMode.Vanish);
				}
			}
			MutantUtility.SetPawnAsMutantInstantly(pawn, mutant, rotStage);
		}

		// Token: 0x06008615 RID: 34325 RVA: 0x003072CC File Offset: 0x003054CC
		public static void SetPawnAsMutantInstantly(Pawn pawn, MutantDef mutant, RotStage rotStage = RotStage.Fresh)
		{
			pawn.mutant = new Pawn_MutantTracker(pawn, mutant, rotStage);
			pawn.mutant.Turn(true);
			if (mutant.defaultFaction != null)
			{
				Faction faction = Find.FactionManager.FirstFactionOfDef(mutant.defaultFaction);
				if (faction != null && pawn.Faction != faction)
				{
					pawn.SetFaction(faction, null);
				}
			}
			MutantUtility.RegenerateHealth(pawn);
		}

		// Token: 0x06008616 RID: 34326 RVA: 0x00307328 File Offset: 0x00305528
		public static Thing FindShamblerTarget(Pawn pawn)
		{
			TargetScanFlags flags = TargetScanFlags.NeedLOSToPawns | TargetScanFlags.NeedReachable | TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable;
			Thing thing = null;
			if (thing == null)
			{
				thing = (Thing)AttackTargetFinder.BestAttackTarget(pawn, flags, null, 0f, 20f, default(IntVec3), float.MaxValue, true, true, true, false);
			}
			return thing;
		}

		// Token: 0x06008617 RID: 34327 RVA: 0x0030736C File Offset: 0x0030556C
		public static void ActivateNearbyShamblers(Pawn pawn, Thing target)
		{
			if (!pawn.SpawnedOrAnyParentSpawned)
			{
				return;
			}
			foreach (Pawn pawn2 in pawn.MapHeld.mapPawns.SpawnedShamblers)
			{
				if (pawn.Faction == pawn2.Faction && pawn.Position.InHorDistOf(pawn2.Position, 10f))
				{
					Hediff_Shambler hediff_Shambler = pawn2.mutant.Hediff as Hediff_Shambler;
					if (hediff_Shambler != null)
					{
						hediff_Shambler.Notify_DelayedAlert(target);
					}
				}
			}
		}

		// Token: 0x06008618 RID: 34328 RVA: 0x00307410 File Offset: 0x00305610
		public static void RegenerateHealth(Pawn pawn)
		{
			if (!pawn.IsMutant || pawn.Dead)
			{
				return;
			}
			MutantDef def = pawn.mutant.Def;
			MutantUtility.tmpHediffs.Clear();
			MutantUtility.tmpHediffs.AddRange(pawn.health.hediffSet.hediffs);
			foreach (Hediff hediff in MutantUtility.tmpHediffs)
			{
				Hediff_Injury hediff_Injury = hediff as Hediff_Injury;
				Hediff_MissingPart hediff_MissingPart = hediff as Hediff_MissingPart;
				Hediff_Addiction hediff_Addiction = hediff as Hediff_Addiction;
				Hediff_ChemicalDependency hediff_ChemicalDependency = hediff as Hediff_ChemicalDependency;
				if (def.isImmuneToInfections && hediff.def.isInfection)
				{
					pawn.health.RemoveHediff(hediff);
				}
				if (def.removeChronicIllnesses && hediff.def.chronic)
				{
					pawn.health.RemoveHediff(hediff);
				}
				else if (def.removeAddictions && (hediff_Addiction != null || hediff_ChemicalDependency != null))
				{
					pawn.health.RemoveHediff(hediff);
				}
				else if (def.removeAllInjuries && hediff_Injury != null)
				{
					pawn.health.RemoveHediff(hediff);
				}
				else if (def.removePermanentInjuries && hediff_Injury != null && !hediff_Injury.IsPermanent())
				{
					pawn.health.RemoveHediff(hediff);
				}
				else if (def.removesHediffs.Contains(hediff.def))
				{
					pawn.health.RemoveHediff(hediff);
				}
				else if (def.restoreLegs && hediff_MissingPart != null && (hediff_MissingPart.Part.def.tags.Contains(BodyPartTagDefOf.MovingLimbCore) || hediff_MissingPart.Part.def.tags.Contains(BodyPartTagDefOf.MovingLimbSegment)) && (hediff_MissingPart.Part.parent == null || pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null).Contains(hediff_MissingPart.Part.parent)))
				{
					pawn.health.RestorePart(hediff_MissingPart.Part, null, true);
				}
			}
			MutantUtility.tmpHediffs.Clear();
		}

		// Token: 0x06008619 RID: 34329 RVA: 0x00307638 File Offset: 0x00305838
		public static void RestoreBodyParts(Pawn pawn)
		{
			foreach (Hediff hediff2 in (from hediff in pawn.health.hediffSet.hediffs
			where hediff is Hediff_MissingPart
			select hediff).ToList<Hediff>())
			{
				if (hediff2.Part.def.tags.Contains(BodyPartTagDefOf.MovingLimbCore) || !hediff2.Part.def.canSuggestAmputation)
				{
					pawn.health.RestorePart(hediff2.Part, null, true);
				}
				if (hediff2.Part.def == BodyPartDefOf.Arm && Rand.Bool)
				{
					pawn.health.RestorePart(hediff2.Part, null, true);
				}
			}
			pawn.health.RestorePart(pawn.health.hediffSet.GetBrain(), null, true);
		}

		// Token: 0x0600861A RID: 34330 RVA: 0x00307744 File Offset: 0x00305944
		public static void RestoreUntilNotDowned(Pawn pawn)
		{
			MutantUtility.tmpHediffs.Clear();
			MutantUtility.tmpHediffs.AddRange(pawn.health.hediffSet.hediffs);
			int num = 0;
			while (num < 300 && pawn.Downed)
			{
				num++;
				Hediff hediff = MutantUtility.tmpHediffs.RandomElement<Hediff>();
				if (hediff is Hediff_Injury)
				{
					hediff.Severity -= 1f;
					pawn.health.Notify_HediffChanged(hediff);
				}
			}
			MutantUtility.tmpHediffs.Clear();
			if (pawn.Downed)
			{
				MutantUtility.RestoreBodyParts(pawn);
			}
			if (pawn.Downed)
			{
				MutantUtility.RegenerateHealth(pawn);
			}
		}

		// Token: 0x0600861B RID: 34331 RVA: 0x003077E4 File Offset: 0x003059E4
		public static bool CheckShamblerHostility(Pawn a, Pawn b)
		{
			if (a.Faction == b.Faction)
			{
				return false;
			}
			if ((!a.IsShambler || !a.Faction.HostileTo(b.Faction)) && (!b.IsShambler || !b.Faction.HostileTo(a.Faction)))
			{
				return (a.IsShambler && a.Faction == null && b.Faction != null) || (b.IsShambler && b.Faction == null && a.Faction != null);
			}
			if (a.IsAnimal)
			{
				return a.Faction != null && (!a.RaceProps.herdAnimal || a.IsReleasedToAttack());
			}
			return !b.IsAnimal || (b.Faction != null && (!b.RaceProps.herdAnimal || b.IsReleasedToAttack()));
		}

		// Token: 0x0600861C RID: 34332 RVA: 0x003078BC File Offset: 0x00305ABC
		public static bool ShamblerShouldCollideWith(Pawn me, Pawn other)
		{
			return me.mindState.anyCloseHostilesRecently || other.mindState.anyCloseHostilesRecently || (other.mindState.lastAttackedTarget.Thing != null && (other.mindState.lastAttackTargetTick > Find.TickManager.TicksGame - 300 && other.mindState.lastAttackedTarget.Thing.Spawned));
		}

		// Token: 0x0600861D RID: 34333 RVA: 0x00307930 File Offset: 0x00305B30
		public static bool CanUseDrug(Pawn pawn, ThingDef drug)
		{
			return !pawn.IsMutant || !pawn.mutant.HasTurned || (pawn.mutant.Def.canUseDrugs && pawn.mutant.Def.drugWhitelist.Contains(drug));
		}

		// Token: 0x0600861E RID: 34334 RVA: 0x00307984 File Offset: 0x00305B84
		public static Color GetMutantSkinColor(Pawn pawn, Color baseColor)
		{
			if (!pawn.IsMutant || !pawn.mutant.HasTurned)
			{
				return baseColor;
			}
			if (pawn.IsShambler)
			{
				return MutantUtility.GetShamblerColor(baseColor);
			}
			if (pawn.mutant.Def.skinColorTint != null)
			{
				return baseColor * (1f - pawn.mutant.Def.skinColorTintStrength) + pawn.mutant.Def.skinColorTint.Value * pawn.mutant.Def.skinColorTintStrength;
			}
			if (pawn.mutant.Def.skinColorOverride != null)
			{
				return pawn.mutant.Def.skinColorOverride.Value;
			}
			return baseColor;
		}

		// Token: 0x0600861F RID: 34335 RVA: 0x00307A48 File Offset: 0x00305C48
		public static Color GetShamblerColor(Color color)
		{
			float num;
			float num2;
			float num3;
			Color.RGBToHSV(color, out num, out num2, out num3);
			return Color.HSVToRGB((num + 0.05f) % 1f, Mathf.Clamp(num2 * 0.5f, 0.2f, 0.5f), Mathf.Max(num3 * 0.5f, 0.3f));
		}

		// Token: 0x04005298 RID: 21144
		public const float ShamblerLOS = 20f;

		// Token: 0x04005299 RID: 21145
		public const float ShamblerBuildingDamageFactor = 1.5f;

		// Token: 0x0400529A RID: 21146
		private const float ActivateRadius = 10f;

		// Token: 0x0400529B RID: 21147
		public static IntRange CallIntervalRange = new IntRange(500, 1000);

		// Token: 0x0400529C RID: 21148
		private static List<Hediff> tmpHediffs = new List<Hediff>();
	}
}
