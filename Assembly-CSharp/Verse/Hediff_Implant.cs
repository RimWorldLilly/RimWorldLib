using System;

namespace Verse
{
	// Token: 0x020004B0 RID: 1200
	public class Hediff_Implant : HediffWithComps
	{
		// Token: 0x0600213E RID: 8510 RVA: 0x000C5FEE File Offset: 0x000C41EE
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			if (base.Part == null)
			{
				Log.Error(this.def.defName + " has null Part. It should be set before PostAdd.");
				return;
			}
		}

		// Token: 0x0600213F RID: 8511 RVA: 0x000C601C File Offset: 0x000C421C
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit && base.Part == null)
			{
				Log.Error(base.GetType().Name + " has null part after loading.");
				this.pawn.health.hediffSet.hediffs.Remove(this);
				return;
			}
		}
	}
}
