using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020017F6 RID: 6134
	public class CompCauseGameCondition : ThingComp
	{
		// Token: 0x17001A06 RID: 6662
		// (get) Token: 0x06009913 RID: 39187 RVA: 0x003769B0 File Offset: 0x00374BB0
		public CompProperties_CausesGameCondition Props
		{
			get
			{
				return (CompProperties_CausesGameCondition)this.props;
			}
		}

		// Token: 0x17001A07 RID: 6663
		// (get) Token: 0x06009914 RID: 39188 RVA: 0x003769BD File Offset: 0x00374BBD
		public GameConditionDef ConditionDef
		{
			get
			{
				return this.Props.conditionDef;
			}
		}

		// Token: 0x17001A08 RID: 6664
		// (get) Token: 0x06009915 RID: 39189 RVA: 0x003769CA File Offset: 0x00374BCA
		public IEnumerable<GameCondition> CausedConditions
		{
			get
			{
				return this.causedConditions.Values;
			}
		}

		// Token: 0x17001A09 RID: 6665
		// (get) Token: 0x06009916 RID: 39190 RVA: 0x003769D7 File Offset: 0x00374BD7
		public virtual bool Active
		{
			get
			{
				return this.initiatableComp == null || this.initiatableComp.Initiated;
			}
		}

		// Token: 0x17001A0A RID: 6666
		// (get) Token: 0x06009917 RID: 39191 RVA: 0x003769EE File Offset: 0x00374BEE
		public PlanetTile MyTile
		{
			get
			{
				if (this.siteLink != null)
				{
					return this.siteLink.Tile;
				}
				if (this.parent.SpawnedOrAnyParentSpawned)
				{
					return this.parent.Tile;
				}
				return PlanetTile.Invalid;
			}
		}

		// Token: 0x06009918 RID: 39192 RVA: 0x00376A22 File Offset: 0x00374C22
		public void LinkWithSite(Site site)
		{
			this.siteLink = site;
		}

		// Token: 0x06009919 RID: 39193 RVA: 0x00376A2B File Offset: 0x00374C2B
		public override void PostPostMake()
		{
			base.PostPostMake();
			this.CacheComps();
		}

		// Token: 0x0600991A RID: 39194 RVA: 0x00376A39 File Offset: 0x00374C39
		private void CacheComps()
		{
			this.initiatableComp = this.parent.GetComp<CompInitiatable>();
		}

		// Token: 0x0600991B RID: 39195 RVA: 0x00376A4C File Offset: 0x00374C4C
		public override void PostExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.causedConditions.RemoveAll((KeyValuePair<Map, GameCondition> x) => !Find.Maps.Contains(x.Key));
			}
			Scribe_References.Look<Site>(ref this.siteLink, "siteLink", false);
			Scribe_Collections.Look<Map, GameCondition>(ref this.causedConditions, "causedConditions", LookMode.Reference, LookMode.Reference);
			if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
			{
				this.causedConditions.RemoveAll((KeyValuePair<Map, GameCondition> x) => x.Value == null);
				foreach (KeyValuePair<Map, GameCondition> keyValuePair in this.causedConditions)
				{
					keyValuePair.Value.conditionCauser = this.parent;
					keyValuePair.Value.hideSource = this.Props.hideSource;
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.CacheComps();
			}
		}

		// Token: 0x0600991C RID: 39196 RVA: 0x00376B5C File Offset: 0x00374D5C
		public bool InAoE(PlanetTile tile)
		{
			return this.MyTile.Valid && tile.Valid && this.Active && (tile == this.MyTile || (this.Props.worldRange > 0 && tile.Layer == this.MyTile.Layer && Find.WorldGrid.ApproxDistanceInTiles(tile, this.MyTile) < (float)this.Props.worldRange));
		}

		// Token: 0x0600991D RID: 39197 RVA: 0x00376BE4 File Offset: 0x00374DE4
		protected GameCondition GetConditionInstance(Map map)
		{
			GameCondition activeCondition;
			if (!this.causedConditions.TryGetValue(map, out activeCondition) && this.Props.preventConditionStacking)
			{
				activeCondition = map.GameConditionManager.GetActiveCondition(this.Props.conditionDef);
				if (activeCondition != null)
				{
					this.causedConditions.Add(map, activeCondition);
					this.SetupCondition(activeCondition, map);
				}
			}
			return activeCondition;
		}

		// Token: 0x0600991E RID: 39198 RVA: 0x00376C40 File Offset: 0x00374E40
		public override void CompTick()
		{
			if (this.Active)
			{
				foreach (Map map in Find.Maps)
				{
					if (this.InAoE(map.Tile))
					{
						this.EnforceConditionOn(map);
					}
				}
			}
			CompCauseGameCondition.tmpDeadConditionMaps.Clear();
			foreach (KeyValuePair<Map, GameCondition> keyValuePair in this.causedConditions)
			{
				if (keyValuePair.Value.Expired || !keyValuePair.Key.GameConditionManager.ConditionIsActive(keyValuePair.Value.def))
				{
					CompCauseGameCondition.tmpDeadConditionMaps.Add(keyValuePair.Key);
				}
			}
			foreach (Map key in CompCauseGameCondition.tmpDeadConditionMaps)
			{
				this.causedConditions.Remove(key);
			}
		}

		// Token: 0x0600991F RID: 39199 RVA: 0x00376D78 File Offset: 0x00374F78
		private GameCondition EnforceConditionOn(Map map)
		{
			GameCondition gameCondition = this.GetConditionInstance(map);
			if (gameCondition == null)
			{
				gameCondition = this.CreateConditionOn(map);
			}
			else
			{
				gameCondition.TicksLeft = gameCondition.TransitionTicks;
			}
			return gameCondition;
		}

		// Token: 0x06009920 RID: 39200 RVA: 0x00376DA8 File Offset: 0x00374FA8
		protected virtual GameCondition CreateConditionOn(Map map)
		{
			GameCondition gameCondition = GameConditionMaker.MakeCondition(this.ConditionDef, -1);
			gameCondition.Duration = gameCondition.TransitionTicks;
			gameCondition.conditionCauser = this.parent;
			gameCondition.hideSource = this.Props.hideSource;
			map.gameConditionManager.RegisterCondition(gameCondition);
			this.causedConditions.Add(map, gameCondition);
			this.SetupCondition(gameCondition, map);
			return gameCondition;
		}

		// Token: 0x06009921 RID: 39201 RVA: 0x00376E0D File Offset: 0x0037500D
		protected virtual void SetupCondition(GameCondition condition, Map map)
		{
			condition.suppressEndMessage = true;
		}

		// Token: 0x06009922 RID: 39202 RVA: 0x00376E18 File Offset: 0x00375018
		protected void ReSetupAllConditions()
		{
			foreach (KeyValuePair<Map, GameCondition> keyValuePair in this.causedConditions)
			{
				this.SetupCondition(keyValuePair.Value, keyValuePair.Key);
			}
		}

		// Token: 0x06009923 RID: 39203 RVA: 0x00376E78 File Offset: 0x00375078
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			Messages.Message("MessageConditionCauserDespawned".Translate(this.parent.def.LabelCap), new TargetInfo(this.parent.Position, previousMap, false), MessageTypeDefOf.NeutralEvent, true);
		}

		// Token: 0x06009924 RID: 39204 RVA: 0x00376ECC File Offset: 0x003750CC
		public override string CompInspectStringExtra()
		{
			if (!DebugSettings.godMode)
			{
				return base.CompInspectStringExtra();
			}
			GameCondition gameCondition = this.parent.Map.GameConditionManager.ActiveConditions.Find((GameCondition c) => c.def == this.Props.conditionDef);
			if (gameCondition == null)
			{
				return base.CompInspectStringExtra();
			}
			return "[DEV] Current map condition\n[DEV] Ticks Passed: " + gameCondition.TicksPassed.ToString() + "\n[DEV] Ticks Left: " + gameCondition.TicksLeft.ToString();
		}

		// Token: 0x06009925 RID: 39205 RVA: 0x00376F43 File Offset: 0x00375143
		public virtual void RandomizeSettings(Site site)
		{
		}

		// Token: 0x04005B72 RID: 23410
		protected CompInitiatable initiatableComp;

		// Token: 0x04005B73 RID: 23411
		protected Site siteLink;

		// Token: 0x04005B74 RID: 23412
		private Dictionary<Map, GameCondition> causedConditions = new Dictionary<Map, GameCondition>();

		// Token: 0x04005B75 RID: 23413
		private static List<Map> tmpDeadConditionMaps = new List<Map>();
	}
}
