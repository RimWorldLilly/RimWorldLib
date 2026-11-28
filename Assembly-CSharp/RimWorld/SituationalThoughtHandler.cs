using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Verse;

namespace RimWorld
{
	// Token: 0x020015CE RID: 5582
	public sealed class SituationalThoughtHandler
	{
		// Token: 0x06008AA2 RID: 35490 RVA: 0x00324A03 File Offset: 0x00322C03
		public SituationalThoughtHandler(Pawn pawn)
		{
			this.pawn = pawn;
		}

		// Token: 0x06008AA3 RID: 35491 RVA: 0x00324A3A File Offset: 0x00322C3A
		public void SituationalThoughtInterval()
		{
			this.UpdateAllMoodThoughts();
			this.RemoveExpiredThoughtsFromCache();
		}

		// Token: 0x06008AA4 RID: 35492 RVA: 0x00324A48 File Offset: 0x00322C48
		public void AppendMoodThoughts(List<Thought> outThoughts)
		{
			if (this.thoughtsDirty)
			{
				this.UpdateAllMoodThoughts();
			}
			foreach (Thought_Situational thought_Situational in this.cachedThoughts)
			{
				if (thought_Situational.Active)
				{
					outThoughts.Add(thought_Situational);
				}
			}
		}

		// Token: 0x06008AA5 RID: 35493 RVA: 0x00324AB4 File Offset: 0x00322CB4
		public void AppendMoodThoughts(ThoughtDef def, List<Thought> thoughts)
		{
			bool flag = false;
			foreach (Thought_Situational thought_Situational in this.cachedThoughts)
			{
				if (thought_Situational.def == def && thought_Situational.Active)
				{
					thought_Situational.RecalculateState();
					if (thought_Situational.Active)
					{
						flag = true;
						thoughts.Add(thought_Situational);
					}
				}
			}
			if (flag)
			{
				return;
			}
			Thought_Situational thought_Situational2 = this.TryCreateThought(def);
			if (thought_Situational2 != null)
			{
				this.cachedThoughts.Add(thought_Situational2);
				thoughts.Add(thought_Situational2);
			}
		}

		// Token: 0x06008AA6 RID: 35494 RVA: 0x00324B4C File Offset: 0x00322D4C
		public void AppendSocialThoughts(Pawn otherPawn, List<ISocialThought> outThoughts)
		{
			this.CheckRecalculateSocialThoughts(otherPawn);
			SituationalThoughtHandler.CachedSocialThoughts cachedSocialThoughts = this.cachedSocialThoughts[otherPawn];
			cachedSocialThoughts.lastQueryTick = Find.TickManager.TicksGame;
			List<Thought_SituationalSocial> activeThoughts = cachedSocialThoughts.activeThoughts;
			for (int i = 0; i < activeThoughts.Count; i++)
			{
				outThoughts.Add(activeThoughts[i]);
			}
		}

		// Token: 0x06008AA7 RID: 35495 RVA: 0x00324BA0 File Offset: 0x00322DA0
		private void UpdateAllMoodThoughts()
		{
			this.thoughtsDirty = false;
			List<ThoughtDef> situationalNonSocialThoughtDefs = ThoughtUtility.situationalNonSocialThoughtDefs;
			foreach (Thought_Situational thought_Situational in this.cachedThoughts)
			{
				thought_Situational.RecalculateState();
			}
			foreach (ThoughtDef thoughtDef in situationalNonSocialThoughtDefs)
			{
				bool flag = false;
				using (List<Thought_Situational>.Enumerator enumerator = this.cachedThoughts.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						if (enumerator.Current.def == thoughtDef)
						{
							flag = true;
						}
					}
				}
				if (!flag)
				{
					Thought_Situational thought_Situational2 = this.TryCreateThought(thoughtDef);
					if (thought_Situational2 != null)
					{
						this.cachedThoughts.Add(thought_Situational2);
					}
				}
			}
			if (ModsConfig.IdeologyActive && this.pawn.Ideo != null)
			{
				foreach (Precept precept in this.pawn.Ideo.PreceptsListForReading)
				{
					foreach (Thought_Situational item in precept.SituationThoughtsToAdd(this.pawn, this.cachedThoughts))
					{
						this.cachedThoughts.Add(item);
					}
				}
			}
		}

