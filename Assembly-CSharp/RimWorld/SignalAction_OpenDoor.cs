using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001B0A RID: 6922
	public class SignalAction_OpenDoor : SignalAction
	{
		// Token: 0x0600B0F2 RID: 45298 RVA: 0x003E56F8 File Offset: 0x003E38F8
		protected override void DoAction(SignalArgs args)
		{
			if (this.door != null)
			{
				Pawn opener;
				if (args.TryGetArg<Pawn>("SUBJECT", out opener))
				{
					this.door.StartManualOpenBy(opener);
					return;
				}
				this.door.StartManualOpenBy(null);
			}
		}

		// Token: 0x0600B0F3 RID: 45299 RVA: 0x003E5736 File Offset: 0x003E3936
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Building_Door>(ref this.door, "door", false);
		}

		// Token: 0x0400662C RID: 26156
		public Building_Door door;
	}
}
