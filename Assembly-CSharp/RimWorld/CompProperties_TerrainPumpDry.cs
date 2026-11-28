using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020019B7 RID: 6583
	public class CompProperties_TerrainPumpDry : CompProperties_TerrainPump
	{
		// Token: 0x0600A544 RID: 42308 RVA: 0x003AF76F File Offset: 0x003AD96F
		public CompProperties_TerrainPumpDry()
		{
			this.compClass = typeof(CompTerrainPumpDry);
		}

		// Token: 0x04006191 RID: 24977
		public SoundDef soundWorking;
	}
}
