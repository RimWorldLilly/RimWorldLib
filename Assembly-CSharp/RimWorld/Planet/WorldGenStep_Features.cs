using System;
using System.Linq;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200234C RID: 9036
	public class WorldGenStep_Features : WorldGenStep
	{
		// Token: 0x1700274B RID: 10059
		// (get) Token: 0x0600DFB8 RID: 57272 RVA: 0x005331B4 File Offset: 0x005313B4
		public override int SeedPart
		{
			get
			{
				return 26611132;
			}
		}

		// Token: 0x0600DFB9 RID: 57273 RVA: 0x005331BC File Offset: 0x005313BC
		public override void GenerateFresh(string seed, PlanetLayer layer)
		{
			foreach (FeatureDef featureDef in from x in DefDatabase<FeatureDef>.AllDefsListForReading
			orderby x.order, x.index
			select x)
			{
				try
				{
					featureDef.Worker.GenerateWhereAppropriate(layer);
				}
				catch (Exception value)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(43, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Could not generate world features of def ");
					defaultInterpolatedStringHandler.AppendFormatted<FeatureDef>(featureDef);
					defaultInterpolatedStringHandler.AppendLiteral(": ");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
					Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
		}
	}
}
