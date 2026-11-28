using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A2 RID: 2466
	public class InspirationHandler : IExposable
	{
		// Token: 0x17000DA0 RID: 3488
		// (get) Token: 0x06004AAF RID: 19119 RVA: 0x001D171D File Offset: 0x001CF91D
		public bool Inspired
		{
			get
			{
				return this.curState != null;
			}
		}

		// Token: 0x17000DA1 RID: 3489
		// (get) Token: 0x06004AB0 RID: 19120 RVA: 0x001D1728 File Offset: 0x001CF928
		public Inspiration CurState
		{
			get
			{
				return this.curState;
			}
		}

		// Token: 0x17000DA2 RID: 3490
		// (get) Token: 0x06004AB1 RID: 19121 RVA: 0x001D1730 File Offset: 0x001CF930
		public InspirationDef CurStateDef
		{
			get
			{
				Inspiration inspiration = this.curState;
				if (inspiration == null)
				{
					return null;
				}
				return inspiration.def;
			}
		}

		// Token: 0x17000DA3 RID: 3491
		// (get) Token: 0x06004AB2 RID: 19122 RVA: 0x001D1744 File Offset: 0x001CF944
		private float StartInspirationMTBDays
		{
			get
			{
				if (this.pawn.needs.mood == null)
				{
					return -1f;
				}
				float curLevel = this.pawn.needs.mood.CurLevel;
				if (curLevel < 0.5f)
				{
					return -1f;
				}
				return GenMath.LerpDouble(0.5f, 1f, 210f, 10f, curLevel);
			}
		}

		// Token: 0x06004AB3 RID: 19123 RVA: 0x001D17A7 File Offset: 0x001CF9A7
		public InspirationHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06004AB4 RID: 19124 RVA: 0x001D17B6 File Offset: 0x001CF9B6
		public void ExposeData()
		{
			Scribe_Deep.Look<Inspiration>(ref this.curState, "curState", Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.curState != null)
			{
				this.curState.pawn = this.pawn;
			}
		}

		// Token: 0x06004AB5 RID: 19125 RVA: 0x001D17EE File Offset: 0x001CF9EE
		public void InspirationHandlerTickInterval(int delta)
		{
			if (this.curState != null)
			{
				this.curState.InspirationTick(delta);
			}
			if (this.pawn.IsHashIntervalTick(100, delta))
			{
				this.CheckStartRandomInspiration();
			}
		}

		// Token: 0x06004AB6 RID: 19126 RVA: 0x001D181C File Offset: 0x001CFA1C
		public bool TryStartInspiration(InspirationDef def, string reason = null, bool sendLetter = true)
		{
			if (this.Inspired)
			{
				return false;
			}
			if (this.BlockedByHediff())
			{
				return false;
			}
			if (!def.Worker.InspirationCanOccur(this.pawn))
			{
				return false;
			}
			this.curState = (Inspiration)Activator.CreateInstance(def.inspirationClass);
			this.curState.def = def;
			this.curState.pawn = this.pawn;
			this.curState.reason = reason;
			this.curState.PostStart(sendLetter);
			return true;
		}

		// Token: 0x06004AB7 RID: 19127 RVA: 0x001D18A0 File Offset: 0x001CFAA0
		public void EndInspiration(Inspiration inspiration)
		{
			if (inspiration == null)
			{
				return;
			}
			if (this.curState != inspiration)
			{
				Log.Error("Tried to end inspiration " + inspiration.ToStringSafe<Inspiration>() + " but current inspiration is " + this.curState.ToStringSafe<Inspiration>());
				return;
			}
			this.curState = null;
			inspiration.PostEnd();
		}

		// Token: 0x06004AB8 RID: 19128 RVA: 0x001D18ED File Offset: 0x001CFAED
		public void EndInspiration(InspirationDef inspirationDef)
		{
			if (this.curState != null && this.curState.def == inspirationDef)
			{
				this.EndInspiration(this.curState);
			}
		}

		// Token: 0x06004AB9 RID: 19129 RVA: 0x001D1911 File Offset: 0x001CFB11
		public void Reset()
		{
			this.curState = null;
		}

		// Token: 0x06004ABA RID: 19130 RVA: 0x001D191C File Offset: 0x001CFB1C
		private void CheckStartRandomInspiration()
		{
			if (this.Inspired)
			{
				return;
			}
			if (!this.pawn.health.capacities.CanBeAwake)
			{
				return;
			}
			float startInspirationMTBDays = this.StartInspirationMTBDays;
			if (startInspirationMTBDays < 0f)
			{
				return;
			}
			if (Rand.MTBEventOccurs(startInspirationMTBDays, 60000f, 100f))
			{
				InspirationDef randomAvailableInspirationDef = this.GetRandomAvailableInspirationDef();
				if (randomAvailableInspirationDef != null)
				{
					this.TryStartInspiration(randomAvailableInspirationDef, "LetterInspirationBeginThanksToHighMoodPart".Translate(), true);
				}
			}
		}

		// Token: 0x06004ABB RID: 19131 RVA: 0x001D198E File Offset: 0x001CFB8E
		public InspirationDef GetRandomAvailableInspirationDef()
		{
			return (from x in DefDatabase<InspirationDef>.AllDefsListForReading
			where x.Worker.InspirationCanOccur(this.pawn)
			select x).RandomElementByWeightWithFallback((InspirationDef x) => x.Worker.CommonalityFor(this.pawn), null);
		}

		// Token: 0x06004ABC RID: 19132 RVA: 0x001D19B8 File Offset: 0x001CFBB8
		private bool BlockedByHediff()
		{
			foreach (Hediff hediff in this.pawn.health.hediffSet.hediffs)
			{
				if (hediff.CurStage != null && hediff.CurStage.blocksInspirations)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x04002F9C RID: 12188
		public Pawn pawn;

		// Token: 0x04002F9D RID: 12189
		private Inspiration curState;

		// Token: 0x04002F9E RID: 12190
		private const int CheckStartInspirationIntervalTicks = 100;

		// Token: 0x04002F9F RID: 12191
		private const float StartInspirationMTBDaysAtMaxMood = 10f;

		// Token: 0x04002FA0 RID: 12192
		private const float MinMood = 0.5f;
	}
}
