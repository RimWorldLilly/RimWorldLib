using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020019AB RID: 6571
	public class CompProperties_TargetingBeam : CompProperties
	{
		// Token: 0x0600A4FB RID: 42235 RVA: 0x003AE996 File Offset: 0x003ACB96
		public CompProperties_TargetingBeam()
		{
			this.compClass = typeof(CompTargetingBeam);
		}

		// Token: 0x0400617A RID: 24954
		public int delayTicks;

		// Token: 0x0400617B RID: 24955
		public DamageDef damage;
	}
}
