using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B16 RID: 6934
	public class FilthRevenantSmear : Filth
	{
		// Token: 0x0600B156 RID: 45398 RVA: 0x003E7B50 File Offset: 0x003E5D50
		private void SendLetter(Pawn triggerer)
		{
			Find.LetterStack.ReceiveLetter("LetterLabelRevenantSmearDiscovered".Translate(), "LetterRevenantSmearDiscovered".Translate(triggerer.Named("PAWN")), LetterDefOf.ThreatSmall, this, null, null, null, null, 0, true);
			this.revenant.TryGetComp<CompRevenant>().revenantSmearNotified = true;
		}

		// Token: 0x0600B157 RID: 45399 RVA: 0x003E7BA8 File Offset: 0x003E5DA8
		protected override void Tick()
		{
			Pawn pawn = this.revenant;
			if (((pawn != null) ? pawn.mindState : null) == null)
			{
				return;
			}
			if (this.revenant.TryGetComp<CompRevenant>().revenantSmearNotified)
			{
				return;
			}
			if (this.revenant.mindState.lastBecameVisibleTick > 0 || this.revenant.mindState.lastForcedVisibleTick > 0)
			{
				return;
			}
			if (this.IsHashIntervalTick(60))
			{
				int num = GenRadial.NumCellsInRadius(4f);
				for (int i = 0; i < num; i++)
				{
					IntVec3 c = base.Position + GenRadial.RadialPattern[i];
					if (c.InBounds(base.Map))
					{
						List<Thing> thingList = c.GetThingList(base.Map);
						for (int j = 0; j < thingList.Count; j++)
						{
							Pawn pawn2 = thingList[j] as Pawn;
							if (pawn2 != null && pawn2.IsColonistPlayerControlled)
							{
								this.SendLetter(pawn2);
								return;
							}
						}
					}
				}
			}
		}

		// Token: 0x0400666E RID: 26222
		private const int Radius = 4;

		// Token: 0x0400666F RID: 26223
		public Pawn revenant;
	}
}
