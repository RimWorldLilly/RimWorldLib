using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A76 RID: 6774
	public class Building_ShipComputerCore : Building
	{
		// Token: 0x17001E0F RID: 7695
		// (get) Token: 0x0600AA70 RID: 43632 RVA: 0x003C5A66 File Offset: 0x003C3C66
		private bool CanLaunchNow
		{
			get
			{
				return !ShipUtility.LaunchFailReasons(this).Any<string>();
			}
		}

		// Token: 0x0600AA71 RID: 43633 RVA: 0x003C5A76 File Offset: 0x003C3C76
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Building_ShipComputerCore.<GetGizmos>d__2 <GetGizmos>d__ = new Building_ShipComputerCore.<GetGizmos>d__2(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600AA72 RID: 43634 RVA: 0x003C5A86 File Offset: 0x003C3C86
		public void ForceLaunch()
		{
			ShipCountdown.InitiateCountdown(this);
			if (base.Spawned)
			{
				QuestUtility.SendQuestTargetSignals(base.Map.Parent.questTags, "LaunchedShip");
			}
		}

		// Token: 0x0600AA73 RID: 43635 RVA: 0x003C5AB0 File Offset: 0x003C3CB0
		private void TryLaunch()
		{
			if (this.CanLaunchNow)
			{
				this.ForceLaunch();
			}
		}
	}
}
