using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F99 RID: 3993
	public class PrefabDef : Def
	{
		// Token: 0x060065CE RID: 26062 RVA: 0x002439A5 File Offset: 0x00241BA5
		[return: TupleElementNames(new string[]
		{
			"data",
			"cell"
		})]
		public IEnumerable<ValueTuple<PrefabThingData, IntVec3>> GetThings()
		{
			PrefabDef.<GetThings>d__6 <GetThings>d__ = new PrefabDef.<GetThings>d__6(-2);
			<GetThings>d__.<>4__this = this;
			return <GetThings>d__;
		}

		// Token: 0x060065CF RID: 26063 RVA: 0x002439B5 File Offset: 0x00241BB5
		[return: TupleElementNames(new string[]
		{
			"data",
			"cell"
		})]
		public IEnumerable<ValueTuple<SubPrefabData, IntVec3>> GetPrefabs()
		{
			PrefabDef.<GetPrefabs>d__7 <GetPrefabs>d__ = new PrefabDef.<GetPrefabs>d__7(-2);
			<GetPrefabs>d__.<>4__this = this;
			return <GetPrefabs>d__;
		}

		// Token: 0x060065D0 RID: 26064 RVA: 0x002439C5 File Offset: 0x00241BC5
		[return: TupleElementNames(new string[]
		{
			"data",
			"cell"
		})]
		public IEnumerable<ValueTuple<PrefabTerrainData, IntVec3>> GetTerrain()
		{
			PrefabDef.<GetTerrain>d__8 <GetTerrain>d__ = new PrefabDef.<GetTerrain>d__8(-2);
			<GetTerrain>d__.<>4__this = this;
			return <GetTerrain>d__;
		}

		// Token: 0x04003CF6 RID: 15606
		internal List<PrefabThingData> things = new List<PrefabThingData>();

		// Token: 0x04003CF7 RID: 15607
		internal List<SubPrefabData> prefabs = new List<SubPrefabData>();

		// Token: 0x04003CF8 RID: 15608
		internal List<PrefabTerrainData> terrain = new List<PrefabTerrainData>();

		// Token: 0x04003CF9 RID: 15609
		public IntVec2 size;

		// Token: 0x04003CFA RID: 15610
		public RotEnum rotations = RotEnum.All;

		// Token: 0x04003CFB RID: 15611
		public bool edgeOnly;
	}
}
