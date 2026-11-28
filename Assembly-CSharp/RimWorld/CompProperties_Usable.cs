using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EEA RID: 3818
	public class CompProperties_Usable : CompProperties
	{
		// Token: 0x060063C8 RID: 25544 RVA: 0x0023C5A0 File Offset: 0x0023A7A0
		public CompProperties_Usable()
		{
			this.compClass = typeof(CompUsable);
		}

		// Token: 0x0400382F RID: 14383
		public JobDef useJob;

		// Token: 0x04003830 RID: 14384
		[MustTranslate]
		public string useLabel;

		// Token: 0x04003831 RID: 14385
		[MustTranslate]
		public string useMessage;

		// Token: 0x04003832 RID: 14386
		public int useDuration = 100;

		// Token: 0x04003833 RID: 14387
		public HediffDef userMustHaveHediff;

		// Token: 0x04003834 RID: 14388
		public MenuOptionPriority floatMenuOptionPriority = MenuOptionPriority.Default;

		// Token: 0x04003835 RID: 14389
		public FactionDef floatMenuFactionIcon;

		// Token: 0x04003836 RID: 14390
		public ThingDef warmupMote;

		// Token: 0x04003837 RID: 14391
		public ThingDef finalizeMote;

		// Token: 0x04003838 RID: 14392
		public bool ignoreOtherReservations;

		// Token: 0x04003839 RID: 14393
		public bool showUseGizmo;

		// Token: 0x0400383A RID: 14394
		public List<MutantDef> allowedMutants = new List<MutantDef>();

		// Token: 0x0400383B RID: 14395
		public List<PlanetLayerDef> layerWhitelist = new List<PlanetLayerDef>();

		// Token: 0x0400383C RID: 14396
		public List<PlanetLayerDef> layerBlacklist = new List<PlanetLayerDef>();
	}
}
