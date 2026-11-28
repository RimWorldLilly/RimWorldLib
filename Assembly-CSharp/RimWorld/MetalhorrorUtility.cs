using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020014B5 RID: 5301
	public static class MetalhorrorUtility
	{
		// Token: 0x060081BA RID: 33210 RVA: 0x002E4968 File Offset: 0x002E2B68
		public static void TryEmerge(Pawn infected, string reasonKey = null, bool sympathetic = false)
		{
			Hediff_MetalhorrorImplant firstHediff = infected.health.hediffSet.GetFirstHediff<Hediff_MetalhorrorImplant>();
			if (firstHediff != null)
			{
				firstHediff.Emerge(reasonKey, sympathetic);
			}
		}

		// Token: 0x060081BB RID: 33211 RVA: 0x002E4994 File Offset: 0x002E2B94
		public static Pawn SpawnMetalhorror(Pawn infected, Hediff_MetalhorrorImplant hediff)
		{
			if (!ModLister.CheckAnomaly("Metalhorror"))
			{
				return null;
			}
			if (hediff == null)
			{
				Log.Error("Attempted to emerged from pawn which did not have a Metalhorror implant hediff");
				return null;
			}
			Pawn pawn = PawnGenerator.GeneratePawn(new PawnGenerationRequest(PawnKindDefOf.Metalhorror, Faction.OfEntities, PawnGenerationContext.NonPlayer, null, false, false, false, true, false, 1f, false, true, false, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, false, false, false, false, null, null, null, null, null, 0f, DevelopmentalStage.Adult, null, null, null, false, false, false, -1, 0, false));
			IntVec3 positionHeld;
			if (!GenAdj.TryFindRandomAdjacentCell8WayWithRoom(infected.SpawnedParentOrMe, out positionHeld))
			{
				positionHeld = infected.PositionHeld;
			}
			CompMetalhorror compMetalhorror = pawn.TryGetComp<CompMetalhorror>();
			compMetalhorror.emergedFrom = infected;
			compMetalhorror.implantSource = hediff.ImplantSource;
			int index = 2;
			int num = hediff.ageTicks / 2500;
			if (num <= 24)
			{
				index = 0;
			}
			else if (num <= 72)
			{
				index = 1;
			}
			pawn.ageTracker.LockCurrentLifeStageIndex(index);
			pawn.ageTracker.AgeBiologicalTicks = (long)hediff.ageTicks;
			pawn.ageTracker.AgeChronologicalTicks = (long)hediff.ageTicks;
			Pawn pawn2 = (Pawn)GenSpawn.Spawn(pawn, positionHeld, infected.MapHeld, WipeMode.Vanish);
			compMetalhorror.FindOrCreateEmergedLord();
			infected.health.RemoveHediff(hediff);
			Find.BattleLog.Add(new BattleLogEntry_Event(infected, RulePackDefOf.Event_MetalhorrorEmerged, pawn2));
			pawn2.stances.stunner.StunFor(60, null, false, true, false);
			return pawn2;
		}

		// Token: 0x060081BC RID: 33212 RVA: 0x002E4B30 File Offset: 0x002E2D30
		public static Thing FindTarget(Pawn pawn)
		{
			TargetScanFlags flags = TargetScanFlags.NeedLOSToPawns | TargetScanFlags.NeedReachable | TargetScanFlags.NeedThreat | TargetScanFlags.NeedAutoTargetable;
			return (Thing)AttackTargetFinder.BestAttackTarget(pawn, flags, null, 0f, 29.9f, default(IntVec3), float.MaxValue, false, true, false, false);
		}

		// Token: 0x060081BD RID: 33213 RVA: 0x002E4B6C File Offset: 0x002E2D6C
		public static void Infect(Pawn pawn, Pawn source = null, string descKey = "InsectImplant", string descResolved = null)
		{
			if (!MetalhorrorUtility.CanBeInfected(pawn))
			{
				return;
			}
			ImplantSource implantSource = new ImplantSource(source, descKey, descResolved);
			if (Find.Anomaly.emergedBiosignatures.Contains(implantSource.Biosignature))
			{
				return;
			}
			if (source == null)
			{
				Find.Anomaly.Notify_NewMetalhorrorBiosignatureImplanted();
			}
			((Hediff_MetalhorrorImplant)pawn.health.AddHediff(HediffDefOf.MetalhorrorImplant, null, null, null)).ImplantSource = implantSource;
		}

		// Token: 0x060081BE RID: 33214 RVA: 0x002E4BD8 File Offset: 0x002E2DD8
		public static bool IsInfected(Pawn pawn)
		{
			Hediff_MetalhorrorImplant hediff_MetalhorrorImplant;
			return MetalhorrorUtility.IsInfected(pawn, out hediff_MetalhorrorImplant);
		}

		// Token: 0x060081BF RID: 33215 RVA: 0x002E4BED File Offset: 0x002E2DED
		public static bool IsInfected(Pawn pawn, out Hediff_MetalhorrorImplant hediff)
		{
			hediff = null;
			if (!pawn.RaceProps.IsFlesh)
			{
				return false;
			}
			hediff = pawn.health.hediffSet.GetFirstHediff<Hediff_MetalhorrorImplant>();
			return hediff != null;
		}

		// Token: 0x060081C0 RID: 33216 RVA: 0x002E4C18 File Offset: 0x002E2E18
		public static bool CanBeInfected(Pawn pawn)
		{
			return ModsConfig.AnomalyActive && pawn.RaceProps.Humanlike && !pawn.DevelopmentalStage.Baby() && !MetalhorrorUtility.IsInfected(pawn);
		}

		// Token: 0x060081C1 RID: 33217 RVA: 0x002E4C48 File Offset: 0x002E2E48
		public static void Detect(Pawn infected, string reason, string subtleMessage, float realisedDetectedChance)
		{
			Hediff_MetalhorrorImplant firstHediff = infected.health.hediffSet.GetFirstHediff<Hediff_MetalhorrorImplant>();
			if (firstHediff == null || firstHediff.Visible)
			{
				return;
			}
			firstHediff.Detect(reason, Rand.Chance(realisedDetectedChance));
			if (!firstHediff.KnowsDetected)
			{
				Find.LetterStack.ReceiveLetter("MetalhorrorDetected".Translate(), subtleMessage, LetterDefOf.ThreatSmall, null, 0, true);
			}
		}

		// Token: 0x060081C2 RID: 33218 RVA: 0x002E4CAC File Offset: 0x002E2EAC
		public static bool ShouldRandomEmerge(Pawn pawn, int delta)
		{
			if (!pawn.IsHashIntervalTick(300, delta))
			{
				return false;
			}
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			if (!pawn.SpawnedOrAnyParentSpawned)
			{
				return false;
			}
			Hediff_MetalhorrorImplant firstHediff = pawn.health.hediffSet.GetFirstHediff<Hediff_MetalhorrorImplant>();
			if (firstHediff == null)
			{
				return false;
			}
			if (firstHediff.Emerging)
			{
				return false;
			}
			if (MetalhorrorUtility.cachedFrame != GenTicks.TicksGame)
			{
				MetalhorrorUtility.cachedFrame = GenTicks.TicksGame;
				MetalhorrorUtility.cachedMTB.Clear();
			}
			if (!MetalhorrorUtility.cachedMTB.ContainsKey(pawn.MapHeld))
			{
				int num = 0;
				int num2 = 0;
				foreach (Pawn pawn2 in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive)
				{
					if (pawn2.RaceProps.Humanlike && (!pawn2.IsMutant || !pawn2.mutant.Def.preventIllnesses) && (pawn2.Faction == Faction.OfPlayer || pawn2.HostFaction == Faction.OfPlayer) && !pawn2.IsPrisoner)
					{
						if (MetalhorrorUtility.IsInfected(pawn2))
						{
							num++;
						}
						num2++;
					}
				}
				if (num2 == 0)
				{
					MetalhorrorUtility.cachedMTB[pawn.MapHeld] = -1f;
					return false;
				}
				float x = (float)num / (float)num2;
				MetalhorrorUtility.cachedMTB[pawn.MapHeld] = MetalhorrorUtility.NaturalEmergenceMTBCurve.Evaluate(x);
			}
			return MetalhorrorUtility.cachedMTB[pawn.MapHeld] >= 0f && Rand.MTBEventOccurs(MetalhorrorUtility.cachedMTB[pawn.MapHeld], 1f, 300f);
		}

		// Token: 0x060081C3 RID: 33219 RVA: 0x002E4E48 File Offset: 0x002E3048
		public static bool TryPawnExitMap(Pawn pawn)
		{
			bool flag = pawn.guest != null && pawn.guest.Released;
			if (!pawn.Spawned || !flag || !MetalhorrorUtility.IsInfected(pawn))
			{
				return true;
			}
			MetalhorrorUtility.TryEmerge(pawn, "MetalhorrorReasonTriedToLeaveMap".Translate(pawn.Named("INFECTED")), false);
			return false;
		}

		// Token: 0x0400506F RID: 20591
		public const float MetalhorrorLOS = 29.9f;

		// Token: 0x04005070 RID: 20592
		private const int MTBUpdateRateTicks = 300;

		// Token: 0x04005071 RID: 20593
		public const int MaxAgeHoursLarva = 24;

		// Token: 0x04005072 RID: 20594
		public const int MaxAgeHoursJuvenile = 72;

		// Token: 0x04005073 RID: 20595
		private const int LarvaLifeStageIndex = 0;

		// Token: 0x04005074 RID: 20596
		private const int JuvenileLifeStageIndex = 1;

		// Token: 0x04005075 RID: 20597
		private const int MatureLifeStageIndex = 2;

		// Token: 0x04005076 RID: 20598
		private const int SpawnStunTicks = 60;

		// Token: 0x04005077 RID: 20599
		private static readonly SimpleCurve NaturalEmergenceMTBCurve = new SimpleCurve
		{
			{
				new CurvePoint(0.19f, 7200000f),
				true
			},
			{
				new CurvePoint(0.2f, 1800000f),
				true
			},
			{
				new CurvePoint(0.4f, 240000f),
				true
			},
			{
				new CurvePoint(0.5f, 1800f),
				true
			}
		};

		// Token: 0x04005078 RID: 20600
		private static int cachedFrame;

		// Token: 0x04005079 RID: 20601
		private static readonly Dictionary<Map, float> cachedMTB = new Dictionary<Map, float>();
	}
}
