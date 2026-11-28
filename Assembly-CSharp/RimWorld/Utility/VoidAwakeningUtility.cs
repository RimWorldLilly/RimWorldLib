using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI.Group;

namespace RimWorld.Utility
{
	// Token: 0x020020B2 RID: 8370
	public static class VoidAwakeningUtility
	{
		// Token: 0x0600D2F0 RID: 54000 RVA: 0x004D9084 File Offset: 0x004D7284
		public static string EncodeWaveType(string signal, VoidAwakeningUtility.WaveType waveType, float pointsFactor)
		{
			string[] array = new string[5];
			array[0] = signal;
			array[1] = ".";
			int num = 2;
			int num2 = (int)waveType;
			array[num] = num2.ToString();
			array[3] = ".";
			array[4] = pointsFactor.ToString("F2");
			return string.Concat(array);
		}

		// Token: 0x0600D2F1 RID: 54001 RVA: 0x004D90CC File Offset: 0x004D72CC
		public static void DecodeWaveType(string signal, out VoidAwakeningUtility.WaveType waveType, out float pointsFactor)
		{
			string[] array = signal.Split('.', StringSplitOptions.None);
			pointsFactor = float.Parse(array[array.Length - 2] + "." + array[array.Length - 1]);
			waveType = (VoidAwakeningUtility.WaveType)int.Parse(array[array.Length - 3]);
		}

		// Token: 0x0600D2F2 RID: 54002 RVA: 0x004D9110 File Offset: 0x004D7310
		public static void EmbraceTheVoid(Pawn pawn)
		{
			VoidAwakeningUtility.CloseMetalHell(pawn);
			Find.Anomaly.monolith.quest.End(QuestEndOutcome.Success, true, false);
			Find.Anomaly.SetLevel(MonolithLevelDefOf.Embraced, false);
			pawn.health.AddHediff(HediffDefOf.VoidTouched, null, null, null);
			pawn.health.AddHediff(HediffDefOf.Inhumanized, null, null, null);
			Pawn_NeedsTracker needs = pawn.needs;
			if (needs != null)
			{
				Need_Mood mood = needs.mood;
				if (mood != null)
				{
					ThoughtHandler thoughts = mood.thoughts;
					if (thoughts != null)
					{
						MemoryThoughtHandler memories = thoughts.memories;
						if (memories != null)
						{
							memories.TryGainMemory(ThoughtDefOf.EmbracedTheVoid, null, null);
						}
					}
				}
			}
			TaleRecorder.RecordTale(TaleDefOf.EmbracedTheVoid, new object[]
			{
				pawn
			});
			Hediff hediff;
			if (pawn.health.hediffSet.TryGetHediff(HediffDefOf.DeathRefusal, out hediff))
			{
				pawn.health.RemoveHediff(hediff);
			}
			(pawn.health.AddHediff(HediffDefOf.DeathRefusal, null, null, null) as Hediff_DeathRefusal).SetUseAmountDirect(4, true);
			GameVictoryUtility.ShowCredits("EmbraceTheVoidCredits".Translate(pawn.Named("PAWN")), null, false, 5f);
			Find.Anomaly.metalHellReturnLetterText = "EmbracedVoidText".Translate(pawn.Named("PAWN"));
		}

