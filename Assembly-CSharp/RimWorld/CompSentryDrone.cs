using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x0200196A RID: 6506
	public class CompSentryDrone : ThingComp
	{
		// Token: 0x17001C47 RID: 7239
		// (get) Token: 0x0600A34B RID: 41803 RVA: 0x003A4D74 File Offset: 0x003A2F74
		private Pawn Drone
		{
			get
			{
				return this.parent as Pawn;
			}
		}

		// Token: 0x17001C48 RID: 7240
		// (get) Token: 0x0600A34C RID: 41804 RVA: 0x003A4D81 File Offset: 0x003A2F81
		// (set) Token: 0x0600A34D RID: 41805 RVA: 0x003A4D89 File Offset: 0x003A2F89
		public CompSentryDrone.SentryDroneMode Mode
		{
			get
			{
				return this.mode;
			}
			set
			{
				this.mode = value;
			}
		}

		// Token: 0x0600A34E RID: 41806 RVA: 0x003A4D92 File Offset: 0x003A2F92
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (respawningAfterLoad)
			{
				return;
			}
			this.spawnPos = this.parent.Position;
		}

		// Token: 0x0600A34F RID: 41807 RVA: 0x003A4DAC File Offset: 0x003A2FAC
		public override void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			if (!this.Drone.Spawned || this.Drone.Dead)
			{
				return;
			}
			if (this.Mode == CompSentryDrone.SentryDroneMode.Attack)
			{
				return;
			}
			this.Mode = CompSentryDrone.SentryDroneMode.Attack;
			this.Drone.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			this.lastSensedHostileTick = (long)Find.TickManager.TicksGame;
		}

		// Token: 0x0600A350 RID: 41808 RVA: 0x003A4E0C File Offset: 0x003A300C
		public override void CompTickInterval(int delta)
		{
			if (!this.parent.Spawned || !this.parent.IsHashIntervalTick(60, delta))
			{
				return;
			}
			if (this.Mode == CompSentryDrone.SentryDroneMode.Patrol)
			{
				Map map = this.parent.Map;
				IntVec3 position = this.parent.Position;
				using (IEnumerator<Pawn> enumerator = map.mapPawns.AllPawnsSpawned.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						Pawn pawn = enumerator.Current;
						if (pawn.Position.DistanceTo(position) <= 20f && pawn.HostileTo(this.parent) && GenSight.LineOfSight(position, pawn.Position, map))
						{
							this.Mode = CompSentryDrone.SentryDroneMode.Attack;
							this.Drone.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
							break;
						}
					}
					return;
				}
			}
			if (this.Drone.mindState.anyCloseHostilesRecently)
			{
				this.lastSensedHostileTick = (long)Find.TickManager.TicksGame;
				return;
			}
			if (Math.Abs(this.lastSensedHostileTick - (long)Find.TickManager.TicksGame) < 3600L)
			{
				return;
			}
			this.Mode = CompSentryDrone.SentryDroneMode.Patrol;
			this.Drone.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
		}

		// Token: 0x0600A351 RID: 41809 RVA: 0x003A4F44 File Offset: 0x003A3144
		public IntVec3 GetNextPatrolDest()
		{
			if (!this.parent.Spawned)
			{
				return IntVec3.Invalid;
			}
			IntVec3 actualLastPatrolDest = this.lastPatrolDest;
			this.lastPatrolDest = this.parent.Position;
			Room room = this.parent.GetRoom(RegionType.Set_All);
			if (room == null || room.PsychologicallyOutdoors)
			{
				if (this.spawnPos.IsValid)
				{
					Room room4 = this.spawnPos.GetRoom(this.parent.Map);
					if (room4 != null && !room4.PsychologicallyOutdoors)
					{
						return room4.Cells.RandomElement<IntVec3>();
					}
				}
				return IntVec3.Invalid;
			}
			List<Room> adjacentRooms = CompSentryDrone.GetAdjacentRooms(room);
			if (adjacentRooms.Count > 0)
			{
				if (adjacentRooms.Count == 1)
				{
					return adjacentRooms[0].Cells.RandomElement<IntVec3>();
				}
				if (Rand.Chance(0.1f))
				{
					Room room2 = adjacentRooms.FirstOrDefault((Room r) => r.ContainsCell(actualLastPatrolDest));
					if (room2 != null)
					{
						return room2.Cells.RandomElement<IntVec3>();
					}
				}
				List<Room> list = (from r in adjacentRooms
				where !r.ContainsCell(actualLastPatrolDest)
				select r).ToList<Room>();
				if (list.Count == 0)
				{
					Log.Error("CompSentryDrone found multiple adjacent rooms but none of them were not the last room we were in?");
					return IntVec3.Invalid;
				}
				return list.RandomElement<Room>().Cells.RandomElement<IntVec3>();
			}
			else
			{
				List<IntVec3> list2 = GenRadial.RadialCellsAround(this.parent.Position, 14f, true).Where(delegate(IntVec3 c)
				{
					Room room3 = c.GetRoom(this.parent.Map);
					return room3 != null && !room3.PsychologicallyOutdoors && room3 != room;
				}).Take(30).ToList<IntVec3>();
				if (list2.Count > 0)
				{
					return list2.RandomElement<IntVec3>();
				}
				return room.Cells.RandomElement<IntVec3>();
			}
		}

		// Token: 0x0600A352 RID: 41810 RVA: 0x003A50F4 File Offset: 0x003A32F4
		private static List<Room> GetAdjacentRooms(Room room)
		{
			List<Room> list = new List<Room>();
			foreach (Region region in room.Regions)
			{
				foreach (RegionLink regionLink in region.links)
				{
					Region otherRegion = regionLink.GetOtherRegion(region);
					if (otherRegion.IsDoorway)
					{
						foreach (RegionLink regionLink2 in otherRegion.links)
						{
							Region otherRegion2 = regionLink2.GetOtherRegion(otherRegion);
							if (!otherRegion2.Room.PsychologicallyOutdoors && !list.Contains(otherRegion2.Room) && otherRegion2.Room != room)
							{
								list.Add(otherRegion2.Room);
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x0600A353 RID: 41811 RVA: 0x003A5218 File Offset: 0x003A3418
		public override bool WantHoldWeapon(Pawn pawn)
		{
			return this.mode == CompSentryDrone.SentryDroneMode.Attack;
		}

		// Token: 0x0600A354 RID: 41812 RVA: 0x003A5224 File Offset: 0x003A3424
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<CompSentryDrone.SentryDroneMode>(ref this.mode, "mode", CompSentryDrone.SentryDroneMode.Patrol, false);
			Scribe_Values.Look<IntVec3>(ref this.lastPatrolDest, "lastPatrolDest", default(IntVec3), false);
			Scribe_Values.Look<IntVec3>(ref this.spawnPos, "spawnPos", default(IntVec3), false);
			Scribe_Values.Look<long>(ref this.lastSensedHostileTick, "lastSensedHostileTick", 0L, false);
		}

		// Token: 0x04006072 RID: 24690
		private const int TicksUntilLeaveCombatState = 3600;

		// Token: 0x04006073 RID: 24691
		private const float DoubleBackChance = 0.1f;

		// Token: 0x04006074 RID: 24692
		private CompSentryDrone.SentryDroneMode mode;

		// Token: 0x04006075 RID: 24693
		private IntVec3 lastPatrolDest;

		// Token: 0x04006076 RID: 24694
		private IntVec3 spawnPos;

		// Token: 0x04006077 RID: 24695
		private long lastSensedHostileTick;

		// Token: 0x020036C0 RID: 14016
		public enum SentryDroneMode
		{
			// Token: 0x0400DAAD RID: 55981
			Patrol,
			// Token: 0x0400DAAE RID: 55982
			Attack
		}
	}
}
