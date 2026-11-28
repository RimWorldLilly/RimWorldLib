using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001AF9 RID: 6905
	[StaticConstructorOnStartup]
	public class MonumentMarker : Thing
	{
		// Token: 0x17001FD5 RID: 8149
		// (get) Token: 0x0600B07D RID: 45181 RVA: 0x003E2980 File Offset: 0x003E0B80
		public override CellRect? CustomRectForSelector
		{
			get
			{
				if (!base.Spawned)
				{
					return null;
				}
				return new CellRect?(this.sketch.OccupiedRect.MovedBy(base.Position));
			}
		}

		// Token: 0x17001FD6 RID: 8150
		// (get) Token: 0x0600B07E RID: 45182 RVA: 0x003E29C0 File Offset: 0x003E0BC0
		public bool AllDone
		{
			get
			{
				if (!base.Spawned)
				{
					return false;
				}
				foreach (SketchEntity sketchEntity in this.sketch.Entities)
				{
					if (!sketchEntity.IsSameSpawned(base.Position + sketchEntity.pos, base.Map))
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17001FD7 RID: 8151
		// (get) Token: 0x0600B07F RID: 45183 RVA: 0x003E2A44 File Offset: 0x003E0C44
		public IntVec2 Size
		{
			get
			{
				return this.sketch.OccupiedSize;
			}
		}

		// Token: 0x17001FD8 RID: 8152
		// (get) Token: 0x0600B080 RID: 45184 RVA: 0x003E2A54 File Offset: 0x003E0C54
		public Thing FirstDisallowedBuilding
		{
			get
			{
				if (!base.Spawned)
				{
					return null;
				}
				List<SketchTerrain> terrain = this.sketch.Terrain;
				for (int i = 0; i < terrain.Count; i++)
				{
					MonumentMarker.tmpAllowedBuildings.Clear();
					SketchThing sketchThing;
					List<SketchThing> list;
					this.sketch.ThingsAt(terrain[i].pos, out sketchThing, out list);
					if (sketchThing != null)
					{
						MonumentMarker.tmpAllowedBuildings.Add(sketchThing.def);
					}
					if (list != null)
					{
						for (int j = 0; j < list.Count; j++)
						{
							MonumentMarker.tmpAllowedBuildings.Add(list[j].def);
						}
					}
					List<Thing> thingList = (terrain[i].pos + base.Position).GetThingList(base.Map);
					for (int k = 0; k < thingList.Count; k++)
					{
						if (thingList[k].def.IsBuildingArtificial && !thingList[k].def.IsBlueprint && !thingList[k].def.IsFrame && !thingList[k].def.building.isPowerConduit && !MonumentMarker.tmpAllowedBuildings.Contains(thingList[k].def))
						{
							return thingList[k];
						}
					}
				}
				return null;
			}
		}

		// Token: 0x17001FD9 RID: 8153
		// (get) Token: 0x0600B081 RID: 45185 RVA: 0x003E2BB3 File Offset: 0x003E0DB3
		public bool AnyDisallowedBuilding
		{
			get
			{
				return this.FirstDisallowedBuilding != null;
			}
		}

		// Token: 0x17001FDA RID: 8154
		// (get) Token: 0x0600B082 RID: 45186 RVA: 0x003E2BC0 File Offset: 0x003E0DC0
		public SketchEntity FirstEntityWithMissingBlueprint
		{
			get
			{
				if (!base.Spawned)
				{
					return null;
				}
				foreach (SketchEntity sketchEntity in this.sketch.Entities)
				{
					if (!sketchEntity.IsSameSpawnedOrBlueprintOrFrame(base.Position + sketchEntity.pos, base.Map))
					{
						return sketchEntity;
					}
				}
				return null;
			}
		}

		// Token: 0x17001FDB RID: 8155
		// (get) Token: 0x0600B083 RID: 45187 RVA: 0x003E2C44 File Offset: 0x003E0E44
		public bool DisallowedBuildingTicksExpired
		{
			get
			{
				return this.ticksSinceDisallowedBuilding >= 60000;
			}
		}

		// Token: 0x0600B084 RID: 45188 RVA: 0x003E2C56 File Offset: 0x003E0E56
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.sketch.Rotate(base.Rotation);
			}
			if (!ModLister.CheckRoyalty("Monument"))
			{
				this.Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x0600B085 RID: 45189 RVA: 0x003E2C88 File Offset: 0x003E0E88
		protected override void Tick()
		{
			if (!this.IsHashIntervalTick(177))
			{
				return;
			}
			bool allDone = this.AllDone;
			if (!this.complete && allDone)
			{
				this.complete = true;
				QuestUtility.SendQuestTargetSignals(this.questTags, "MonumentCompleted", this.Named("SUBJECT"));
			}
			if (this.complete && !allDone)
			{
				QuestUtility.SendQuestTargetSignals(this.questTags, "MonumentDestroyed", this.Named("SUBJECT"));
				if (!base.Destroyed)
				{
					this.Destroy(DestroyMode.Vanish);
				}
				return;
			}
			if (allDone)
			{
				if (this.AnyDisallowedBuilding)
				{
					this.ticksSinceDisallowedBuilding += 177;
					if (this.DisallowedBuildingTicksExpired)
					{
						Messages.Message("MessageMonumentDestroyedBecauseOfDisallowedBuilding".Translate(), new TargetInfo(base.Position, base.Map, false), MessageTypeDefOf.NegativeEvent, true);
						QuestUtility.SendQuestTargetSignals(this.questTags, "MonumentDestroyed", this.Named("SUBJECT"));
						if (!base.Destroyed)
						{
							this.Destroy(DestroyMode.Vanish);
						}
						return;
					}
				}
				else
				{
					this.ticksSinceDisallowedBuilding = 0;
				}
			}
		}

		// Token: 0x0600B086 RID: 45190 RVA: 0x003E2D9C File Offset: 0x003E0F9C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<Sketch>(ref this.sketch, "sketch", Array.Empty<object>());
			Scribe_Values.Look<int>(ref this.ticksSinceDisallowedBuilding, "ticksSinceDisallowedBuilding", 0, false);
			Scribe_Values.Look<bool>(ref this.complete, "complete", false, false);
		}

		// Token: 0x0600B087 RID: 45191 RVA: 0x003E2DE8 File Offset: 0x003E0FE8
		protected override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			this.DrawGhost(drawLoc.ToIntVec3(), false, base.Rotation);
		}

		// Token: 0x0600B088 RID: 45192 RVA: 0x003E2E00 File Offset: 0x003E1000
		public void DrawGhost(IntVec3 at, bool placingMode, Rot4 rotation)
		{
			CellRect rect = this.sketch.OccupiedRect.MovedBy(at);
			Blueprint_Install thingToIgnore = this.FindMyBlueprint(rect, Find.CurrentMap);
			this.sketch.Rotate(rotation);
			Func<SketchEntity, IntVec3, List<Thing>, Map, bool> validator = null;
			if (placingMode)
			{
				validator = ((SketchEntity entity, IntVec3 offset, List<Thing> things, Map map) => MonumentMarkerUtility.GetFirstAdjacentBuilding(entity, offset, things, map) == null);
			}
			this.sketch.DrawGhost(at, Sketch.SpawnPosType.Unchanged, placingMode, thingToIgnore, validator);
		}

		// Token: 0x0600B089 RID: 45193 RVA: 0x003E2E70 File Offset: 0x003E1070
		public Blueprint_Install FindMyBlueprint(CellRect rect, Map map)
		{
			foreach (IntVec3 c in rect)
			{
				if (c.InBounds(map))
				{
					List<Thing> thingList = c.GetThingList(map);
					for (int i = 0; i < thingList.Count; i++)
					{
						Blueprint_Install blueprint_Install = thingList[i] as Blueprint_Install;
						if (blueprint_Install != null && blueprint_Install.ThingToInstall == this)
						{
							return blueprint_Install;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x0600B08A RID: 45194 RVA: 0x003E2F00 File Offset: 0x003E1100
		public override IEnumerable<Gizmo> GetGizmos()
		{
			MonumentMarker.<GetGizmos>d__29 <GetGizmos>d__ = new MonumentMarker.<GetGizmos>d__29(-2);
			<GetGizmos>d__.<>4__this = this;
			return <GetGizmos>d__;
		}

		// Token: 0x0600B08B RID: 45195 RVA: 0x003E2F10 File Offset: 0x003E1110
		private Command_Action GetPlaceBlueprintsCommand(SketchBuildable buildable)
		{
			Action<ThingDef> <>9__1;
			return new Command_Action
			{
				defaultLabel = "CommandPlaceBlueprintsSpecific".Translate(buildable.Label).CapitalizeFirst(),
				defaultDesc = "CommandPlaceBlueprintsSpecificDesc".Translate(buildable.Label).CapitalizeFirst(),
				icon = MonumentMarker.PlaceBlueprintsCommandTex,
				Order = 2000f,
				action = delegate()
				{
					List<ThingDef> list = this.AllowedStuffsFor(buildable);
					if (!list.Any<ThingDef>())
					{
						this.PlaceBlueprintsSimilarTo(buildable, null);
						SoundDefOf.Click.PlayOneShotOnCamera(null);
						return;
					}
					if (list.Count<ThingDef>() == 1)
					{
						this.PlaceBlueprintsSimilarTo(buildable, list.First<ThingDef>());
						SoundDefOf.Click.PlayOneShotOnCamera(null);
						return;
					}
					MonumentMarker <>4__this = this;
					IEnumerable<ThingDef> allowedStuff = list;
					Action<ThingDef> action;
					if ((action = <>9__1) == null)
					{
						action = (<>9__1 = delegate(ThingDef stuff)
						{
							this.PlaceBlueprintsSimilarTo(buildable, stuff);
						});
					}
					<>4__this.ListFloatMenuOptions(allowedStuff, action);
				}
			};
		}

		// Token: 0x0600B08C RID: 45196 RVA: 0x003E2FB8 File Offset: 0x003E11B8
		private void ListFloatMenuOptions(IEnumerable<ThingDef> allowedStuff, Action<ThingDef> action)
		{
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			bool flag = false;
			foreach (ThingDef def in allowedStuff)
			{
				if (base.Map.listerThings.ThingsOfDef(def).Count > 0)
				{
					flag = true;
					break;
				}
			}
			foreach (ThingDef thingDef in allowedStuff)
			{
				if (!flag || base.Map.listerThings.ThingsOfDef(thingDef).Count != 0)
				{
					ThingDef stuffLocal = thingDef;
					list.Add(new FloatMenuOption(stuffLocal.LabelCap, delegate()
					{
						action(stuffLocal);
					}, thingDef, null, false, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0, null));
				}
			}
			Find.WindowStack.Add(new FloatMenu(list));
		}

		// Token: 0x0600B08D RID: 45197 RVA: 0x003E30E8 File Offset: 0x003E12E8
		public void DebugBuildAll()
		{
			this.sketch.Spawn(base.Map, base.Position, Faction.OfPlayer, Sketch.SpawnPosType.Unchanged, Sketch.SpawnMode.Normal, false, false, false, null, false, false, null, null, null);
		}

		// Token: 0x0600B08E RID: 45198 RVA: 0x003E311C File Offset: 0x003E131C
		private void PlaceBlueprintsSimilarTo(SketchBuildable buildable, ThingDef preferredStuffIfNone)
		{
			bool flag = buildable is SketchTerrain;
			foreach (SketchBuildable sketchBuildable in this.sketch.Buildables)
			{
				if (flag)
				{
					SketchTerrain sketchTerrain = sketchBuildable as SketchTerrain;
					if (sketchTerrain != null && sketchTerrain.IsSameOrSimilar(buildable.Buildable))
					{
						goto IL_5E;
					}
				}
				if (flag)
				{
					continue;
				}
				SketchThing sketchThing = sketchBuildable as SketchThing;
				if (sketchThing == null || buildable.Buildable != sketchThing.def)
				{
					continue;
				}
				IL_5E:
				MonumentMarker.tmpBuildables.Add(sketchBuildable);
			}
			foreach (SketchBuildable entity in MonumentMarker.tmpBuildables)
			{
				this.PlaceBlueprint(entity, preferredStuffIfNone);
			}
			MonumentMarker.tmpBuildables.Clear();
		}

		// Token: 0x0600B08F RID: 45199 RVA: 0x003E320C File Offset: 0x003E140C
		private void PlaceAllBlueprints(ThingDef preferredStuffIfNone)
		{
			foreach (SketchEntity entity in this.sketch.Entities)
			{
				this.PlaceBlueprint(entity, preferredStuffIfNone);
			}
		}

		// Token: 0x0600B090 RID: 45200 RVA: 0x003E3268 File Offset: 0x003E1468
		private void PlaceBlueprint(SketchEntity entity, ThingDef preferredStuffIfNone)
		{
			SketchBuildable sketchBuildable = entity as SketchBuildable;
			if (sketchBuildable == null)
			{
				return;
			}
			if (entity.IsSameSpawnedOrBlueprintOrFrame(entity.pos + base.Position, base.Map))
			{
				return;
			}
			if (entity.IsSpawningBlocked(entity.pos + base.Position, base.Map, null, false))
			{
				return;
			}
			if (BuildCopyCommandUtility.FindAllowedDesignator(sketchBuildable.Buildable, true) == null)
			{
				return;
			}
			SketchThing sketchThing = entity as SketchThing;
			if (sketchThing != null && sketchThing.def.MadeFromStuff && sketchThing.stuff == null && preferredStuffIfNone != null && preferredStuffIfNone.stuffProps.CanMake(sketchThing.def))
			{
				sketchThing.stuff = preferredStuffIfNone;
				entity.Spawn(entity.pos + base.Position, base.Map, Faction.OfPlayer, Sketch.SpawnMode.Blueprint, false, false, null, false, null);
				sketchThing.stuff = null;
				return;
			}
			SketchTerrain sketchTerrain = entity as SketchTerrain;
			if (sketchTerrain != null && sketchTerrain.stuffForComparingSimilar == null && preferredStuffIfNone != null)
			{
				sketchTerrain.stuffForComparingSimilar = preferredStuffIfNone;
				entity.Spawn(entity.pos + base.Position, base.Map, Faction.OfPlayer, Sketch.SpawnMode.Blueprint, false, false, null, false, null);
				sketchTerrain.stuffForComparingSimilar = null;
				return;
			}
			entity.Spawn(entity.pos + base.Position, base.Map, Faction.OfPlayer, Sketch.SpawnMode.Blueprint, false, false, null, false, null);
		}

		// Token: 0x0600B091 RID: 45201 RVA: 0x003E33B4 File Offset: 0x003E15B4
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			Quest quest = Find.QuestManager.QuestsListForReading.FirstOrDefault((Quest q) => q.QuestLookTargets.Contains(this));
			if (quest != null)
			{
				stringBuilder.Append("Quest".Translate() + ": " + quest.name);
			}
			QuestUtility.AppendInspectStringsFromQuestParts(stringBuilder, this);
			if (base.Spawned && !this.AllDone)
			{
				MonumentMarker.tmpBuiltParts.Clear();
				foreach (SketchEntity sketchEntity in this.sketch.Entities)
				{
					Pair<int, int> value;
					if (!MonumentMarker.tmpBuiltParts.TryGetValue(sketchEntity.LabelCap, out value))
					{
						value = default(Pair<int, int>);
					}
					if (sketchEntity.IsSameSpawned(sketchEntity.pos + base.Position, base.Map))
					{
						value = new Pair<int, int>(value.First + 1, value.Second + 1);
					}
					else
					{
						value = new Pair<int, int>(value.First, value.Second + 1);
					}
					MonumentMarker.tmpBuiltParts[sketchEntity.LabelCap] = value;
				}
				foreach (KeyValuePair<string, Pair<int, int>> keyValuePair in MonumentMarker.tmpBuiltParts)
				{
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.Append(string.Concat(new string[]
					{
						keyValuePair.Key.CapitalizeFirst(),
						": ",
						keyValuePair.Value.First.ToString(),
						" / ",
						keyValuePair.Value.Second.ToString()
					}));
				}
				MonumentMarker.tmpBuiltParts.Clear();
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600B092 RID: 45202 RVA: 0x003E35CC File Offset: 0x003E17CC
		private void RemovePossiblyRelatedBlueprints()
		{
			if (!base.Spawned)
			{
				return;
			}
			foreach (SketchBuildable sketchBuildable in this.sketch.Buildables)
			{
				Blueprint blueprint = sketchBuildable.GetSpawnedBlueprintOrFrame(base.Position + sketchBuildable.pos, base.Map) as Blueprint;
				if (blueprint != null)
				{
					blueprint.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x0600B093 RID: 45203 RVA: 0x003E3654 File Offset: 0x003E1854
		public bool IsPart(Thing thing)
		{
			MonumentMarker.<>c__DisplayClass40_0 CS$<>8__locals1;
			CS$<>8__locals1.thing = thing;
			CS$<>8__locals1.<>4__this = this;
			if (!base.Spawned)
			{
				return false;
			}
			if (!this.sketch.OccupiedRect.MovedBy(base.Position).Contains(CS$<>8__locals1.thing.Position))
			{
				return false;
			}
			SketchThing sketchThing;
			List<SketchThing> list;
			this.sketch.ThingsAt(CS$<>8__locals1.thing.Position - base.Position, out sketchThing, out list);
			if (sketchThing != null && this.<IsPart>g__IsPartInternal|40_0(sketchThing, ref CS$<>8__locals1))
			{
				return true;
			}
			if (list != null)
			{
				for (int i = 0; i < list.Count; i++)
				{
					if (this.<IsPart>g__IsPartInternal|40_0(list[i], ref CS$<>8__locals1))
					{
						return true;
					}
				}
			}
			if (CS$<>8__locals1.thing.def.entityDefToBuild != null)
			{
				SketchTerrain sketchTerrain = this.sketch.SketchTerrainAt(CS$<>8__locals1.thing.Position - base.Position);
				if (sketchTerrain != null && this.<IsPart>g__IsPartInternal|40_0(sketchTerrain, ref CS$<>8__locals1))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x0600B094 RID: 45204 RVA: 0x003E3754 File Offset: 0x003E1954
		public bool AllowsPlacingBlueprint(BuildableDef buildable, IntVec3 pos, Rot4 rot, ThingDef stuff)
		{
			MonumentMarker.<>c__DisplayClass41_0 CS$<>8__locals1;
			CS$<>8__locals1.buildable = buildable;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.stuff = stuff;
			CS$<>8__locals1.rot = rot;
			if (!base.Spawned)
			{
				return true;
			}
			CS$<>8__locals1.newRect = GenAdj.OccupiedRect(pos, CS$<>8__locals1.rot, CS$<>8__locals1.buildable.Size);
			if (!this.sketch.OccupiedRect.MovedBy(base.Position).Overlaps(CS$<>8__locals1.newRect))
			{
				return true;
			}
			CS$<>8__locals1.collided = false;
			foreach (IntVec3 a in CS$<>8__locals1.newRect)
			{
				SketchThing sketchThing;
				List<SketchThing> list;
				this.sketch.ThingsAt(a - base.Position, out sketchThing, out list);
				if (sketchThing != null && this.<AllowsPlacingBlueprint>g__CheckEntity|41_1(sketchThing, ref CS$<>8__locals1))
				{
					return true;
				}
				if (list != null)
				{
					for (int i = 0; i < list.Count; i++)
					{
						if (this.<AllowsPlacingBlueprint>g__CheckEntity|41_1(list[i], ref CS$<>8__locals1))
						{
							return true;
						}
					}
				}
				SketchTerrain sketchTerrain = this.sketch.SketchTerrainAt(a - base.Position);
				if (sketchTerrain != null && this.<AllowsPlacingBlueprint>g__CheckEntity|41_1(sketchTerrain, ref CS$<>8__locals1))
				{
					return true;
				}
			}
			return !CS$<>8__locals1.collided;
		}

		// Token: 0x0600B095 RID: 45205 RVA: 0x003E38C4 File Offset: 0x003E1AC4
		public IEnumerable<ThingDef> AllowedStuffs()
		{
			MonumentMarker.tmpStuffCategories.Clear();
			bool flag = true;
			List<SketchThing> things = this.sketch.Things;
			for (int i = 0; i < things.Count; i++)
			{
				if (things[i].def.MadeFromStuff && things[i].stuff == null)
				{
					if (flag)
					{
						flag = false;
						MonumentMarker.tmpStuffCategories.AddRange(things[i].def.stuffCategories);
					}
					else
					{
						bool flag2 = false;
						for (int j = 0; j < things[i].def.stuffCategories.Count; j++)
						{
							if (MonumentMarker.tmpStuffCategories.Contains(things[i].def.stuffCategories[j]))
							{
								flag2 = true;
								break;
							}
						}
						if (flag2)
						{
							for (int k = MonumentMarker.tmpStuffCategories.Count - 1; k >= 0; k--)
							{
								if (!things[i].def.stuffCategories.Contains(MonumentMarker.tmpStuffCategories[k]))
								{
									MonumentMarker.tmpStuffCategories.RemoveAt(k);
								}
							}
						}
					}
				}
			}
			return GenStuff.AllowedStuffs(MonumentMarker.tmpStuffCategories, TechLevel.Undefined);
		}

		// Token: 0x0600B096 RID: 45206 RVA: 0x003E39F4 File Offset: 0x003E1BF4
		public List<ThingDef> AllowedStuffsFor(SketchBuildable buildable)
		{
			if (buildable.Buildable.MadeFromStuff && buildable.Stuff == null)
			{
				return GenStuff.AllowedStuffs(buildable.Buildable.stuffCategories, TechLevel.Undefined).ToList<ThingDef>();
			}
			SketchTerrain sketchTerrain = buildable as SketchTerrain;
			if (sketchTerrain != null)
			{
				List<ThingDef> list = new List<ThingDef>();
				foreach (TerrainDef terrainDef in DefDatabase<TerrainDef>.AllDefs)
				{
					if (terrainDef.BuildableByPlayer && sketchTerrain.IsSameOrSimilar(terrainDef) && !terrainDef.CostList.NullOrEmpty<ThingDefCountClass>())
					{
						list.Add(terrainDef.CostList.First<ThingDefCountClass>().thingDef);
					}
				}
				return list;
			}
			return null;
		}

		// Token: 0x0600B0A1 RID: 45217 RVA: 0x003E3C24 File Offset: 0x003E1E24
		[CompilerGenerated]
		private bool <IsPart>g__IsPartInternal|40_0(SketchBuildable b, ref MonumentMarker.<>c__DisplayClass40_0 A_2)
		{
			BuildableDef buildable = b.Buildable;
			if (A_2.thing.def != buildable && A_2.thing.def.entityDefToBuild != buildable)
			{
				return false;
			}
			if (b.GetSpawnedBlueprintOrFrame(b.pos + base.Position, base.Map) == A_2.thing)
			{
				return true;
			}
			SketchThing sketchThing = b as SketchThing;
			return sketchThing != null && sketchThing.GetSameSpawned(sketchThing.pos + base.Position, base.Map) == A_2.thing;
		}

		// Token: 0x0600B0A2 RID: 45218 RVA: 0x003E3CB4 File Offset: 0x003E1EB4
		[CompilerGenerated]
		private bool <AllowsPlacingBlueprint>g__IsSameOrSimilar|41_0(SketchBuildable entity, ref MonumentMarker.<>c__DisplayClass41_0 A_2)
		{
			if (A_2.buildable != entity.Buildable)
			{
				SketchTerrain sketchTerrain = entity as SketchTerrain;
				if (sketchTerrain == null || !sketchTerrain.IsSameOrSimilar(A_2.buildable))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600B0A3 RID: 45219 RVA: 0x003E3CEC File Offset: 0x003E1EEC
		[CompilerGenerated]
		private bool <AllowsPlacingBlueprint>g__CheckEntity|41_1(SketchBuildable entity, ref MonumentMarker.<>c__DisplayClass41_0 A_2)
		{
			if (entity.IsSameSpawned(entity.pos + base.Position, base.Map))
			{
				return false;
			}
			if (entity.OccupiedRect.MovedBy(base.Position).Overlaps(A_2.newRect))
			{
				A_2.collided = true;
			}
			SketchThing sketchThing = entity as SketchThing;
			return entity.OccupiedRect.MovedBy(base.Position).Equals(A_2.newRect) && this.<AllowsPlacingBlueprint>g__IsSameOrSimilar|41_0(entity, ref A_2) && (A_2.stuff == null || entity.Stuff == null || A_2.stuff == entity.Stuff) && (sketchThing == null || sketchThing.rot == A_2.rot || sketchThing.rot == A_2.rot.Opposite || !sketchThing.def.rotatable);
		}

		// Token: 0x040065DE RID: 26078
		public Sketch sketch = new Sketch();

		// Token: 0x040065DF RID: 26079
		public int ticksSinceDisallowedBuilding;

		// Token: 0x040065E0 RID: 26080
		public bool complete;

		// Token: 0x040065E1 RID: 26081
		private static readonly Texture2D PlaceBlueprintsCommandTex = ContentFinder<Texture2D>.Get("UI/Commands/PlaceBlueprints", true);

		// Token: 0x040065E2 RID: 26082
		private static readonly Texture2D CancelCommandTex = ContentFinder<Texture2D>.Get("UI/Designators/Cancel", true);

		// Token: 0x040065E3 RID: 26083
		public const int DestroyAfterTicksSinceDisallowedBuilding = 60000;

		// Token: 0x040065E4 RID: 26084
		private const int MonumentCompletedCheckIntervalTicks = 177;

		// Token: 0x040065E5 RID: 26085
		private static List<ThingDef> tmpAllowedBuildings = new List<ThingDef>();

		// Token: 0x040065E6 RID: 26086
		private static HashSet<Pair<BuildableDef, ThingDef>> tmpUniqueBuildableDefs = new HashSet<Pair<BuildableDef, ThingDef>>();

		// Token: 0x040065E7 RID: 26087
		private static List<SketchBuildable> tmpBuildables = new List<SketchBuildable>();

		// Token: 0x040065E8 RID: 26088
		private static Dictionary<string, Pair<int, int>> tmpBuiltParts = new Dictionary<string, Pair<int, int>>();

		// Token: 0x040065E9 RID: 26089
		private static List<StuffCategoryDef> tmpStuffCategories = new List<StuffCategoryDef>();
	}
}
