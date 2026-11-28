using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200194B RID: 6475
	public class CompProperties_RadialPulse : CompProperties
	{
		// Token: 0x0600A26F RID: 41583 RVA: 0x003A0DBC File Offset: 0x0039EFBC
		public CompProperties_RadialPulse()
		{
			this.compClass = typeof(CompRadialPulse);
		}

		// Token: 0x04005FF5 RID: 24565
		public int ticksBetweenPulses = 300;

		// Token: 0x04005FF6 RID: 24566
		public int ticksPerPulse = 60;

		// Token: 0x04005FF7 RID: 24567
		public Color color;

		// Token: 0x04005FF8 RID: 24568
		public float radius;
	}
}
