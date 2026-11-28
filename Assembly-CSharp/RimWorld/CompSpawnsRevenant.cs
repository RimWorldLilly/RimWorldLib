using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001990 RID: 6544
	public class CompSpawnsRevenant : ThingComp
	{
		// Token: 0x0600A444 RID: 42052 RVA: 0x003AAFD3 File Offset: 0x003A91D3
		public override void PostExposeData()
		{
			Scribe_Values.Look<int>(ref this.spawnTick, "spawnTick", 0, false);
		}

		// Token: 0x0600A445 RID: 42053 RVA: 0x003AAFE8 File Offset: 0x003A91E8
		public override void CompTickRare()
		{
			if (this.spawnTick > 0 && Find.TickManager.TicksGame > this.spawnTick && this.parent.MapHeld != null)
			{
				Pawn pawn = PawnGenerator.GeneratePawn(PawnKindDefOf.Revenant, Faction.OfEntities, null);
				GenSpawn.Spawn(pawn, this.parent.PositionHeld, this.parent.MapHeld, WipeMode.Vanish);
				CompRevenant compRevenant = pawn.TryGetComp<CompRevenant>();
				compRevenant.Invisibility.BecomeVisible(true);
				compRevenant.SetState(RevenantState.Escape);
				Find.LetterStack.ReceiveLetter("LetterLabelRevenantEmergence".Translate(), "LetterRevenantEmergence".Translate(), LetterDefOf.ThreatBig, pawn, null, null, null, null, 0, true);
				this.parent.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x04006119 RID: 24857
		public int spawnTick = -1;
	}
}
