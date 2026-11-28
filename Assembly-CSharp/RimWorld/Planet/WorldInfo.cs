using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002356 RID: 9046
	public class WorldInfo : IExposable
	{
		// Token: 0x17002766 RID: 10086
		// (get) Token: 0x0600E03F RID: 57407 RVA: 0x00536919 File Offset: 0x00534B19
		public string FileNameNoExtension
		{
			get
			{
				return GenText.CapitalizedNoSpaces(this.name);
			}
		}

		// Token: 0x17002767 RID: 10087
		// (get) Token: 0x0600E040 RID: 57408 RVA: 0x00536926 File Offset: 0x00534B26
		public int Seed
		{
			get
			{
				return GenText.StableStringHash(this.seedString);
			}
		}

		// Token: 0x0600E041 RID: 57409 RVA: 0x00536934 File Offset: 0x00534B34
		public void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<float>(ref this.planetCoverage, "planetCoverage", 0f, false);
			Scribe_Values.Look<string>(ref this.seedString, "seedString", null, false);
			Scribe_Values.Look<int>(ref this.persistentRandomValue, "persistentRandomValue", 0, false);
			Scribe_Values.Look<OverallRainfall>(ref this.overallRainfall, "overallRainfall", OverallRainfall.AlmostNone, false);
			Scribe_Values.Look<OverallTemperature>(ref this.overallTemperature, "overallTemperature", OverallTemperature.VeryCold, false);
			Scribe_Values.Look<IntVec3>(ref this.initialMapSize, "initialMapSize", default(IntVec3), false);
			Scribe_Values.Look<float>(ref this.pollution, "pollution", 0f, false);
			Scribe_Collections.Look<FactionDef>(ref this.factions, "factions", LookMode.Def, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				Dictionary<FactionDef, int> dictionary = null;
				Scribe_Collections.Look<FactionDef, int>(ref dictionary, "factionCounts", LookMode.Def, LookMode.Value);
				if (dictionary != null)
				{
					this.factions = new List<FactionDef>();
					foreach (KeyValuePair<FactionDef, int> keyValuePair in dictionary)
					{
						if (keyValuePair.Value > 0)
						{
							for (int i = 0; i < keyValuePair.Value; i++)
							{
								this.factions.Add(keyValuePair.Key);
							}
						}
					}
				}
			}
			BackCompatibility.PostExposeData(this);
		}

		// Token: 0x04008FDA RID: 36826
		public string name = "DefaultWorldName";

		// Token: 0x04008FDB RID: 36827
		public float planetCoverage;

		// Token: 0x04008FDC RID: 36828
		public string seedString = "SeedError";

		// Token: 0x04008FDD RID: 36829
		public int persistentRandomValue = Rand.Int;

		// Token: 0x04008FDE RID: 36830
		public OverallRainfall overallRainfall = OverallRainfall.Normal;

		// Token: 0x04008FDF RID: 36831
		public OverallTemperature overallTemperature = OverallTemperature.Normal;

		// Token: 0x04008FE0 RID: 36832
		public OverallPopulation overallPopulation = OverallPopulation.Normal;

		// Token: 0x04008FE1 RID: 36833
		public LandmarkDensity landmarkDensity = LandmarkDensity.Normal;

		// Token: 0x04008FE2 RID: 36834
		public IntVec3 initialMapSize = new IntVec3(250, 1, 250);

		// Token: 0x04008FE3 RID: 36835
		public List<FactionDef> factions;

		// Token: 0x04008FE4 RID: 36836
		public float pollution;
	}
}
