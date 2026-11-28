using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02001399 RID: 5017
	public class SketchThing : SketchBuildable
	{
		// Token: 0x1700152B RID: 5419
		// (get) Token: 0x06007C54 RID: 31828 RVA: 0x002C5ACD File Offset: 0x002C3CCD
		public override BuildableDef Buildable
		{
			get
			{
				return this.def;
			}
		}

		// Token: 0x1700152C RID: 5420
		// (get) Token: 0x06007C55 RID: 31829 RVA: 0x002C5AD5 File Offset: 0x002C3CD5
		public override ThingDef Stuff
		{
			get
			{
				return this.stuff;
			}
		}

		// Token: 0x1700152D RID: 5421
		// (get) Token: 0x06007C56 RID: 31830 RVA: 0x002C5ADD File Offset: 0x002C3CDD
		public override string Label
		{
			get
			{
				return GenLabel.ThingLabel(this.def, this.stuff, this.stackCount);
			}
		}

		// Token: 0x1700152E RID: 5422
		// (get) Token: 0x06007C57 RID: 31831 RVA: 0x002C5AF6 File Offset: 0x002C3CF6
		public override string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst();
			}
		}

		// Token: 0x1700152F RID: 5423
		// (get) Token: 0x06007C58 RID: 31832 RVA: 0x002C5B03 File Offset: 0x002C3D03
		public override CellRect OccupiedRect
		{
			get
			{
				return GenAdj.OccupiedRect(this.pos, this.rot, this.def.size);
			}
		}

		// Token: 0x17001530 RID: 5424
		// (get) Token: 0x06007C59 RID: 31833 RVA: 0x002C5B21 File Offset: 0x002C3D21
		public override float SpawnOrder
		{
			get
			{
				return this.spawnOrder;
			}
		}

		// Token: 0x17001531 RID: 5425
		// (get) Token: 0x06007C5A RID: 31834 RVA: 0x002C5B29 File Offset: 0x002C3D29
		public int MaxHitPoints
		{
			get
			{
				return Mathf.RoundToInt(this.def.GetStatValueAbstract(StatDefOf.MaxHitPoints, this.stuff ?? GenStuff.DefaultStuffFor(this.def)));
			}
		}

		// Token: 0x06007C5B RID: 31835 RVA: 0x002C5B58 File Offset: 0x002C3D58
		public Thing Instantiate()
		{
			Thing thing = ThingMaker.MakeThing(this.def, this.stuff ?? GenStuff.DefaultStuffFor(this.def));
			thing.stackCount = this.stackCount;
			if (this.quality != null)
			{
				CompQuality compQuality = thing.TryGetComp<CompQuality>();
				if (compQuality != null)
				{
					compQuality.SetQuality(this.quality.Value, new ArtGenerationContext?(ArtGenerationContext.Outsider));
				}
			}
			if (this.hitPoints != null)
			{
				thing.HitPoints = this.hitPoints.Value;
			}
			return thing;
		}

		// Token: 0x06007C5C RID: 31836 RVA: 0x002C5BE0 File Offset: 0x002C3DE0
		public override void DrawGhost(IntVec3 at, Color color)
		{
			GhostDrawer.DrawGhostThing(at, this.rot, this.def, this.def.graphic, color, AltitudeLayer.Blueprint, null, false, null);
		}

		// Token: 0x06007C5D RID: 31837 RVA: 0x002C5C10 File Offset: 0x002C3E10
		public Thing GetSameSpawned(IntVec3 at, Map map)
		{
			if (!at.InBounds(map))
			{
				return null;
			}
			List<Thing> thingList = at.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				if (this.IsSame(thingList[i]))
				{
					return thingList[i];
				}
			}
			return null;
		}

		// Token: 0x06007C5E RID: 31838 RVA: 0x002C5C5C File Offset: 0x002C3E5C
		public bool IsSame(Thing thing)
		{
			CellRect lhs = GenAdj.OccupiedRect(thing.Position, this.rot, thing.def.Size);
			CellRect lhs2 = GenAdj.OccupiedRect(thing.Position, this.rot.Opposite, thing.def.Size);
			CellRect rhs = thing.OccupiedRect();
			return thing.def == this.def && (this.stuff == null || thing.Stuff == this.stuff) && (thing.Rotation == this.rot || thing.Rotation == this.rot.Opposite || !this.def.rotatable) && (lhs == rhs || lhs2 == rhs);
		}

		// Token: 0x06007C5F RID: 31839 RVA: 0x002C5D1F File Offset: 0x002C3F1F
		public override bool IsSameSpawned(IntVec3 at, Map map)
		{
			return this.GetSameSpawned(at, map) != null;
		}

		// Token: 0x06007C60 RID: 31840 RVA: 0x002C5D2C File Offset: 0x002C3F2C
		public override Thing GetSpawnedBlueprintOrFrame(IntVec3 at, Map map)
		{
			if (!at.InBounds(map))
			{
				return null;
			}
			List<Thing> thingList = at.GetThingList(map);
			for (int i = 0; i < thingList.Count; i++)
			{
				CellRect lhs = GenAdj.OccupiedRect(at, this.rot, thingList[i].def.Size);
				CellRect lhs2 = GenAdj.OccupiedRect(at, this.rot.Opposite, thingList[i].def.Size);
				CellRect rhs = thingList[i].OccupiedRect();
				if ((lhs == rhs || lhs2 == rhs) && thingList[i].def.entityDefToBuild == this.def && (this.stuff == null || ((IConstructible)thingList[i]).EntityToBuildStuff() == this.stuff) && (thingList[i].Rotation == this.rot || thingList[i].Rotation == this.rot.Opposite || !this.def.rotatable))
				{
					return thingList[i];
				}
			}
			return null;
		}

		// Token: 0x06007C61 RID: 31841 RVA: 0x002C5E4C File Offset: 0x002C404C
		public override bool IsSpawningBlocked(IntVec3 at, Map map, Thing thingToIgnore = null, bool wipeIfCollides = false)
		{
			return this.IsSpawningBlockedPermanently(at, map, thingToIgnore, wipeIfCollides) || !GenAdj.OccupiedRect(at, this.rot, this.def.Size).InBounds(map) || !GenConstruct.CanPlaceBlueprintAt(this.def, at, this.rot, map, wipeIfCollides, thingToIgnore, null, this.stuff ?? GenStuff.DefaultStuffFor(this.def), false, false, false).Accepted;
		}

		// Token: 0x06007C62 RID: 31842 RVA: 0x002C5EC8 File Offset: 0x002C40C8
		public override bool IsSpawningBlockedPermanently(IntVec3 at, Map map, Thing thingToIgnore = null, bool wipeIfCollides = false)
		{
			return !at.InBounds(map) || !this.CanBuildOnTerrain(at, map) || base.FirstPermanentBlockerAt(at, map) != null;
		}

		// Token: 0x06007C63 RID: 31843 RVA: 0x002C5EEE File Offset: 0x002C40EE
		public override bool CanBuildOnTerrain(IntVec3 at, Map map)
		{
			return GenConstruct.CanBuildOnTerrain(this.def, at, map, this.rot, null, this.stuff ?? GenStuff.DefaultStuffFor(this.def));
		}

		// Token: 0x06007C64 RID: 31844 RVA: 0x002C5F1C File Offset: 0x002C411C
		public override bool Spawn(IntVec3 at, Map map, Faction faction, Sketch.SpawnMode spawnMode = Sketch.SpawnMode.Normal, bool wipeIfCollides = false, bool forceTerrainAffordance = false, List<Thing> spawnedThings = null, bool dormant = false, TerrainDef defaultAffordanceTerrain = null)
		{
			if (!at.RectAbout(this.def.size, this.rot).InBounds(map))
			{
				return false;
			}
			if (forceTerrainAffordance && !this.CanBuildOnTerrain(at, map))
			{
				base.ForceTerrainAffordance(at, this.rot, map, defaultAffordanceTerrain);
			}
			if (this.IsSpawningBlocked(at, map, null, wipeIfCollides))
			{
				return false;
			}
			switch (spawnMode)
			{
			case Sketch.SpawnMode.Blueprint:
				GenConstruct.PlaceBlueprintForBuild(this.def, at, map, this.rot, faction, this.stuff ?? GenStuff.DefaultStuffFor(this.def), null, null, false);
				break;
			case Sketch.SpawnMode.Normal:
			{
				Thing thing = this.Instantiate();
				if (spawnedThings != null)
				{
					spawnedThings.Add(thing);
				}
				if (faction != null && thing.def.CanHaveFaction)
				{
					thing.SetFactionDirect(faction);
				}
				this.SetDormant(thing, dormant);
				GenSpawn.Spawn(thing, at, map, this.rot, WipeMode.VanishOrMoveAside, false, false);
				break;
			}
			case Sketch.SpawnMode.TransportPod:
			{
				Thing thing2 = this.Instantiate();
				thing2.Position = at;
				thing2.Rotation = this.rot;
				if (spawnedThings != null)
				{
					spawnedThings.Add(thing2);
				}
				if (faction != null)
				{
					thing2.SetFactionDirect(faction);
				}
				this.SetDormant(thing2, dormant);
				ActiveTransporterInfo activeTransporterInfo = new ActiveTransporterInfo();
				activeTransporterInfo.innerContainer.TryAdd(thing2, 1, true);
				activeTransporterInfo.openDelay = 60;
				activeTransporterInfo.leaveSlag = false;
				activeTransporterInfo.despawnPodBeforeSpawningThing = true;
				activeTransporterInfo.spawnWipeMode = (wipeIfCollides ? new WipeMode?(WipeMode.VanishOrMoveAside) : null);
				activeTransporterInfo.moveItemsAsideBeforeSpawning = true;
				activeTransporterInfo.setRotation = new Rot4?(this.rot);
				DropPodUtility.MakeDropPodAt(at, map, activeTransporterInfo, faction);
				break;
			}
			default:
				throw new NotImplementedException("Spawn mode " + spawnMode.ToString() + " not implemented!");
			}
			return true;
		}

		// Token: 0x06007C65 RID: 31845 RVA: 0x002C60D4 File Offset: 0x002C42D4
		private void SetDormant(Thing thing, bool dormant)
		{
			CompCanBeDormant compCanBeDormant = thing.TryGetComp<CompCanBeDormant>();
			if (compCanBeDormant != null)
			{
				if (dormant)
				{
					compCanBeDormant.ToSleep();
					return;
				}
				compCanBeDormant.WakeUp();
			}
		}

		// Token: 0x06007C66 RID: 31846 RVA: 0x002C60FC File Offset: 0x002C42FC
		public override bool SameForSubtracting(SketchEntity other)
		{
			SketchThing sketchThing = other as SketchThing;
			if (sketchThing == null)
			{
				return false;
			}
			if (sketchThing == this)
			{
				return true;
			}
			if (this.def == sketchThing.def && this.stuff == sketchThing.stuff && this.stackCount == sketchThing.stackCount && this.pos == sketchThing.pos && this.rot == sketchThing.rot)
			{
				QualityCategory? qualityCategory = this.quality;
				QualityCategory? qualityCategory2 = sketchThing.quality;
				if (qualityCategory.GetValueOrDefault() == qualityCategory2.GetValueOrDefault() & qualityCategory != null == (qualityCategory2 != null))
				{
					int? num = this.hitPoints;
					int? num2 = sketchThing.hitPoints;
					if (num.GetValueOrDefault() == num2.GetValueOrDefault() & num != null == (num2 != null))
					{
						return this.spawnOrder == sketchThing.spawnOrder;
					}
				}
			}
			return false;
		}

		// Token: 0x06007C67 RID: 31847 RVA: 0x002C61EC File Offset: 0x002C43EC
		public override SketchEntity DeepCopy()
		{
			SketchThing sketchThing = (SketchThing)base.DeepCopy();
			sketchThing.def = this.def;
			sketchThing.stuff = this.stuff;
			sketchThing.stackCount = this.stackCount;
			sketchThing.rot = this.rot;
			sketchThing.quality = this.quality;
			sketchThing.hitPoints = this.hitPoints;
			sketchThing.spawnOrder = this.spawnOrder;
			return sketchThing;
		}

		// Token: 0x06007C68 RID: 31848 RVA: 0x002C6258 File Offset: 0x002C4458
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.def, "def");
			Scribe_Defs.Look<ThingDef>(ref this.stuff, "stuff");
			Scribe_Values.Look<int>(ref this.stackCount, "stackCount", 0, false);
			Scribe_Values.Look<Rot4>(ref this.rot, "rot", default(Rot4), false);
			Scribe_Values.Look<QualityCategory?>(ref this.quality, "quality", null, false);
			Scribe_Values.Look<int?>(ref this.hitPoints, "hitPoints", null, false);
			Scribe_Values.Look<float>(ref this.spawnOrder, "spawnOrder", 0f, false);
		}

		// Token: 0x04004E41 RID: 20033
		public ThingDef def;

		// Token: 0x04004E42 RID: 20034
		public ThingDef stuff;

		// Token: 0x04004E43 RID: 20035
		public int stackCount;

		// Token: 0x04004E44 RID: 20036
		public Rot4 rot;

		// Token: 0x04004E45 RID: 20037
		public QualityCategory? quality;

		// Token: 0x04004E46 RID: 20038
		public int? hitPoints;

		// Token: 0x04004E47 RID: 20039
		public float spawnOrder = 2f;
	}
}
