using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020022F6 RID: 8950
	public class MoveableStockpile : MoveableArea
	{
		// Token: 0x0600DD3D RID: 56637 RVA: 0x00525FD4 File Offset: 0x005241D4
		public MoveableStockpile()
		{
		}

		// Token: 0x0600DD3E RID: 56638 RVA: 0x00525FDC File Offset: 0x005241DC
		public MoveableStockpile(Gravship gravship, Zone_Stockpile stockpile) : base(gravship, stockpile.label, stockpile.RenamableLabel, stockpile.color, stockpile.ID)
		{
			this.hidden = stockpile.Hidden;
			this.settings = new StorageSettings();
			this.settings.CopyFrom(stockpile.settings);
		}

		// Token: 0x0600DD3F RID: 56639 RVA: 0x00526030 File Offset: 0x00524230
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.hidden, "hidden", false, false);
			Scribe_Deep.Look<StorageSettings>(ref this.settings, "settings", Array.Empty<object>());
		}

		// Token: 0x0600DD40 RID: 56640 RVA: 0x00526060 File Offset: 0x00524260
		public void TryCreateStockpile(ZoneManager zoneManager, IntVec3 newOrigin)
		{
			Zone_Stockpile zone_Stockpile = new Zone_Stockpile(StorageSettingsPreset.DefaultStockpile, zoneManager)
			{
				label = this.label,
				Hidden = this.hidden,
				color = this.color,
				ID = this.id
			};
			zone_Stockpile.settings = new StorageSettings(zone_Stockpile);
			zone_Stockpile.settings.CopyFrom(this.settings);
			zoneManager.RegisterZone(zone_Stockpile);
			foreach (IntVec3 b in base.RelativeCells)
			{
				zone_Stockpile.AddCell(newOrigin + b);
			}
		}

		// Token: 0x04008DCE RID: 36302
		private bool hidden;

		// Token: 0x04008DCF RID: 36303
		private StorageSettings settings;
	}
}
