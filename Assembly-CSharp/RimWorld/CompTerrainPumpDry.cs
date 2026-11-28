using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x020019B6 RID: 6582
	public class CompTerrainPumpDry : CompTerrainPump
	{
		// Token: 0x17001CC5 RID: 7365
		// (get) Token: 0x0600A53D RID: 42301 RVA: 0x003AF5F0 File Offset: 0x003AD7F0
		private CompProperties_TerrainPumpDry Props
		{
			get
			{
				return (CompProperties_TerrainPumpDry)this.props;
			}
		}

		// Token: 0x0600A53E RID: 42302 RVA: 0x003AF5FD File Offset: 0x003AD7FD
		protected override void AffectCell(IntVec3 c)
		{
			CompTerrainPumpDry.AffectCell(this.parent.Map, c);
		}

		// Token: 0x0600A53F RID: 42303 RVA: 0x003AF610 File Offset: 0x003AD810
		public static void AffectCell(Map map, IntVec3 c)
		{
			TerrainDef terrainDef = map.terrainGrid.TopTerrainAt(c);
			TerrainDef terrainToDryTo = CompTerrainPumpDry.GetTerrainToDryTo(map, terrainDef);
			if (terrainToDryTo != null)
			{
				map.terrainGrid.SetTerrain(c, terrainToDryTo);
			}
			TerrainDef terrainDef2 = map.terrainGrid.UnderTerrainAt(c);
			if (terrainDef2 != null)
			{
				TerrainDef terrainToDryTo2 = CompTerrainPumpDry.GetTerrainToDryTo(map, terrainDef2);
				if (terrainToDryTo2 != null)
				{
					map.terrainGrid.SetUnderTerrain(c, terrainToDryTo2);
				}
			}
		}

		// Token: 0x0600A540 RID: 42304 RVA: 0x003AF66A File Offset: 0x003AD86A
		public override void PostDeSpawn(Map map, DestroyMode mode = DestroyMode.Vanish)
		{
			base.PostDeSpawn(map, mode);
			if (this.sustainer != null && !this.sustainer.Ended)
			{
				this.sustainer.End();
			}
		}

		// Token: 0x0600A541 RID: 42305 RVA: 0x003AF694 File Offset: 0x003AD894
		public override void CompTickRare()
		{
			base.CompTickRare();
			if (!this.Props.soundWorking.NullOrUndefined() && base.Working && base.CurrentRadius < this.Props.radius - 0.0001f)
			{
				if (this.sustainer == null || this.sustainer.Ended)
				{
					this.sustainer = this.Props.soundWorking.TrySpawnSustainer(SoundInfo.InMap(this.parent, MaintenanceType.None));
				}
				this.sustainer.Maintain();
				return;
			}
			if (this.sustainer != null && !this.sustainer.Ended)
			{
				this.sustainer.End();
			}
		}

		// Token: 0x0600A542 RID: 42306 RVA: 0x003AF742 File Offset: 0x003AD942
		private static TerrainDef GetTerrainToDryTo(Map map, TerrainDef terrainDef)
		{
			if (terrainDef.driesTo == null)
			{
				return null;
			}
			if (map.Biome == BiomeDefOf.SeaIce)
			{
				return TerrainDefOf.Ice;
			}
			return terrainDef.driesTo;
		}

		// Token: 0x04006190 RID: 24976
		private Sustainer sustainer;
	}
}
