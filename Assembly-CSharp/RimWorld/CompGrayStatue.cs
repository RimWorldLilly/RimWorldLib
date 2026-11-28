using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020019ED RID: 6637
	public abstract class CompGrayStatue : ThingComp
	{
		// Token: 0x17001D46 RID: 7494
		// (get) Token: 0x0600A726 RID: 42790 RVA: 0x003B8E21 File Offset: 0x003B7021
		private CompProperties_GrayStatue Props
		{
			get
			{
				return (CompProperties_GrayStatue)this.props;
			}
		}

		// Token: 0x0600A727 RID: 42791 RVA: 0x003B8E2E File Offset: 0x003B702E
		public override void PostPostMake()
		{
			this.triggerRadius = this.Props.triggerRadiusRange.RandomInRange;
		}

		// Token: 0x0600A728 RID: 42792 RVA: 0x003B8E48 File Offset: 0x003B7048
		public override string CompInspectStringExtra()
		{
			return (this.triggered ? "Activated" : "ActivatedByProximity").Translate().CapitalizeFirst();
		}

		// Token: 0x0600A729 RID: 42793 RVA: 0x003B8E7B File Offset: 0x003B707B
		public override void CompTick()
		{
			if (this.triggered)
			{
				return;
			}
			if (this.parent.IsHashIntervalTick(250))
			{
				this.CheckTrigger();
			}
		}

		// Token: 0x0600A72A RID: 42794 RVA: 0x003B8EA0 File Offset: 0x003B70A0
		private void CheckTrigger()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			foreach (IntVec3 intVec in GenRadial.RadialCellsAround(this.parent.Position, this.triggerRadius, false))
			{
				if (intVec.InBounds(this.parent.Map) && GenSight.LineOfSight(this.parent.Position, intVec, this.parent.Map))
				{
					foreach (Thing thing in intVec.GetThingList(this.parent.Map))
					{
						Pawn pawn = thing as Pawn;
						if (pawn != null && pawn.IsColonist)
						{
							this.triggered = true;
							this.Trigger(pawn);
							TaggedString label = this.Props.letterLabel.Formatted(pawn.Named("PAWN"));
							TaggedString text = this.Props.letterText.Formatted(pawn.Named("PAWN"));
							Find.LetterStack.ReceiveLetter(label, text, this.Props.letterDef ?? LetterDefOf.ThreatBig, pawn, null, null, null, null, 0, true);
							return;
						}
					}
				}
			}
		}

		// Token: 0x0600A72B RID: 42795
		protected abstract void Trigger(Pawn target);

		// Token: 0x0600A72C RID: 42796 RVA: 0x003B9034 File Offset: 0x003B7234
		public override void PostExposeData()
		{
			Scribe_Values.Look<float>(ref this.triggerRadius, "triggerRadius", 0f, false);
			Scribe_Values.Look<bool>(ref this.triggered, "triggered", false, false);
		}

		// Token: 0x04006278 RID: 25208
		private float triggerRadius;

		// Token: 0x04006279 RID: 25209
		private bool triggered;
	}
}
