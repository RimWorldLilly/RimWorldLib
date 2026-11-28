using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200130E RID: 4878
	public class TileMutatorWorker_AncientRuins_Frozen : TileMutatorWorker_AncientRuins
	{
		// Token: 0x06007923 RID: 31011 RVA: 0x002B07EA File Offset: 0x002AE9EA
		public TileMutatorWorker_AncientRuins_Frozen(TileMutatorDef def) : base(def)
		{
		}

		// Token: 0x06007924 RID: 31012 RVA: 0x002B07F4 File Offset: 0x002AE9F4
		public override void GenerateNonCriticalStructures(Map map)
		{
			base.GenerateNonCriticalStructures(map);
			GenMorphology.GenPatchParms value = GenMorphology.GenPatchParms.For(ThingDefOf.SolidIce, TerrainDefOf.Ice);
			value.edificeThreshold = 0.75f;
			value.roofThreshold = 0.8f;
			GenStep_FrozenRuins.EncaseStructuresInIce(map, TileMutatorWorker_AncientRuins.GenStep.structureSketches, new GenMorphology.GenPatchParms?(value));
		}
	}
}
