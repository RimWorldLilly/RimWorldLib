using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000550 RID: 1360
	public abstract class PawnTrigger : Thing
	{
		// Token: 0x06002876 RID: 10358 RVA: 0x000F9F9A File Offset: 0x000F819A
		protected bool TriggeredBy(Thing thing)
		{
			return thing.def.category == ThingCategory.Pawn && thing.def.race.intelligence == Intelligence.Humanlike && thing.Faction == Faction.OfPlayer;
		}

		// Token: 0x06002877 RID: 10359 RVA: 0x000F9FCC File Offset: 0x000F81CC
		public void ActivatedBy(Pawn p)
		{
			Find.SignalManager.SendSignal(new Signal(this.signalTag, p.Named("SUBJECT")));
			if (!base.Destroyed)
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06002878 RID: 10360 RVA: 0x000F9FFD File Offset: 0x000F81FD
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.signalTag, "signalTag", null, false);
		}

		// Token: 0x04001C94 RID: 7316
		public string signalTag;
	}
}
