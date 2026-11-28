using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02002029 RID: 8233
	public class GameComponent_PsychicRitualManager : GameComponent
	{
		// Token: 0x0600D07A RID: 53370 RVA: 0x004CA62E File Offset: 0x004C882E
		public GameComponent_PsychicRitualManager(Game game)
		{
		}

		// Token: 0x0600D07B RID: 53371 RVA: 0x004CA64C File Offset: 0x004C884C
		public override void GameComponentTick()
		{
			if (!ModsConfig.AnomalyActive)
			{
				return;
			}
			this.availableRitualDefs = (from cd in this.ritualCooldowns
			where cd.Value <= Find.TickManager.TicksGame
			select cd).ToList<KeyValuePair<PsychicRitualDef, int>>();
			foreach (KeyValuePair<PsychicRitualDef, int> keyValuePair in this.availableRitualDefs)
			{
				this.ritualCooldowns.Remove(keyValuePair.Key);
				if (keyValuePair.Key.researchPrerequisite != null && keyValuePair.Key.researchPrerequisite.IsFinished)
				{
					Messages.Message("PsychicRitualCanBeCastAgain".Translate(keyValuePair.Key.label), MessageTypeDefOf.PositiveEvent, true);
				}
			}
		}

		// Token: 0x0600D07C RID: 53372 RVA: 0x004CA734 File Offset: 0x004C8934
		public void RegisterCooldown(PsychicRitualDef psychicRitualDef)
		{
			this.ritualCooldowns[psychicRitualDef] = Find.TickManager.TicksGame + psychicRitualDef.cooldownHours * 2500;
		}

		// Token: 0x0600D07D RID: 53373 RVA: 0x004CA759 File Offset: 0x004C8959
		public void ClearCooldown(PsychicRitualDef psychicRitualDef)
		{
			this.ritualCooldowns.Remove(psychicRitualDef);
		}

		// Token: 0x0600D07E RID: 53374 RVA: 0x004CA768 File Offset: 0x004C8968
		public int GetAvailableTick(PsychicRitualDef psychicRitualDef)
		{
			if (!this.ritualCooldowns.ContainsKey(psychicRitualDef))
			{
				return Find.TickManager.TicksGame;
			}
			return this.ritualCooldowns[psychicRitualDef];
		}

		// Token: 0x0600D07F RID: 53375 RVA: 0x004CA790 File Offset: 0x004C8990
		public AcceptanceReport CanInvoke(PsychicRitualDef psychicRitualDef, Map map)
		{
			if (!psychicRitualDef.castableOnPocketMaps && map.IsPocketMap)
			{
				return new AcceptanceReport("CantCastOnPocketMap".Translate(psychicRitualDef.label).Resolve());
			}
			if (map.Tile.Valid && !psychicRitualDef.layerWhitelist.NullOrEmpty<PlanetLayerDef>() && !psychicRitualDef.layerWhitelist.Contains(map.Tile.LayerDef))
			{
				return new AcceptanceReport("CannotPerformPlanetLayer".Translate(map.Tile.LayerDef.gerundLabel.Named("GERUND"), map.Tile.LayerDef.label.Named("LAYER")).EndWithPeriod().Resolve());
			}
			int num;
			if (!this.ritualCooldowns.TryGetValue(psychicRitualDef, out num))
			{
				return true;
			}
			if (Find.TickManager.TicksGame >= num)
			{
				return true;
			}
			return new AcceptanceReport("PsychicRitualOnCooldown".Translate((this.GetAvailableTick(psychicRitualDef) - Find.TickManager.TicksGame).ToStringTicksToPeriod(true, false, true, true, false)).Resolve());
		}

		// Token: 0x0600D080 RID: 53376 RVA: 0x004CA8C8 File Offset: 0x004C8AC8
		public void ClearAllCooldowns()
		{
			this.ritualCooldowns.Clear();
		}

		// Token: 0x0600D081 RID: 53377 RVA: 0x004CA8D8 File Offset: 0x004C8AD8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<PsychicRitualDef, int>(ref this.ritualCooldowns, "ritualCooldowns", LookMode.Def, LookMode.Value, ref this.tmpRitualDefs, ref this.tmpCooldowns, true, false, false);
		}

		// Token: 0x040084A6 RID: 33958
		private Dictionary<PsychicRitualDef, int> ritualCooldowns = new Dictionary<PsychicRitualDef, int>();

		// Token: 0x040084A7 RID: 33959
		private List<KeyValuePair<PsychicRitualDef, int>> availableRitualDefs = new List<KeyValuePair<PsychicRitualDef, int>>();

		// Token: 0x040084A8 RID: 33960
		private List<PsychicRitualDef> tmpRitualDefs;

		// Token: 0x040084A9 RID: 33961
		private List<int> tmpCooldowns;
	}
}