		// Token: 0x0600D2F3 RID: 54003 RVA: 0x004D9268 File Offset: 0x004D7468
		public static void DisruptTheLink(Pawn pawn)
		{
			VoidAwakeningUtility.CloseMetalHell(pawn);
			Find.Anomaly.monolith.quest.End(QuestEndOutcome.Success, true, false);
			Find.Anomaly.SetLevel(MonolithLevelDefOf.Disrupted, false);
			Pawn_NeedsTracker needs = pawn.needs;
			if (needs != null)
			{
				Need_Mood mood = needs.mood;
				if (mood != null)
				{
					ThoughtHandler thoughts = mood.thoughts;
					if (thoughts != null)
					{
						MemoryThoughtHandler memories = thoughts.memories;
						if (memories != null)
						{
							memories.TryGainMemory(ThoughtDefOf.ClosedTheVoid, null, null);
						}
					}
				}
			}
			TaleRecorder.RecordTale(TaleDefOf.ClosedTheVoid, new object[]
			{
				pawn
			});
			foreach (Pawn pawn2 in PawnsFinder.AllMapsCaravansAndTravellingTransporters_Alive_OfPlayerFaction)
			{
				if (pawn2.Inhumanized())
				{
					pawn2.Rehumanize();
				}
				Pawn_NeedsTracker needs2 = pawn2.needs;
				if (needs2 != null)
				{
					Need_Mood mood2 = needs2.mood;
					if (mood2 != null)
					{
						ThoughtHandler thoughts2 = mood2.thoughts;
						if (thoughts2 != null)
						{
							MemoryThoughtHandler memories2 = thoughts2.memories;
							if (memories2 != null)
							{
								memories2.TryGainMemory(ThoughtDefOf.VoidClosed, null, null);
							}
						}
					}
				}
				Pawn_NeedsTracker needs3 = pawn2.needs;
				if (needs3 != null)
				{
					Need_Mood mood3 = needs3.mood;
					if (mood3 != null)
					{
						ThoughtHandler thoughts3 = mood3.thoughts;
						if (thoughts3 != null)
						{
							MemoryThoughtHandler memories3 = thoughts3.memories;
							if (memories3 != null)
							{
								memories3.TryGainMemory(ThoughtDefOf.ClosedTheVoidOpinion, pawn, null);
							}
						}
					}
				}
			}
			Find.Anomaly.monolith.Collapse();
			GameVictoryUtility.ShowCredits("DisruptTheLinkCredits".Translate(pawn.Named("PAWN")), null, false, 5f);
			Find.Anomaly.metalHellReturnLetterText = "DisruptedLinkText".Translate(pawn.Named("PAWN"));
		}

		// Token: 0x0600D2F4 RID: 54004 RVA: 0x004D9410 File Offset: 0x004D7610
		private static void CloseMetalHell(Pawn pawn)
		{
			Map mapHeld = pawn.MapHeld;
			if (mapHeld != null && mapHeld.IsPocketMap)
			{
				Find.Anomaly.voidNodeActivator = pawn;
				foreach (Pawn pawn2 in mapHeld.mapPawns.AllPawns.ToList<Pawn>())
				{
					pawn2.DeSpawn(DestroyMode.Vanish);
					Find.Anomaly.metalHellPawns.Add(pawn2);
				}
				Find.Anomaly.metalHellClosedTick = Find.TickManager.TicksGame;
				Find.Anomaly.metalHellReturnTick = Find.TickManager.TicksGame + VoidAwakeningUtility.MetalHellReturnDelaySeconds.RandomInRange * 60;
				PocketMapUtility.DestroyPocketMap(mapHeld);
			}
		}

		// Token: 0x0600D2F5 RID: 54005 RVA: 0x004D94E0 File Offset: 0x004D76E0
		public static void SpawnMetalhorrorDefenders(Thing structure)
		{
			float num = VoidAwakeningUtility.DefenderPointsByCombatPoints.Evaluate(StorytellerUtility.DefaultThreatPointsNow(structure.Map));
			num = Mathf.Max(num, Faction.OfEntities.def.MinPointsToGeneratePawnGroup(PawnGroupKindDefOf.Metalhorrors, null) * 1.05f);
			List<Pawn> list = PawnGroupMakerUtility.GeneratePawns(new PawnGroupMakerParms
			{
				groupKind = PawnGroupKindDefOf.Metalhorrors,
				points = num,
				faction = Faction.OfEntities
			}, true).ToList<Pawn>();
			Predicate<IntVec3> <>9__1;
			foreach (Pawn pawn in list)
			{
				IntVec3 position = structure.Position;
				Map map = structure.Map;
				int squareRadius = 6;
				Predicate<IntVec3> validator;
				if ((validator = <>9__1) == null)
				{
					validator = (<>9__1 = ((IntVec3 c) => c.Walkable(structure.Map) && !c.Fogged(structure.Map)));
				}
				IntVec3 loc;
				if (CellFinder.TryFindRandomCellNear(position, map, squareRadius, validator, out loc, -1))
				{
					GenSpawn.Spawn(pawn, loc, structure.Map, WipeMode.Vanish);
					Lord lord = pawn.GetLord();
					if (lord != null)
					{
						lord.RemovePawn(pawn);
					}
				}
			}
			list.RemoveAll((Pawn p) => !p.Spawned);
			LordMaker.MakeNewLord(Faction.OfEntities, new LordJob_VoidAwakeningDefendStructure(structure.Position, 35f), structure.Map, list);
		}

