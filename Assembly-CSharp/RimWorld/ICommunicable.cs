using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AA3 RID: 6819
	public interface ICommunicable
	{
		// Token: 0x0600AC3D RID: 44093
		string GetCallLabel();

		// Token: 0x0600AC3E RID: 44094
		string GetInfoText();

		// Token: 0x0600AC3F RID: 44095
		void TryOpenComms(Pawn negotiator);

		// Token: 0x0600AC40 RID: 44096
		Faction GetFaction();

		// Token: 0x0600AC41 RID: 44097
		FloatMenuOption CommFloatMenuOption(Building_CommsConsole console, Pawn negotiator);
	}
}
