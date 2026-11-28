using System;
using Verse;
using Verse.AI;

namespace RimWorld
{
	// Token: 0x02000A00 RID: 2560
	public class JobDriver_ViewArt : JobDriver_VisitJoyThing
	{
		// Token: 0x17000E17 RID: 3607
		// (get) Token: 0x06004D78 RID: 19832 RVA: 0x001DA900 File Offset: 0x001D8B00
		private Thing ArtThing
		{
			get
			{
				return this.job.GetTarget(TargetIndex.A).Thing;
			}
		}

		// Token: 0x06004D79 RID: 19833 RVA: 0x001DA924 File Offset: 0x001D8B24
		protected override void WaitTickAction(int delta)
		{
			float num = this.ArtThing.GetStatValue(StatDefOf.Beauty, true, -1) / this.ArtThing.def.GetStatValueAbstract(StatDefOf.Beauty, null);
			float extraJoyGainFactor = (num > 0f) ? num : 0f;
			this.pawn.GainComfortFromCellIfPossible(delta, false);
			JoyUtility.JoyTickCheckEnd(this.pawn, delta, JoyTickFullJoyAction.EndJob, extraJoyGainFactor, (Building)this.ArtThing);
		}
	}
}
