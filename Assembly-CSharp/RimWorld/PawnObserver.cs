using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x02001589 RID: 5513
	public class PawnObserver
	{
		// Token: 0x06008713 RID: 34579 RVA: 0x0030CFBC File Offset: 0x0030B1BC
		public PawnObserver(Pawn pawn)
		{
			this.pawn = pawn;
			this.intervalsUntilObserve = 0;
		}

		// Token: 0x06008714 RID: 34580 RVA: 0x0030CFDD File Offset: 0x0030B1DD
		public void ObserverInterval()
		{
			if (!this.pawn.Spawned)
			{
				return;
			}
			this.intervalsUntilObserve--;
			if (this.intervalsUntilObserve <= 0)
			{
				this.ObserveSurroundingThings();
				this.intervalsUntilObserve = 4 + Rand.RangeInclusive(-1, 1);
			}
		}

		// Token: 0x06008715 RID: 34581 RVA: 0x0030D01C File Offset: 0x0030B21C
		private void ObserveSurroundingThings()
		{
			TerrorUtility.RemoveAllTerrorThoughts(this.pawn);
			this.terrorThoughts.Clear();
			if (PawnUtility.IsBiologicallyOrArtificiallyBlind(this.pawn) || !this.pawn.Awake() || this.pawn.needs.mood == null)
			{
				return;
			}
			RegionTraverser.BreadthFirstTraverse(this.pawn.Position, this.pawn.Map, (Region from, Region to) => this.pawn.Position.InHorDistOf(to.extentsClose.ClosestCellTo(this.pawn.Position), 5f), delegate(Region reg)
			{
				foreach (Thing thing in reg.ListerThings.ThingsInGroup(ThingRequestGroup.Corpse))
				{
					if (this.PossibleToObserve(thing))
					{
						this.<ObserveSurroundingThings>g__TryCreateObservedThought|7_0(thing);
						this.<ObserveSurroundingThings>g__TryCreateObservedHistoryEvent|7_1(thing);
					}
				}
				foreach (Thing thing2 in reg.ListerThings.ThingsInGroup(ThingRequestGroup.Pawn))
				{
					if (this.PossibleToObserve(thing2))
					{
						this.<ObserveSurroundingThings>g__TryCreateObservedThought|7_0(thing2);
						this.<ObserveSurroundingThings>g__TryCreateObservedHistoryEvent|7_1(thing2);
					}
				}
				foreach (Thing thing3 in reg.ListerThings.ThingsInGroup(ThingRequestGroup.BuildingArtificial))
				{
					if (this.PossibleToObserve(thing3))
					{
						this.<ObserveSurroundingThings>g__TryCreateObservedThought|7_0(thing3);
						this.<ObserveSurroundingThings>g__TryCreateObservedHistoryEvent|7_1(thing3);
					}
				}
				return false;
			}, 999999, RegionType.Set_Passable);
			foreach (Thought_MemoryObservationTerror newThought in TerrorUtility.TakeTopTerrorThoughts(this.terrorThoughts))
			{
				this.pawn.needs.mood.thoughts.memories.TryGainMemory(newThought, null);
			}
			this.terrorThoughts.Clear();
		}

		// Token: 0x06008716 RID: 34582 RVA: 0x0030D110 File Offset: 0x0030B310
		private bool PossibleToObserve(Thing thing)
		{
			return thing.Position.InHorDistOf(this.pawn.Position, 5f) && GenSight.LineOfSight(thing.Position, this.pawn.Position, this.pawn.Map, true, null, 0, 0);
		}

		// Token: 0x06008717 RID: 34583 RVA: 0x0030D164 File Offset: 0x0030B364
		[CompilerGenerated]
		private void <ObserveSurroundingThings>g__TryCreateObservedThought|7_0(Thing thing)
		{
			Thought_MemoryObservationTerror item;
			if (TerrorUtility.TryCreateTerrorThought(thing, out item))
			{
				this.terrorThoughts.Add(item);
			}
			IObservedThoughtGiver observedThoughtGiver = thing as IObservedThoughtGiver;
			if (observedThoughtGiver != null)
			{
				Thought_Memory thought_Memory = observedThoughtGiver.GiveObservedThought(this.pawn);
				if (thought_Memory != null)
				{
					this.pawn.needs.mood.thoughts.memories.TryGainMemory(thought_Memory, null);
				}
			}
		}

		// Token: 0x06008718 RID: 34584 RVA: 0x0030D1C4 File Offset: 0x0030B3C4
		[CompilerGenerated]
		private void <ObserveSurroundingThings>g__TryCreateObservedHistoryEvent|7_1(Thing thing)
		{
			IObservedThoughtGiver observedThoughtGiver = thing as IObservedThoughtGiver;
			if (observedThoughtGiver != null)
			{
				HistoryEventDef historyEventDef = observedThoughtGiver.GiveObservedHistoryEvent(this.pawn);
				if (historyEventDef != null)
				{
					HistoryEvent historyEvent = new HistoryEvent(historyEventDef, this.pawn.Named(HistoryEventArgsNames.Doer), thing.Named(HistoryEventArgsNames.Subject));
					Find.HistoryEventsManager.RecordEvent(historyEvent, true);
				}
			}
		}

		// Token: 0x04005383 RID: 21379
		private Pawn pawn;

		// Token: 0x04005384 RID: 21380
		private int intervalsUntilObserve;

		// Token: 0x04005385 RID: 21381
		private List<Thought_MemoryObservationTerror> terrorThoughts = new List<Thought_MemoryObservationTerror>();

		// Token: 0x04005386 RID: 21382
		private const int IntervalsBetweenObservations = 4;

		// Token: 0x04005387 RID: 21383
		private const float ObservationRadius = 5f;
	}
}
