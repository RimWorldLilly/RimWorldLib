using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A67 RID: 6759
	public class Building_BlastingCharge : Building
	{
		// Token: 0x0600A9D8 RID: 43480 RVA: 0x003C3694 File Offset: 0x003C1894
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Building_BlastingCharge.<GetGizmos>d__0 <GetGizmos>d__ = new Building_BlastingCharge.<GetGizmos>d__0(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600A9D9 RID: 43481 RVA: 0x003C36A4 File Offset: 0x003C18A4
		private void Command_Detonate()
		{
			base.GetComp<CompExplosive>().StartWick(null);
		}
	}
}
