using System;
using System.Collections;
using System.Collections.Generic;
using LudeonTK;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002318 RID: 8984
	public class WorldDrawLayer_DebugNoise : WorldDrawLayer
	{
		// Token: 0x170026D9 RID: 9945
		// (get) Token: 0x0600DDE4 RID: 56804 RVA: 0x005292A0 File Offset: 0x005274A0
		public override bool Visible
		{
			get
			{
				Dialog_DevNoiseWorld dialog_DevNoiseWorld;
				return base.Visible && this.TryGetWindow(out dialog_DevNoiseWorld) && dialog_DevNoiseWorld.alpha > 0f;
			}
		}

		// Token: 0x170026DA RID: 9946
		// (get) Token: 0x0600DDE5 RID: 56805 RVA: 0x005292CE File Offset: 0x005274CE
		public override bool VisibleWhenLayerNotSelected
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170026DB RID: 9947
		// (get) Token: 0x0600DDE6 RID: 56806 RVA: 0x005292D1 File Offset: 0x005274D1
		public override bool VisibleInBackground
		{
			get
			{
				return false;
			}
		}

		// Token: 0x0600DDE7 RID: 56807 RVA: 0x005292D4 File Offset: 0x005274D4
		private bool TryGetWindow(out Dialog_DevNoiseWorld window)
		{
			return Find.WindowStack.TryGetWindow<Dialog_DevNoiseWorld>(out window);
		}

		// Token: 0x0600DDE8 RID: 56808 RVA: 0x005292E1 File Offset: 0x005274E1
		public override IEnumerable Regenerate()
		{
			WorldDrawLayer_DebugNoise.<Regenerate>d__9 <Regenerate>d__ = new WorldDrawLayer_DebugNoise.<Regenerate>d__9(-2);
			<Regenerate>d__.<>4__this = this;
			return <Regenerate>d__;
		}

		// Token: 0x04008E79 RID: 36473
		private readonly List<List<int>> triangleIndexToTileID = new List<List<int>>();

		// Token: 0x04008E7A RID: 36474
		private Material material;
	}
}
