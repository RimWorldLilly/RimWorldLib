using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020012F4 RID: 4852
	public static class ManhunterPackGenStepUtility
	{
		// Token: 0x06007864 RID: 30820 RVA: 0x002A91C9 File Offset: 0x002A73C9
		public static bool TryGetAnimalsKind(float points, PlanetTile tile, out PawnKindDef animalKind)
		{
			return AggressiveAnimalIncidentUtility.TryFindAggressiveAnimalKind(points, tile, out animalKind) || AggressiveAnimalIncidentUtility.TryFindAggressiveAnimalKind(points, PlanetTile.Invalid, out animalKind);
		}
	}
}
