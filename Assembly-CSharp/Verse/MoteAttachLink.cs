using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x0200058A RID: 1418
	public struct MoteAttachLink
	{
		// Token: 0x1700085A RID: 2138
		// (get) Token: 0x060029E3 RID: 10723 RVA: 0x00100101 File Offset: 0x000FE301
		public bool Linked
		{
			get
			{
				return this.targetInt.IsValid;
			}
		}

		// Token: 0x1700085B RID: 2139
		// (get) Token: 0x060029E4 RID: 10724 RVA: 0x0010010E File Offset: 0x000FE30E
		public TargetInfo Target
		{
			get
			{
				return this.targetInt;
			}
		}

		// Token: 0x1700085C RID: 2140
		// (get) Token: 0x060029E5 RID: 10725 RVA: 0x00100116 File Offset: 0x000FE316
		public Vector3 LastDrawPos
		{
			get
			{
				return this.lastDrawPosInt;
			}
		}

		// Token: 0x060029E6 RID: 10726 RVA: 0x0010011E File Offset: 0x000FE31E
		public MoteAttachLink(TargetInfo target, Vector3 offset, bool rotateWithTarget = false)
		{
			this.targetInt = target;
			this.offsetInt = offset;
			this.rotateWithTarget = rotateWithTarget;
			this.lastDrawPosInt = Vector3.zero;
			if (target.IsValid)
			{
				this.UpdateDrawPos();
			}
		}

		// Token: 0x060029E7 RID: 10727 RVA: 0x0010014F File Offset: 0x000FE34F
		public void UpdateTarget(TargetInfo target, Vector3 offset)
		{
			this.targetInt = target;
			this.offsetInt = offset;
		}

		// Token: 0x060029E8 RID: 10728 RVA: 0x00100160 File Offset: 0x000FE360
		public Vector3 GetOffset()
		{
			if (!this.rotateWithTarget || !this.targetInt.HasThing || !this.targetInt.Thing.SpawnedOrAnyParentSpawned)
			{
				return this.offsetInt;
			}
			return this.offsetInt.RotatedBy(this.targetInt.Thing.Rotation);
		}

		// Token: 0x060029E9 RID: 10729 RVA: 0x001001B8 File Offset: 0x000FE3B8
		public void UpdateDrawPos()
		{
			if (this.targetInt.HasThing && this.targetInt.Thing.SpawnedOrAnyParentSpawned)
			{
				this.lastDrawPosInt = this.targetInt.Thing.SpawnedParentOrMe.DrawPos + this.GetOffset();
				return;
			}
			this.lastDrawPosInt = this.targetInt.Cell.ToVector3Shifted() + this.GetOffset();
		}

		// Token: 0x04001D0A RID: 7434
		private TargetInfo targetInt;

		// Token: 0x04001D0B RID: 7435
		private Vector3 offsetInt;

		// Token: 0x04001D0C RID: 7436
		private Vector3 lastDrawPosInt;

		// Token: 0x04001D0D RID: 7437
		public bool rotateWithTarget;

		// Token: 0x04001D0E RID: 7438
		public static readonly MoteAttachLink Invalid = new MoteAttachLink(TargetInfo.Invalid, Vector3.zero, false);
	}
}
