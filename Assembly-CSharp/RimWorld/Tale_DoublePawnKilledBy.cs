using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020017DD RID: 6109
	public class Tale_DoublePawnKilledBy : Tale_DoublePawn
	{
		// Token: 0x06009882 RID: 39042 RVA: 0x003745C7 File Offset: 0x003727C7
		public Tale_DoublePawnKilledBy()
		{
		}

		// Token: 0x06009883 RID: 39043 RVA: 0x003745CF File Offset: 0x003727CF
		public Tale_DoublePawnKilledBy(Pawn victim, DamageInfo dinfo) : base(victim, null)
		{
			if (dinfo.Instigator != null && dinfo.Instigator is Pawn)
			{
				this.secondPawnData = TaleData_Pawn.GenerateFrom((Pawn)dinfo.Instigator);
			}
		}
	}
}
