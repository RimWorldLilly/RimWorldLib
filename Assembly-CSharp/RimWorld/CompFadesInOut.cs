using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001880 RID: 6272
	public class CompFadesInOut : ThingComp
	{
		// Token: 0x17001AE0 RID: 6880
		// (get) Token: 0x06009D10 RID: 40208 RVA: 0x00387EB9 File Offset: 0x003860B9
		public CompProperties_FadesInOut Props
		{
			get
			{
				return (CompProperties_FadesInOut)this.props;
			}
		}

		// Token: 0x06009D11 RID: 40209 RVA: 0x00387EC6 File Offset: 0x003860C6
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.Spawned)
			{
				this.ageTicks++;
			}
		}

		// Token: 0x06009D12 RID: 40210 RVA: 0x00387EEC File Offset: 0x003860EC
		public float Opacity()
		{
			float num = this.ageTicks.TicksToSeconds();
			if (num <= this.Props.fadeInSecs)
			{
				if (this.Props.fadeInSecs > 0f)
				{
					return num / this.Props.fadeInSecs;
				}
				return 1f;
			}
			else
			{
				if (num <= this.Props.fadeInSecs + this.Props.solidTimeSecs)
				{
					return 1f;
				}
				if (this.Props.fadeOutSecs > 0f)
				{
					return 1f - Mathf.InverseLerp(this.Props.fadeInSecs + this.Props.solidTimeSecs, this.Props.fadeInSecs + this.Props.solidTimeSecs + this.Props.fadeOutSecs, num);
				}
				return 1f;
			}
		}

		// Token: 0x06009D13 RID: 40211 RVA: 0x00387FB7 File Offset: 0x003861B7
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.ageTicks, "ageTicks", 0, false);
		}

		// Token: 0x04005D1A RID: 23834
		private int ageTicks;
	}
}
