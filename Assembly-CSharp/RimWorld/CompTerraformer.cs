using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020019B3 RID: 6579
	public class CompTerraformer : ThingComp
	{
		// Token: 0x17001CBD RID: 7357
		// (get) Token: 0x0600A521 RID: 42273 RVA: 0x003AF11A File Offset: 0x003AD31A
		public CompProperties_Terraformer Props
		{
			get
			{
				return (CompProperties_Terraformer)this.props;
			}
		}

		// Token: 0x17001CBE RID: 7358
		// (get) Token: 0x0600A522 RID: 42274 RVA: 0x003AF127 File Offset: 0x003AD327
		public int TicksTillConvert
		{
			get
			{
				return Mathf.Max(this.convertTick - GenTicks.TicksGame, 0);
			}
		}

		// Token: 0x17001CBF RID: 7359
		// (get) Token: 0x0600A523 RID: 42275 RVA: 0x003AF13C File Offset: 0x003AD33C
		private bool Active
		{
			get
			{
				IntVec3 intVec;
				return this.parent.Spawned && this.TryGetCellToCovert(out intVec);
			}
		}

		// Token: 0x0600A524 RID: 42276 RVA: 0x003AF165 File Offset: 0x003AD365
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad && !this.parent.BeingTransportedOnGravship)
			{
				this.convertTick = GenTicks.TicksGame + this.Props.secondsPerConvert.SecondsToTicks();
			}
		}

		// Token: 0x0600A525 RID: 42277 RVA: 0x003AF19C File Offset: 0x003AD39C
		private bool TryGetCellToCovert(out IntVec3 cell)
		{
			int num = GenRadial.NumCellsInRadius(this.Props.radius);
			Map map = this.parent.Map;
			for (int i = 0; i < num; i++)
			{
				cell = this.parent.Position + GenRadial.RadialPattern[i];
				if (CompTerraformer.CanEverConvertCell(cell, map, this.Props.convertTerrainDef))
				{
					return true;
				}
			}
			cell = IntVec3.Invalid;
			return false;
		}

		// Token: 0x0600A526 RID: 42278 RVA: 0x003AF21C File Offset: 0x003AD41C
		public static bool CanEverConvertCell(IntVec3 cell, Map map, TerrainDef skip = null)
		{
			if (!cell.InBounds(map))
			{
				return false;
			}
			if (cell.GetEdifice(map) != null)
			{
				return false;
			}
			TerrainDef terrain = cell.GetTerrain(map);
			return terrain.canEverTerraform && (skip == null || terrain != skip) && terrain.passability != Traversability.Impassable && terrain.affordances.Contains(TerrainAffordanceDefOf.Light) && !terrain.isFoundation;
		}

		// Token: 0x0600A527 RID: 42279 RVA: 0x003AF284 File Offset: 0x003AD484
		private void Convert()
		{
			IntVec3 c;
			if (!this.TryGetCellToCovert(out c))
			{
				return;
			}
			this.parent.Map.terrainGrid.SetTerrain(c, this.Props.convertTerrainDef);
		}

		// Token: 0x0600A528 RID: 42280 RVA: 0x003AF2BD File Offset: 0x003AD4BD
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			CompTerraformer.<CompGetGizmosExtra>d__11 <CompGetGizmosExtra>d__ = new CompTerraformer.<CompGetGizmosExtra>d__11(-2);
			<CompGetGizmosExtra>d__.<>4__this = this;
			return <CompGetGizmosExtra>d__;
		}

		// Token: 0x0600A529 RID: 42281 RVA: 0x003AF2CD File Offset: 0x003AD4CD
		public override void CompTickRare()
		{
			if (GenTicks.TicksGame >= this.convertTick && this.Active)
			{
				this.convertTick = GenTicks.TicksGame + this.Props.secondsPerConvert.SecondsToTicks();
				this.Convert();
			}
		}

		// Token: 0x0600A52A RID: 42282 RVA: 0x003AF306 File Offset: 0x003AD506
		public override void PostDrawExtraSelectionOverlays()
		{
			GenDraw.DrawRadiusRing(this.parent.Position, this.Props.radius);
		}

		// Token: 0x0600A52B RID: 42283 RVA: 0x003AF323 File Offset: 0x003AD523
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.convertTick, "convertTick", 0, false);
		}

		// Token: 0x0400618B RID: 24971
		private int convertTick;
	}
}
