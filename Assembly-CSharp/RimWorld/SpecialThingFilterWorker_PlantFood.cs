using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001F51 RID: 8017
	public class SpecialThingFilterWorker_PlantFood : SpecialThingFilterWorker
	{
		// Token: 0x0600CC79 RID: 52345 RVA: 0x004B5C8F File Offset: 0x004B3E8F
		public override bool Matches(Thing t)
		{
			return this.AlwaysMatches(t.def);
		}

		// Token: 0x0600CC7A RID: 52346 RVA: 0x004B5C9D File Offset: 0x004B3E9D
		public override bool AlwaysMatches(ThingDef def)
		{
			return def.ingestible != null && (def.ingestible.foodType & FoodTypeFlags.Plant) > FoodTypeFlags.None;
		}

		// Token: 0x0600CC7B RID: 52347 RVA: 0x004B5CBA File Offset: 0x004B3EBA
		public override bool CanEverMatch(ThingDef def)
		{
			return this.AlwaysMatches(def);
		}
	}
}
