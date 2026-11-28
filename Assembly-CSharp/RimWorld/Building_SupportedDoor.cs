using System;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001ACE RID: 6862
	public class Building_SupportedDoor : Building_Door
	{
		// Token: 0x17001F5C RID: 8028
		// (get) Token: 0x0600AEAE RID: 44718 RVA: 0x003D8484 File Offset: 0x003D6684
		private Graphic SupportGraphic
		{
			get
			{
				if (this.suppportGraphic == null)
				{
					GraphicData doorSupportGraphic = this.def.building.doorSupportGraphic;
					Graphic graphic = (doorSupportGraphic != null) ? doorSupportGraphic.Graphic : null;
					if (graphic == null)
					{
						return null;
					}
					this.suppportGraphic = graphic.GetColoredVersion(graphic.Shader, this.DrawColor, this.Graphic.ColorTwo);
				}
				return this.suppportGraphic;
			}
		}

		// Token: 0x17001F5D RID: 8029
		// (get) Token: 0x0600AEAF RID: 44719 RVA: 0x003D84E4 File Offset: 0x003D66E4
		private Graphic TopGraphic
		{
			get
			{
				if (this.topGraphic == null)
				{
					GraphicData doorTopGraphic = this.def.building.doorTopGraphic;
					Graphic graphic = (doorTopGraphic != null) ? doorTopGraphic.Graphic : null;
					if (graphic == null)
					{
						return null;
					}
					this.topGraphic = graphic.GetColoredVersion(graphic.Shader, this.DrawColor, this.Graphic.ColorTwo);
				}
				return this.topGraphic;
			}
		}

		// Token: 0x0600AEB0 RID: 44720 RVA: 0x003D8544 File Offset: 0x003D6744
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			base.DoorPreDraw();
			Vector3 a = this.DrawPos;
			bool flag = base.Rotation == Rot4.North || base.Rotation == Rot4.South;
			a += (flag ? this.def.building.doorTopHorizontalOffset : this.def.building.doorTopVerticalOffset);
			a.y = (flag ? AltitudeLayer.BuildingOnTop.AltitudeFor() : AltitudeLayer.Blueprint.AltitudeFor());
			Graphic supportGraphic = this.SupportGraphic;
			if (supportGraphic != null)
			{
				supportGraphic.Draw(a + this.def.building.doorSupportGraphicOffset, base.Rotation, this, 0f);
			}
			a.y = AltitudeLayer.Blueprint.AltitudeFor();
			Graphic graphic = this.TopGraphic;
			if (graphic != null)
			{
				graphic.Draw(a + this.def.building.doorTopGraphicOffset, base.Rotation, this, 0f);
			}
			base.DrawAt(drawLoc, flip);
		}

		// Token: 0x0600AEB1 RID: 44721 RVA: 0x003D8648 File Offset: 0x003D6848
		protected override void Tick()
		{
			base.Tick();
			if (!this.openInt && this.OpenPct <= 0f && this.openedBefore)
			{
				this.openedBefore = false;
				if (!this.def.building.soundDoorCloseEnd.NullOrUndefined())
				{
					this.def.building.soundDoorCloseEnd.PlayOneShot(this);
				}
			}
		}

		// Token: 0x0600AEB2 RID: 44722 RVA: 0x003D86B1 File Offset: 0x003D68B1
		protected override void DoorOpen(int ticksToClose = 110)
		{
			base.DoorOpen(ticksToClose);
			this.openedBefore = true;
		}

		// Token: 0x0600AEB3 RID: 44723 RVA: 0x003D86C1 File Offset: 0x003D68C1
		public override void Notify_ColorChanged()
		{
			this.suppportGraphic = null;
			this.topGraphic = null;
			base.Notify_ColorChanged();
		}

		// Token: 0x0600AEB4 RID: 44724 RVA: 0x003D86D7 File Offset: 0x003D68D7
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.openedBefore, "openedBefore", false, false);
		}

		// Token: 0x04006507 RID: 25863
		private bool openedBefore;

		// Token: 0x04006508 RID: 25864
		[Unsaved(false)]
		private Graphic suppportGraphic;

		// Token: 0x04006509 RID: 25865
		[Unsaved(false)]
		private Graphic topGraphic;
	}
}
