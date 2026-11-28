using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x020001F2 RID: 498
	[StaticConstructorOnStartup]
	public abstract class LogEntry : IExposable, ILoadReferenceable
	{
		// Token: 0x170002EA RID: 746
		// (get) Token: 0x06000E2A RID: 3626 RVA: 0x00049E32 File Offset: 0x00048032
		public int Age
		{
			get
			{
				return Find.TickManager.TicksAbs - this.ticksAbs;
			}
		}

		// Token: 0x170002EB RID: 747
		// (get) Token: 0x06000E2B RID: 3627 RVA: 0x00049E45 File Offset: 0x00048045
		public int Tick
		{
			get
			{
				return this.ticksAbs;
			}
		}

		// Token: 0x170002EC RID: 748
		// (get) Token: 0x06000E2C RID: 3628 RVA: 0x00049E4D File Offset: 0x0004804D
		public int LogID
		{
			get
			{
				return this.logID;
			}
		}

		// Token: 0x170002ED RID: 749
		// (get) Token: 0x06000E2D RID: 3629 RVA: 0x00049E55 File Offset: 0x00048055
		public int Timestamp
		{
			get
			{
				return this.ticksAbs;
			}
		}

		// Token: 0x06000E2E RID: 3630 RVA: 0x00049E5D File Offset: 0x0004805D
		public LogEntry(LogEntryDef def = null)
		{
			this.ticksAbs = Find.TickManager.TicksAbs;
			this.def = def;
			if (Scribe.mode == LoadSaveMode.Inactive)
			{
				this.logID = Find.UniqueIDsManager.GetNextLogID();
			}
		}

		// Token: 0x06000E2F RID: 3631 RVA: 0x00049E9A File Offset: 0x0004809A
		public virtual void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ticksAbs, "ticksAbs", 0, false);
			Scribe_Values.Look<int>(ref this.logID, "logID", 0, false);
			Scribe_Defs.Look<LogEntryDef>(ref this.def, "def");
		}

		// Token: 0x06000E30 RID: 3632 RVA: 0x00049ED0 File Offset: 0x000480D0
		public string ToGameStringFromPOV(Thing pov, bool forceLog = false)
		{
			if (this.cachedString == null || pov == null != (this.cachedStringPov == null) || (this.cachedStringPov != null && pov != this.cachedStringPov.Target) || DebugViewSettings.logGrammarResolution || forceLog)
			{
				Rand.PushState();
				try
				{
					Rand.Seed = this.logID;
					this.cachedStringPov = ((pov != null) ? new WeakReference<Thing>(pov) : null);
					this.cachedString = this.ToGameStringFromPOV_Worker(pov, forceLog);
					this.cachedHeightWidth = 0f;
					this.cachedHeight = 0f;
				}
				finally
				{
					Rand.PopState();
				}
			}
			return this.cachedString;
		}

		// Token: 0x06000E31 RID: 3633 RVA: 0x00049F7C File Offset: 0x0004817C
		protected virtual string ToGameStringFromPOV_Worker(Thing pov, bool forceLog)
		{
			return GrammarResolver.Resolve("r_logentry", this.GenerateGrammarRequest(), null, forceLog, null, null, null, true);
		}

		// Token: 0x06000E32 RID: 3634 RVA: 0x00049F94 File Offset: 0x00048194
		protected virtual GrammarRequest GenerateGrammarRequest()
		{
			return default(GrammarRequest);
		}

		// Token: 0x06000E33 RID: 3635 RVA: 0x00049FAC File Offset: 0x000481AC
		public float GetTextHeight(Thing pov, float width)
		{
			string text = this.ToGameStringFromPOV(pov, false);
			if (this.cachedHeightWidth != width)
			{
				this.cachedHeightWidth = width;
				this.cachedHeight = Text.CalcHeight(text, width);
			}
			return this.cachedHeight;
		}

		// Token: 0x06000E34 RID: 3636 RVA: 0x00049FE5 File Offset: 0x000481E5
		protected void ResetCache()
		{
			this.cachedStringPov = null;
			this.cachedString = null;
			this.cachedHeightWidth = 0f;
			this.cachedHeight = 0f;
		}

		// Token: 0x06000E35 RID: 3637
		public abstract bool Concerns(Thing t);

		// Token: 0x06000E36 RID: 3638
		public abstract IEnumerable<Thing> GetConcerns();

		// Token: 0x06000E37 RID: 3639 RVA: 0x0004A00B File Offset: 0x0004820B
		public virtual bool CanBeClickedFromPOV(Thing pov)
		{
			return false;
		}

		// Token: 0x06000E38 RID: 3640 RVA: 0x0004A00E File Offset: 0x0004820E
		public virtual void ClickedFromPOV(Thing pov)
		{
		}

		// Token: 0x06000E39 RID: 3641 RVA: 0x0004A010 File Offset: 0x00048210
		public virtual Texture2D IconFromPOV(Thing pov)
		{
			return null;
		}

		// Token: 0x06000E3A RID: 3642 RVA: 0x0004A014 File Offset: 0x00048214
		public virtual Color? IconColorFromPOV(Thing pov)
		{
			return null;
		}

		// Token: 0x06000E3B RID: 3643 RVA: 0x0004A02A File Offset: 0x0004822A
		public virtual void Notify_FactionRemoved(Faction faction)
		{
		}

		// Token: 0x06000E3C RID: 3644 RVA: 0x0004A02C File Offset: 0x0004822C
		public virtual void Notify_IdeoRemoved(Ideo ideo)
		{
		}

		// Token: 0x06000E3D RID: 3645 RVA: 0x0004A030 File Offset: 0x00048230
		public virtual string GetTipString()
		{
			return "OccurredTimeAgo".Translate(this.Age.ToStringTicksToPeriod(true, false, true, true, false)).CapitalizeFirst() + ".";
		}

		// Token: 0x06000E3E RID: 3646 RVA: 0x0004A073 File Offset: 0x00048273
		public virtual bool ShowInCompactView()
		{
			return true;
		}

		// Token: 0x06000E3F RID: 3647 RVA: 0x0004A076 File Offset: 0x00048276
		public void Debug_OverrideTicks(int newTicks)
		{
			this.ticksAbs = newTicks;
		}

		// Token: 0x06000E40 RID: 3648 RVA: 0x0004A07F File Offset: 0x0004827F
		public string GetUniqueLoadID()
		{
			return string.Format("LogEntry_{0}_{1}", this.ticksAbs, this.logID);
		}

		// Token: 0x04000E27 RID: 3623
		protected int logID;

		// Token: 0x04000E28 RID: 3624
		protected int ticksAbs = -1;

		// Token: 0x04000E29 RID: 3625
		public LogEntryDef def;

		// Token: 0x04000E2A RID: 3626
		private WeakReference<Thing> cachedStringPov;

		// Token: 0x04000E2B RID: 3627
		private string cachedString;

		// Token: 0x04000E2C RID: 3628
		private float cachedHeightWidth;

		// Token: 0x04000E2D RID: 3629
		private float cachedHeight;

		// Token: 0x04000E2E RID: 3630
		public static readonly Texture2D Blood = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/Blood", true);

		// Token: 0x04000E2F RID: 3631
		public static readonly Texture2D BloodTarget = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/BloodTarget", true);

		// Token: 0x04000E30 RID: 3632
		public static readonly Texture2D Downed = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/Downed", true);

		// Token: 0x04000E31 RID: 3633
		public static readonly Texture2D DownedTarget = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/DownedTarget", true);

		// Token: 0x04000E32 RID: 3634
		public static readonly Texture2D Skull = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/Skull", true);

		// Token: 0x04000E33 RID: 3635
		public static readonly Texture2D SkullTarget = ContentFinder<Texture2D>.Get("Things/Mote/BattleSymbols/SkullTarget", true);
	}
}
