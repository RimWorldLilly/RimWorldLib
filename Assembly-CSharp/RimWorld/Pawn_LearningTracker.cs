using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x0200159A RID: 5530
	public class Pawn_LearningTracker : IExposable
	{
		// Token: 0x1700175C RID: 5980
		// (get) Token: 0x0600883B RID: 34875 RVA: 0x00314EC8 File Offset: 0x003130C8
		public Pawn Pawn
		{
			get
			{
				return this.pawn;
			}
		}

		// Token: 0x1700175D RID: 5981
		// (get) Token: 0x0600883C RID: 34876 RVA: 0x00314ED0 File Offset: 0x003130D0
		public List<LearningDesireDef> ActiveLearningDesires
		{
			get
			{
				return this.active;
			}
		}

		// Token: 0x0600883D RID: 34877 RVA: 0x00314ED8 File Offset: 0x003130D8
		public Pawn_LearningTracker()
		{
		}

		// Token: 0x0600883E RID: 34878 RVA: 0x00314EEB File Offset: 0x003130EB
		public Pawn_LearningTracker(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x0600883F RID: 34879 RVA: 0x00314F08 File Offset: 0x00313108
		public void AddNewLearningDesire()
		{
			LearningDesireDef item = (from ld in DefDatabase<LearningDesireDef>.AllDefsListForReading
			where !this.active.Contains(ld) && ld.Worker.CanGiveDesire
			select ld).RandomElementByWeight((LearningDesireDef ld) => ld.selectionWeight);
			if (this.active.Count >= 2)
			{
				this.active.RemoveAt(0);
			}
			this.active.Add(item);
		}

		// Token: 0x06008840 RID: 34880 RVA: 0x00314F76 File Offset: 0x00313176
		public void LearningTickInterval(int delta)
		{
			this.newLearningDesireTicksLeft -= delta;
			if (this.newLearningDesireTicksLeft <= 0)
			{
				this.AddNewLearningDesire();
				this.newLearningDesireTicksLeft = Rand.Range(20000, 40000);
			}
		}

		// Token: 0x06008841 RID: 34881 RVA: 0x00314FAA File Offset: 0x003131AA
		public void Debug_SetLearningDesire(LearningDesireDef desire)
		{
			this.active.Clear();
			this.active.Add(desire);
		}

		// Token: 0x06008842 RID: 34882 RVA: 0x00314FC3 File Offset: 0x003131C3
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.newLearningDesireTicksLeft, "newLearningDesireTicksLeft", 0, false);
			Scribe_Collections.Look<LearningDesireDef>(ref this.active, "active", LookMode.Def, Array.Empty<object>());
		}

		// Token: 0x04005400 RID: 21504
		private const int MaxActiveLearningDesires = 2;

		// Token: 0x04005401 RID: 21505
		private const int MinLearningDesireIntervalTicks = 20000;

		// Token: 0x04005402 RID: 21506
		private const int MaxLearningDesireIntervalTicks = 40000;

		// Token: 0x04005403 RID: 21507
		private Pawn pawn;

		// Token: 0x04005404 RID: 21508
		private List<LearningDesireDef> active = new List<LearningDesireDef>();

		// Token: 0x04005405 RID: 21509
		private int newLearningDesireTicksLeft;
	}
}
