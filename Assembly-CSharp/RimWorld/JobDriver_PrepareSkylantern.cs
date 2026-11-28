using System;
using System.Linq;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x020009E7 RID: 2535
	public class JobDriver_PrepareSkylantern : JobDriver_GotoAndStandSociallyActive
	{
		// Token: 0x17000E01 RID: 3585
		// (get) Token: 0x06004CD5 RID: 19669 RVA: 0x001D82A4 File Offset: 0x001D64A4
		public override Toil StandToil
		{
			get
			{
				if (!ModLister.CheckIdeology("Skylantern job"))
				{
					return null;
				}
				Toil toil = base.StandToil.WithEffect(EffecterDefOf.MakingSkylantern, () => this.pawn.Position + this.pawn.Rotation.FacingCell, null);
				toil.AddPreInitAction(delegate
				{
					Thing thing = this.pawn.inventory.innerContainer.FirstOrDefault((Thing t) => t.def == this.job.thingDefToCarry && t.stackCount >= this.job.count);
					if (thing != null)
					{
						this.pawn.carryTracker.TryStartCarry(thing, this.job.count, true);
					}
				});
				return toil;
			}
		}
	}
}
