using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020019CD RID: 6605
	public class CompProperties_TurretGun : CompProperties
	{
		// Token: 0x0600A619 RID: 42521 RVA: 0x003B405C File Offset: 0x003B225C
		public CompProperties_TurretGun()
		{
			this.compClass = typeof(CompTurretGun);
		}

		// Token: 0x0600A61A RID: 42522 RVA: 0x003B407B File Offset: 0x003B227B
		public override IEnumerable<string> ConfigErrors(ThingDef parentDef)
		{
			CompProperties_TurretGun.<ConfigErrors>d__5 <ConfigErrors>d__ = new CompProperties_TurretGun.<ConfigErrors>d__5(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x04006203 RID: 25091
		public ThingDef turretDef;

		// Token: 0x04006204 RID: 25092
		public float angleOffset;

		// Token: 0x04006205 RID: 25093
		public bool autoAttack = true;

		// Token: 0x04006206 RID: 25094
		public List<PawnRenderNodeProperties> renderNodeProperties;
	}
}
