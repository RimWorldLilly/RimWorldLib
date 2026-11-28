using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A6B RID: 6763
	public class Building_TrapExplosive : Building_Trap
	{
		// Token: 0x0600AA06 RID: 43526 RVA: 0x003C4060 File Offset: 0x003C2260
		protected override void SpringSub(Pawn p)
		{
			base.GetComp<CompExplosive>().StartWick(p);
		}
	}
}
