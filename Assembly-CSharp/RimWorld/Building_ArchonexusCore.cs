using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A99 RID: 6809
	public class Building_ArchonexusCore : Building
	{
		// Token: 0x17001E8B RID: 7819
		// (get) Token: 0x0600ABB9 RID: 43961 RVA: 0x003CA148 File Offset: 0x003C8348
		public bool CanActivateNow
		{
			get
			{
				return !ArchonexusCountdown.CountdownActivated;
			}
		}

		// Token: 0x0600ABBA RID: 43962 RVA: 0x003CA152 File Offset: 0x003C8352
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Building_ArchonexusCore.<GetGizmos>d__3 <GetGizmos>d__ = new Building_ArchonexusCore.<GetGizmos>d__3(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600ABBB RID: 43963 RVA: 0x003CA162 File Offset: 0x003C8362
		public void Activate()
		{
			if (!this.CanActivateNow)
			{
				return;
			}
			ArchonexusCountdown.InitiateCountdown(this);
		}

		// Token: 0x0600ABBC RID: 43964 RVA: 0x003CA173 File Offset: 0x003C8373
		public override IEnumerable<FloatMenuOption> GetMultiSelectFloatMenuOptions(IEnumerable<Pawn> selPawns)
		{
			Building_ArchonexusCore.<GetMultiSelectFloatMenuOptions>d__7 <GetMultiSelectFloatMenuOptions>d__ = new Building_ArchonexusCore.<GetMultiSelectFloatMenuOptions>d__7(-2);
			<GetMultiSelectFloatMenuOptions>d__.<>4__this = this;
			<GetMultiSelectFloatMenuOptions>d__.<>3__selPawns = selPawns;
			return <GetMultiSelectFloatMenuOptions>d__;
		}

		// Token: 0x040063F1 RID: 25585
		private const int ObservationDistance = 8;

		// Token: 0x040063F2 RID: 25586
		private List<Pawn> tmpPawnsCanReach = new List<Pawn>();

		// Token: 0x040063F3 RID: 25587
		private List<IntVec3> tmpObservationSpots = new List<IntVec3>();
	}
}
