using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DDC RID: 3548
	public class ThoughtWorker_Precept_GrowthVatInColony : ThoughtWorker_Precept
	{
		// Token: 0x06006098 RID: 24728 RVA: 0x0022FD16 File Offset: 0x0022DF16
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (!ModsConfig.IdeologyActive || !ModsConfig.BiotechActive || !p.IsColonist)
			{
				return ThoughtState.Inactive;
			}
			return p.MapHeld.listerBuildings.AllBuildingsColonistOfDef(ThingDefOf.GrowthVat).Any<Building>();
		}
	}
}
