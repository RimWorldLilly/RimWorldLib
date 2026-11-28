using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020009C5 RID: 2501
	public class JobDriver_RemoveFloor : JobDriver_AffectFloor
	{
		// Token: 0x17000DCA RID: 3530
		// (get) Token: 0x06004BFE RID: 19454 RVA: 0x001D639E File Offset: 0x001D459E
		protected override int BaseWorkAmount
		{
			get
			{
				return 200;
			}
		}

		// Token: 0x17000DCB RID: 3531
		// (get) Token: 0x06004BFF RID: 19455 RVA: 0x001D63A5 File Offset: 0x001D45A5
		protected override DesignationDef DesDef
		{
			get
			{
				return DesignationDefOf.RemoveFloor;
			}
		}

		// Token: 0x17000DCC RID: 3532
		// (get) Token: 0x06004C00 RID: 19456 RVA: 0x001D63AC File Offset: 0x001D45AC
		protected override StatDef SpeedStat
		{
			get
			{
				return StatDefOf.ConstructionSpeed;
			}
		}

		// Token: 0x06004C01 RID: 19457 RVA: 0x001D63B3 File Offset: 0x001D45B3
		public JobDriver_RemoveFloor()
		{
			this.clearSnow = true;
		}

		// Token: 0x06004C02 RID: 19458 RVA: 0x001D63C4 File Offset: 0x001D45C4
		protected override void DoEffect(IntVec3 c)
		{
			if (base.Map.terrainGrid.CanRemoveTopLayerAt(c))
			{
				TerrainDef terrainDef = base.Map.terrainGrid.TempTerrainAt(c);
				if (terrainDef != null && terrainDef.Removable)
				{
					base.Map.terrainGrid.RemoveTempTerrain(c, false, false);
				}
				else
				{
					base.Map.terrainGrid.RemoveTopLayer(base.TargetLocA, true);
				}
				FilthMaker.RemoveAllFilth(c, base.Map);
			}
		}
	}
}
