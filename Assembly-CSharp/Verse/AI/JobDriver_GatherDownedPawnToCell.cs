using System;
using Verse.AI.Group;

namespace Verse.AI
{
	// Token: 0x02000835 RID: 2101
	public class JobDriver_GatherDownedPawnToCell : JobDriver_HaulToCell
	{
		// Token: 0x17000C93 RID: 3219
		// (get) Token: 0x060041C6 RID: 16838 RVA: 0x001A07E7 File Offset: 0x0019E9E7
		protected override bool DropCarriedThingIfNotTarget
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060041C7 RID: 16839 RVA: 0x001A07EA File Offset: 0x0019E9EA
		protected override Toil BeforeDrop()
		{
			return Toils_General.DoAtomic(delegate
			{
				Pawn pawn = this.pawn.carryTracker.CarriedThing as Pawn;
				if (pawn == null)
				{
					Log.Error("Carried thing wasn't a Pawn in JobDriver_GatherDownedPawnToCell.");
					return;
				}
				LordToil curLordToil = this.pawn.GetLord().CurLordToil;
				if (curLordToil == null)
				{
					return;
				}
				curLordToil.Notify_ReachedDutyLocation(pawn);
			});
		}
	}
}
