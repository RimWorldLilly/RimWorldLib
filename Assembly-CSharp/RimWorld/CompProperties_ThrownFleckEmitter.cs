using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019BD RID: 6589
	public class CompProperties_ThrownFleckEmitter : CompProperties
	{
		// Token: 0x0600A56B RID: 42347 RVA: 0x003AFE4D File Offset: 0x003AE04D
		public CompProperties_ThrownFleckEmitter()
		{
			this.compClass = typeof(CompThrownFleckEmitter);
		}

		// Token: 0x0600A56C RID: 42348 RVA: 0x003AFE89 File Offset: 0x003AE089
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_ThrownFleckEmitter.<ConfigErrors>d__12 <ConfigErrors>d__ = new CompProperties_ThrownFleckEmitter.<ConfigErrors>d__12(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x0400619A RID: 24986
		public FleckDef fleck;

		// Token: 0x0400619B RID: 24987
		public Vector3 offsetMin;

		// Token: 0x0400619C RID: 24988
		public Vector3 offsetMax;

		// Token: 0x0400619D RID: 24989
		public int emissionInterval = -1;

		// Token: 0x0400619E RID: 24990
		public int burstCount = 1;

		// Token: 0x0400619F RID: 24991
		public Color colorA = Color.white;

		// Token: 0x040061A0 RID: 24992
		public Color colorB = Color.white;

		// Token: 0x040061A1 RID: 24993
		public FloatRange scale;

		// Token: 0x040061A2 RID: 24994
		public FloatRange rotationRate;

		// Token: 0x040061A3 RID: 24995
		public FloatRange velocityX;

		// Token: 0x040061A4 RID: 24996
		public FloatRange velocityY;
	}
}
