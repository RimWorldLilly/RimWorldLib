using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000B70 RID: 2928
	public class JoyGiver_InteractBuildingInteractionCell : JoyGiver_InteractBuilding
	{
		// Token: 0x06005534 RID: 21812 RVA: 0x001FAD64 File Offset: 0x001F8F64
		protected override Job TryGivePlayJob(Pawn pawn, Thing t)
		{
			if (t.InteractionCell.Standable(t.Map) && !t.IsForbidden(pawn) && !t.InteractionCell.IsForbidden(pawn) && !pawn.Map.pawnDestinationReservationManager.IsReserved(t.InteractionCell) && pawn.CanReserveSittableOrSpot(t.InteractionCell, false))
			{
				return JobMaker.MakeJob(this.def.jobDef, t, t.InteractionCell);
			}
			return null;
		}
	}
}
