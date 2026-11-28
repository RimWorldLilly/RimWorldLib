using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x0200121E RID: 4638
	public abstract class ComplexThreatWorker_SleepingThreat : ComplexThreatWorker
	{
		// Token: 0x1700138C RID: 5004
		// (get) Token: 0x06007309 RID: 29449 RVA: 0x00283D14 File Offset: 0x00281F14
		private Faction Faction
		{
			get
			{
				return Find.FactionManager.FirstFactionOfDef(this.def.faction);
			}
		}

		// Token: 0x0600730A RID: 29450 RVA: 0x00283D2B File Offset: 0x00281F2B
		protected override bool CanResolveInt(ComplexResolveParams parms)
		{
			return base.CanResolveInt(parms) && parms.room != null && this.Faction != null && this.GetPawnKindsForPoints(parms.points).Any<PawnKindDef>();
		}

		// Token: 0x0600730B RID: 29451
		protected abstract IEnumerable<PawnKindDef> GetPawnKindsForPoints(float points);

		// Token: 0x0600730C RID: 29452 RVA: 0x00283D5C File Offset: 0x00281F5C
		protected override void ResolveInt(ComplexResolveParams parms, ref float threatPointsUsed, List<Thing> outSpawnedThings)
		{
			List<Pawn> list = this.SpawnThreatPawns(parms.room, parms.points, parms.map);
			for (int i = 0; i < list.Count; i++)
			{
				threatPointsUsed += list[i].kindDef.combatPower;
			}
			LordJob_SleepThenAssaultColony lordJob = new LordJob_SleepThenAssaultColony(this.Faction, true);
			Lord lord = LordMaker.MakeNewLord(this.Faction, lordJob, parms.map, null);
			lord.AddPawns(list, true);
			if (!parms.passive && Rand.Chance(ComplexThreatWorker_SleepingThreat.RadialTriggerChance))
			{
				parms.triggerSignal = ComplexUtility.SpawnRadialDistanceTrigger(list, parms.map, ComplexThreatWorker_SleepingThreat.RadiusWakeUpDistanceRange.RandomInRange);
			}
			SignalAction_DormancyWakeUp signalAction_DormancyWakeUp = (SignalAction_DormancyWakeUp)ThingMaker.MakeThing(ThingDefOf.SignalAction_DormancyWakeUp, null);
			signalAction_DormancyWakeUp.lord = lord;
			signalAction_DormancyWakeUp.signalTag = parms.triggerSignal;
			if (parms.delayTicks != null)
			{
				signalAction_DormancyWakeUp.delayTicks = parms.delayTicks.Value;
				SignalAction_Message signalAction_Message = (SignalAction_Message)ThingMaker.MakeThing(ThingDefOf.SignalAction_Message, null);
				signalAction_Message.signalTag = parms.triggerSignal;
				signalAction_Message.lookTargets = list;
				signalAction_Message.messageType = MessageTypeDefOf.ThreatBig;
				signalAction_Message.allMustBeAsleep = true;
				signalAction_Message.message = "MessageSleepingThreatDelayActivated".Translate(this.Faction, signalAction_DormancyWakeUp.delayTicks.ToStringTicksToPeriod(true, false, true, true, false));
				GenSpawn.Spawn(signalAction_Message, parms.room.rects[0].CenterCell, parms.map, WipeMode.Vanish);
			}
			GenSpawn.Spawn(signalAction_DormancyWakeUp, parms.map.Center, parms.map, WipeMode.Vanish);
		}

		// Token: 0x0600730D RID: 29453 RVA: 0x00283F04 File Offset: 0x00282104
		private List<Pawn> SpawnThreatPawns(LayoutRoom room, float threatPoints, Map map)
		{
			List<Pawn> list = new List<Pawn>();
			ComplexThreatWorker_SleepingThreat.tmpCells.Clear();
			ComplexThreatWorker_SleepingThreat.tmpCells.AddRange(room.rects.SelectMany((CellRect r) => r.Cells));
			ComplexThreatWorker_SleepingThreat.tmpCells.Shuffle<IntVec3>();
			float num = threatPoints;
			for (int i = 0; i < ComplexThreatWorker_SleepingThreat.tmpCells.Count; i++)
			{
				if (ComplexThreatWorker_SleepingThreat.CanSpawnAt(ComplexThreatWorker_SleepingThreat.tmpCells[i], map))
				{
					IntVec3 loc = ComplexThreatWorker_SleepingThreat.tmpCells[i];
					IEnumerable<PawnKindDef> pawnKindsForPoints = this.GetPawnKindsForPoints(num);
					if (!pawnKindsForPoints.Any<PawnKindDef>())
					{
						break;
					}
					PawnKindDef pawnKindDef = pawnKindsForPoints.RandomElement<PawnKindDef>();
					Pawn pawn = PawnGenerator.GeneratePawn(pawnKindDef, this.Faction, new PlanetTile?(map.Tile));
					GenSpawn.Spawn(pawn, loc, map, WipeMode.Vanish);
					list.Add(pawn);
					num -= pawnKindDef.combatPower;
				}
			}
			return list;
		}

		// Token: 0x0600730E RID: 29454 RVA: 0x00283FEB File Offset: 0x002821EB
		private static bool CanSpawnAt(IntVec3 c, Map map)
		{
			return c.Standable(map) && c.GetFirstPawn(map) == null && c.GetDoor(map) == null;
		}

		// Token: 0x040048C1 RID: 18625
		private const string WakeUpSignalTag = "WakeUp";

		// Token: 0x040048C2 RID: 18626
		private static readonly IntRange RadiusWakeUpDistanceRange = new IntRange(1, 4);

		// Token: 0x040048C3 RID: 18627
		private static readonly float RadialTriggerChance = 0.2f;

		// Token: 0x040048C4 RID: 18628
		private static readonly List<IntVec3> tmpCells = new List<IntVec3>();
	}
}
