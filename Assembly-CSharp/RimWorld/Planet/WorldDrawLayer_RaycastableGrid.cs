using System;
using System.Collections;
using UnityEngine;

namespace RimWorld.Planet
{
	// Token: 0x0200231F RID: 8991
	public class WorldDrawLayer_RaycastableGrid : WorldDrawLayer
	{
		// Token: 0x170026E8 RID: 9960
		// (get) Token: 0x0600DE15 RID: 56853 RVA: 0x0052A29D File Offset: 0x0052849D
		public override bool VisibleWhenLayerNotSelected
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170026E9 RID: 9961
		// (get) Token: 0x0600DE16 RID: 56854 RVA: 0x0052A2A0 File Offset: 0x005284A0
		public override bool VisibleInBackground
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600DE17 RID: 56855 RVA: 0x0052A2A3 File Offset: 0x005284A3
		public override IEnumerable Regenerate()
		{
			WorldDrawLayer_RaycastableGrid.<Regenerate>d__5 <Regenerate>d__ = new WorldDrawLayer_RaycastableGrid.<Regenerate>d__5(-2);
			<Regenerate>d__.<>4__this = this;
			return <Regenerate>d__;
		}

		// Token: 0x0600DE18 RID: 56856 RVA: 0x0052A2B3 File Offset: 0x005284B3
		public override void Render()
		{
			if (this.ShouldRegenerate)
			{
				base.RegenerateNow();
			}
		}

		// Token: 0x04008E99 RID: 36505
		private Material material;
	}
}
