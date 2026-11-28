using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200234B RID: 9035
	public class WorldGenStep_Factions : WorldGenStep
	{
		// Token: 0x1700274A RID: 10058
		// (get) Token: 0x0600DFB4 RID: 57268 RVA: 0x0053318C File Offset: 0x0053138C
		public override int SeedPart
		{
			get
			{
				return 777998381;
			}
		}

		// Token: 0x0600DFB5 RID: 57269 RVA: 0x00533193 File Offset: 0x00531393
		public override void GenerateFresh(string seed, PlanetLayer layer)
		{
			FactionGenerator.GenerateFactionsIntoWorldLayer(layer, Current.CreatingWorld.info.factions);
		}

		// Token: 0x0600DFB6 RID: 57270 RVA: 0x005331AA File Offset: 0x005313AA
		public override void GenerateWithoutWorldData(string seed, PlanetLayer layer)
		{
		}
	}
}
