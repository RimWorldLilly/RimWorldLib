using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200154D RID: 5453
	public static class PawnNameDatabaseSolid
	{
		// Token: 0x0600853E RID: 34110 RVA: 0x00300A7C File Offset: 0x002FEC7C
		static PawnNameDatabaseSolid()
		{
			foreach (object obj in Enum.GetValues(typeof(GenderPossibility)))
			{
				GenderPossibility key = (GenderPossibility)obj;
				PawnNameDatabaseSolid.solidNames.Add(key, new List<NameTriple>());
			}
		}

		// Token: 0x0600853F RID: 34111 RVA: 0x00300AF4 File Offset: 0x002FECF4
		public static void Clear()
		{
			foreach (object obj in Enum.GetValues(typeof(GenderPossibility)))
			{
				GenderPossibility key = (GenderPossibility)obj;
				PawnNameDatabaseSolid.solidNames[key].Clear();
			}
		}

		// Token: 0x06008540 RID: 34112 RVA: 0x00300B60 File Offset: 0x002FED60
		public static void AddPlayerContentName(NameTriple newName, GenderPossibility genderPos)
		{
			PawnNameDatabaseSolid.solidNames[genderPos].Add(newName);
		}

		// Token: 0x06008541 RID: 34113 RVA: 0x00300B73 File Offset: 0x002FED73
		public static List<NameTriple> GetListForGender(GenderPossibility gp)
		{
			return PawnNameDatabaseSolid.solidNames[gp];
		}

		// Token: 0x06008542 RID: 34114 RVA: 0x00300B80 File Offset: 0x002FED80
		public static IEnumerable<NameTriple> AllNames()
		{
			return new PawnNameDatabaseSolid.<AllNames>d__6(-2);
		}

		// Token: 0x04005204 RID: 20996
		private static Dictionary<GenderPossibility, List<NameTriple>> solidNames = new Dictionary<GenderPossibility, List<NameTriple>>();

		// Token: 0x04005205 RID: 20997
		private const float PreferredNameChance = 0.5f;
	}
}
