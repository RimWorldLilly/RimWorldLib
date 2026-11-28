using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F7F RID: 3967
	public class IssueDef : Def
	{
		// Token: 0x1700115E RID: 4446
		// (get) Token: 0x0600658F RID: 25999 RVA: 0x00242D06 File Offset: 0x00240F06
		public Texture2D Icon
		{
			get
			{
				if (string.IsNullOrEmpty(this.iconPath))
				{
					return null;
				}
				if (this.icon == null)
				{
					this.icon = ContentFinder<Texture2D>.Get(this.iconPath, true);
				}
				return this.icon;
			}
		}

		// Token: 0x1700115F RID: 4447
		// (get) Token: 0x06006590 RID: 26000 RVA: 0x00242D3D File Offset: 0x00240F3D
		public bool HasDefaultPrecept
		{
			get
			{
				return DefDatabase<PreceptDef>.AllDefs.Any((PreceptDef x) => x.issue == this && x.defaultSelectionWeight > 0f && x.visible);
			}
		}

		// Token: 0x04003C3C RID: 15420
		public bool allowMultiplePrecepts;

		// Token: 0x04003C3D RID: 15421
		public bool forceWriteLabelInPreceptFloatMenuOption;

		// Token: 0x04003C3E RID: 15422
		public string iconPath;

		// Token: 0x04003C3F RID: 15423
		private Texture2D icon;
	}
}