		// Token: 0x06008AA8 RID: 35496 RVA: 0x00324D4C File Offset: 0x00322F4C
		private void CheckRecalculateSocialThoughts(Pawn otherPawn)
		{
			try
			{
				SituationalThoughtHandler.CachedSocialThoughts cachedSocialThoughts;
				if (!this.cachedSocialThoughts.TryGetValue(otherPawn, out cachedSocialThoughts))
				{
					cachedSocialThoughts = new SituationalThoughtHandler.CachedSocialThoughts();
					this.cachedSocialThoughts.Add(otherPawn, cachedSocialThoughts);
				}
				if (cachedSocialThoughts.ShouldRecalculateState)
				{
					cachedSocialThoughts.lastRecalculationTick = Find.TickManager.TicksGame;
					this.tmpCachedSocialThoughts.Clear();
					for (int i = 0; i < cachedSocialThoughts.thoughts.Count; i++)
					{
						Thought_SituationalSocial thought_SituationalSocial = cachedSocialThoughts.thoughts[i];
						thought_SituationalSocial.RecalculateState();
						this.tmpCachedSocialThoughts.Add(thought_SituationalSocial.def);
					}
					List<ThoughtDef> situationalSocialThoughtDefs = ThoughtUtility.situationalSocialThoughtDefs;
					int j = 0;
					int count = situationalSocialThoughtDefs.Count;
					while (j < count)
					{
						if (!this.tmpCachedSocialThoughts.Contains(situationalSocialThoughtDefs[j]))
						{
							Thought_SituationalSocial thought_SituationalSocial2 = this.TryCreateSocialThought(situationalSocialThoughtDefs[j], otherPawn);
							if (thought_SituationalSocial2 != null)
							{
								cachedSocialThoughts.thoughts.Add(thought_SituationalSocial2);
							}
						}
						j++;
					}
					cachedSocialThoughts.activeThoughts.Clear();
					for (int k = 0; k < cachedSocialThoughts.thoughts.Count; k++)
					{
						Thought_SituationalSocial thought_SituationalSocial3 = cachedSocialThoughts.thoughts[k];
						if (thought_SituationalSocial3.Active)
						{
							cachedSocialThoughts.activeThoughts.Add(thought_SituationalSocial3);
						}
					}
				}
			}
			catch (Exception value)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(56, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Exception when recalculating social thoughts for pawn ");
				defaultInterpolatedStringHandler.AppendFormatted<Pawn>(this.pawn);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted<Exception>(value);
				Log.Error(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			finally
			{
			}
		}

		// Token: 0x06008AA9 RID: 35497 RVA: 0x00324F00 File Offset: 0x00323100
		private Thought_Situational TryCreateThought(ThoughtDef def)
		{
			Thought_Situational thought_Situational = null;
			try
			{
				if (!ThoughtUtility.CanGetThought(this.pawn, def, false))
				{
					return null;
				}
				if (!def.Worker.CurrentState(this.pawn).ActiveFor(def))
				{
					return null;
				}
				thought_Situational = (Thought_Situational)ThoughtMaker.MakeThought(def);
				thought_Situational.pawn = this.pawn;
				if (def.Worker is ThoughtWorker_Precept)
				{
					thought_Situational.sourcePrecept = this.pawn.Ideo.GetFirstPreceptAllowingSituationalThought(def);
				}
				thought_Situational.RecalculateState();
			}
			catch (Exception ex)
			{
				string[] array = new string[6];
				array[0] = "Exception while recalculating ";
				array[1] = ((def != null) ? def.ToString() : null);
				array[2] = " thought state for pawn ";
				int num = 3;
				Pawn pawn = this.pawn;
				array[num] = ((pawn != null) ? pawn.ToString() : null);
				array[4] = ": ";
				int num2 = 5;
				Exception ex2 = ex;
				array[num2] = ((ex2 != null) ? ex2.ToString() : null);
				Log.Error(string.Concat(array));
			}
			return thought_Situational;
		}

		// Token: 0x06008AAA RID: 35498 RVA: 0x00325000 File Offset: 0x00323200
		private Thought_SituationalSocial TryCreateSocialThought(ThoughtDef def, Pawn otherPawn)
		{
			Thought_SituationalSocial thought_SituationalSocial = null;
			try
			{
				if (!ThoughtUtility.CanGetThought(this.pawn, def, false))
				{
					return null;
				}
				if (!def.Worker.CurrentSocialState(this.pawn, otherPawn).ActiveFor(def))
				{
					return null;
				}
				if (!def.socialTargetDevelopmentalStageFilter.HasAny(otherPawn.DevelopmentalStage))
				{
					return null;
				}
				if (def.ignoreSubhumans && otherPawn.IsSubhuman)
				{
					return null;
				}
				thought_SituationalSocial = (Thought_SituationalSocial)ThoughtMaker.MakeThought(def);
				thought_SituationalSocial.pawn = this.pawn;
				thought_SituationalSocial.otherPawn = otherPawn;
				if (def.Worker is ThoughtWorker_Precept_Social)
				{
					thought_SituationalSocial.sourcePrecept = this.pawn.Ideo.GetFirstPreceptAllowingSituationalThought(def);
				}
				thought_SituationalSocial.RecalculateState();
			}
			catch (Exception ex)
			{
				string[] array = new string[6];
				array[0] = "Exception while recalculating ";
				array[1] = ((def != null) ? def.ToString() : null);
				array[2] = " thought state for pawn ";
				int num = 3;
				Pawn pawn = this.pawn;
				array[num] = ((pawn != null) ? pawn.ToString() : null);
				array[4] = ": ";
				int num2 = 5;
				Exception ex2 = ex;
				array[num2] = ((ex2 != null) ? ex2.ToString() : null);
				Log.Error(string.Concat(array));
			}
			return thought_SituationalSocial;
		}

		// Token: 0x06008AAB RID: 35499 RVA: 0x00325138 File Offset: 0x00323338
		public void Notify_SituationalThoughtsDirty()
		{
			this.cachedThoughts.Clear();
			this.cachedSocialThoughts.Clear();
			this.thoughtsDirty = true;
		}

		// Token: 0x06008AAC RID: 35500 RVA: 0x00325157 File Offset: 0x00323357
		private void RemoveExpiredThoughtsFromCache()
		{
			this.cachedSocialThoughts.RemoveAll((KeyValuePair<Pawn, SituationalThoughtHandler.CachedSocialThoughts> x) => x.Value.Expired || x.Key.Discarded);
		}

		// Token: 0x04005563 RID: 21859
		public Pawn pawn;

		// Token: 0x04005564 RID: 21860
		private readonly List<Thought_Situational> cachedThoughts = new List<Thought_Situational>();

		// Token: 0x04005565 RID: 21861
		private readonly Dictionary<Pawn, SituationalThoughtHandler.CachedSocialThoughts> cachedSocialThoughts = new Dictionary<Pawn, SituationalThoughtHandler.CachedSocialThoughts>();

		// Token: 0x04005566 RID: 21862
		private bool thoughtsDirty = true;

		// Token: 0x04005567 RID: 21863
		private const int RecalculateStateEveryTicks = 100;

		// Token: 0x04005568 RID: 21864
		private readonly HashSet<ThoughtDef> tmpCachedSocialThoughts = new HashSet<ThoughtDef>();

		// Token: 0x020033E1 RID: 13281
		private class CachedSocialThoughts
		{
			// Token: 0x170031D5 RID: 12757
			// (get) Token: 0x06013202 RID: 78338 RVA: 0x006530D7 File Offset: 0x006512D7
			public bool Expired
			{
				get
				{
					return Find.TickManager.TicksGame - this.lastQueryTick >= 300;
				}
			}

			// Token: 0x170031D6 RID: 12758
			// (get) Token: 0x06013203 RID: 78339 RVA: 0x006530F4 File Offset: 0x006512F4
			public bool ShouldRecalculateState
			{
				get
				{
					return Find.TickManager.TicksGame - this.lastRecalculationTick >= 100;
				}
			}

			// Token: 0x0400D022 RID: 53282
			public readonly List<Thought_SituationalSocial> thoughts = new List<Thought_SituationalSocial>();

			// Token: 0x0400D023 RID: 53283
			public readonly List<Thought_SituationalSocial> activeThoughts = new List<Thought_SituationalSocial>();

			// Token: 0x0400D024 RID: 53284
			public int lastRecalculationTick = -99999;

			// Token: 0x0400D025 RID: 53285
			public int lastQueryTick = -99999;

			// Token: 0x0400D026 RID: 53286
			private const int ExpireAfterTicks = 300;
		}
	}
}