		// Token: 0x0600D2F6 RID: 54006 RVA: 0x004D9660 File Offset: 0x004D7860
		public static void KillAllFreeEntities(Map map)
		{
			foreach (Pawn pawn in map.mapPawns.PawnsInFaction(Faction.OfEntities))
			{
				if (pawn.IsEntity)
				{
					CompHoldingPlatformTarget compHoldingPlatformTarget = pawn.TryGetComp<CompHoldingPlatformTarget>();
					if (compHoldingPlatformTarget == null || !compHoldingPlatformTarget.CurrentlyHeldOnPlatform)
					{
						BodyPartRecord brain = pawn.health.hediffSet.GetBrain();
						if (brain != null)
						{
							pawn.TakeDamage(new DamageInfo(DamageDefOf.Burn, 99999f, 999f, -1f, null, brain, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true, QualityCategory.Normal, true, false));
						}
						else
						{
							pawn.Kill(null, null);
						}
					}
				}
			}
		}

		// Token: 0x0600D2F7 RID: 54007 RVA: 0x004D9724 File Offset: 0x004D7924
		public static void SpawnVoidMetalAround(Thing thing, int size, int numMasses, bool withSkipEffects = false, CellRect? avoidRect = null)
		{
			List<IntVec3> list = GridShapeMaker.IrregularLump(thing.Position, thing.Map, size, null).ToList<IntVec3>();
			list.Shuffle<IntVec3>();
			int num = 0;
			foreach (IntVec3 intVec in list)
			{
				if (intVec.InBounds(thing.Map))
				{
					thing.Map.terrainGrid.SetTerrain(intVec, TerrainDefOf.Voidmetal);
					thing.Map.snowGrid.SetDepth(intVec, 0f);
					SandGrid sandGrid = thing.Map.sandGrid;
					if (sandGrid != null)
					{
						sandGrid.SetDepth(intVec, 0f);
					}
					if (num != numMasses && (avoidRect == null || !avoidRect.GetValueOrDefault().Contains(intVec)) && intVec.Standable(thing.Map) && Rand.Chance(0.1f))
					{
						Thing thing2 = ThingMaker.MakeThing(ThingDefOf.VoidmetalMassSmall, null);
						if (thing2.Faction != thing.Faction)
						{
							thing2.SetFaction(thing.Faction, null);
						}
						GenSpawn.Spawn(thing2, intVec, thing.Map, WipeMode.Vanish);
						if (withSkipEffects)
						{
							EffecterDefOf.Skip_EntryNoDelay.Spawn(thing2, thing.Map, 1f).Cleanup();
						}
						num++;
					}
				}
			}
		}

		// Token: 0x040085AB RID: 34219
		private static readonly IntRange MetalHellReturnDelaySeconds = new IntRange(5, 10);

		// Token: 0x040085AC RID: 34220
		private const int DefenderSpawnRadius = 6;

		// Token: 0x040085AD RID: 34221
		private static readonly SimpleCurve DefenderPointsByCombatPoints = new SimpleCurve
		{
			{
				new CurvePoint(400f, 400f),
				true
			},
			{
				new CurvePoint(1000f, 800f),
				true
			},
			{
				new CurvePoint(2000f, 1500f),
				true
			},
			{
				new CurvePoint(4000f, 2500f),
				true
			},
			{
				new CurvePoint(10000f, 5000f),
				true
			}
		};

		// Token: 0x02003C78 RID: 15480
		public enum WaveType
		{
			// Token: 0x0400EFA6 RID: 61350
			Fleshbeast,
			// Token: 0x0400EFA7 RID: 61351
			Twisted,
			// Token: 0x0400EFA8 RID: 61352
			NoctolSightstealer,
			// Token: 0x0400EFA9 RID: 61353
			DevourerGorehulk,
			// Token: 0x0400EFAA RID: 61354
			ShamblerGorehulk
		}
	}
}
