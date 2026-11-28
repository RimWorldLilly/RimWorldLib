using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld.QuestGen
{
	// Token: 0x0200229F RID: 8863
	public class QuestPart_GiveMemoryToHumansOnMap : QuestPart
	{
		// Token: 0x0600DAD7 RID: 56023 RVA: 0x00517C6C File Offset: 0x00515E6C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			if (signal.tag == this.inSignal)
			{
				MapParent mapParent = this.mapParent;
				if (((mapParent != null) ? mapParent.Map : null) != null)
				{
					foreach (Pawn pawn in this.mapParent.Map.mapPawns.AllPawnsSpawned)
					{
						Pawn_NeedsTracker needs = pawn.needs;
						if (needs != null)
						{
							Need_Mood mood = needs.mood;
							if (mood != null)
							{
								ThoughtHandler thoughts = mood.thoughts;
								if (thoughts != null)
								{
									MemoryThoughtHandler memories = thoughts.memories;
									if (memories != null)
									{
										memories.TryGainMemory(this.memory, null, null);
									}
								}
							}
						}
					}
				}
			}
		}

		// Token: 0x0600DAD8 RID: 56024 RVA: 0x00517D24 File Offset: 0x00515F24
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThoughtDef>(ref this.memory, "memory");
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
		}

		// Token: 0x04008C0B RID: 35851
		public ThoughtDef memory;

		// Token: 0x04008C0C RID: 35852
		public string inSignal;

		// Token: 0x04008C0D RID: 35853
		public MapParent mapParent;
	}
}
