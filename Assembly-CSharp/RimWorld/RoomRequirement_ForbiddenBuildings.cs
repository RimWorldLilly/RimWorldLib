using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200200C RID: 8204
	public class RoomRequirement_ForbiddenBuildings : RoomRequirement
	{
		// Token: 0x17002565 RID: 9573
		// (get) Token: 0x0600CFD7 RID: 53207 RVA: 0x004C71E0 File Offset: 0x004C53E0
		private HashSet<string> BuildingTagsSet
		{
			get
			{
				HashSet<string> result;
				if ((result = this.buildingTagsSet) == null)
				{
					result = (this.buildingTagsSet = new HashSet<string>(this.buildingTags));
				}
				return result;
			}
		}

		// Token: 0x0600CFD8 RID: 53208 RVA: 0x004C720C File Offset: 0x004C540C
		public override bool Met(Room r, Pawn p = null)
		{
			foreach (Region region in r.Regions)
			{
				List<Thing> allThings = region.ListerThings.AllThings;
				for (int i = 0; i < allThings.Count; i++)
				{
					Thing thing = allThings[i];
					if (thing.def.building != null && thing.def.building.buildingTags != null)
					{
						foreach (string item in thing.def.building.buildingTags)
						{
							if (this.BuildingTagsSet.Contains(item))
							{
								return false;
							}
						}
					}
				}
			}
			return true;
		}

		// Token: 0x0600CFD9 RID: 53209 RVA: 0x004C7304 File Offset: 0x004C5504
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.buildingTags, "buildingTags", LookMode.Undefined, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.buildingTagsSet = new HashSet<string>(this.buildingTags);
			}
		}

		// Token: 0x04008424 RID: 33828
		public List<string> buildingTags = new List<string>();

		// Token: 0x04008425 RID: 33829
		private HashSet<string> buildingTagsSet;
	}
}
