using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x020015CD RID: 5581
	public sealed class MemoryThoughtHandler : IExposable
	{
		// Token: 0x170017E8 RID: 6120
		// (get) Token: 0x06008A8D RID: 35469 RVA: 0x00324110 File Offset: 0x00322310
		public List<Thought_Memory> Memories
		{
			get
			{
				return this.memories;
			}
		}

		// Token: 0x06008A8E RID: 35470 RVA: 0x00324118 File Offset: 0x00322318
		public MemoryThoughtHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06008A8F RID: 35471 RVA: 0x00324140 File Offset: 0x00322340
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.tmpMemories.Clear();
				for (int i = this.memories.Count - 1; i >= 0; i--)
				{
					if (!this.memories[i].Save)
					{
						this.tmpMemories.Add(this.memories[i]);
						this.memories.Remove(this.memories[i]);
					}
				}
			}
			Scribe_Collections.Look<Thought_Memory>(ref this.memories, "memories", LookMode.Deep, Array.Empty<object>());
			foreach (Thought_Memory item in this.tmpMemories)
			{
				this.memories.Add(item);
			}
			this.tmpMemories.Clear();
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				for (int j = this.memories.Count - 1; j >= 0; j--)
				{
					if (this.memories[j].def == null)
					{
						this.memories.RemoveAt(j);
					}
					else
					{
						this.memories[j].pawn = this.pawn;
						if (this.memories[j].permanent)
						{
							bool flag = false;
							foreach (Hediff hd in this.pawn.health.hediffSet.hediffs)
							{
								HediffComp_ThoughtSetter hediffComp_ThoughtSetter = hd.TryGetComp<HediffComp_ThoughtSetter>();
								if (hediffComp_ThoughtSetter != null && hediffComp_ThoughtSetter.Props.thought == this.memories[j].def)
								{
									flag = true;
									break;
								}
							}
							if (!flag)
							{
								this.memories[j].permanent = false;
							}
						}
					}
				}
			}
		}

		// Token: 0x06008A90 RID: 35472 RVA: 0x00324334 File Offset: 0x00322534
		public void MemoryThoughtInterval()
		{
			for (int i = 0; i < this.memories.Count; i++)
			{
				this.memories[i].ThoughtInterval();
			}
			this.RemoveExpiredMemories();
		}

		// Token: 0x06008A91 RID: 35473 RVA: 0x00324370 File Offset: 0x00322570
		private void RemoveExpiredMemories()
		{
			for (int i = this.memories.Count - 1; i >= 0; i--)
			{
				Thought_Memory thought_Memory = this.memories[i];
				if (thought_Memory.ShouldDiscard)
				{
					this.RemoveMemory(thought_Memory);
					if (thought_Memory.def.nextThought != null)
					{
						this.TryGainMemory(thought_Memory.def.nextThought, null, null);
					}
				}
			}
		}

		// Token: 0x06008A92 RID: 35474 RVA: 0x003243D4 File Offset: 0x003225D4
		public void TryGainMemoryFast(ThoughtDef mem, Precept sourcePrecept = null)
		{
			Thought_Memory firstMemoryOfDef = this.GetFirstMemoryOfDef(mem);
			if (firstMemoryOfDef != null)
			{
				firstMemoryOfDef.Renew();
				return;
			}
			this.TryGainMemory(mem, null, sourcePrecept);
		}

		// Token: 0x06008A93 RID: 35475 RVA: 0x003243FC File Offset: 0x003225FC
		public void TryGainMemoryFast(ThoughtDef mem, int stage, Precept sourcePrecept = null)
		{
			Thought_Memory firstMemoryOfDef = this.GetFirstMemoryOfDef(mem);
			if (firstMemoryOfDef != null)
			{
				firstMemoryOfDef.Renew();
				firstMemoryOfDef.SetForcedStage(stage);
				return;
			}
			this.TryGainMemory(mem, null, sourcePrecept);
			Thought_Memory firstMemoryOfDef2 = this.GetFirstMemoryOfDef(mem);
			if (firstMemoryOfDef2 == null)
			{
				return;
			}
			firstMemoryOfDef2.SetForcedStage(stage);
		}

		// Token: 0x06008A94 RID: 35476 RVA: 0x0032443D File Offset: 0x0032263D
		public void TryGainMemory(ThoughtDef def, Pawn otherPawn = null, Precept sourcePrecept = null)
		{
			if (!def.IsMemory)
			{
				Log.Error(((def != null) ? def.ToString() : null) + " is not a memory thought.");
				return;
			}
			this.TryGainMemory(ThoughtMaker.MakeThought(def, sourcePrecept), otherPawn);
		}

		// Token: 0x06008A95 RID: 35477 RVA: 0x00324474 File Offset: 0x00322674
		public void TryGainMemory(Thought_Memory newThought, Pawn otherPawn = null)
		{
			if (!ThoughtUtility.CanGetThought(this.pawn, newThought.def, false))
			{
				return;
			}
			if (newThought is Thought_MemorySocial)
			{
				if (newThought.otherPawn == null && otherPawn == null)
				{
					string str = "Can't gain social thought ";
					ThoughtDef def = newThought.def;
					Log.Error(str + ((def != null) ? def.ToString() : null) + " because its otherPawn is null and otherPawn passed to this method is also null. Social thoughts must have otherPawn.");
					return;
				}
				otherPawn = (otherPawn ?? newThought.otherPawn);
				if (!newThought.def.socialTargetDevelopmentalStageFilter.Has(otherPawn.DevelopmentalStage))
				{
					return;
				}
			}
			newThought.pawn = this.pawn;
			newThought.otherPawn = otherPawn;
			bool flag;
			if (!newThought.TryMergeWithExistingMemory(out flag))
			{
				this.memories.Add(newThought);
			}
			if (newThought.def.stackLimitForSameOtherPawn >= 0)
			{
				while (this.NumMemoriesInGroup(newThought) > newThought.def.stackLimitForSameOtherPawn)
				{
					this.RemoveMemory(this.OldestMemoryInGroup(newThought));
				}
			}
			if (newThought.def.stackLimit >= 0)
			{
				while (this.NumMemoriesOfDef(newThought.def) > newThought.def.stackLimit)
				{
					this.RemoveMemory(this.OldestMemoryOfDef(newThought.def));
				}
			}
			if (newThought.def.thoughtToMake != null)
			{
				this.TryGainMemory(newThought.def.thoughtToMake, newThought.otherPawn, null);
			}
			List<Thought_Memory> list = (newThought.def.replaceThoughts == null) ? this.memories : new List<Thought_Memory>(this.memories);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i] != newThought && list[i].GroupsWith(newThought))
				{
					list[i].Notify_NewThoughtInGroupAdded(newThought);
				}
				if (newThought.def.replaceThoughts != null && newThought.def.replaceThoughts.Contains(list[i].def))
				{
					this.RemoveMemory(list[i]);
				}
			}
			if (flag && newThought.def.showBubble && this.pawn.Spawned && PawnUtility.ShouldSendNotificationAbout(this.pawn))
			{
				MoteMaker.MakeMoodThoughtBubble(this.pawn, newThought);
			}
		}

		// Token: 0x06008A96 RID: 35478 RVA: 0x00324678 File Offset: 0x00322878
		public Thought_Memory OldestMemoryInGroup(Thought_Memory group)
		{
			Thought_Memory result = null;
			int num = -9999;
			for (int i = 0; i < this.memories.Count; i++)
			{
				Thought_Memory thought_Memory = this.memories[i];
				if (thought_Memory.GroupsWith(group) && thought_Memory.age > num)
				{
					result = thought_Memory;
					num = thought_Memory.age;
				}
			}
			return result;
		}

		// Token: 0x06008A97 RID: 35479 RVA: 0x003246CC File Offset: 0x003228CC
		public Thought_Memory OldestMemoryOfDef(ThoughtDef def)
		{
			Thought_Memory result = null;
			int num = -9999;
			for (int i = 0; i < this.memories.Count; i++)
			{
				Thought_Memory thought_Memory = this.memories[i];
				if (thought_Memory.def == def && thought_Memory.age > num)
				{
					result = thought_Memory;
					num = thought_Memory.age;
				}
			}
			return result;
		}

		// Token: 0x06008A98 RID: 35480 RVA: 0x00324720 File Offset: 0x00322920
		public void RemoveMemory(Thought_Memory th)
		{
			if (!this.memories.Remove(th))
			{
				Log.Warning("Tried to remove memory thought of def " + th.def.defName + " but it's not here.");
			}
		}

		// Token: 0x06008A99 RID: 35481 RVA: 0x00324750 File Offset: 0x00322950
		public int NumMemoriesInGroup(Thought_Memory group)
		{
			int num = 0;
			for (int i = 0; i < this.memories.Count; i++)
			{
				if (this.memories[i].GroupsWith(group))
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06008A9A RID: 35482 RVA: 0x00324790 File Offset: 0x00322990
		public int NumMemoriesOfDef(ThoughtDef def)
		{
			int num = 0;
			for (int i = 0; i < this.memories.Count; i++)
			{
				if (this.memories[i].def == def)
				{
					num++;
				}
			}
			return num;
		}

		// Token: 0x06008A9B RID: 35483 RVA: 0x003247D0 File Offset: 0x003229D0
		public Thought_Memory GetFirstMemoryOfDef(ThoughtDef def)
		{
			for (int i = 0; i < this.memories.Count; i++)
			{
				if (this.memories[i].def == def)
				{
					return this.memories[i];
				}
			}
			return null;
		}

		// Token: 0x06008A9C RID: 35484 RVA: 0x00324818 File Offset: 0x00322A18
		public void RemoveMemoriesOfDefWhereOtherPawnIs(ThoughtDef def, Pawn otherPawn)
		{
			for (int i = this.memories.Count - 1; i >= 0; i--)
			{
				Thought_Memory thought_Memory = this.memories[i];
				if (thought_Memory.def == def && thought_Memory.otherPawn == otherPawn)
				{
					this.RemoveMemory(thought_Memory);
				}
			}
		}

		// Token: 0x06008A9D RID: 35485 RVA: 0x00324864 File Offset: 0x00322A64
		public void RemoveMemoriesWhereOtherPawnIs(Pawn otherPawn)
		{
			Predicate<Thought_Memory> <>9__0;
			for (;;)
			{
				List<Thought_Memory> list = this.memories;
				Predicate<Thought_Memory> match;
				if ((match = <>9__0) == null)
				{
					match = (<>9__0 = ((Thought_Memory x) => x.otherPawn == otherPawn));
				}
				Thought_Memory thought_Memory = list.Find(match);
				if (thought_Memory == null)
				{
					break;
				}
				this.RemoveMemory(thought_Memory);
			}
		}

		// Token: 0x06008A9E RID: 35486 RVA: 0x003248B8 File Offset: 0x00322AB8
		public void RemoveMemoriesOfDef(ThoughtDef def)
		{
			if (!def.IsMemory)
			{
				ThoughtDef def2 = def;
				Log.Warning(((def2 != null) ? def2.ToString() : null) + " is not a memory thought.");
				return;
			}
			Predicate<Thought_Memory> <>9__0;
			for (;;)
			{
				List<Thought_Memory> list = this.memories;
				Predicate<Thought_Memory> match;
				if ((match = <>9__0) == null)
				{
					match = (<>9__0 = ((Thought_Memory x) => x.def == def));
				}
				Thought_Memory thought_Memory = list.Find(match);
				if (thought_Memory == null)
				{
					break;
				}
				this.RemoveMemory(thought_Memory);
			}
		}

		// Token: 0x06008A9F RID: 35487 RVA: 0x00324938 File Offset: 0x00322B38
		public void RemoveMemoriesOfDefIf(ThoughtDef def, Func<Thought_Memory, bool> predicate)
		{
			if (!def.IsMemory)
			{
				ThoughtDef def2 = def;
				Log.Warning(((def2 != null) ? def2.ToString() : null) + " is not a memory thought.");
				return;
			}
			Predicate<Thought_Memory> <>9__0;
			for (;;)
			{
				List<Thought_Memory> list = this.memories;
				Predicate<Thought_Memory> match;
				if ((match = <>9__0) == null)
				{
					match = (<>9__0 = ((Thought_Memory x) => x.def == def && predicate(x)));
				}
				Thought_Memory thought_Memory = list.Find(match);
				if (thought_Memory == null)
				{
					break;
				}
				this.RemoveMemory(thought_Memory);
			}
		}

		// Token: 0x06008AA0 RID: 35488 RVA: 0x003249C0 File Offset: 0x00322BC0
		public bool AnyMemoryConcerns(Pawn otherPawn)
		{
			for (int i = 0; i < this.memories.Count; i++)
			{
				if (this.memories[i].otherPawn == otherPawn)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008AA1 RID: 35489 RVA: 0x003249FA File Offset: 0x00322BFA
		public void Notify_PawnDiscarded(Pawn discarded)
		{
			this.RemoveMemoriesWhereOtherPawnIs(discarded);
		}

		// Token: 0x04005560 RID: 21856
		public Pawn pawn;

		// Token: 0x04005561 RID: 21857
		private List<Thought_Memory> memories = new List<Thought_Memory>();

		// Token: 0x04005562 RID: 21858
		private List<Thought_Memory> tmpMemories = new List<Thought_Memory>();
	}
}
