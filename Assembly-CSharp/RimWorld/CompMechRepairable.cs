using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020018F6 RID: 6390
	public class CompMechRepairable : ThingComp
	{
		// Token: 0x17001B9D RID: 7069
		// (get) Token: 0x0600A030 RID: 41008 RVA: 0x00396093 File Offset: 0x00394293
		public CompProperties_MechRepairable Props
		{
			get
			{
				return (CompProperties_MechRepairable)this.props;
			}
		}

		// Token: 0x0600A031 RID: 41009 RVA: 0x003960A0 File Offset: 0x003942A0
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompMechRepairable.<CompGetGizmosExtra>d__3 <CompGetGizmosExtra>d__ = new CompMechRepairable.<CompGetGizmosExtra>d__3(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A032 RID: 41010 RVA: 0x003960B0 File Offset: 0x003942B0
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.autoRepair, "autoRepair", false, false);
		}

		// Token: 0x04005EC3 RID: 24259
		public bool autoRepair;
	}
}
