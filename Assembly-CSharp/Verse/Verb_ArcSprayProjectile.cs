using System;

namespace Verse
{
	// Token: 0x02000743 RID: 1859
	public class Verb_ArcSprayProjectile : Verb_ArcSpray
	{
		// Token: 0x06003AF1 RID: 15089 RVA: 0x00178260 File Offset: 0x00176460
		protected override void HitCell(IntVec3 cell)
		{
			base.HitCell(cell);
			Map map = this.caster.Map;
			if (GenSight.LineOfSight(this.caster.Position, cell, map, true, null, 0, 0))
			{
				((Projectile)GenSpawn.Spawn(this.verbProps.defaultProjectile, this.caster.Position, map, WipeMode.Vanish)).Launch(this.caster, this.caster.DrawPos, cell, cell, ProjectileHitFlags.IntendedTarget, this.preventFriendlyFire, null, null);
			}
		}
	}
}
