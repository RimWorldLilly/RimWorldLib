using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B28 RID: 6952
	public class Mechlink : ThingWithComps
	{
		// Token: 0x0600B231 RID: 45617 RVA: 0x003EBDA6 File Offset: 0x003E9FA6
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.sentMechsToPlayer, "sentMechsToPlayer", false, false);
		}

		// Token: 0x040066CE RID: 26318
		public bool sentMechsToPlayer;
	}
}
