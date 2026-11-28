using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001A82 RID: 6786
	public interface INotifyHauledTo
	{
		// Token: 0x0600AB36 RID: 43830
		void Notify_HauledTo(Pawn hauler, Thing thing, int count);
	}
}
