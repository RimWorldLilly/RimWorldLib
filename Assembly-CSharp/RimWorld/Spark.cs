using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B48 RID: 6984
	public class Spark : Projectile
	{
		// Token: 0x0600B356 RID: 45910 RVA: 0x003F34EC File Offset: 0x003F16EC
		protected override void Impact(Thing hitThing, bool blockedByShield = false)
		{
			Map map = base.Map;
			base.Impact(hitThing, blockedByShield);
			Thing instigator = this.launcher;
			Fire fire = this.launcher as Fire;
			if (fire != null)
			{
				instigator = fire.instigator;
			}
			FireUtility.TryStartFireIn(base.Position, map, 0.1f, instigator, null);
		}
	}
}
