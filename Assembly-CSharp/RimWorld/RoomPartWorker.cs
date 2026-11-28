using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001266 RID: 4710
	public abstract class RoomPartWorker
	{
		// Token: 0x060074C8 RID: 29896 RVA: 0x0028FFAB File Offset: 0x0028E1AB
		public RoomPartWorker(RoomPartDef def)
		{
			this.def = def;
		}

		// Token: 0x170013D1 RID: 5073
		// (get) Token: 0x060074C9 RID: 29897 RVA: 0x0028FFBA File Offset: 0x0028E1BA
		public virtual bool FillOnPost
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060074CA RID: 29898
		public abstract void FillRoom(Map map, LayoutRoom room, Faction faction, float threatPoints);

		// Token: 0x04004950 RID: 18768
		public readonly RoomPartDef def;
	}
}
