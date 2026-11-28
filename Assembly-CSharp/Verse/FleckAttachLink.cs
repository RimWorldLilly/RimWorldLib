using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000264 RID: 612
	public struct FleckAttachLink
	{
		// Token: 0x1700035B RID: 859
		// (get) Token: 0x060011C8 RID: 4552 RVA: 0x000660E2 File Offset: 0x000642E2
		public bool Linked
		{
			get
			{
				return this.targetInt.IsValid;
			}
		}

		// Token: 0x1700035C RID: 860
		// (get) Token: 0x060011C9 RID: 4553 RVA: 0x000660EF File Offset: 0x000642EF
		public TargetInfo Target
		{
			get
			{
				return this.targetInt;
			}
		}

		// Token: 0x1700035D RID: 861
		// (get) Token: 0x060011CA RID: 4554 RVA: 0x000660F7 File Offset: 0x000642F7
		public Vector3 LastDrawPos
		{
			get
			{
				return this.lastDrawPosInt;
			}
		}

		// Token: 0x060011CB RID: 4555 RVA: 0x000660FF File Offset: 0x000642FF
		public FleckAttachLink(TargetInfo target)
		{
			this.targetInt = target;
			this.detachAfterTicks = -1;
			this.lastDrawPosInt = Vector3.zero;
			if (target.IsValid)
			{
				this.UpdateDrawPos();
			}
		}

		// Token: 0x060011CC RID: 4556 RVA: 0x0006612C File Offset: 0x0006432C
		public void UpdateDrawPos()
		{
			if (this.targetInt.HasThing)
			{
				this.lastDrawPosInt = (this.targetInt.Thing.DrawPosHeld ?? this.lastDrawPosInt);
				return;
			}
			this.lastDrawPosInt = this.targetInt.Cell.ToVector3Shifted();
		}

		// Token: 0x04001030 RID: 4144
		private TargetInfo targetInt;

		// Token: 0x04001031 RID: 4145
		private Vector3 lastDrawPosInt;

		// Token: 0x04001032 RID: 4146
		public int detachAfterTicks;

		// Token: 0x04001033 RID: 4147
		public static readonly FleckAttachLink Invalid = new FleckAttachLink(TargetInfo.Invalid);
	}
}
