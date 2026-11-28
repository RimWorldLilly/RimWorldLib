using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001636 RID: 5686
	public class PreceptComp_KnowsMemoryThought : PreceptComp_Thought
	{
		// Token: 0x1700189A RID: 6298
		// (get) Token: 0x06008E89 RID: 36489 RVA: 0x003464B0 File Offset: 0x003446B0
		public override IEnumerable<TraitRequirement> TraitsAffecting
		{
			get
			{
				return ThoughtUtility.GetNullifyingTraits(this.thought);
			}
		}

		// Token: 0x06008E8A RID: 36490 RVA: 0x003464C0 File Offset: 0x003446C0
		public override void Notify_MemberWitnessedAction(HistoryEvent ev, Precept precept, Pawn member)
		{
			if (ev.def != this.eventDef)
			{
				return;
			}
			if (!precept.def.enabledForNPCFactions && !member.CountsAsNonNPCForPrecepts())
			{
				return;
			}
			Pawn pawn;
			bool flag = ev.args.TryGetArg<Pawn>(HistoryEventArgsNames.Doer, out pawn);
			bool flag2 = false;
			if (this.doerMustBeMyFaction != null)
			{
				flag2 = this.doerMustBeMyFaction.Value;
			}
			else
			{
				if (flag)
				{
					using (List<ThoughtStage>.Enumerator enumerator = this.thought.stages.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (enumerator.Current.baseMoodEffect != 0f)
							{
								flag2 = true;
								break;
							}
						}
						goto IL_9E;
					}
				}
				flag2 = false;
			}
			IL_9E:
			if (member.needs != null && member.needs.mood != null && (!flag2 || (flag && pawn.Faction == member.Faction)) && (!this.doerMustBeMyIdeo || pawn.Ideo == member.Ideo) && (!this.onlyForNonSlaves || !member.IsSlave) && (!typeof(Thought_MemorySocial).IsAssignableFrom(this.thought.thoughtClass) || flag))
			{
				Thought_Memory thought_Memory = ThoughtMaker.MakeThought(this.thought, precept);
				int val;
				if (ev.args.TryGetArg<int>(HistoryEventArgsNames.ExecutionThoughtStage, out val))
				{
					thought_Memory.SetForcedStage(Math.Min(val, this.thought.stages.Count - 1));
				}
				Thought_KilledInnocentAnimal thought_KilledInnocentAnimal = thought_Memory as Thought_KilledInnocentAnimal;
				Pawn animal;
				if (thought_KilledInnocentAnimal != null && ev.args.TryGetArg<Pawn>(HistoryEventArgsNames.Victim, out animal))
				{
					thought_KilledInnocentAnimal.SetAnimal(animal);
				}
				member.needs.mood.thoughts.memories.TryGainMemory(thought_Memory, pawn);
				if (this.removesThought != null)
				{
					member.needs.mood.thoughts.memories.RemoveMemoriesOfDef(this.removesThought);
				}
			}
		}

		// Token: 0x0400575A RID: 22362
		public HistoryEventDef eventDef;

		// Token: 0x0400575B RID: 22363
		public bool? doerMustBeMyFaction;

		// Token: 0x0400575C RID: 22364
		public ThoughtDef removesThought;

		// Token: 0x0400575D RID: 22365
		public bool onlyForNonSlaves;

		// Token: 0x0400575E RID: 22366
		public bool doerMustBeMyIdeo;
	}
}
