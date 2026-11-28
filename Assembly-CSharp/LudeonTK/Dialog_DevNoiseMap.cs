using System;
using System.Runtime.CompilerServices;
using Verse;

namespace LudeonTK
{
	// Token: 0x02000049 RID: 73
	public class Dialog_DevNoiseMap : Dialog_DevNoiseBase
	{
		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060001E8 RID: 488 RVA: 0x0000E2DA File Offset: 0x0000C4DA
		protected override string Title
		{
			get
			{
				return "Map Noise Visualizer";
			}
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0000E2E1 File Offset: 0x0000C4E1
		protected override void OnNoiseChanged()
		{
			Map currentMap = Find.CurrentMap;
			if (currentMap == null)
			{
				return;
			}
			currentMap.mapDrawer.RegenerateEverythingNow();
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0000E2F7 File Offset: 0x0000C4F7
		protected override int GetSeed()
		{
			Map currentMap = Find.CurrentMap;
			if (currentMap == null)
			{
				return 0;
			}
			return currentMap.ConstantRandSeed;
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000E30C File Offset: 0x0000C50C
		protected override void DoWindowListing()
		{
			if (Find.CurrentMap == null)
			{
				base.PrintLabel("Not viewing map", GameFont.Tiny);
				return;
			}
			base.DoWindowListing();
			IntVec3 intVec = UI.MouseCell();
			if (!intVec.InBounds(Find.CurrentMap))
			{
				base.PrintLabel("Out of bounds", GameFont.Tiny);
				return;
			}
			float value = (float)this.noise.GetValue((double)intVec.x, (double)intVec.y, (double)intVec.z);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(15, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Selected cell: ");
			defaultInterpolatedStringHandler.AppendFormatted<IntVec3>(intVec);
			base.PrintLabel(defaultInterpolatedStringHandler.ToStringAndClear(), GameFont.Tiny);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(13, 1);
			defaultInterpolatedStringHandler2.AppendLiteral("Noise value: ");
			defaultInterpolatedStringHandler2.AppendFormatted<float>(value, "0.###");
			base.PrintLabel(defaultInterpolatedStringHandler2.ToStringAndClear(), GameFont.Tiny);
		}
	}
}
