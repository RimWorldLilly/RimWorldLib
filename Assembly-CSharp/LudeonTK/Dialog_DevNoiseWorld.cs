using System;
using System.Runtime.CompilerServices;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace LudeonTK
{
	// Token: 0x0200004A RID: 74
	public class Dialog_DevNoiseWorld : Dialog_DevNoiseBase
	{
		// Token: 0x1700005E RID: 94
		// (get) Token: 0x060001ED RID: 493 RVA: 0x0000E3DA File Offset: 0x0000C5DA
		protected override string Title
		{
			get
			{
				return "World Noise Visualizer";
			}
		}

		// Token: 0x060001EE RID: 494 RVA: 0x0000E3E1 File Offset: 0x0000C5E1
		public override void PreOpen()
		{
			base.PreOpen();
			this.OnNoiseChanged();
		}

		// Token: 0x060001EF RID: 495 RVA: 0x0000E3EF File Offset: 0x0000C5EF
		public override void PostClose()
		{
			base.PostClose();
			this.OnNoiseChanged();
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000E3FD File Offset: 0x0000C5FD
		protected override void OnNoiseChanged()
		{
			Find.World.renderer.SetDirty<WorldDrawLayer_DebugNoise>(PlanetLayer.Selected);
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000E413 File Offset: 0x0000C613
		protected override int GetSeed()
		{
			return Find.World.info.Seed;
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000E424 File Offset: 0x0000C624
		protected override void DoWindowListing()
		{
			WorldDrawLayer_DebugNoise worldDrawLayer_DebugNoise;
			if (!Find.World.renderer.TryGetLayer<WorldDrawLayer_DebugNoise>(PlanetLayer.Selected, out worldDrawLayer_DebugNoise))
			{
				return;
			}
			base.DoWindowListing();
			if (Find.WorldSelector.SelectedTile == PlanetTile.Invalid)
			{
				base.PrintLabel("No tile selected", GameFont.Tiny);
				return;
			}
			PlanetTile selectedTile = Find.WorldSelector.SelectedTile;
			Vector3 tileCenter = Find.WorldGrid.GetTileCenter(selectedTile);
			float value = (float)this.noise.GetValue((double)tileCenter.x, (double)tileCenter.y, (double)tileCenter.z);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(17, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Selected tile (");
			defaultInterpolatedStringHandler.AppendFormatted<PlanetTile>(selectedTile);
			defaultInterpolatedStringHandler.AppendLiteral("):");
			base.PrintLabel(defaultInterpolatedStringHandler.ToStringAndClear(), GameFont.Tiny);
			base.PrintLabel("Coordinates: " + tileCenter.ToString("0.#"), GameFont.Tiny);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler2 = new DefaultInterpolatedStringHandler(13, 1);
			defaultInterpolatedStringHandler2.AppendLiteral("Noise value: ");
			defaultInterpolatedStringHandler2.AppendFormatted<float>(value, "0.###");
			base.PrintLabel(defaultInterpolatedStringHandler2.ToStringAndClear(), GameFont.Tiny);
		}
	}
}
