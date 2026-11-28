using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020018A6 RID: 6310
	public class CompGiveThoughtToAllMapPawnsOnDestroy : ThingComp
	{
		// Token: 0x17001B10 RID: 6928
		// (get) Token: 0x06009DFF RID: 40447 RVA: 0x0038B9A3 File Offset: 0x00389BA3
		private CompProperties_GiveThoughtToAllMapPawnsOnDestroy Props
		{
			get
			{
				return (CompProperties_GiveThoughtToAllMapPawnsOnDestroy)this.props;
			}
		}

		// Token: 0x06009E00 RID: 40448 RVA: 0x0038B9B0 File Offset: 0x00389BB0
		public override void PostDestroy(DestroyMode mode, Map previousMap)
		{
			if (mode == DestroyMode.Vanish && this.Props.ignoreOnVanish)
			{
				return;
			}
			if (this.Props.onlyWhenKilled && mode != DestroyMode.KillFinalize && mode != DestroyMode.KillFinalizeLeavingsOnly)
			{
				return;
			}
			if (previousMap != null)
			{
				if (!this.Props.message.NullOrEmpty())
				{
					Messages.Message(this.Props.message, new TargetInfo(this.parent.Position, previousMap, false), MessageTypeDefOf.NegativeEvent, true);
				}
				foreach (Pawn pawn in previousMap.mapPawns.AllPawnsSpawned)
				{
					Pawn_NeedsTracker needs = pawn.needs;
					if (needs != null)
					{
						Need_Mood mood = needs.mood;
						if (mood != null)
						{
							mood.thoughts.memories.TryGainMemory(this.Props.thought, null, null);
						}
					}
				}
			}
		}
	}
}
