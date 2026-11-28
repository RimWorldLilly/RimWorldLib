using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200194D RID: 6477
	public class CompProperties_Rechargeable : CompProperties
	{
		// Token: 0x0600A277 RID: 41591 RVA: 0x003A0F4E File Offset: 0x0039F14E
		public CompProperties_Rechargeable()
		{
			this.compClass = typeof(CompRechargeable);
		}

		// Token: 0x0600A278 RID: 41592 RVA: 0x003A0F66 File Offset: 0x0039F166
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_Rechargeable.<ConfigErrors>d__4 <ConfigErrors>d__ = new CompProperties_Rechargeable.<ConfigErrors>d__4(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x04005FFC RID: 24572
		public int ticksToRecharge;

		// Token: 0x04005FFD RID: 24573
		public SoundDef chargedSoundDef;

		// Token: 0x04005FFE RID: 24574
		public SoundDef dischargeSoundDef;
	}
}
