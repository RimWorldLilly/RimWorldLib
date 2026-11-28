using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020017CC RID: 6092
	public class MetalHellTransition : MusicTransition
	{
		// Token: 0x06009813 RID: 38931 RVA: 0x003724DC File Offset: 0x003706DC
		public override bool IsTransitionSatisfied()
		{
			if (!ModsConfig.AnomalyActive)
			{
				return false;
			}
			if (!base.IsTransitionSatisfied())
			{
				return false;
			}
			foreach (PocketMapParent pocketMapParent in Find.World.pocketMaps)
			{
				if (pocketMapParent.Map != null && MetalHellTransition.IsValidPocketMap(pocketMapParent))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06009814 RID: 38932 RVA: 0x00372558 File Offset: 0x00370758
		private static bool IsValidPocketMap(PocketMapParent pocketMap)
		{
			return pocketMap.Map.generatorDef == MapGeneratorDefOf.MetalHell;
		}
	}
}
