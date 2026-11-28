using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200188D RID: 6285
	public class CompFleshmassHeartChild : ThingComp
	{
		// Token: 0x17001AED RID: 6893
		// (get) Token: 0x06009D57 RID: 40279 RVA: 0x003895D8 File Offset: 0x003877D8
		public CompFleshmassHeart HeartComp
		{
			get
			{
				Building_FleshmassHeart building_FleshmassHeart = this.heart;
				if (building_FleshmassHeart == null)
				{
					return null;
				}
				return building_FleshmassHeart.GetComp<CompFleshmassHeart>();
			}
		}

		// Token: 0x06009D58 RID: 40280 RVA: 0x003895EB File Offset: 0x003877EB
		public override void PostExposeData()
		{
			Scribe_References.Look<Building_FleshmassHeart>(ref this.heart, "heart", false);
		}

		// Token: 0x06009D59 RID: 40281 RVA: 0x003895FE File Offset: 0x003877FE
		public override void CompTick()
		{
			if (this.destroyTick > 0 && Find.TickManager.TicksGame >= this.destroyTick)
			{
				this.parent.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06009D5A RID: 40282 RVA: 0x00389627 File Offset: 0x00387827
		public override void PostSwapMap()
		{
			if (this.heart.Map != this.parent.Map)
			{
				CompFleshmassHeart heartComp = this.HeartComp;
				if (heartComp != null)
				{
					heartComp.Notify_ChildDied(this.parent);
				}
				this.Notify_HeartDied();
			}
		}

		// Token: 0x06009D5B RID: 40283 RVA: 0x0038965E File Offset: 0x0038785E
		public override void Notify_Killed(Map prevMap, DamageInfo? dinfo = null)
		{
			CompFleshmassHeart heartComp = this.HeartComp;
			if (heartComp == null)
			{
				return;
			}
			heartComp.Notify_ChildDied(this.parent);
		}

		// Token: 0x06009D5C RID: 40284 RVA: 0x00389678 File Offset: 0x00387878
		public void Notify_HeartDied()
		{
			this.destroyTick = Find.TickManager.TicksGame + CompFleshmassHeartChild.DestroyDelayRangeTicks.RandomInRange;
		}

		// Token: 0x04005D3E RID: 23870
		private static readonly IntRange DestroyDelayRangeTicks = new IntRange(180, 480);

		// Token: 0x04005D3F RID: 23871
		public Building_FleshmassHeart heart;

		// Token: 0x04005D40 RID: 23872
		private int destroyTick = -99999;
	}
}
