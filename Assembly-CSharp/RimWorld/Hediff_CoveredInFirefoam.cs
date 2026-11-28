using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02002092 RID: 8338
	public class Hediff_CoveredInFirefoam : HediffWithComps
	{
		// Token: 0x0600D22D RID: 53805 RVA: 0x004D34A1 File Offset: 0x004D16A1
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			this.pawn.Drawer.renderer.FirefoamOverlays.coveredInFoam = true;
		}

		// Token: 0x0600D22E RID: 53806 RVA: 0x004D34C5 File Offset: 0x004D16C5
		public override void PostRemoved()
		{
			base.PostRemoved();
			this.pawn.Drawer.renderer.FirefoamOverlays.coveredInFoam = this.pawn.health.hediffSet.HasHediff(HediffDefOf.CoveredInFirefoam, false);
		}

		// Token: 0x0600D22F RID: 53807 RVA: 0x004D3502 File Offset: 0x004D1702
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawn.Drawer.renderer.FirefoamOverlays.coveredInFoam = true;
			}
		}
	}
}
