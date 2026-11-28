using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200058C RID: 1420
	public class MoteDualAttached : Mote
	{
		// Token: 0x060029EF RID: 10735 RVA: 0x0010051F File Offset: 0x000FE71F
		public void Attach(TargetInfo a, TargetInfo b)
		{
			this.link1 = new MoteAttachLink(a, Vector3.zero, false);
			this.link2 = new MoteAttachLink(b, Vector3.zero, false);
		}

		// Token: 0x060029F0 RID: 10736 RVA: 0x00100545 File Offset: 0x000FE745
		public void Attach(TargetInfo a, TargetInfo b, Vector3 offsetA, Vector3 offsetB)
		{
			this.link1 = new MoteAttachLink(a, offsetA, false);
			this.link2 = new MoteAttachLink(b, offsetB, false);
		}

		// Token: 0x060029F1 RID: 10737 RVA: 0x00100564 File Offset: 0x000FE764
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			this.UpdatePositionAndRotation();
			base.DrawAt(drawLoc, flip);
		}

		// Token: 0x060029F2 RID: 10738 RVA: 0x00100574 File Offset: 0x000FE774
		public void UpdateTargets(TargetInfo a, TargetInfo b, Vector3 offsetA, Vector3 offsetB)
		{
			this.link1.UpdateTarget(a, offsetA);
			this.link2.UpdateTarget(b, offsetB);
		}

		// Token: 0x060029F3 RID: 10739 RVA: 0x00100594 File Offset: 0x000FE794
		protected void UpdatePositionAndRotation()
		{
			if (this.link1.Linked)
			{
				if (this.link2.Linked)
				{
					if (!this.link1.Target.ThingDestroyed)
					{
						this.link1.UpdateDrawPos();
					}
					if (!this.link2.Target.ThingDestroyed)
					{
						this.link2.UpdateDrawPos();
					}
					this.exactPosition = (this.link1.LastDrawPos + this.link2.LastDrawPos) * 0.5f;
					if (this.def.mote.rotateTowardsTarget)
					{
						this.exactRotation = this.link1.LastDrawPos.AngleToFlat(this.link2.LastDrawPos) + 90f;
					}
					if (this.def.mote.scaleToConnectTargets)
					{
						this.linearScale = new Vector3(this.def.graphicData.drawSize.y, 1f, (this.link2.LastDrawPos - this.link1.LastDrawPos).MagnitudeHorizontal());
					}
				}
				else
				{
					if (!this.link1.Target.ThingDestroyed)
					{
						this.link1.UpdateDrawPos();
					}
					this.exactPosition = this.link1.LastDrawPos + this.def.mote.attachedDrawOffset;
				}
			}
			this.exactPosition.y = this.def.altitudeLayer.AltitudeFor();
		}

		// Token: 0x04001D10 RID: 7440
		protected MoteAttachLink link2 = MoteAttachLink.Invalid;
	}
}
