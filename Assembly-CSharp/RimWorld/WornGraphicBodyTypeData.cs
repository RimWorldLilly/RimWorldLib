using System;
using UnityEngine;

namespace RimWorld
{
	// Token: 0x02000F26 RID: 3878
	public struct WornGraphicBodyTypeData
	{
		// Token: 0x17001114 RID: 4372
		// (get) Token: 0x06006453 RID: 25683 RVA: 0x0023DA58 File Offset: 0x0023BC58
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

		// Token: 0x0400394A RID: 14666
		public Vector2 offset;

		// Token: 0x0400394B RID: 14667
		public Vector2? scale;
	}
}
