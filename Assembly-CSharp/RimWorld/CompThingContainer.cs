using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020019B9 RID: 6585
	public class CompThingContainer : ThingComp, IThingHolder, ISearchableContents
	{
		// Token: 0x17001CC6 RID: 7366
		// (get) Token: 0x0600A546 RID: 42310 RVA: 0x003AF796 File Offset: 0x003AD996
		public CompProperties_ThingContainer Props
		{
			get
			{
				return (CompProperties_ThingContainer)this.props;
			}
		}

		// Token: 0x17001CC7 RID: 7367
		// (get) Token: 0x0600A547 RID: 42311 RVA: 0x003AF7A3 File Offset: 0x003AD9A3
		public Thing ContainedThing
		{
			get
			{
				if (!this.innerContainer.Any)
				{
					return null;
				}
				return this.innerContainer[0];
			}
		}

		// Token: 0x17001CC8 RID: 7368
		// (get) Token: 0x0600A548 RID: 42312 RVA: 0x003AF7C0 File Offset: 0x003AD9C0
		public bool Empty
		{
			get
			{
				return this.ContainedThing == null;
			}
		}

		// Token: 0x17001CC9 RID: 7369
		// (get) Token: 0x0600A549 RID: 42313 RVA: 0x003AF7CB File Offset: 0x003AD9CB
		public int TotalStackCount
		{
			get
			{
				if (!this.Empty)
				{
					return this.innerContainer.TotalStackCountOfDef(this.innerContainer[0].def);
				}
				return 0;
			}
		}

		// Token: 0x17001CCA RID: 7370
		// (get) Token: 0x0600A54A RID: 42314 RVA: 0x003AF7F3 File Offset: 0x003AD9F3
		public bool Full
		{
			get
			{
				return !this.Empty && this.TotalStackCount >= this.Props.stackLimit;
			}
		}

		// Token: 0x17001CCB RID: 7371
		// (get) Token: 0x0600A54B RID: 42315 RVA: 0x003AF815 File Offset: 0x003ADA15
		public string LabelCapWithTotalCount
		{
			get
			{
				if (!this.Empty)
				{
					return this.ContainedThing.LabelCapNoCount + " x" + this.TotalStackCount.ToStringCached();
				}
				return null;
			}
		}

		// Token: 0x17001CCC RID: 7372
		// (get) Token: 0x0600A54C RID: 42316 RVA: 0x003AF841 File Offset: 0x003ADA41
		public ThingOwner SearchableContents
		{
			get
			{
				return this.innerContainer;
			}
		}

		// Token: 0x0600A54D RID: 42317 RVA: 0x003AF849 File Offset: 0x003ADA49
		public CompThingContainer()
		{
			this.innerContainer = new ThingOwner<Thing>(this);
		}

		// Token: 0x0600A54E RID: 42318 RVA: 0x003AF85D File Offset: 0x003ADA5D
		public virtual bool Accepts(ThingDef thingDef)
		{
			return this.Empty || (this.TotalStackCount < this.Props.stackLimit && thingDef == this.ContainedThing.def);
		}

		// Token: 0x0600A54F RID: 42319 RVA: 0x003AF88C File Offset: 0x003ADA8C
		public virtual bool Accepts(Thing thing)
		{
			return this.Accepts(thing.def);
		}

		// Token: 0x0600A550 RID: 42320 RVA: 0x003AF89A File Offset: 0x003ADA9A
		public override void CompTick()
		{
			this.innerContainer.DoTick();
		}

		// Token: 0x0600A551 RID: 42321 RVA: 0x003AF8A8 File Offset: 0x003ADAA8
		public override void PostDraw()
		{
			if (!this.Empty && this.Props.drawContainedThing)
			{
				this.ContainedThing.DrawNowAt((this.parent.Position + this.Props.containedThingOffset.RotatedBy(this.parent.Rotation)).ToVector3ShiftedWithAltitude(AltitudeLayer.BuildingOnTop), false);
			}
		}

		// Token: 0x0600A552 RID: 42322 RVA: 0x003AF90C File Offset: 0x003ADB0C
		public override void DrawGUIOverlay()
		{
			if (this.parent.Spawned && this.Props.drawStackLabel && !this.Empty && Find.CameraDriver.CurrentZoom == CameraZoomRange.Closest)
			{
				GenMapUI.DrawThingLabel(this.parent, this.TotalStackCount.ToStringCached());
			}
		}

		// Token: 0x0600A553 RID: 42323 RVA: 0x003AF95D File Offset: 0x003ADB5D
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode != DestroyMode.WillReplace)
			{
				this.innerContainer.TryDropAll(this.parent.Position, map, ThingPlaceMode.Near, null, null, true);
			}
		}

		// Token: 0x0600A554 RID: 42324 RVA: 0x003AF97F File Offset: 0x003ADB7F
		public override string CompInspectStringExtra()
		{
			return "Contents".Translate() + ": " + (this.Empty ? "Nothing".Translate() : this.LabelCapWithTotalCount);
		}

		// Token: 0x0600A555 RID: 42325 RVA: 0x003AF9BE File Offset: 0x003ADBBE
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			CompThingContainer.<SpecialDisplayStats>d__23 <SpecialDisplayStats>d__ = new CompThingContainer.<SpecialDisplayStats>d__23(-2);
			<SpecialDisplayStats>d__.<>4__this = this;
			return <SpecialDisplayStats>d__;
		}

		// Token: 0x0600A556 RID: 42326 RVA: 0x003AF9CE File Offset: 0x003ADBCE
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x0600A557 RID: 42327 RVA: 0x003AF9DC File Offset: 0x003ADBDC
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x0600A558 RID: 42328 RVA: 0x003AF9E4 File Offset: 0x003ADBE4
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
		}

		// Token: 0x0600A559 RID: 42329 RVA: 0x003AFA06 File Offset: 0x003ADC06
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompThingContainer.<CompGetGizmosExtra>d__27 <CompGetGizmosExtra>d__ = new CompThingContainer.<CompGetGizmosExtra>d__27(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x04006198 RID: 24984
		public ThingOwner innerContainer;
	}
}
