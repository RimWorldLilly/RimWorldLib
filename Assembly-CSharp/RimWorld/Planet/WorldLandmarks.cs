using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002359 RID: 9049
	public class WorldLandmarks : IExposable
	{
		// Token: 0x17002769 RID: 10089
		public Landmark this[PlanetTile tile]
		{
			get
			{
				if (this.landmarks.ContainsKey(tile))
				{
					return this.landmarks[tile];
				}
				return null;
			}
			set
			{
				this.landmarks[tile] = value;
			}
		}

		// Token: 0x0600E048 RID: 57416 RVA: 0x00536BB1 File Offset: 0x00534DB1
		public void ExposeData()
		{
			Scribe_Collections.Look<PlanetTile, Landmark>(ref this.landmarks, "landmarks", LookMode.Value, LookMode.Deep);
		}

		// Token: 0x0600E049 RID: 57417 RVA: 0x00536BC8 File Offset: 0x00534DC8
		public void AddLandmark(LandmarkDef landmarkDef, PlanetTile tile, PlanetLayer layer = null, bool forced = false)
		{
			if (!ModsConfig.OdysseyActive)
			{
				return;
			}
			if (layer == null)
			{
				layer = tile.Layer;
			}
			Landmark landmark = (Landmark)Activator.CreateInstance(landmarkDef.workerClass, new object[]
			{
				landmarkDef
			});
			if (landmarkDef.nameMaker != null)
			{
				landmark.name = NameGenerator.GenerateName(landmarkDef.nameMaker, null, false, "r_name", null, null);
			}
			else
			{
				landmark.name = landmarkDef.LabelCap;
			}
			this[tile] = landmark;
			foreach (MutatorChance mutatorChance in landmarkDef.mutatorChances)
			{
				if (Rand.Chance(mutatorChance.chance) && ((mutatorChance.required && forced) || mutatorChance.mutator.IsValidTile(tile, layer)))
				{
					layer[tile].AddMutator(mutatorChance.mutator);
				}
			}
			foreach (MutatorChance mutatorChance2 in landmarkDef.comboLandmarkMutators)
			{
				if (Rand.Chance(mutatorChance2.chance) && ((mutatorChance2.required && forced) || mutatorChance2.mutator.IsValidTile(tile, layer)))
				{
					layer[tile].AddMutator(mutatorChance2.mutator);
					landmark.isComboLandmark = true;
					break;
				}
			}
		}

		// Token: 0x0600E04A RID: 57418 RVA: 0x00536D38 File Offset: 0x00534F38
		public void RemoveLandmark(PlanetTile tile)
		{
			if (this.landmarks.ContainsKey(tile))
			{
				this.landmarks.Remove(tile);
			}
		}

		// Token: 0x04008FF5 RID: 36853
		public Dictionary<PlanetTile, Landmark> landmarks = new Dictionary<PlanetTile, Landmark>();
	}
}
