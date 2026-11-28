using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001820 RID: 6176
	public class CompAtomizer : CompThingContainer, INotifyHauledTo
	{
		// Token: 0x17001A4F RID: 6735
		// (get) Token: 0x06009A7E RID: 39550 RVA: 0x0037BCBA File Offset: 0x00379EBA
		public new CompProperties_Atomizer Props
		{
			get
			{
				return (CompProperties_Atomizer)this.props;
			}
		}

		// Token: 0x17001A50 RID: 6736
		// (get) Token: 0x06009A7F RID: 39551 RVA: 0x0037BCC7 File Offset: 0x00379EC7
		public bool AutoLoad
		{
			get
			{
				return this.autoLoad;
			}
		}

		// Token: 0x17001A51 RID: 6737
		// (get) Token: 0x06009A80 RID: 39552 RVA: 0x0037BCCF File Offset: 0x00379ECF
		public int SpaceLeft
		{
			get
			{
				return this.Props.stackLimit - base.TotalStackCount;
			}
		}

		// Token: 0x17001A52 RID: 6738
		// (get) Token: 0x06009A81 RID: 39553 RVA: 0x0037BCE3 File Offset: 0x00379EE3
		public int TicksPerAtomize
		{
			get
			{
				return this.Props.ticksPerAtomize;
			}
		}

		// Token: 0x17001A53 RID: 6739
		// (get) Token: 0x06009A82 RID: 39554 RVA: 0x0037BCF0 File Offset: 0x00379EF0
		public int TicksLeftUntilAllAtomized
		{
			get
			{
				return base.TotalStackCount * this.TicksPerAtomize - this.ticksAtomized;
			}
		}

		// Token: 0x17001A54 RID: 6740
		// (get) Token: 0x06009A83 RID: 39555 RVA: 0x0037BD06 File Offset: 0x00379F06
		public float FillPercent
		{
			get
			{
				return (float)base.TotalStackCount / (float)this.Props.stackLimit;
			}
		}

		// Token: 0x17001A55 RID: 6741
		// (get) Token: 0x06009A84 RID: 39556 RVA: 0x0037BD1C File Offset: 0x00379F1C
		private Graphic ContentsGraphic
		{
			get
			{
				if (this.contentsGraphic == null)
				{
					ThingDef thingDef = this.Props.thingDef;
					this.contentsGraphic = thingDef.graphic.GetColoredVersion(thingDef.graphic.Shader, thingDef.graphic.color, thingDef.graphic.colorTwo);
				}
				return this.contentsGraphic;
			}
		}

		// Token: 0x06009A85 RID: 39557 RVA: 0x0037BD75 File Offset: 0x00379F75
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			if (!ModLister.CheckBiotech("Atomizer"))
			{
				this.parent.Destroy(DestroyMode.Vanish);
				return;
			}
			base.PostSpawnSetup(respawningAfterLoad);
			this.powerComp = this.parent.TryGetComp<CompPowerTrader>();
		}

		// Token: 0x06009A86 RID: 39558 RVA: 0x0037BDA8 File Offset: 0x00379FA8
		public override bool Accepts(Thing thing)
		{
			return base.Accepts(thing) && thing.def == this.Props.thingDef;
		}

		// Token: 0x06009A87 RID: 39559 RVA: 0x0037BDC8 File Offset: 0x00379FC8
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompAtomizer.<CompGetGizmosExtra>d__22 <CompGetGizmosExtra>d__ = new CompAtomizer.<CompGetGizmosExtra>d__22(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x06009A88 RID: 39560 RVA: 0x0037BDD8 File Offset: 0x00379FD8
		private void DoAtomize()
		{
			Thing containedThing = base.ContainedThing;
			containedThing.stackCount--;
			if (this.Props.resolveEffecter != null)
			{
				this.parent.MapHeld.effecterMaintainer.AddEffecterToMaintain(this.Props.resolveEffecter.Spawn(this.parent, this.parent.MapHeld, 1f), this.parent, this.Props.resolveEffecterTicks);
			}
			if (containedThing.stackCount <= 0)
			{
				this.innerContainer.Remove(containedThing);
				containedThing.Destroy(DestroyMode.Vanish);
			}
			this.ticksAtomized = 0;
		}

		// Token: 0x06009A89 RID: 39561 RVA: 0x0037BE77 File Offset: 0x0037A077
		private void EjectContents()
		{
			this.innerContainer.TryDropAll(this.parent.InteractionCell, this.parent.Map, ThingPlaceMode.Near, null, null, true);
			this.ticksAtomized = 0;
		}

		// Token: 0x06009A8A RID: 39562 RVA: 0x0037BEA6 File Offset: 0x0037A0A6
		public void Notify_HauledTo(Pawn hauler, Thing thing, int count)
		{
			this.Props.materialsAddedSound.PlayOneShot(this.parent);
		}

		// Token: 0x06009A8B RID: 39563 RVA: 0x0037BEC4 File Offset: 0x0037A0C4
		public override void PostDraw()
		{
			base.PostDraw();
			if (!this.autoLoad)
			{
				this.parent.Map.overlayDrawer.DrawOverlay(this.parent, OverlayTypes.ForbiddenAtomizer);
			}
			if (!base.Empty)
			{
				Vector3 loc = this.parent.DrawPos + this.Props.contentsDrawOffset;
				loc.y -= 0.03658537f;
				Graphic_StackCount graphic_StackCount = this.ContentsGraphic as Graphic_StackCount;
				if (graphic_StackCount != null)
				{
					graphic_StackCount.SubGraphicForStackCount(Mathf.Min(base.TotalStackCount, this.Props.thingDef.stackLimit), this.Props.thingDef).Draw(loc, Rot4.North, this.parent, 0f);
					return;
				}
				this.ContentsGraphic.Draw(loc, Rot4.North, this.parent, 0f);
			}
		}

		// Token: 0x06009A8C RID: 39564 RVA: 0x0037BFA4 File Offset: 0x0037A1A4
		public override void CompTick()
		{
			base.CompTick();
			if (!base.Empty && this.powerComp.PowerOn)
			{
				this.ticksAtomized++;
				if (this.ticksAtomized >= this.TicksPerAtomize)
				{
					this.DoAtomize();
				}
				if (this.workingEffecter == null)
				{
					this.workingEffecter = this.Props.workingEffecter.Spawn(this.parent, this.parent.Map, 1f);
				}
				this.workingEffecter.EffectTick(this.parent, this.parent);
				return;
			}
			Effecter effecter = this.workingEffecter;
			if (effecter != null)
			{
				effecter.Cleanup();
			}
			this.workingEffecter = null;
		}

		// Token: 0x06009A8D RID: 39565 RVA: 0x0037C05F File Offset: 0x0037A25F
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.autoLoad, "autoLoad", true, false);
			Scribe_Values.Look<int>(ref this.ticksAtomized, "ticksAtomized", 0, false);
		}

		// Token: 0x06009A8E RID: 39566 RVA: 0x0037C08C File Offset: 0x0037A28C
		public override string CompInspectStringExtra()
		{
			TaggedString t = "";
			if (!base.Empty)
			{
				t += "ContainedThings".Translate(this.Props.thingDef) + ": " + base.TotalStackCount.ToString() + " / " + this.Props.stackLimit.ToString();
				t += "\n" + "FinishesIn".Translate() + ": " + "PeriodDays".Translate(((float)this.TicksLeftUntilAllAtomized / 60000f).ToString("F1"));
				t += "\n" + "DurationUntilNextAtomization".Translate() + ": " + (this.TicksPerAtomize - this.ticksAtomized).ToStringTicksToPeriod(true, false, true, true, false);
				if (!this.powerComp.PowerOn)
				{
					t += " (" + "Paused".Translate() + ")";
				}
			}
			return t.Resolve();
		}

		// Token: 0x04005BD4 RID: 23508
		private CachedTexture EjectContentsIcon = new CachedTexture("UI/Icons/EjectContentFromAtomizer");

		// Token: 0x04005BD5 RID: 23509
		private bool autoLoad = true;

		// Token: 0x04005BD6 RID: 23510
		private int ticksAtomized;

		// Token: 0x04005BD7 RID: 23511
		private CompPowerTrader powerComp;

		// Token: 0x04005BD8 RID: 23512
		private Effecter workingEffecter;

		// Token: 0x04005BD9 RID: 23513
		private Graphic contentsGraphic;
	}
}
