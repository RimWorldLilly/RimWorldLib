using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001883 RID: 6275
	public abstract class CompFireOverlayBase : ThingComp
	{
		// Token: 0x17001AE3 RID: 6883
		// (get) Token: 0x06009D1A RID: 40218 RVA: 0x003880ED File Offset: 0x003862ED
		public CompProperties_FireOverlay Props
		{
			get
			{
				return (CompProperties_FireOverlay)this.props;
			}
		}

		// Token: 0x17001AE4 RID: 6884
		// (get) Token: 0x06009D1B RID: 40219 RVA: 0x003880FC File Offset: 0x003862FC
		public float FireSize
		{
			get
			{
				if (this.startedGrowingAtTick < 0)
				{
					return this.Props.fireSize;
				}
				return Mathf.Lerp(this.Props.fireSize, this.Props.finalFireSize, (float)(GenTicks.TicksAbs - this.startedGrowingAtTick) / this.Props.fireGrowthDurationTicks);
			}
		}

		// Token: 0x06009D1C RID: 40220 RVA: 0x00388152 File Offset: 0x00386352
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.startedGrowingAtTick, "startedGrowingAtTick", -1, false);
		}

		// Token: 0x04005D1F RID: 23839
		protected int startedGrowingAtTick = -1;
	}
}
