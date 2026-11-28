using System;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CFE RID: 3326
	public class LordToil_Venerate : LordToil
	{
		// Token: 0x1700106B RID: 4203
		// (get) Token: 0x06005DBF RID: 23999 RVA: 0x00225579 File Offset: 0x00223779
		private LordToilData_Venerate Data
		{
			get
			{
				return (LordToilData_Venerate)this.data;
			}
		}

		// Token: 0x06005DC0 RID: 24000 RVA: 0x00225588 File Offset: 0x00223788
		public LordToil_Venerate(Thing target)
		{
			this.target = target;
			this.data = new LordToilData_Venerate();
		}

		// Token: 0x06005DC1 RID: 24001 RVA: 0x002255D8 File Offset: 0x002237D8
		public override void UpdateAllDuties()
		{
			LordToilData_Venerate data = this.Data;
			CellRect cellRect = CellRect.CenteredOn(this.target.Position, this.target.def.size.x, this.target.def.size.z);
			CellRect cellRect2 = cellRect.ExpandedBy(this.FarSpectateRectExpansion);
			SpectateRectSide preferredSpectateSide = this.GetPreferredSpectateSide();
			int num = -1;
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (pawn.mindState.duty != null && pawn.mindState.duty.def == DutyDefOf.Pilgrims_Spectate && pawn.mindState.duty.spectateRect == cellRect)
				{
					num = i;
					break;
				}
			}
			if (num >= 0 && this.lord.ownedPawns.Count > 1)
			{
				this.MakePawnSpectate(this.lord.ownedPawns[num], cellRect2, preferredSpectateSide);
				data.currentNearVeneratorTicks = this.VenerateHandoverTicksRange.RandomInRange;
				return;
			}
			num = (data.lastNearVeneratorIndex + 1) % this.lord.ownedPawns.Count;
			for (int j = 0; j < this.lord.ownedPawns.Count; j++)
			{
				Pawn pawn2 = this.lord.ownedPawns[j];
				CellRect cellRect3 = (num == j) ? cellRect : cellRect2;
				if (pawn2.mindState.duty == null || pawn2.mindState.duty.def != DutyDefOf.Pilgrims_Spectate || pawn2.mindState.duty.spectateRect != cellRect3)
				{
					this.MakePawnSpectate(pawn2, cellRect3, preferredSpectateSide);
				}
			}
			data.currentNearVeneratorTicks = this.VenerateCloseTicksRange.RandomInRange;
			data.lastNearVeneratorIndex = num;
		}

		// Token: 0x06005DC2 RID: 24002 RVA: 0x002257BC File Offset: 0x002239BC
		private void MakePawnSpectate(Pawn p, CellRect rect, SpectateRectSide side)
		{
			PawnDuty pawnDuty = new PawnDuty(DutyDefOf.Pilgrims_Spectate);
			pawnDuty.spectateRect = rect;
			pawnDuty.spectateRectAllowedSides = side;
			pawnDuty.locomotion = LocomotionUrgency.Amble;
			pawnDuty.focus = this.target;
			p.mindState.duty = pawnDuty;
			if (p.jobs.curJob != null)
			{
				p.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
		}

		// Token: 0x06005DC3 RID: 24003 RVA: 0x00225822 File Offset: 0x00223A22
		public override void LordToilTick()
		{
			LordToilData_Venerate data = this.Data;
			if (data.currentNearVeneratorTicks < 0)
			{
				this.UpdateAllDuties();
			}
			data.currentNearVeneratorTicks--;
		}

		// Token: 0x06005DC4 RID: 24004 RVA: 0x00225848 File Offset: 0x00223A48
		private SpectateRectSide GetPreferredSpectateSide()
		{
			IntVec3 interactionCell = this.target.InteractionCell;
			IntVec3 intVec = this.target.Position - interactionCell;
			if (intVec.x > 0)
			{
				return SpectateRectSide.Left;
			}
			if (intVec.x < 0)
			{
				return SpectateRectSide.Right;
			}
			if (intVec.z > 0)
			{
				return SpectateRectSide.Down;
			}
			if (intVec.z < 0)
			{
				return SpectateRectSide.Up;
			}
			return SpectateRectSide.All;
		}

		// Token: 0x04003532 RID: 13618
		private Thing target;

		// Token: 0x04003533 RID: 13619
		private int FarSpectateRectExpansion = 2;

		// Token: 0x04003534 RID: 13620
		private IntRange VenerateCloseTicksRange = new IntRange(750, 1500);

		// Token: 0x04003535 RID: 13621
		private IntRange VenerateHandoverTicksRange = new IntRange(60, 120);
	}
}
