using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x0200108F RID: 4239
	public class QuestPart_WorldObjectTimeout : QuestPart_Delay
	{
		// Token: 0x17001236 RID: 4662
		// (get) Token: 0x06006A37 RID: 27191 RVA: 0x00255F6E File Offset: 0x0025416E
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_WorldObjectTimeout.<get_QuestLookTargets>d__3 <get_QuestLookTargets>d__ = new QuestPart_WorldObjectTimeout.<get_QuestLookTargets>d__3(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x06006A38 RID: 27192 RVA: 0x00255F80 File Offset: 0x00254180
		public override string ExtraInspectString(ISelectable target)
		{
			if (target == this.worldObject)
			{
				Site site = target as Site;
				if (site != null)
				{
					for (int i = 0; i < site.parts.Count; i++)
					{
						if (site.parts[i].def.handlesWorldObjectTimeoutInspectString)
						{
							return null;
						}
					}
				}
				return "WorldObjectTimeout".Translate(base.TicksLeft.ToStringTicksToPeriod(true, false, true, true, false));
			}
			return null;
		}

		// Token: 0x06006A39 RID: 27193 RVA: 0x00255FF6 File Offset: 0x002541F6
		protected override void DelayFinished()
		{
			QuestPart_DestroyWorldObject.TryRemove(this.worldObject);
			if (this.worldObject != null)
			{
				base.Complete(this.worldObject.Named("SUBJECT"));
				return;
			}
			base.Complete();
		}

		// Token: 0x06006A3A RID: 27194 RVA: 0x00256028 File Offset: 0x00254228
		public override void Cleanup()
		{
			base.Cleanup();
			if (this.destroyOnCleanup)
			{
				QuestPart_DestroyWorldObject.TryRemove(this.worldObject);
			}
		}

		// Token: 0x06006A3B RID: 27195 RVA: 0x00256043 File Offset: 0x00254243
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<WorldObject>(ref this.worldObject, "worldObject", false);
			Scribe_Values.Look<bool>(ref this.destroyOnCleanup, "destroyOnCleanup", false, false);
		}

		// Token: 0x06006A3C RID: 27196 RVA: 0x00256070 File Offset: 0x00254270
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			PlanetTile tile;
			if (TileFinder.TryFindNewSiteTile(out tile, 7, 27, false, null, 0.5f, true, TileFinderMode.Near, false, false, null, null))
			{
				this.worldObject = SiteMaker.MakeSite(null, tile, null, true, null, null);
			}
		}

		// Token: 0x0400437A RID: 17274
		public WorldObject worldObject;

		// Token: 0x0400437B RID: 17275
		public bool destroyOnCleanup;
	}
}
