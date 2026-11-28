using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CD5 RID: 3285
	public abstract class LordToil_EntitySwarm : LordToil
	{
		// Token: 0x1700103C RID: 4156
		// (get) Token: 0x06005CFF RID: 23807 RVA: 0x00221E44 File Offset: 0x00220044
		protected LordToilData_EntitySwarm Data
		{
			get
			{
				return (LordToilData_EntitySwarm)this.data;
			}
		}

		// Token: 0x06005D00 RID: 23808
		protected abstract DutyDef GetDutyDef();

		// Token: 0x06005D01 RID: 23809 RVA: 0x00221E51 File Offset: 0x00220051
		public LordToil_EntitySwarm(IntVec3 start, IntVec3 dest)
		{
			this.data = new LordToilData_EntitySwarm
			{
				pos = start,
				dest = dest,
				lastMoved = GenTicks.TicksGame
			};
		}

		// Token: 0x06005D02 RID: 23810 RVA: 0x00221E80 File Offset: 0x00220080
		private void GetPath()
		{
			this.path = this.lord.Map.pathFinder.FindPathNow(this.Data.pos, this.Data.dest, TraverseParms.For(TraverseMode.NoPassClosedDoors, Danger.Deadly, false, false, false, true, false), null, PathEndMode.OnCell, null);
		}

		// Token: 0x06005D03 RID: 23811 RVA: 0x00221EDC File Offset: 0x002200DC
		public override void Init()
		{
			this.GetPath();
			for (int i = 0; i < 7; i++)
			{
				this.Data.pos = this.path.ConsumeNextNode();
			}
		}

		// Token: 0x06005D04 RID: 23812 RVA: 0x00221F14 File Offset: 0x00220114
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				this.lord.ownedPawns[i].mindState.duty = new PawnDuty(this.GetDutyDef());
			}
		}

		// Token: 0x06005D05 RID: 23813 RVA: 0x00221F64 File Offset: 0x00220164
		public override void LordToilTick()
		{
			if (this.path == null)
			{
				this.GetPath();
			}
			if (this.path.NodesLeftCount <= 1 || !this.path.Found || this.lord.ownedPawns.Count == 0)
			{
				this.lord.ReceiveMemo("TravelArrived");
				return;
			}
			if (Find.TickManager.TicksGame > this.Data.lastMoved + 600)
			{
				this.Data.pos = this.path.ConsumeNextNode();
				this.Data.lastMoved = Find.TickManager.TicksGame;
			}
			int index = Find.TickManager.TicksGame % this.lord.ownedPawns.Count;
			Pawn pawn = this.lord.ownedPawns[index];
			if (!pawn.Position.InHorDistOf(this.Data.pos, 7f) && !pawn.pather.Moving)
			{
				IntVec3 c;
				CellFinder.TryFindRandomReachableCellNearPosition(pawn.Position, this.Data.pos, this.lord.Map, 7f, TraverseParms.For(pawn, Danger.Deadly, TraverseMode.ByPawn, false, false, false, true), (IntVec3 x) => x.Standable(this.lord.Map), null, out c, 999999);
				if (!c.IsValid)
				{
					this.lord.RemovePawn(pawn);
				}
				pawn.mindState.duty = new PawnDuty(this.GetDutyDef(), c, -1f);
			}
			base.LordToilTick();
		}

		// Token: 0x06005D06 RID: 23814 RVA: 0x002220E8 File Offset: 0x002202E8
		public override void Notify_PawnAcquiredTarget(Pawn detector, Thing newTarg)
		{
			this.lord.Notify_PawnLost(detector, PawnLostCondition.LeftVoluntarily, null);
		}

		// Token: 0x040034F3 RID: 13555
		private PawnPath path;

		// Token: 0x040034F4 RID: 13556
		private const int SwarmRadius = 7;

		// Token: 0x040034F5 RID: 13557
		private const int MoveInterval = 600;
	}
}
