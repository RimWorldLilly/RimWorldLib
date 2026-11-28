using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020010B7 RID: 4279
	public class QuestPart_DestroyWorldObject : QuestPart
	{
		// Token: 0x17001255 RID: 4693
		// (get) Token: 0x06006B0D RID: 27405 RVA: 0x002594A8 File Offset: 0x002576A8
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_DestroyWorldObject.<get_QuestLookTargets>d__3 <get_QuestLookTargets>d__ = new QuestPart_DestroyWorldObject.<get_QuestLookTargets>d__3(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x06006B0E RID: 27406 RVA: 0x002594B8 File Offset: 0x002576B8
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal)
			{
				QuestPart_DestroyWorldObject.TryRemove(this.worldObject);
			}
		}

		// Token: 0x06006B0F RID: 27407 RVA: 0x002594DF File Offset: 0x002576DF
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<WorldObject>(ref this.worldObject, "worldObject", false);
		}

		// Token: 0x06006B10 RID: 27408 RVA: 0x0025950C File Offset: 0x0025770C
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int.ToString();
			PlanetTile tile;
			if (TileFinder.TryFindNewSiteTile(out tile, 7, 27, false, null, 0.5f, true, TileFinderMode.Near, false, false, null, null))
			{
				this.worldObject = SiteMaker.MakeSite(null, tile, null, true, null, null);
			}
		}

		// Token: 0x06006B11 RID: 27409 RVA: 0x00259570 File Offset: 0x00257770
		public static void TryRemove(WorldObject worldObject)
		{
			if (worldObject != null && worldObject.Spawned)
			{
				MapParent mapParent = worldObject as MapParent;
				if (mapParent != null && mapParent.HasMap)
				{
					mapParent.forceRemoveWorldObjectWhenMapRemoved = true;
					return;
				}
				worldObject.Destroy();
			}
		}

		// Token: 0x0400440B RID: 17419
		public string inSignal;

		// Token: 0x0400440C RID: 17420
		public WorldObject worldObject;
	}
}
