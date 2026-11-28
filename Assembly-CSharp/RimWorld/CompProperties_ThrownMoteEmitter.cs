using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019BF RID: 6591
	public class CompProperties_ThrownMoteEmitter : CompProperties
	{
		// Token: 0x0600A575 RID: 42357 RVA: 0x003B019C File Offset: 0x003AE39C
		public CompProperties_ThrownMoteEmitter()
		{
			this.compClass = typeof(CompThrownMoteEmitter);
		}

		// Token: 0x0600A576 RID: 42358 RVA: 0x003B01EE File Offset: 0x003AE3EE
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_ThrownMoteEmitter.<ConfigErrors>d__12 <ConfigErrors>d__ = new CompProperties_ThrownMoteEmitter.<ConfigErrors>d__12(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x040061A7 RID: 24999
		public ThingDef mote;

		// Token: 0x040061A8 RID: 25000
		public Vector3 offsetMin;

		// Token: 0x040061A9 RID: 25001
		public Vector3 offsetMax;

		// Token: 0x040061AA RID: 25002
		public int emissionInterval = -1;

		// Token: 0x040061AB RID: 25003
		public int burstCount = 1;

		// Token: 0x040061AC RID: 25004
		public Color colorA = Color.white;

		// Token: 0x040061AD RID: 25005
		public Color colorB = Color.white;

		// Token: 0x040061AE RID: 25006
		public FloatRange scale = FloatRange.One;

		// Token: 0x040061AF RID: 25007
		public FloatRange rotationRate;

		// Token: 0x040061B0 RID: 25008
		public FloatRange velocityX;

		// Token: 0x040061B1 RID: 25009
		public FloatRange velocityY;
	}
}
