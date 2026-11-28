using System;
using System.Collections.Generic;
using Verse.Grammar;

namespace Verse
{
	// Token: 0x020001F3 RID: 499
	public abstract class LogEntry_DamageResult : LogEntry
	{
		// Token: 0x06000E42 RID: 3650 RVA: 0x0004A111 File Offset: 0x00048311
		public LogEntry_DamageResult(LogEntryDef def = null) : base(def)
		{
		}

		// Token: 0x06000E43 RID: 3651 RVA: 0x0004A11A File Offset: 0x0004831A
		public void FillTargets(List<BodyPartRecord> recipientParts, List<bool> recipientPartsDestroyed, bool deflected)
		{
			this.damagedParts = recipientParts;
			this.damagedPartsDestroyed = recipientPartsDestroyed;
			this.deflected = deflected;
			base.ResetCache();
		}

		// Token: 0x06000E44 RID: 3652 RVA: 0x0004A137 File Offset: 0x00048337
		protected virtual BodyDef DamagedBody()
		{
			return null;
		}

		// Token: 0x06000E45 RID: 3653 RVA: 0x0004A13C File Offset: 0x0004833C
		protected override GrammarRequest GenerateGrammarRequest()
		{
			GrammarRequest result = base.GenerateGrammarRequest();
			result.Rules.AddRange(PlayLogEntryUtility.RulesForDamagedParts("recipient_part", this.DamagedBody(), this.damagedParts, this.damagedPartsDestroyed, result.Constants));
			result.Constants.Add("deflected", this.deflected.ToString());
			return result;
		}

		// Token: 0x06000E46 RID: 3654 RVA: 0x0004A19C File Offset: 0x0004839C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Collections.Look<BodyPartRecord>(ref this.damagedParts, "damagedParts", LookMode.BodyPart, Array.Empty<object>());
			Scribe_Collections.Look<bool>(ref this.damagedPartsDestroyed, "damagedPartsDestroyed", LookMode.Value, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.deflected, "deflected", false, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.damagedParts != null)
			{
				for (int i = this.damagedParts.Count - 1; i >= 0; i--)
				{
					if (this.damagedParts[i] == null)
					{
						this.damagedParts.RemoveAt(i);
						if (i < this.damagedPartsDestroyed.Count)
						{
							this.damagedPartsDestroyed.RemoveAt(i);
						}
					}
				}
			}
		}

		// Token: 0x04000E34 RID: 3636
		protected List<BodyPartRecord> damagedParts;

		// Token: 0x04000E35 RID: 3637
		protected List<bool> damagedPartsDestroyed;

		// Token: 0x04000E36 RID: 3638
		protected bool deflected;
	}
}
