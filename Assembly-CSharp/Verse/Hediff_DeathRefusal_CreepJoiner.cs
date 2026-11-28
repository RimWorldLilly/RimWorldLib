using System;

namespace Verse
{
	// Token: 0x020004A8 RID: 1192
	public class Hediff_DeathRefusal_CreepJoiner : Hediff_DeathRefusal
	{
		// Token: 0x1700064D RID: 1613
		// (get) Token: 0x06002105 RID: 8453 RVA: 0x000C5373 File Offset: 0x000C3573
		public override int MaxUses
		{
			get
			{
				return 4;
			}
		}

		// Token: 0x06002106 RID: 8454 RVA: 0x000C5376 File Offset: 0x000C3576
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			this.usesLeft = this.MaxUses;
		}
	}
}
