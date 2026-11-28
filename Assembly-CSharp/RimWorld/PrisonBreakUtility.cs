using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02001556 RID: 5462
	public static class PrisonBreakUtility
	{
		// Token: 0x06008593 RID: 34195 RVA: 0x00302B78 File Offset: 0x00300D78
		public static float InitiatePrisonBreakMtbDays(Pawn pawn, StringBuilder sb = null, bool ignoreAsleep = false)
		{
			if (!ignoreAsleep && !pawn.Awake())
			{
				return -1f;
			}
			if (!PrisonBreakUtility.CanParticipateInPrisonBreak(pawn))
			{
				return -1f;
			}
			Room room = pawn.GetRoom(RegionType.Set_All);
			if (room == null || !room.IsPrisonCell)
			{
				return -1f;
			}
			float num = pawn.kindDef.basePrisonBreakMtbDays;
			float num2 = Mathf.Clamp(pawn.health.capacities.GetLevel(PawnCapacityDefOf.Moving), 0.01f, 1f);
			num /= num2;
			if (sb != null && num2 != 1f)
			{
				sb.AppendLineIfNotEmpty();
				sb.Append("  - " + "FactorForMovement".Translate() + ": " + (1f / num2).ToStringPercent());
			}
			if (pawn.genes != null)
			{
				float prisonBreakIntervalFactor = pawn.genes.PrisonBreakIntervalFactor;
				num *= prisonBreakIntervalFactor;
				if (sb != null && prisonBreakIntervalFactor != 1f)
				{
					sb.AppendLineIfNotEmpty();
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 2);
					defaultInterpolatedStringHandler.AppendLiteral("  - ");
					defaultInterpolatedStringHandler.AppendFormatted<TaggedString>("FactorForGenes".Translate());
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendFormatted(prisonBreakIntervalFactor.ToStringPercent());
					sb.Append(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			PrisonBreakUtility.tmpRegions.Clear();
			PrisonBreakUtility.tmpDoors.Clear();
			foreach (Region region in room.Regions)
			{
				foreach (RegionLink regionLink in region.links)
				{
					Region otherRegion = regionLink.GetOtherRegion(region);
					if (otherRegion.type == RegionType.Portal && PrisonBreakUtility.tmpRegions.Add(otherRegion))
					{
						Building_Door door = otherRegion.door;
						for (int i = 0; i < otherRegion.links.Count; i++)
						{
							Region regionA = otherRegion.links[i].RegionA;
							Region regionB = otherRegion.links[i].RegionB;
							if ((regionA.Room != room && regionA != otherRegion && regionA.door != door) || (regionB.Room != room && regionB != otherRegion && regionB.door != door))
							{
								PrisonBreakUtility.tmpDoors.Add(door);
								break;
							}
						}
					}
				}
			}
			float num3 = (float)PrisonBreakUtility.tmpDoors.Count;
			if (num3 > 0f)
			{
				num /= num3;
				if (sb != null && num3 > 1f)
				{
					sb.AppendLineIfNotEmpty();
					sb.Append("  - " + "FactorForDoorCount".Translate() + ": " + (1f / num3).ToStringPercent());
				}
				if (ModsConfig.AnomalyActive)
				{
					bool flag = false;
					bool flag2 = true;
					using (HashSet<Building_Door>.Enumerator enumerator3 = PrisonBreakUtility.tmpDoors.GetEnumerator())
					{
						while (enumerator3.MoveNext())
						{
							if (enumerator3.Current.def != ThingDefOf.SecurityDoor)
							{
								flag2 = false;
								break;
							}
							flag = true;
						}
					}
					if (flag && flag2)
					{
						if (sb != null)
						{
							sb.AppendLineIfNotEmpty();
							sb.Append("  - " + "SecurityDoorsOnly".Translate() + ": " + 1.1f.ToStringPercent());
						}
						num *= 1.1f;
					}
				}
			}
			if (ModsConfig.AnomalyActive && pawn.health.hediffSet.HasHediff(HediffDefOf.BlissLobotomy, false))
			{
				num *= 10f;
				if (sb != null)
				{
					sb.AppendLineIfNotEmpty();
					sb.Append("  - " + "BlissLobotomy".Translate() + ": " + 10f.ToStringPercent());
				}
			}
			if (pawn.guest.everParticipatedInPrisonBreak)
			{
				float x = (float)(Find.TickManager.TicksGame - pawn.guest.lastPrisonBreakTicks) / 60000f;
				num *= PrisonBreakUtility.PrisonBreakMTBFactorForDaysSincePrisonBreak.Evaluate(x);
			}
			return num;
		}

		// Token: 0x06008594 RID: 34196 RVA: 0x00302FF8 File Offset: 0x003011F8
		public static bool CanParticipateInPrisonBreak(Pawn pawn)
		{
			Gene gene;
			return !pawn.Downed && pawn.IsPrisoner && !PrisonBreakUtility.IsPrisonBreaking(pawn) && !PrisonBreakUtility.GenePreventsPrisonBreaking(pawn, out gene) && (!pawn.Spawned || !pawn.Map.IsPocketMap || !pawn.Map.generatorDef.pocketMapProperties.preventPrisonerEscape) && (!pawn.Spawned || !pawn.Map.Biome.inVacuum);
		}

		// Token: 0x06008595 RID: 34197 RVA: 0x0030307C File Offset: 0x0030127C
		public static bool GenePreventsPrisonBreaking(Pawn pawn, out Gene gene)
		{
			if (ModsConfig.BiotechActive && pawn.genes != null)
			{
				foreach (Gene gene2 in pawn.genes.GenesListForReading)
				{
					if (gene2.def.prisonBreakMTBFactor < 0f)
					{
						gene = gene2;
						return true;
					}
				}
			}
			gene = null;
			return false;
		}

		// Token: 0x06008596 RID: 34198 RVA: 0x003030FC File Offset: 0x003012FC
		public static bool IsPrisonBreaking(Pawn pawn)
		{
			Lord lord = pawn.GetLord();
			return lord != null && lord.LordJob is LordJob_PrisonBreak;
		}

		// Token: 0x06008597 RID: 34199 RVA: 0x00303124 File Offset: 0x00301324
		public static void StartPrisonBreak(Pawn initiator)
		{
			string str;
			string str2;
			LetterDef textLetterDef;
			List<Pawn> targets;
			PrisonBreakUtility.StartPrisonBreak(initiator, out str, out str2, out textLetterDef, out targets);
			if (!str.NullOrEmpty())
			{
				Find.LetterStack.ReceiveLetter(str2, str, textLetterDef, targets, null, null, null, null, 0, true);
			}
		}

		// Token: 0x06008598 RID: 34200 RVA: 0x0030316C File Offset: 0x0030136C
		public static void StartPrisonBreak(Pawn initiator, out string letterText, out string letterLabel, out LetterDef letterDef, out List<Pawn> escapingPrisoners)
		{
			PrisonBreakUtility.participatingRooms.Clear();
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(initiator.Position, 20f, true))
			{
				if (intVec.InBounds(initiator.Map))
				{
					Room room = intVec.GetRoom(initiator.Map);
					if (room != null && PrisonBreakUtility.IsOrCanBePrisonCell(room))
					{
						PrisonBreakUtility.participatingRooms.Add(room);
					}
				}
			}
			PrisonBreakUtility.RemoveRandomRooms(PrisonBreakUtility.participatingRooms, initiator);
			int sapperThingID = -1;
			if (Rand.Value < 0.5f)
			{
				sapperThingID = initiator.thingIDNumber;
			}
			PrisonBreakUtility.allEscapingPrisoners.Clear();
			foreach (Room room2 in PrisonBreakUtility.participatingRooms)
			{
				PrisonBreakUtility.StartPrisonBreakIn(room2, PrisonBreakUtility.allEscapingPrisoners, sapperThingID, PrisonBreakUtility.participatingRooms);
			}
			PrisonBreakUtility.participatingRooms.Clear();
			if (PrisonBreakUtility.allEscapingPrisoners.Any<Pawn>())
			{
				StringBuilder stringBuilder = new StringBuilder();
				for (int i = 0; i < PrisonBreakUtility.allEscapingPrisoners.Count; i++)
				{
					stringBuilder.AppendLine("  - " + PrisonBreakUtility.allEscapingPrisoners[i].NameShortColored.Resolve());
					PrisonBreakUtility.allEscapingPrisoners[i].Notify_PrisonBreakout();
				}
				letterText = "LetterPrisonBreak".Translate(stringBuilder.ToString().TrimEndNewlines());
				letterLabel = "LetterLabelPrisonBreak".Translate();
				letterDef = LetterDefOf.ThreatBig;
				escapingPrisoners = PrisonBreakUtility.allEscapingPrisoners.ToList<Pawn>();
				PrisonBreakUtility.allEscapingPrisoners.Clear();
			}
			else
			{
				letterText = null;
				letterLabel = null;
				letterDef = null;
				escapingPrisoners = null;
			}
			Find.TickManager.slower.SignalForceNormalSpeed();
		}

		// Token: 0x06008599 RID: 34201 RVA: 0x00303354 File Offset: 0x00301554
		private static bool IsOrCanBePrisonCell(Room room)
		{
			if (room.IsPrisonCell)
			{
				return true;
			}
			if (room.TouchesMapEdge)
			{
				return false;
			}
			bool result = false;
			List<Thing> containedAndAdjacentThings = room.ContainedAndAdjacentThings;
			for (int i = 0; i < containedAndAdjacentThings.Count; i++)
			{
				Pawn pawn = containedAndAdjacentThings[i] as Pawn;
				if (pawn != null && pawn.IsPrisoner)
				{
					result = true;
					break;
				}
			}
			return result;
		}

		// Token: 0x0600859A RID: 34202 RVA: 0x003033AC File Offset: 0x003015AC
		private static void RemoveRandomRooms(HashSet<Room> participatingRooms, Pawn initiator)
		{
			Room room = initiator.GetRoom(RegionType.Set_All);
			PrisonBreakUtility.tmpToRemove.Clear();
			foreach (Room room2 in participatingRooms)
			{
				if (room2 != room && Rand.Value >= 0.5f)
				{
					PrisonBreakUtility.tmpToRemove.Add(room2);
				}
			}
			for (int i = 0; i < PrisonBreakUtility.tmpToRemove.Count; i++)
			{
				participatingRooms.Remove(PrisonBreakUtility.tmpToRemove[i]);
			}
			PrisonBreakUtility.tmpToRemove.Clear();
		}

		// Token: 0x0600859B RID: 34203 RVA: 0x00303454 File Offset: 0x00301654
		private static void StartPrisonBreakIn(Room room, List<Pawn> outAllEscapingPrisoners, int sapperThingID, HashSet<Room> participatingRooms)
		{
			PrisonBreakUtility.escapingPrisonersGroup.Clear();
			PrisonBreakUtility.AddPrisonersFrom(room, PrisonBreakUtility.escapingPrisonersGroup);
			if (!PrisonBreakUtility.escapingPrisonersGroup.Any<Pawn>())
			{
				return;
			}
			foreach (Room room2 in participatingRooms)
			{
				if (room2 != room && PrisonBreakUtility.RoomsAreCloseToEachOther(room, room2))
				{
					PrisonBreakUtility.AddPrisonersFrom(room2, PrisonBreakUtility.escapingPrisonersGroup);
				}
			}
			IntVec3 exitPoint;
			if (!RCellFinder.TryFindRandomExitSpot(PrisonBreakUtility.escapingPrisonersGroup[0], out exitPoint, TraverseMode.PassDoors))
			{
				return;
			}
			IntVec3 groupUpLoc;
			if (!PrisonBreakUtility.TryFindGroupUpLoc(PrisonBreakUtility.escapingPrisonersGroup, exitPoint, out groupUpLoc))
			{
				return;
			}
			LordMaker.MakeNewLord(PrisonBreakUtility.escapingPrisonersGroup[0].Faction, new LordJob_PrisonBreak(groupUpLoc, exitPoint, sapperThingID), room.Map, PrisonBreakUtility.escapingPrisonersGroup);
			for (int i = 0; i < PrisonBreakUtility.escapingPrisonersGroup.Count; i++)
			{
				Pawn pawn = PrisonBreakUtility.escapingPrisonersGroup[i];
				if (pawn.CurJob != null && pawn.GetPosture().Laying())
				{
					pawn.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
				else
				{
					pawn.jobs.CheckForJobOverride(0f, true);
				}
				pawn.guest.everParticipatedInPrisonBreak = true;
				pawn.guest.lastPrisonBreakTicks = Find.TickManager.TicksGame;
				outAllEscapingPrisoners.Add(pawn);
			}
			PrisonBreakUtility.escapingPrisonersGroup.Clear();
		}

		// Token: 0x0600859C RID: 34204 RVA: 0x003035C0 File Offset: 0x003017C0
		private static void AddPrisonersFrom(Room room, List<Pawn> outEscapingPrisoners)
		{
			foreach (Thing thing in room.ContainedAndAdjacentThings)
			{
				Pawn pawn = thing as Pawn;
				if (pawn != null && PrisonBreakUtility.CanParticipateInPrisonBreak(pawn) && !outEscapingPrisoners.Contains(pawn))
				{
					outEscapingPrisoners.Add(pawn);
				}
			}
		}

		// Token: 0x0600859D RID: 34205 RVA: 0x0030362C File Offset: 0x0030182C
		public static bool TryFindGroupUpLoc(List<Pawn> escapingPrisoners, IntVec3 exitPoint, out IntVec3 groupUpLoc)
		{
			groupUpLoc = IntVec3.Invalid;
			Map map = escapingPrisoners[0].Map;
			using (PawnPath pawnPath = map.pathFinder.FindPathNow(escapingPrisoners[0].Position, exitPoint, TraverseParms.For(escapingPrisoners[0], Danger.Deadly, TraverseMode.PassDoors, false, false, false, true), null, PathEndMode.OnCell, null))
			{
				if (!pawnPath.Found)
				{
					string str = "Prison break: could not find path for prisoner ";
					Pawn pawn = escapingPrisoners[0];
					Log.Warning(str + ((pawn != null) ? pawn.ToString() : null) + " to the exit point.");
					return false;
				}
				Func<IntVec3, bool> <>9__0;
				for (int i = 0; i < pawnPath.NodesLeftCount; i++)
				{
					IntVec3 intVec = pawnPath.Peek(pawnPath.NodesLeftCount - i - 1);
					Room room = intVec.GetRoom(map);
					if (room != null && !room.IsPrisonCell)
					{
						if (!room.TouchesMapEdge && !room.IsHuge)
						{
							IEnumerable<IntVec3> cells = room.Cells;
							Func<IntVec3, bool> predicate;
							if ((predicate = <>9__0) == null)
							{
								predicate = (<>9__0 = ((IntVec3 x) => x.Standable(map)));
							}
							if (cells.Count(predicate) < 5)
							{
								goto IL_127;
							}
						}
						groupUpLoc = CellFinder.RandomClosewalkCellNear(intVec, map, 3, null);
					}
					IL_127:;
				}
			}
			if (!groupUpLoc.IsValid)
			{
				groupUpLoc = escapingPrisoners[0].Position;
			}
			return true;
		}

		// Token: 0x0600859E RID: 34206 RVA: 0x003037AC File Offset: 0x003019AC
		private static bool RoomsAreCloseToEachOther(Room a, Room b)
		{
			IntVec3 anyCell = a.FirstRegion.AnyCell;
			IntVec3 anyCell2 = b.FirstRegion.AnyCell;
			if (a.Map != b.Map)
			{
				return false;
			}
			if (!anyCell.WithinRegions(anyCell2, a.Map, 18, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false), RegionType.Set_Passable))
			{
				return false;
			}
			bool result;
			using (PawnPath pawnPath = a.Map.pathFinder.FindPathNow(anyCell, anyCell2, TraverseParms.For(TraverseMode.PassDoors, Danger.Deadly, false, false, false, true, false), null, PathEndMode.OnCell, null))
			{
				if (!pawnPath.Found)
				{
					result = false;
				}
				else
				{
					result = (pawnPath.NodesLeftCount < 24);
				}
			}
			return result;
		}

		// Token: 0x0400523F RID: 21055
		private const float DistanceToJoinPrisonBreak = 20f;

		// Token: 0x04005240 RID: 21056
		private const float ChanceForRoomToJoinPrisonBreak = 0.5f;

		// Token: 0x04005241 RID: 21057
		private const float SapperChance = 0.5f;

		// Token: 0x04005242 RID: 21058
		private const float BlissLobotomyMultiplier = 10f;

		// Token: 0x04005243 RID: 21059
		private const float PrisonBreakMTBFactorSecurityDoorsOnly = 1.1f;

		// Token: 0x04005244 RID: 21060
		private static readonly SimpleCurve PrisonBreakMTBFactorForDaysSincePrisonBreak = new SimpleCurve
		{
			{
				new CurvePoint(0f, 20f),
				true
			},
			{
				new CurvePoint(5f, 1.5f),
				true
			},
			{
				new CurvePoint(10f, 1f),
				true
			}
		};

		// Token: 0x04005245 RID: 21061
		private static HashSet<Region> tmpRegions = new HashSet<Region>();

		// Token: 0x04005246 RID: 21062
		private static HashSet<Building_Door> tmpDoors = new HashSet<Building_Door>();

		// Token: 0x04005247 RID: 21063
		private static HashSet<Room> participatingRooms = new HashSet<Room>();

		// Token: 0x04005248 RID: 21064
		private static List<Pawn> allEscapingPrisoners = new List<Pawn>();

		// Token: 0x04005249 RID: 21065
		private static List<Room> tmpToRemove = new List<Room>();

		// Token: 0x0400524A RID: 21066
		private static List<Pawn> escapingPrisonersGroup = new List<Pawn>();
	}
}
