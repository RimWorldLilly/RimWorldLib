using System;
using System.Collections.Generic;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000C5E RID: 3166
	public class LordToil_BestowingCeremony_Perform : LordToil_Wait
	{
		// Token: 0x17000FB3 RID: 4019
		// (get) Token: 0x060059CE RID: 22990 RVA: 0x00210950 File Offset: 0x0020EB50
		public LordToilData_Gathering Data
		{
			get
			{
				return (LordToilData_Gathering)this.data;
			}
		}

		// Token: 0x060059CF RID: 22991 RVA: 0x0021095D File Offset: 0x0020EB5D
		public LordToil_BestowingCeremony_Perform(Pawn target, Pawn bestower) : base(true)
		{
			this.target = target;
			this.bestower = bestower;
			this.data = new LordToilData_Gathering();
		}

		// Token: 0x060059D0 RID: 22992 RVA: 0x0021097F File Offset: 0x0020EB7F
		public override void Init()
		{
			base.Init();
			if (!this.target.Awake())
			{
				RestUtility.WakeUp(this.target, true);
			}
		}

		// Token: 0x060059D1 RID: 22993 RVA: 0x002109A0 File Offset: 0x0020EBA0
		public override void LordToilTick()
		{
			List<Pawn> ownedPawns = this.lord.ownedPawns;
			for (int i = 0; i < ownedPawns.Count; i++)
			{
				if (GatheringsUtility.InGatheringArea(ownedPawns[i].Position, this.target.Position, base.Map))
				{
					if (!this.Data.presentForTicks.ContainsKey(ownedPawns[i]))
					{
						this.Data.presentForTicks.Add(ownedPawns[i], 0);
					}
					Dictionary<Pawn, int> presentForTicks = this.Data.presentForTicks;
					Pawn key = ownedPawns[i];
					int num = presentForTicks[key];
					presentForTicks[key] = num + 1;
				}
			}
		}

		// Token: 0x060059D2 RID: 22994 RVA: 0x00210A48 File Offset: 0x0020EC48
		public override void UpdateAllDuties()
		{
			IntVec3 spot = ((LordJob_BestowingCeremony)this.lord.LordJob).GetSpot();
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (!pawn.Awake())
				{
					RestUtility.WakeUp(pawn, true);
				}
				if (pawn == this.bestower)
				{
					PawnDuty pawnDuty = new PawnDuty(DutyDefOf.Idle);
					pawnDuty.focus = spot;
					pawn.mindState.duty = pawnDuty;
				}
				else if (pawn == this.target)
				{
					PawnDuty duty = new PawnDuty(DutyDefOf.Bestow, this.bestower, spot, -1f);
					pawn.mindState.duty = duty;
				}
				else
				{
					PawnDuty pawnDuty2 = new PawnDuty(DutyDefOf.Spectate, spot, -1f);
					pawnDuty2.spectateRect = CellRect.CenteredOn(spot, 0);
					pawnDuty2.spectateRectAllowedSides = SpectateRectSide.All;
					pawnDuty2.spectateDistance = new IntRange(2, 2);
					pawn.mindState.duty = pawnDuty2;
				}
				Pawn_JobTracker jobs = pawn.jobs;
				if (jobs != null)
				{
					jobs.CheckForJobOverride(0f, true);
				}
			}
		}

		// Token: 0x04003378 RID: 13176
		public Pawn target;

		// Token: 0x04003379 RID: 13177
		public Pawn bestower;
	}
}
