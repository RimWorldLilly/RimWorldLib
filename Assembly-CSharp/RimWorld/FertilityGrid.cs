using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001210 RID: 4624
	public sealed class FertilityGrid
	{
		// Token: 0x17001388 RID: 5000
		// (get) Token: 0x060072D3 RID: 29395 RVA: 0x00282504 File Offset: 0x00280704
		public CellBoolDrawer Drawer
		{
			get
			{
				if (this.drawerInt == null)
				{
					this.drawerInt = new CellBoolDrawer(new Func<int, bool>(this.CellBoolDrawerGetBoolInt), new Func<Color>(this.CellBoolDrawerColorInt), new Func<int, Color>(this.CellBoolDrawerGetExtraColorInt), this.map.Size.x, this.map.Size.z, 3610, 0.33f);
				}
				return this.drawerInt;
			}
		}

		// Token: 0x060072D4 RID: 29396 RVA: 0x00282578 File Offset: 0x00280778
		public FertilityGrid(Map map)
		{
			this.map = map;
		}

		// Token: 0x060072D5 RID: 29397 RVA: 0x00282587 File Offset: 0x00280787
		public float FertilityAt(IntVec3 loc)
		{
			return this.CalculateFertilityAt(loc);
		}

		// Token: 0x060072D6 RID: 29398 RVA: 0x00282590 File Offset: 0x00280790
		private float CalculateFertilityAt(IntVec3 loc)
		{
			Thing edifice = loc.GetEdifice(this.map);
			if (edifice != null && edifice.def.AffectsFertility)
			{
				return edifice.def.fertility;
			}
			float num = this.map.terrainGrid.TerrainAt(loc).fertility;
			if (ModsConfig.BiotechActive && this.map.pollutionGrid.IsPolluted(loc))
			{
				num = Mathf.Min(num, 0.5f);
			}
			return num;
		}

		// Token: 0x060072D7 RID: 29399 RVA: 0x00282604 File Offset: 0x00280804
		public void FertilityGridUpdate()
		{
			if (Find.PlaySettings.showFertilityOverlay && !Find.ScreenshotModeHandler.Active)
			{
				this.Drawer.MarkForDraw();
			}
			this.Drawer.CellBoolDrawerUpdate();
		}

		// Token: 0x060072D8 RID: 29400 RVA: 0x00282634 File Offset: 0x00280834
		private Color CellBoolDrawerColorInt()
		{
			return Color.white;
		}

		// Token: 0x060072D9 RID: 29401 RVA: 0x0028263C File Offset: 0x0028083C
		private bool CellBoolDrawerGetBoolInt(int index)
		{
			IntVec3 intVec = CellIndicesUtility.IndexToCell(index, this.map.Size.x);
			if (intVec.Filled(this.map) || intVec.Fogged(this.map))
			{
				return false;
			}
			float num = this.FertilityAt(intVec);
			if (ModsConfig.BiotechActive)
			{
				return num >= 0.5f;
			}
			return num > 0.69f;
		}

		// Token: 0x060072DA RID: 29402 RVA: 0x002826A4 File Offset: 0x002808A4
		private Color CellBoolDrawerGetExtraColorInt(int index)
		{
			float num = this.FertilityAt(CellIndicesUtility.IndexToCell(index, this.map.Size.x));
			if (ModsConfig.BiotechActive && num <= 0.5f)
			{
				return FertilityGrid.ExtraLowFertilityColor;
			}
			if (num <= 0.95f)
			{
				return FertilityGrid.LowFertilityColor;
			}
			if (num <= 1.1f)
			{
				return FertilityGrid.MediumFertilityColor;
			}
			if (num >= 1.1f)
			{
				return FertilityGrid.HighFertilityColor;
			}
			return Color.white;
		}

		// Token: 0x040048A0 RID: 18592
		public const float MaxPollutedFertility = 0.5f;

		// Token: 0x040048A1 RID: 18593
		private Map map;

		// Token: 0x040048A2 RID: 18594
		private CellBoolDrawer drawerInt;

		// Token: 0x040048A3 RID: 18595
		private static readonly Color ExtraLowFertilityColor = Color.red;

		// Token: 0x040048A4 RID: 18596
		private static readonly Color LowFertilityColor = Color.yellow;

		// Token: 0x040048A5 RID: 18597
		private static readonly Color MediumFertilityColor = new Color(0.59f, 0.98f, 0.59f, 1f);

		// Token: 0x040048A6 RID: 18598
		private static readonly Color HighFertilityColor = Color.green;
	}
}
