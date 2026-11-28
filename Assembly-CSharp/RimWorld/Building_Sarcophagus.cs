using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001AC7 RID: 6855
	public class Building_Sarcophagus : Building_Grave
	{
		// Token: 0x0600AE75 RID: 44661 RVA: 0x003D70A0 File Offset: 0x003D52A0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.everNonEmpty, "everNonEmpty", false, false);
		}

		// Token: 0x0600AE76 RID: 44662 RVA: 0x003D70BA File Offset: 0x003D52BA
		public override bool TryAcceptThing(Thing thing, bool allowSpecialEffects = true)
		{
			if (base.TryAcceptThing(thing, allowSpecialEffects))
			{
				this.thisIsFirstBodyEver = !this.everNonEmpty;
				this.everNonEmpty = true;
				return true;
			}
			return false;
		}

		// Token: 0x0600AE77 RID: 44663 RVA: 0x003D70E0 File Offset: 0x003D52E0
		public override void Notify_HauledTo(Pawn hauler, Thing thing, int count)
		{
			base.Notify_HauledTo(hauler, thing, count);
			if (this.thisIsFirstBodyEver && hauler.IsColonist && base.Corpse.InnerPawn.def.race.Humanlike && !base.Corpse.everBuriedInSarcophagus)
			{
				base.Corpse.everBuriedInSarcophagus = true;
				foreach (Pawn pawn in base.Map.mapPawns.FreeColonists)
				{
					if (pawn.needs.mood != null)
					{
						pawn.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.KnowBuriedInSarcophagus, null, null);
					}
				}
			}
		}

		// Token: 0x040064EA RID: 25834
		private bool everNonEmpty;

		// Token: 0x040064EB RID: 25835
		private bool thisIsFirstBodyEver;
	}
}
