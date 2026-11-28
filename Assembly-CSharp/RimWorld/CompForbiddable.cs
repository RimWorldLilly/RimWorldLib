using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001897 RID: 6295
	public class CompForbiddable : ThingComp
	{
		// Token: 0x17001B00 RID: 6912
		// (get) Token: 0x06009DAD RID: 40365 RVA: 0x0038A821 File Offset: 0x00388A21
		public CompProperties_Forbiddable Props
		{
			get
			{
				return (CompProperties_Forbiddable)this.props;
			}
		}

		// Token: 0x17001B01 RID: 6913
		// (get) Token: 0x06009DAE RID: 40366 RVA: 0x0038A82E File Offset: 0x00388A2E
		// (set) Token: 0x06009DAF RID: 40367 RVA: 0x0038A838 File Offset: 0x00388A38
		public bool Forbidden
		{
			get
			{
				return this.forbiddenInt;
			}
			set
			{
				if (value == this.forbiddenInt)
				{
					return;
				}
				this.forbiddenInt = value;
				if (this.parent.Spawned)
				{
					if (this.forbiddenInt)
					{
						this.parent.Map.listerHaulables.Notify_Forbidden(this.parent);
						this.parent.Map.listerMergeables.Notify_Forbidden(this.parent);
					}
					else
					{
						this.parent.Map.listerHaulables.Notify_Unforbidden(this.parent);
						this.parent.Map.listerMergeables.Notify_Unforbidden(this.parent);
					}
					if (this.parent is Building_Door)
					{
						this.parent.Map.reachability.ClearCache();
					}
				}
				this.UpdateOverlayHandle();
			}
		}

		// Token: 0x17001B02 RID: 6914
		// (get) Token: 0x06009DB0 RID: 40368 RVA: 0x0038A904 File Offset: 0x00388B04
		private OverlayTypes MyOverlayType
		{
			get
			{
				if (this.parent is Blueprint || this.parent is Frame)
				{
					if (this.parent.def.size.x > 1 || this.parent.def.size.z > 1)
					{
						return OverlayTypes.ForbiddenBig;
					}
					return OverlayTypes.Forbidden;
				}
				else
				{
					if (this.parent.def.category == ThingCategory.Building)
					{
						return OverlayTypes.ForbiddenBig;
					}
					return OverlayTypes.Forbidden;
				}
			}
		}

		// Token: 0x06009DB1 RID: 40369 RVA: 0x0038A978 File Offset: 0x00388B78
		private void UpdateOverlayHandle()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			this.parent.Map.overlayDrawer.Disable(this.parent, ref this.overlayHandle);
			if (this.parent.Spawned && this.forbiddenInt)
			{
				this.overlayHandle = new OverlayHandle?(this.parent.Map.overlayDrawer.Enable(this.parent, this.MyOverlayType));
			}
		}

		// Token: 0x06009DB2 RID: 40370 RVA: 0x0038A9F5 File Offset: 0x00388BF5
		public override void PostPostMake()
		{
			base.PostPostMake();
			if (this.Props.forbidOnMake)
			{
				this.forbiddenInt = true;
				this.parent.SetForbidden(true, true);
			}
		}

		// Token: 0x06009DB3 RID: 40371 RVA: 0x0038AA1E File Offset: 0x00388C1E
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.UpdateOverlayHandle();
		}

		// Token: 0x06009DB4 RID: 40372 RVA: 0x0038AA26 File Offset: 0x00388C26
		public override void PostExposeData()
		{
			Scribe_Values.Look<bool>(ref this.forbiddenInt, "forbidden", false, false);
		}

		// Token: 0x06009DB5 RID: 40373 RVA: 0x0038AA3A File Offset: 0x00388C3A
		public override void PostSplitOff(Thing piece)
		{
			piece.SetForbidden(this.forbiddenInt, true);
		}

		// Token: 0x06009DB6 RID: 40374 RVA: 0x0038AA49 File Offset: 0x00388C49
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompForbiddable.<CompGetGizmosExtra>d__14 <CompGetGizmosExtra>d__ = new CompForbiddable.<CompGetGizmosExtra>d__14(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x04005D62 RID: 23906
		private bool forbiddenInt;

		// Token: 0x04005D63 RID: 23907
		private OverlayHandle? overlayHandle;
	}
}
