using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02000F27 RID: 3879
	public struct WornGraphicDirectionData
	{
		// Token: 0x17001115 RID: 4373
		// (get) Token: 0x06006454 RID: 25684 RVA: 0x0023DA84 File Offset: 0x0023BC84
		public Vector2 Scale
		{
			get
			{
				Vector2? vector = this.scale;
				if (vector == null)
				{
					return Vector2.one;
				}
				return vector.GetValueOrDefault();
			}
		}

		// Token: 0x0400394C RID: 14668
		public Vector2 offset;

		// Token: 0x0400394D RID: 14669
		public Vector2? scale;

		// Token: 0x0400394E RID: 14670
		public WornGraphicBodyTypeData male;

		// Token: 0x0400394F RID: 14671
		public WornGraphicBodyTypeData female;

		// Token: 0x04003950 RID: 14672
		public WornGraphicBodyTypeData thin;

		// Token: 0x04003951 RID: 14673
		public WornGraphicBodyTypeData hulk;

		// Token: 0x04003952 RID: 14674
		public WornGraphicBodyTypeData fat;
	}
}
