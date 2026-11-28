using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A36 RID: 6710
	public class CompUseEffect_DestroySelf : CompUseEffect
	{
		// Token: 0x17001D88 RID: 7560
		// (get) Token: 0x0600A85A RID: 43098 RVA: 0x003BD230 File Offset: 0x003BB430
		private CompProperties_UseEffectDestroySelf Props
		{
			get
			{
				return (CompProperties_UseEffectDestroySelf)this.props;
			}
		}

		// Token: 0x17001D89 RID: 7561
		// (get) Token: 0x0600A85B RID: 43099 RVA: 0x003BD23D File Offset: 0x003BB43D
		public override float OrderPriority
		{
			get
			{
				return this.Props.orderPriority;
			}
		}

		// Token: 0x0600A85C RID: 43100 RVA: 0x003BD24A File Offset: 0x003BB44A
		public override void DoEffect(Pawn usedBy)
		{
			base.DoEffect(usedBy);
			if (this.Props.delayTicks <= 0)
			{
				this.DoDestroy();
				return;
			}
			this.delayTicks = this.Props.delayTicks;
		}

		// Token: 0x0600A85D RID: 43101 RVA: 0x003BD27C File Offset: 0x003BB47C
		private void DoDestroy()
		{
			if (this.Props.effecterDef != null)
			{
				Effecter effecter = new Effecter(this.Props.effecterDef);
				effecter.Trigger(new TargetInfo(this.parent.Position, this.parent.Map, false), TargetInfo.Invalid, -1);
				effecter.Cleanup();
			}
			if (this.Props.spawnKilledLeavings)
			{
				GenLeaving.DoLeavingsFor(this.parent, this.parent.MapHeld, DestroyMode.KillFinalizeLeavingsOnly, null);
			}
			if (!this.Props.leavings.NullOrEmpty<ThingDefCountClass>())
			{
				Rand.PushState(this.parent.thingIDNumber);
				for (int i = 0; i < this.Props.leavings.Count; i++)
				{
					ThingDefCountClass thingDefCountClass = this.Props.leavings[i];
					if (!thingDefCountClass.IsChanceBased || Rand.Chance(thingDefCountClass.DropChance))
					{
						Thing thing = ThingMaker.MakeThing(this.Props.leavings[i].thingDef, null);
						thing.stackCount = this.Props.leavings[i].count;
						Thing thing2;
						GenDrop.TryDropSpawn(thing, this.parent.PositionHeld, this.parent.MapHeld, ThingPlaceMode.Near, out thing2, null, null, true);
					}
				}
				Rand.PopState();
			}
			this.parent.SplitOff(1).Destroy(DestroyMode.Vanish);
		}

		// Token: 0x0600A85E RID: 43102 RVA: 0x003BD3D7 File Offset: 0x003BB5D7
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.delayTicks, "delayTicks", -1, false);
		}

		// Token: 0x0600A85F RID: 43103 RVA: 0x003BD3F1 File Offset: 0x003BB5F1
		public override void CompTick()
		{
			base.CompTick();
			if (this.delayTicks > 0)
			{
				this.delayTicks--;
			}
			if (this.delayTicks == 0)
			{
				this.DoDestroy();
				this.delayTicks = -1;
			}
		}

		// Token: 0x040062EE RID: 25326
		private int delayTicks = -1;
	}
}
