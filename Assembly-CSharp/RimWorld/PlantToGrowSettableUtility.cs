using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020013C6 RID: 5062
	public static class PlantToGrowSettableUtility
	{
		// Token: 0x06007D94 RID: 32148 RVA: 0x002CE707 File Offset: 0x002CC907
		public static Command_SetPlantToGrow SetPlantToGrowCommand(IPlantToGrowSettable settable)
		{
			return new Command_SetPlantToGrow
			{
				defaultDesc = "CommandSelectPlantToGrowDesc".Translate(),
				hotKey = KeyBindingDefOf.Misc12,
				settable = settable
			};
		}
	}
}
