using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010D6 RID: 4310
	public class QuestPart_PawnKilled : QuestPart
	{
		// Token: 0x06006BCA RID: 27594 RVA: 0x0025D894 File Offset: 0x0025BA94
		public override void Notify_PawnKilled(Pawn pawn, DamageInfo? dinfo)
		{
			base.Notify_PawnKilled(pawn, dinfo);
			if (pawn.Faction == this.faction && pawn.MapHeld != null && pawn.MapHeld.Parent == this.mapParent)
			{
				Find.SignalManager.SendSignal(new Signal(this.outSignal, false));
			}
		}

		// Token: 0x06006BCB RID: 27595 RVA: 0x0025D8E8 File Offset: 0x0025BAE8
		public override void Notify_FactionRemoved(Faction f)
		{
			if (this.faction == f)
			{
				this.faction = null;
			}
		}

		// Token: 0x06006BCC RID: 27596 RVA: 0x0025D8FA File Offset: 0x0025BAFA
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
			Scribe_Values.Look<string>(ref this.outSignal, "outSignal", null, false);
		}

		// Token: 0x0400448C RID: 17548
		public Faction faction;

		// Token: 0x0400448D RID: 17549
		public MapParent mapParent;

		// Token: 0x0400448E RID: 17550
		public string outSignal;
	}
}
