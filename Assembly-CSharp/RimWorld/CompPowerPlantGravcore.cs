using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200135E RID: 4958
	public class CompPowerPlantGravcore : CompPowerPlant
	{
		// Token: 0x170014F6 RID: 5366
		// (get) Token: 0x06007ACE RID: 31438 RVA: 0x002BB85D File Offset: 0x002B9A5D
		protected override float DesiredPowerOutput
		{
			get
			{
				if (!this.onSubstructure)
				{
					return 0f;
				}
				return base.DesiredPowerOutput;
			}
		}

		// Token: 0x06007ACF RID: 31439 RVA: 0x002BB873 File Offset: 0x002B9A73
		public override void CompTickInterval(int delta)
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			if (!this.parent.IsHashIntervalTick(60, delta))
			{
				return;
			}
			this.CheckSubstructure();
		}

		// Token: 0x06007AD0 RID: 31440 RVA: 0x002BB89A File Offset: 0x002B9A9A
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.CheckSubstructure();
		}

		// Token: 0x06007AD1 RID: 31441 RVA: 0x002BB8AC File Offset: 0x002B9AAC
		private void CheckSubstructure()
		{
			TerrainDef terrainDef = this.parent.Map.terrainGrid.FoundationAt(this.parent.Position);
			this.onSubstructure = (terrainDef != null && terrainDef.IsSubstructure);
		}

		// Token: 0x06007AD2 RID: 31442 RVA: 0x002BB8EC File Offset: 0x002B9AEC
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.onSubstructure, "onSubstructure", false, false);
		}

		// Token: 0x06007AD3 RID: 31443 RVA: 0x002BB908 File Offset: 0x002B9B08
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (this.onSubstructure)
			{
				return text;
			}
			return text + "\n" + ("Disabled".Translate() + ": " + "MessageMustBePlacedOnSubstructure".Translate()).Colorize(ColorLibrary.RedReadable);
		}

		// Token: 0x04004D83 RID: 19843
		private const int CheckSubstructureInterval = 60;

		// Token: 0x04004D84 RID: 19844
		private bool onSubstructure;
	}
}
