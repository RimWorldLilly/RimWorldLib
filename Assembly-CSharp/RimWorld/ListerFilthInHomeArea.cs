using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x0200127F RID: 4735
	public class ListerFilthInHomeArea
	{
		// Token: 0x170013DE RID: 5086
		// (get) Token: 0x06007546 RID: 30022 RVA: 0x00292B74 File Offset: 0x00290D74
		public List<Thing> FilthInHomeArea
		{
			get
			{
				return this.filthInHomeArea;
			}
		}

		// Token: 0x06007547 RID: 30023 RVA: 0x00292B7C File Offset: 0x00290D7C
		public ListerFilthInHomeArea(Map map)
		{
			this.map = map;
		}

		// Token: 0x06007548 RID: 30024 RVA: 0x00292B98 File Offset: 0x00290D98
		public void RebuildAll()
		{
			this.filthInHomeArea.Clear();
			foreach (IntVec3 c in this.map.AllCells)
			{
				this.Notify_HomeAreaChanged(c);
			}
		}

		// Token: 0x06007549 RID: 30025 RVA: 0x00292BF8 File Offset: 0x00290DF8
		public void Notify_FilthSpawned(Filth f)
		{
			if (this.map.areaManager.Home[f.Position])
			{
				this.filthInHomeArea.Add(f);
			}
		}

		// Token: 0x0600754A RID: 30026 RVA: 0x00292C24 File Offset: 0x00290E24
		public void Notify_FilthDespawned(Filth f)
		{
			for (int i = 0; i < this.filthInHomeArea.Count; i++)
			{
				if (this.filthInHomeArea[i] == f)
				{
					this.filthInHomeArea.RemoveAt(i);
					return;
				}
			}
		}

		// Token: 0x0600754B RID: 30027 RVA: 0x00292C64 File Offset: 0x00290E64
		public void Notify_HomeAreaChanged(IntVec3 c)
		{
			if (this.map.areaManager.Home[c])
			{
				List<Thing> thingList = c.GetThingList(this.map);
				for (int i = 0; i < thingList.Count; i++)
				{
					Filth filth = thingList[i] as Filth;
					if (filth != null)
					{
						this.filthInHomeArea.Add(filth);
					}
				}
				return;
			}
			for (int j = this.filthInHomeArea.Count - 1; j >= 0; j--)
			{
				if (this.filthInHomeArea[j].Position == c)
				{
					this.filthInHomeArea.RemoveAt(j);
				}
			}
		}

		// Token: 0x0600754C RID: 30028 RVA: 0x00292D04 File Offset: 0x00290F04
		internal string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine("======= Filth in home area");
			foreach (Thing thing in this.filthInHomeArea)
			{
				stringBuilder.AppendLine(thing.ThingID + " " + thing.Position.ToString());
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0400497A RID: 18810
		private Map map;

		// Token: 0x0400497B RID: 18811
		private List<Thing> filthInHomeArea = new List<Thing>();
	}
}
