using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B3E RID: 6974
	public class Plant_Boomshroom : Plant
	{
		// Token: 0x0600B335 RID: 45877 RVA: 0x003F27D8 File Offset: 0x003F09D8
		public override void Kill(DamageInfo? dinfo = null, Hediff exactCulprit = null)
		{
			if (!base.Destroyed && this.HarvestableNow)
			{
				GenExplosion.DoExplosion(base.Position, base.Map, 4.9f, DamageDefOf.Flame, this, -1, -1f, null, null, null, null, null, 0f, 1, null, null, 255, false, null, 0f, 1, 0f, false, null, null, null, true, 1f, 0f, true, null, 1f, null, null, null, null);
			}
			base.Kill(dinfo, exactCulprit);
		}
	}
}
