using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001638 RID: 5688
	public class PreceptComp_SelfTookMemoryThought : PreceptComp_Thought
	{
		// Token: 0x1700189B RID: 6299
		// (get) Token: 0x06008E8E RID: 36494 RVA: 0x003466D4 File Offset: 0x003448D4
		public override IEnumerable<TraitRequirement> TraitsAffecting
		{
			get
			{
				return ThoughtUtility.GetNullifyingTraits(this.thought);
			}
		}

		// Token: 0x06008E8F RID: 36495 RVA: 0x003466E4 File Offset: 0x003448E4
		public override void Notify_MemberTookAction(HistoryEvent ev, Precept precept, bool canApplySelfTookThoughts)
		{
			if (ev.def != this.eventDef || !canApplySelfTookThoughts)
			{
				return;
			}
			Pawn arg = ev.args.GetArg<Pawn>(HistoryEventArgsNames.Doer);
			if (arg.needs != null && arg.needs.mood != null && (!this.onlyForNonSlaves || !arg.IsSlave))
			{
				if (this.thought.minExpectation != null && ExpectationsUtility.CurrentExpectationFor(arg).order < this.thought.minExpectation.order)
				{
					return;
				}
				Thought_Memory thought_Memory = ThoughtMaker.MakeThought(this.thought, precept);
				Thought_KilledInnocentAnimal thought_KilledInnocentAnimal = thought_Memory as Thought_KilledInnocentAnimal;
				Pawn animal;
				if (thought_KilledInnocentAnimal != null && ev.args.TryGetArg<Pawn>(HistoryEventArgsNames.Victim, out animal))
				{
					thought_KilledInnocentAnimal.SetAnimal(animal);
				}
				Thought_MemoryObservation thought_MemoryObservation = thought_Memory as Thought_MemoryObservation;
				Corpse target;
				if (thought_MemoryObservation != null && ev.args.TryGetArg<Corpse>(HistoryEventArgsNames.Subject, out target))
				{
					thought_MemoryObservation.Target = target;
				}
				arg.needs.mood.thoughts.memories.TryGainMemory(thought_Memory, null);
			}
		}

		// Token: 0x04005760 RID: 22368
		public HistoryEventDef eventDef;

		// Token: 0x04005761 RID: 22369
		public bool onlyForNonSlaves;
	}
}
