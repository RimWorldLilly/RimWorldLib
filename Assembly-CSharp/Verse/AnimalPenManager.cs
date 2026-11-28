using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld;

namespace Verse
{
	// Token: 0x02000233 RID: 563
	public class AnimalPenManager
	{
		// Token: 0x0600105F RID: 4191 RVA: 0x0005DE54 File Offset: 0x0005C054
		public AnimalPenManager(Map map)
		{
			this.map = map;
			map.events.BuildingSpawned += this.Notify_BuildingSpawned;
			map.events.BuildingDespawned += this.Notify_BuildingDespawned;
		}

		// Token: 0x06001060 RID: 4192 RVA: 0x0005DEC4 File Offset: 0x0005C0C4
		public void RebuildAllPens()
		{
			this.ForceRebuildPens();
		}

		// Token: 0x06001061 RID: 4193 RVA: 0x0005DECC File Offset: 0x0005C0CC
		public PenMarkerState GetPenMarkerState(CompAnimalPenMarker marker)
		{
			this.RebuildIfDirty();
			return this.penMarkers[marker];
		}

		// Token: 0x06001062 RID: 4194 RVA: 0x0005DEE0 File Offset: 0x0005C0E0
		public CompAnimalPenMarker GetPenNamed(string name)
		{
			return this.penMarkers.Keys.FirstOrDefault((CompAnimalPenMarker marker) => marker.label == name);
		}

		// Token: 0x06001063 RID: 4195 RVA: 0x0005DF18 File Offset: 0x0005C118
		public ThingFilter GetFixedAutoCutFilter()
		{
			if (this.cachedFixedAutoCutFilter == null)
			{
				this.cachedFixedAutoCutFilter = new ThingFilter();
				foreach (ThingDef thingDef in this.map.wildPlantSpawner.AllWildPlants)
				{
					if (thingDef.plant.allowAutoCut)
					{
						this.cachedFixedAutoCutFilter.SetAllow(thingDef, true);
					}
				}
				this.cachedFixedAutoCutFilter.SetAllow(ThingCategoryDefOf.Stumps, true, null, null);
			}
			return this.cachedFixedAutoCutFilter;
		}

		// Token: 0x06001064 RID: 4196 RVA: 0x0005DFB4 File Offset: 0x0005C1B4
		public ThingFilter GetDefaultAutoCutFilter()
		{
			if (this.cachedDefaultAutoCutFilter == null)
			{
				this.cachedDefaultAutoCutFilter = new ThingFilter();
				ThingDef plant_Grass = ThingDefOf.Plant_Grass;
				float num = plant_Grass.ingestible.CachedNutrition / plant_Grass.plant.growDays * 0.5f - 0.0001f;
				foreach (ThingDef thingDef in this.GetFixedAutoCutFilter().AllowedThingDefs)
				{
					if (!MapPlantGrowthRateCalculator.IsEdibleByPastureAnimals(thingDef) || (thingDef.ingestible.CachedNutrition / thingDef.plant.growDays < num && (thingDef.plant.harvestedThingDef == null || !thingDef.plant.harvestedThingDef.IsIngestible)))
					{
						this.cachedDefaultAutoCutFilter.SetAllow(thingDef, true);
					}
				}
			}
			return this.cachedDefaultAutoCutFilter;
		}

		// Token: 0x06001065 RID: 4197 RVA: 0x0005E094 File Offset: 0x0005C294
		private void Notify_BuildingSpawned(Building building)
		{
			this.dirty = true;
		}

		// Token: 0x06001066 RID: 4198 RVA: 0x0005E09D File Offset: 0x0005C29D
		private void Notify_BuildingDespawned(Building building)
		{
			this.dirty = true;
		}

		// Token: 0x06001067 RID: 4199 RVA: 0x0005E0A6 File Offset: 0x0005C2A6
		private void RebuildIfDirty()
		{
			if (this.dirty)
			{
				this.ForceRebuildPens();
			}
		}

		// Token: 0x06001068 RID: 4200 RVA: 0x0005E0B8 File Offset: 0x0005C2B8
		private void ForceRebuildPens()
		{
			this.dirty = false;
			this.penMarkers.Clear();
			foreach (Building thing in this.map.listerBuildings.allBuildingsAnimalPenMarkers)
			{
				CompAnimalPenMarker compAnimalPenMarker = thing.TryGetComp<CompAnimalPenMarker>();
				this.penMarkers.Add(compAnimalPenMarker, new PenMarkerState(compAnimalPenMarker));
			}
		}

		// Token: 0x06001069 RID: 4201 RVA: 0x0005E138 File Offset: 0x0005C338
		public string MakeNewAnimalPenName()
		{
			this.existingPenNames.Clear();
			this.existingPenNames.AddRange(from marker in this.penMarkers.Keys
			select marker.label);
			int num = 1;
			string text;
			for (;;)
			{
				text = "AnimalPenMarkerDefaultLabel".Translate(num);
				if (!this.existingPenNames.Contains(text))
				{
					break;
				}
				num++;
			}
			this.existingPenNames.Clear();
			return text;
		}

		// Token: 0x0600106A RID: 4202 RVA: 0x0005E1C4 File Offset: 0x0005C3C4
		public void DrawPlacingMouseAttachments(BuildableDef def)
		{
			ThingDef thingDef = def as ThingDef;
			if (((thingDef != null) ? thingDef.CompDefFor<CompAnimalPenMarker>() : null) == null)
			{
				return;
			}
			IntVec3 intVec = UI.MouseCell();
			if (!intVec.InBounds(this.map))
			{
				return;
			}
			if (this.cached_placingPenFoodCalculator_forPosition == null || intVec != this.cached_placingPenFoodCalculator_forPosition)
			{
				this.cached_placingPenFoodCalculator.ResetAndProcessPen(intVec, this.map, true);
				this.cached_placingPenFoodCalculator_forPosition = new IntVec3?(intVec);
			}
			AnimalPenGUI.DrawPlacingMouseAttachments(intVec, this.map, this.cached_placingPenFoodCalculator);
		}

		// Token: 0x04000F4A RID: 3914
		private readonly Map map;

		// Token: 0x04000F4B RID: 3915
		private readonly Dictionary<CompAnimalPenMarker, PenMarkerState> penMarkers = new Dictionary<CompAnimalPenMarker, PenMarkerState>();

		// Token: 0x04000F4C RID: 3916
		private bool dirty = true;

		// Token: 0x04000F4D RID: 3917
		private ThingFilter cachedDefaultAutoCutFilter;

		// Token: 0x04000F4E RID: 3918
		private ThingFilter cachedFixedAutoCutFilter;

		// Token: 0x04000F4F RID: 3919
		private HashSet<string> existingPenNames = new HashSet<string>();

		// Token: 0x04000F50 RID: 3920
		private readonly PenFoodCalculator cached_placingPenFoodCalculator = new PenFoodCalculator();

		// Token: 0x04000F51 RID: 3921
		private IntVec3? cached_placingPenFoodCalculator_forPosition;
	}
}
