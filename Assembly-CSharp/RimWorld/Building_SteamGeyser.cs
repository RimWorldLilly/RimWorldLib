using System;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001ACA RID: 6858
	public class Building_SteamGeyser : Building
	{
		// Token: 0x0600AE82 RID: 44674 RVA: 0x003D7386 File Offset: 0x003D5586
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			this.steamSprayer = new IntermittentSteamSprayer(this)
			{
				startSprayCallback = new Action(this.StartSpray),
				endSprayCallback = new Action(this.EndSpray)
			};
		}

		// Token: 0x0600AE83 RID: 44675 RVA: 0x003D73C0 File Offset: 0x003D55C0
		private void StartSpray()
		{
			WeatherBuildupUtility.AddSnowRadial(this.OccupiedRect().RandomCell, base.Map, 4f, -0.06f);
			this.spraySustainer = SoundDefOf.GeyserSpray.TrySpawnSustainer(new TargetInfo(base.Position, base.Map, false));
			this.spraySustainerStartTick = Find.TickManager.TicksGame;
		}

		// Token: 0x0600AE84 RID: 44676 RVA: 0x003D7427 File Offset: 0x003D5627
		private void EndSpray()
		{
			if (this.spraySustainer != null)
			{
				this.spraySustainer.End();
				this.spraySustainer = null;
			}
		}

		// Token: 0x0600AE85 RID: 44677 RVA: 0x003D7444 File Offset: 0x003D5644
		protected override void Tick()
		{
			if (this.harvester == null)
			{
				this.steamSprayer.SteamSprayerTick();
			}
			if (this.spraySustainer != null && Find.TickManager.TicksGame > this.spraySustainerStartTick + 1000)
			{
				Log.Message("Geyser spray sustainer still playing after 1000 ticks. Force-ending.");
				this.spraySustainer.End();
				this.spraySustainer = null;
			}
		}

		// Token: 0x040064EF RID: 25839
		private IntermittentSteamSprayer steamSprayer;

		// Token: 0x040064F0 RID: 25840
		public Building harvester;

		// Token: 0x040064F1 RID: 25841
		private Sustainer spraySustainer;

		// Token: 0x040064F2 RID: 25842
		private int spraySustainerStartTick = -999;
	}
}
