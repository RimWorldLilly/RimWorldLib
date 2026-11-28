using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018C2 RID: 6338
	public class CompProperties_HunterDrone : CompProperties
	{
		// Token: 0x06009EE8 RID: 40680 RVA: 0x00390566 File Offset: 0x0038E766
		public CompProperties_HunterDrone()
		{
			this.compClass = typeof(CompHunterDrone);
		}

		// Token: 0x06009EE9 RID: 40681 RVA: 0x00390591 File Offset: 0x0038E791
		public override void ResolveReferences(ThingDef parentDef)
		{
			base.ResolveReferences(parentDef);
			if (this.explosionDamageType == null)
			{
				this.explosionDamageType = DamageDefOf.Bomb;
			}
		}

		// Token: 0x04005E0A RID: 24074
		public float explosionRadius = 1.9f;

		// Token: 0x04005E0B RID: 24075
		public DamageDef explosionDamageType;

		// Token: 0x04005E0C RID: 24076
		public int explosionDamageAmount = 50;
	}
}
