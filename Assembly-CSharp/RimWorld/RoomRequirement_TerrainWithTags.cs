using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02002003 RID: 8195
	public class RoomRequirement_TerrainWithTags : RoomRequirement
	{
		// Token: 0x17002564 RID: 9572
		// (get) Token: 0x0600CFA0 RID: 53152 RVA: 0x004C68DC File Offset: 0x004C4ADC
		private HashSet<string> TagsSet
		{
			get
			{
				HashSet<string> result;
				if ((result = this.tagsSet) == null)
				{
					result = (this.tagsSet = new HashSet<string>(this.tags));
				}
				return result;
			}
		}

		// Token: 0x0600CFA1 RID: 53153 RVA: 0x004C6908 File Offset: 0x004C4B08
		public override bool Met(Room r, Pawn p = null)
		{
			Map map = r.Map;
			foreach (IntVec3 c in r.Cells)
			{
				List<string> list = c.GetTerrain(map).tags;
				if (list.NullOrEmpty<string>())
				{
					return false;
				}
				bool flag = false;
				foreach (string item in list)
				{
					if (this.TagsSet.Contains(item))
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600CFA2 RID: 53154 RVA: 0x004C69C8 File Offset: 0x004C4BC8
		public override bool SameOrSubsetOf(RoomRequirement other)
		{
			if (!base.SameOrSubsetOf(other))
			{
				return false;
			}
			RoomRequirement_TerrainWithTags roomRequirement_TerrainWithTags = (RoomRequirement_TerrainWithTags)other;
			foreach (string item in this.tags)
			{
				if (!roomRequirement_TerrainWithTags.tags.Contains(item))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x0600CFA3 RID: 53155 RVA: 0x004C6A3C File Offset: 0x004C4C3C
		public override IEnumerable<string> ConfigErrors()
		{
			RoomRequirement_TerrainWithTags.<ConfigErrors>d__6 <ConfigErrors>d__ = new RoomRequirement_TerrainWithTags.<ConfigErrors>d__6(-2);
			<ConfigErrors>d__.<>4__this = this;
			return <ConfigErrors>d__;
		}

		// Token: 0x0600CFA4 RID: 53156 RVA: 0x004C6A4C File Offset: 0x004C4C4C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<string>(ref this.tags, "tags", LookMode.Undefined, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.tagsSet = new HashSet<string>(this.tags);
			}
		}

		// Token: 0x0400841C RID: 33820
		public List<string> tags;

		// Token: 0x0400841D RID: 33821
		private HashSet<string> tagsSet;
	}
}
