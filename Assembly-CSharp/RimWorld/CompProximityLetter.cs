using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001944 RID: 6468
	public class CompProximityLetter : ThingComp
	{
		// Token: 0x17001C11 RID: 7185
		// (get) Token: 0x0600A246 RID: 41542 RVA: 0x003A0030 File Offset: 0x0039E230
		public CompProperties_ProximityLetter Props
		{
			get
			{
				return (CompProperties_ProximityLetter)this.props;
			}
		}

		// Token: 0x0600A247 RID: 41543 RVA: 0x003A0040 File Offset: 0x0039E240
		public void SendLetter(Pawn triggerer)
		{
			Find.LetterStack.ReceiveLetter(this.Props.letterLabel.Formatted(triggerer.Named("PAWN")), this.Props.letterText.Formatted(triggerer.Named("PAWN")), this.Props.letterDef, this.parent, null, null, null, null, 0, true);
			this.letterSent = true;
		}

		// Token: 0x0600A248 RID: 41544 RVA: 0x003A00B0 File Offset: 0x0039E2B0
		public override void CompTick()
		{
			base.CompTick();
			if (this.letterSent || Find.Anomaly.Level > 0)
			{
				return;
			}
			if (!this.parent.IsHashIntervalTick(60))
			{
				return;
			}
			Map map = this.parent.Map;
			int num = GenRadial.NumCellsInRadius((float)this.Props.radius);
			for (int i = 0; i < num; i++)
			{
				IntVec3 c = this.parent.Position + GenRadial.RadialPattern[i];
				if (c.InBounds(map))
				{
					List<Thing> thingList = c.GetThingList(map);
					for (int j = 0; j < thingList.Count; j++)
					{
						Pawn pawn = thingList[j] as Pawn;
						if (pawn != null && pawn.IsColonistPlayerControlled && pawn.health.capacities.CapableOf(PawnCapacityDefOf.Sight) && pawn.Awake() && GenSight.LineOfSightToThing(pawn.Position, this.parent, this.parent.Map, false, null))
						{
							this.SendLetter(pawn);
							return;
						}
					}
				}
			}
		}

		// Token: 0x0600A249 RID: 41545 RVA: 0x003A01C8 File Offset: 0x0039E3C8
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.letterSent, "letterSent", false, false);
		}

		// Token: 0x04005FE5 RID: 24549
		public bool letterSent;
	}
}
