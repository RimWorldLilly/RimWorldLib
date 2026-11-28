using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018F0 RID: 6384
	public class CompProperties_MechPowerCell : CompProperties
	{
		// Token: 0x0600A013 RID: 40979 RVA: 0x003958A5 File Offset: 0x00393AA5
		public CompProperties_MechPowerCell()
		{
			this.compClass = typeof(CompMechPowerCell);
		}

		// Token: 0x04005EAB RID: 24235
		public int totalPowerTicks = 2500;

		// Token: 0x04005EAC RID: 24236
		public bool killWhenDepleted = true;

		// Token: 0x04005EAD RID: 24237
		public bool showGizmoOnNonPlayerControlled;

		// Token: 0x04005EAE RID: 24238
		[MustTranslate]
		public string labelOverride;

		// Token: 0x04005EAF RID: 24239
		[MustTranslate]
		public string tooltipOverride;
	}
}
