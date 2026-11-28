using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001ABB RID: 6843
	public class Building_JammedDoor : Building_Door
	{
		// Token: 0x17001F38 RID: 7992
		// (get) Token: 0x0600ADFB RID: 44539 RVA: 0x003D547B File Offset: 0x003D367B
		public bool Jammed
		{
			get
			{
				return this.jammed;
			}
		}

		// Token: 0x0600ADFC RID: 44540 RVA: 0x003D5483 File Offset: 0x003D3683
		public void UnlockDoor()
		{
			this.jammed = false;
		}

		// Token: 0x0600ADFD RID: 44541 RVA: 0x003D548C File Offset: 0x003D368C
		public void UnlockAndOpenDoor()
		{
			this.jammed = false;
			this.DoorOpen(110);
		}

		// Token: 0x0600ADFE RID: 44542 RVA: 0x003D549D File Offset: 0x003D369D
		protected override void DoorOpen(int ticksToClose = 110)
		{
			base.DoorOpen(ticksToClose);
			this.holdOpenInt = true;
		}

		// Token: 0x0600ADFF RID: 44543 RVA: 0x003D54AD File Offset: 0x003D36AD
		public override bool PawnCanOpen(Pawn p)
		{
			return !this.jammed;
		}

		// Token: 0x0600AE00 RID: 44544 RVA: 0x003D54B8 File Offset: 0x003D36B8
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			if (!this.jammed)
			{
				base.DrawAt(drawLoc, flip);
				return;
			}
			base.DoorPreDraw();
			foreach (ThingComp thingComp in base.AllComps)
			{
				IJammedDoorDrawer jammedDoorDrawer = thingComp as IJammedDoorDrawer;
				if (jammedDoorDrawer != null)
				{
					jammedDoorDrawer.DrawJammed(base.Rotation);
					break;
				}
			}
		}

		// Token: 0x0600AE01 RID: 44545 RVA: 0x003D5534 File Offset: 0x003D3734
		public override IEnumerable<Gizmo> GetGizmos()
		{
			Building_JammedDoor.<GetGizmos>d__8 <GetGizmos>d__ = new Building_JammedDoor.<GetGizmos>d__8(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600AE02 RID: 44546 RVA: 0x003D5544 File Offset: 0x003D3744
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.jammed, "jammed", false, false);
		}

		// Token: 0x040064C3 RID: 25795
		private bool jammed = true;
	}
}
