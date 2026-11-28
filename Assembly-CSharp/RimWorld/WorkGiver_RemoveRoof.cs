using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000BCD RID: 3021
	public class WorkGiver_RemoveRoof : WorkGiver_Scanner
	{
		// Token: 0x17000F33 RID: 3891
		// (get) Token: 0x060056F6 RID: 22262 RVA: 0x002049DD File Offset: 0x00202BDD
		public override bool Prioritized
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060056F7 RID: 22263 RVA: 0x002049E0 File Offset: 0x00202BE0
		public override IEnumerable<IntVec3> PotentialWorkCellsGlobal(Pawn pawn)
		{
			return pawn.Map.areaManager.NoRoof.ActiveCells;
		}

		// Token: 0x060056F8 RID: 22264 RVA: 0x002049F7 File Offset: 0x00202BF7
		public override bool ShouldSkip(Pawn pawn, bool forced = false)
		{
			return pawn.Map.areaManager.NoRoof.TrueCount == 0;
		}

		// Token: 0x17000F34 RID: 3892
		// (get) Token: 0x060056F9 RID: 22265 RVA: 0x00204A11 File Offset: 0x00202C11
		public override PathEndMode PathEndMode
		{
			get
			{
				return PathEndMode.ClosestTouch;
			}
		}

		// Token: 0x060056FA RID: 22266 RVA: 0x00204A14 File Offset: 0x00202C14
		public override bool HasJobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return pawn.Map.areaManager.NoRoof[c] && c.Roofed(pawn.Map) && pawn.CanReserve(c, 1, -1, ReservationLayerDefOf.Ceiling, forced);
		}

		// Token: 0x060056FB RID: 22267 RVA: 0x00204A64 File Offset: 0x00202C64
		public override Job JobOnCell(Pawn pawn, IntVec3 c, bool forced = false)
		{
			return JobMaker.MakeJob(JobDefOf.RemoveRoof, c, c);
		}

		// Token: 0x060056FC RID: 22268 RVA: 0x00204A7C File Offset: 0x00202C7C
		public override float GetPriority(Pawn pawn, TargetInfo t)
		{
			IntVec3 cell = t.Cell;
			int num = 0;
			for (int i = 0; i < 8; i++)
			{
				IntVec3 c = cell + GenAdj.AdjacentCells[i];
				if (c.InBounds(t.Map))
				{
					Building edifice = c.GetEdifice(t.Map);
					if (edifice != null && edifice.def.holdsRoof)
					{
						return -60f;
					}
					if (c.Roofed(pawn.Map))
					{
						num++;
					}
				}
			}
			return (float)(-(float)Mathf.Min(num, 3));
		}
	}
}
