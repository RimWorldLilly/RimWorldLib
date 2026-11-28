using System;
using RimWorld.Planet;
using Verse;
using Verse.AI;
using Verse.AI.Group;

namespace RimWorld
{
	// Token: 0x02000CB8 RID: 3256
	public class LordToil_PrepareCaravan_GatherItems : LordToil
	{
		// Token: 0x1700101D RID: 4125
		// (get) Token: 0x06005C7C RID: 23676 RVA: 0x0021FB85 File Offset: 0x0021DD85
		public override float? CustomWakeThreshold
		{
			get
			{
				return new float?(0.5f);
			}
		}

		// Token: 0x1700101E RID: 4126
		// (get) Token: 0x06005C7D RID: 23677 RVA: 0x0021FB91 File Offset: 0x0021DD91
		public override bool AllowRestingInBed
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06005C7E RID: 23678 RVA: 0x0021FB94 File Offset: 0x0021DD94
		public LordToil_PrepareCaravan_GatherItems(IntVec3 meetingPoint)
		{
			this.meetingPoint = meetingPoint;
		}

		// Token: 0x06005C7F RID: 23679 RVA: 0x0021FBA4 File Offset: 0x0021DDA4
		public override void UpdateAllDuties()
		{
			for (int i = 0; i < this.lord.ownedPawns.Count; i++)
			{
				Pawn pawn = this.lord.ownedPawns[i];
				if (pawn.IsColonist)
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_GatherItems);
				}
				else
				{
					pawn.mindState.duty = new PawnDuty(DutyDefOf.PrepareCaravan_Wait, this.meetingPoint, -1f);
				}
			}
		}

		// Token: 0x06005C80 RID: 23680 RVA: 0x0021FC24 File Offset: 0x0021DE24
		public override void LordToilTick()
		{
			base.LordToilTick();
			if (Find.TickManager.TicksGame % 120 == 0 && CaravanFormingUtility.AllItemsLoadedOntoCaravan(this.lord, base.Map))
			{
				foreach (Pawn pawn in this.lord.ownedPawns)
				{
					pawn.inventory.ClearHaulingCaravanCache();
				}
				this.lord.ReceiveMemo("AllItemsGathered");
			}
		}

		// Token: 0x040034C7 RID: 13511
		private IntVec3 meetingPoint;
	}
}
