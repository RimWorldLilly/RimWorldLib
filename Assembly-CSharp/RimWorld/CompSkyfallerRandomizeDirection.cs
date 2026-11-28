using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A01 RID: 6657
	public class CompSkyfallerRandomizeDirection : ThingComp
	{
		// Token: 0x17001D53 RID: 7507
		// (get) Token: 0x0600A798 RID: 42904 RVA: 0x003BB11A File Offset: 0x003B931A
		public CompProperties_SkyfallerRandomizeDirection Props
		{
			get
			{
				return (CompProperties_SkyfallerRandomizeDirection)this.props;
			}
		}

		// Token: 0x17001D54 RID: 7508
		// (get) Token: 0x0600A799 RID: 42905 RVA: 0x003BB127 File Offset: 0x003B9327
		public Skyfaller Skyfaller
		{
			get
			{
				return (Skyfaller)this.parent;
			}
		}

		// Token: 0x17001D55 RID: 7509
		// (get) Token: 0x0600A79A RID: 42906 RVA: 0x003BB134 File Offset: 0x003B9334
		public Vector3 Offset
		{
			get
			{
				return this.currentOffset;
			}
		}

		// Token: 0x17001D56 RID: 7510
		// (get) Token: 0x0600A79B RID: 42907 RVA: 0x003BB13C File Offset: 0x003B933C
		public float ExtraDrawAngle
		{
			get
			{
				return Mathf.Lerp(this.lastAngle, this.currentAngle, Mathf.Clamp((float)(Find.TickManager.TicksGame + CompSkyfallerRandomizeDirection.DirectionChangeBlendDuration / 2 - this.lastDirectionChange), 0f, (float)CompSkyfallerRandomizeDirection.DirectionChangeBlendDuration) / (float)CompSkyfallerRandomizeDirection.DirectionChangeBlendDuration) / 2f;
			}
		}

		// Token: 0x0600A79C RID: 42908 RVA: 0x003BB191 File Offset: 0x003B9391
		public override void PostPostMake()
		{
			base.PostPostMake();
			this.initialAngle = this.Skyfaller.angle;
			this.directionChangeInterval = this.Props.directionChangeInterval.RandomInRange;
			this.lastDirectionChange = Find.TickManager.TicksGame;
		}

		// Token: 0x0600A79D RID: 42909 RVA: 0x003BB1D0 File Offset: 0x003B93D0
		public override void CompTick()
		{
			base.CompTick();
			if (this.parent.IsHashIntervalTick(this.directionChangeInterval))
			{
				this.lastAngle = this.currentAngle;
				this.currentAngle = Rand.Value * this.Props.maxDeviationFromStartingAngle * (float)Rand.Sign;
				this.lastDirectionChange = Find.TickManager.TicksGame;
			}
			Quaternion rotation = Quaternion.AngleAxis(this.currentAngle - this.initialAngle, Vector3.up);
			this.currentOffset += rotation * Vector3.forward * Time.deltaTime;
		}

		// Token: 0x0600A79E RID: 42910 RVA: 0x003BB270 File Offset: 0x003B9470
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.directionChangeInterval, "directionChangeInterval", 0, false);
			Scribe_Values.Look<float>(ref this.currentAngle, "currentAngle", 0f, false);
			Scribe_Values.Look<float>(ref this.lastAngle, "lastAngle", 0f, false);
			Scribe_Values.Look<Vector3>(ref this.currentOffset, "currentOffset", default(Vector3), false);
			Scribe_Values.Look<float>(ref this.initialAngle, "initialAngle", 0f, false);
		}

		// Token: 0x0400629D RID: 25245
		private static int DirectionChangeBlendDuration = 100;

		// Token: 0x0400629E RID: 25246
		private int directionChangeInterval;

		// Token: 0x0400629F RID: 25247
		private int lastDirectionChange;

		// Token: 0x040062A0 RID: 25248
		private float initialAngle;

		// Token: 0x040062A1 RID: 25249
		private float currentAngle;

		// Token: 0x040062A2 RID: 25250
		private float lastAngle;

		// Token: 0x040062A3 RID: 25251
		private Vector3 currentOffset;
	}
}
