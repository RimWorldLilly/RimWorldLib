using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A77 RID: 6775
	public class Building_ShipReactor : Building
	{
		// Token: 0x0600AA76 RID: 43638 RVA: 0x003C5AD0 File Offset: 0x003C3CD0
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (this.charlonsReactor)
			{
				QuestUtility.SendQuestTargetSignals(base.Map.Parent.questTags, "ReactorDestroyed");
			}
			base.Destroy(mode);
		}

		// Token: 0x0600AA77 RID: 43639 RVA: 0x003C5AFB File Offset: 0x003C3CFB
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Building_ShipReactor.<GetGizmos>d__2 <GetGizmos>d__ = new Building_ShipReactor.<GetGizmos>d__2(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600AA78 RID: 43640 RVA: 0x003C5B0B File Offset: 0x003C3D0B
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.charlonsReactor, "charlonsReactor", false, false);
		}

		// Token: 0x0400639B RID: 25499
		public bool charlonsReactor;
	}
}
