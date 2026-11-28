using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018DA RID: 6362
	public class CompLeaveFilthOnDestroyed : ThingComp
	{
		// Token: 0x17001B79 RID: 7033
		// (get) Token: 0x06009F95 RID: 40853 RVA: 0x003938FC File Offset: 0x00391AFC
		public CompProperties_LeaveFilthOnDestroyed Props
		{
			get
			{
				return (CompProperties_LeaveFilthOnDestroyed)this.props;
			}
		}

		// Token: 0x17001B7A RID: 7034
		// (get) Token: 0x06009F96 RID: 40854 RVA: 0x00393909 File Offset: 0x00391B09
		private float Radius
		{
			get
			{
				return (float)this.parent.def.size.x / 2f;
			}
		}

		// Token: 0x06009F97 RID: 40855 RVA: 0x00393928 File Offset: 0x00391B28
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			if (mode == DestroyMode.Vanish)
			{
				return;
			}
			foreach (IntVec3 b in GenRadial.RadialPatternInRadius(this.Radius - 0.1f))
			{
				FilthMaker.TryMakeFilth(this.parent.Position + b, previousMap, this.Props.filthDef, this.Props.thickness, FilthSourceFlags.None, true);
			}
		}
	}
}
