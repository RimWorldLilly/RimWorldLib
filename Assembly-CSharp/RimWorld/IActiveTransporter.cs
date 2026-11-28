using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B4B RID: 6987
	public interface IActiveTransporter : IThingHolder
	{
		// Token: 0x17002071 RID: 8305
		// (get) Token: 0x0600B35E RID: 45918
		ActiveTransporterInfo Contents { get; }
	}
}
