using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200183E RID: 6206
	public class CompBreakdownable : ThingComp
	{
		// Token: 0x17001A84 RID: 6788
		// (get) Token: 0x06009B62 RID: 39778 RVA: 0x00380552 File Offset: 0x0037E752
		public bool BrokenDown
		{
			get
			{
				return this.brokenDownInt;
			}
		}

		// Token: 0x06009B63 RID: 39779 RVA: 0x0038055A File Offset: 0x0037E75A
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.brokenDownInt, "brokenDown", false, false);
		}

		// Token: 0x06009B64 RID: 39780 RVA: 0x00380574 File Offset: 0x0037E774
		private void UpdateOverlays()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			this.parent.Map.overlayDrawer.Disable(this.parent, ref this.overlayBrokenDown);
			if (this.brokenDownInt)
			{
				this.overlayBrokenDown = new OverlayHandle?(this.parent.Map.overlayDrawer.Enable(this.parent, OverlayTypes.BrokenDown));
			}
		}

		// Token: 0x06009B65 RID: 39781 RVA: 0x003805E0 File Offset: 0x0037E7E0
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.powerComp = this.parent.GetComp<CompPowerTrader>();
			this.parent.Map.GetComponent<BreakdownManager>().Register(this);
			this.UpdateOverlays();
		}

		// Token: 0x06009B66 RID: 39782 RVA: 0x00380616 File Offset: 0x0037E816
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			base.PostDeSpawn(map, mode);
			map.GetComponent<BreakdownManager>().Deregister(this);
		}

		// Token: 0x06009B67 RID: 39783 RVA: 0x0038062C File Offset: 0x0037E82C
		public void CheckForBreakdown()
		{
			if (this.CanBreakdownNow() && Rand.MTBEventOccurs(13680000f, 1f, 1041f))
			{
				this.DoBreakdown();
			}
		}

		// Token: 0x06009B68 RID: 39784 RVA: 0x00380652 File Offset: 0x0037E852
		protected bool CanBreakdownNow()
		{
			return !this.BrokenDown && (this.powerComp == null || this.powerComp.PowerOn);
		}

		// Token: 0x06009B69 RID: 39785 RVA: 0x00380674 File Offset: 0x0037E874
		public void Notify_Repaired()
		{
			this.brokenDownInt = false;
			this.parent.Map.GetComponent<BreakdownManager>().Notify_Repaired(this.parent);
			if (this.parent is Building_PowerSwitch)
			{
				this.parent.Map.powerNetManager.Notfiy_TransmitterTransmitsPowerNowChanged(this.parent.GetComp<CompPower>());
			}
			this.UpdateOverlays();
		}

		// Token: 0x06009B6A RID: 39786 RVA: 0x003806D6 File Offset: 0x0037E8D6
		public void DoBreakdown()
		{
			this.brokenDownInt = true;
			this.parent.BroadcastCompSignal("Breakdown");
			this.parent.Map.GetComponent<BreakdownManager>().Notify_BrokenDown(this.parent);
			this.UpdateOverlays();
		}

		// Token: 0x06009B6B RID: 39787 RVA: 0x00380710 File Offset: 0x0037E910
		public override string CompInspectStringExtra()
		{
			if (this.BrokenDown)
			{
				return "BrokenDown".Translate();
			}
			return null;
		}

		// Token: 0x06009B6C RID: 39788 RVA: 0x0038072B File Offset: 0x0037E92B
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompBreakdownable.<CompGetGizmosExtra>d__16 <CompGetGizmosExtra>d__ = new CompBreakdownable.<CompGetGizmosExtra>d__16(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x04005C59 RID: 23641
		private bool brokenDownInt;

		// Token: 0x04005C5A RID: 23642
		private CompPowerTrader powerComp;

		// Token: 0x04005C5B RID: 23643
		private const int BreakdownMTBTicks = 13680000;

		// Token: 0x04005C5C RID: 23644
		public const string BreakdownSignal = "Breakdown";

		// Token: 0x04005C5D RID: 23645
		private OverlayHandle? overlayBrokenDown;
	}
}
