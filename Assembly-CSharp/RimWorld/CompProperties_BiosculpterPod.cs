using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200182B RID: 6187
	public class CompProperties_BiosculpterPod : CompProperties
	{
		// Token: 0x06009AC6 RID: 39622 RVA: 0x0037D0E0 File Offset: 0x0037B2E0
		public CompProperties_BiosculpterPod()
		{
			this.compClass = typeof(CompBiosculpterPod);
		}

		// Token: 0x06009AC7 RID: 39623 RVA: 0x0037D0F8 File Offset: 0x0037B2F8
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_BiosculpterPod.<ConfigErrors>d__7 <ConfigErrors>d__ = new CompProperties_BiosculpterPod.<ConfigErrors>d__7(-2);
			<ConfigErrors>d__.<>4__this = this;
			<ConfigErrors>d__.<>3__parentDef = parentDef;
			return <ConfigErrors>d__;
		}

		// Token: 0x04005C08 RID: 23560
		public SoundDef enterSound;

		// Token: 0x04005C09 RID: 23561
		public SoundDef exitSound;

		// Token: 0x04005C0A RID: 23562
		public EffecterDef operatingEffecter;

		// Token: 0x04005C0B RID: 23563
		public EffecterDef readyEffecter;

		// Token: 0x04005C0C RID: 23564
		public Color selectCycleColor;

		// Token: 0x04005C0D RID: 23565
		public float biotunedCycleSpeedFactor;
	}
}
