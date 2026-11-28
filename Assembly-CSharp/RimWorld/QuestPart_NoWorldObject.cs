using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001088 RID: 4232
	public class QuestPart_NoWorldObject : QuestPartActivable
	{
		// Token: 0x17001229 RID: 4649
		// (get) Token: 0x06006A09 RID: 27145 RVA: 0x002556D2 File Offset: 0x002538D2
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				QuestPart_NoWorldObject.<get_QuestLookTargets>d__2 <get_QuestLookTargets>d__ = new QuestPart_NoWorldObject.<get_QuestLookTargets>d__2(-2);
				<get_QuestLookTargets>d__.<>4__this = this;
				return <get_QuestLookTargets>d__;
			}
		}

		// Token: 0x06006A0A RID: 27146 RVA: 0x002556E2 File Offset: 0x002538E2
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (this.worldObject == null || !this.worldObject.Spawned)
			{
				base.Complete();
			}
		}

		// Token: 0x06006A0B RID: 27147 RVA: 0x00255705 File Offset: 0x00253905
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<WorldObject>(ref this.worldObject, "worldObject", false);
		}

		// Token: 0x06006A0C RID: 27148 RVA: 0x00255720 File Offset: 0x00253920
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			Site site = Find.WorldObjects.Sites.FirstOrDefault<Site>();
			if (site != null)
			{
				this.worldObject = site;
				return;
			}
			Map randomPlayerHomeMap = Find.RandomPlayerHomeMap;
			if (randomPlayerHomeMap != null)
			{
				this.worldObject = randomPlayerHomeMap.Parent;
			}
		}

		// Token: 0x04004367 RID: 17255
		public WorldObject worldObject;
	}
}
