using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A01 RID: 2561
	public class JobDriver_VisitGrave : JobDriver_VisitJoyThing
	{
		// Token: 0x17000E18 RID: 3608
		// (get) Token: 0x06004D7B RID: 19835 RVA: 0x001DA99C File Offset: 0x001D8B9C
		private Building_Grave Grave
		{
			get
			{
				return (Building_Grave)this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06004D7C RID: 19836 RVA: 0x001DA9C4 File Offset: 0x001D8BC4
		protected override void WaitTickAction(int delta)
		{
			float num = 1f;
			Room room = this.pawn.GetRoom(RegionType.Set_All);
			if (room != null)
			{
				num *= room.GetStat(RoomStatDefOf.GraveVisitingJoyGainFactor);
			}
			this.pawn.GainComfortFromCellIfPossible(delta, false);
			JoyUtility.JoyTickCheckEnd(this.pawn, delta, JoyTickFullJoyAction.EndJob, num, this.Grave);
		}

		// Token: 0x06004D7D RID: 19837 RVA: 0x001DAA18 File Offset: 0x001D8C18
		public override object[] TaleParameters()
		{
			object[] array = new object[2];
			array[0] = this.pawn;
			int num = 1;
			Corpse corpse = this.Grave.Corpse;
			array[num] = ((corpse != null) ? corpse.InnerPawn : null);
			return array;
		}
	}
}
