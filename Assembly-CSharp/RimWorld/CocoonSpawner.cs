using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AEA RID: 6890
	public class CocoonSpawner : TunnelHiveSpawner
	{
		// Token: 0x0600B001 RID: 45057 RVA: 0x003E068C File Offset: 0x003DE88C
		protected override void Spawn(Map map, IntVec3 loc)
		{
			Thing thing = GenSpawn.Spawn(ThingMaker.MakeThing(this.cocoon, null), loc, map, WipeMode.Vanish);
			thing.SetFaction(Faction.OfInsects, null);
			thing.questTags = this.questTags;
			if (this.groupID >= 0)
			{
				CompWakeUpDormant compWakeUpDormant = thing.TryGetComp<CompWakeUpDormant>();
				if (compWakeUpDormant != null)
				{
					compWakeUpDormant.groupID = this.groupID;
				}
			}
		}

		// Token: 0x0600B002 RID: 45058 RVA: 0x003E06E5 File Offset: 0x003DE8E5
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.cocoon, "cocoon");
			Scribe_Values.Look<int>(ref this.groupID, "groupID", -1, false);
		}

		// Token: 0x04006596 RID: 26006
		public ThingDef cocoon;

		// Token: 0x04006597 RID: 26007
		public int groupID = -1;
	}
}
