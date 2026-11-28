using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A95 RID: 6805
	public class Building_AncientCryptosleepPod : Building_AncientCryptosleepCasket
	{
		// Token: 0x0600ABB0 RID: 43952 RVA: 0x003CA05F File Offset: 0x003C825F
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn myPawn)
		{
			return Enumerable.Empty<FloatMenuOption>();
		}
	}
}
